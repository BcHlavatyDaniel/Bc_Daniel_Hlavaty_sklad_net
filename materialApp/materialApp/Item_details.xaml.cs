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
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.IO;
using System.Net;
using WinSCP;
namespace materialApp
{
    /// <summary>
    /// Interaction logic for Item_details.xaml
    /// </summary>
    public partial class Item_details : Window
    {
        DbActions mDbActions;
        CommonActions mCommonActions;
        string mId;
        string mUserId;
        string mPhotoPath = "";
        bool mCloseWin;
        int mCurrState = 0;
        bool mCurrArch = false;
        enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived }

        EditItemStruct mLastSuccessStruct;
        EditItemStruct mLastUnsuccessStruct;

        ImageViewer mViewer;
        VideoCapture mCapture;

        public Item_details(string id, string f_name, string s_name, string user_id, ImageViewer view, VideoCapture cap)
        {
            InitializeComponent();
            Init( id, f_name, s_name, user_id, view, cap);
        }

        private void Init(string id, string f_name, string s_name, string user_id, ImageViewer view, VideoCapture cap)
        {
            mDbActions = new DbActions();
            mCommonActions = new CommonActions();

            mViewer = view;
            mCapture = cap;

            text_first_name.Text = f_name;
            text_second_name.Text = s_name;
            text_user_id.Text = user_id;
            mUserId = user_id;
            mId = id;
            cmbChangeItemState.Items.Add("Nepredany"); //0
            cmbChangeItemState.Items.Add("Predany karta"); //1
            cmbChangeItemState.Items.Add("Predany Hotovost"); //2
            cmbChangeItemState.Items.Add("Vrateny"); //3
            cmbChangeItemState.Items.Add("Zaplateny Karta"); //4
            cmbChangeItemState.Items.Add("Zaplateny Hotovost"); //5
            LoadItem();
            icon_edit_err.Visibility = Visibility.Hidden;
            text_edit_err.Text = "";
        }

        private void LoadItem()
        {
            DataRow row = mDbActions.LoadSpecificItem(mId).Tables[0].Rows[0];
            text_description.Text = row["description"].ToString();
            text_price.Text = row["price"].ToString();
            text_size.Text = row["size"].ToString();
            text_name.Text = row["name"].ToString();

            if (row["archived"].ToString() == "True")
            {
                mCurrArch = true;
                archiveBox.IsChecked = true;
            }
            else
            {
                mCurrArch = false;
                archiveBox.IsChecked = false;
            }

            int type;
            int.TryParse(row["stav"].ToString(), out type);
            cmbChangeItemState.SelectedIndex = type;

            mCurrState = type;

            //if file does not exist locally, download it
            mPhotoPath = row["photo"].ToString();
            if (mPhotoPath != "")
            {
                if (!File.Exists("~/../../../imageres/" + mPhotoPath))
                {
                    Ftp_Download(mPhotoPath);
                }
                string uri = System.IO.Path.GetFullPath("../../imageres/" + mPhotoPath);
                image1.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            }


            mLastSuccessStruct = new EditItemStruct
            {
                description = row["description"].ToString(),
                photo = row["photo"].ToString(),
                name = row["name"].ToString() ,
                size = row["size"].ToString(),
                price = row["price"].ToString(),
            };
        }

