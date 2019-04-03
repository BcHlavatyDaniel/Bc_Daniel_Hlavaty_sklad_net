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
    /// Interaction logic for Item_details.xaml
    /// </summary>
    public partial class Item_details : Window
    {
        DbActions mDbActions;
        string mId;
        DataRowView mDatRow;
        string mPhotoPath = "";

        public Item_details(DataRowView datRow, string id, string f_name, string s_name)
        {
            InitializeComponent();
            Init(datRow, id, f_name, s_name);
        }

        private void Init(DataRowView datRow, string id, string f_name, string s_name)
        {
            mDbActions = new DbActions();
            mDatRow = datRow;

            ChangeSaveVisibility(false);
            text_first_name.IsEnabled = false;
            text_second_name.IsEnabled = false;
            text_first_name.Text = f_name;
            text_second_name.Text = s_name;
            mId = id;
            LoadItem();
            icon_edit_err.Visibility = Visibility.Hidden;
        }

        private void LoadItem()
        {
            DataRow row = mDbActions.LoadItemData(mId).Tables[0].Rows[0];
            text_description.Text = row["description"].ToString();
            text_price.Text = row["price"].ToString();
            text_size.Text = row["size"].ToString();
            // image1.Source = new BitmapImage(new Uri(@"" + row["photo"].ToString(), UriKind.Relative));  
            image1.Source = new BitmapImage(new Uri(row["photo"].ToString(), UriKind.RelativeOrAbsolute));
            if ("1/1/0001 12:00:00 AM" == row["created_at"].ToString()) text_created_at.Text = "-----";
            else text_created_at.Text = row["created_at"].ToString();
            if ("1/1/0001 12:00:00 AM" == row["sold_at"].ToString()) text_sold_at.Text = "-----";
            else text_sold_at.Text = row["sold_at"].ToString();
            if ("1/1/0001 12:00:00 AM" == row["returned_at"].ToString()) text_returned_at.Text = "-----";
            else text_returned_at.Text = row["returned_at"].ToString();
            if ("1/1/0001 12:00:00 AM" == row["paid_at"].ToString()) text_paid_at.Text = "-----";
            else text_paid_at.Text = row["paid_at"].ToString();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            //User_details userWindow = new User_details(mDatRow);
            //userWindow.Show();
            this.Close();
        }

        private void AddPhotoPath(object sender, RoutedEventArgs s)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                mPhotoPath = dlg.FileName;
                image1.Source = new BitmapImage(new Uri(mPhotoPath, UriKind.RelativeOrAbsolute));
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            ChangeSaveVisibility(true);
            icon_edit_err.Visibility = Visibility.Hidden;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            DateTime createTime;
            DateTime.TryParse(picker_created_at.ToString(), out createTime);
            DateTime soldTime;
            DateTime.TryParse(picker_sold_at.ToString(), out soldTime);
            DateTime paidTime;
            DateTime.TryParse(picker_paid_at.ToString(), out paidTime);
            DateTime returnedTime;
            DateTime.TryParse(picker_returned_at.ToString(), out returnedTime);

            icon_edit_err.Visibility = Visibility.Visible;

            double num;
            if (!double.TryParse(text_size.Text, out num))
            {
                text_edit_err.Foreground = Brushes.Red;
                text_edit_err.Text = "Velkost musi byt cislo!";
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                return;
            }
            if (!double.TryParse(text_price.Text, out num))
            {
                text_edit_err.Foreground = Brushes.Red;
                text_edit_err.Text = "Cena musi byt cislo!";
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                return;
            }

            EditItemStruct itemStruct = new EditItemStruct
            {
                id = mId,
                description = text_description.Text,
                price = text_price.Text,
                size = text_size.Text,
                created_at = createTime,
                sold_at = soldTime,
                paid_at = paidTime,
                returned_at = returnedTime,
                photo = mPhotoPath
            };

            mDbActions.EditItemData(itemStruct);
            ChangeSaveVisibility(false);
            ClearOptions();
            LoadItem();
        }

        private void ClearOptions()
        {
            text_edit_err.Text = "Uspesne zmenene";
            text_edit_err.Foreground = Brushes.Green;
            icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            picker_created_at.SelectedDate = null;
            picker_paid_at.SelectedDate = null;
            picker_returned_at.SelectedDate = null;
            picker_sold_at.SelectedDate = null;
        }

        private void ChangeSaveVisibility(bool val)
        {
            if (val)
            {
                BtnSave.Visibility = Visibility.Visible;
                BtnAddPhotoPath.Visibility = Visibility.Visible;
                text_edit_err.Text = "";
                picker_created_at.Visibility = Visibility.Visible;
                picker_paid_at.Visibility = Visibility.Visible;
                picker_returned_at.Visibility = Visibility.Visible;
                picker_sold_at.Visibility = Visibility.Visible;
                text_created_at.Visibility = Visibility.Collapsed;
                text_paid_at.Visibility = Visibility.Collapsed;
                text_sold_at.Visibility = Visibility.Collapsed;
                text_returned_at.Visibility = Visibility.Collapsed;
                text_price.IsEnabled = true;
                text_size.IsEnabled = true;
                text_description.IsEnabled = true;
                text_paid_at.IsEnabled = true;
                text_created_at.IsEnabled = true;
                text_returned_at.IsEnabled = true;
                text_sold_at.IsEnabled = true;

            } else
            {
                BtnSave.Visibility = Visibility.Hidden;
                picker_created_at.Visibility = Visibility.Collapsed;
                picker_paid_at.Visibility = Visibility.Collapsed;
                picker_returned_at.Visibility = Visibility.Collapsed;
                picker_sold_at.Visibility = Visibility.Collapsed;
                text_created_at.Visibility = Visibility.Visible;
                text_paid_at.Visibility = Visibility.Visible;
                text_sold_at.Visibility = Visibility.Visible;
                text_returned_at.Visibility = Visibility.Visible;
                text_price.IsEnabled = false;
                text_size.IsEnabled = false;
                text_description.IsEnabled = false;
                text_paid_at.IsEnabled = false;
                text_created_at.IsEnabled = false;
                text_returned_at.IsEnabled = false;
                text_sold_at.IsEnabled = false;
            }
        }

    }
}
