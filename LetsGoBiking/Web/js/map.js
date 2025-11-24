export function initMap(mapEl) {
    const map = L.map(mapEl, {
        zoomControl: false
    }).setView([43.6152, 7.0702], 12);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);
    
    const zoomControl = L.control.zoom({ position: 'bottomright' }).addTo(map);
    map.zoomControlContainer = zoomControl.getContainer();
    
    return map;
}