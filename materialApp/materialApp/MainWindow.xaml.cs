//using Syncfusion.Pdf;
//using Syncfusion.Pdf.Graphics;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Data;
using Emgu.CV;
using Emgu.CV.UI;
using DatabaseProj;

namespace materialApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class MainWindow : Window
    {
        bool hamClosed = true;
        enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived };

        public MainWindow()
        {
            InitializeComponent();
        }
		/*
        private void Init()
        {
           
            mDbActions = new DbActions();
            mCommonActions = new CommonActions();
            DataSet data = mDbActions.LoadAllUsers();
            LoadGrid(data);
            DataSet itData = mDbActions.LoadAllItems();
            LoadItemsGrid(itData, mDbActions.LoadAllUsers());
            DataSet logData = mDbActions.LoadAllLogs();
            LoadLogGrid(logData);
            itemsGrid.Visibility = Visibility.Collapsed;
            logGrid.Visibility = Visibility.Collapsed;
            icon_add_err.Visibility = Visibility.Hidden;

            FirstNameItemCmb.Items.Insert(0,"");
            SecondNameItemCmb.Items.Insert(0,"");
            Year_numbersItemCmb.Items.Insert(0,"");
            Item_idLogCmb.Items.Insert(0,"");
            Year_numbersLogCmb.Items.Insert(0,"");
            TypeCmb.Items.Insert(0,"");
            ItemNameCmb.Items.Insert(0,"");
            FirstNameSearchCmb.Items.Insert(0,"");
            SecondnameSearchCmb.Items.Insert(0,"");
            Year_numbersSearchCmb.Items.Insert(0,"");
            AddressSearchCmb.Items.Insert(0,"");
            PhoneSearchCmb.Items.Insert(0,"");
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

            gridData.Tables[0].Columns["first_name"].ColumnName = "Prve meno";
            gridData.Tables[0].Columns["second_name"].ColumnName = "Druhe meno";
            gridData.Tables[0].Columns["address"].ColumnName = "Adresa";
            gridData.Tables[0].Columns["telephone"].ColumnName = "Telefon";

            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            if (dataGrid.Columns.Count != 0)
            {
                UpdateColumnsWidths(0);
            }
            updateTooltips();
            UpdateCmbox_items(gridData, 0);
         //   dataGrid_CmbPositionUpdate(dataGrid, 0);
        }

        private void UpdateCmbox_items(DataSet data, int type)  //prolly will have to remove content b4 adding, this way old ones will stay as option
        {
            
            if (type == 0)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    if (!FirstNameSearchCmb.Items.Contains(row["Prve meno"].ToString()))
                        FirstNameSearchCmb.Items.Add(row["Prve meno"].ToString());
                    if (!FirstNameItemCmb.Items.Contains(row["Prve meno"].ToString()))
                        FirstNameItemCmb.Items.Add(row["Prve meno"].ToString());
                    if (!SecondNameItemCmb.Items.Contains(row["Druhe meno"].ToString()))
                        SecondNameItemCmb.Items.Add(row["Druhe meno"].ToString());
                    if (!SecondnameSearchCmb.Items.Contains(row["Druhe meno"].ToString()))
                        SecondnameSearchCmb.Items.Add(row["Druhe meno"].ToString());
                    if (!Year_numbersSearchCmb.Items.Contains(row["rok-id"].ToString()))
                        Year_numbersSearchCmb.Items.Add(row["rok-id"].ToString());
                    if (!Year_numbersItemCmb.Items.Contains(row["rok-id"].ToString()))
                        Year_numbersItemCmb.Items.Add(row["rok-id"].ToString());
                    if (!AddressSearchCmb.Items.Contains(row["Adresa"].ToString()))
                        AddressSearchCmb.Items.Add(row["Adresa"].ToString());
                    if (!PhoneSearchCmb.Items.Contains(row["Telefon"].ToString()))
                        PhoneSearchCmb.Items.Add(row["Telefon"].ToString());
                }
            }
            if (type == 1)
            {
                foreach (DataRow row in data.Tables[0].Rows)
                {
                    if (!ItemNameCmb.Items.Contains(row["Nazov"].ToString()))
                        ItemNameCmb.Items.Add(row["Nazov"].ToString());
                }
            }
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

        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            UpdateColumnsWidths(0);
            updateTooltips();
         //   dataGrid_CmbPositionUpdate(dataGrid, 0);
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
            ResetCmbs(0);
            UpdateGrids();
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
            //DataSet data = mDbActions.LoadAllUsers();
            //LoadGrid(data);
            ResetCmbs(0);
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
            int num;
            if (!int.TryParse(text_tel.Text, out num))
            {
                err = true;
                text_add_err.Foreground = System.Windows.Media.Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Tel. cislo moc dlhe";
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
            if (!ResetCmbs(0))
                LoadGrid(mDbActions.LoadAllUsers());
            text_add_err.Text = "Uspesne pridane.";
            text_add_err.Foreground = System.Windows.Media.Brushes.Green;
            icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            text_address.Text = "";
            text_tel.Text = "";
            text_first_name.Text = "";
            text_second_name.Text = "";
            CloseModalAfterAdd();                                   
        }

        private async void CloseModalAfterAdd()
        {
            await Task.Delay(1000);
            icon_add_err.Visibility = Visibility.Hidden;
            text_add_err.Text = "";
            DialogHost.IsOpen = false;
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

        private void LoadItemsGrid(DataSet allItems, DataSet allUsers)
        {
            itemsDataGrid.ItemsSource = null;
            allItems.Tables[0].Columns.Add("rok-id", typeof(string));
            allItems.Tables[0].Columns.Add("Prve meno", typeof(string));
            allItems.Tables[0].Columns.Add("Druhe meno", typeof(string));

            for ( int i = allItems.Tables[0].Rows.Count -1; i >= 0; i--)
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
            UpdateCmbox_items(allItems,1);
            itemsDataGrid.ItemsSource = allItems.Tables[0].DefaultView;
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
            if (itemsDataGrid.Columns.Count > 4)
            {
                UpdateColumnsWidths(1);
                  //itemsDataGrid.Columns[1].MaxWidth = 150;
                  //itemsDataGrid.Columns[2].MaxWidth = 150;
                  //itemsDataGrid.Columns[4].MaxWidth = 150;
                  //itemsDataGrid.Columns[5].MaxWidth = 150;
                  //itemsDataGrid.Columns[6].MaxWidth = 150;
            }
            dataGrid_CmbPositionUpdate(itemsDataGrid, 1);
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
            if(ItemNameCmb.SelectedIndex != -1)
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
                address = name //NJ OJEBY :D
            };

            DataSet userData = new DataSet();
            DataSet data = mDbActions.SearchForItems(userStruct, ref userData);
           // mDbActions.LoadAllUsers()
            LoadItemsGrid(data, userData);
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
            if(!ResetCmbs(1))
                UpdateGrids();
        }

        private void Item_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = ((DataRowView)itemsDataGrid.SelectedItem);
            Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[3].ToString(), datView.Row.ItemArray[2].ToString(), datView.Row.ItemArray[1].ToString(), datView.Row.ItemArray[0].ToString() , mViewer, mCapture);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            if(!ResetCmbs(1))
                UpdateGrids();
        }

        ///<summary>
        ///   ####################################    LOG   ####################################        
        ///</summary>

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
                UpdateColumnsWidths(2);
            }
            dataGrid_CmbPositionUpdate(logDataGrid, 2);
        }

        private void Item_Log_Open(object sender, RoutedEventArgs e)//MouseDoubleClick="Item_Log_Open"
        {
            DataRowView datView = ((DataRowView)logDataGrid.SelectedItem);
            DataSet itemRow = mDbActions.LoadSpecificItem(datView.Row.ItemArray[0].ToString());
            DataSet userRow = mDbActions.LoadSpecificUser(itemRow.Tables[0].Rows[0]["user_year"].ToString(), itemRow.Tables[0].Rows[0]["user_numbers"].ToString());
            Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[0].ToString(), userRow.Tables[0].Rows[0]["first_name"].ToString(), userRow.Tables[0].Rows[0]["second_name"].ToString(), userRow.Tables[0].Rows[0]["year"].ToString() + "-" + userRow.Tables[0].Rows[0]["_numbers"].ToString(), mViewer, mCapture);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            picker.Text = "";
            ResetCmbs(2);
            UpdateGrids();
            
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

        ///<summary>
        ///   ####################################    HAMBURGER MENU   ####################################       
        ///</summary>

		*/
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
			Content.Content = new UsersWindow();
        }

        private void Items_Open(object sender, RoutedEventArgs e)
        {
			Content.Content = new ItemsWindow();
        }

        private void Log_Open(object sender, RoutedEventArgs e)
        {
			Content.Content = new LogWindow();
        }
		/*
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
            else if (type == 1)
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                itemsDataGrid.Columns[0].DisplayIndex = itemsDataGrid.Columns.Count - 1;
                //   DataGridCell cell = CommonActions.GetGridCell(gridRow, 1);
                //   Year_numbersItemCmb.Width = cell.ActualWidth;
                //   cell = CommonActions.GetGridCell(gridRow, 2);
                //   FirstNameItemCmb.Width = cell.ActualWidth;
                //   cell = CommonActions.GetGridCell(gridRow, 3);
                //   SecondNameItemCmb.Width = cell.ActualWidth;
                //   cell = CommonActions.GetGridCell(gridRow, 5);
                //   ItemNameCmb.Width = cell.ActualWidth;
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
            else if (type == 2)
            {
                DataGridRow gridRow = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(0);
                if (gridRow == null) return;
                logDataGrid.Columns[0].DisplayIndex = logDataGrid.Columns.Count - 1; //posunutie popisu 
             //   DataGridCell cell = CommonActions.GetGridCell(gridRow, 1);
             //   Item_idLogCmb.Width = cell.ActualWidth;
             //   cell = CommonActions.GetGridCell(gridRow, 2);
             //   Year_numbersLogCmb.Width = cell.ActualWidth;
             //  cell = CommonActions.GetGridCell(gridRow, 3);
             //   TypeCmb.Width = cell.ActualWidth;
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
            LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
            LoadLogGrid(mDbActions.LoadAllLogs());
        }

        private bool ResetCmbs(int type)
        {
            bool changed = false;
            if (type == 0)
            {
                if (Year_numbersSearchCmb.SelectedIndex != 0)
                {
                    changed = true;
                    Year_numbersSearchCmb.SelectedIndex = 0;
                }
                if (FirstNameSearchCmb.SelectedIndex != 0)
                {
                    changed = true;
                    FirstNameSearchCmb.SelectedIndex = 0;
                }
                if (SecondnameSearchCmb.SelectedIndex != 0)
                {
                    changed = true;
                    SecondnameSearchCmb.SelectedIndex = 0;
                }
                if (AddressSearchCmb.SelectedIndex != 0)
                {
                    changed = true;
                    AddressSearchCmb.SelectedIndex = 0;
                }
                if (PhoneSearchCmb.SelectedIndex != 0)
                {
                    changed = true;
                    PhoneSearchCmb.SelectedIndex = 0;
                }
            }
            else if (type == 1)
            {
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
            }
            else if (type == 2)
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

        private void UpdateColumnsWidths(int type)
        {
           // double actWidth = this.ActualWidth - 250;
            if (type == 0)
            {
                dataGrid.Columns[0].Width = 150; //actWidth / 10; //150;   
                dataGrid.Columns[1].Width = 250; // actWidth / 6;//250;
                dataGrid.Columns[2].Width = 250; //actWidth / 6;//250;
                dataGrid.Columns[3].Width = 475; //actWidth / 3.15;//475;
                dataGrid.Columns[4].Width = 250; //actWidth / 6;//250;
                dataGrid.Columns[5].Width = 125; //actWidth / 12;//125;
            }
            else if (type == 1)
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
            else if (type == 2)
            {
                logDataGrid.Columns[0].Width = 150; //actWidth / 10;//150;
                logDataGrid.Columns[1].Width = 150; //actWidth / 10;//150;
                logDataGrid.Columns[2].Width = 150; //actWidth / 10;//150;
                logDataGrid.Columns[3].Width = 150; //actWidth / 10;//150;
                logDataGrid.Columns[4].Width = 650; //actWidth / 2.3;//650;
                logDataGrid.Columns[5].Width = 250; //actWidth / 6;//250;
            }
        }
*/
    }
}



/*      private void Open_Description(object sender, RoutedEventArgs e) //DUPLICATE daff aside
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
      }*/

/*     private void Save_Description(object sender, RoutedEventArgs e)
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
*/
