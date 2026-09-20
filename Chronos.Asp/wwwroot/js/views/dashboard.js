/** The week's data behind the stacked bar chart; kept so the chart can be redrawn when its container is resized. */
let dashboardWeekDays = null;

function productivePanelHtml(idPrefix, title) {
  return `
    <div class="panel panel-fill">
      <div class="panel-title">${title}</div>
      <div class="donut-wrap" id="${idPrefix}-donut"></div>
      <div class="panel-scroll">
        <table class="data-table">
          <thead><tr><th>Time account</th><th class="num">Duration</th><th class="num">Share</th></tr></thead>
          <tbody id="${idPrefix}-body"><tr><td class="empty-state" colspan="3">Loading…</td></tr></tbody>
        </table>
      </div>
    </div>
  `;
}

/** Builds the static page skeleton once; later calls keep it (and the data in it) and only refresh the data. */
async function renderDashboard() {
  const page = document.getElementById('page-dashboard');
  if (!page.firstElementChild) {
    page.innerHTML = `
      <div class="page-head">
        <div><h1>Dashboard</h1></div>
      </div>
      <div class="dash-layout">
        ${productivePanelHtml('dashboard-all-time', 'Productive time by account (all time)')}
        ${productivePanelHtml('dashboard-week', 'Productive time by account (current week)')}
        <div class="panel panel-fill panel-wide">
          <div class="panel-title">Time by day and account (current week)</div>
          <div class="chart-fill" id="dashboard-week-days"></div>
          <div class="legend" id="dashboard-week-days-legend"></div>
        </div>
      </div>
    `;

    // The chart is drawn at its container's pixel size, so redraw whenever that size changes (window resize, page shown).
    new ResizeObserver(drawDashboardWeekDays).observe(document.getElementById('dashboard-week-days'));
  }

  await refreshDashboard();
}

/** Fetches fresh dashboard data and patches it into the existing skeleton. */
async function refreshDashboard() {
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

  patchInnerHtml(donut, donutChartHtml(balances.map((b) => ({
    key: b.timeAccountId,
    label: b.timeAccountName,
    value: b.accumulatedDuration,
    share: b.proportion,
    color: chartColorForAccount(b.timeAccountId),
  }))));

  if (!balances.length || total === 0) {
    patchInnerHtml(body, `<tr><td class="empty-state" colspan="3">${emptyMessage}</td></tr>`);
    return;
  }

  patchInnerHtml(body, balances.map((b) => `
    <tr data-key="${b.timeAccountId}">
      <td><span class="legend-dot" style="background:${escapeHtml(chartColorForAccount(b.timeAccountId))}"></span> ${escapeHtml(b.timeAccountName)}</td>
      <td class="num mono">${formatDuration(b.accumulatedDuration)}</td>
      <td class="num mono">${(b.proportion * 100).toFixed(1)}%</td>
    </tr>
  `).join(''));
}

function renderDashboardWeekDays(days) {
  dashboardWeekDays = days;
  drawDashboardWeekDays();
}

function drawDashboardWeekDays() {
  const chart = document.getElementById('dashboard-week-days');
  const legend = document.getElementById('dashboard-week-days-legend');
  if (!chart || !legend || !dashboardWeekDays) return;

  const { clientWidth: width, clientHeight: height } = chart;
  if (!width || !height) return; // page hidden; the resize observer redraws once it has a size

  patchInnerHtml(chart, stackedBarChartHtml(dashboardWeekDays, width, height));
  patchInnerHtml(legend, stackedBarLegendHtml(dashboardWeekDays));
}
