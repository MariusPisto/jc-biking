export function setStatus(statusEl, text, type = 'info') {
    statusEl.innerHTML = text || ''; 
    statusEl.style.color = type === 'error' ? '#d93025' : 'var(--text-dark)';
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
                <div class="step-marker" style="font-size: 1.5rem; color: var(--text-light);">${icon}</div>
                <div class="step-content">
                    <div class="step-title" style="font-weight: 500;">${instr}</div>
                    ${name ? `<div class="step-sub">sur ${name}</div>` : ''}
                </div>
                <div class="step-badge">${dist} m</div>
            `;
            stepsEl.appendChild(li);
        });
    });
}

export function addCustomStep(stepsEl, title, subtext = '', icon = '📍') {
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
    stepsEl.appendChild(li);
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