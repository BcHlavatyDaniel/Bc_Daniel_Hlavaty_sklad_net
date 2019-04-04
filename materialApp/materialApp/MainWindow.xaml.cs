using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbActions mDbActions;
        bool hamClosed = true;

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

            itemsGrid.Visibility = Visibility.Collapsed;

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

        private void Users_Open(object sender, RoutedEventArgs e)
        {
            //LoadGrid(mDbActions.LoadData());
            itemsGrid.Visibility = Visibility.Collapsed;
            usersGrid.Visibility = Visibility.Visible;
        }

        private void Items_Open(object sender, RoutedEventArgs e)
        {
            usersGrid.Visibility = Visibility.Collapsed;
            itemsGrid.Visibility = Visibility.Visible;
        }

        private void Profile_Open(object sender, RoutedEventArgs e)
        {
            User_details mUserDWindow = new User_details((DataRowView)dataGrid.SelectedItem);
            //mUserDWindow.Show();
            mUserDWindow.Owner = this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            //this.Close();
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
       /*     DependencyObject dpobj = sender as DependencyObject;
            string caller = dpobj.GetValue(FrameworkElement.NameProperty) as string;
            if (caller == "FirstNameSearchCmb")         
            {

            }
            else
            {

            }
         */   
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
           // UpdateCmbItems(data); TO DO
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

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            gridData.Tables[0].Columns.Add("rok-id", typeof(string));
            gridData.Tables[0].Columns.Add("pocet tovaru", typeof(int));
            gridData.Tables[0].Columns.Remove("created_at"); // TO DO +kolko ma tovaru
            foreach(DataRow row in gridData.Tables[0].Rows)
            {
                row["rok-id"] = row["year"] + "-" + row["_numbers"];
                row["pocet tovaru"] = mDbActions.GetNumberOfItemsForUser(row["year"].ToString(), row["_numbers"].ToString());
            }
            gridData.Tables[0].Columns.Remove("year");
            gridData.Tables[0].Columns.Remove("_numbers");
            gridData.Tables[0].Columns["rok-id"].SetOrdinal(0);
            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.CanUserAddRows = false;
        }

        private void LoadItemsGrid(DataSet allItems)
        {
            //DataSet dSet = mDbActions.LoadData();
            //get all items + names -> do i wanna go to uzivatel detail from here?

            //DataSet allItems = mDbActions.LoadAllItems();
            DataSet allUsers = mDbActions.LoadData();
            //TO DO
            itemsDataGrid.ItemsSource = null;
            allItems.Tables[0].Columns.Add("rok-id", typeof(string));
            allItems.Tables[0].Columns.Add("prve meno", typeof(string));
            allItems.Tables[0].Columns.Add("druhe meno", typeof(string));
            allItems.Tables[0].Columns.Remove("created_at");
            allItems.Tables[0].Columns.Remove("sold_at");
            allItems.Tables[0].Columns.Remove("paid_at");
            allItems.Tables[0].Columns.Remove("returned_at");

            for (int i = allItems.Tables[0].Rows.Count -1; i >= 0; i--)
            {
                if (allItems.Tables[0].Rows[i]["is_Stored"].ToString() == "False")
                {
                    allItems.Tables[0].Rows[i].Delete();
                }
            }
            allItems.Tables[0].AcceptChanges();

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
            allItems.Tables[0].Columns.Remove("is_Stored");
            allItems.Tables[0].Columns.Remove("photo");
            allItems.Tables[0].Columns["rok-id"].SetOrdinal(0);
            allItems.Tables[0].Columns["prve meno"].SetOrdinal(1);
            allItems.Tables[0].Columns["druhe meno"].SetOrdinal(2);
            itemsDataGrid.ItemsSource = allItems.Tables[0].DefaultView;
            itemsDataGrid.CanUserAddRows = false;
        }
    }
}
