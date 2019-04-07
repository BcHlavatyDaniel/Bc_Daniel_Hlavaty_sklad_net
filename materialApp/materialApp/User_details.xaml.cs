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
using System.Windows.Controls.Primitives;
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
        List<int> mVisibleList = new List<int>();
        List<int> mButtonList;

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
            dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

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
   
        private void Edit(object sender, RoutedEventArgs e)
        {
            ChangeSaveVisibility(true);
            BtnEdit.Visibility = Visibility.Hidden;
            icon_edit_err.Visibility = Visibility.Hidden;
            text_edit_err.Text = "";
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
            BtnEdit.Visibility = Visibility.Visible;
            text_edit_err.Text = "Uspesne zmenene udaje.";
            icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            text_edit_err.Foreground = Brushes.Green;
        }

        private void Item_Description_Open(object sender, RoutedEventArgs e)
        {
            int index = dataGrid.SelectedIndex;
      //      DataGridRow wataFak = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView rowView = (DataRowView)dataGrid.SelectedItem;
            //  DataRowView datView = (DataRowView)dataGrid.SelectedItem;

            string id = rowView.Row.ItemArray[0].ToString();
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

        private void Item_Details_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;

            Item_details mItemDWindow = new Item_details(mDatRow, datView.Row.ItemArray[0].ToString(), text_first_name.Text, text_second_name.Text);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
        }

        private void Item_Sell(object sender, RoutedEventArgs e)
        {
            //update na stav 1
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.ItemEditTimes(datView.Row.ItemArray[0].ToString(), 0);
            ButtonVisibilityEdit(gridRow, 2);
        }

        private void Item_Return(object sender, RoutedEventArgs e)
        {
            //update na stav 0
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.ItemEditTimes(datView.Row.ItemArray[0].ToString(), 2);
            ButtonVisibilityEdit(gridRow, 0);
        }

        private void Item_Pay(object sender, RoutedEventArgs e)
        {
            //update na stav 2
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.ItemEditTimes(datView.Row.ItemArray[0].ToString(), 1);
            ButtonVisibilityEdit(gridRow, 1);
        }

        private void MkayUpdate(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            int counter = 0;
            foreach (var item in dataGrid.Items)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null) ButtonVisibilityEdit(row, mButtonList.ElementAt(counter));
                counter++;
            }
        }

        private void ButtonVisibilityEdit(DataGridRow row, int id)
        {
            FrameworkElement element = dataGrid.Columns[1].GetCellContent(row);
            element.ApplyTemplate();
            Button butSell = ((DataGridTemplateColumn)dataGrid.Columns[1]).CellTemplate.FindName("btnSell", element) as Button;
            Button butPay = ((DataGridTemplateColumn)dataGrid.Columns[1]).CellTemplate.FindName("btnPay", element) as Button;
            Button butRet = ((DataGridTemplateColumn)dataGrid.Columns[1]).CellTemplate.FindName("btnReturn", element) as Button;
            Button butEdit = ((DataGridTemplateColumn)dataGrid.Columns[1]).CellTemplate.FindName("btnEditGrid", element) as Button;
            TextBox text = ((DataGridTemplateColumn)dataGrid.Columns[1]).CellTemplate.FindName("text_Paid", element) as TextBox;

            if (id == 0) //TO DO scale width accordingly
            { //50 /50 //skladom, da sa predat
                butSell.Width = 120;
                butEdit.Width = 120;
                butSell.Visibility = Visibility.Visible;
                butPay.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;
                text.Visibility = Visibility.Collapsed;
            }
            else if (id == 2)//predane nezaplatene
            { //33/33/33 //
                butSell.Visibility = Visibility.Collapsed;
                butPay.Visibility = Visibility.Visible;
                butRet.Visibility = Visibility.Visible;
                butPay.Width = 80;
                butRet.Width = 80;
                butEdit.Width = 80;
                text.Visibility = Visibility.Collapsed;
            }
            else 
            { //33/33/33 
                butSell.Visibility = Visibility.Collapsed;
                butPay.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Visible;
                text.Visibility = Visibility.Visible;
                butRet.Width = 80;
                text.Width = 80;
                butEdit.Width = 80;
            }
            dataGrid.Columns[1].Width = 265;
        }

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            // TO DO hide id
            gridData.Tables[0].Columns.Remove("photo");
            gridData.Tables[0].Columns.Remove("user_year");
            gridData.Tables[0].Columns.Remove("user_numbers");

            DateTime retTime;
            DateTime paidTime;
            DateTime soldTime;
            DateTime createdTime;
            //   gridData.Tables[0].Columns.Add("stav", typeof(string));
            mButtonList = new List<int>();

            foreach (DataRow row in gridData.Tables[0].Rows)
            {
                DateTime.TryParse(row["returned_at"].ToString(), out retTime);
                DateTime.TryParse(row["paid_at"].ToString(), out paidTime);
                DateTime.TryParse(row["sold_at"].ToString(), out soldTime);
                DateTime.TryParse(row["created_at"].ToString(), out createdTime);

                if (createdTime < retTime)
                {
                    createdTime = retTime;
                }

                if (createdTime > soldTime ) //ak je skladom, nepredany nezaplateny ! inak minimalne predany
                {
                    mButtonList.Add(0); //0 skladom da sa predat
                } else if (createdTime > paidTime)
                {
                    mButtonList.Add(2); //predane nezaplatene
                } else
                {
                    mButtonList.Add(1); //predane, uz len zaplatit
                }
           //     row["stav"] = stav;
            }

            gridData.Tables[0].Columns.Remove("description");
            dataGrid.CanUserAddRows = false;
            gridData.Tables[0].Columns.Remove("created_at");
            gridData.Tables[0].Columns.Remove("returned_at");
            gridData.Tables[0].Columns.Remove("sold_at");
            gridData.Tables[0].Columns.Remove("paid_at");
            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            UpdateButtons();
        }

        ///<summary>
        ///     MODAL
        ///</summary>


        private void ModalItemAddInit(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = true;
        }


        private void ModalBack(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            bool err = false;
            icon_add_err.Visibility = Visibility.Visible;

            if (text_name.Text == "")
            {
                err = true;
                text_add_err.Foreground = Brushes.Red;
                icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_add_err.Text = "Doplna nazov!";
            }

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

            if (err) return;

            EditItemStruct itemStruct = new EditItemStruct
            {
                keyy = year_key,
                keyn = number_key,
                name = text_name.Text,
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
            text_name.Text = "";
            text_description.Text = "";
        }

        /// <summary>
        ///         COMMON
        /// </summary>

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

