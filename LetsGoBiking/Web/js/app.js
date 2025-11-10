import './components/AddressAutocomplete.js';
import { initMap } from './map.js';
import { geocodeAddress, getItinerary } from './api.js';
import { setStatus, appendRouteSteps, addCustomStep, togglePanel, toggleSteps, setView } from './ui.js';

document.addEventListener('DOMContentLoaded', () => {
    let currentMap = null;
    let routeLayerGroup = null;

    const elements = {
        startAC: document.getElementById('start-autocomplete'),
        endAC: document.getElementById('end-autocomplete'),
        calculateBtn: document.getElementById('calculate-route-btn'),
        resetBtn: document.getElementById('reset-route-btn'),
        statusEl: document.getElementById('itinerary-status'),
        stepsEl: document.getElementById('steps'),
        mapEl: document.getElementById('map'),
        panel: document.getElementById('itinerary-panel'),
        panelToggleBtn: document.getElementById('panel-toggle-btn'),
        resultsContainer: document.getElementById('results-container'),
        viewSwitchBtn: document.getElementById('view-switch-btn')
    };

    function initialize() {
        currentMap = initMap(elements.mapEl);
        routeLayerGroup = L.layerGroup().addTo(currentMap);
        
        elements.panelToggleBtn.addEventListener('click', () => togglePanel(elements.panel, elements.panelToggleBtn, currentMap));
        elements.statusEl.addEventListener('click', () => toggleSteps(elements.resultsContainer));
        elements.calculateBtn.addEventListener('click', handleRouteCalculation);
        elements.resetBtn.addEventListener('click', resetRoute);   
        elements.viewSwitchBtn.addEventListener('click', toggleView);

        const initialStart = localStorage.getItem('itinerary_start');
        const initialEnd = localStorage.getItem('itinerary_end');
        
        if (initialStart) {
            elements.startAC.value = initialStart;
        }
        if (initialEnd) {
            elements.endAC.value = initialEnd;
        }

        if (initialStart && initialEnd) {
            buildRoute(initialStart, initialEnd);
            if (window.innerWidth < 768) {
                setView('map', currentMap, elements.viewSwitchBtn);
            }
        } else {
            elements.resultsContainer.classList.add('collapsed');
            if (window.innerWidth < 768) {
                setView('panel', currentMap, elements.viewSwitchBtn);
            }
        }
    }

    async function handleRouteCalculation() {
        const startText = elements.startAC.value;
        const endText = elements.endAC.value;

        if (!startText || !endText) {
            setStatus(elements.statusEl, 'Veuillez entrer un départ et une arrivée.', 'error');
            return;
        }

        localStorage.setItem('itinerary_start', startText);
        localStorage.setItem('itinerary_end', endText);

        await buildRoute(startText, endText);
        
        if (window.innerWidth < 768) {
            setView('map', currentMap, elements.viewSwitchBtn);
        }
    }

    async function buildRoute(startText, endText) {
        try {
            setStatus(elements.statusEl, 'Recherche de l\'itinéraire vélo…');
            elements.stepsEl.innerHTML = ''; 
            elements.resultsContainer.classList.remove('collapsed');
            routeLayerGroup.clearLayers(); 

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

            const allRoutes = [...walkRoutes, ...bikeRoutes].sort((a, b) => (a.position || 0) - (b.position || 0));
            
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
                const durationMin = Math.round(route.feature.properties.summary.duration / 60);
                const isFirst = index === 0;
                const isLast = index === allRoutes.length - 1;
                
                if (isWalking) {
                    let destinationText = 'la destination';
                    if (isFirst && bikeRouteInfo) {
                        destinationText = bikeRouteInfo.addressStart || 'la station de prise';
                    } else if (isLast) {
                        destinationText = endGeocoded.label;
                    }
                    
                    const icon = isFirst ? '🚶' : '🏁';
                    const subtext = isFirst && bikeRouteInfo 
                        ? `~${durationMin} min | <b>${bikeRouteInfo.availableBikes || 'N/A'} vélos dispo</b>`
                        : `~${durationMin} min`;
                    
                    addCustomStep(
                        elements.stepsEl,
                        `Marchez vers ${destinationText}`,
                        subtext,
                        icon
                    );
                } else if (isBike) {
                    const subtext = `~${durationMin} min | <b>${route.availableDropPlace || 'N/A'} places dispo</b>`;
                    addCustomStep(
                        elements.stepsEl,
                        `Roulez vers ${route.addressEnd || 'la station de rendu'}`,
                        subtext,
                        '🚲'
                    );
                }
                
                appendRouteSteps(elements.stepsEl, route.feature.properties.segments);
            });

            const totalKm = (totalDistance / 1000).toFixed(1);
            const totalMin = Math.round(totalDuration / 60);
            setStatus(elements.statusEl, `Total: ${totalKm} km • ~${totalMin} min (🚶+🚲)`);

        } catch (err) {
            console.error('Erreur lors du calcul d\'itinéraire:', err);
            setStatus(elements.statusEl, 'Erreur: ' + (err.message || 'échec du chargement'), 'error');
        }
    }
      
    function resetRoute() {
        elements.startAC.value = '';
        elements.endAC.value = '';
        
        routeLayerGroup.clearLayers();
        
        setStatus(elements.statusEl, '');
        elements.stepsEl.innerHTML = '';
        elements.resultsContainer.classList.add('collapsed');
        
        localStorage.removeItem('itinerary_start');
        localStorage.removeItem('itinerary_end');

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