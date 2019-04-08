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


namespace materialApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbActions mDbActions;
        bool hamClosed = true;
        List<int> mVisibleList = new List<int>();
        VideoCapture capture = new VideoCapture();
        ImageViewer viewer = new ImageViewer();

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
           
            mDbActions = new DbActions();
            DataSet data = mDbActions.LoadData();
            LoadGrid(data);
            DataSet itemData = mDbActions.LoadAllItems();
            LoadItemsGrid(itemData);
            DataSet logData = mDbActions.LoadLogData();
            LoadLogGrid(logData);
            itemsGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;

            FirstNameSearchCmb.Items.Add("");
            SecondnameSearchCmb.Items.Add("");
            Year_numbersSearchCmb.Items.Add("");
            AddressSearchCmb.Items.Add("");
            PhoneSearchCmb.Items.Add("");
            FirstNameItemCmb.Items.Add("");
            SecondNameItemCmb.Items.Add("");
            Year_numbersItemCmb.Items.Add("");
            DataTable dataTable = data.Tables[0];
            
            foreach (DataRow row in dataTable.Rows)
            {
                if (!FirstNameSearchCmb.Items.Contains(row["first_name"].ToString()))
                    FirstNameSearchCmb.Items.Add(row["first_name"].ToString());
                if (!FirstNameItemCmb.Items.Contains(row["first_name"].ToString()))
                    FirstNameItemCmb.Items.Add(row["first_name"].ToString());
                if (!SecondNameItemCmb.Items.Contains(row["second_name"].ToString()))
                    SecondNameItemCmb.Items.Add(row["second_name"].ToString());
                if (!SecondnameSearchCmb.Items.Contains(row["second_name"].ToString()))
                    SecondnameSearchCmb.Items.Add(row["second_name"].ToString());
                if (!Year_numbersSearchCmb.Items.Contains(row["rok-id"].ToString()))
                    Year_numbersSearchCmb.Items.Add(row["rok-id"].ToString());
                if (!Year_numbersItemCmb.Items.Contains(row["rok-id"].ToString()))
                    Year_numbersItemCmb.Items.Add(row["rok-id"].ToString());
                if (!AddressSearchCmb.Items.Contains(row["address"].ToString()))
                    AddressSearchCmb.Items.Add(row["address"].ToString());
                if (!PhoneSearchCmb.Items.Contains(row["telephone"].ToString()))
                    PhoneSearchCmb.Items.Add(row["telephone"].ToString());
            }
            // dataGrid.Columns[1].Visibility = Visibility.Collapsed;
        }
        /* TO DO
        private void UpdateCmbItems(DataSet data)
        {
            DataTable dataTable = data.Tables[0];

            for (int i = FirstNameSearchCmb.Items.Count; i > 1; i--)
            {
                FirstNameSearchCmb.Items.RemoveAt(i-1);
            }

            for (int i = SecondnameSearchCmb.Items.Count; i > 1; i--)
            {
                SecondnameSearchCmb.Items.RemoveAt(i-1);
            }
            

            foreach (DataRow row in dataTable.Rows)
            {
                if (!FirstNameSearchCmb.Items.Contains(row["first_name"].ToString()))
                    FirstNameSearchCmb.Items.Add(row["first_name"].ToString());
                if (!SecondnameSearchCmb.Items.Contains(row["second_name"].ToString()))
                    SecondnameSearchCmb.Items.Add(row["second_name"].ToString());
            }
        }*/

        private void Users_Open(object sender, RoutedEventArgs e)   //TO DO bude treba aj tabulky updatovat
        {
            itemsGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;
            usersGrid.Visibility = Visibility.Visible;
        }

        private void Items_Open(object sender, RoutedEventArgs e)
        {
            usersGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;
            itemsGrid.Visibility = Visibility.Visible;
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
            dataGrid_CmbPositionUpdate(itemsDataGrid, 1);
        }

        private void Log_Open(object sender, RoutedEventArgs e)
        {
            usersGrid.Visibility = Visibility.Collapsed;
            itemsGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Visible;          
        }

        private void Profile_Open(object sender, RoutedEventArgs e)
        {
            User_details mUserDWindow = new User_details((DataRowView)dataGrid.SelectedItem, viewer, capture);
            mUserDWindow.Owner = this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
        }

        private void Open_Hamburger(object sender, RoutedEventArgs e)
        {
            if(hamClosed)
            {
                Storyboard board = this.FindResource("OpenMenu") as Storyboard;
                board.Begin();
            }
            else
            {
                Storyboard board = this.FindResource("CloseMenu") as Storyboard;
                board.Begin();
            }
            hamClosed = !hamClosed;
        }

        private void Search(object sender, SelectionChangedEventArgs e)
        {
            string name = "";
            string sName = "";
            string keyy = "";
            string keyn = "";
            string address = "";
            string phone = "";

            if (FirstNameSearchCmb.SelectedIndex != -1)
            {
                name = FirstNameSearchCmb.SelectedItem.ToString();
            }

            if (SecondnameSearchCmb.SelectedIndex != -1)
            {
                sName = SecondnameSearchCmb.SelectedItem.ToString();
            }

            if (Year_numbersSearchCmb.SelectedIndex != -1)
            {
                if (Year_numbersSearchCmb.SelectedItem.ToString() != "")
                {
                    keyy = Year_numbersSearchCmb.SelectedItem.ToString().Substring(0, 2);
                    keyn = Year_numbersSearchCmb.SelectedItem.ToString().Substring(3, 3);
                }
                
            }

            if(AddressSearchCmb.SelectedIndex != -1)
            {
                address = AddressSearchCmb.SelectedItem.ToString();
            }

            if (PhoneSearchCmb.SelectedIndex != -1)
            {
                phone = PhoneSearchCmb.SelectedItem.ToString();
            }

            if (name == "" && sName == "" && keyy == "" && address == "" && phone == "")
            {
                LoadGrid(mDbActions.LoadData());
                return;
            }

            EditUserStruct userStruct = new EditUserStruct {
                s_name = sName,
                f_name = name,
                address = address,
                tel = phone,
                keyy = keyy,
                keyn = keyn
            };

            DataSet data = mDbActions.LoadSearchedNamesData(userStruct);
            LoadGrid(data);
        }

        private void Open_Description(object sender, RoutedEventArgs e) //DUPLICATE daff aside
        {
            
            int index = itemsDataGrid.SelectedIndex;
            DataGridRow gridRow = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromItem(itemsDataGrid.SelectedItem);
            DataRowView rowView = (DataRowView)itemsDataGrid.SelectedItem;

            string id = rowView.Row.ItemArray[3].ToString();
            if (mVisibleList.Contains(index))
            {
                gridRow.DetailsVisibility = Visibility.Collapsed;
                DataGridDetailsPresenter presenter = FindVisualChild<DataGridDetailsPresenter>(gridRow);
                presenter.ApplyTemplate();
                var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
                mDbActions.ItemDescription(id, true, textbox.Text);
                mVisibleList.Remove(index);
            }
            else
            {
                string desc = mDbActions.ItemDescription(id, false, "");
                mVisibleList.Add(index);
                DataGridDetailsPresenter presenter = FindVisualChild<DataGridDetailsPresenter>(gridRow);
                presenter.ApplyTemplate();
                var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
                textbox.Text = desc;
                gridRow.DetailsVisibility = Visibility.Visible;
            }
        }

        private void SearchItems(object sender, RoutedEventArgs e)
        {
            string name = "";
            string sName = "";
            string keyy = "";
            string keyn = "";

            if (FirstNameItemCmb.SelectedIndex != -1)
            {
                name = FirstNameItemCmb.SelectedItem.ToString();
            }
            if (SecondNameItemCmb.SelectedIndex != -1)
            {
                sName = SecondNameItemCmb.SelectedItem.ToString();
            }
            if (Year_numbersItemCmb.SelectedIndex != -1)
            {
                if (Year_numbersItemCmb.SelectedItem.ToString() != "")
                {
                    keyy = Year_numbersItemCmb.SelectedItem.ToString().Substring(0, 2);
                    keyn = Year_numbersItemCmb.SelectedItem.ToString().Substring(3, 3);
                }
            }

            if (name == "" && sName == "" && keyy == "")
            { 
                LoadItemsGrid(mDbActions.LoadAllItems());
                return;
            }

            EditUserStruct userStruct = new EditUserStruct
            {
                s_name = sName,
                f_name = name,
                keyy = keyy,
                keyn = keyn
            };


            DataSet data = mDbActions.LoadSearchedItems(userStruct);
            LoadItemsGrid(data);
        }

        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)  //TO DO ONE METHOD FOR THESE COZ OMFGGKGDKFLSAD
        {
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            dataGrid_CmbPositionUpdate(dataGrid, 0);
        }

        private void dataGrid_CmbPositionUpdate(DataGrid grid, int type)
        {
            if(type == 0)
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                DataGridCell cell = GetGridCell(gridRow, 0);
                Year_numbersSearchCmb.Width = cell.ActualWidth;
                cell = GetGridCell(gridRow, 1);
                FirstNameSearchCmb.Width = cell.ActualWidth;
                cell = GetGridCell(gridRow, 2);
                SecondnameSearchCmb.Width = cell.ActualWidth;
                cell = GetGridCell(gridRow, 3);
                AddressSearchCmb.Width = cell.ActualWidth;
                cell = GetGridCell(gridRow, 4);
                PhoneSearchCmb.Width = cell.ActualWidth;
            } else
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                DataGridCell cell = GetGridCell(gridRow, 0);
                Year_numbersItemCmb.Margin = new Thickness(cell.ActualWidth,20,0,0);
                cell = GetGridCell(gridRow, 1); //dlzka prveho ako margin, 
                Year_numbersItemCmb.Width = cell.ActualWidth;
                cell = GetGridCell(gridRow, 2);
                FirstNameItemCmb.Width = cell.ActualWidth;
                cell = GetGridCell(gridRow, 3);
                SecondNameItemCmb.Width = cell.ActualWidth; //pridat meno cmb
            }
        }

        public static DataGridCell GetGridCell(DataGridRow row, int column = 0)
        {
            if (row == null) return null;

            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) return null;

            DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
            if (cell != null) return cell;

            return cell;
        }

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            gridData.Tables[0].Columns.Add("rok-id", typeof(string));
            gridData.Tables[0].Columns.Add("pocet tovaru", typeof(int));
            gridData.Tables[0].Columns.Remove("created_at"); 
            foreach(DataRow row in gridData.Tables[0].Rows)
            {
                row["rok-id"] = row["year"] + "-" + row["_numbers"];
                row["pocet tovaru"] = mDbActions.GetNumberOfItemsForUser(row["year"].ToString(), row["_numbers"].ToString());
            }
            gridData.Tables[0].Columns.Remove("year");
            gridData.Tables[0].Columns.Remove("_numbers");
            gridData.Tables[0].Columns["rok-id"].SetOrdinal(0);
            
            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            //dataGrid.CanUserAddRows = false;
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            dataGrid_CmbPositionUpdate(dataGrid, 0);
        }

        private void LoadLogGrid(DataSet data)
        {
            logDataGrid.ItemsSource = null;
            data.Tables[0].Columns.Add("Popis", typeof(string));

            foreach(DataRow row in data.Tables[0].Rows)
            {
                if (row["type"].ToString() == "INSERT")
                {
                    row["Popis"] = "Pridanie";
                }
                else
                {
                    if (row["item_paid_at"].ToString() == "1/1/0001 12:00:00 AM" && row["item_sold_at"].ToString() != "1/1/0001 12:00:00 AM" && row["item_returned_at"].ToString() == "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Predanie";
                    }
                    else if (row["item_paid_at"].ToString() != "1/1/0001 12:00:00 AM" && row["item_sold_at"].ToString() == "1/1/0001 12:00:00 AM" && row["item_returned_at"].ToString() == "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Vyplatenie";
                    }
                    else if (row["item_paid_at"].ToString() == "1/1/0001 12:00:00 AM" && row["item_sold_at"].ToString() == "1/1/0001 12:00:00 AM" && row["item_returned_at"].ToString() != "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Vratenie";
                    }
                    else if (row["price"].ToString() == "0")
                    {
                        row["Popis"] = "Zmena ceny";
                    }
                }
                row["Popis"] += ", id tovaru " + row["i_id"];
            }

            data.Tables[0].Columns.Remove("id");
            data.Tables[0].Columns.Remove("i_id");
            data.Tables[0].Columns.Remove("price");
            data.Tables[0].Columns.Remove("item_created_at");
            data.Tables[0].Columns.Remove("item_paid_at");
            data.Tables[0].Columns.Remove("item_sold_at");
            data.Tables[0].Columns.Remove("item_returned_at");
            data.Tables[0].Columns.Remove("type");

            logDataGrid.ItemsSource = data.Tables[0].DefaultView;
        }

        private void LoadItemsGrid(DataSet allItems)
        {
            //DataSet dSet = mDbActions.LoadData();
            //get all items + names -> do i wanna go to uzivatel detail from here?

            //DataSet allItems = mDbActions.LoadAllItems();
            DataSet allUsers = mDbActions.LoadData();
            itemsDataGrid.ItemsSource = null;
            allItems.Tables[0].Columns.Add("rok-id", typeof(string));
            allItems.Tables[0].Columns.Add("prve meno", typeof(string));
            allItems.Tables[0].Columns.Add("druhe meno", typeof(string));
            allItems.Tables[0].Columns.Remove("created_at");
            allItems.Tables[0].Columns.Remove("sold_at");
            allItems.Tables[0].Columns.Remove("paid_at");
            allItems.Tables[0].Columns.Remove("returned_at");

          /*  for (int i = allItems.Tables[0].Rows.Count -1; i >= 0; i--) <-- TO DO by dates if is stored
            {
                if (allItems.Tables[0].Rows[i]["is_Stored"].ToString() == "False")
                {
                    allItems.Tables[0].Rows[i].Delete();
                }
            }
            allItems.Tables[0].AcceptChanges();*/

            foreach(DataRow row in allItems.Tables[0].Rows)
            {
                row["rok-id"] = row["user_year"] + "-" + row["user_numbers"];

                foreach(DataRow uRow in allUsers.Tables[0].Rows)
                {
                    if (row["user_year"].ToString() == uRow["year"].ToString())
                    {
                        if (row["user_numbers"].ToString() == uRow["_numbers"].ToString())
                        {
                            row["prve meno"] = uRow["first_name"].ToString();
                            row["druhe meno"] = uRow["second_name"].ToString();
                            break;
                        }
                        
                    }
                }
            }
            allItems.Tables[0].Columns.Remove("user_year");
            allItems.Tables[0].Columns.Remove("user_numbers");
            allItems.Tables[0].Columns.Remove("description");
            allItems.Tables[0].Columns.Remove("photo");
            allItems.Tables[0].Columns["rok-id"].SetOrdinal(0);
            allItems.Tables[0].Columns["prve meno"].SetOrdinal(1);
            allItems.Tables[0].Columns["druhe meno"].SetOrdinal(2);
            itemsDataGrid.ItemsSource = allItems.Tables[0].DefaultView;
            itemsDataGrid.CanUserAddRows = false;
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
            dataGrid_CmbPositionUpdate(itemsDataGrid, 1);
        }

        ///<summary>
        ///     COMMON
        ///</summary>
        ///


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


    }
}

