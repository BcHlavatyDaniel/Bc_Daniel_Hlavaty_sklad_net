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
    /// 

    public partial class MainWindow : Window
    {
        DbActions mDbActions;
        CommonActions mCommonActions;
        bool hamClosed = true;
        List<int> mVisibleList = new List<int>();
        List<int> mButtonList;
        VideoCapture mCapture = new VideoCapture();
        ImageViewer mViewer = new ImageViewer();

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
           
            mDbActions = new DbActions();
            mCommonActions = new CommonActions();
            DataSet data = mDbActions.LoadAllUsers();
            LoadGrid(data);
            DataSet itData = mDbActions.LoadAllItems();
            LoadItemsGrid(itData);
            LoadLogGrid(mDbActions.LoadAllLogs());
            itemsGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;
            icon_add_err.Visibility = Visibility.Hidden;

            FirstNameSearchCmb.Items.Add("");
            SecondnameSearchCmb.Items.Add("");
            Year_numbersSearchCmb.Items.Add("");
            AddressSearchCmb.Items.Add("");
            PhoneSearchCmb.Items.Add("");
            FirstNameItemCmb.Items.Add("");
            SecondNameItemCmb.Items.Add("");
            Year_numbersItemCmb.Items.Add("");
            DataTable dataTable = data.Tables[0];
            ItemNameCmb.Items.Add("");

            foreach(DataRow row in itData.Tables[0].Rows)
            {
                if (!ItemNameCmb.Items.Contains(row["name"].ToString()))
                    ItemNameCmb.Items.Add(row["name"].ToString());
            }

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
        }

        ///<summary>                                                 
        ///   ####################################    USER    ####################################    
        ///</summary>

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            gridData.Tables[0].Columns.Add("rok-id", typeof(string));
            gridData.Tables[0].Columns.Add("pocet tovaru", typeof(int));
            gridData.Tables[0].Columns.Remove("created_at");
            foreach (DataRow row in gridData.Tables[0].Rows)
            {
                row["rok-id"] = row["year"] + "-" + row["_numbers"];
                row["pocet tovaru"] = mDbActions.GetNumberOfItemsForUser(row["year"].ToString(), row["_numbers"].ToString());
            }
            gridData.Tables[0].Columns.Remove("year");
            gridData.Tables[0].Columns.Remove("_numbers");
            gridData.Tables[0].Columns["rok-id"].SetOrdinal(0);

            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            if (dataGrid.Columns.Count != 0)
            {
                dataGrid.Columns[1].MaxWidth = 150;
                dataGrid.Columns[2].MaxWidth = 150;
                dataGrid.Columns[3].MaxWidth = 200;
                dataGrid.Columns[4].MaxWidth = 100;
                dataGrid.Columns[5].MaxWidth = 50;
            }
            updateTooltips();
            dataGrid_CmbPositionUpdate(dataGrid, 0);
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

            if (AddressSearchCmb.SelectedIndex != -1)
            {
                address = AddressSearchCmb.SelectedItem.ToString();
            }

            if (PhoneSearchCmb.SelectedIndex != -1)
            {
                phone = PhoneSearchCmb.SelectedItem.ToString();
            }

            if (name == "" && sName == "" && keyy == "" && address == "" && phone == "")
            {
                LoadGrid(mDbActions.LoadAllUsers());
                return;
            }

            EditUserStruct userStruct = new EditUserStruct
            {
                s_name = sName,
                f_name = name,
                address = address,
                tel = phone,
                keyy = keyy,
                keyn = keyn
            };

            DataSet data = mDbActions.SearchForUserNames(userStruct);
            LoadGrid(data);
        }

        private void Profile_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = ((DataRowView)dataGrid.SelectedItem);
            if (datView == null) return;
            EditUserStruct userStruct = new EditUserStruct
            {
                keyy = datView.Row.ItemArray[0].ToString().Substring(0, 2),
                keyn = datView.Row.ItemArray[0].ToString().Substring(3, 3),
                f_name = datView.Row.ItemArray[1].ToString(),
                s_name = datView.Row.ItemArray[2].ToString(),
                address = datView.Row.ItemArray[3].ToString(),
                tel = datView.Row.ItemArray[4].ToString()
            };

            User_details mUserDWindow = new User_details(userStruct, mViewer, mCapture);
            mUserDWindow.Owner = this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            UpdateGrids();
        }

        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            dataGrid.Columns[1].MaxWidth = 150;
            dataGrid.Columns[2].MaxWidth = 150;
            dataGrid.Columns[3].MaxWidth = 200;
            dataGrid.Columns[4].MaxWidth = 100;
            dataGrid.Columns[5].MaxWidth = 50;
            updateTooltips();
            dataGrid_CmbPositionUpdate(dataGrid, 0);
        }


        ///<summary>
        ///    ------------------------------------    USER_MODAL    ------------------------------------   
        ///</summary>

        public void ModalUserAddInit(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = true;
        }

        public void ModalBack(object sender, RoutedEventArgs e)
        {
            icon_add_err.Visibility = Visibility.Hidden;
            text_add_err.Text = "";
            DialogHost.IsOpen = false;
            DataSet data = mDbActions.LoadAllUsers();
            LoadGrid(data);
        }



        public void ModalAdd(object sender, RoutedEventArgs e)
        {
            bool err = false;
            icon_add_err.Visibility = Visibility.Visible;

            if (text_first_name.Text == "")
            {
                err = true;
                text_add_err.Foreground = System.Windows.Media.Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln prve meno";
            }
            if (text_second_name.Text == "")
            {
                err = true;
                text_add_err.Foreground = System.Windows.Media.Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln druhe meno";
            }
            if (text_address.Text == "")
            {
                err = true;
                text_add_err.Foreground = System.Windows.Media.Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln adresu";
            }
            if (text_tel.Text == "")
            {
                err = true;
                text_add_err.Foreground = System.Windows.Media.Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln tel. cislo";
            }

            if (err) return;

            EditUserStruct userStruct = new EditUserStruct
            {
                f_name = text_first_name.Text,
                s_name = text_second_name.Text,
                address = text_address.Text,
                tel = text_tel.Text
            };

            mDbActions.AddUser(userStruct);
            text_add_err.Text = "Uspesne pridane.";
            text_add_err.Foreground = System.Windows.Media.Brushes.Green;
            icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            text_address.Text = "";
            text_tel.Text = "";
            text_first_name.Text = "";
            text_second_name.Text = "";
        }

        private new void PreviewTextInput(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (!CommonActions.IsNumeric(box.Text))
            {
                for (int i = 0; i < box.Text.Length; i++)
                {
                    if (!int.TryParse(box.Text[i].ToString(), out int outVar))
                    {
                        box.Text = box.Text.Remove(i, 1);
                    }
                }
            }
        }

        ///<summary>
        ///   ####################################    ITEMS grid   ####################################      
        ///</summary>

        private void LoadItemsGrid(DataSet allItems)
        {
            DataSet allUsers = mDbActions.LoadAllUsers();
            itemsDataGrid.ItemsSource = null;
            allItems.Tables[0].Columns.Add("rok-id", typeof(string));
            allItems.Tables[0].Columns.Add("prve meno", typeof(string));
            allItems.Tables[0].Columns.Add("druhe meno", typeof(string));


            foreach (DataRow row in allItems.Tables[0].Rows)
            {
                row["rok-id"] = row["user_year"] + "-" + row["user_numbers"];

                foreach (DataRow uRow in allUsers.Tables[0].Rows)
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

            DateTime retTime;
            DateTime paidTime;
            DateTime soldTime;
            DateTime createdTime;
            DateTime archTime;
            int usedCard;
            mButtonList = new List<int>();

            foreach (DataRow row in allItems.Tables[0].Rows)
            {
                DateTime.TryParse(row["returned_at"].ToString(), out retTime);
                DateTime.TryParse(row["paid_at"].ToString(), out paidTime);
                DateTime.TryParse(row["sold_at"].ToString(), out soldTime);
                DateTime.TryParse(row["created_at"].ToString(), out createdTime);
                DateTime.TryParse(row["archived_at"].ToString(), out archTime);
                int.TryParse(row["used_card"].ToString(), out usedCard);

                if (archTime.ToString() != "1/1/0001 12:00:00 AM")
                {
                    mButtonList.Add(6);
                }
                else if (retTime.ToString() != "1/1/0001 12:00:00 AM")
                {
                    mButtonList.Add(3);
                }
                else if (paidTime.ToString() != "1/1/0001 12:00:00 AM")
                {
                    if (usedCard == 1) mButtonList.Add(4);
                    else mButtonList.Add(5);
                }
                else if (soldTime.ToString() != "1/1/0001 12:00:00 AM")
                {
                    if (usedCard == 1) mButtonList.Add(1);
                    else mButtonList.Add(2);
                }
                else
                    mButtonList.Add(0);
            }

            allItems.Tables[0].Columns.Remove("created_at");
            allItems.Tables[0].Columns.Remove("sold_at");
            allItems.Tables[0].Columns.Remove("paid_at");
            allItems.Tables[0].Columns.Remove("returned_at");
            allItems.Tables[0].Columns.Remove("archived_at");
            allItems.Tables[0].Columns.Remove("user_year");
            allItems.Tables[0].Columns.Remove("user_numbers");
            allItems.Tables[0].Columns.Remove("description");
            allItems.Tables[0].Columns.Remove("used_card");
            allItems.Tables[0].Columns.Remove("photo");
            allItems.Tables[0].Columns["rok-id"].SetOrdinal(0);
            allItems.Tables[0].Columns["prve meno"].SetOrdinal(1);
            allItems.Tables[0].Columns["druhe meno"].SetOrdinal(2);
            itemsDataGrid.ItemsSource = allItems.Tables[0].DefaultView;
            itemsDataGrid.CanUserAddRows = false;
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
            if (itemsDataGrid.Columns.Count > 4)
            {
                itemsDataGrid.Columns[1].MaxWidth = 150;
                itemsDataGrid.Columns[2].MaxWidth = 150;
                itemsDataGrid.Columns[4].MaxWidth = 150;
                itemsDataGrid.Columns[5].MaxWidth = 150;
                itemsDataGrid.Columns[6].MaxWidth = 150;
            }
            dataGrid_CmbPositionUpdate(itemsDataGrid, 1);
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


            DataSet data = mDbActions.SearchForItems(userStruct);
            LoadItemsGrid(data);
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
            mUserDWindow.Owner = this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            UpdateGrids();
        }

        private void Item_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = ((DataRowView)itemsDataGrid.SelectedItem);
            Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[3].ToString(), datView.Row.ItemArray[2].ToString(), datView.Row.ItemArray[1].ToString(), datView.Row.ItemArray[0].ToString() , mViewer, mCapture);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            UpdateGrids();
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
                DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);
                presenter.ApplyTemplate();
                var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
                mDbActions.LoadSaveSpecificItemDescription(id, true, textbox.Text);
                mVisibleList.Remove(index);
            }
            else
            {
                string desc = mDbActions.LoadSaveSpecificItemDescription(id, false, "");
                mVisibleList.Add(index);
                DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);// FindVisualChild<DataGridDetailsPresenter>(gridRow);
                presenter.ApplyTemplate();
                var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
                textbox.Text = desc;
                gridRow.DetailsVisibility = Visibility.Visible;
            }
        }

        private void Save_Description(object sender, RoutedEventArgs e)
        {
            int index = itemsDataGrid.SelectedIndex;
            DataGridRow gridRow = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromItem(itemsDataGrid.SelectedItem);
            DataRowView rowView = (DataRowView)itemsDataGrid.SelectedItem;

            string id = rowView.Row.ItemArray[3].ToString();
            DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);// FindVisualChild<DataGridDetailsPresenter>(gridRow);
            presenter.ApplyTemplate();
            var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
            mDbActions.LoadSaveSpecificItemDescription(id, true, textbox.Text);
        }

        ///<summary>
        ///   ####################################    LOG   ####################################        
        ///</summary>

        private void LoadLogGrid(DataSet data)
        {
            logDataGrid.ItemsSource = null;
            data.Tables[0].Columns.Add("Popis", typeof(string));
            data.Tables[0].Columns.Add("Dna", typeof(string));
            DateTime outTime;

            foreach (DataRow row in data.Tables[0].Rows)
            {
                DateTime.TryParse(row["created_at"].ToString(), out outTime);
                row["Dna"] = outTime.ToShortDateString();
                if (row["type"].ToString() == "INSERT")
                {
                    row["Popis"] = "Pridanie";
                }
                else
                {
                    if (row["price"].ToString() != "0")
                    {
                        row["Popis"] = "Zmena ceny z " + row["old_price"] + " => " + row["price"];
                    }
                    else if (row["item_returned_at"].ToString() != "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Vratenie";
                    }
                    else if (row["item_archived_at"].ToString() != "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Archivovanie";
                    }
                    else if (row["item_sold_at"].ToString() != "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Predaj";
                    }
                    else if (row["item_paid_at"].ToString() != "1/1/0001 12:00:00 AM")
                    {
                        row["Popis"] = "Zaplatenie";
                    } else
                    {
                        row["Popis"] = "Ine upravy tovaru";
                    }
                }
            }

            data.Tables[0].Columns.Remove("id");
            data.Tables[0].Columns.Remove("price");
            data.Tables[0].Columns.Remove("old_price");
            data.Tables[0].Columns.Remove("item_created_at");
            data.Tables[0].Columns.Remove("created_at");
            data.Tables[0].Columns.Remove("item_paid_at");
            data.Tables[0].Columns.Remove("item_archived_at");
            data.Tables[0].Columns.Remove("item_sold_at");
            data.Tables[0].Columns.Remove("item_returned_at");
            data.Tables[0].Columns.Remove("type");

            logDataGrid.ItemsSource = data.Tables[0].DefaultView;
        }

        private void Item_Log_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = ((DataRowView)logDataGrid.SelectedItem);
            DataSet itemRow = mDbActions.LoadSpecificItem(datView.Row.ItemArray[0].ToString());
            DataSet userRow = mDbActions.LoadSpecificUser(itemRow.Tables[0].Rows[0]["user_year"].ToString(), itemRow.Tables[0].Rows[0]["user_numbers"].ToString());
            Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[0].ToString(), userRow.Tables[0].Rows[0]["first_name"].ToString(), userRow.Tables[0].Rows[0]["second_name"].ToString(), userRow.Tables[0].Rows[0]["year"].ToString() + "-" + userRow.Tables[0].Rows[0]["_numbers"].ToString(), mViewer, mCapture);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            picker.Text = "";
            UpdateGrids();
        }

        private void Select_log(object sender, RoutedEventArgs e)
        {
            DateTime day;
            DateTime.TryParse(picker.ToString(), out day);
            DataSet data = mDbActions.LoadLogByDay(day);
            LoadLogGrid(data);
        }

        ///<summary>
        ///   ####################################    HAMBURGER MENU   ####################################       
        ///</summary>

        private void Open_Hamburger(object sender, RoutedEventArgs e)
        {
            if (hamClosed)
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

        private void Users_Open(object sender, RoutedEventArgs e)
        {
            itemsGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;
            usersGrid.Visibility = Visibility.Visible;
            btnAddUser.Visibility = Visibility.Visible;
        }

        private void Items_Open(object sender, RoutedEventArgs e)
        {
            usersGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;
            btnAddUser.Visibility = Visibility.Collapsed;
            itemsGrid.Visibility = Visibility.Visible;
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
            dataGrid_CmbPositionUpdate(itemsDataGrid, 1);
        }

        private void Log_Open(object sender, RoutedEventArgs e)
        {
            usersGrid.Visibility = Visibility.Collapsed;
            itemsGrid.Visibility = Visibility.Collapsed;
            btnAddUser.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Visible;
        }

        ///<summary>
        ///   ####################################    COMMON   ####################################     
        ///</summary>
        ///

        private void dataGrid_CmbPositionUpdate(DataGrid grid, int type)
        {
            if (type == 0)
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                DataGridCell cell = CommonActions.GetGridCell(gridRow, 0);
                Year_numbersSearchCmb.Width = cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 1);
                FirstNameSearchCmb.Width = cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 2);
                SecondnameSearchCmb.Width = cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 3);
                AddressSearchCmb.Width = cell.ActualWidth > 200 ? 200 : cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 4);
                PhoneSearchCmb.Width = cell.ActualWidth;
            }
            else
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                itemsDataGrid.Columns[0].DisplayIndex = itemsDataGrid.Columns.Count - 1;
                DataGridCell cell = CommonActions.GetGridCell(gridRow, 1);
                Year_numbersItemCmb.Width = cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 2);
                FirstNameItemCmb.Width = cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 3);
                SecondNameItemCmb.Width = cell.ActualWidth;
                cell = CommonActions.GetGridCell(gridRow, 5);
                ItemNameCmb.Width = cell.ActualWidth;
                int counter = 0;
                foreach (var item in itemsDataGrid.Items)
                {
                    DataGridRow row = (DataGridRow)itemsDataGrid.ItemContainerGenerator.ContainerFromItem(item);
                    if (row == null) continue;
                    switch (mButtonList.ElementAt(counter))
                    {
                        case 1:
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
                                break;
                            }
                        case 2:
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
                                break;
                            }
                        case 3:
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
                                break;
                            }
                        case 4:
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
                                break;
                            }
                        case 5:
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
                                break;
                            }
                        case 6:
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
                                break;
                            }
                    }
                    counter++;
                }
            }
        }


        private void updateTooltips()
        {
            int max = dataGrid.Items.Count;
            //adresa he col 3
            for (int i = 0; i < max; i++)
            {
                DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                if (gridRow == null) return;
                DataGridCell cell = CommonActions.GetGridCell(gridRow, 3);
                var cellContent = (TextBlock)cell.Content;
                cell.ToolTip = cellContent.Text;
            }
        }

        private void UpdateGrids()
        {
            LoadGrid(mDbActions.LoadAllUsers());
            LoadItemsGrid(mDbActions.LoadAllItems());
            LoadLogGrid(mDbActions.LoadAllLogs());
        }

    }
}

