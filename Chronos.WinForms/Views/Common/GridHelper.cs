using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Chronos.WinForms.Attributes;
using Chronos.WinForms.DataObjects;

namespace Chronos.WinForms.Views.Common
{
    internal static class GridHelper
    {
        public static bool TryGetSingleSelectedDataBoundItem<T>(DataGridView dataGridView, [NotNullWhen(true)] out T? selectedItem)
        {
            var selectedCells = dataGridView.SelectedCells;

            if (selectedCells.Count != 1)
            {
                selectedItem = default;
                return false;
            }

            if (selectedCells[0].OwningRow.DataBoundItem is not T item)
            {
                selectedItem = default;
                return false;
            }

            selectedItem = item;
            return true;
        }

        public static void ApplyAttributes<T>(DataGridView dataGridView)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(var propertyInfo in propertyInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes().ToList();
                if(attributes.Count == 0)
                {
                    continue;
                }

                var hiddenColumnAttribute = attributes.OfType<HiddenColumnAttribute>().FirstOrDefault();
                if(hiddenColumnAttribute != null)
                {
                    dataGridView.Columns[propertyInfo.Name].Visible = false;
                }

                var cellTemplateTypeAttribute = attributes.OfType<CellTemplateTypeAttribute>().FirstOrDefault();
                if(cellTemplateTypeAttribute != null)
                {
                    dataGridView.Columns[propertyInfo.Name].CellTemplate = (DataGridViewCell) Activator.CreateInstance(cellTemplateTypeAttribute.CellTemplateType)!;
                }
            }
        }
    }
}
