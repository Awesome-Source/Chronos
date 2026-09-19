/* Records: view/edit raw start-end tracking records for a chosen day.
   Add/Remove are not implemented here, mirroring Chronos.WinForms (those actions
   are stubbed "Not implemented yet." there and Chronos.Core exposes no delete). */

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
        <thead><tr><th>Start</th><th>End</th><th class="num">Duration</th><th>Activity</th><th>Objective</th><th>Active</th></tr></thead>
        <tbody id="records-body"><tr><td class="empty-state" colspan="6">Loading…</td></tr></tbody>
      </table>
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

function renderRecordsRows(records) {
  const body = document.getElementById('records-body');
  if (!body) return;

  if (!records.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="6">No records for this day.</td></tr>`;
    return;
  }

  body.innerHTML = records.map((r) => `
    <tr class="${r.isActive ? 'row-active' : ''}">
      <td><input type="time" class="mono" value="${timeOnlyStringToTimeInput(r.start)}" ${r.isActive ? 'disabled' : `onchange="updateRecordTime(${r.id})" data-record="${r.id}" data-field="start"`}></td>
      <td>${r.isActive ? '<span class="mono">--:--</span>' : `<input type="time" class="mono" value="${timeOnlyStringToTimeInput(r.end)}" onchange="updateRecordTime(${r.id})" data-record="${r.id}" data-field="end">`}</td>
      <td class="num mono">${r.isActive ? '&mdash;' : formatDuration(r.duration)}</td>
      <td>${escapeHtml(r.activityName)}</td>
      <td>${escapeHtml(r.objectiveName)}</td>
      <td>${r.isActive ? '<span class="pulse-dot"></span>' : ''}</td>
    </tr>
  `).join('');
}

function changeRecordsDate(value) {
  recordsSelectedDate = inputValueToDate(value);
  refreshRecordsTable();
}

async function updateRecordTime(recordId) {
  const startInput = document.querySelector(`input[data-record="${recordId}"][data-field="start"]`);
  const endInput = document.querySelector(`input[data-record="${recordId}"][data-field="end"]`);
  if (!startInput || !endInput) return;

  try {
    await Api.updateRecord(recordId, timeInputToTimeOnlyString(startInput.value), timeInputToTimeOnlyString(endInput.value));
    toast('Record updated.');
  } catch (err) {
    toast(err.message, 'error');
  }

  await refreshRecordsTable();
}
