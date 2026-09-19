/* Tracking: the core clock in/out screen. */

let trackingTargetsCache = [];

async function renderTracking() {
  const page = document.getElementById('page-tracking');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Tracking</h1><span class="sub">Today's activity targets</span></div>
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
        <button class="btn btn-outline" onclick="openNewActivityModal()">${ICONS.plus} New activity</button>
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
    body.innerHTML = `<tr><td class="empty-state" colspan="5">No activity targets for today yet. Use "New activity" to start one.</td></tr>`;
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

function openNewActivityModal() {
  const activitySelect = document.getElementById('na-activity');
  const objectiveSelect = document.getElementById('na-objective');

  if (!cache.activities.length || !cache.objectives.length) {
    toast('Add at least one Activity and Objective in Master Data first.', 'error');
    return;
  }

  activitySelect.innerHTML = cache.activities.map((a) => `<option value="${a.id}">${escapeHtml(a.name)}</option>`).join('');
  objectiveSelect.innerHTML = cache.objectives.map((o) => `<option value="${o.id}">${escapeHtml(o.name)}</option>`).join('');
  document.getElementById('na-planned').checked = false;
  openModal('modal-new-activity');
}

async function submitNewActivity() {
  const activityId = Number(document.getElementById('na-activity').value);
  const objectiveId = Number(document.getElementById('na-objective').value);
  const isPlannedActivity = document.getElementById('na-planned').checked;

  try {
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
