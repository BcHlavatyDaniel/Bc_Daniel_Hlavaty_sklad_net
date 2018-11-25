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
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;

//TO DO checkout

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        // DatItem mItem;
        Parser mParser;
        string connectStr;
        //  List<string> mShowingDataList;
        //List<DataItem> mShowingDataList;

        public MainWindow()
        {
            InitializeComponent();
            radioButtType1.IsChecked = false;
            radioButtType2.IsChecked = false;
            radioButtType3.IsChecked = false;
            //mShowingDataList = new List<string>();
            //mItem = new DatItem();
          //  mShowingDataList = new List<DataItem>();
            mParser = new Parser();
           // dataGrid.ItemsSource = mShowingDataList;
            // dataGrid.DataContext = mShowingDataList;
            connectStr = "Server=127.0.0.1; port = 3306; Database=bakalarkadb;Uid=root;";   // TO DO config.xml + pwd
            LoadData();
        }

        private void buttAdd_Click(object sender, RoutedEventArgs e)
        {
            //TO DO, never ludom osetri vstupy

            string name = EditName.GetLineText(0);
            string surName = EditSurName.GetLineText(0);

            if (name.Length == 0 || surName.Length == 0)
                return;

            char type = '0';
            if (radioButtType1.IsChecked == true)
            {
                type = '1';

            } else if (radioButtType2.IsChecked == true)
            {
                type = '2';
            } else if (radioButtType3.IsChecked == true)
            {
                type = '3';
            }

            if (type == '0')
                return;

       
            string timeStr = datePicker.ToString();

            double stat;
            try
            {
                stat = Convert.ToDouble(EditStat.GetLineText(0).Replace(',', '.'));
            }
            catch (Exception)
            {
                return;
            }

            string nowTimeStr = DateTime.Now.Date.ToString();

            MySqlConnection connection = new MySqlConnection(connectStr);
            connection.Open(); //TO DO 
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO main(FirstName, SecondName, Stat, StampAdded, StampToLeave, CheckedOut, Type)VALUES(@fName, @sName, @Stat,@StampAdded, @StampToLeave, @checkedOut, @typ)";
                cmd.Parameters.AddWithValue("@fName", name);
                cmd.Parameters.AddWithValue("@sName", surName);
                cmd.Parameters.AddWithValue("@Stat", stat);
                cmd.Parameters.AddWithValue("@StampAdded",nowTimeStr);
                cmd.Parameters.AddWithValue("@StampToLeave",timeStr);
                cmd.Parameters.AddWithValue("@checkedOut", 0);
                cmd.Parameters.AddWithValue("@typ", type);
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    LoadData();
                }
            }

            /*
            DataItem item = new DataItem
            {
                mType = type,
                mFirstName = name,
                mSecondName = surName,
                mStat = stat,
                mTimeStart = DateTime.Now.Date,
                mTimeEnd = time
            };*/

            //mShowingDataList.Add(item);

            //dataGrid.ItemsSource = null;
            //dataGrid.ItemsSource = mShowingDataList;
            
        }

        private void buttDelete_Click(object sender, RoutedEventArgs e)
        {
            /*
            // mShowingDataList.RemoveAt(dataGrid.SelectedIndex);
                
            }*/

            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            int index  = dataGrid.SelectedIndex;

            MySqlConnection connection = new MySqlConnection(connectStr);
            connection.Open(); //..

            try
            {
                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = "DELETE from main WHERE FirstName = @fName AND SecondName = @sName AND Stat = @dStat;";// SecondName Stat Type
                cmd.Parameters.AddWithValue("@fName", row.Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@sName", row.Row.ItemArray[1].ToString());
                cmd.Parameters.AddWithValue("@dStat", row.Row.ItemArray[2].ToString());
                cmd.ExecuteNonQuery();

            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    LoadData();
                }
            }

        }

        private void LoadData()
        {
            MySqlConnection mSql = new MySqlConnection(connectStr);
            mSql.Open(); //Open tiez musi byt v try

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM main";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = data.Tables[0].DefaultView;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open)
                {
                    mSql.Close();
                }
            }
        }
    }
}
