import { ORS_API_KEY } from '../../config.js';

export async function getRouteSegments(start, pickup, dropoff, end) {
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
      
    return {
        routeWalk1: walk1Data.features[0],
        routeBike: bikeData.features[0],
        routeWalk2: walk2Data.features[0]
    };
}