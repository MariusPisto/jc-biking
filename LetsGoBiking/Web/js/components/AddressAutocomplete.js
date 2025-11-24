import { searchAddresses } from '../api.js';

export class AddressAutocomplete extends HTMLElement {
    constructor() {
        super();

        this.wrapper = document.createElement('div');
        this.wrapper.className = 'ac-wrapper';

        this.input = document.createElement('input');
        this.input.type = 'text';
        this.input.autocomplete = 'off';
        this.input.className = 'ac-input';

        this.suggestions = document.createElement('ul');
        this.suggestions.className = 'ac-suggestions';
        this.suggestions.hidden = true;

        this.wrapper.appendChild(this.input);
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
        this.suggestions.addEventListener('mousedown', (e) => {
            const li = e.target.closest('li');
            if (li && li.dataset.fulladdress) {
                this.selectSuggestion(li.dataset.fulladdress);
            }
        });
    }

    onInput(e) {
        const value = e.target.value.trim();
        if (this.debounceTimeout) clearTimeout(this.debounceTimeout);
        if (!value) {
            this.hideSuggestions();
            return;
        }
        this.debounceTimeout = setTimeout(() => this.fetchSuggestions(value), 1000);
    }

    fetchSuggestions(query) {
        this.setLoading(true);
        searchAddresses(query)
            .then(results => {
                this.showSuggestions(results.map(r => r.label));
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

    get value() { return this.input.value; }
    set value(val) { this.input.value = val; }
}
customElements.define('address-autocomplete', AddressAutocomplete);