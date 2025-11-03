class AddressAutocomplete extends HTMLElement {
    constructor() {
        super();
          
        this.wrapper = document.createElement('div');
        this.wrapper.className = 'ac-wrapper';
          
        this.input = document.createElement('input');
        this.input.type = 'text';
        this.input.autocomplete = 'off';
        this.input.className = 'ac-input';
        
        this.suggestions = document.createElement('ul');
        this.suggestions.className = 'ac-suggestions';
        this.suggestions.hidden = true;
        
        this.wrapper.appendChild(this.input);
        this.wrapper.appendChild(this.suggestions);
        this.appendChild(this.wrapper);
        
        this.debounceTimeout = null;
    }

    connectedCallback() {
        if (this.hasAttribute('placeholder')) {
            this.input.placeholder = this.getAttribute('placeholder');
        }
        this.input.addEventListener('input', (e) => this.onInput(e));
        this.input.addEventListener('blur', () => setTimeout(() => this.hideSuggestions(), 200));
        this.suggestions.addEventListener('mousedown', (e) => {
            const li = e.target.closest('li');
            if (li && li.dataset.fulladdress) {
                this.selectSuggestion(li.dataset.fulladdress);
            }
        });
    }

    onInput(e) {
        const value = e.target.value.trim();
        if (this.debounceTimeout) clearTimeout(this.debounceTimeout);
        if (!value) {
            this.hideSuggestions();
            return;
        }
        this.debounceTimeout = setTimeout(() => this.fetchSuggestions(value), 300);
    }

    fetchSuggestions(query) {
        this.setLoading(true);
        fetch(`https://api-adresse.data.gouv.fr/search/?q=${encodeURIComponent(query)}&limit=5`)
            .then(r => r.json())
            .then(data => {
                this.showSuggestions(data.features.map(f => f.properties.label));
            })
            .catch(() => this.hideSuggestions())
            .finally(() => this.setLoading(false));
    }

    showSuggestions(addresses) {
        this.suggestions.innerHTML = '';
        if (!addresses.length) {
            this.hideSuggestions();
            return;
        }
        addresses.forEach(addr => {
            const li = document.createElement('li');
            li.textContent = addr;
            li.dataset.fulladdress = addr;
            li.className = 'ac-suggestion';
            this.suggestions.appendChild(li);
        });
        this.suggestions.hidden = false;
    }

    hideSuggestions() {
        this.suggestions.hidden = true;
    }

    selectSuggestion(address) {
        this.input.value = address;
        this.hideSuggestions();
        this.dispatchEvent(new CustomEvent('address-selected', { detail: address, bubbles: true }));
    }

    setLoading(loading) {
        if (loading) {
            this.suggestions.innerHTML = '';
            const li = document.createElement('li');
            li.textContent = 'Recherche…';
            li.className = 'ac-loading';
            this.suggestions.appendChild(li);
            this.suggestions.hidden = false;
        }
    }

    get value() { return this.input.value; }
    set value(val) { this.input.value = val; }
}
customElements.define('address-autocomplete', AddressAutocomplete);

