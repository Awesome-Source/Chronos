/* Thin fetch wrapper. This is the only file that knows route strings and JSON shapes —
   every view module calls Api.* only, never fetch() directly. */

const API_BASE = '/api';

async function apiRequest(method, path, body) {
  const res = await fetch(API_BASE + path, {
    method,
    headers: body !== undefined ? { 'Content-Type': 'application/json' } : undefined,
    body: body !== undefined ? JSON.stringify(body) : undefined,
  });

  if (!res.ok) {
    let message = res.statusText;
    try {
      const problem = await res.json();
      if (problem && problem.title) {
        message = problem.title;
      }
    } catch {
      /* response had no JSON body */
    }
    throw new Error(message);
  }

  if (res.status === 204) {
    return undefined;
  }

  return res.json();
}

const Api = {
  // Time accounts
  getTimeAccounts: () => apiRequest('GET', '/timeaccounts'),
  createTimeAccount: (req) => apiRequest('POST', '/timeaccounts', req),
  updateTimeAccount: (id, req) => apiRequest('PUT', `/timeaccounts/${id}`, req),
  removeTimeAccount: (id) => apiRequest('DELETE', `/timeaccounts/${id}`),

  // Activities
  getActivities: () => apiRequest('GET', '/activities'),
  createActivity: (req) => apiRequest('POST', '/activities', req),
  updateActivity: (id, req) => apiRequest('PUT', `/activities/${id}`, req),
  removeActivity: (id) => apiRequest('DELETE', `/activities/${id}`),

  // Objectives
  getObjectives: () => apiRequest('GET', '/objectives'),
  createObjective: (req) => apiRequest('POST', '/objectives', req),
  updateObjective: (id, req) => apiRequest('PUT', `/objectives/${id}`, req),
  removeObjective: (id) => apiRequest('DELETE', `/objectives/${id}`),

  // Categories
  getCategories: () => apiRequest('GET', '/categories'),
  createCategory: (req) => apiRequest('POST', '/categories', req),
  updateCategory: (id, req) => apiRequest('PUT', `/categories/${id}`, req),
  removeCategory: (id) => apiRequest('DELETE', `/categories/${id}`),

  // Statistics
  getStatisticsBalances: () => apiRequest('GET', '/statistics/productive-time-account-balances'),

  // Tracking
  getTodaysTargets: () => apiRequest('GET', '/tracking/targets/today'),
  createTrackingTarget: (req) => apiRequest('POST', '/tracking/targets', req),
  startTracking: (targetId, start) => apiRequest('POST', `/tracking/targets/${targetId}/start`, { start }),
  stopTracking: (end) => apiRequest('POST', '/tracking/stop', { end }),
  getRecordsForDay: (dateStr) => apiRequest('GET', `/tracking/records?date=${dateStr}`),
  updateRecord: (id, start, end) => apiRequest('PUT', `/tracking/records/${id}`, { start, end }),
  getTimeSheetForDay: (dateStr) => apiRequest('GET', `/tracking/timesheet?date=${dateStr}`),
  getLatestDayBefore: async (dateStr) => {
    const res = await fetch(`${API_BASE}/tracking/latest-day-before?date=${dateStr}`);
    if (res.status === 404) {
      return null;
    }
    if (!res.ok) {
      throw new Error(res.statusText);
    }
    return res.json();
  },
};
