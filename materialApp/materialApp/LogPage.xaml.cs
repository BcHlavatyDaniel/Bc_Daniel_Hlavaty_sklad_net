//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
using System;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.IO;
using System.Net;
using DatabaseProj;

namespace materialApp
{

    public partial class LogPage : Page
    {
		private DbActions mDbActions;
        CommonActions mCommonActions;
        VideoCapture mCapture;
        ImageViewer mViewer;

        public LogPage(DbActions mDbActions, ImageViewer view, VideoCapture capture)
        {
            InitializeComponent();
			this.mDbActions = mDbActions;
            Init(view, capture);
        }

        private void Init(ImageViewer view, VideoCapture capture)
        {
            mCommonActions = new CommonActions();
            DataSet logData = mDbActions.LoadAllLogs();
            LoadLogGrid(logData);
            Item_idLogCmb.Items.Insert(0, "");
            Year_numbersLogCmb.Items.Insert(0, "");
            TypeCmb.Items.Insert(0, "");
            mCapture = capture;
            mViewer = view;
        }

        private void LoadLogGrid(DataSet data)
        {
            logDataGrid.ItemsSource = null;

            data.Tables[0].Columns.Remove("id");
            data.Tables[0].Columns["item_id"].ColumnName = "id tovar";
            data.Tables[0].Columns["user_id"].ColumnName = "id uzivatel";
            data.Tables[0].Columns["type"].ColumnName = "typ zmeny";
            data.Tables[0].Columns["change_text"].ColumnName = "popis zmeny";

            UpdateCmbox_items(data, 2);
            logDataGrid.ItemsSource = data.Tables[0].DefaultView;
            logDataGrid.Items.Refresh();
            logDataGrid.UpdateLayout();
            if (logDataGrid.Columns.Count > 4)
            {
                UpdateColumnsWidths();
            }
        }

        private void UpdateColumnsWidths()
        {
            logDataGrid.Columns[0].Width = 150; //actWidth / 10;//150;
            logDataGrid.Columns[1].Width = 150; //actWidth / 10;//150;
            logDataGrid.Columns[2].Width = 150; //actWidth / 10;//150;
            logDataGrid.Columns[3].Width = 150; //actWidth / 10;//150;
            logDataGrid.Columns[4].Width = 650; //actWidth / 2.3;//650;
            logDataGrid.Columns[5].Width = 250; //actWidth / 6;//250;
            dataGrid_CmbPositionUpdate(logDataGrid, 2);
        }

        private void UpdateCmbox_items(DataSet data, int type)
        {
            if (type == 2)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    if (!Year_numbersLogCmb.Items.Contains(row["id uzivatel"].ToString()))
                        Year_numbersLogCmb.Items.Add(row["id uzivatel"].ToString());
                    if (!Item_idLogCmb.Items.Contains(row["id tovar"].ToString()))
                        Item_idLogCmb.Items.Add(row["id tovar"].ToString());
                    if (!TypeCmb.Items.Contains(row["typ zmeny"].ToString()))
                        TypeCmb.Items.Add(row["typ zmeny"].ToString());
                }
            }
        }

        private void dataGrid_CmbPositionUpdate(DataGrid grid, int type)
        {
            if (type == 2)
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                logDataGrid.Columns[0].DisplayIndex = logDataGrid.Columns.Count - 1; //posunutie popisu 
            }
        }

        private void Item_Log_Open(object sender, RoutedEventArgs e)//MouseDoubleClick="Item_Log_Open"
		{
            DataRowView datView = ((DataRowView)logDataGrid.SelectedItem);
            DataSet itemRow = mDbActions.LoadSpecificItem(datView.Row.ItemArray[0].ToString());
            DataSet userRow = mDbActions.LoadSpecificUser(itemRow.Tables[0].Rows[0]["user_year"].ToString(), itemRow.Tables[0].Rows[0]["user_numbers"].ToString());
            Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[0].ToString(), userRow.Tables[0].Rows[0]["first_name"].ToString(), userRow.Tables[0].Rows[0]["second_name"].ToString(), userRow.Tables[0].Rows[0]["year"].ToString() + "-" + userRow.Tables[0].Rows[0]["_numbers"].ToString(), mViewer, mCapture);
            mItemDWindow.Owner = Window.GetWindow(this);//this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            picker.Text = "";
            ResetCmbs(2);
            LoadLogGrid(mDbActions.LoadAllLogs());
        }

        private bool ResetCmbs(int type)
        {
            bool changed = false;
            if (type == 2)
            {
                if (Item_idLogCmb.SelectedIndex != 0)
                {
                    changed = true;
                    Item_idLogCmb.SelectedIndex = 0;
                }
                if (Year_numbersLogCmb.SelectedIndex != 0)
                {
                    changed = true;
                    Year_numbersLogCmb.SelectedIndex = 0;
                }
                if (TypeCmb.SelectedIndex != 0)
                {
                    changed = true;
                    TypeCmb.SelectedIndex = 0;
                }
            }
            return changed;
        }

        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)
        {
            logDataGrid.Items.Refresh();
            logDataGrid.UpdateLayout();
            if (logDataGrid.Columns.Count > 4)
            {
                UpdateColumnsWidths();
            }
        }

        private void SearchLogs(object sender, RoutedEventArgs e)
		{
            string item_id = "";
            string user_id = "";
            string type = "";
            DateTime day = new DateTime();
            bool pickerEmpty = true;

            if (picker.ToString() != "")
            {
                DateTime.TryParse(picker.ToString(), out day);
                pickerEmpty = false;
            }

            if (Item_idLogCmb.SelectedIndex != -1)
            {
                item_id = Item_idLogCmb.SelectedItem.ToString();
            }
            if (Year_numbersLogCmb.SelectedIndex != -1)
            {
                user_id = Year_numbersLogCmb.SelectedItem.ToString();
            }
            if (TypeCmb.SelectedIndex != -1)
            {
                type = TypeCmb.SelectedItem.ToString();
            }

            if (pickerEmpty && item_id == "" && user_id == "" && type == "")
            {
                LoadLogGrid(mDbActions.LoadAllLogs());
                return;
            }

            LoadLogGrid(mDbActions.SearchForLogs(item_id, user_id, type, day));
        }
    }
}


