/* Client-side UI state. Volatile tracking/records/timesheet data is always re-fetched
   fresh from the API on each page-show instead of being cached here. */

let currentPage = 'dashboard';
let selectedTargetId = null;
let recordsSelectedDate = TODAY;
let timesheetSelectedDate = null;
let masterTab = 'accounts';

/** Shared lookup cache for master data, populated at startup and refreshed after any mutation. */
const cache = {
  timeAccounts: [],
  activities: [],
  objectives: [],
  categories: [],
};

function timeAccountById(id) {
  return cache.timeAccounts.find((a) => a.id === id);
}
