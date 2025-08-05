using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms;
using Chronos.WinForms.DataObjects;

namespace Chronos.Views.TabPages
{
    public partial class DashboardPage : UserControl
    {
        private readonly ChronosCore _chronosCore;
        private BindingList<RelativeTimeAccountBalanceGridEntry> _timeAccountSummaryGridEntries = new();

        public DashboardPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;

            _gridTimeAccountStatistics.BindDataSource(_timeAccountSummaryGridEntries);
            _gridTimeAccountStatistics.GridView.CellFormatting += GridView_CellFormatting;

            //TODO statistics for: total logged time, accumulated time per time account / activity / objective last 5 days
        }

        private void GridView_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is TimeSpan timeSpan)
            {
                e.Value = $"{timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
            }

            if(e.Value is double percentage)
            {
                e.Value = $"{percentage:F2} %";
            }
        }

        private void RefreshView()
        {
            _timeAccountSummaryGridEntries.Clear();

            IReadOnlyList<RelativeTimeAccountBalance> timeAccountBalances;

            try
            {
                 timeAccountBalances = _chronosCore.StatisticsService.RetrieveAllTimeAccountBalances();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve time account balances.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _timeAccountSummaryGridEntries.AddRange(timeAccountBalances.Select(tab => new RelativeTimeAccountBalanceGridEntry(tab.TimeAccountName, tab.AccumulatedDuration, tab.Proportion * 100.0)));
        }

        private void DashboardPage_VisibleChanged(object sender, EventArgs e)
        {
            RefreshView();
        }
    }
}
