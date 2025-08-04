using System.ComponentModel;

namespace Chronos.WinForms.Views.Common
{
    public partial class ReadOnlyGrid : UserControl
    {
        public DataGridView GridView => _grid;

        public ReadOnlyGrid()
        {
            InitializeComponent();
        }

        public void BindDataSource<T>(BindingList<T> bindingList)
        {
            _grid.DataBindingComplete += (s,e) =>
            {
                GridHelper.ApplyAttributes<T>(_grid);
            };
            _grid.DataSource = bindingList;
        }
    }
}
