import { showNotification } from './toaster.js';

document.addEventListener("DOMContentLoaded", () => {
    const WEBSOCKET_URL = "ws://localhost:61614";
    const BROKER_LOGIN = "admin";
    const BROKER_PASSCODE = "password";
    const NOTIFICATION_TOPIC = "/topic/project-notifications";

    const weatherCheckbox = document.getElementById("sub-weather-btn");
    const pollutionCheckbox = document.getElementById("sub-pollution-btn");

    let stompClient = null;
    const activeSubscriptions = {};

    function handleNotificationMessage(message) {
        try {
            const notif = JSON.parse(message.body);
            const level = notif.Level ? notif.Level.toLowerCase() : 'info';
            showNotification(notif.Type.toUpperCase(), notif.Message, level);
        } catch (e) {
            console.error("Erreur réception notif : ", e, message.body);
        }
    }

    function toggleSubscription(checkbox, filterType, filterValue) {
        if (!stompClient || !stompClient.connected) {
            console.warn("Client STOMP non connecté. Impossible de s'abonner.");
            return;
        }

        if (checkbox.checked) {
            // S'abonner
            console.log(`Abonnement à ${filterType} = ${filterValue}`);
            const sub = stompClient.subscribe(NOTIFICATION_TOPIC, handleNotificationMessage, {
                id: filterValue,
                selector: `${filterType} = '${filterValue}'`
            });
            activeSubscriptions[filterValue] = sub;
        } else {
            // Se désabonner
            console.log(`Désabonnement de ${filterValue}`);
            if (activeSubscriptions[filterValue]) {
                stompClient.unsubscribe(activeSubscriptions[filterValue].id);
                delete activeSubscriptions[filterValue];
            }
        }
    }

    function connectToBroker() {
        if (!window.Stomp) {
            console.error("La librairie stomp.js n'est pas chargée.");
            return;
        }

        stompClient = Stomp.client(WEBSOCKET_URL);
        stompClient.debug = null;
        stompClient.connect(
            BROKER_LOGIN,
            BROKER_PASSCODE,
            (frame) => {
                console.log("Connecté à ActiveMQ via STOMP/WS.");

                weatherCheckbox.addEventListener("change", () => {
                    toggleSubscription(weatherCheckbox, 'notificationType', 'weather');
                });

                pollutionCheckbox.addEventListener("change", () => {
                    toggleSubscription(pollutionCheckbox, 'notificationType', 'pollution');
                });
            },
            (error) => {
                console.error("Erreur de connexion STOMP : ", error);
                showNotification("Système", "Connexion au service d'alertes échouée.", "high");
            }
        );
    }

    connectToBroker();
});