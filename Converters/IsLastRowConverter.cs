using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using Dateien_Sortierprogramm.Data;

namespace Dateien_Sortierprogramm.Converters
{
    public class IsLastRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridCell cell)
            {
                DataGridRow row = FindParent<DataGridRow>(cell);
                DataGrid dataGrid = FindParent<DataGrid>(cell);
                int rowIndex = dataGrid.ItemContainerGenerator.IndexFromContainer(row);
                int rowCount = dataGrid.Items.Count;

                return rowIndex == rowCount - 1;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            if (parent == null)
                return null;

            if (parent is T parentControl)
                return parentControl;

            return FindParent<T>(parent);
        }
    }
}