        private void TakeAPic(object sender, RoutedEventArgs e)
        {
            mViewer.Image = mCapture.QueryFrame(); //TO DO if throws err
            mViewer.Image.Save("webImage0.png"); // -> odtialto ho skopcit do imageres, nazov +id
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
            while(ses.FileExists(imgName))
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

            mPhotoPath = System.IO.Path.GetFullPath("../../imageres/" + imgName);
            image1.Source = new BitmapImage(new Uri(mPhotoPath, UriKind.RelativeOrAbsolute));
            /*  DirectoryInfo di = new DirectoryInfo("~/../../../imageres/");
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
              mPhotoPath = "C://Users/Daniel/source/repos/materialApp/materialApp/imageres/" + imgName;   //TO DO this directory path to config
              */                                                                                            // image1.Source = new BitmapImage(new Uri(photo_path, UriKind.RelativeOrAbsolute));
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

        private void Save(object sender, RoutedEventArgs e)
        {
            
            double num;

            EditItemStruct itemStruct = new EditItemStruct
            {
                description = text_description.Text,
                price = text_price.Text,
                size = text_size.Text,
                name = text_name.Text,
                photo = mPhotoPath
            };

            if (mLastUnsuccessStruct != null)
            {
                if (CompareItemStruct(itemStruct, mLastUnsuccessStruct) && mCurrArch == archiveBox.IsChecked && mCurrState == cmbChangeItemState.SelectedIndex) return;
            }
            if (CompareItemStruct(itemStruct, mLastSuccessStruct) && mCurrArch == archiveBox.IsChecked && mCurrState == cmbChangeItemState.SelectedIndex) return;

            if (!double.TryParse(text_price.Text, out num))
            {
                icon_edit_err.Visibility = Visibility.Visible;
                text_edit_err.Foreground = Brushes.Red;
                text_edit_err.Text = "Cena musi byt cislo!";
                icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Error;

                mLastUnsuccessStruct = new EditItemStruct
                {
                    description = text_description.Text,
                    photo = mPhotoPath,
                    name = text_name.Text,
                    size = text_size.Text,
                    price = text_price.Text
                };

                return;
            }

            string changeString = "";
            int logType = 0;

            if (archiveBox.IsChecked != mCurrArch)
            {
                mDbActions.UpdateSpecificItem(mId, 6, archiveBox.IsChecked.ToString());
                if (archiveBox.IsChecked == true) changeString = "Tovar archivovany";
                else changeString = "Tovar odarchivovany";
                logType = 2;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }
            if (cmbChangeItemState.SelectedIndex != mCurrState)
            {
                mDbActions.UpdateSpecificItem(mId, 5, cmbChangeItemState.SelectedIndex.ToString());
                changeString = "Zmena stavu z ";   
                if (mCurrState == 0)
                {
                    changeString += "nepredany";
                } else if (mCurrState == 1)
                {
                    changeString += "predany hotovostou";
                } else if (mCurrState == 2)
                {
                    changeString += "predany kartou";
                } else if (mCurrState == 3)
                {
                    changeString += "vrateny";
                } else if (mCurrState == 4)
                {
                    changeString += "zaplateny hotovostou";
                } else if (mCurrState == 5)
                {
                    changeString += "zaplateny kartou";
                }
                changeString += " na ";
                if (cmbChangeItemState.SelectedIndex == 0)
                {
                    changeString += "nepredany";
                }
                else if (cmbChangeItemState.SelectedIndex == 1)
                {
                    changeString += "predany hotovostou";
                }
                else if (cmbChangeItemState.SelectedIndex == 2)
                {
                    changeString += "predany kartou";
                }
                else if (cmbChangeItemState.SelectedIndex == 3)
                {
                    changeString += "vrateny";
                }
                else if (cmbChangeItemState.SelectedIndex == 4)
                {
                    changeString += "zaplateny hotovostou";
                }
                else if (cmbChangeItemState.SelectedIndex == 5)
                {
                    changeString += "zaplateny kartou";
                }

                logType = 1;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }
            if (mPhotoPath != mLastSuccessStruct.photo)
            {
                //photoPath to ftp, + fpt link to photoPath 
                Ftp_Upload(mPhotoPath);
                string[] split = mPhotoPath.Split('/');
                string fileName;
                if (split.Length == 1)
                {
                    split = mPhotoPath.Split('\\');
                    fileName = split[split.Length - 1];
                }
                else
                {
                    fileName = split[split.Length - 1];
                }

                mDbActions.UpdateSpecificItem(mId, 4, fileName);
                changeString = "Zmena obrazku.";
                logType = 3;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }
            if (text_name.Text != mLastSuccessStruct.name)
            {
                mDbActions.UpdateSpecificItem(mId, 3, text_name.Text);
                changeString = "Zmena nazvu tovaru z " + mLastSuccessStruct.name + " na " + text_name.Text;
                logType = 3;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }
            if (text_price.Text != mLastSuccessStruct.price)
            {
                mDbActions.UpdateSpecificItem(mId, 2, text_price.Text);
                changeString = "Zmena ceny tovaru z " + mLastSuccessStruct.price + " na " + text_price.Text;
                logType = 0;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }
            if (text_size.Text != mLastSuccessStruct.size)
            {
                mDbActions.UpdateSpecificItem(mId, 1, text_size.Text);
                changeString = "Zmena velkosti tovaru z " + mLastSuccessStruct.size + " na " + text_size.Text;
                logType = 3;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }
            if (text_description.Text != mLastSuccessStruct.description)
            {
                mDbActions.UpdateSpecificItem(mId, 0, text_description.Text);
                changeString = "Zmena popisu z " + mLastSuccessStruct.description + " na " + text_description.Text;
                logType = 3;
                mDbActions.AddLog(mId, mUserId, logType, changeString);
            }

            if (changeString != "")
            {
                icon_edit_err.Visibility = Visibility.Visible;

                mLastSuccessStruct = new EditItemStruct
                {
                    description = text_description.Text,
                    photo = mPhotoPath,
                    name = text_name.Text,
                    size = text_size.Text,
                    price = text_price.Text
                };

                mCurrState = cmbChangeItemState.SelectedIndex;

                if (archiveBox.IsChecked == true) mCurrArch = true;
                else mCurrArch = false;

                ClearOptions();
                LoadItem();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EditItemStruct compStruct = new EditItemStruct
            {
                name = text_name.Text,
                size = text_size.Text,
                price = text_price.Text,
                description = text_description.Text,
                photo = mPhotoPath
            };

            bool showPopup = false;

            if (mLastUnsuccessStruct == null)
            {
                if (!CompareItemStruct(mLastSuccessStruct, compStruct))
                {
                    showPopup = true;
                    e.Cancel = true;
                }
            }
            else
            {
                if (!CompareItemStruct(mLastSuccessStruct, compStruct) && (!CompareItemStruct(mLastUnsuccessStruct, compStruct)))
                {
                    showPopup = true;
                    e.Cancel = true;
                }
            }

            if (cmbChangeItemState.SelectedIndex != mCurrState)
            {
                showPopup = true;
                e.Cancel = true;
            }

            if (archiveBox.IsChecked != mCurrArch)
            {
                showPopup = true;
                e.Cancel = true;
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
            text_popupSline.Text = "Chcete ulozit?";
            text_popupwarning.Text = "";
            PopupSave.Visibility = Visibility.Visible;
            PopupDontSave.Visibility = Visibility.Visible;

            if (mCloseWin)
            {
                this.Close();
            }
            else
            {
                OnClosePopup.IsOpen = true;
                HideGrid.Visibility = Visibility.Visible;
            }

        }

        private void Close_Popup(object sender, RoutedEventArgs e)
        {
            Button caller = (Button)sender;
            if (caller.Name == "PopupSave")
            {
                Save(sender, e);
                if (icon_edit_err.Kind == MaterialDesignThemes.Wpf.PackIconKind.Error)
                {
                    text_popupSline.Text = "";
                    text_popupFline.Text = "";
                    text_popupwarning.Text = "Nespravne udaje";
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
                OnClosePopup.IsOpen = false;    //TO DO isnt closing 
                mLastSuccessStruct.name = text_name.Text;
                mLastSuccessStruct.description = text_description.Text;
                mLastSuccessStruct.size = text_size.Text;
                mLastSuccessStruct.price = text_price.Text;
                mLastSuccessStruct.photo = mPhotoPath;
                mCurrState = cmbChangeItemState.SelectedIndex;
                if (archiveBox.IsChecked == true) mCurrArch = true;
                else mCurrArch = false;

                this.Close();
            }
        }

        private new void PreviewTextInput(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (box.Text[0] == '.')
            {
                box.Text = box.Text.Remove(0, 1);
                return;
            }
            if (box.Text[box.Text.Length - 1] == '.')
            {
                box.Text = box.Text.Remove(box.Text.Length - 1, 1);
                return;
            }

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

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearOptions()
        {
            text_edit_err.Text = "Uspesne zmenene";
            text_edit_err.Foreground = Brushes.Green;
            icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
        }

        private void Ftp_Upload(string path)
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

        private void Ftp_Download(string imgName)
        {
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("test", "test");
            //string[] split = path.Split('/');
            //string fileName = split[split.Length - 1];
            client.DownloadFile("ftp://dokelu.kst.fri.uniza.sk/" + imgName, "~/../../../imageres/" + imgName);
        }

        private bool CompareItemStruct(EditItemStruct first, EditItemStruct second)
        {
            if (first.name != second.name)
            {
                return false;
            }
            if (first.description != second.description)
            {
                return false;
            }
            if (first.size != second.size)
            {
                return false;
            }
            if (first.price != second.price)
            {
                return false;
            }
            if (first.photo != second.photo)
            {
                return false;
            }

            return true;
        }

    }
}
