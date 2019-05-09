using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Data;
using Emgu.CV;
using Emgu.CV.UI;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.ComponentModel;
using System.Net;
using WinSCP;
using DatabaseProj;

namespace materialApp
{
    /// <summary>
    /// Interaction logic for User_details.xaml
    /// </summary>
    public partial class User_details : Window, INotifyPropertyChanged
    {
        DbActions mDbActions;
        string mYear_key;
        string mNumber_key;
        string mPhoto_path = "";
        List<int> mVisibleList = new List<int>();
        List<int> mButtonList;
        bool mCloseWin;
        bool mArchived = false;
        enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived };

        ImageViewer mViewer;
        VideoCapture mCapture;

        User mLastSuccesfulUser;
        User mLastUnsuccesfulUser;

        public Item Item { get; set; }
        public User User { get; set; }
        public Item NewItem { get; set; }
        public List<Item> ItemList { get; set; } 
        public List<string> nameCmbList { get; set; } = new List<string>();

        private int mSelectedName;
        public int selectedName
        {
            get
            {
                return mSelectedName;
            }
            set
            {
                mSelectedName = value;
                NotifyPropertyChanged("selectedName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public User_details(User userStruct, ImageViewer view, VideoCapture cap)
        {
            InitializeComponent();
            DataContext = this;
            DialogHost.DataContext = this;
            Init(userStruct, view, cap);
        }

        private void Init(User userStruct, ImageViewer view, VideoCapture cap)
        {
            mViewer = view;
            mCapture = cap;

            mDbActions = new DbActions();

            mYear_key = userStruct.IdYear.ToString();
            mNumber_key = userStruct.IdNumber.ToString();

            User = new User();
            User.IdYear = int.Parse(mYear_key);
            User.IdNumber = int.Parse(mNumber_key);
            User.FName = userStruct.FName;
            User.SName = userStruct.SName;
            User.Address = userStruct.Address;
            User.Phone = userStruct.Phone;
            User.Icon_Visibility = "Hidden";
            NewItem = new Item();
            NewItem.UserId = mYear_key + "-" + mNumber_key;

            mLastSuccesfulUser = new User
            {
                FName = userStruct.FName,
                SName = userStruct.SName,
                Phone = userStruct.Phone,
                Address = userStruct.Address
            };

            DataSet data = mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key);
            ItemList = new List<Item>();
            LoadGrid(data);
            nameCmbList.Insert(0, "");
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
            page.Canvas.DrawString(User.FName, font, PdfBrushes.Black, new System.Drawing.PointF(40, 80f));
            page.Canvas.DrawString(User.SName, font, PdfBrushes.Black, new System.Drawing.PointF(160, 80f));
            if (User.Address.Length > 25) page.Canvas.DrawString(User.Address, fontSmall, PdfBrushes.Black, new System.Drawing.PointF(300, 85f));
            else page.Canvas.DrawString(User.Address, font, PdfBrushes.Black, new System.Drawing.PointF(300, 80f));
            page.Canvas.DrawString(mYear_key + "-" + mNumber_key, font, PdfBrushes.Black, new System.Drawing.PointF(520, 80f));
            var itemSource = dataGrid.ItemsSource as IEnumerable;
            double fullPrice = 0;
            float marginTop = 167f;
            foreach (var item in dataGrid.ItemsSource)
            {
                DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                Item rowItem = (Item)gridRow.Item;
                page.Canvas.DrawString(rowItem.Name, font, PdfBrushes.Black, new System.Drawing.PointF(200, marginTop));
                page.Canvas.DrawString(rowItem.Price.ToString(), font, PdfBrushes.Black, new System.Drawing.PointF(520, marginTop));
                fullPrice += rowItem.Price;
                marginTop += 23f;
            }
            page.Canvas.DrawString(fullPrice.ToString(), font, PdfBrushes.Black, new System.Drawing.PointF(520, 147f));
            page.Canvas.DrawString(DateTime.Now.ToShortDateString(), font, PdfBrushes.Black, new System.Drawing.PointF(300, 795f));
            pdf.SaveToFile("doesitwork.pdf");

            Print_Details print_details = new Print_Details(pdf);
            print_details.Owner = GetWindow(this);
            print_details.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            print_details.ShowDialog();
            
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            User user = User.Copy();

            bool err = false;
            User.Icon_Visibility = "Visible";

            if (user.FName == "")
            {
                User.Color = "Red";
                User.Kind = "Error";
                User.Edit_Text = "Dopln meno!";
                err = true;
            }

            if (user.SName == "")
            {
                User.Color = "Red";
                User.Kind = "Error";
                User.Edit_Text = "Dopln priezvisko!";
                err = true; ;
            }

            if (user.Address == "")
            {
                User.Color = "Red";
                User.Kind = "Error";
                User.Edit_Text = "Dopln adresu!";
                err = true;
            };

            if (mLastSuccesfulUser.Compare(user))
            {
                User.Icon_Visibility = "Hidden";
                return;
            }

            if (err)
            {
                mLastUnsuccesfulUser = user.Copy();
                return;
            }

            mDbActions.UpdateUser2(user);
            mLastSuccesfulUser = User.Copy();
            
            User.Edit_Text = "Uspesne zmenene udaje";
            User.Kind = "Done";
            User.Color = "Green";
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
                if (user.Kind == "Error")
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

        private void SearchItems(object sender, RoutedEventArgs e)
        {
            DataSet data;
            if (selectedName == 0)
            {
                data = mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key);
            }
            else
            {
                data = mDbActions.SearchForItemsByName(nameCmbList.ElementAt(selectedName), mYear_key, mNumber_key);
            }

            LoadGrid(data);

        }

        private void Close_Window(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Item_Details_Open(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)dataGrid.SelectedItem;
            Item_details mItemDWindow = new Item_details(currItem.Id.ToString(), User.FName, User.SName, mYear_key + "-" + mNumber_key, mViewer, mCapture);
            mItemDWindow.Owner = this;
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();

            if (selectedName != 0) selectedName = 0;
            else LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
        }

        private void Item_SellCash(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)dataGrid.SelectedItem;
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            mDbActions.UpdateSpecificItem(currItem.Id.ToString(),5,"2");
            mDbActions.AddLog(currItem.Id.ToString(),mYear_key + "-" + mNumber_key, 1, "Tovar predany hotovostou");
            ButtonEdit(2, ref currItem);
        }

        private void Item_SellCard(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)dataGrid.SelectedItem;
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            mDbActions.UpdateSpecificItem(currItem.Id.ToString(), 5, "1");
            mDbActions.AddLog(currItem.Id.ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar predany kartou");
            ButtonEdit(1, ref currItem);
        }

        private void Item_Return(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)dataGrid.SelectedItem;
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            mDbActions.UpdateSpecificItem(currItem.Id.ToString(), 5, "3");
            mDbActions.AddLog(currItem.Id.ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar vrateny");
            ButtonEdit(3, ref currItem);
        }

        private void Item_Pay(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)dataGrid.SelectedItem;
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            if (mDbActions.LoadSpecificItemPaidType(currItem.Id.ToString()))
            {
                ButtonEdit(4, ref currItem);
                mDbActions.UpdateSpecificItem(currItem.Id.ToString(), 5, "4");
                mDbActions.AddLog(currItem.Id.ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar zaplateny kartou");
            }
            else
            {
                ButtonEdit(5, ref currItem);
                mDbActions.UpdateSpecificItem(currItem.Id.ToString(), 5, "5");
                mDbActions.AddLog(currItem.Id.ToString(), mYear_key + "-" + mNumber_key, 1, "Tovar zaplateny hotovostou");
            }
        }

        private void ButtonEdit(int type, ref Item curItem)
        {
            switch(type)
            {
                case 1:
                    {
                        curItem.StateText = "Karta";
                        curItem.StateColor = "LightGreen";
                        curItem.SellCardVisibility = "Collapsed";
                        curItem.SellCashVisibility = "Collapsed";
                        curItem.ReturnVisibility = "Collapsed";
                        curItem.PayVisibility = "Visible";
                        break;
                    }
                case 2:
                    {
                        curItem.StateText = "Hotovost";
                        curItem.StateColor = "LightYellow";
                        curItem.SellCardVisibility = "Collapsed";
                        curItem.SellCashVisibility = "Collapsed";
                        curItem.ReturnVisibility = "Collapsed";
                        curItem.PayVisibility = "Visible";
                        break;
                    }
                case 3:
                    {
                        curItem.StateText = "Vratene";
                        curItem.StateColor = "LightGray";
                        curItem.SellCardVisibility = "Collapsed";
                        curItem.SellCashVisibility = "Collapsed";
                        curItem.ReturnVisibility = "Collapsed";
                        curItem.PayVisibility = "Collapsed";
                        break;
                    }
                case 4:
                    {
                        curItem.StateText = "Vyplatene Karta";
                        curItem.StateColor = "Green";
                        curItem.SellCardVisibility = "Collapsed";
                        curItem.SellCashVisibility = "Collapsed";
                        curItem.ReturnVisibility = "Collapsed";
                        curItem.PayVisibility = "Collapsed";
                        break;
                    }
                case 5:
                    {
                        curItem.StateText = "Vyplatene Hotovost";
                        curItem.StateColor = "Yellow";
                        curItem.SellCardVisibility = "Collapsed";
                        curItem.SellCashVisibility = "Collapsed";
                        curItem.ReturnVisibility = "Collapsed";
                        curItem.PayVisibility = "Collapsed";
                        break;
                    }
            }
        }

        private void Item_Archive(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)dataGrid.SelectedItem;
            DataGridRow gridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem);
            mDbActions.UpdateSpecificItem(currItem.Id.ToString(), 6, "True");
            mDbActions.AddLog(currItem.Id.ToString(), mYear_key + "-" + mNumber_key, 2, "Tovar archivovany");
            LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));

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

            mButtonList = new List<int>();
            int add;
            string stateText = "";
            string color = "";
            string bSellCash = "";
            string bSellCard = "";
            string bReturn = "";
            string bPay = "";

            ItemList.Clear();
            foreach (DataRow row in gridData.Tables[0].Rows)
            {
                if (row["archived"].ToString() == "True")
                {
                    mButtonList.Add((int)mState.Earchived);
                    add = (int)mState.Earchived;
                }
                else
                {
                    int.TryParse(row["stav"].ToString(), out add);
                    mButtonList.Add(add);
                }

                if (!nameCmbList.Contains(row["name"].ToString()))
                {
                    nameCmbList.Add(row["name"].ToString());
                }

                switch(add)
                {
                    case 0:
                        {
                            color = "";
                            stateText = "Skladom";
                            bSellCard = "Visible";
                            bSellCash = "Visible";
                            bReturn = "Visible";
                            bPay = "Collapsed";
                            break;
                        }
                    case 1:
                        {
                            color = "LimeGreen";
                            stateText = "Karta";
                            bSellCard = "Collapsed";
                            bSellCash = "Collapsed";
                            bReturn = "Collapsed";
                            bPay = "Visible";
                            break;
                        }
                    case 2:
                        {
                            color = "LightYellow";
                            stateText = "Hotovost";
                            bSellCard = "Collapsed";
                            bSellCash = "Collapsed";
                            bReturn = "Collapsed";
                            bPay = "Visible";
                            break;
                        }
                    case 3:
                        {
                            color = "LightGray";
                            stateText = "Vratene";
                            bSellCard = "Collapsed";
                            bSellCash = "Collapsed";
                            bReturn = "Collapsed";
                            bPay = "Collapsed";
                            break;
                        }
                    case 4:
                        {
                            color = "Green";
                            stateText = "Vyplatene karta";
                            bSellCard = "Collapsed";
                            bSellCash = "Collapsed";
                            bReturn = "Collapsed";
                            bPay = "Collapsed";
                            break;
                        }
                    case 5:
                        {
                            color = "Yellow";
                            stateText = "Vyplatene hotovost";
                            bSellCard = "Collapsed";
                            bSellCash = "Collapsed";
                            bReturn = "Collapsed";
                            bPay = "Collapsed";
                            break;
                        }
                    case 6:
                        {
                            color = "OrangeRed";
                            stateText = "Archivovane";
                            bSellCard = "Collapsed";
                            bSellCash = "Collapsed";
                            bReturn = "Collapsed";
                            bPay = "Collapsed";
                            break;
                        }
                }

                ItemList.Add(new Item
                {
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    Size = row["size"].ToString(),
                    Price = double.Parse(row["price"].ToString()),
                    Description = row["description"].ToString(),
                    StateText = stateText,
                    StateColor = color,
                    SellCashVisibility = bSellCash,
                    SellCardVisibility = bSellCard,
                    ReturnVisibility = bReturn,
                    PayVisibility = bPay
                });
            }

            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
        }

        private new void PreviewTextInput(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text == "") return;
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
            NewItem.Add_Text = "";
            NewItem.Description = "";
            NewItem.Name = "";
            NewItem.UserFName = ""; 
            NewItem.Size = "";
            NewItem.Photo = "";
            NewItem.Kind = "Done";
            NewItem.Icon_Visibility = "Hidden";
            DialogHost.IsOpen = true;
        }

        private void ModalBack(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
        }

        private void TakeAPic(object sender, RoutedEventArgs e)
        {
            mViewer.Image = mCapture.QueryFrame(); 
            mViewer.Image.Save("webImage0.png");

            SessionOptions sesOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "dokelu.kst.fri.uniza.sk",
                UserName = "test",
                Password = "test",
            };
            Session ses = new Session();
            ses.Open(sesOptions);

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
            NewItem.Photo = new BitmapImage(new Uri(mPhoto_path, UriKind.RelativeOrAbsolute)).ToString();
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
                NewItem.Photo = new BitmapImage(new Uri(mPhoto_path, UriKind.RelativeOrAbsolute)).ToString();
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            bool err = false;
            Item currItem = NewItem.Copy();
            NewItem.Icon_Visibility = "Visible";

            if (currItem.Name == "")
            {
                err = true;
                NewItem.Color = "Red";
                NewItem.Kind = "Error";
                NewItem.Add_Text = "Dopln nazov!";
            }

            if (currItem.Description == "")
            {
                currItem.Description = " ";
            }

            if (currItem.Size == "")
            {
                err = true;
                NewItem.Color = "Red";
                NewItem.Kind = "Error";
                NewItem.Add_Text = "Dopln velkost!";
            }

            if (currItem.UserFName == "")
            {
                err = true;
                NewItem.Color = "Red";
                NewItem.Kind = "Error";
                NewItem.Add_Text = "Dopln cenu!";
            }

            double num;
            if (!double.TryParse(currItem.UserFName, out num))
            {
                NewItem.Color = "Red";
                NewItem.Kind = "Error";
                NewItem.Add_Text = "Cena musi byt cislo!";
                err = true;
            }

            if (err) return;
            Ftp_Upload(mPhoto_path); 
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

            mDbActions.AddItem(currItem);
            mDbActions.AddLog(mDbActions.GetLastItemId().ToString(), mYear_key + "-" + mNumber_key, 4, "Tovar pridaný");
            NewItem.Add_Text = "Uspesne pridane.";
            NewItem.Color = "Green";
            NewItem.Kind = "Done";
            mPhoto_path = "";
            selectedName = 0;
            NewItem.Photo = "";
            LoadGrid(mDbActions.SearchForItemsByUserkeys(mYear_key, mNumber_key));
            CloseModalAfterAdd();
        }

        private async void CloseModalAfterAdd()
        {
            await Task.Delay(1000);
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