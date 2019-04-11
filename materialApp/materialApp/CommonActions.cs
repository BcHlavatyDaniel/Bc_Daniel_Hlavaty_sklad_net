using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace materialApp
{
    class CommonActions
    {
        public CommonActions()
        {

        }

        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        public static DataGridCell GetGridCell(DataGridRow row, int column = 0)
        {
            if (row == null) return null;

            DataGridCellsPresenter presenter = CommonActions.FindVisualChild<DataGridCellsPresenter>(row); //FindVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) return null;

            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
            if (cell != null) return cell;

            return cell;
        }

        public static bool IsNumeric(string text)
        {
            int outVar;
            return int.TryParse(text, out outVar);
        }

        public static bool IsDouble(string text)
        {
            double outVar;
            return double.TryParse(text, out outVar);
        }
    }
}
