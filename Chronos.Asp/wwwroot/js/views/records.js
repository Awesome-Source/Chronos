/* Records: view/edit/add/remove raw start-end tracking records for a chosen day.
   Records are always shown in temporal order. Gaps and overlaps between consecutive records are
   marked as conflicts and can be fixed by aligning a record to its predecessor's end or its
   successor's start. */

const RECORD_COLUMN_COUNT = 10;

/** The records as last rendered (temporal order); the fix buttons refer to rows by index into this list. */
let displayedRecords = [];

async function renderRecords() {
  const page = document.getElementById('page-records');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Records</h1><span class="sub" id="records-sub"></span></div>
      <div class="page-head-actions">
        <input type="date" id="records-date" value="${dateToInputValue(recordsSelectedDate)}" onchange="changeRecordsDate(this.value)">
      </div>
    </div>
    <div class="panel">
      <table class="data-table">
        <thead><tr>
          <th>Start</th><th>End</th><th class="num">Duration</th><th>Activity</th><th>Objective</th><th>Active</th>
          <th>Status</th>
          <th title="Align start of current record to end of previous one">Fix start</th>
          <th title="Align end of current record to start of next one">Fix end</th>
          <th></th>
        </tr></thead>
        <tbody id="records-body"><tr><td class="empty-state" colspan="${RECORD_COLUMN_COUNT}">Loading…</td></tr></tbody>
      </table>
      <div class="toolbar">
        <button class="btn btn-outline" onclick="openNewRecordModal()">${ICONS.plus} Add record</button>
      </div>
    </div>
  `;
  await refreshRecordsTable();
}

async function refreshRecordsTable() {
  const sub = document.getElementById('records-sub');
  if (sub) sub.textContent = dayLabelFor(recordsSelectedDate);

  try {
    const records = await Api.getRecordsForDay(dateToInputValue(recordsSelectedDate));
    renderRecordsRows(records);
  } catch (err) {
    toast(err.message, 'error');
  }
}

function compareRecords(a, b) {
  return a.start.localeCompare(b.start) || a.end.localeCompare(b.end) || a.id - b.id;
}

/** Kind of conflict at the junction of two temporally consecutive records: 'gap', 'overlap' or null. */
function junctionConflict(earlier, later) {
  // A running record has no end yet, so anything starting after its start overlaps it.
  if (earlier.isActive) return 'overlap';
  if (later.start > earlier.end) return 'gap';
  if (later.start < earlier.end) return 'overlap';
  return null;
}

/** For each record of the (sorted) list: the conflict with its predecessor (`before`) and successor (`after`). */
function computeRecordConflicts(sorted) {
  const junctions = sorted.slice(1).map((later, i) => junctionConflict(sorted[i], later));
  return sorted.map((_, i) => ({
    before: i > 0 ? junctions[i - 1] : null,
    after: i < junctions.length ? junctions[i] : null,
  }));
}

function conflictBadges(conflict) {
  const labels = [];
  if (conflict.before) labels.push(`${conflict.before === 'gap' ? 'Gap' : 'Overlap'} before`);
  if (conflict.after) labels.push(`${conflict.after === 'gap' ? 'Gap' : 'Overlap'} after`);
  return labels.map((label) => `<span class="badge badge-conflict">${label}</span>`).join(' ');
}

function fixButton(icon, title, handler, disabledReason) {
  return disabledReason
    ? `<button class="row-icon-btn" disabled title="${disabledReason}">${icon}</button>`
    : `<button class="row-icon-btn" title="${title}" onclick="${handler}">${icon}</button>`;
}

function fixStartButton(records, index) {
  const record = records[index];
  const previous = records[index - 1];

  let disabledReason = null;
  if (record.isActive) disabledReason = 'The running record cannot be changed.';
  else if (previous.isActive) disabledReason = 'The running record has no end yet.';
  else if (previous.end >= record.end) disabledReason = 'The record would have no duration left.';

  return fixButton(ICONS.alignStart, 'Align start of this record to end of previous one', `alignStartToPrevious(${index})`, disabledReason);
}

function fixEndButton(records, index) {
  const record = records[index];
  const next = records[index + 1];

  let disabledReason = null;
  if (record.isActive) disabledReason = 'The running record cannot be changed.';
  else if (next.start <= record.start) disabledReason = 'The record would have no duration left.';

  return fixButton(ICONS.alignEnd, 'Align end of this record to start of next one', `alignEndToNext(${index})`, disabledReason);
}

function renderRecordsRows(records) {
  const body = document.getElementById('records-body');
  if (!body) return;

  displayedRecords = [...records].sort(compareRecords);

  if (!displayedRecords.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="${RECORD_COLUMN_COUNT}">No records for this day.</td></tr>`;
    return;
  }

  const conflicts = computeRecordConflicts(displayedRecords);

  body.innerHTML = displayedRecords.map((r, i) => {
    const conflict = conflicts[i];
    const isConflicted = Boolean(conflict.before || conflict.after);
    return `
    <tr class="${r.isActive ? 'row-active' : ''} ${isConflicted ? 'row-conflict' : ''}">
      <td><input type="time" class="mono" value="${timeOnlyStringToTimeInput(r.start)}" ${r.isActive ? 'disabled' : `onchange="updateRecordTime(${r.id})" data-record="${r.id}" data-field="start"`}></td>
      <td>${r.isActive ? '<span class="mono">--:--</span>' : `<input type="time" class="mono" value="${timeOnlyStringToTimeInput(r.end)}" onchange="updateRecordTime(${r.id})" data-record="${r.id}" data-field="end">`}</td>
      <td class="num mono">${r.isActive ? '&mdash;' : formatDuration(r.duration)}</td>
      <td>${escapeHtml(r.activityName)}</td>
      <td>${escapeHtml(r.objectiveName)}</td>
      <td>${r.isActive ? '<span class="pulse-dot"></span>' : ''}</td>
      <td>${conflictBadges(conflict)}</td>
      <td>${conflict.before ? fixStartButton(displayedRecords, i) : ''}</td>
      <td>${conflict.after ? fixEndButton(displayedRecords, i) : ''}</td>
      <td>${r.isActive ? '' : `<button class="row-icon-btn danger" title="Remove record" onclick="deleteRecord(${r.id})">${ICONS.trash}</button>`}</td>
    </tr>
  `;
  }).join('');
}

