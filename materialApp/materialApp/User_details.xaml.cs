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
using System.Windows.Shapes;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for User_details.xaml
    /// </summary>
    public partial class User_details : Window
    {
        DbActions mDbActions;
        DataRowView mDatRow;
        string year_key;
        string number_key;
        string photo_path = "";

        public User_details(DataRowView dataRow)
        {
            InitializeComponent();
            Init(dataRow);
        }

        private void Init(DataRowView dataRow)
        {
            mDbActions = new DbActions();
            mDatRow = dataRow;

            BtnSave.Visibility = Visibility.Hidden;
            icon_add_err.Visibility = Visibility.Hidden;
            icon_edit_err.Visibility = Visibility.Hidden;

            year_key = dataRow.Row.ItemArray[0].ToString().Substring(0,2);
            number_key = dataRow.Row.ItemArray[0].ToString().Substring(3, 3); 
            
            text_first_name.Text = dataRow.Row.ItemArray[1].ToString(); //TO DO tu mozu by zmeny ak to nie je initnute z mainW
            text_second_name.Text = dataRow.Row.ItemArray[2].ToString();
            text_address.Text = dataRow.Row.ItemArray[3].ToString();
            text_tel_number.Text = dataRow.Row.ItemArray[4].ToString();

            text_first_name.IsEnabled = false;
            text_second_name.IsEnabled = false;
            text_address.IsEnabled = false;
            text_tel_number.IsEnabled = false;

            DataSet data = mDbActions.LoadUserData(year_key, number_key);
            LoadGrid(data);
        }

        private void Back(object sender, RoutedEventArgs e)
        {
           // MainWindow mWindow = new MainWindow();
           // mWindow.Show();
            this.Close();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            ChangeSaveVisibility(true);
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            bool err = false;
            icon_add_err.Visibility = Visibility.Visible;

            if (text_description.Text == "")
            {
                err = true;
                text_add_err.Foreground = Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln popis!";
            }
            if (text_size.Text == "")
            {
                err = true;
                text_add_err.Foreground = Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln velkost!";
            }
            
            if (text_price.Text == "")
            {
                err = true;
                text_add_err.Foreground = Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Dopln cenu!";
            }

            double num;
            if (!double.TryParse(text_price.Text, out num))
            {
                text_add_err.Text = "Cena musi byt cislo";
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Foreground = Brushes.Red;
                err = true;
            }

            if (!double.TryParse(text_size.Text, out num))
            {
                text_add_err.Text = "Velkost musi byt cislo";
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Foreground = Brushes.Red;
                err = true;
            }

            if (err) return;

            EditItemStruct itemStruct = new EditItemStruct
            {
                keyy = year_key,
                keyn = number_key,
                description = text_description.Text,
                price = text_price.Text,
                size = text_size.Text,
                photo = photo_path
            };

            mDbActions.AddItem(itemStruct);
            DataSet data = mDbActions.LoadUserData(year_key, number_key);
            LoadGrid(data);
            text_add_err.Text = "Uspesne pridane.";
            text_add_err.Foreground = Brushes.Green;
            icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            photo_path = "";
            image1.Source = null;
            text_size.Text = "";
            text_price.Text = "";
            text_description.Text = "";
        }

        private void AddPhotoPath(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                photo_path = dlg.FileName;
                image1.Source = new BitmapImage(new Uri(photo_path, UriKind.RelativeOrAbsolute));
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            //datachecks, result to text_edit_err
            //TO DO limit numeric inputs
            bool err = false;
            icon_edit_err.Visibility = Visibility.Visible;

            if (text_first_name.Text == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Dopln meno!";
                err = true;
            }
            if (text_second_name.Text == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Dopln priezvisko";
                err = true;
            }
            if (text_tel_number.Text == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Dopln tel. cislo";
                err = true;
            }

            if (text_address.Text == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_address.Text = "Dopln adresu";
                err = true;
            }

            int num;
            if (!int.TryParse(text_tel_number.Text, out num))
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Telefonne cislo musi byt cislo";
                err = true;
            }

            if (err) return;

            EditUserStruct userStruct = new EditUserStruct
            {
                keyy = year_key,
                keyn = number_key,
                f_name = text_first_name.Text,
                s_name = text_second_name.Text,
                address = text_address.Text,
                tel = text_tel_number.Text
            };

            mDbActions.EditUserData(userStruct);
            ChangeSaveVisibility(false);
            text_edit_err.Text = "Uspesne zmenene udaje.";
            icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            text_edit_err.Foreground = Brushes.Green;
        }

        private void Item_Details_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            Item_details mItemDWindow = new Item_details(mDatRow ,datView.Row.ItemArray[0].ToString(), text_first_name.Text, text_second_name.Text);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            //mItemDWindow.Show();
            //this.Close();
        }

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            //add stav column TO DO hide id
            gridData.Tables[0].Columns.Remove("photo");
            gridData.Tables[0].Columns.Remove("user_year");
            gridData.Tables[0].Columns.Remove("user_numbers");

            DateTime retTime;
            DateTime paidTime;
            DateTime soldTime;
            string stav = "";
            gridData.Tables[0].Columns.Add("stav", typeof(string));

            foreach(DataRow row in gridData.Tables[0].Rows)
            {
                stav = "";
                DateTime.TryParse(row["returned_at"].ToString(), out retTime);
                DateTime.TryParse(row["paid_at"].ToString(), out paidTime);
                DateTime.TryParse(row["sold_at"].ToString(), out soldTime);

                if ("1/1/0001 12:00:00 AM" != row["paid_at"].ToString())
                {
                    stav += "Zaplatene ";
                }

                if ("1/1/0001 12:00:00 AM" != row["sold_at"].ToString())
                {
                    stav += "Predane ";
                }
                else
                {
                    stav += "Skladom ";
                }

                if ("1/1/0001 12:00:00 AM" != row["returned_at"].ToString())
                {
                    stav += "Vratene ";
                }

                row["stav"] = stav;

            }

            gridData.Tables[0].Columns.Remove("created_at");
            gridData.Tables[0].Columns.Remove("returned_at");
            gridData.Tables[0].Columns.Remove("sold_at");
            gridData.Tables[0].Columns.Remove("paid_at");

            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.CanUserAddRows = false;
        }

        private void ChangeSaveVisibility(bool val)
        {
            if (val)
            {
                BtnSave.Visibility = Visibility.Visible;
                text_first_name.IsEnabled = true;
                text_second_name.IsEnabled = true;
                text_address.IsEnabled = true;
                text_tel_number.IsEnabled = true;
            } else
            {
                BtnSave.Visibility = Visibility.Hidden;
                text_first_name.IsEnabled = false;
                text_second_name.IsEnabled = false;
                text_address.IsEnabled = false;
                text_tel_number.IsEnabled = false;
            }
        }
    }
}
