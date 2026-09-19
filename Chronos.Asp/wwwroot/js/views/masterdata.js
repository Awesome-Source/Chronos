/* Master Data: CRUD for Time Accounts, Activities, Objectives and Categories.
   Inline per-cell editing (no modal), matching chronos.html's original UX pattern.
   Delete failures (FK violations from the server) surface as an error toast instead
   of a client-side pre-check, since the server is now authoritative. */

/* ---------- Sorting & filtering (frontend only) ----------
   Every table has clickable sort headers and a filter row. Only <tbody> is re-rendered on
   sort/filter changes, so the filter inputs in <thead> keep focus while typing. State lives in
   masterView (state.js) and is applied to a copy of the cache right before rows are rendered. */

const MASTER_TABLES = {
  accounts: {
    columns: [
      { key: 'color', label: 'Color', type: 'color', value: (r) => r.color },
      { key: 'name', label: 'Name', type: 'text', value: (r) => r.name },
      { key: 'isWorkTime', label: 'Work time', type: 'bool', value: (r) => r.isWorkTime },
    ],
    renderRows: () => renderTimeAccountsRows(),
  },
  activities: {
    columns: [
      { key: 'name', label: 'Name', type: 'text', value: (r) => r.name },
      { key: 'timeAccount', label: 'Time account', type: 'text', value: (r) => timeAccountById(r.timeAccountId)?.name },
    ],
    renderRows: () => renderActivitiesRows(),
  },
  objectives: {
    columns: [
      { key: 'name', label: 'Name', type: 'text', value: (r) => r.name },
      { key: 'description', label: 'Description', type: 'text', value: (r) => r.description },
      { key: 'category', label: 'Category', type: 'text', value: (r) => cache.categories.find((c) => c.id === r.categoryId)?.name },
      { key: 'isDone', label: 'Done', type: 'bool', value: (r) => r.isDone },
    ],
    renderRows: () => renderObjectivesRows(),
  },
  categories: {
    columns: [
      { key: 'name', label: 'Name', type: 'text', value: (r) => r.name },
    ],
    renderRows: () => renderCategoriesRows(),
  },
};

/** Two-row <thead>: clickable sort headers, then the filter row (color columns get no filter). */
function masterTableHead(table) {
  const { columns } = MASTER_TABLES[table];
  const { sort, filters } = masterView[table];

  const headCells = columns.map((c) => {
    const ariaSort = sort && sort.key === c.key ? (sort.dir === 'asc' ? 'ascending' : 'descending') : 'none';
    return `<th data-key="${c.key}" aria-sort="${ariaSort}"><button type="button" class="sort-btn" onclick="setMasterSort('${table}', '${c.key}')">${c.label}</button></th>`;
  }).join('');

  const filterCells = columns.map((c) => {
    if (c.type === 'color') return '<th></th>';
    if (c.type === 'bool') {
      const state = filters[c.key] ?? null;
      return `<th><input type="checkbox" class="tri-filter" data-tri="${state}" title="Filter: any / yes / no" onclick="cycleMasterBoolFilter('${table}', '${c.key}', this)"></th>`;
    }
    return `<th><input type="text" class="filter-input" placeholder="Filter…" value="${escapeHtml(filters[c.key] ?? '')}" oninput="setMasterFilter('${table}', '${c.key}', this.value)"></th>`;
  }).join('');

  return `<thead><tr>${headCells}<th></th></tr><tr class="filter-row">${filterCells}<th></th></tr></thead>`;
}

/** Applies indeterminate/checked to the tri-state filter checkboxes (not settable via HTML attributes). */
function initMasterTriStates(panel) {
  panel.querySelectorAll('input[data-tri]').forEach((el) => {
    setTriState(el, el.dataset.tri === 'true' ? true : el.dataset.tri === 'false' ? false : null);
  });
}

function setTriState(el, value) {
  el.indeterminate = value === null;
  el.checked = value === true;
}

function compareMasterValues(a, b, type) {
  if (type === 'bool') return Number(Boolean(a)) - Number(Boolean(b));
  return String(a ?? '').localeCompare(String(b ?? ''), undefined, { numeric: true, sensitivity: 'base' });
}

