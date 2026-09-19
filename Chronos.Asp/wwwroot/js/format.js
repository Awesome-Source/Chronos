/* Pure date/time/text formatting helpers with no dependency on app state. */

const WEEKDAYS = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
const TODAY = startOfDay(new Date());
const YESTERDAY = addDays(TODAY, -1);

function pad2(n) {
  return String(n).padStart(2, '0');
}

function addDays(date, days) {
  const d = new Date(date);
  d.setDate(d.getDate() + days);
  return d;
}

function startOfDay(date) {
  const d = new Date(date);
  d.setHours(0, 0, 0, 0);
  return d;
}

function sameYmd(a, b) {
  return a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth() && a.getDate() === b.getDate();
}

/** Date -> "yyyy-MM-dd" for <input type="date"> and API query strings. */
function dateToInputValue(date) {
  return `${date.getFullYear()}-${pad2(date.getMonth() + 1)}-${pad2(date.getDate())}`;
}

/** "yyyy-MM-dd" -> Date (local midnight). */
function inputValueToDate(value) {
  const [y, m, d] = value.split('-').map(Number);
  return new Date(y, m - 1, d);
}

/** <input type="time" step="1"> gives "HH:MM:SS" ("HH:MM" when the seconds are 0); the API's TimeOnly JSON wants "HH:MM:SS". */
function timeInputToTimeOnlyString(value) {
  return value.length === 5 ? value + ':00' : value;
}

/** API's TimeOnly JSON ("HH:MM:SS") -> value usable in <input type="time" step="1">. */
function timeOnlyStringToTimeInput(hhmmss) {
  return hhmmss ? hhmmss.slice(0, 8) : '00:00:00';
}

function nowAsTimeOnlyString() {
  const now = new Date();
  return `${pad2(now.getHours())}:${pad2(now.getMinutes())}:${pad2(now.getSeconds())}`;
}

/** Seconds (number) -> "Xh Ym" / "Ym" / "0m" style short duration. */
function formatDuration(totalSeconds) {
  const total = Math.max(0, Math.round(totalSeconds || 0));
  const h = Math.floor(total / 3600);
  const m = Math.floor((total % 3600) / 60);
  const s = total % 60;
  if (h > 0) {
      return `${h}h ${m}m ${s}s`;
  }
  return `${m}m ${s}s`;
}

function dayLabelFor(date) {
  if (sameYmd(date, TODAY)) return 'Today';
  if (sameYmd(date, YESTERDAY)) return 'Yesterday';
  return `${WEEKDAYS[date.getDay()]}, ${date.toLocaleDateString(undefined, { month: 'short', day: 'numeric' })}`;
}

function escapeHtml(value) {
  return String(value ?? '').replace(/[&<>"']/g, (c) => ({
    '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;',
  }[c]));
}
