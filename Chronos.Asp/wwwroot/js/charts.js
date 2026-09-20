/* Dependency-free SVG chart builders. Each function returns an HTML string; colors that are not
   data-driven come from CSS variables so the charts follow the theme. */

const CHART_FALLBACK_COLORS = ['#379468', '#dc9c46', '#5b8fd6', '#b8503f', '#8a6fc4', '#3aa6a6', '#c76fa0', '#9aa84a'];

/** Time account color (from master data) with a stable fallback for accounts without a usable one. */
function chartColorForAccount(timeAccountId) {
  const account = timeAccountById(timeAccountId);
  return (account && account.color) || CHART_FALLBACK_COLORS[timeAccountId % CHART_FALLBACK_COLORS.length];
}

/** slices: [{ label, value (seconds), share (0..1), color }] */
function donutChartHtml(slices) {
  const size = 200;
  const center = size / 2;
  const radius = 70;
  const strokeWidth = 28;
  const circumference = 2 * Math.PI * radius;
  const total = slices.reduce((sum, s) => sum + s.value, 0);

  let offset = 0;
  const arcs = total > 0
    ? slices.filter((s) => s.value > 0).map((s) => {
      const length = (s.value / total) * circumference;
      const arc = `
        <circle cx="${center}" cy="${center}" r="${radius}" fill="none" stroke="${escapeHtml(s.color)}" stroke-width="${strokeWidth}"
          stroke-dasharray="${length} ${circumference - length}" stroke-dashoffset="${-offset}" transform="rotate(-90 ${center} ${center})">
          <title>${escapeHtml(s.label)}: ${formatDuration(s.value)} (${(s.share * 100).toFixed(1)}%)</title>
        </circle>`;
      offset += length;
      return arc;
    }).join('')
    : '';

  const centerText = total > 0
    ? `<text class="chart-donut-value" x="${center}" y="${center + 2}" text-anchor="middle">${formatDuration(total)}</text>
       <text class="chart-axis-text" x="${center}" y="${center + 20}" text-anchor="middle">total</text>`
    : `<text class="chart-axis-text" x="${center}" y="${center + 4}" text-anchor="middle">No tracked time</text>`;

  return `
    <svg class="chart-donut" viewBox="0 0 ${size} ${size}" role="img" aria-label="Productive time by account">
      <circle cx="${center}" cy="${center}" r="${radius}" fill="none" stroke="var(--surface-2)" stroke-width="${strokeWidth}"></circle>
      ${arcs}
      ${centerText}
    </svg>`;
}

/** Picks an axis maximum and step (in hours) that gives at most ~6 gridlines. */
function niceHourAxis(maxHours) {
  const steps = [0.25, 0.5, 1, 2, 3, 4, 5, 10, 20, 50];
  const step = steps.find((s) => Math.ceil(maxHours / s) <= 6) || 100;
  return { step, max: step * Math.max(2, Math.ceil(maxHours / step)) };
}

/** days: [{ date: "yyyy-MM-dd", totalWorkTime (seconds), accounts: [{ timeAccountId, timeAccountName, isWorkTime, duration (seconds) }] }]
    One stacked bar per day (all accounts), plus a marker and label for that day's total work time. */