/** Returns the rows matching the table's filters, ordered by its sort. Never mutates the input. */
function applyMasterView(table, rows) {
  const { columns } = MASTER_TABLES[table];
  const { sort, filters } = masterView[table];

  const result = rows.filter((row) => columns.every((c) => {
    const filter = filters[c.key];
    if (filter === undefined || c.type === 'color') return true;
    if (c.type === 'bool') return Boolean(c.value(row)) === filter;
    return String(c.value(row) ?? '').toLowerCase().includes(filter.trim().toLowerCase());
  }));

  if (sort) {
    const column = columns.find((c) => c.key === sort.key);
    const direction = sort.dir === 'asc' ? 1 : -1;
    result.sort((a, b) => direction * compareMasterValues(column.value(a), column.value(b), column.type));
  }
  return result;
}

/** Header click: ascending, then descending, then unsorted. */
function setMasterSort(table, key) {
  const view = masterView[table];
  if (!view.sort || view.sort.key !== key) view.sort = { key, dir: 'asc' };
  else if (view.sort.dir === 'asc') view.sort = { key, dir: 'desc' };
  else view.sort = null;

  document.querySelectorAll(`#tab-${table} thead th[data-key]`).forEach((th) => {
    const active = view.sort && view.sort.key === th.dataset.key;
    th.setAttribute('aria-sort', active ? (view.sort.dir === 'asc' ? 'ascending' : 'descending') : 'none');
  });
  MASTER_TABLES[table].renderRows();
}

function setMasterFilter(table, key, value) {
  const { filters } = masterView[table];
  if (value === '') delete filters[key];
  else filters[key] = value;
  MASTER_TABLES[table].renderRows();
}

/** Tri-state checkbox filter: any (indeterminate) -> yes (checked) -> no (unchecked) -> any. */
function cycleMasterBoolFilter(table, key, el) {
  const { filters } = masterView[table];
  const current = filters[key] ?? null;
  const next = current === null ? true : current === true ? false : null;

  if (next === null) delete filters[key];
  else filters[key] = next;
  setTriState(el, next);
  MASTER_TABLES[table].renderRows();
}