function changeRecordsDate(value) {
  recordsSelectedDate = inputValueToDate(value);
  refreshRecordsTable();
}

async function saveRecordTimes(recordId, start, end) {
  try {
    await Api.updateRecord(recordId, start, end);
    toast('Record updated.');
  } catch (err) {
    toast(err.message, 'error');
  }

  await refreshRecordsTable();
}

async function updateRecordTime(recordId) {
  const startInput = document.querySelector(`input[data-record="${recordId}"][data-field="start"]`);
  const endInput = document.querySelector(`input[data-record="${recordId}"][data-field="end"]`);
  if (!startInput || !endInput) return;

  await saveRecordTimes(recordId, timeInputToTimeOnlyString(startInput.value), timeInputToTimeOnlyString(endInput.value));
}

/** Moves the start of the record at `index` to the end of its predecessor (closes a gap / resolves an overlap). */
async function alignStartToPrevious(index) {
  const record = displayedRecords[index];
  const previous = displayedRecords[index - 1];
  if (!record || !previous) return;

  await saveRecordTimes(record.id, previous.end, record.end);
}

/** Moves the end of the record at `index` to the start of its successor (closes a gap / resolves an overlap). */
async function alignEndToNext(index) {
  const record = displayedRecords[index];
  const next = displayedRecords[index + 1];
  if (!record || !next) return;

  await saveRecordTimes(record.id, record.start, next.start);
}

async function deleteRecord(recordId) {
  try {
    await Api.removeRecord(recordId);
    toast('Record removed.');
  } catch (err) {
    toast(err.message, 'error');
  }

  await refreshRecordsTable();
}

/* ---------- Add record dialog ---------- */

let newRecordDropdowns = null;