document.addEventListener('DOMContentLoaded', () => {

    const ORS_API_KEY = 'eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjU4MTIzY2Q5NGM4YzRlYmNhNGExZTJjZGEwNmVkYzBkIiwiaCI6Im11cm11cjY0In0=';

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

    function initMap() {
        currentMap = L.map(elements.mapEl, {
            zoomControl: false
        }).setView([43.6152, 7.0702], 12); 

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors'
        }).addTo(currentMap);
        
        L.control.zoom({ position: 'bottomright' }).addTo(currentMap);
        
        routeLayerGroup = L.layerGroup().addTo(currentMap);
    }

    function setStatus(text, type = 'info') {
        elements.statusEl.innerHTML = text || ''; 
        elements.statusEl.style.color = type === 'error' ? '#d93025' : 'var(--text-dark)';
    }

    async function geocodeAddress(address) {
        const url = `https://api-adresse.data.gouv.fr/search/?q=${encodeURIComponent(address)}&limit=1`;
        const res = await fetch(url);
        if (!res.ok) throw new Error('Échec du géocodage');
        const data = await res.json();
        if (!data.features || !data.features.length) throw new Error(`Adresse introuvable: ${address}`);
        const { coordinates } = data.features[0].geometry;
        const { label } = data.features[0].properties;
        return { lat: coordinates[1], lon: coordinates[0], label };
    }

    async function handleRouteCalculation() {
        const startText = elements.startAC.value;
        const endText = elements.endAC.value;

        if (!startText || !endText) {
            setStatus('Veuillez entrer un départ et une arrivée.', 'error');
            return;
        }

        localStorage.setItem('itinerary_start', startText);
        localStorage.setItem('itinerary_end', endText);

        await buildRoute(startText, endText);
        
        if (window.innerWidth < 768) {
            setView('map');
        }
    }

    function appendRouteSteps(segments) {
        const orsIcons = {
            0: '↰', 1: '↱', 2: '↰', 3: '↱', 4: '↰', 5: '↱',
            6: '↑', 7: '⟲', 8: '⟲', 9: '↩', 10: '★', 11: '●'
        };

        segments.forEach(segment => {
            segment.steps.forEach(step => {
                const rawName = (step.name || '').trim();
                const name = (rawName && rawName !== '-' && rawName !== '–' && rawName !== '—') ? rawName : '';
                const dist = Math.max(1, Math.round(step.distance));
                const instr = step.instruction || 'Continuer';
                const type = step.type;
                
                let icon = orsIcons[type] || '→'; 

                const li = document.createElement('li');
                li.className = 'step-item';
                li.innerHTML = `
                    <div class="step-marker" style="font-size: 1.5rem; color: var(--text-light);">${icon}</div>
                    <div class="step-content">
                        <div class="step-title" style="font-weight: 500;">${instr}</div>
                        ${name ? `<div class="step-sub">sur ${name}</div>` : ''}
                    </div>
                    <div class="step-badge">${dist} m</div>
                `;
                elements.stepsEl.appendChild(li);
            });
        });
    }

    function addCustomStep(title, subtext = '', icon = '📍') {
        const li = document.createElement('li');
        li.className = 'step-item';
        
        li.style.background = 'var(--bg-light)';
        li.style.borderRadius = '8px';
        li.style.paddingTop = '1rem';
        li.style.paddingBottom = '1rem';
        li.style.borderBottom = 'none';

        li.innerHTML = `
            <div class="step-marker" style="color: var(--text-dark);">${icon}</div>
            <div class="step-content">
                <div class="step-title">${title}</div>
                ${subtext ? `<div class="step-sub" style="color: var(--primary-color); font-weight: 500;">${subtext}</div>` : ''}
            </div>
        `;
        elements.stepsEl.appendChild(li);
    }

    async function buildRoute(startText, endText) {
        try {
            setStatus('Recherche de l\'itinéraire vélo…');
            elements.stepsEl.innerHTML = ''; 
            elements.resultsContainer.classList.remove('collapsed');
            routeLayerGroup.clearLayers(); 

            
            const [startGeocoded, endGeocoded] = await Promise.all([
                geocodeAddress(startText),
                geocodeAddress(endText)
            ]);

            
            const backendUrl = `http://localhost:8733/itinerary?originLat=${startGeocoded.lat}&originLng=${startGeocoded.lon}&destLat=${endGeocoded.lat}&destLng=${endGeocoded.lon}`;
            let itineraryData;
            try {
                const backendRes = await fetch(backendUrl);
                if (!backendRes.ok) throw new Error(`Erreur ${backendRes.status}`);
                itineraryData = await backendRes.json();
            } catch (backendErr) {
                throw new Error(`Service de vélos indisponible (vérifiez que le service C# est lancé sur localhost:8733)`);
            }
              
            const { start, pickup, dropoff, end } = itineraryData;

            
            setStatus('Calcul des segments d\'itinéraire…');
            const walk1Url = `https://api.openrouteservice.org/v2/directions/foot-walking?api_key=${ORS_API_KEY}&start=${start.longitude},${start.latitude}&end=${pickup.longitude},${pickup.latitude}`;
            const bikeUrl = `https://api.openrouteservice.org/v2/directions/cycling-regular?api_key=${ORS_API_KEY}&start=${pickup.longitude},${pickup.latitude}&end=${dropoff.longitude},${dropoff.latitude}`;
            const walk2Url = `https://api.openrouteservice.org/v2/directions/foot-walking?api_key=${ORS_API_KEY}&start=${dropoff.longitude},${dropoff.latitude}&end=${end.longitude},${end.latitude}`;
              
            const [walk1Res, bikeRes, walk2Res] = await Promise.all([
                fetch(walk1Url), fetch(bikeUrl), fetch(walk2Url)
            ]);

            if (!walk1Res.ok || !bikeRes.ok || !walk2Res.ok) {
                throw new Error('Service d\'itinéraire (OpenRouteService) indisponible');
            }
              
            const [walk1Data, bikeData, walk2Data] = await Promise.all([
                walk1Res.json(), bikeRes.json(), walk2Res.json()
            ]);
              
            if (!walk1Data.features?.[0] || !bikeData.features?.[0] || !walk2Data.features?.[0]) {
                 throw new Error('Impossible de calculer un segment de l\'itinéraire');
            }
              
            const routeWalk1 = walk1Data.features[0];
            const routeBike = bikeData.features[0];
            const routeWalk2 = walk2Data.features[0];


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
                `Marchez vers ${pickup.address || 'la station'}`,
                pickupSub,
                '🚶'
            );
            appendRouteSteps(routeWalk1.properties.segments);

            const dropoffSub = `~${Math.round(routeBike.properties.summary.duration / 60)} min | <b>${dropoff.availableDropPlace} places dispo</b>`;
            addCustomStep(
                `Roulez vers ${dropoff.address || 'la station'}`,
                dropoffSub,
                '🚲'
            );
            appendRouteSteps(routeBike.properties.segments);

            addCustomStep(
                `Marchez vers ${endGeocoded.label}`,
                `~${Math.round(routeWalk2.properties.summary.duration / 60)} min`,
                '🏁'
            );
            appendRouteSteps(routeWalk2.properties.segments);

            
            const totalKm = ((routeWalk1.properties.summary.distance + routeBike.properties.summary.distance + routeWalk2.properties.summary.distance) / 1000).toFixed(1);
            const totalMin = Math.round((routeWalk1.properties.summary.duration + routeBike.properties.summary.duration + routeWalk2.properties.summary.duration) / 60);
            setStatus(`Total: ${totalKm} km • ~${totalMin} min (🚶+🚲)`);

        } catch (err) {
            console.error('Erreur lors du calcul d\'itinéraire:', err);
            setStatus('Erreur: ' + (err.message || 'échec du chargement'), 'error');
        }
    }
      
    function togglePanel() {
        elements.panel.classList.toggle('collapsed');
        elements.panelToggleBtn.classList.toggle('collapsed');
          
        const isCollapsed = elements.panel.classList.contains('collapsed');
        elements.panelToggleBtn.setAttribute('aria-label', 
            isCollapsed ? 'Afficher le panneau' : 'Masquer le panneau'
        );
          
        setTimeout(() => {
            currentMap.invalidateSize();
        }, 400);
    }

    function toggleSteps() {
        elements.resultsContainer.classList.toggle('collapsed');
    }
      
    function setView(view) {
        if (view === 'map') {
            document.body.classList.add('view-map');
            elements.viewSwitchBtn.textContent = 'Voir le panneau';
            setTimeout(() => {
                currentMap.invalidateSize();
            }, 400);
        } else {
            document.body.classList.remove('view-map');
            elements.viewSwitchBtn.textContent = 'Voir la carte';
        }
    }

function resetRoute() {
        
        elements.startAC.value = '';
        elements.endAC.value = '';
        
        
        routeLayerGroup.clearLayers();
        
        
        setStatus('');
        elements.stepsEl.innerHTML = '';
        elements.resultsContainer.classList.add('collapsed');
        
        
        localStorage.removeItem('itinerary_start');
        localStorage.removeItem('itinerary_end');

        
        if (window.innerWidth < 768) {
            setView('panel');
        }
    }
      
    function toggleView() {
        const isMapView = document.body.classList.contains('view-map');
        setView(isMapView ? 'panel' : 'map');
    }
      
    initMap();

    elements.panelToggleBtn.addEventListener('click', togglePanel);
    elements.statusEl.addEventListener('click', toggleSteps);
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
            setView('map');
        }
    } else {
        elements.resultsContainer.classList.add('collapsed');
        if (window.innerWidth < 768) {
            setView('panel');
        }
    }
});