/* Client-side UI state. Volatile tracking/records/timesheet data is always re-fetched
   fresh from the API on each page-show instead of being cached here. */

let currentPage = 'dashboard';
let selectedTargetId = null;
let recordsSelectedDate = TODAY;
let timesheetSelectedDate = null;
let masterTab = 'accounts';
let newTargetTab = 'existing';
let newRecordTab = 'existing';

/** Shared lookup cache for master data, populated at startup and refreshed after any mutation. */
const cache = {
  timeAccounts: [],
  activities: [],
  objectives: [],
  categories: [],
};

/** Per-table sort/filter state for the Master Data tables (frontend-only, survives tab switches).
    sort: { key, dir: 'asc' | 'desc' } | null.
    filters[key]: string for text columns, true/false for checkbox columns; absent = no filter. */
const masterView = {
  accounts: { sort: null, filters: {} },
  activities: { sort: null, filters: {} },
  objectives: { sort: null, filters: {} },
  categories: { sort: null, filters: {} },
};

function timeAccountById(id) {
  return cache.timeAccounts.find((a) => a.id === id);
}
