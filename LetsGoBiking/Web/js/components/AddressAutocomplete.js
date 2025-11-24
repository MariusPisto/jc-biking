export class AddressAutocomplete extends HTMLElement {
    constructor() {
        super();
          
        this.wrapper = document.createElement('div');
        this.wrapper.className = 'ac-wrapper';
          
        this.input = document.createElement('input');
        this.input.type = 'text';
        this.input.autocomplete = 'off';
        this.input.className = 'ac-input';
        
        this.clearBtn = document.createElement('button');
        this.clearBtn.type = 'button';
        this.clearBtn.className = 'ac-clear';
        this.clearBtn.setAttribute('aria-label', 'Effacer le champ');
        this.clearBtn.textContent = '×';
        this.clearBtn.hidden = true;

        this.suggestions = document.createElement('ul');
        this.suggestions.className = 'ac-suggestions';
        this.suggestions.hidden = true;
        
        this.wrapper.appendChild(this.input);
        this.wrapper.appendChild(this.clearBtn);
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
        this.clearBtn.addEventListener('click', () => this.clearInput());
        this.suggestions.addEventListener('mousedown', (e) => {
            const li = e.target.closest('li');
            if (li && li.dataset.fulladdress) {
                this.selectSuggestion(li.dataset.fulladdress);
            }
        });

        this.updateClearButtonVisibility();
    }

    onInput(e) {
        const value = e.target.value.trim();
        this.updateClearButtonVisibility();
        if (this.debounceTimeout) clearTimeout(this.debounceTimeout);
        if (!value) {
            this.hideSuggestions();
            return;
        }
        this.debounceTimeout = setTimeout(() => this.fetchSuggestions(value), 300);
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
        this.updateClearButtonVisibility();
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

    clearInput() {
        if (!this.input.value) return;
        this.input.value = '';
        this.hideSuggestions();
        this.updateClearButtonVisibility();
        this.dispatchEvent(new CustomEvent('address-cleared', { bubbles: true }));
    }

    updateClearButtonVisibility() {
        this.clearBtn.hidden = !this.input.value;
    }

    get value() { return this.input.value; }
    set value(val) {
        this.input.value = val;
        this.updateClearButtonVisibility();
    }
}
customElements.define('address-autocomplete', AddressAutocomplete);