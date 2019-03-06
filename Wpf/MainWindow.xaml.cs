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
            LoadGrid(); //if conn err TO DO
        }

        private void Zmazat_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            mDatabaseAction.DeleteData(row);
            LoadGrid();
        }

        private void Zmenit_Click(object sender, RoutedEventArgs e) //id cant be changed from ui
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            int index = dataGrid.SelectedIndex;
            mDatabaseAction.EditData(row, index);
            LoadGrid();
        }

        private void Pridat_Click(object sender, RoutedEventArgs e)
        {
            AWindow mAddWindow = new AWindow();
            mAddWindow.Show();
            this.Close();
        }

        private void LoadGrid()
        {
            DataSet gridData = mDatabaseAction.LoadData();   //if conn err TO DO
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.CanUserAddRows = false;
        }
    }
}