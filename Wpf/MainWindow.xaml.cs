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

//TO DO adapter dispose
namespace Bakalaris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseActions mDatabaseAction;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            mDatabaseAction = new DatabaseActions();  
            DataSet data = mDatabaseAction.LoadData();
            LoadGrid(data);
            DataTable dataTable = data.Tables[0];
            foreach(DataRow row in dataTable.Rows)
            {
                if (!FirstNameSearchCmb.Items.Contains(row["FirstName"].ToString()))
                    FirstNameSearchCmb.Items.Add(row["FirstName"].ToString());
                if (!SecondnameSearchCmb.Items.Contains(row["SecondName"].ToString()))
                    SecondnameSearchCmb.Items.Add(row["SecondName"].ToString());
            }

        }

        private void Zmazat_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            mDatabaseAction.DeleteData(row);
            LoadGrid(mDatabaseAction.LoadData());
        }

        private void Zmenit_Click(object sender, RoutedEventArgs e) //id cant be changed from ui
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            int index = dataGrid.SelectedIndex;
            mDatabaseAction.EditData(row, index);
            LoadGrid(mDatabaseAction.LoadData());
        }

        private void Pridat_Click(object sender, RoutedEventArgs e)
        {
            AWindow mAddWindow = new AWindow();
            mAddWindow.Show();
            this.Close();
        }

        private void Hladat_Click(object sender, RoutedEventArgs e)
        {
            //to do add search by date/skladom
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

            if (name == "" && sName == "") return;

            LoadGrid(mDatabaseAction.LoadSearchedData(name, sName));
        }

        private void Unset_Click(object sender, RoutedEventArgs e)
        {
            FirstNameSearchCmb.SelectedIndex = -1;
            SecondnameSearchCmb.SelectedIndex = -1;
            LoadGrid(mDatabaseAction.LoadData());
        }

        private void Open_Profile(object sender, RoutedEventArgs e)
        {
            //   DataRowView dataRowView = (DataRowView)((Button)e.Source).DataContext;
            //Button btn = (Button)sender;
            int index = dataGrid.SelectedIndex; //
        }

        private void LoadGrid(DataSet gridData)
        {
           //if conn err TO DO
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.CanUserAddRows = false;
        }
    }
}