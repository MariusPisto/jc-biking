import './components/AddressAutocomplete.js';
import { initMap } from './map.js';
import { geocodeAddress, getItinerary } from './api.js';
import { getRouteSegments } from './api/openrouteservice.js';
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
            const { start, pickup, dropoff, end } = itineraryData;

            setStatus(elements.statusEl, 'Calcul des segments d\'itinéraire…');
            const { routeWalk1, routeBike, routeWalk2 } = await getRouteSegments(start, pickup, dropoff, end);

            const routeLines = L.featureGroup();

            const coordsW1 = routeWalk1.geometry.coordinates.map(([lon, lat]) => [lat, lon]);
            L.polyline(coordsW1, { 
                color: '#0056b3', 
                weight: 5, 
                opacity: 0.8,
                dashArray: '5, 8' 
            }).addTo(routeLines);

            const coordsBike = routeBike.geometry.coordinates.map(([lon, lat]) => [lat, lon]);
            L.polyline(coordsBike, { 
                color: 'var(--primary-color)', 
                weight: 7, 
                opacity: 0.9 
            }).addTo(routeLines);
              
            const coordsW2 = routeWalk2.geometry.coordinates.map(([lon, lat]) => [lat, lon]);
            L.polyline(coordsW2, { 
                color: '#0056b3', 
                weight: 5, 
                opacity: 0.8,
                dashArray: '5, 8'
            }).addTo(routeLines);

            L.marker([start.latitude, start.longitude])
                .addTo(routeLines)
                .bindPopup(`<b>Départ</b><br>${startGeocoded.label}`);

            L.marker([pickup.latitude, pickup.longitude])
                .addTo(routeLines)
                .bindPopup(`<b>🚲 Station de prise</b><br>${pickup.address}<br><b>Vélos dispo: ${pickup.availableBikes}</b>`);

            L.marker([dropoff.latitude, dropoff.longitude])
                .addTo(routeLines)
                .bindPopup(`<b>🅿️ Station de rendu</b><br>${dropoff.address || 'Adresse inconnue'}<br><b>Places dispo: ${dropoff.availableDropPlace}</b>`);

            L.marker([end.latitude, end.longitude])
                .addTo(routeLines)
                .bindPopup(`<b>Arrivée</b><br>${endGeocoded.label}`);

            routeLines.addTo(routeLayerGroup);
            currentMap.fitBounds(routeLines.getBounds().pad(0.15));

            elements.stepsEl.innerHTML = ''; 
              
            const pickupSub = `~${Math.round(routeWalk1.properties.summary.duration / 60)} min | <b>${pickup.availableBikes} vélos dispo</b>`;
            addCustomStep(
                elements.stepsEl,
                `Marchez vers ${pickup.address || 'la station'}`,
                pickupSub,
                '🚶'
            );
            appendRouteSteps(elements.stepsEl, routeWalk1.properties.segments);

            const dropoffSub = `~${Math.round(routeBike.properties.summary.duration / 60)} min | <b>${dropoff.availableDropPlace} places dispo</b>`;
            addCustomStep(
                elements.stepsEl,
                `Roulez vers ${dropoff.address || 'la station'}`,
                dropoffSub,
                '🚲'
            );
            appendRouteSteps(elements.stepsEl, routeBike.properties.segments);

            addCustomStep(
                elements.stepsEl,
                `Marchez vers ${endGeocoded.label}`,
                `~${Math.round(routeWalk2.properties.summary.duration / 60)} min`,
                '🏁'
            );
            appendRouteSteps(elements.stepsEl, routeWalk2.properties.segments);

            const totalKm = ((routeWalk1.properties.summary.distance + routeBike.properties.summary.distance + routeWalk2.properties.summary.distance) / 1000).toFixed(1);
            const totalMin = Math.round((routeWalk1.properties.summary.duration + routeBike.properties.summary.duration + routeWalk2.properties.summary.duration) / 60);
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
});