function stackedBarChartHtml(days) {
  const width = 720;
  const height = 300;
  const margin = { top: 30, right: 16, bottom: 34, left: 52 };
  const plotWidth = width - margin.left - margin.right;
  const plotHeight = height - margin.top - margin.bottom;

  const dayTotals = days.map((d) => d.accounts.reduce((sum, a) => sum + a.duration, 0));
  const maxHours = Math.max(0, ...dayTotals, ...days.map((d) => d.totalWorkTime)) / 3600;
  const axis = niceHourAxis(maxHours);
  const yFor = (seconds) => margin.top + plotHeight - (seconds / 3600 / axis.max) * plotHeight;

  // Stack order is the same on every day: accounts with the most time this week at the bottom.
  const weekTotals = new Map();
  days.forEach((d) => d.accounts.forEach((a) => {
    const entry = weekTotals.get(a.timeAccountId) || { id: a.timeAccountId, name: a.timeAccountName, seconds: 0 };
    entry.seconds += a.duration;
    weekTotals.set(a.timeAccountId, entry);
  }));
  const accountOrder = [...weekTotals.values()].sort((a, b) => b.seconds - a.seconds);
  const orderIndex = new Map(accountOrder.map((a, i) => [a.id, i]));

  const gridlines = [];
  for (let value = 0; value <= axis.max + 1e-9; value += axis.step) {
    const y = yFor(value * 3600);
    gridlines.push(`
      <line class="chart-grid" x1="${margin.left}" x2="${width - margin.right}" y1="${y}" y2="${y}"></line>
      <text class="chart-axis-text" x="${margin.left - 8}" y="${y + 4}" text-anchor="end">${+value.toFixed(2)}h</text>`);
  }

  const slot = plotWidth / days.length;
  const barWidth = Math.min(64, slot * 0.6);

  const columns = days.map((day, i) => {
    const cx = margin.left + slot * i + slot / 2;
    const x = cx - barWidth / 2;
    const date = inputValueToDate(day.date);

    let stacked = 0;
    const segments = [...day.accounts]
      .sort((a, b) => orderIndex.get(a.timeAccountId) - orderIndex.get(b.timeAccountId))
      .filter((a) => a.duration > 0)
      .map((a) => {
        const top = yFor(stacked + a.duration);
        const bottom = yFor(stacked);
        stacked += a.duration;
        return `
          <rect x="${x}" y="${top}" width="${barWidth}" height="${Math.max(0, bottom - top)}" fill="${escapeHtml(chartColorForAccount(a.timeAccountId))}">
            <title>${escapeHtml(a.timeAccountName)}${a.isWorkTime ? '' : ' (non-work)'}: ${formatDuration(a.duration)}</title>
          </rect>`;
      }).join('');

    const workMarker = day.totalWorkTime > 0
      ? `<line class="chart-work-marker-outline" x1="${cx - barWidth / 2 - 6}" x2="${cx + barWidth / 2 + 6}" y1="${yFor(day.totalWorkTime)}" y2="${yFor(day.totalWorkTime)}"></line>
         <line class="chart-work-marker" x1="${cx - barWidth / 2 - 6}" x2="${cx + barWidth / 2 + 6}" y1="${yFor(day.totalWorkTime)}" y2="${yFor(day.totalWorkTime)}">
           <title>Total work time: ${formatDuration(day.totalWorkTime)}</title>
         </line>`
      : '';

    return `
      ${segments}
      ${workMarker}
      <text class="chart-work-label" x="${cx}" y="${yFor(dayTotals[i]) - 8}" text-anchor="middle">${formatDuration(day.totalWorkTime)}</text>
      <text class="chart-axis-text" x="${cx}" y="${height - margin.bottom + 18}" text-anchor="middle">${WEEKDAYS[date.getDay()]} ${date.getDate()}</text>`;
  }).join('');

  const emptyNote = dayTotals.every((t) => t === 0)
    ? `<text class="chart-axis-text" x="${margin.left + plotWidth / 2}" y="${margin.top + plotHeight / 2}" text-anchor="middle">No tracked time this week yet</text>`
    : '';

  const legendItems = accountOrder.map((a) => `
    <span class="legend-item"><span class="legend-dot" style="background:${escapeHtml(chartColorForAccount(a.id))}"></span>${escapeHtml(a.name)}</span>`).join('');

  return `
    <svg class="chart-bars" viewBox="0 0 ${width} ${height}" role="img" aria-label="Time by day and account, current week">
      ${gridlines.join('')}
      ${columns}
      ${emptyNote}
    </svg>
    <div class="legend">
      ${legendItems}
      <span class="legend-item"><span class="legend-line"></span>Total work time</span>
    </div>`;
}
