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
        List<LogTableItem> itemList = new List<LogTableItem>();
        List<string> nameCmbList = new List<string>(); //Chapem ze toto je napicu riesenie, ale ked tam chcem aj ten empty item, rychlejsie zbuchatelne..
        List<string> itemCmbList = new List<string>();
        List<string> typCmbList = new List<string>();

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
            nameCmbList.Insert(0, "");
            itemCmbList.Insert(0, "");
            typCmbList.Insert(0, "");
            mCapture = capture;
            mViewer = view;
            logDataGrid.ItemsSource = itemList;
            Item_idLogCmb.ItemsSource = itemCmbList;
            Year_numbersLogCmb.ItemsSource = nameCmbList;
            TypeCmb.ItemsSource = typCmbList;
        }

        private void LoadLogGrid(DataSet data)
        {
            data.Tables[0].Columns.Remove("id");
            data.Tables[0].Columns["item_id"].ColumnName = "id tovar";
            data.Tables[0].Columns["user_id"].ColumnName = "id uzivatel";
            data.Tables[0].Columns["type"].ColumnName = "typ zmeny";
            data.Tables[0].Columns["change_text"].ColumnName = "popis zmeny";

            itemList.Clear();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                itemList.Add(new LogTableItem
                {
                    id_tovar = row["id tovar"].ToString(),
                    id_uzivatela = row["id uzivatel"].ToString(),
                    typ_zmeny = row["typ zmeny"].ToString(),
                    popis = row["popis zmeny"].ToString(),
                    time = row["time"].ToString(),
                });
            }
            logDataGrid.Items.Refresh();
            logDataGrid.UpdateLayout();
            UpdateCmbox_items(data);
        }

        private void UpdateCmbox_items(DataSet data)
        {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    if (!nameCmbList.Contains(row["id uzivatel"].ToString()))
                        nameCmbList.Add(row["id uzivatel"].ToString());
                    if (!itemCmbList.Contains(row["id tovar"].ToString()))
                        itemCmbList.Add(row["id tovar"].ToString());
                    if (!typCmbList.Contains(row["typ zmeny"].ToString()))
                        typCmbList.Add(row["typ zmeny"].ToString());
                }
        }

        private void Item_Log_Open(object sender, RoutedEventArgs e)//MouseDoubleClick="Item_Log_Open"
		{
            LogTableItem item = ((LogTableItem)logDataGrid.SelectedItem);
            DataSet itemRow = mDbActions.LoadSpecificItem(item.id_tovar);
            DataSet userRow = mDbActions.LoadSpecificUser(itemRow.Tables[0].Rows[0]["user_year"].ToString(), itemRow.Tables[0].Rows[0]["user_numbers"].ToString());
            Item_details mItemDWindow = new Item_details(item.id_tovar, userRow.Tables[0].Rows[0]["first_name"].ToString(), userRow.Tables[0].Rows[0]["second_name"].ToString(), userRow.Tables[0].Rows[0]["year"].ToString() + "-" + userRow.Tables[0].Rows[0]["_numbers"].ToString(), mViewer, mCapture);
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