function renderMasterData() {
  const page = document.getElementById('page-masterdata');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Master Data</h1><span class="sub">Manage time accounts, activities, objectives and categories</span></div>
    </div>
    <div class="tabs">
      <button class="tab-btn" data-tab="accounts" onclick="setMasterTab('accounts')">Time Accounts</button>
      <button class="tab-btn" data-tab="activities" onclick="setMasterTab('activities')">Activities</button>
      <button class="tab-btn" data-tab="objectives" onclick="setMasterTab('objectives')">Objectives</button>
      <button class="tab-btn" data-tab="categories" onclick="setMasterTab('categories')">Categories</button>
    </div>
    <div class="tab-panel" id="tab-accounts"></div>
    <div class="tab-panel" id="tab-activities"></div>
    <div class="tab-panel" id="tab-objectives"></div>
    <div class="tab-panel" id="tab-categories"></div>
  `;
  setMasterTab(masterTab);
}

function setMasterTab(tab) {
  masterTab = tab;
  document.querySelectorAll('.tab-btn').forEach((btn) => btn.classList.toggle('active', btn.dataset.tab === tab));
  document.querySelectorAll('.tab-panel').forEach((panel) => panel.classList.toggle('active', panel.id === `tab-${tab}`));

  if (tab === 'accounts') renderTimeAccountsTab();
  else if (tab === 'activities') renderActivitiesTab();
  else if (tab === 'objectives') renderObjectivesTab();
  else if (tab === 'categories') renderCategoriesTab();
}

/* ---------- Time Accounts ---------- */

function renderTimeAccountsTab() {
  const panel = document.getElementById('tab-accounts');
  panel.innerHTML = `
    <div class="panel">
      <table class="data-table">
        ${masterTableHead('accounts')}
        <tbody id="ta-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addTimeAccount()">${ICONS.plus} Add time account</button>
      </div>
    </div>
  `;
  initMasterTriStates(panel);
  renderTimeAccountsRows();
}

function renderTimeAccountsRows() {
  const body = document.getElementById('ta-body');
  if (!body) return;

  if (!cache.timeAccounts.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="4">No time accounts yet.</td></tr>`;
    return;
  }

  const rows = applyMasterView('accounts', cache.timeAccounts);
  if (!rows.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="4">No matching rows.</td></tr>`;
    return;
  }

  body.innerHTML = rows.map((a) => `
    <tr>
      <td><input type="color" value="${a.color}" onchange="updateTimeAccountField(${a.id}, 'colorHex', this.value)"></td>
      <td><input class="name-input" type="text" value="${escapeHtml(a.name)}" onchange="updateTimeAccountField(${a.id}, 'name', this.value)"></td>
      <td><input type="checkbox" ${a.isWorkTime ? 'checked' : ''} onchange="updateTimeAccountField(${a.id}, 'isWorkTime', this.checked)"></td>
      <td><button class="row-icon-btn danger" onclick="deleteTimeAccount(${a.id})">${ICONS.trash}</button></td>
    </tr>
  `).join('');
}

async function updateTimeAccountField(id, field, value) {
  const account = timeAccountById(id);
  if (!account) return;

  const payload = {
    name: field === 'name' ? value : account.name,
    colorHex: field === 'colorHex' ? value : account.color,
    isWorkTime: field === 'isWorkTime' ? value : account.isWorkTime,
  };

  try {
    await Api.updateTimeAccount(id, payload);
    await refreshMasterDataCache();
    toast('Time account updated.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderTimeAccountsRows();
}

async function deleteTimeAccount(id) {
  try {
    await Api.removeTimeAccount(id);
    await refreshMasterDataCache();
    toast('Time account removed.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderTimeAccountsRows();
}

async function addTimeAccount() {
  try {
    await Api.createTimeAccount({ name: 'New time account', colorHex: '#379468', isWorkTime: true });
    await refreshMasterDataCache();
    toast('Time account added.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderTimeAccountsRows();
}

/* ---------- Activities ---------- */

function renderActivitiesTab() {
  const panel = document.getElementById('tab-activities');
  panel.innerHTML = `
    <div class="panel">
      <table class="data-table">
        ${masterTableHead('activities')}
        <tbody id="act-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addActivity()">${ICONS.plus} Add activity</button>
      </div>
    </div>
  `;
  initMasterTriStates(panel);
  renderActivitiesRows();
}

function renderActivitiesRows() {
  const body = document.getElementById('act-body');
  if (!body) return;

  if (!cache.timeAccounts.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="3">Add a time account first.</td></tr>`;
    return;
  }
  if (!cache.activities.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="3">No activities yet.</td></tr>`;
    return;
  }

  const rows = applyMasterView('activities', cache.activities);
  if (!rows.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="3">No matching rows.</td></tr>`;
    return;
  }

  body.innerHTML = rows.map((a) => `
    <tr>
      <td><input class="name-input" type="text" value="${escapeHtml(a.name)}" onchange="updateActivityField(${a.id}, 'name', this.value)"></td>
      <td>
        <select onchange="updateActivityField(${a.id}, 'timeAccountId', Number(this.value))">
          ${cache.timeAccounts.map((ta) => `<option value="${ta.id}" ${ta.id === a.timeAccountId ? 'selected' : ''}>${escapeHtml(ta.name)}</option>`).join('')}
        </select>
      </td>
      <td><button class="row-icon-btn danger" onclick="deleteActivity(${a.id})">${ICONS.trash}</button></td>
    </tr>
  `).join('');
}

