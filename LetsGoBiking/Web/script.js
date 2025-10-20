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
        this.debounceTimeout = setTimeout(() => this.fetchSuggestions(value), 750);
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

    let currentMap = null;
    let routeLayerGroup = null;

    const elements = {
        startAC: document.getElementById('start-autocomplete'),
        endAC: document.getElementById('end-autocomplete'),
        calculateBtn: document.getElementById('calculate-route-btn'),
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
        elements.statusEl.textContent = text || '';
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

    async function buildRoute(startText, endText) {
        try {
            setStatus('Chargement de l\'itinéraire…');
            elements.stepsEl.innerHTML = '';
            elements.resultsContainer.classList.remove('collapsed');
            
            const [start, end] = await Promise.all([
                geocodeAddress(startText),
                geocodeAddress(endText)
            ]);

            routeLayerGroup.clearLayers();

            L.marker([start.lat, start.lon]).addTo(routeLayerGroup).bindPopup('Départ: ' + start.label);
            L.marker([end.lat, end.lon]).addTo(routeLayerGroup).bindPopup('Arrivée: ' + end.label);

            const osrmUrl = `https://router.project-osrm.org/route/v1/foot/${start.lon},${start.lat};${end.lon},${end.lat}?overview=full&geometries=geojson&steps=true`;
            const res = await fetch(osrmUrl);
            if (!res.ok) throw new Error('Service d\'itinéraire indisponible');
            const routeData = await res.json();
            if (!routeData.routes || !routeData.routes.length) throw new Error('Aucun itinéraire trouvé');
            
            const route = routeData.routes[0];

            const coords = route.geometry.coordinates.map(([lon, lat]) => [lat, lon]);
            const line = L.polyline(coords, { 
                color: 'var(--primary-color)', 
                weight: 6, 
                opacity: 0.8
            }).addTo(routeLayerGroup);
            
            currentMap.fitBounds(line.getBounds().pad(0.15));

            renderSteps(route.legs);

            const km = (route.distance / 1000).toFixed(1);
            const min = Math.round(route.duration / 60);
            setStatus(`Distance: ${km} km • Durée: ~${min} min`);

        } catch (err) {
            setStatus('Erreur: ' + (err.message || 'échec du chargement'), 'error');
        }
    }

    function renderSteps(legs) {
        elements.stepsEl.innerHTML = '';
        
        const icons = {
            turn: '↱', continue: '→', depart: '●', arrive: '★',
            fork: 'C', merge: '⑁', roundabout: '⟲'
        };

        legs.forEach(leg => {
            leg.steps.forEach(step => {
                const name = step.name || '';
                const dist = Math.max(1, Math.round(step.distance));
                const instr = step.maneuver.instruction || step.maneuver.type || 'Continuer';
                const type = (step.maneuver.type || 'continue').toLowerCase();
                const icon = icons[type] || icons.continue;

                const li = document.createElement('li');
                li.className = 'step-item';
                li.innerHTML = `
                    <div class="step-marker">${icon}</div>
                    <div class="step-content">
                        <div class="step-title">${instr}</div>
                        ${name ? `<div class="step-sub">sur ${name}</div>` : ''}
                    </div>
                    <div class="step-badge">${dist} m</div>
                `;
                elements.stepsEl.appendChild(li);
            });
        });
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
    
    function toggleView() {
        const isMapView = document.body.classList.contains('view-map');
        setView(isMapView ? 'panel' : 'map');
    }
    
    function checkAndSwitchView() {
        if (window.innerWidth < 768 && elements.startAC.value && elements.endAC.value) {
            setView('map');
        }
    }

    initMap();

    elements.panelToggleBtn.addEventListener('click', togglePanel);
    elements.statusEl.addEventListener('click', toggleSteps);
    elements.calculateBtn.addEventListener('click', handleRouteCalculation);
    elements.viewSwitchBtn.addEventListener('click', toggleView);
    
    elements.startAC.addEventListener('address-selected', checkAndSwitchView);
    elements.endAC.addEventListener('address-selected', checkAndSwitchView);

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