
const TOASTER_DURATION_MS = 6000;

export function showNotification(title, message, level = 'info') {
    const toasterContainer = document.getElementById("toaster-container");
    if (!toasterContainer) return;

    const toaster = document.createElement('div');
    toaster.classList.add('toaster-notification', level);

    // Icon selection
    let icon = '';
    if (level === 'error' || level === 'high') {
        icon = `<svg class="toaster-icon" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path><line x1="12" y1="9" x2="12" y2="13"></line><line x1="12" y1="17" x2="12.01" y2="17"></line></svg>`;
    } else if (level === 'warning') {
        icon = `<svg class="toaster-icon" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="8" x2="12" y2="12"></line><line x1="12" y1="16" x2="12.01" y2="16"></line></svg>`;
    } else {
        icon = `<svg class="toaster-icon" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"></circle><line x1="12" y1="16" x2="12" y2="12"></line><line x1="12" y1="8" x2="12.01" y2="8"></line></svg>`;
    }

    toaster.innerHTML = `
        <div class="toaster-icon-wrapper">${icon}</div>
        <div class="toaster-content">
            <button class="close-btn">&times;</button>
            <h4>${title}</h4>
            <p>${message}</p>
            <div class="toaster-timer-bar"></div>
        </div>
    `;

    toasterContainer.prepend(toaster);

    const dismiss = () => {
        toaster.classList.add('dismiss');
        setTimeout(() => toaster.remove(), 500);
    };

    const timer = setTimeout(dismiss, TOASTER_DURATION_MS);

    const closeBtn = toaster.querySelector('.close-btn');
    if (closeBtn) {
        closeBtn.addEventListener('click', () => {
            clearTimeout(timer);
            dismiss();
        });
    }

    // Swipe to dismiss logic
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
}
