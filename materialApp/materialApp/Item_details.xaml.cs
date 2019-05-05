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
using DatabaseProj;

namespace materialApp
{
	/// <summary>
	/// Interaction logic for Item_details.xaml
	/// </summary>
	public partial class Item_details : Window
    {
        DbActions mDbActions;
		public Item Item { get; set; }
		public User User { get; set; }
		bool mCloseWin;
        Item mLastSuccessStruct;

        ImageViewer mViewer;
        VideoCapture mCapture;

        public Item_details(string id, string f_name, string s_name, string user_id, ImageViewer view, VideoCapture cap)
        {
            InitializeComponent();
			DataContext = this;
            Init( id, f_name, s_name, user_id, view, cap);
        }

        private void Init(string id, string f_name, string s_name, string user_id, ImageViewer view, VideoCapture cap)
        {
            mDbActions = new DbActions();

            mViewer = view;
            mCapture = cap;

			User = new User();
            User.IdYear = int.Parse(user_id.Substring(0, 2));
            User.IdNumber = int.Parse(user_id.Substring(3, 3));
			User.FName = f_name;
			User.SName = s_name;

            LoadItem(id, user_id);
            icon_edit_err.Visibility = Visibility.Hidden;
            text_edit_err.Text = "";
        }

        private void LoadItem(string id, string userId)
        {
            Item = mDbActions.LoadSpecificItem2(id);
            User.IdYear = int.Parse(userId.Substring(0, 2));
            User.IdNumber = int.Parse(userId.Substring(3, 3));

            if (Item.Photo != "")
            {
                if (!File.Exists("~/../../../imageres/" + Item.Photo))
                {
                    Ftp_Download(Item.Photo);
                }
                string uri = System.IO.Path.GetFullPath("../../imageres/" + Item.Photo);
                image1.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            }


			mLastSuccessStruct = Item.Copy();
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

            Item.Photo = System.IO.Path.GetFullPath("../../imageres/" + imgName);
            image1.Source = new BitmapImage(new Uri(Item.Photo, UriKind.RelativeOrAbsolute));
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
              Item.Photo = "C://Users/Daniel/source/repos/materialApp/materialApp/imageres/" + imgName;   //TO DO this directory path to config
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
                Item.Photo = dlg.FileName;
                image1.Source = new BitmapImage(new Uri(Item.Photo, UriKind.RelativeOrAbsolute));
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
			Item item = Item.Copy();

          //  if (mLastUnsuccessStruct != null)
          //  {
          //      if (item.Compare(Item)) return;
          //  }
            if (item.Compare(mLastSuccessStruct)) return;

            string changeString = "";
            int logType = 0;

            if (Item.Archived)
            {
                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 6, Item.Archived.ToString());
                if (Item.Archived) changeString = "Tovar archivovany";
                else changeString = "Tovar odarchivovany";
                logType = 2;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }
			// todo takto to chces?
            if (mLastSuccessStruct.State != Item.State)
            {
                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 5, Item.State.ToString());
                changeString = "Zmena stavu z ";   
                if (Item.State == 0)
                {
                    changeString += "nepredany";
                } else if (Item.State == 1)
                {
                    changeString += "predany hotovostou";
                } else if (Item.State == 2)
                {
                    changeString += "predany kartou";
                } else if (Item.State == 3)
                {
                    changeString += "vrateny";
                } else if (Item.State == 4)
                {
                    changeString += "zaplateny hotovostou";
                } else if (Item.State == 5)
                {
                    changeString += "zaplateny kartou";
                }
                changeString += " na ";
                if (Item.State == 0)
                {
                    changeString += "nepredany";
                }
                else if (Item.State == 1)
                {
                    changeString += "predany hotovostou";
                }
                else if (Item.State == 2)
                {
                    changeString += "predany kartou";
                }
                else if (Item.State == 3)
                {
                    changeString += "vrateny";
                }
                else if (Item.State == 4)
                {
                    changeString += "zaplateny hotovostou";
                }
                else if (Item.State == 5)
                {
                    changeString += "zaplateny kartou";
                }

                logType = 1;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }
            if (Item.Photo != mLastSuccessStruct.Photo)
            {
                //photoPath to ftp, + fpt link to photoPath 
                Ftp_Upload(Item.Photo);
                string[] split = Item.Photo.Split('/');
                string fileName;
                if (split.Length == 1)
                {
                    split = Item.Photo.Split('\\');
                    fileName = split[split.Length - 1];
                }
                else
                {
                    fileName = split[split.Length - 1];
                }

                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 4, fileName);
                changeString = "Zmena obrazku.";
                logType = 3;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }
            if (Item.Name != mLastSuccessStruct.Name)
            {
                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 3, Item.Name);
                changeString = "Zmena nazvu tovaru z " + mLastSuccessStruct.Name + " na " + Item.Name.ToString();
                logType = 3;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }
            if (item.Price != mLastSuccessStruct.Price)
            {
                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 2, Item.Price.ToString());
                changeString = "Zmena ceny tovaru z " + mLastSuccessStruct.Price + " na " + Item.Price;
                logType = 0;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }
            if (item.Size != mLastSuccessStruct.Size)
            {
                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 1, Item.Size.ToString());
                changeString = "Zmena velkosti tovaru z " + mLastSuccessStruct.Size + " na " + Item.Size.ToString();
                logType = 3;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }
            if (item.Description != mLastSuccessStruct.Description)
            {
                mDbActions.UpdateSpecificItem(Item.Id.ToString(), 0, Item.Description.ToString());
                changeString = "Zmena popisu z " + mLastSuccessStruct.Description + " na " + Item.Description.ToString();
                logType = 3;
                mDbActions.AddLog(Item.Id.ToString(), Item.UserId.ToString(), logType, changeString);
            }

            if (changeString != "")
            {
                icon_edit_err.Visibility = Visibility.Visible;

				mLastSuccessStruct = Item.Copy();
				
                ClearOptions();
                LoadItem(Item.Id.ToString(), Item.UserId.ToString());
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Item item = Item.Copy();
            bool showPopup = false;
            if (!mLastSuccessStruct.Compare(item))
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
				mLastSuccessStruct = Item.Copy();

                this.Close();
            }
        }

        private new void PreviewTextInput(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

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


    }
}
