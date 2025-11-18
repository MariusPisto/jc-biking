export async function geocodeAddress(address) {
    const url = `https://api-adresse.data.gouv.fr/search/?q=${encodeURIComponent(address)}&limit=1`;
    const res = await fetch(url);
    if (!res.ok) throw new Error('Échec du géocodage');
    const data = await res.json();
    if (!data.features || !data.features.length) throw new Error(`Adresse introuvable: ${address}`);
    const { coordinates } = data.features[0].geometry;
    const { label } = data.features[0].properties;
    return { lat: coordinates[1], lon: coordinates[0], label };
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