export async function searchAddresses(query) {
    // Call the backend server which calls the proxy -> Geoapify
    const url = `http://localhost:8733/api/addresses?text=${encodeURIComponent(query)}`;
    try {
        const res = await fetch(url);
        if (!res.ok) throw new Error('Échec de la recherche');
        const data = await res.json();
        return data.map(addr => ({
            label: addr.Label,
            lat: addr.Lat,
            lon: addr.Lon
        }));
    } catch (e) {
        console.error("Error fetching addresses:", e);
        return [];
    }
}

export async function geocodeAddress(address) {
    // Check if input is in coordinate format (e.g., "43.6152, 7.0702" or "43.6152,7.0702")
    const coordPattern = /^\s*(-?\d+\.?\d*)\s*,\s*(-?\d+\.?\d*)\s*$/;
    const match = address.trim().match(coordPattern);
    
    if (match) {
        const lat = parseFloat(match[1]);
        const lon = parseFloat(match[2]);
        
        // Validate coordinate ranges
        if (isNaN(lat) || isNaN(lon) || lat < -90 || lat > 90 || lon < -180 || lon > 180) {
            throw new Error(`Coordonnées invalides: ${address}`);
        }
        
        return { lat, lon, label: address.trim() };
    }
    
    // Otherwise, treat as address and search
    const results = await searchAddresses(address);
    if (!results.length) throw new Error(`Adresse introuvable: ${address}`);
    return results[0];
}

export async function getItinerary(start, end) {
    const isDemo = localStorage.getItem('itinerary_demo_mode') === 'true';
    const backendUrl = isDemo
        ? "http://localhost:8733/api/itinerary?originLat=50.8998481&originLng=4.2808363&destLat=49.5806013&destLng=6.1321121"
        : `http://localhost:8733/api/itinerary?originLat=${start.lat}&originLng=${start.lon}&destLat=${end.lat}&destLng=${end.lon}`;
    try {
        const backendRes = await fetch(backendUrl);
        if (!backendRes.ok) throw new Error(`Erreur ${backendRes.status}`);
        return await backendRes.json();
    } catch (backendErr) {
        throw new Error(`Service de vélos indisponible (vérifiez que le service C# est lancé sur localhost:8733)`);
    }
}