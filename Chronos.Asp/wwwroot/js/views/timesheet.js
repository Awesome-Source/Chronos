/* Time Sheet: daily summary/report for a completed (past) day.
   Defaults to the most recent day with tracking data before today; only
   reports on past days. */

async function renderTimesheet() {
  const page = document.getElementById('page-timesheet');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Time Sheet</h1><span class="sub" id="timesheet-sub"></span></div>
      <div class="page-head-actions">
        <input type="date" id="timesheet-date" onchange="changeTimesheetDate(this.value)">
      </div>
    </div>
    <div class="sheet-content" id="timesheet-content"><div class="panel"><p class="card-sub">Loading…</p></div></div>
  `;

  if (!timesheetSelectedDate) {
    try {
      const latest = await Api.getLatestDayBefore(dateToInputValue(TODAY));
      timesheetSelectedDate = latest ? inputValueToDate(latest.date) : YESTERDAY;
    } catch {
      timesheetSelectedDate = YESTERDAY;
    }
  }

  document.getElementById('timesheet-date').value = dateToInputValue(timesheetSelectedDate);
  await refreshTimesheet();
}

function changeTimesheetDate(value) {
  timesheetSelectedDate = inputValueToDate(value);
  refreshTimesheet();
}

async function refreshTimesheet() {
  const sub = document.getElementById('timesheet-sub');
  const content = document.getElementById('timesheet-content');
  if (sub) sub.textContent = dayLabelFor(timesheetSelectedDate);

  if (sameYmd(timesheetSelectedDate, TODAY)) {
    content.innerHTML = `<div class="panel"><p class="card-sub">The time sheet only reports on completed days. Pick a past date.</p></div>`;
    return;
  }

  try {
    const targets = await Api.getTimeSheetForDay(dateToInputValue(timesheetSelectedDate));
    renderTimesheetContent(targets);
  } catch (err) {
    toast(err.message, 'error');
  }
}

function renderTimesheetContent(targets) {
  const content = document.getElementById('timesheet-content');
  if (!content) return;

  if (!targets.length) {
    content.innerHTML = `<div class="panel"><p class="card-sub">No tracked time on this day.</p></div>`;
    return;
  }

  const totalSeconds = targets.reduce((s, t) => s + t.accumulatedTime, 0);
  const plannedSeconds = targets.filter((t) => t.isPlannedActivity).reduce((s, t) => s + t.accumulatedTime, 0);
  const unplannedSeconds = totalSeconds - plannedSeconds;

  const byTimeAccount = groupSum(targets, (t) => t.timeAccountName, (t) => t.accumulatedTime);
  const byActivity = groupSum(targets, (t) => t.activityName, (t) => t.accumulatedTime);
  const byObjective = groupSum(targets, (t) => t.objectiveName, (t) => t.accumulatedTime);

  content.innerHTML = `
    <div class="sheet-layout">
      <div class="panel panel-fill">
        <div class="panel-title">Full time sheet</div>
        <div class="panel-scroll">
          <table class="data-table">
            <thead><tr><th>Activity</th><th>Objective</th><th>Time account</th><th>Planned</th><th class="num">Duration</th></tr></thead>
            <tbody>
              ${targets.map((t) => `
                <tr>
                  <td>${escapeHtml(t.activityName)}</td>
                  <td>${escapeHtml(t.objectiveName)}</td>
                  <td>${escapeHtml(t.timeAccountName)}</td>
                  <td>${t.isPlannedActivity ? '<span class="badge badge-planned">Planned</span>' : '<span class="badge badge-unplanned">Unplanned</span>'}</td>
                  <td class="num mono">${formatDuration(t.accumulatedTime)}</td>
                </tr>
              `).join('')}
            </tbody>
          </table>
        </div>
      </div>
      <div class="sheet-summary">
        <div class="panel panel-fill">
          <div class="panel-title">Day statistics</div>
          <div class="panel-scroll">
            <table class="data-table">
              <tbody>
                <tr><td>Total</td><td class="num mono">${formatDuration(totalSeconds)}</td></tr>
                <tr><td>Planned</td><td class="num mono">${formatDuration(plannedSeconds)}</td></tr>
                <tr><td>Unplanned</td><td class="num mono">${formatDuration(unplannedSeconds)}</td></tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="panel panel-fill">
          <div class="panel-title">By time account</div>
          <div class="panel-scroll">${summaryTable(byTimeAccount)}</div>
        </div>
        <div class="panel panel-fill">
          <div class="panel-title">By activity</div>
          <div class="panel-scroll">${summaryTable(byActivity)}</div>
        </div>
        <div class="panel panel-fill">
          <div class="panel-title">By objective</div>
          <div class="panel-scroll">${summaryTable(byObjective)}</div>
        </div>
      </div>
    </div>
  `;
}

function groupSum(items, keyFn, valueFn) {
  const map = new Map();
  for (const item of items) {
    const key = keyFn(item);
    map.set(key, (map.get(key) || 0) + valueFn(item));
  }
  return [...map.entries()].sort((a, b) => b[1] - a[1]);
}

function summaryTable(entries) {
  return `
    <table class="data-table">
      <tbody>
        ${entries.map(([name, seconds]) => `
          <tr><td>${escapeHtml(name)}</td><td class="num mono">${formatDuration(seconds)}</td></tr>
        `).join('')}
      </tbody>
    </table>
  `;
}
