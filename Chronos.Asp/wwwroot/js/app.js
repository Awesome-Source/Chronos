/* Orchestrator: navigation, modal/toast helpers, master-data cache refresh, startup, and
   the 10-second poll that mirrors Chronos.WinForms' TrackingPage timer. */

const PAGES = [
  { key: 'dashboard', label: 'Dashboard', icon: 'dashboard' },
  { key: 'tracking', label: 'Tracking', icon: 'tracking' },
  { key: 'records', label: 'Records', icon: 'records' },
  { key: 'timesheet', label: 'Time Sheet', icon: 'timesheet' },
  { key: 'statistics', label: 'Statistics', icon: 'statistics' },
  { key: 'masterdata', label: 'Master Data', icon: 'masterdata' },
];

const PAGE_RENDERERS = {
  dashboard: renderDashboard,
  tracking: renderTracking,
  records: renderRecords,
  timesheet: renderTimesheet,
  statistics: renderStatistics,
  masterdata: renderMasterData,
};

function renderNav() {
  const nav = document.getElementById('nav');
  nav.innerHTML = PAGES.map((p) => `
    <div class="nav-item" data-page="${p.key}" onclick="showPage('${p.key}')">
      ${ICONS[p.icon] || ''}<span>${p.label}</span>
    </div>
  `).join('');
}

async function showPage(key) {
  currentPage = key;

  document.querySelectorAll('.nav-item').forEach((el) => el.classList.toggle('active', el.dataset.page === key));
  document.querySelectorAll('.page').forEach((el) => el.classList.toggle('active', el.id === `page-${key}`));

  const renderer = PAGE_RENDERERS[key];
  if (renderer) {
    await renderer();
  }
}

/* ---------- modal helpers ---------- */

function openModal(id) {
  document.getElementById('modal-backdrop').classList.add('open');
  document.getElementById(id).classList.add('open');
}

function closeModals() {
  document.getElementById('modal-backdrop').classList.remove('open');
  document.querySelectorAll('.modal.open').forEach((m) => m.classList.remove('open'));
}

document.addEventListener('DOMContentLoaded', () => {
  document.getElementById('modal-backdrop').addEventListener('click', closeModals);
});

document.addEventListener('keydown', (e) => {
  if (e.key === 'Escape') closeModals();
});

/* ---------- toasts ---------- */

function toast(message, type) {
  const container = document.getElementById('toast-container');
  const el = document.createElement('div');
  el.className = 'toast' + (type === 'error' ? ' error' : '');
  el.textContent = message;
  container.appendChild(el);
  setTimeout(() => el.remove(), 3200);
}

/* ---------- master data cache ---------- */

async function refreshMasterDataCache() {
  const [timeAccounts, activities, objectives, categories] = await Promise.all([
    Api.getTimeAccounts(),
    Api.getActivities(),
    Api.getObjectives(),
    Api.getCategories(),
  ]);
  cache.timeAccounts = timeAccounts;
  cache.activities = activities;
  cache.objectives = objectives;
  cache.categories = categories;
}

/* ---------- statusbar + poll ---------- */

async function renderStatusBar() {
  const el = document.getElementById('statusbar');
  try {
    const targets = await Api.getTodaysTargets();
    const active = targets.find((t) => t.isActive);

    if (active) {
      el.innerHTML = `
        <div class="sb-left"><span class="pulse-dot"></span> Recording <strong>${escapeHtml(active.activityName)}</strong> - ${escapeHtml(active.objectiveName)}</div>
        <div class="sb-right mono">${formatDuration(active.accumulatedTime)}</div>
      `;
    } else {
      el.innerHTML = `<div class="sb-left">Not tracking</div><div class="sb-right"></div>`;
    }
  } catch {
    el.innerHTML = `<div class="sb-left">Not tracking</div><div class="sb-right"></div>`;
  }
}

async function pollActiveTracking() {
  await renderStatusBar();
  if (currentPage === 'tracking') {
    await refreshTrackingTable();
  }
  if (currentPage === 'dashboard') {
    await renderDashboard();
  }
}

/* ---------- startup ---------- */

async function init() {
  renderNav();

  try {
    await refreshMasterDataCache();
  } catch (err) {
    toast(err.message, 'error');
  }

  await renderStatusBar();
  await showPage('dashboard');

  setInterval(pollActiveTracking, 10000);
}

document.addEventListener('DOMContentLoaded', init);
