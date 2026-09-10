using System.ComponentModel;
using Chronos.Core;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Views.Dialogs;
using Chronos.WinForms.DataObjects;
using Chronos.WinForms.Views.Common;

namespace Chronos.Views.TabPages
{
    public partial class CategoryPage : UserControl
    {
        private readonly ChronosCore _chronosCore;
        private BindingList<CategoryGridEntry> _categories = new BindingList<CategoryGridEntry>();

        public CategoryPage(ChronosCore chronosCore)
        {
            InitializeComponent();
            _chronosCore = chronosCore;

            _gridCategories.BindDataSource(_categories);
            RefreshView();
        }

        private void RefreshView()
        {
            _categories.Clear();

            IReadOnlyList<Category> categories;

            try
            {
                categories = _chronosCore.CategoryService.GetAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not retrieve categories.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var category in categories)
            {
                _categories.Add(new CategoryGridEntry(category));
            }
        }

        private void GridActionBar_AddClicked(object sender, EventArgs e)
        {
            var dialog = new ManageCategoryDialog();
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.CategoryService.Create(dialog.CategoryName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not create category.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_EditClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridCategories.GridView, out CategoryGridEntry? categoryGridEntry))
            {
                return;
            }

            var dialog = new ManageCategoryDialog();
            dialog.CategoryName = categoryGridEntry.Name;            

            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            try
            {
                _chronosCore.CategoryService.Update(categoryGridEntry.Id, dialog.CategoryName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not update category.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }

        private void GridActionBar_RemoveClicked(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSingleSelectedDataBoundItem(_gridCategories.GridView, out CategoryGridEntry? categoryGridEntry))
            {
                return;
            }

            try
            {
                _chronosCore.CategoryService.Remove(categoryGridEntry.Id);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, $"Could not delete category.\n\nDetails: {exception}", "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshView();
        }
    }
}