/** Creates the modal's table dropdowns on first use (they render their own markup into the hosts). */
function getNewRecordDropdowns() {
  if (!newRecordDropdowns) {
    newRecordDropdowns = {
      target: new TableDropdown('nr-target', [
        { label: 'Activity', value: (t) => t.activityName },
        { label: 'Objective', value: (t) => t.objectiveName },
        { label: 'Accumulated', value: (t) => formatDuration(t.accumulatedTime) },
      ]),
      activity: new TableDropdown('nr-activity', [
        { label: 'Activity', value: (a) => a.name },
        { label: 'Time account', value: (a) => timeAccountById(a.timeAccountId)?.name },
      ]),
      objective: new TableDropdown('nr-objective', [
        { label: 'Name', value: (o) => o.name },
        { label: 'Description', value: (o) => o.description },
        { label: 'Category', value: (o) => cache.categories.find((c) => c.id === o.categoryId)?.name },
      ]),
    };
  }
  return newRecordDropdowns;
}

/** "HH:MM" plus one hour, clamped to 23:59. */
function addHourToTimeInput(hhmm) {
  const [hours, minutes] = hhmm.split(':').map(Number);
  return hours >= 23 ? '23:59' : `${pad2(hours + 1)}:${pad2(minutes)}`;
}

/** Suggested start for a new record: the end of the latest completed record shown, else 08:00. */
function suggestedRecordStart() {
  const ends = displayedRecords.filter((r) => !r.isActive).map((r) => r.end);
  if (!ends.length) return '08:00';
  return timeOnlyStringToTimeInput(ends.reduce((latest, end) => (end > latest ? end : latest)));
}

async function openNewRecordModal() {
  let targets;
  try {
    const dayTargets = await Api.getTrackingTargetsForDay(dateToInputValue(recordsSelectedDate));
    targets = dayTargets.map((t) => ({ ...t, id: t.internalId }));
  } catch (err) {
    toast(err.message, 'error');
    return;
  }

  if (!targets.length && (!cache.activities.length || !cache.objectives.length)) {
    toast('Add at least one Activity and one Objective in Master Data first.', 'error');
    return;
  }

  const dropdowns = getNewRecordDropdowns();
  dropdowns.target.setRows(targets);
  dropdowns.activity.setRows(cache.activities);
  dropdowns.objective.setRows(cache.objectives);
  document.getElementById('nr-planned').checked = false;

  const start = suggestedRecordStart();
  document.getElementById('nr-start').value = start;
  document.getElementById('nr-end').value = addHourToTimeInput(start);

  setNewRecordTab(targets.length ? 'existing' : 'new');
  openModal('modal-new-record');
}

function setNewRecordTab(tab) {
  newRecordTab = tab;
  const modal = document.getElementById('modal-new-record');
  modal.querySelectorAll('.tab-btn').forEach((btn) => btn.classList.toggle('active', btn.dataset.nrTab === tab));
  modal.querySelectorAll('.tab-panel').forEach((panel) => panel.classList.toggle('active', panel.id === `nr-${tab}`));
}

async function submitNewRecord() {
  const startValue = document.getElementById('nr-start').value;
  const endValue = document.getElementById('nr-end').value;
  if (!startValue || !endValue) {
    toast('Enter a start and an end time.', 'error');
    return;
  }

  const start = timeInputToTimeOnlyString(startValue);
  const end = timeInputToTimeOnlyString(endValue);
  if (end <= start) {
    toast('End must be after start.', 'error');
    return;
  }

  try {
    const { target, activity, objective } = getNewRecordDropdowns();
    let trackingTargetId;

    if (newRecordTab === 'new') {
      if (activity.value === null || objective.value === null) {
        toast('Select an activity and an objective.', 'error');
        return;
      }

      ({ id: trackingTargetId } = await Api.createTrackingTarget({
        activityId: Number(activity.value),
        objectiveId: Number(objective.value),
        isPlannedActivity: document.getElementById('nr-planned').checked,
        date: dateToInputValue(recordsSelectedDate),
      }));
    } else {
      if (target.value === null) {
        toast('Select a tracking target, or create a new one.', 'error');
        return;
      }

      trackingTargetId = Number(target.value);
    }

    await Api.createRecord({ trackingTargetId, start, end });
    closeModals();
    toast('Record added.');
    await refreshRecordsTable();
  } catch (err) {
    toast(err.message, 'error');
  }
}
