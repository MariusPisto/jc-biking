import { showNotification } from './toaster.js';

export function setStatus(statusEl, text, type = 'info') {
    if (type === 'error') {
        showNotification('Erreur', text, 'error');
        statusEl.innerHTML = '';
    } else {
        statusEl.innerHTML = text || '';
        statusEl.style.color = 'var(--text-dark)';
    }
}

export function appendRouteSteps(stepsEl, segments) {
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
                <div class="step-marker">${icon}</div>
                <div class="step-content">
                    <div class="step-title">${instr}</div>
                    ${name ? `<div class="step-sub">sur ${name}</div>` : ''}
                </div>
                <div class="step-badge">${dist} m</div>
            `;
            stepsEl.appendChild(li);
        });
    });
}

export function createRouteSegment(stepsEl, title, subtext = '', icon = '📍') {
    const li = document.createElement('li');
    li.className = 'segment-item';

    const header = document.createElement('div');
    header.className = 'step-item segment-header';

    header.innerHTML = `
        <div class="step-marker" style="color: var(--text-dark);">${icon}</div>
        <div class="step-content">
            <div class="step-title">${title}</div>
            ${subtext ? `<div class="step-sub" style="color: var(--primary-color); font-weight: 500;">${subtext}</div>` : ''}
        </div>
        <div class="toggle-icon">
            <svg width="12" height="12" viewBox="0 0 12 12" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="2 4 6 8 10 4"></polyline>
            </svg>
        </div>
    `;

    const stepsList = document.createElement('ul');
    stepsList.className = 'segment-steps';

    header.addEventListener('click', () => {
        li.classList.toggle('expanded');
    });

    li.appendChild(header);
    li.appendChild(stepsList);
    stepsEl.appendChild(li);

    return stepsList;
}

export function addCustomStep(stepsEl, title, subtext = '', icon = '📍') {
    createRouteSegment(stepsEl, title, subtext, icon);
}

export function togglePanel(panel, panelToggleBtn, map) {
    panel.classList.toggle('collapsed');
    panelToggleBtn.classList.toggle('collapsed');

    const isCollapsed = panel.classList.contains('collapsed');
    panelToggleBtn.setAttribute('aria-label',
        isCollapsed ? 'Afficher le panneau' : 'Masquer le panneau'
    );

    setTimeout(() => {
        map.invalidateSize();
    }, 400);
}

export function toggleSteps(resultsContainer) {
    resultsContainer.classList.toggle('collapsed');
}

export function setView(view, map, viewSwitchBtn) {
    if (view === 'map') {
        document.body.classList.add('view-map');
        viewSwitchBtn.textContent = 'Voir le panneau';
        setTimeout(() => {
            map.invalidateSize();
        }, 400);
    } else {
        document.body.classList.remove('view-map');
        viewSwitchBtn.textContent = 'Voir la carte';
    }
}