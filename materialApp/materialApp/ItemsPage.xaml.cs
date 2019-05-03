//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
using DatabaseProj;
using System.Windows;
using System.Windows.Controls;
using System;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace materialApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class ItemsPage : Page
	{
        CommonActions mCommonActions;
        private DbActions mDbActions;
        List<int> mButtonList;
        enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived };
        VideoCapture mCapture;
        ImageViewer mViewer;

        public ItemsPage(DbActions mDbActions, ImageViewer view, VideoCapture capture)
        {
            InitializeComponent();
			this.mDbActions = mDbActions;
            Init(view, capture);
            FirstNameItemCmb.Items.Insert(0, "");
            SecondNameItemCmb.Items.Insert(0, "");
            Year_numbersItemCmb.Items.Insert(0, "");
            ItemNameCmb.Items.Insert(0, "");
        }

        private void Init(ImageViewer view, VideoCapture capture)
        {
            mCommonActions = new CommonActions();
            mCapture = capture;
            mViewer = view;
            DataSet itData = mDbActions.LoadAllItems();
            LoadItemsGrid(itData, mDbActions.LoadAllUsers());
        }

        private void LoadItemsGrid(DataSet allItems, DataSet allUsers)
        {
            itemsDataGrid.ItemsSource = null;
            allItems.Tables[0].Columns.Add("rok-id", typeof(string));
            allItems.Tables[0].Columns.Add("Prve meno", typeof(string));
            allItems.Tables[0].Columns.Add("Druhe meno", typeof(string));

            for (int i = allItems.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = allItems.Tables[0].Rows[i];
                row["rok-id"] = row["user_year"] + "-" + row["user_numbers"];
                foreach (DataRow uRow in allUsers.Tables[0].Rows)
                {
                    if (row["user_year"].ToString() == uRow["year"].ToString())
                    {
                        if (row["user_numbers"].ToString() == uRow["_numbers"].ToString())
                        {
                            row["Prve meno"] = uRow["first_name"].ToString();
                            row["Druhe meno"] = uRow["second_name"].ToString();
                            break;
                        }

                    }
                } //remove missing
                if (row["Prve meno"].ToString() == "")
                {
                    row.Delete();
                }
            }
            allItems.AcceptChanges();

            mButtonList = new List<int>();
            int add;

            foreach (DataRow row in allItems.Tables[0].Rows)
            {

                if (row["archived"].ToString() == "True")
                {
                    mButtonList.Add((int)mState.Earchived);
                }
                else
                {
                    int.TryParse(row["stav"].ToString(), out add);
                    mButtonList.Add(add);
                }
            }

            allItems.Tables[0].Columns.Remove("stav");
            allItems.Tables[0].Columns.Remove("archived");
            allItems.Tables[0].Columns.Remove("user_year");
            allItems.Tables[0].Columns.Remove("user_numbers");
            allItems.Tables[0].Columns.Remove("photo");
            allItems.Tables[0].Columns["name"].ColumnName = "Nazov";
            allItems.Tables[0].Columns["size"].ColumnName = "Velkost";
            allItems.Tables[0].Columns["price"].ColumnName = "Cena";
            allItems.Tables[0].Columns["description"].ColumnName = "Popis";
            allItems.Tables[0].Columns["rok-id"].SetOrdinal(0);
            allItems.Tables[0].Columns["Prve meno"].SetOrdinal(1);
            allItems.Tables[0].Columns["Druhe meno"].SetOrdinal(2);

            foreach (DataRow row in allUsers.Tables[0].Rows)
            {
                if (!FirstNameItemCmb.Items.Contains(row["first_name"].ToString()))
                    FirstNameItemCmb.Items.Add(row["first_name"].ToString());
                if (!SecondNameItemCmb.Items.Contains(row["second_name"].ToString()))
                    SecondNameItemCmb.Items.Add(row["second_name"].ToString());
                if (!Year_numbersItemCmb.Items.Contains(row["year"].ToString()+"-" + row["_numbers"].ToString()))
                    Year_numbersItemCmb.Items.Add(row["year"].ToString() + "-" + row["_numbers"].ToString());
            }

            foreach (DataRow row in allItems.Tables[0].Rows)
            {
                if (!ItemNameCmb.Items.Contains(row["Nazov"].ToString()))
                    ItemNameCmb.Items.Add(row["Nazov"].ToString());
            }

            itemsDataGrid.ItemsSource = allItems.Tables[0].DefaultView;
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();

            if (itemsDataGrid.Columns.Count > 4)
            {
                itemsDataGrid.Columns[0].Width = 200; //actWidth / 7.5; //200;
                itemsDataGrid.Columns[1].Width = 75; //actWidth / 20; //75;
                itemsDataGrid.Columns[2].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[3].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[4].Width = 75; //actWidth / 20; //75;
                itemsDataGrid.Columns[5].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[6].Width = 100; //actWidth / 15;//100;
                itemsDataGrid.Columns[7].Width = 100; //actWidth / 15;//100;
                itemsDataGrid.Columns[8].Width = 500; //actWidth / 3; //500;
            }
            doColors();
        }

        private void doColors()
        {
            if (itemsDataGrid.Columns.Count > 4)
            {
                itemsDataGrid.Columns[0].Width = 200; //actWidth / 7.5; //200;
                itemsDataGrid.Columns[1].Width = 75; //actWidth / 20; //75;
                itemsDataGrid.Columns[2].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[3].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[4].Width = 75; //actWidth / 20; //75;
                itemsDataGrid.Columns[5].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[6].Width = 100; //actWidth / 15;//100;
                itemsDataGrid.Columns[7].Width = 100; //actWidth / 15;//100;
                itemsDataGrid.Columns[8].Width = 500; //actWidth / 3; //500;
            }
            DataGridRow gridRow = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromIndex(0);
            if (gridRow == null) return;
            itemsDataGrid.Columns[0].DisplayIndex = itemsDataGrid.Columns.Count - 1;
            DataGridCell cell;
            int counter = 0;
            foreach (var item in itemsDataGrid.Items)
            {
                DataGridRow row = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row == null) continue;
                switch (mButtonList.ElementAt(counter))
                {
                    case (int)mState.Esold_cash:
                        {
                            cell = CommonActions.GetGridCell(row, 0);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 1);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 2);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 3);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 4);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 5);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 6);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 7);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            cell = CommonActions.GetGridCell(row, 8);
                            cell.Background = System.Windows.Media.Brushes.LimeGreen;
                            break;
                        }
                    case (int)mState.Esold_card:
                        {
                            cell = CommonActions.GetGridCell(row, 0);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 1);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 2);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 3);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 4);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 5);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 6);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 7);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            cell = CommonActions.GetGridCell(row, 8);
                            cell.Background = System.Windows.Media.Brushes.LightYellow;
                            break;
                        }
                    case (int)mState.Ereturned:
                        {
                            cell = CommonActions.GetGridCell(row, 0);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 1);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 2);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 3);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 4);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 5);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 6);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 7);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            cell = CommonActions.GetGridCell(row, 8);
                            cell.Background = System.Windows.Media.Brushes.LightGray;
                            break;
                        }
                    case (int)mState.Epaid_cash:
                        {
                            cell = CommonActions.GetGridCell(row, 0);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 1);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 2);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 3);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 4);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 5);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 6);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 7);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            cell = CommonActions.GetGridCell(row, 8);
                            cell.Background = System.Windows.Media.Brushes.Green;
                            break;
                        }
                    case (int)mState.Epaid_card:
                        {
                            cell = CommonActions.GetGridCell(row, 0);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 1);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 2);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 3);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 4);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 5);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 6);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 7);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            cell = CommonActions.GetGridCell(row, 8);
                            cell.Background = System.Windows.Media.Brushes.Yellow;
                            break;
                        }
                    case (int)mState.Earchived:
                        {
                            cell = CommonActions.GetGridCell(row, 0);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 1);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 2);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 3);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 4);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 5);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 6);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 7);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            cell = CommonActions.GetGridCell(row, 8);
                            cell.Background = System.Windows.Media.Brushes.OrangeRed;
                            break;
                        }
                }
                counter++;
            }
        }

        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)
        {
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
            if (itemsDataGrid.Columns.Count > 4)
            {
                itemsDataGrid.Columns[0].Width = 200; //actWidth / 7.5; //200;
                itemsDataGrid.Columns[1].Width = 75; //actWidth / 20; //75;
                itemsDataGrid.Columns[2].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[3].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[4].Width = 75; //actWidth / 20; //75;
                itemsDataGrid.Columns[5].Width = 150; //actWidth / 10; //150;
                itemsDataGrid.Columns[6].Width = 100; //actWidth / 15;//100;
                itemsDataGrid.Columns[7].Width = 100; //actWidth / 15;//100;
                itemsDataGrid.Columns[8].Width = 500; //actWidth / 3; //500;
            }
            doColors();
        }

		private void Item_Open(object sender, RoutedEventArgs e)
		{
            DataRowView datView = ((DataRowView)itemsDataGrid.SelectedItem);
            DataSet userRow = mDbActions.LoadSpecificUser(datView.Row.ItemArray[0].ToString().Substring(0, 2), datView.Row.ItemArray[0].ToString().Substring(3, 3));
            EditUserStruct userStruct = new EditUserStruct
            {
                keyy = datView.Row.ItemArray[0].ToString().Substring(0, 2),
                keyn = datView.Row.ItemArray[0].ToString().Substring(3, 3),
                f_name = userRow.Tables[0].Rows[0]["first_name"].ToString(),
                s_name = userRow.Tables[0].Rows[0]["second_name"].ToString(),
                address = userRow.Tables[0].Rows[0]["address"].ToString(),
                tel = userRow.Tables[0].Rows[0]["telephone"].ToString()
            };

            User_details mUserDWindow = new User_details(userStruct, mViewer, mCapture);
            mUserDWindow.Owner = Window.GetWindow(this);// this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            if (!ResetCmbs())
                LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
        }
		private void Profile_Item_Open(object sender, RoutedEventArgs e)
		{
            DataRowView datView = ((DataRowView)itemsDataGrid.SelectedItem);
            DataSet userRow = mDbActions.LoadSpecificUser(datView.Row.ItemArray[0].ToString().Substring(0, 2), datView.Row.ItemArray[0].ToString().Substring(3, 3));
            EditUserStruct userStruct = new EditUserStruct
            {
                keyy = datView.Row.ItemArray[0].ToString().Substring(0, 2),
                keyn = datView.Row.ItemArray[0].ToString().Substring(3, 3),
                f_name = userRow.Tables[0].Rows[0]["first_name"].ToString(),
                s_name = userRow.Tables[0].Rows[0]["second_name"].ToString(),
                address = userRow.Tables[0].Rows[0]["address"].ToString(),
                tel = userRow.Tables[0].Rows[0]["telephone"].ToString()
            };

            User_details mUserDWindow = new User_details(userStruct, mViewer, mCapture);
            mUserDWindow.Owner = Window.GetWindow(this);// this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            if (!ResetCmbs())
                LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
        }

        private bool ResetCmbs()
        {
            bool changed = false;
            if (Year_numbersItemCmb.SelectedIndex != 0)
            {
                changed = true;
                Year_numbersItemCmb.SelectedIndex = 0;
            }
            if (FirstNameItemCmb.SelectedIndex != 0)
            {
                changed = true;
                FirstNameItemCmb.SelectedIndex = 0;
            }
            if (SecondNameItemCmb.SelectedIndex != 0)
            {
                changed = true;
                SecondNameItemCmb.SelectedIndex = 0;
            }
            if (ItemNameCmb.SelectedIndex != 0)
            {
                changed = true;
                ItemNameCmb.SelectedIndex = 0;
            }
            return changed;
        }

		private void SearchItems(object sender, RoutedEventArgs e)
		{
            string fName = "";
            string sName = "";
            string keyy = "";
            string keyn = "";
            string name = "";

            if (FirstNameItemCmb.SelectedIndex != -1)
            {
                fName = FirstNameItemCmb.SelectedItem.ToString();
            }
            if (SecondNameItemCmb.SelectedIndex != -1)
            {
                sName = SecondNameItemCmb.SelectedItem.ToString();
            }
            if (ItemNameCmb.SelectedIndex != -1)
            {
                name = ItemNameCmb.SelectedItem.ToString();
            }

            if (Year_numbersItemCmb.SelectedIndex != -1)
            {
                if (Year_numbersItemCmb.SelectedItem.ToString() != "")
                {
                    keyy = Year_numbersItemCmb.SelectedItem.ToString().Substring(0, 2);
                    keyn = Year_numbersItemCmb.SelectedItem.ToString().Substring(3, 3);
                }
            }

            if (fName == "" && sName == "" && keyy == "" && name == "")
            {
                LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
                return;
            }

            EditUserStruct userStruct = new EditUserStruct
            {
                s_name = sName,
                f_name = fName,
                keyy = keyy,
                keyn = keyn,
                address = name 
            };

            DataSet userData = new DataSet();
            DataSet data = mDbActions.SearchForItems(userStruct, ref userData);
            // mDbActions.LoadAllUsers()
            LoadItemsGrid(data, userData);
        }
	}
}

