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
using System.Net;
using DatabaseProj;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class UsersPage : Page
    {
        private DbActions mDbActions;
        CommonActions mCommonActions;
        VideoCapture mCapture;
        ImageViewer mViewer;

        public UsersPage(DbActions mDbActions, ImageViewer view, VideoCapture capture)
        {
            InitializeComponent();
			this.mDbActions = mDbActions;
            Init(view, capture);
        }

        private void Init(ImageViewer view, VideoCapture capture)
        {
            mCommonActions = new CommonActions();
            DataSet data = mDbActions.LoadAllUsers();
            LoadGrid(data);
            mCapture = capture;
            mViewer = view;

            //icon add err?
            FirstNameSearchCmb.Items.Insert(0, "");
            SecondnameSearchCmb.Items.Insert(0, "");
            Year_numbersSearchCmb.Items.Insert(0, "");
            AddressSearchCmb.Items.Insert(0, "");
            PhoneSearchCmb.Items.Insert(0, "");
        }

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
                UpdateColumnsWidths();
            }
            updateTooltips();
            UpdateCmbox_items(gridData, 0);
        }

        private void UpdateColumnsWidths()
        {
            dataGrid.Columns[0].Width = 150; //actWidth / 10; //150;   
            dataGrid.Columns[1].Width = 250; // actWidth / 6;//250;
            dataGrid.Columns[2].Width = 250; //actWidth / 6;//250;
            dataGrid.Columns[3].Width = 475; //actWidth / 3.15;//475;
            dataGrid.Columns[4].Width = 250; //actWidth / 6;//250;
            dataGrid.Columns[5].Width = 125; //actWidth / 12;//125;
        }

        private void UpdateCmbox_items(DataSet data, int type)
        {
            if (type == 0)
            {
                foreach(DataRow row in data.Tables[0].Rows)
                {
                    if (!FirstNameSearchCmb.Items.Contains(row["Prve meno"].ToString()))
                        FirstNameSearchCmb.Items.Add(row["Prve meno"].ToString());
                    if (!SecondnameSearchCmb.Items.Contains(row["Druhe meno"].ToString()))
                        SecondnameSearchCmb.Items.Add(row["Druhe meno"].ToString());
                    if (!Year_numbersSearchCmb.Items.Contains(row["rok-id"].ToString()))
                        Year_numbersSearchCmb.Items.Add(row["rok-id"].ToString());
                    if (!AddressSearchCmb.Items.Contains(row["Adresa"].ToString()))
                        AddressSearchCmb.Items.Add(row["Adresa"].ToString());
                    if (!PhoneSearchCmb.Items.Contains(row["Telefon"].ToString()))
                        PhoneSearchCmb.Items.Add(row["Telefon"].ToString());
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

        private void Datagrid_Cmb_Update(object sender, RoutedEventArgs e)
		{
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            UpdateColumnsWidths();
            updateTooltips();
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
            mUserDWindow.Owner = Window.GetWindow(this);//this;
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            ResetCmbs(0);
            LoadGrid(mDbActions.LoadAllUsers());
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
            return changed;
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

        public void ModalUserAddInit(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = true;
        }

        public void ModalBack(object sender, RoutedEventArgs e)
        {
            icon_add_err.Visibility = Visibility.Hidden;
            text_add_err.Text = "";
            DialogHost.IsOpen = false;
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

            if (err) return;

            EditUserStruct userStruct = new EditUserStruct
            {
                f_name = text_first_name.Text,
                s_name = text_second_name.Text,
                address = text_address.Text,
                tel = text_tel.Text
            };

            mDbActions.AddUser(userStruct);
            ResetCmbs(0);
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

    }
}