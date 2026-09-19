async function renderDashboard() {
  const page = document.getElementById('page-dashboard');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Dashboard</h1><span class="sub">Accumulated durations per productive Time Account (all time)</span></div>
    </div>
    <div class="grid-dash">
      <div class="panel">
        <div class="panel-title">Productive time by account</div>
        <table class="data-table">
          <thead><tr><th>Time account</th><th class="num">Duration</th><th class="num">Share</th></tr></thead>
          <tbody id="dashboard-balances-body"><tr><td class="empty-state" colspan="3">Loading…</td></tr></tbody>
        </table>
      </div>
      <div class="card" id="dashboard-current"></div>
    </div>
  `;

  try {
    const [balances, targets] = await Promise.all([Api.getStatisticsBalances(), Api.getTodaysTargets()]);
    renderDashboardBalances(balances);
    renderDashboardCurrent(targets);
  } catch (err) {
    toast(err.message, 'error');
  }
}

function renderDashboardBalances(balances) {
  const body = document.getElementById('dashboard-balances-body');
  if (!body) return;

  if (!balances.length) {
    body.innerHTML = `<tr><td class="empty-state" colspan="3">No tracked time yet.</td></tr>`;
    return;
  }

  body.innerHTML = balances.map((b) => `
    <tr>
      <td>${escapeHtml(b.timeAccountName)}</td>
      <td class="num mono">${formatDuration(b.accumulatedDuration)}</td>
      <td class="num mono">${(b.proportion * 100).toFixed(1)}%</td>
    </tr>
  `).join('');
}

function renderDashboardCurrent(targets) {
  const el = document.getElementById('dashboard-current');
  if (!el) return;

  const active = targets.find((t) => t.isActive);
  if (active) {
    el.innerHTML = `
      <div class="card-label">Currently tracking</div>
      <div class="card-value accent mono">${formatDuration(active.accumulatedTime)}</div>
      <div class="card-sub">${escapeHtml(active.activityName)} &middot; ${escapeHtml(active.objectiveName)}</div>
    `;
  } else {
    el.innerHTML = `
      <div class="card-label">Currently tracking</div>
      <div class="card-value mono">--</div>
      <div class="card-sub">Not tracking right now</div>
    `;
  }
}
