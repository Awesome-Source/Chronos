/* Table dropdown: a select-like control whose dropdown panel holds a filterable table.
   Rows must carry an `id`; columns are { label, value: (row) => text }. The first column's text is
   shown in the trigger. Styled by the .td rules in style.css. */

class TableDropdown {
  constructor(hostId, columns) {
    this.columns = columns;
    this.rows = [];
    this.filtered = [];
    this.selectedId = null;
    this.activeIndex = 0;

    this.host = document.getElementById(hostId);
    this.host.classList.add('td');
    this.host.innerHTML = `
      <button type="button" class="td-trigger" id="${hostId}-trigger" aria-haspopup="true" aria-expanded="false"></button>
      <div class="td-panel">
        <input type="text" class="td-filter" placeholder="Filter…" autocomplete="off">
        <div class="td-scroll">
          <table class="data-table">
            <thead><tr>${columns.map((c) => `<th>${escapeHtml(c.label)}</th>`).join('')}</tr></thead>
            <tbody></tbody>
          </table>
        </div>
      </div>
    `;

    this.trigger = this.host.querySelector('.td-trigger');
    this.panel = this.host.querySelector('.td-panel');
    this.filterInput = this.host.querySelector('.td-filter');
    this.body = this.host.querySelector('tbody');

    this.trigger.addEventListener('click', () => (this.isOpen ? this.close() : this.open()));
    this.trigger.addEventListener('keydown', (e) => {
      if (e.key === 'ArrowDown') {
        e.preventDefault();
        this.open();
      }
    });
    this.filterInput.addEventListener('input', () => {
      this.activeIndex = 0;
      this.renderRows();
    });
    this.filterInput.addEventListener('keydown', (e) => this.onFilterKeydown(e));
    this.body.addEventListener('click', (e) => {
      const tr = e.target.closest('tr[data-index]');
      if (tr) this.select(this.filtered[Number(tr.dataset.index)]);
    });
    this.body.addEventListener('mousemove', (e) => {
      const tr = e.target.closest('tr[data-index]');
      if (tr) this.setActive(Number(tr.dataset.index), false);
    });
    // Escape closes only the panel, not the surrounding modal (app.js listens on document).
    this.host.addEventListener('keydown', (e) => {
      if (e.key === 'Escape' && this.isOpen) {
        e.stopPropagation();
        this.close();
        this.trigger.focus();
      }
    });
    document.addEventListener('mousedown', (e) => {
      if (this.isOpen && !this.host.contains(e.target)) this.close();
    });
  }

  get isOpen() {
    return this.host.classList.contains('open');
  }

  /** Selected row id, or null when there are no rows. */
  get value() {
    return this.selectedId;
  }

  /** Replaces the rows, selects the first one (like a <select>), and resets filter and panel. */
  setRows(rows) {
    this.rows = rows;
    this.selectedId = rows.length ? rows[0].id : null;
    this.filterInput.value = '';
    this.close();
    this.updateTrigger();
  }

  updateTrigger() {
    const row = this.rows.find((r) => r.id === this.selectedId);
    this.trigger.textContent = row ? String(this.columns[0].value(row) ?? '') : 'No entries';
  }

  open() {
    if (!this.rows.length) return;

    this.filterInput.value = '';
    this.filtered = this.rows;
    this.activeIndex = Math.max(0, this.rows.findIndex((r) => r.id === this.selectedId));
    this.renderRows();

    this.host.classList.remove('up');
    this.host.classList.add('open');
    this.trigger.setAttribute('aria-expanded', 'true');

    // Flip above the trigger when the panel does not fit below it.
    const rect = this.host.getBoundingClientRect();
    const spaceBelow = window.innerHeight - rect.bottom;
    if (spaceBelow < this.panel.offsetHeight + 8 && rect.top > spaceBelow) this.host.classList.add('up');

    this.filterInput.focus();
    this.scrollActiveIntoView();
  }

  close() {
    this.host.classList.remove('open', 'up');
    this.trigger.setAttribute('aria-expanded', 'false');
  }

  select(row) {
    if (!row) return;
    this.selectedId = row.id;
    this.updateTrigger();
    this.close();
    this.trigger.focus();
  }

  renderRows() {
    const query = this.filterInput.value.trim().toLowerCase();
    this.filtered = this.rows.filter((row) => !query
      || this.columns.some((c) => String(c.value(row) ?? '').toLowerCase().includes(query)));

    if (!this.filtered.length) {
      this.body.innerHTML = `<tr><td class="empty-state" colspan="${this.columns.length}">No matching rows.</td></tr>`;
      return;
    }

    this.body.innerHTML = this.filtered.map((row, i) => `
      <tr data-index="${i}" class="${row.id === this.selectedId ? 'row-selected' : ''} ${i === this.activeIndex ? 'td-active' : ''}">
        ${this.columns.map((c) => `<td>${escapeHtml(c.value(row) ?? '')}</td>`).join('')}
      </tr>
    `).join('');
  }

  setActive(index, scroll = true) {
    if (index === this.activeIndex || index < 0 || index >= this.filtered.length) return;
    this.body.querySelector('tr.td-active')?.classList.remove('td-active');
    this.activeIndex = index;
    this.body.querySelector(`tr[data-index="${index}"]`)?.classList.add('td-active');
    if (scroll) this.scrollActiveIntoView();
  }

  scrollActiveIntoView() {
    this.body.querySelector('tr.td-active')?.scrollIntoView({ block: 'nearest' });
  }

  onFilterKeydown(e) {
    if (e.key === 'ArrowDown') {
      e.preventDefault();
      this.setActive(this.activeIndex + 1);
    } else if (e.key === 'ArrowUp') {
      e.preventDefault();
      this.setActive(this.activeIndex - 1);
    } else if (e.key === 'Enter') {
      e.preventDefault();
      this.select(this.filtered[this.activeIndex]);
    } else if (e.key === 'Tab') {
      this.close();
    }
  }
}
