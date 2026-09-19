/* Tracking: the core clock in/out screen. */

let trackingTargetsCache = [];

async function renderTracking() {
  const page = document.getElementById('page-tracking');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Tracking</h1><span class="sub">Today's tracking targets</span></div>
      <div class="page-head-actions">
        <span class="card-sub">Total time: <strong class="mono" id="tracking-total">0m</strong></span>
      </div>
    </div>
    <div class="panel">
      <table class="data-table">
        <thead><tr><th>Activity</th><th>Objective</th><th>Time account</th><th>Planned</th><th class="num">Accumulated</th></tr></thead>
        <tbody id="tracking-body"><tr><td class="empty-state" colspan="5">Loading…</td></tr></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-primary" id="tracking-start" disabled onclick="startSelected()">${ICONS.play} Start selected</button>
        <button class="btn btn-outline" onclick="openNewTrackingTargetModal()">${ICONS.plus} New tracking target</button>
        <button class="btn btn-danger" onclick="stopTracking()">${ICONS.stop} Stop tracking</button>
      </div>
    </div>
  `;
  selectedTargetId = null;
  await refreshTrackingTable();
}

async function refreshTrackingTable() {
  try {
    const targets = await Api.getTodaysTargets();
    trackingTargetsCache = targets;
    renderTrackingRows(targets);

    const total = targets.reduce((sum, t) => sum + t.accumulatedTime, 0);
    const totalEl = document.getElementById('tracking-total');
    if (totalEl) totalEl.textContent = formatDuration(total);
  } catch (err) {
    toast(err.message, 'error');
  }
}

function renderTrackingRows(targets) {
  const body = document.getElementById('tracking-body');
  if (!body) return;

  if (!targets.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="5">No activity targets for today yet. Use "New tracking target" to start one.</td></tr>`;
  } else {
    body.innerHTML = targets.map((t) => `
      <tr class="${t.isActive ? 'row-active' : ''} ${selectedTargetId === t.internalId ? 'row-selected' : ''}" style="cursor:pointer" onclick="selectTarget(${t.internalId})">
        <td>${escapeHtml(t.activityName)}</td>
        <td>${escapeHtml(t.objectiveName)}</td>
        <td>${escapeHtml(t.timeAccountName)}</td>
        <td>${t.isPlannedActivity ? '<span class="badge badge-planned">Planned</span>' : '<span class="badge badge-unplanned">Unplanned</span>'}</td>
        <td class="num mono">${formatDuration(t.accumulatedTime)} ${t.isActive ? '<span class="pulse-dot"></span>' : ''}</td>
      </tr>
    `).join('');
  }

  const startBtn = document.getElementById('tracking-start');
  if (startBtn) startBtn.disabled = selectedTargetId === null;
}

function selectTarget(internalId) {
  selectedTargetId = selectedTargetId === internalId ? null : internalId;
  renderTrackingRows(trackingTargetsCache);
}

async function startSelected() {
  if (selectedTargetId === null) return;
  try {
    await Api.startTracking(selectedTargetId, nowAsTimeOnlyString());
    selectedTargetId = null;
    await refreshTrackingTable();
    await renderStatusBar();
    toast('Tracking started.');
  } catch (err) {
    toast(err.message, 'error');
  }
}

async function stopTracking() {
  try {
    await Api.stopTracking(nowAsTimeOnlyString());
    await refreshTrackingTable();
    await renderStatusBar();
    toast('Tracking stopped.');
  } catch (err) {
    toast(err.message, 'error');
  }
}

let newTargetDropdowns = null;

/** Creates the modal's table dropdowns on first use (they render their own markup into the hosts). */
function getNewTargetDropdowns() {
  if (!newTargetDropdowns) {
    const activityColumns = [
      { label: 'Activity', value: (a) => a.name },
      { label: 'Time account', value: (a) => timeAccountById(a.timeAccountId)?.name },
    ];
    newTargetDropdowns = {
      existingActivity: new TableDropdown('na-activity', activityColumns),
      newActivity: new TableDropdown('nn-activity', activityColumns),
      objective: new TableDropdown('na-objective', [
        { label: 'Name', value: (o) => o.name },
        { label: 'Description', value: (o) => o.description },
        { label: 'Category', value: (o) => cache.categories.find((c) => c.id === o.categoryId)?.name },
      ]),
    };
  }
  return newTargetDropdowns;
}

function openNewTrackingTargetModal() {
  const availableObjectives = cache.objectives.filter((o) => !o.isDone);

  if (!cache.activities.length) {
    toast('Add at least one Activity in Master Data first.', 'error');
    return;
  }

  const dropdowns = getNewTargetDropdowns();
  dropdowns.existingActivity.setRows(cache.activities);
  dropdowns.newActivity.setRows(cache.activities);
  dropdowns.objective.setRows(availableObjectives);
  document.getElementById('nn-category').innerHTML = cache.categories.map((c) => `<option value="${c.id}">${escapeHtml(c.name)}</option>`).join('');
  document.getElementById('na-planned').checked = false;
  document.getElementById('nn-planned').checked = false;
  document.getElementById('nn-name').value = '';
  document.getElementById('nn-description').value = '';

  setNewTargetTab(availableObjectives.length ? 'existing' : 'new');
    openModal('modal-new-tracking-target');
}

function setNewTargetTab(tab) {
  newTargetTab = tab;
  const modal = document.getElementById('modal-new-tracking-target');
  modal.querySelectorAll('.tab-btn').forEach((btn) => btn.classList.toggle('active', btn.dataset.ntTab === tab));
  modal.querySelectorAll('.tab-panel').forEach((panel) => panel.classList.toggle('active', panel.id === `nt-${tab}`));
}

async function submitNewTrackingTarget() {
  try {
    let activityId;
    let objectiveId;
    let isPlannedActivity;

    if (newTargetTab === 'new') {
      const name = document.getElementById('nn-name').value.trim();
      const description = document.getElementById('nn-description').value.trim();
      const categoryId = Number(document.getElementById('nn-category').value);

      if (!cache.categories.length) {
        toast('Add a category first.', 'error');
        return;
      }
      if (!name) {
        toast('Enter a name for the objective.', 'error');
        return;
      }

      activityId = Number(getNewTargetDropdowns().newActivity.value);
      isPlannedActivity = document.getElementById('nn-planned').checked;
      ({ id: objectiveId } = await Api.createObjective({ name, description, categoryId }));
      await refreshMasterDataCache();
    } else {
      const { existingActivity, objective } = getNewTargetDropdowns();

      if (objective.value === null) {
        toast('Select an objective, or create a new one.', 'error');
        return;
      }

      activityId = Number(existingActivity.value);
      objectiveId = Number(objective.value);
      isPlannedActivity = document.getElementById('na-planned').checked;
    }

    const { id } = await Api.createTrackingTarget({ activityId, objectiveId, isPlannedActivity });
    await Api.startTracking(id, nowAsTimeOnlyString());
    closeModals();
    await refreshTrackingTable();
    await renderStatusBar();
    toast('Activity started.');
  } catch (err) {
    toast(err.message, 'error');
  }
}
