/* Master Data: CRUD for Time Accounts, Activities, Objectives and Categories.
   Inline per-cell editing (no modal), matching chronos.html's original UX pattern.
   Delete failures (FK violations from the server) surface as an error toast instead
   of a client-side pre-check, since the server is now authoritative. */

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
        <thead><tr><th>Color</th><th>Name</th><th>Work time</th><th></th></tr></thead>
        <tbody id="ta-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addTimeAccount()">${ICONS.plus} Add time account</button>
      </div>
    </div>
  `;
  renderTimeAccountsRows();
}

function renderTimeAccountsRows() {
  const body = document.getElementById('ta-body');
  if (!body) return;

  if (!cache.timeAccounts.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="4">No time accounts yet.</td></tr>`;
    return;
  }

  body.innerHTML = cache.timeAccounts.map((a) => `
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
        <thead><tr><th>Name</th><th>Time account</th><th></th></tr></thead>
        <tbody id="act-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addActivity()">${ICONS.plus} Add activity</button>
      </div>
    </div>
  `;
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

  body.innerHTML = cache.activities.map((a) => `
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
        <thead><tr><th>Name</th><th>Description</th><th>Category</th><th>Done</th><th></th></tr></thead>
        <tbody id="obj-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addObjective()">${ICONS.plus} Add objective</button>
      </div>
    </div>
  `;
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

  body.innerHTML = cache.objectives.map((o) => `
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
        <thead><tr><th>Name</th><th></th></tr></thead>
        <tbody id="cat-body"></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="addCategory()">${ICONS.plus} Add category</button>
      </div>
    </div>
  `;
  renderCategoriesRows();
}

function renderCategoriesRows() {
  const body = document.getElementById('cat-body');
  if (!body) return;

  if (!cache.categories.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="2">No categories yet.</td></tr>`;
    return;
  }

  body.innerHTML = cache.categories.map((c) => `
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
