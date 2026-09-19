const STATISTICS_PLACEHOLDER_ITEMS = [
  'Percentage per Time Account (ever)',
  'Percentage per Time Account (last year)',
  'Percentage per Time Account (last week)',
  'Percentage per Time Account (today)',
  'Context switches per day (last week)',
  'Context switches per day (today)',
  'Percentage planned activity vs unplanned',
];

function renderStatistics() {
  const page = document.getElementById('page-statistics');
  page.innerHTML = `
    <div class="page-head">
      <div><h1>Statistics</h1><span class="sub">Planned reports &mdash; not yet available</span></div>
    </div>
    <div class="panel">
      <div class="panel-title">Planned reports <span class="badge badge-unplanned">Coming soon</span></div>
      <div class="recent-list">
        ${STATISTICS_PLACEHOLDER_ITEMS.map((item) => `
          <div class="recent-row">
            <span class="dot" style="background:var(--border-strong)"></span>
            <span class="title">${escapeHtml(item)}</span>
          </div>
        `).join('')}
      </div>
    </div>
  `;
}
