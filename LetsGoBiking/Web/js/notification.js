document.addEventListener("DOMContentLoaded", () => {
    const WEBSOCKET_URL = "ws://localhost:61614";
    const BROKER_LOGIN = "admin";
    const BROKER_PASSCODE = "password";
    const NOTIFICATION_TOPIC = "/topic/project-notifications";
    const TOASTER_DURATION_MS = 6000; // Durée de la barre de 6 secondes

    const weatherCheckbox = document.getElementById("sub-weather-btn");
    const pollutionCheckbox = document.getElementById("sub-pollution-btn");
    const toasterContainer = document.getElementById("toaster-container");

    let stompClient = null;
    const activeSubscriptions = {}; 

    function showNotification(message) {
    try {
        const notif = JSON.parse(message.body);
        const level = notif.Level ? notif.Level.toLowerCase() : 'info';

        const toaster = document.createElement('div');
        toaster.classList.add('toaster-notification', level);

        toaster.innerHTML = `
            <button class="close-btn">&times;</button>
            <h4>${notif.Type.toUpperCase()}</h4>
            <p>${notif.Message}</p>
            <div class="toaster-timer-bar"></div>
        `;

        toasterContainer.prepend(toaster);

        const dismiss = () => {
            toaster.classList.add('dismiss');
            setTimeout(() => toaster.remove(), 500);
        };

        const timer = setTimeout(dismiss, TOASTER_DURATION_MS);

        toaster.querySelector('.close-btn').addEventListener('click', () => {
            clearTimeout(timer);
            dismiss();
        });

        let touchStartX = 0;
        let touchStartY = 0;

        toaster.addEventListener('touchstart', (e) => {
            touchStartX = e.changedTouches[0].screenX;
            touchStartY = e.changedTouches[0].screenY;
        }, { passive: true });

        toaster.addEventListener('touchend', (e) => {
            const touchEndX = e.changedTouches[0].screenX;
            const touchEndY = e.changedTouches[0].screenY;
            const deltaX = touchEndX - touchStartX;
            const deltaY = touchEndY - touchStartY;

            if (window.innerWidth <= 768) { // Mobile
                if (deltaY < -50 && Math.abs(deltaX) < 50) {
                    clearTimeout(timer);
                    dismiss();
                }
            } else { // Desktop
                if (deltaX > 50 && Math.abs(deltaY) < 50) {
                    clearTimeout(timer);
                    dismiss();
                }
            }
        }, { passive: true });

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
            const sub = stompClient.subscribe(NOTIFICATION_TOPIC, showNotification, {
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
                showNotification({ 
                    body: JSON.stringify({
                        Type: "Système",
                        Level: "high",
                        Message: "Connexion au service d'alertes échouée."
                    })
                });
            }
        );
    }

    connectToBroker();
});