async function updateActivityField(id, field, value) {
  const activity = cache.activities.find((a) => a.id === id);
  if (!activity) return;

  const payload = {
    name: field === 'name' ? value : activity.name,
    timeAccountId: field === 'timeAccountId' ? value : activity.timeAccountId,
  };

  try {
    await Api.updateActivity(id, payload);
    await refreshMasterDataCache();
    toast('Activity updated.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderActivitiesRows();
}

async function deleteActivity(id) {
  try {
    await Api.removeActivity(id);
    await refreshMasterDataCache();
    toast('Activity removed.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderActivitiesRows();
}

async function addActivity() {
  if (!cache.timeAccounts.length) {
    toast('Add a time account first.', 'error');
    return;
  }
  try {
    await Api.createActivity({ name: 'New activity', timeAccountId: cache.timeAccounts[0].id });
    await refreshMasterDataCache();
    toast('Activity added.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderActivitiesRows();
}

/* ---------- Objectives ---------- */

function renderObjectivesTab() {
  const panel = document.getElementById('tab-objectives');
  panel.innerHTML = `
    <div class="panel">
      <table class="data-table">
        ${masterTableHead('objectives')}
        <tbody id="obj-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addObjective()">${ICONS.plus} Add objective</button>
      </div>
    </div>
  `;
  initMasterTriStates(panel);
  renderObjectivesRows();
}

function renderObjectivesRows() {
  const body = document.getElementById('obj-body');
  if (!body) return;

  if (!cache.categories.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="5">Add a category first.</td></tr>`;
    return;
  }
  if (!cache.objectives.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="5">No objectives yet.</td></tr>`;
    return;
  }

  const rows = applyMasterView('objectives', cache.objectives);
  if (!rows.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="5">No matching rows.</td></tr>`;
    return;
  }

  body.innerHTML = rows.map((o) => `
    <tr>
      <td><input class="name-input" type="text" value="${escapeHtml(o.name)}" onchange="updateObjectiveField(${o.id}, 'name', this.value)"></td>
      <td><input class="name-input" type="text" value="${escapeHtml(o.description)}" onchange="updateObjectiveField(${o.id}, 'description', this.value)"></td>
      <td>
        <select onchange="updateObjectiveField(${o.id}, 'categoryId', Number(this.value))">
          ${cache.categories.map((c) => `<option value="${c.id}" ${c.id === o.categoryId ? 'selected' : ''}>${escapeHtml(c.name)}</option>`).join('')}
        </select>
      </td>
      <td><input type="checkbox" ${o.isDone ? 'checked' : ''} onchange="updateObjectiveField(${o.id}, 'isDone', this.checked)"></td>
      <td><button class="row-icon-btn danger" onclick="deleteObjective(${o.id})">${ICONS.trash}</button></td>
    </tr>
  `).join('');
}

async function updateObjectiveField(id, field, value) {
  const objective = cache.objectives.find((o) => o.id === id);
  if (!objective) return;

  const payload = {
    name: field === 'name' ? value : objective.name,
    description: field === 'description' ? value : objective.description,
    categoryId: field === 'categoryId' ? value : objective.categoryId,
    isDone: field === 'isDone' ? value : objective.isDone,
  };

  try {
    await Api.updateObjective(id, payload);
    await refreshMasterDataCache();
    toast('Objective updated.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderObjectivesRows();
}

async function deleteObjective(id) {
  try {
    await Api.removeObjective(id);
    await refreshMasterDataCache();
    toast('Objective removed.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderObjectivesRows();
}

async function addObjective() {
  if (!cache.categories.length) {
    toast('Add a category first.', 'error');
    return;
  }
  try {
    await Api.createObjective({ name: 'New objective', description: '', categoryId: cache.categories[0].id });
    await refreshMasterDataCache();
    toast('Objective added.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderObjectivesRows();
}

/* ---------- Categories ---------- */

function renderCategoriesTab() {
  const panel = document.getElementById('tab-categories');
  panel.innerHTML = `
    <div class="panel">
      <table class="data-table">
        ${masterTableHead('categories')}
        <tbody id="cat-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addCategory()">${ICONS.plus} Add category</button>
      </div>
    </div>
  `;
  initMasterTriStates(panel);
  renderCategoriesRows();
}

function renderCategoriesRows() {
  const body = document.getElementById('cat-body');
  if (!body) return;

  if (!cache.categories.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="2">No categories yet.</td></tr>`;
    return;
  }

  const rows = applyMasterView('categories', cache.categories);
  if (!rows.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="2">No matching rows.</td></tr>`;
    return;
  }

  body.innerHTML = rows.map((c) => `
    <tr>
      <td><input class="name-input" type="text" value="${escapeHtml(c.name)}" onchange="updateCategoryField(${c.id}, this.value)"></td>
      <td><button class="row-icon-btn danger" onclick="deleteCategory(${c.id})">${ICONS.trash}</button></td>
    </tr>
  `).join('');
}

async function updateCategoryField(id, name) {
  try {
    await Api.updateCategory(id, { name });
    await refreshMasterDataCache();
    toast('Category updated.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderCategoriesRows();
}

async function deleteCategory(id) {
  try {
    await Api.removeCategory(id);
    await refreshMasterDataCache();
    toast('Category removed.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderCategoriesRows();
}

async function addCategory() {
  try {
    await Api.createCategory({ name: 'New category' });
    await refreshMasterDataCache();
    toast('Category added.');
  } catch (err) {
    toast(err.message, 'error');
  }
  renderCategoriesRows();
}
