using Chronos.WinForms.Constants;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class MasterDataPage : UserControl
    {
        private readonly PageSwitcher<MasterDataPageType> _pageSwitcher;

        public MasterDataPage(TimeAccountPage timeAccountPage, ActivityPage activityPage, ObjectivePage objectivePage)
        {
            InitializeComponent();
            _pageSwitcher = new PageSwitcher<MasterDataPageType>(_panelContent, highlightButton => highlightButton.BackColor = Color.DarkGray, nonHighlightButton => nonHighlightButton.BackColor = Color.Silver);

            var timeAccounts = new List<TimeAccountGridEntry>();
            

            _pageSwitcher.RegisterPage(MasterDataPageType.TimeAccounts, timeAccountPage, _buttonTimeAccounts);
            _pageSwitcher.RegisterPage(MasterDataPageType.Activities, activityPage, _buttonActivities);
            _pageSwitcher.RegisterPage(MasterDataPageType.Objectives, objectivePage, _buttonObjectives);

            _pageSwitcher.SetActivePage(MasterDataPageType.TimeAccounts);
        }

        private void ButtonTimeAccounts_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MasterDataPageType.TimeAccounts);
        }

        private void ButtonActivities_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MasterDataPageType.Activities);
        }

        private void ButtonObjectives_Click(object sender, EventArgs e)
        {
            _pageSwitcher.SetActivePage(MasterDataPageType.Objectives);
        }
    }
}
