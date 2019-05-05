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
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Management;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Net;
using WinSCP;
using DatabaseProj;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for User_details.xaml
    /// </summary>
    public partial class User_details : Window
    {
        public Item Item { get; set; }
        public User User { get; set; }

        DbActions mDbActions;
        CommonActions mCommonActions;
        string mYear_key;
        string mNumber_key;
        string mPhoto_path = "";
        Brush defBrush = Brushes.Black;
        static readonly Regex mRegex = new Regex(@"-?\d+(?:\.\d+)?");
        List<int> mVisibleList = new List<int>();
        List<int> mButtonList;
        bool mCloseWin;
        bool mArchived = false;
        enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived };

        ImageViewer mViewer;
        VideoCapture mCapture;

        User mLastSuccesfulUser;
        User mLastUnsuccesfulUser;

        public User_details(EditUserStruct userStruct, ImageViewer view, VideoCapture cap)
        {
            InitializeComponent();
            DataContext = this;
            Init(userStruct, view, cap);
        }

        private void Init(EditUserStruct userStruct, ImageViewer view, VideoCapture cap)
        {
            mViewer = view;
            mCapture = cap;

            mDbActions = new DbActions();
            mCommonActions = new CommonActions();

            icon_add_err.Visibility = Visibility.Hidden;
            icon_edit_err.Visibility = Visibility.Hidden;
            dataGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

            mYear_key = userStruct.keyy;
            mNumber_key = userStruct.keyn;

            User = new User();
            User.IdYear = int.Parse(mYear_key);
            User.IdNumber = int.Parse(mNumber_key);
            User.FName = userStruct.f_name;
            User.SName = userStruct.s_name;
            User.Address = userStruct.address;
            User.Phone = int.Parse(userStruct.tel);

            mLastSuccesfulUser = new User
            {
                FName = userStruct.f_name,
                SName = userStruct.s_name,
                Phone = int.Parse(userStruct.tel),
                Address = userStruct.address
            };

            DataSet data = mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key);
            LoadGrid(data);
            Name_Cmb.Items.Insert(0, "");

        }

        private void Print(object sender, RoutedEventArgs e)
        {

            PdfDocument pdf = new PdfDocument();
            if (File.Exists("~/../../../imageres/zmluva.pdf"))
                pdf.LoadFromFile("~/../../../imageres/zmluva.pdf");
            else
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential("test", "test");
                client.DownloadFile("ftp://dokelu.kst.fri.uniza.sk" + "/imageres/zmluva.pdf", "~/../../../imageres/zmluva.pdf");
                pdf.LoadFromFile("~/../../../imageres/zmluva.pdf");
            }
            PdfPageBase page = pdf.Pages[0];
            PdfFont font = new PdfFont(PdfFontFamily.Courier, 14f);
            PdfFont fontSmall = new PdfFont(PdfFontFamily.Courier, 10f);

            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            page.Canvas.DrawString(/*text_first_name.Text*/User.FName, font, PdfBrushes.Black, new System.Drawing.PointF(40, 80f));
            page.Canvas.DrawString(/*text_second_name.Text*/User.SName, font, PdfBrushes.Black, new System.Drawing.PointF(160, 80f));
            if (/*text_address.Text.Length*/User.Address.Length > 25) page.Canvas.DrawString(/*text_address.Text*/User.Address, fontSmall, PdfBrushes.Black, new System.Drawing.PointF(300, 85f));
            else page.Canvas.DrawString(/*text_address.Text*/User.Address, font, PdfBrushes.Black, new System.Drawing.PointF(300, 80f));
            page.Canvas.DrawString(mYear_key + "-" + mNumber_key, font, PdfBrushes.Black, new System.Drawing.PointF(520, 80f));
            //    page.Canvas.DrawString(datView.Row.ItemArray[0].ToString(), font, PdfBrushes.Black, new System.Drawing.PointF(400, 130f));
            var itemSource = dataGrid.ItemsSource as IEnumerable;
            double fullPrice = 0;
            float marginTop = 167f;
            foreach(var item in dataGrid.ItemsSource)
            {
                // DataRowView rowView = (DataRowView)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                DataRowView rowView = (DataRowView)gridRow.Item;
                page.Canvas.DrawString(rowView.Row.ItemArray[1].ToString(), font, PdfBrushes.Black, new System.Drawing.PointF(200, marginTop));
                page.Canvas.DrawString(rowView.Row.ItemArray[3].ToString(), font, PdfBrushes.Black, new System.Drawing.PointF(520, marginTop));
                fullPrice += Convert.ToDouble(rowView.Row.ItemArray[3].ToString());
                marginTop += 23f;
            }
            page.Canvas.DrawString(fullPrice.ToString(), font, PdfBrushes.Black, new System.Drawing.PointF(520, 147f));
            page.Canvas.DrawString(DateTime.Now.ToShortDateString(), font, PdfBrushes.Black, new System.Drawing.PointF(300, 795f));
            pdf.SaveToFile("doesitwork.pdf");

            PrinterSettings settings = new PrinterSettings();
            string printerName = settings.PrinterName;
            if (printerName == null || printerName == "")
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                foreach (ManagementObject printer in searcher.Get())
                {
                    printerName = printer["Name"].ToString();
                    string availability = printer["Availability"].ToString();
                    //ak je available zober ho not sure ako to bude vyzerat tho :D :D
                }
            }
            else
            {
                pdf.PrintSettings.PrinterName = printerName;
                //if virtual -> pdf.PrintSettings.PrintToFile("PrintToXps.xps");
                pdf.Print();
            }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            User user = User.Copy();

            bool err = false;
            icon_edit_err.Visibility = Visibility.Visible;

           // if (text_first_name.Text == "")
            if (user.FName == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Dopln meno!";
                err = true;
            }
            //if (text_second_name.Text == "")
            if (user.SName == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Dopln priezvisko";
                err = true;
            }

            if (user.Address == "")
            {
                text_edit_err.Foreground = Brushes.Red;
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;
                text_edit_err.Text = "Dopln adresu";
                err = true;
            };

            if (mLastSuccesfulUser.Compare(user))
            {
                icon_edit_err.Visibility = Visibility.Hidden;
                return;
            }

            if (err)
            {
                mLastUnsuccesfulUser = user.Copy();
                return;
            }

            mDbActions.UpdateUser2(user);
            mLastSuccesfulUser = User.Copy();

            text_edit_err.Text = "Uspesne zmenene udaje.";
            icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            text_edit_err.Foreground = Brushes.Green;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            User compStruct = User.Copy();

            bool showPopup = false;

            if (mLastUnsuccesfulUser == null)
            {
                if (!mLastSuccesfulUser.Compare(compStruct))
                {
                    showPopup = true;
                    e.Cancel = true;
                }
            }
            else
            {
                if ((!mLastSuccesfulUser.Compare(compStruct)) && (!mLastUnsuccesfulUser.Compare(compStruct)))
                {
                    showPopup = true;
                    e.Cancel = true;
                }
            }

            if (showPopup)
            {
                HideGrid.Visibility = Visibility.Hidden;
                OnClosePopup.IsOpen = true;
            }
        }

        private void Close_from_Popup(object sender, RoutedEventArgs e)
        {
            OkButton.Visibility = Visibility.Hidden;
            text_popupFline.Text = "Zmenene udaje";
            text_popupSline.Text = "Chcete ulozit ?";
            text_popupwarning.Text = "";
            PopupSave.Visibility = Visibility.Visible;
            PopupDontSave.Visibility = Visibility.Visible;

            if (mCloseWin)
            {
                this.Close();
            }
            else
            {
                OnClosePopup.IsOpen = false;
                HideGrid.Visibility = Visibility.Visible;
            }
        }

        private void Close_Popup(object sender, RoutedEventArgs e)
        {
            User user = User.Copy();

            Button caller = (Button)sender;
            if (caller.Name == "PopupSave")
            {
                Save(sender, e);
                if (icon_edit_err.Kind == MaterialDesignThemes.Wpf.PackIconKind.Error)
                {
                    text_popupSline.Text = "";
                    text_popupFline.Text = "";
                    text_popupwarning.Text = "Nespravne udaje!";
                    text_popupwarning.Foreground = Brushes.Red;
                    PopupSave.Visibility = Visibility.Hidden;
                    PopupDontSave.Visibility = Visibility.Hidden;
                    OkButton.Visibility = Visibility.Visible;
                    mCloseWin = false;
                }
                else
                {
                    text_popupSline.Text = "";
                    text_popupFline.Text = "";
                    text_popupwarning.Text = "Ulozene!";
                    text_popupwarning.Foreground = Brushes.Green;
                    PopupSave.Visibility = Visibility.Hidden;
                    PopupDontSave.Visibility = Visibility.Hidden;
                    OkButton.Visibility = Visibility.Visible;
                    mCloseWin = true;
                }
            }
            else
            {
                OnClosePopup.IsOpen = false;
                mLastSuccesfulUser = user.Copy();
                this.Close();
            }

        }

        private bool CompareUserStruct(EditUserStruct first, EditUserStruct second)
        {
            if (first.f_name != second.f_name)
            {
                return false;
            }
            if (first.s_name != second.s_name)
            {
                return false;
            }
            if (first.address != second.address)
            {
                return false;
            }
            if (first.tel != second.tel)
            {
                return false;
            }

            return true;
        }

        private void Item_Details_Open(object sender, RoutedEventArgs e)
        {
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;

            string userId = mYear_key + "-" + mNumber_key;

            //Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[0].ToString(), text_first_name.Text, text_second_name.Text, userId, mViewer, mCapture);
            Item_details mItemDWindow = new Item_details(datView.Row.ItemArray[0].ToString(), User.FName, User.SName, userId, mViewer, mCapture);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();

            //   LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
            if (Name_Cmb.SelectedIndex != 0)
            {
                Name_Cmb.SelectedIndex = 0;
            } else
            {
                //remove old name in case it changed
                LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
            }
            
        }

        private void SearchItems(object sender, RoutedEventArgs e)
        {
            DataSet data;
            if (Name_Cmb.SelectedItem.ToString() == "")
            {
                data = mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key);
            }
            else
            {
                data = mDbActions.SearchForItemsByName(Name_Cmb.SelectedItem.ToString(), mYear_key, mNumber_key);
            }

            LoadGrid(data);

        }

        private void MkayUpdate(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
        }

        private void Sorted(object sender, DataGridSortingEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                UpdateButtons();
            }, null);
        }

        private void UpdateButtons()
        {
            // dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();

            int counter = 0;
            foreach (var item in dataGrid.Items)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null) ButtonVisibilityEdit(row, mButtonList.ElementAt(counter));
                counter++;
            }

            //dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(0);
            if (gridRow == null) return;

            dataGrid.Columns[0].DisplayIndex = dataGrid.Columns.Count - 1;
            dataGrid.Columns[2].DisplayIndex = dataGrid.Columns.Count - 1;
            dataGrid.Columns[1].DisplayIndex = dataGrid.Columns.Count - 1;

            DataGridCell cell = CommonActions.GetGridCell(gridRow, 3);
            DataGridCell cell2 = CommonActions.GetGridCell(gridRow, 4);
            //cmbMargin += cell.ActualWidth;
            Name_Cmb.Margin = new Thickness(cell.ActualWidth, 15, 0, 0);
            Name_Cmb.Width = cell2.ActualWidth;
        }

        private void ButtonVisibilityEdit(DataGridRow row, int id)
        {
            FrameworkElement element = dataGrid.Columns[2].GetCellContent(row);
            FrameworkElement element2 = dataGrid.Columns[0].GetCellContent(row);
            element.ApplyTemplate();
            element2.ApplyTemplate();
            Button butSellCash = ((DataGridTemplateColumn)dataGrid.Columns[2]).CellTemplate.FindName("btnSellCash", element) as Button;
            Button butSellCard = ((DataGridTemplateColumn)dataGrid.Columns[2]).CellTemplate.FindName("btnSellCard", element) as Button;
            Button butPay = ((DataGridTemplateColumn)dataGrid.Columns[2]).CellTemplate.FindName("btnPay", element) as Button;
            Button butRet = ((DataGridTemplateColumn)dataGrid.Columns[2]).CellTemplate.FindName("btnReturn", element) as Button;
            //TextBox text = ((DataGridTemplateColumn)dataGrid.Columns[0]).CellTemplate.FindName("text_Paid", element2) as TextBox;
            TextBlock text = ((DataGridTemplateColumn)dataGrid.Columns[0]).CellTemplate.FindName("text_Paid", element2) as TextBlock;
            if (id == 0) //stav 0, predat kartou, hotovostou, vratit
            {
                butSellCard.Width = 70;
                butSellCash.Width = 100;
                butRet.Width = 70;

                butPay.Visibility = Visibility.Collapsed;
                butSellCard.Visibility = Visibility.Visible;
                butSellCash.Visibility = Visibility.Visible;
                butRet.Visibility = Visibility.Visible;
                text.Text = "Skladom";
                text.Background = Brushes.LightBlue;
            }

            else if (id == 1) //stav 1 predane kartou  -> vyplatit 
            {
                butSellCard.Visibility = Visibility.Collapsed;
                butSellCash.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;

                butPay.Width = 240;

                butPay.Visibility = Visibility.Visible;
                text.Text = "Karta";
                text.Background = Brushes.LightGreen;

            }
            else if (id == 2) //stav 2 predane hotovostou -> vyplatit
            {
                butSellCard.Visibility = Visibility.Collapsed;
                butSellCash.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;

                butPay.Width = 240;

                butPay.Visibility = Visibility.Visible;
                text.Text = "Hotovost";
                text.Background = Brushes.LightYellow;
                text.Visibility = Visibility.Visible;
            }
            else if (id == 3) //stav 3 vratene
            {
                butSellCash.Visibility = Visibility.Collapsed;
                butSellCard.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;
                butPay.Visibility = Visibility.Collapsed;
                text.Text = "Vratene";
                text.Background = Brushes.LightGray;
                text.Visibility = Visibility.Visible;
            }
            else if (id == 4) //vyplatene karta
            {
                butSellCash.Visibility = Visibility.Collapsed;
                butSellCard.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;
                butPay.Visibility = Visibility.Collapsed;
                text.Text = "Vyplatene Karta";
                text.Background = Brushes.Green;
                text.Visibility = Visibility.Visible;
            }
            else if (id == 5) //vyplatene cash
            {
                butSellCash.Visibility = Visibility.Collapsed;
                butSellCard.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;
                butPay.Visibility = Visibility.Collapsed;
                text.Text = "Vyplatene Hotovost";
                text.Background = Brushes.Yellow;
                text.Visibility = Visibility.Visible;
            }
            else if (id == 6)
            {
                butSellCash.Visibility = Visibility.Collapsed;
                butSellCard.Visibility = Visibility.Collapsed;
                butRet.Visibility = Visibility.Collapsed;
                butPay.Visibility = Visibility.Collapsed;
                text.Text = "Archivovane";
                text.Background = Brushes.OrangeRed;
                text.Visibility = Visibility.Visible;
            }

            dataGrid.Columns[2].Width = 270;
            dataGrid.Columns[1].Width = 200;
            dataGrid.Columns[0].Width = 200;
            dataGrid.Columns[3].Width = 80;
            dataGrid.Columns[4].Width = 150;
            dataGrid.Columns[5].Width = 100;
            dataGrid.Columns[6].Width = 100;
            dataGrid.Columns[7].Width = 400;

        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Item_SellCash(object sender, RoutedEventArgs e)
        {
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.UpdateSpecificItem(datView.Row.ItemArray[0].ToString(), 5, "2");
            mDbActions.AddLog(datView.Row.ItemArray[0].ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar predany hotovostou");
            ButtonVisibilityEdit(gridRow, 2);
        }

        private void Item_SellCard(object sender, RoutedEventArgs e)
        {
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.UpdateSpecificItem(datView.Row.ItemArray[0].ToString(), 5, "1");
            mDbActions.AddLog(datView.Row.ItemArray[0].ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar predany kartou");
            ButtonVisibilityEdit(gridRow, 1);
        }

        private void Item_Return(object sender, RoutedEventArgs e)
        {
            //update na stav 3
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.UpdateSpecificItem(datView.Row.ItemArray[0].ToString(), 5, "3");
            mDbActions.AddLog(datView.Row.ItemArray[0].ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar vrateny");
            ButtonVisibilityEdit(gridRow, 3);
        }

        private void Item_Archive(object sender, RoutedEventArgs e)
        {
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            mDbActions.UpdateSpecificItem(datView.Row.ItemArray[0].ToString(), 6, "True");
            mDbActions.AddLog(datView.Row.ItemArray[0].ToString(), mYear_key + "-" + mNumber_key, 2, "Tovar archivovany");
            LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));

        }

        private void Item_Pay(object sender, RoutedEventArgs e)
        {
            //update na stav 4
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            DataRowView datView = (DataRowView)dataGrid.SelectedItem;
            //get ako zaplatene ->ButtonVisiblityEdit(gridRow, 5);
            if (mDbActions.LoadSpecificItemPaidType(datView.Row.ItemArray[0].ToString()))
            {
                ButtonVisibilityEdit(gridRow, 4);
                mDbActions.UpdateSpecificItem(datView.Row.ItemArray[0].ToString(), 5, "4");
                mDbActions.AddLog(datView.Row.ItemArray[0].ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar zaplateny kartou");
            }
            else
            {
                ButtonVisibilityEdit(gridRow, 5);
                mDbActions.UpdateSpecificItem(datView.Row.ItemArray[0].ToString(), 5, "5");
                mDbActions.AddLog(datView.Row.ItemArray[0].ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar zaplateny hotovostou");
            }
        }

        private void Show_Archived(object sender, RoutedEventArgs e)
        {
            mArchived = true;
            LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
        }

        private void Dont_show_Archived(object sender, RoutedEventArgs e)
        {
            mArchived = false;
            LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
        }

        private void LoadGrid(DataSet gridData)
        {
            dataGrid.ItemsSource = null;
            gridData.Tables[0].Columns.Remove("photo");
            gridData.Tables[0].Columns.Remove("user_year");
            gridData.Tables[0].Columns.Remove("user_numbers");

            int count = gridData.Tables[0].Rows.Count;
            if (!mArchived)
            {
                for (int i = 0; i < count; i++)
                {
                    if (gridData.Tables[0].Rows[i]["archived"].ToString() != "False")
                    {
                        gridData.Tables[0].Rows[i].Delete();
                    }
                }
            }
            gridData.AcceptChanges();

            //SORT

            mButtonList = new List<int>();
            int add;

            foreach (DataRow row in gridData.Tables[0].Rows)    //if archive buttList.Add(6)
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

            gridData.Tables[0].Columns.Remove("archived");
            gridData.Tables[0].Columns.Remove("stav");
            gridData.Tables[0].Columns["name"].ColumnName = "Nazov";
            gridData.Tables[0].Columns["size"].ColumnName = "Velkost";
            gridData.Tables[0].Columns["price"].ColumnName = "Cena";
            gridData.Tables[0].Columns["description"].ColumnName = "Popis";

        //    Name_Cmb.Items.Clear();

            foreach (DataRow row in gridData.Tables[0].Rows)
            {
                if (!Name_Cmb.Items.Contains(row["Nazov"].ToString()))
                {
                    Name_Cmb.Items.Add(row["Nazov"].ToString());
                }
            }

            dataGrid.ItemsSource = gridData.Tables[0].DefaultView;
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            UpdateButtons();
        }

        private new void PreviewTextInput(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "") return;
       /*     if (box.Text[0] == '.')
            {
                box.Text = box.Text.Remove(0, 1);
                return;
            }
            if (box.Text[box.Text.Length - 1] == '.')
            {
                box.Text = box.Text.Remove(box.Text.Length - 1, 1);
                return;
            }
            */
            int dotCounter = 0;
            if (!CommonActions.IsDouble(box.Text))
            {
                for (int i = 0; i < box.Text.Length; i++)
                {
                    if (!int.TryParse(box.Text[i].ToString(), out int outVar))
                    {
                        if (box.Text[i] != '.')
                        {
                            box.Text = box.Text.Remove(i, 1);
                        }
                        else
                        {
                            if (dotCounter > 0 || i == 0 || i == box.Text.Length - 1) box.Text = box.Text.Remove(i, 1);
                            dotCounter++;
                        }
                    }
                }
            }
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
            icon_add_err.Visibility = Visibility.Hidden;
            text_add_err.Text = "";
            DialogHost.IsOpen = false;
        }

        private void TakeAPic(object sender, RoutedEventArgs e)
        {

            mViewer.Image = mCapture.QueryFrame(); //TO DO if throws err
            mViewer.Image.Save("webImage0.png");
            //find available name

            SessionOptions sesOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "dokelu.kst.fri.uniza.sk",
                UserName = "test",
                Password = "test",
            };
            Session ses = new Session();
            ses.Open(sesOptions);

            //if file exists, until it does not. choose that name.
            string imgName = "webImage0.png";
            int id = 0;
            while (ses.FileExists(imgName))
            {
                imgName = new String(imgName.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                id++;
                imgName = imgName.Insert(8, id.ToString());
            }
            string getImage = "webImage0.png";
            string saveImage = "~/../../../imageres/" + imgName;
            if (File.Exists(saveImage))
            {
                File.Delete(saveImage);
            }
            File.Copy(getImage, saveImage);

            mPhoto_path = System.IO.Path.GetFullPath("../../imageres/" + imgName);
            image1.Source = new BitmapImage(new Uri(mPhoto_path, UriKind.RelativeOrAbsolute));
            /* DirectoryInfo di = new DirectoryInfo("~/../../../imageres/");
             FileInfo[] currFiles = di.GetFiles("*.png");

             string imgName = "webImage0.png";
             int id = 0;
             while (File.Exists("~/../../../imageres/" + imgName))
             {
                 imgName = new String(imgName.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                 id++;
                 imgName = imgName.Insert(8, id.ToString());
             }

             string getImage = "webImage0.png";

             string saveImage = "~/../../../imageres/" + imgName;
             File.Copy(getImage, saveImage);
             mPhoto_path = "C://Users/Daniel/source/repos/materialApp/materialApp/imageres/" + imgName;   //TO DO this directory path to config */

            /*        System.Windows.Forms.FolderBrowserDialog filedlg = new System.Windows.Forms.FolderBrowserDialog();
                    System.Windows.Forms.DialogResult result = filedlg.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(filedlg.SelectedPath))
                    {
                        string path = filedlg.SelectedPath;
                    }*/
        }

        private void AddPhotoPath(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                mPhoto_path = dlg.FileName;
                image1.Source = new BitmapImage(new Uri(mPhoto_path, UriKind.RelativeOrAbsolute));
            }
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
            //TO DO Cut imagename from imagePath, store only that, + upload it
            Ftp_Upload(mPhoto_path); //TO DO if not empty
            string[] split = mPhoto_path.Split('/');
            string fileName;
            if (split.Length == 1)
            {
                split = mPhoto_path.Split('\\');
                fileName = split[split.Length - 1];
            }
            else
            {
                fileName = split[split.Length - 1];
            }

            EditItemStruct itemStruct = new EditItemStruct
            {
                keyy = mYear_key,
                keyn = mNumber_key,
                name = text_name.Text,
                description = text_description.Text,
                price = text_price.Text,
                size = text_size.Text,
                photo = fileName,
            };

            mDbActions.AddItem(itemStruct);
            mDbActions.AddLog(mDbActions.GetLastItemId().ToString(), mYear_key + "-" + mNumber_key, 4, "Tovar pridaný");
            text_add_err.Text = "Uspesne pridane.";
            text_add_err.Foreground = Brushes.Green;
            icon_add_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
            mPhoto_path = "";
            Name_Cmb.SelectedIndex = 0;
            image1.Source = null;
            text_size.Text = "";
            text_price.Text = "";
            text_name.Text = "";
            text_description.Text = "";
            LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
            CloseModalAfterAdd();
        }

        private async void CloseModalAfterAdd()
        {
            await Task.Delay(1000);
            icon_add_err.Visibility = Visibility.Hidden;
            text_add_err.Text = "";
            DialogHost.IsOpen = false;
        }

        private void Ftp_Upload(string path)
        {
            if (path != "")
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential("test", "test");
                    string[] split = path.Split('/');
                    string fileName;
                    if (split.Length == 1)
                    {
                        split = path.Split('\\');
                        fileName = split[split.Length - 1];
                    }
                    else
                    {
                        fileName = split[split.Length - 1];
                    }

                    client.UploadFile("ftp://dokelu.kst.fri.uniza.sk/" + fileName, path);
                }
            }
        }
    }
    }

/*  private void Item_Description_Open(object sender, RoutedEventArgs e)
  {
      int index = dataGrid.SelectedIndex;
      DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
      DataRowView rowView = (DataRowView)dataGrid.SelectedItem;

      string id = rowView.Row.ItemArray[0].ToString();
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
          DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);
          presenter.ApplyTemplate();
          var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
          textbox.Text = desc;
          gridRow.DetailsVisibility = Visibility.Visible;
      }
  }

  private void Item_Description_Save(object sender, RoutedEventArgs e)
  {
      int index = dataGrid.SelectedIndex;
      DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
      DataRowView rowView = (DataRowView)dataGrid.SelectedItem;

      string id = rowView.Row.ItemArray[0].ToString();
      DataGridDetailsPresenter presenter = CommonActions.FindVisualChild<DataGridDetailsPresenter>(gridRow);
      presenter.ApplyTemplate();
      var textbox = presenter.ContentTemplate.FindName("Descrip", presenter) as TextBox;
      mDbActions.LoadSaveSpecificItemDescription(id, true, textbox.Text);
  }*/
