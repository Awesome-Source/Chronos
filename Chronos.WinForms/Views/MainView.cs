using Chronos.Core;
using Chronos.Views.TabPages;
using Chronos.WinForms.Constants;
using Chronos.WinForms.Views.Common;

namespace Chronos
{
    public partial class MainView : Form
    {
        private readonly PageSwitcher<MainPageType> _pageSwitcher;
        private readonly ChronosCore _chronosCore;

        public MainView(ChronosCore chronosCore, DashboardPage dashboardPage, TrackingPage trackingPage, RecordsPage recordsPage, TimeSheetPage timeSheetPage, MasterDataPage masterDataPage, StatisticsPage statisticsPage)
        {
            InitializeComponent();
            _pageSwitcher = new PageSwitcher<MainPageType>(_panelMainContent, MoveHighlightPanelToButton);

            _pageSwitcher.RegisterPage(MainPageType.Dashboard, dashboardPage, _buttonDashboard);
            _pageSwitcher.RegisterPage(MainPageType.Tracking, trackingPage, _buttonTracking);
            _pageSwitcher.RegisterPage(MainPageType.Records, recordsPage, _buttonRecords);
            _pageSwitcher.RegisterPage(MainPageType.TimeSheet, timeSheetPage, _buttonTimeSheet);
            _pageSwitcher.RegisterPage(MainPageType.MasterData, masterDataPage, _buttonMasterData);
            _pageSwitcher.RegisterPage(MainPageType.Statistics, statisticsPage, _buttonStatistics);
            _pageSwitcher.SetActivePage(MainPageType.Dashboard);
            _chronosCore = chronosCore;
        }

        private void MoveHighlightPanelToButton(Button button)
        {
            _panelTabHighlight.Location = new Point(button.Location.X - _panelTabHighlight.Size.Width, button.Location.Y);
        }

        private void ButtonDashboard_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MainPageType.Dashboard);
        }

        private void ButtonTracking_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MainPageType.Tracking);
        }

        private void ButtonRecords_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MainPageType.Records);
        }

        private void ButtonTimeSheet_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MainPageType.TimeSheet);
        }

        private void ButtonStatistics_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MainPageType.Statistics);
        }

        private void ButtonMasterData_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MainPageType.MasterData);
        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            _chronosCore.TrackingService.StopTracking(TimeOnly.FromDateTime(DateTime.Now));
        }
    }
}
