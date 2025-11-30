import './components/AddressAutocomplete.js';
import { initMap } from './map.js';
import { geocodeAddress, getItinerary } from './api.js';
import { setStatus, appendRouteSteps, createRouteSegment, togglePanel, toggleSteps, setView } from './ui.js';

document.addEventListener('DOMContentLoaded', () => {
    let currentMap = null;
    let routeLayerGroup = null;
    let userLocationLayer = null;
    let userLocationMarker = null;
    let userLocationAccuracyCircle = null;
    let locateButtonEl = null;
    let mapSelectionMode = null;
    let pinLayer = null;
    let startPinMarker = null;
    let endPinMarker = null;
    let addressUpdateTimeouts = { start: null, end: null };
    const DEMO_MODE_KEY = 'itinerary_demo_mode';

    const elements = {
        startAC: document.getElementById('start-autocomplete'),
        endAC: document.getElementById('end-autocomplete'),
        swapAddressesBtn: document.getElementById('swap-addresses-btn'),
        geoStartBtn: document.getElementById('geo-start-btn'),
        geoEndBtn: document.getElementById('geo-end-btn'),
        mapSelectStartBtn: document.getElementById('map-select-start-btn'),
        mapSelectEndBtn: document.getElementById('map-select-end-btn'),
        calculateBtn: document.getElementById('calculate-route-btn'),
        demoBtn: document.getElementById('demo-btn'),
        resetBtn: document.getElementById('reset-route-btn'),
        statusEl: document.getElementById('itinerary-status'),
        stepsEl: document.getElementById('steps'),
        mapEl: document.getElementById('map'),
        panel: document.getElementById('itinerary-panel'),
        panelToggleBtn: document.getElementById('panel-toggle-btn'),
        resultsContainer: document.getElementById('results-container'),
        viewSwitchBtn: document.getElementById('view-switch-btn'),
        loader: document.getElementById('loader')
    };

    function initialize() {
        currentMap = initMap(elements.mapEl);
        routeLayerGroup = L.layerGroup().addTo(currentMap);
        userLocationLayer = L.layerGroup().addTo(currentMap);
        pinLayer = L.layerGroup().addTo(currentMap);
        setupLocateControl();

        elements.panelToggleBtn.addEventListener('click', () => togglePanel(elements.panel, elements.panelToggleBtn, currentMap));
        elements.statusEl.addEventListener('click', () => toggleSteps(elements.resultsContainer));
        elements.calculateBtn.addEventListener('click', () => {
            localStorage.removeItem(DEMO_MODE_KEY);
            handleRouteCalculation();
        });
        elements.demoBtn.addEventListener('click', () => {
            elements.startAC.value = 'Bruxelles';
            elements.endAC.value = 'Luxembourg';
            localStorage.setItem(DEMO_MODE_KEY, 'true');
            handleRouteCalculation();
        });
        elements.viewSwitchBtn.addEventListener('click', toggleView);
        elements.swapAddressesBtn.addEventListener('click', swapAddresses);
        elements.resetBtn.addEventListener('click', resetRoute);

        ['input', 'address-selected', 'address-cleared'].forEach(evt => {
            elements.startAC.addEventListener(evt, (e) => {
                clearRouteResults();
                if (evt === 'address-selected') {
                    // Immediate update when address is selected from suggestions
                    handleAddressUpdate('start', elements.startAC.value);
                } else if (evt === 'input') {
                    // Debounced update when typing
                    if (addressUpdateTimeouts.start) {
                        clearTimeout(addressUpdateTimeouts.start);
                    }
                    addressUpdateTimeouts.start = setTimeout(() => {
                        handleAddressUpdate('start', elements.startAC.value);
                    }, 1000);
                } else if (evt === 'address-cleared') {
                    removePinMarker('start');
                }
            });
            elements.endAC.addEventListener(evt, (e) => {
                clearRouteResults();
                if (evt === 'address-selected') {
                    // Immediate update when address is selected from suggestions
                    handleAddressUpdate('end', elements.endAC.value);
                } else if (evt === 'input') {
                    // Debounced update when typing
                    if (addressUpdateTimeouts.end) {
                        clearTimeout(addressUpdateTimeouts.end);
                    }
                    addressUpdateTimeouts.end = setTimeout(() => {
                        handleAddressUpdate('end', elements.endAC.value);
                    }, 1000);
                } else if (evt === 'address-cleared') {
                    removePinMarker('end');
                }
            });
        });

        bindGeolocationButton(elements.geoStartBtn, 'start');
        bindGeolocationButton(elements.geoEndBtn, 'end');
        bindMapSelectButton(elements.mapSelectStartBtn, 'start');
        bindMapSelectButton(elements.mapSelectEndBtn, 'end');

        currentMap.on('click', (e) => {
            if (!mapSelectionMode) return;

            const { lat, lng } = e.latlng;
            // Format coordinates with 6 decimal places for precision
            const coordString = `${lat.toFixed(6)}, ${lng.toFixed(6)}`;

            if (mapSelectionMode === 'start') {
                elements.startAC.value = coordString;
                highlightAddressField(elements.startAC);
                updatePinMarker('start', lat, lng);
            } else if (mapSelectionMode === 'end') {
                elements.endAC.value = coordString;
                highlightAddressField(elements.endAC);
                updatePinMarker('end', lat, lng);
            }

            setMapSelectionMode(null);
            clearRouteResults();
        });

        elements.resultsContainer.classList.add('collapsed');
        if (window.innerWidth < 768) {
            setView('panel', currentMap, elements.viewSwitchBtn);
        }
    }

    function formatDuration(seconds) {
        const minutes = Math.round(seconds / 60);
        if (minutes < 60) {
            return `~${minutes}min`;
        }
        const hours = Math.floor(minutes / 60);
        const rem = minutes % 60;
        return rem === 0 ? `~${hours}h` : `~${hours}h ${rem}min`;
    }

    function swapAddresses() {
        this.classList.toggle('rotated');
        const startValue = elements.startAC.value;
        elements.startAC.value = elements.endAC.value;
        elements.endAC.value = startValue;
        clearRouteResults();
    }

    function bindGeolocationButton(button, targetType) {
        if (!button) return;
        button.addEventListener('click', () => requestUserLocation(targetType));
    }

    function bindMapSelectButton(button, targetType) {
        if (!button) return;
        button.addEventListener('click', () => {
            if (mapSelectionMode === targetType) {
                setMapSelectionMode(null);
            } else {
                setMapSelectionMode(targetType);
            }
        });
    }

    function setMapSelectionMode(mode) {
        mapSelectionMode = mode;

        if (elements.mapSelectStartBtn) {
            elements.mapSelectStartBtn.classList.toggle('active', mode === 'start');
        }
        if (elements.mapSelectEndBtn) {
            elements.mapSelectEndBtn.classList.toggle('active', mode === 'end');
        }

        if (elements.mapEl) {
            elements.mapEl.style.cursor = mode ? 'crosshair' : '';
        }

        if (mode && window.innerWidth < 768) {
            setView('map', currentMap, elements.viewSwitchBtn);
        }
    }

    function setupLocateControl() {
        const zoomContainer = currentMap.zoomControlContainer || elements.mapEl.querySelector('.leaflet-control-zoom');
        if (!zoomContainer) return;

        locateButtonEl = document.createElement('button');
        locateButtonEl.type = 'button';
        locateButtonEl.className = 'locate-btn';
        locateButtonEl.setAttribute('aria-label', 'Centrer sur ma position');
        locateButtonEl.title = 'Centrer sur ma position';
        locateButtonEl.innerHTML = `
            <svg width="18" height="18" viewBox="0 0 24 24" aria-hidden="true">
                <circle cx="12" cy="12" r="3.5" fill="currentColor"></circle>
                <path d="M12 5V3m0 18v-2M5 12H3m18 0h-2" stroke="currentColor" stroke-width="1.6" stroke-linecap="round"></path>
                <circle cx="12" cy="12" r="7" fill="none" stroke="currentColor" stroke-width="1.4" stroke-linecap="round"></circle>
            </svg>
        `;
        zoomContainer.appendChild(locateButtonEl);
        L.DomEvent.disableClickPropagation(locateButtonEl);
        locateButtonEl.addEventListener('click', (event) => {
            event.preventDefault();
            locateUserOnMap();
        });
    }

    function requestUserLocation(targetType) {
        if (!navigator.geolocation) {
            setStatus(elements.statusEl, 'Géolocalisation indisponible sur ce navigateur.', 'error');
            return;
        }

        const targetField = targetType === 'start' ? elements.startAC : elements.endAC;
        const button = targetType === 'start' ? elements.geoStartBtn : elements.geoEndBtn;
        if (!targetField || !button) return;

        setGeoButtonState(button, true);

        navigator.geolocation.getCurrentPosition(({ coords }) => {
            // Format coordinates with 6 decimal places for precision
            const coordString = `${coords.latitude.toFixed(6)}, ${coords.longitude.toFixed(6)}`;
            targetField.value = coordString;
            highlightAddressField(targetField);
            updatePinMarker(targetType, coords.latitude, coords.longitude);
            clearRouteResults();
            setGeoButtonState(button, false);
        }, (error) => {
            console.error('Geolocation error:', error);
            const message = error.code === error.PERMISSION_DENIED
                ? 'Autorisez la localisation pour utiliser cette fonctionnalité.'
                : 'Géolocalisation indisponible.';
            setStatus(elements.statusEl, message, 'error');
            setGeoButtonState(button, false);
        }, {
            enableHighAccuracy: true,
            timeout: 10000,
            maximumAge: 0
        });
    }

    function setGeoButtonState(button, loading) {
        button.disabled = loading;
        button.classList.toggle('is-loading', loading);
    }

    function locateUserOnMap() {
        if (!navigator.geolocation) {
            setStatus(elements.statusEl, 'Géolocalisation indisponible sur ce navigateur.', 'error');
            return;
        }

        setLocateButtonState(true);
        navigator.geolocation.getCurrentPosition(({ coords }) => {
            updateUserLocation(coords);
            setLocateButtonState(false);
        }, (error) => {
            const message = error.code === error.PERMISSION_DENIED
                ? 'Autorisez la localisation pour utiliser cette fonctionnalité.'
                : 'Impossible de récupérer votre position.';
            setStatus(elements.statusEl, message, 'error');
            setLocateButtonState(false);
        }, {
            enableHighAccuracy: true,
            timeout: 10000,
            maximumAge: 10000
        });
    }

    function updateUserLocation(coords) {
        if (!currentMap || !userLocationLayer) return;
        const latlng = [coords.latitude, coords.longitude];
        const accuracy = Math.max(coords.accuracy || 25, 20);

        if (!userLocationMarker) {
            userLocationMarker = L.circleMarker(latlng, {
                radius: 8,
                color: '#ffffff',
                weight: 3,
                fillColor: '#1a73e8',
                fillOpacity: 1,
                pane: 'markerPane'
            }).addTo(userLocationLayer);
        } else {
            userLocationMarker.setLatLng(latlng);
        }

        if (!userLocationAccuracyCircle) {
            userLocationAccuracyCircle = L.circle(latlng, {
                radius: accuracy,
                color: '#1a73e8',
                weight: 1,
                fillOpacity: 0.15,
                fillColor: '#1a73e8'
            }).addTo(userLocationLayer);
        } else {
            userLocationAccuracyCircle.setLatLng(latlng);
            userLocationAccuracyCircle.setRadius(accuracy);
        }

        const targetZoom = Math.max(currentMap.getZoom(), 15);
        currentMap.flyTo(latlng, targetZoom, { duration: 0.8 });
    }

    function setLocateButtonState(loading) {
        if (!locateButtonEl) return;
        locateButtonEl.disabled = loading;
        locateButtonEl.classList.toggle('is-loading', loading);
    }

    function highlightAddressField(addressComponent) {
        const input = addressComponent?.querySelector('input');
        if (!input) return;
        input.classList.add('geo-highlight');
        setTimeout(() => input.classList.remove('geo-highlight'), 1200);
    }

    async function handleRouteCalculation() {
        const isDemo = localStorage.getItem(DEMO_MODE_KEY) === 'true';
        const startText = elements.startAC.value;
        const endText = elements.endAC.value;

        if (!startText || !endText) {
            setStatus(elements.statusEl, 'Veuillez entrer un départ et une arrivée.', 'error');
            return;
        }



        await buildRoute(startText, endText);

        if (isDemo) {
            localStorage.removeItem(DEMO_MODE_KEY);
        }

        if (window.innerWidth < 768) {
            setView('map', currentMap, elements.viewSwitchBtn);
        }
    }

    async function buildRoute(startText, endText) {
        try {
            elements.loader.classList.remove('hidden');
            setStatus(elements.statusEl, 'Recherche de l\'itinéraire vélo…');
            elements.stepsEl.innerHTML = '';
            elements.resultsContainer.classList.remove('collapsed');
            routeLayerGroup.clearLayers();
            clearPins();

            const [startGeocoded, endGeocoded] = await Promise.all([
                geocodeAddress(startText),
                geocodeAddress(endText)
            ]);

            const itineraryData = await getItinerary(startGeocoded, endGeocoded);

            const walkRoutes = itineraryData.walkRoutes || [];
            const bikeRoutes = itineraryData.bikeRoutes || [];

            if (!Array.isArray(walkRoutes) || !Array.isArray(bikeRoutes)) {
                throw new Error('Format d\'itinéraire invalide');
            }

            const allRoutes = [];
            const maxSegments = Math.max(walkRoutes.length, bikeRoutes.length);
            for (let i = 0; i < maxSegments; i++) {
                if (walkRoutes[i]) {
                    allRoutes.push(walkRoutes[i]);
                }
                if (bikeRoutes[i]) {
                    allRoutes.push(bikeRoutes[i]);
                }
            }

            if (allRoutes.length === 0) {
                throw new Error('Aucun itinéraire trouvé');
            }

            const routeLines = L.featureGroup();
            let totalDistance = 0;
            let totalDuration = 0;
            let bikeRouteInfo = null;

            allRoutes.forEach((route, index) => {
                const isFirst = index === 0;
                const isLast = index === allRoutes.length - 1;
                const isBike = route.type === 'bike';

                const coords = route.feature.geometry.coordinates.map(([lon, lat]) => [lat, lon]);

                const polylineOptions = isBike ? {
                    color: 'var(--primary-color)',
                    weight: 7,
                    opacity: 0.9
                } : {
                    color: '#0056b3',
                    weight: 5,
                    opacity: 0.8,
                    dashArray: '5, 8'
                };

                L.polyline(coords, polylineOptions).addTo(routeLines);

                if (isFirst) {
                    L.marker([route.start.latitude, route.start.longitude])
                        .addTo(routeLines)
                        .bindPopup(`<b>Départ</b><br>${startGeocoded.label}`);
                }

                if (isLast) {
                    L.marker([route.end.latitude, route.end.longitude])
                        .addTo(routeLines)
                        .bindPopup(`<b>Arrivée</b><br>${endGeocoded.label}`);
                }

                if (isBike) {
                    bikeRouteInfo = route;

                    L.marker([route.start.latitude, route.start.longitude])
                        .addTo(routeLines)
                        .bindPopup(`<b>🚲 Station de prise</b><br>${route.addressStart || 'Adresse inconnue'}<br><b>Vélos dispo: ${route.availableBikes || 'N/A'}</b>`);

                    L.marker([route.end.latitude, route.end.longitude])
                        .addTo(routeLines)
                        .bindPopup(`<b>🅿️ Station de rendu</b><br>${route.addressEnd || 'Adresse inconnue'}<br><b>Places dispo: ${route.availableDropPlace || 'N/A'}</b>`);
                }

                totalDistance += route.feature.properties.summary.distance;
                totalDuration += route.feature.properties.summary.duration;
            });

            routeLines.addTo(routeLayerGroup);
            currentMap.fitBounds(routeLines.getBounds().pad(0.15));

            elements.stepsEl.innerHTML = '';

            allRoutes.forEach((route, index) => {
                const isBike = route.type === 'bike';
                const isWalking = route.type === 'simple';
                const durationStr = formatDuration(route.feature.properties.summary.duration);
                const isFirst = index === 0;
                const isLast = index === allRoutes.length - 1;

                let segmentStepsContainer = elements.stepsEl;

                if (isWalking) {
                    let destinationText = 'la destination';
                    if (bikeRouteInfo) {
                        destinationText = bikeRouteInfo.addressStart || 'la station de prise';
                    } else if (isLast) {
                        destinationText = endGeocoded.label;
                    }

                    const icon = isLast ? '🏁' : '🚶';
                    const subtext = isFirst && bikeRouteInfo
                        ? `${durationStr} | <b>${bikeRouteInfo.availableBikes || 'N/A'} vélos dispo</b>`
                        : `${durationStr}`;

                    segmentStepsContainer = createRouteSegment(
                        elements.stepsEl,
                        `Marchez vers ${destinationText}`,
                        subtext,
                        icon
                    );
                } else if (isBike) {
                    const subtext = `${durationStr} | <b>${route.availableDropPlace || 'N/A'} places dispo</b>`;
                    segmentStepsContainer = createRouteSegment(
                        elements.stepsEl,
                        `Roulez vers ${route.addressEnd || 'la station de rendu'}`,
                        subtext,
                        '🚲'
                    );
                }

                appendRouteSteps(segmentStepsContainer, route.feature.properties.segments);
            });

            const totalKm = (totalDistance / 1000).toFixed(1);
            const modeIcons = bikeRoutes.length > 0 ? '🚶+🚲' : '🚶';
            setStatus(elements.statusEl, `Total: ${totalKm} km • ${formatDuration(totalDuration)} (${modeIcons})`);

        } catch (err) {
            console.error('Erreur lors du calcul d\'itinéraire:', err);
            setStatus(elements.statusEl, 'Erreur: ' + (err.message || 'échec du chargement'), 'error');
        } finally {
            elements.loader.classList.add('hidden');
        }
    }

    function updatePinMarker(type, lat, lng) {
        if (!pinLayer) return;

        const isStart = type === 'start';
        let marker = isStart ? startPinMarker : endPinMarker;

        // Pin colors: green for start, red for end
        const color = isStart ? '#28a745' : '#dc3545';
        const label = isStart ? 'Départ' : 'Arrivée';

        if (marker) {
            marker.setLatLng([lat, lng]);
        } else {
            marker = L.marker([lat, lng], {
                icon: L.divIcon({
                    className: 'location-pin',
                    html: `
                        <div style="
                            width: 24px;
                            height: 24px;
                            background-color: ${color};
                            border: 3px solid white;
                            border-radius: 50%;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.3);
                            position: relative;
                        ">
                            <div style="
                                position: absolute;
                                bottom: -8px;
                                left: 50%;
                                transform: translateX(-50%);
                                width: 0;
                                height: 0;
                                border-left: 6px solid transparent;
                                border-right: 6px solid transparent;
                                border-top: 8px solid ${color};
                            "></div>
                        </div>
                    `,
                    iconSize: [24, 32],
                    iconAnchor: [12, 32],
                    popupAnchor: [0, -32]
                })
            }).addTo(pinLayer);

            marker.bindPopup(label);

            if (isStart) {
                startPinMarker = marker;
            } else {
                endPinMarker = marker;
            }
        }
    }

    async function handleAddressUpdate(type, value) {
        if (!value || !value.trim()) {
            removePinMarker(type);
            return;
        }

        const trimmedValue = value.trim();
        
        // Check if input is coordinates
        const coordPattern = /^\s*(-?\d+\.?\d*)\s*,\s*(-?\d+\.?\d*)\s*$/;
        const match = trimmedValue.match(coordPattern);
        
        if (match) {
            const lat = parseFloat(match[1]);
            const lon = parseFloat(match[2]);
            
            // Validate coordinate ranges
            if (!isNaN(lat) && !isNaN(lon) && lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180) {
                updatePinMarker(type, lat, lon);
                return;
            }
        }

        // Otherwise, try to geocode the address
        try {
            const geocoded = await geocodeAddress(trimmedValue);
            if (geocoded && geocoded.lat && geocoded.lon) {
                updatePinMarker(type, geocoded.lat, geocoded.lon);
            }
        } catch (error) {
            // If geocoding fails, don't show a pin
            console.log('Could not geocode address for pin:', error);
        }
    }

    function removePinMarker(type) {
        const isStart = type === 'start';
        const marker = isStart ? startPinMarker : endPinMarker;
        
        if (marker && pinLayer) {
            pinLayer.removeLayer(marker);
        }
        
        if (isStart) {
            startPinMarker = null;
        } else {
            endPinMarker = null;
        }
    }

    function clearPins() {
        if (pinLayer) {
            pinLayer.clearLayers();
        }
        startPinMarker = null;
        endPinMarker = null;
    }

    function clearRouteResults() {
        routeLayerGroup.clearLayers();
        setStatus(elements.statusEl, '');
        elements.stepsEl.innerHTML = '';
        elements.resultsContainer.classList.add('collapsed');
        localStorage.removeItem(DEMO_MODE_KEY);
        
        // Re-show pins based on current address field values
        if (elements.startAC && elements.startAC.value) {
            handleAddressUpdate('start', elements.startAC.value);
        }
        if (elements.endAC && elements.endAC.value) {
            handleAddressUpdate('end', elements.endAC.value);
        }
    }

    function resetRoute() {
        elements.startAC.value = '';
        elements.endAC.value = '';
        clearPins();
        clearRouteResults();

        if (window.innerWidth < 768) {
            setView('panel', currentMap, elements.viewSwitchBtn);
        }
    }

    function toggleView() {
        const isMapView = document.body.classList.contains('view-map');
        setView(isMapView ? 'panel' : 'map', currentMap, elements.viewSwitchBtn);
    }

    initialize();

    const notificationHeader = document.querySelector('.notification-section h4');
    if (notificationHeader) {
        notificationHeader.addEventListener('click', () => {
            const notificationSection = notificationHeader.closest('.notification-section');
            if (notificationSection) {
                notificationSection.classList.toggle('closed');
            }
        });
    }
});
