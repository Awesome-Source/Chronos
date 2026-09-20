function productivePanelHtml(idPrefix, title) {
  return `
    <div class="panel">
      <div class="panel-title">${title}</div>
      <div class="donut-wrap" id="${idPrefix}-donut"></div>
      <table class="data-table">
        <thead><tr><th>Time account</th><th class="num">Duration</th><th class="num">Share</th></tr></thead>
        <tbody id="${idPrefix}-body"><tr><td class="empty-state" colspan="3">Loading…</td></tr></tbody>
      </table>
    </div>
  `;
}

async function renderDashboard() {
  const page = document.getElementById('page-dashboard');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Dashboard</h1></div>
    </div>
    <div class="grid-2">
      ${productivePanelHtml('dashboard-all-time', 'Productive time by account (all time)')}
      ${productivePanelHtml('dashboard-week', 'Productive time by account (current week)')}
    </div>
    <div class="panel">
      <div class="panel-title">Time by day and account (current week)</div>
      <div id="dashboard-week-days"></div>
    </div>
  `;

  try {
    const [balances, weekBalances, weekDays] = await Promise.all([
      Api.getStatisticsBalances(),
      Api.getStatisticsBalancesCurrentWeek(),
      Api.getStatisticsDailyDurationsCurrentWeek(),
    ]);

    renderProductivePanel('dashboard-all-time', balances, 'No tracked time yet.');
    renderProductivePanel('dashboard-week', weekBalances, 'No tracked time this week yet.');
    renderDashboardWeekDays(weekDays);
  } catch (err) {
    toast(err.message, 'error');
  }
}

function renderProductivePanel(idPrefix, balances, emptyMessage) {
  const donut = document.getElementById(`${idPrefix}-donut`);
  const body = document.getElementById(`${idPrefix}-body`);
  if (!donut || !body) return;

  const total = balances.reduce((sum, b) => sum + b.accumulatedDuration, 0);

  donut.innerHTML = donutChartHtml(balances.map((b) => ({
    label: b.timeAccountName,
    value: b.accumulatedDuration,
    share: b.proportion,
    color: chartColorForAccount(b.timeAccountId),
  })));

  if (!balances.length || total === 0) {
    body.innerHTML = `<tr><td class="empty-state" colspan="3">${emptyMessage}</td></tr>`;
    return;
  }

  body.innerHTML = balances.map((b) => `
    <tr>
      <td><span class="legend-dot" style="background:${escapeHtml(chartColorForAccount(b.timeAccountId))}"></span> ${escapeHtml(b.timeAccountName)}</td>
      <td class="num mono">${formatDuration(b.accumulatedDuration)}</td>
      <td class="num mono">${(b.proportion * 100).toFixed(1)}%</td>
    </tr>
  `).join('');
}

function renderDashboardWeekDays(days) {
  const el = document.getElementById('dashboard-week-days');
  if (!el) return;

  el.innerHTML = stackedBarChartHtml(days);
}
