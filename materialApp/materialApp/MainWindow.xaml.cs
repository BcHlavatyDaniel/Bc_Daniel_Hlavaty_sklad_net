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
            FirstNameSearchCmb.Items.Add("");
            SecondnameSearchCmb.Items.Add("");
            DataTable dataTable = data.Tables[0];
            foreach (DataRow row in dataTable.Rows)
            {
                if (!FirstNameSearchCmb.Items.Contains(row["first_name"].ToString()))
                    FirstNameSearchCmb.Items.Add(row["first_name"].ToString());
                if (!SecondnameSearchCmb.Items.Contains(row["second_name"].ToString()))
                    SecondnameSearchCmb.Items.Add(row["second_name"].ToString());
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
            LoadGrid(mDbActions.LoadData());
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

        private void Name_Search(object sender, SelectionChangedEventArgs e)
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

            if (FirstNameSearchCmb.SelectedIndex != -1)
            {
                name = FirstNameSearchCmb.SelectedItem.ToString();
            }

            if (SecondnameSearchCmb.SelectedIndex != -1)
            {
                sName = SecondnameSearchCmb.SelectedItem.ToString();
            }

            if (name == "" && sName == "")
            {
                LoadGrid(mDbActions.LoadData());
                return;
            }

            DataSet data = mDbActions.LoadSearchedNamesData(name, sName);
            LoadGrid(data);
           // UpdateCmbItems(data); TO DO
        }

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            gridData.Tables[0].Columns.Add("rok-id", typeof(string));
            gridData.Tables[0].Columns.Remove("created_at");
            foreach(DataRow row in gridData.Tables[0].Rows)
            {
                row["rok-id"] = row["year"] + "-" + row["_numbers"];
            }
            gridData.Tables[0].Columns.Remove("year");
            gridData.Tables[0].Columns.Remove("_numbers");
            gridData.Tables[0].Columns["rok-id"].SetOrdinal(0);
            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.CanUserAddRows = false;
        }

    }
}
