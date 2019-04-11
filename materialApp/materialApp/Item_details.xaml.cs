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
        string mPhotoPath = "";
        bool mCloseWin;
        int mCurrState = 0;
        int mLastSuccessState = 0;

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

        private void ChangeState(object sender, RoutedEventArgs e)
        {
            mCurrState = cmbChangeItemState.SelectedIndex;
        }

        private void LoadItem()
        {
            DataRow row = mDbActions.LoadSpecificItem(mId).Tables[0].Rows[0];
            text_description.Text = row["description"].ToString();
            text_price.Text = row["price"].ToString();
            text_size.Text = row["size"].ToString();
            text_name.Text = row["name"].ToString();

            DateTime retTime;
            DateTime paidTime;
            DateTime soldTime;
            DateTime createdTime;
            int usedCard;
            DateTime.TryParse(row["returned_at"].ToString(), out retTime);
            DateTime.TryParse(row["paid_at"].ToString(), out paidTime);
            DateTime.TryParse(row["sold_at"].ToString(), out soldTime);
            DateTime.TryParse(row["created_at"].ToString(), out createdTime);
            int.TryParse(row["used_card"].ToString(), out usedCard);

            if (retTime.ToString() != "1/1/0001 12:00:00 AM")
            {
                cmbChangeItemState.SelectedItem = cmbChangeItemState.Items[3];
                mCurrState = 3;
                mLastSuccessState = 3;
            }
            else if (paidTime.ToString() != "1/1/0001 12:00:00 AM")
            {
                if (usedCard == 1)
                {
                    cmbChangeItemState.SelectedItem = cmbChangeItemState.Items[4];
                    mCurrState = 4;
                    mLastSuccessState = 4;
                }
                else
                {
                    cmbChangeItemState.SelectedItem = cmbChangeItemState.Items[5];
                    mCurrState = 5;
                    mLastSuccessState = 5;
                }
            }
            else if (soldTime.ToString() != "1/1/0001 12:00:00 AM")
            {
                if (usedCard == 1)
                {
                    cmbChangeItemState.SelectedItem = cmbChangeItemState.Items[1];
                    mCurrState = 1;
                    mLastSuccessState = 1;
                }
                else
                {
                    cmbChangeItemState.SelectedItem = cmbChangeItemState.Items[2];
                    mCurrState = 2;
                    mLastSuccessState = 2;
                }
            }
            else
            {
                cmbChangeItemState.SelectedItem = cmbChangeItemState.Items[0];
                mCurrState = 0;
                mLastSuccessState = 0;
            }
                
            image1.Source = new BitmapImage(new Uri(row["photo"].ToString(), UriKind.RelativeOrAbsolute));

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
            DirectoryInfo di = new DirectoryInfo("~/../../../imageres/");
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
            //photo_path = "/imageres/" +imgName;
            mPhotoPath = "C://Users/Daniel/source/repos/materialApp/materialApp/imageres/" + imgName;   //TO DO this directory path to config
                                                                                                        // image1.Source = new BitmapImage(new Uri(photo_path, UriKind.RelativeOrAbsolute));
            image1.Source = new BitmapImage(new Uri(mPhotoPath, UriKind.RelativeOrAbsolute));
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
            icon_edit_err.Visibility = Visibility.Visible;

            double num;

            if (!double.TryParse(text_price.Text, out num))
            {
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

            EditItemStruct itemStruct = new EditItemStruct
            {
                id = mId,
                description = text_description.Text,
                price = text_price.Text,
                size = text_size.Text,
                name = text_name.Text,
                photo = mPhotoPath
            };

            bool change = false;

            if (mLastSuccessStruct.name == text_name.Text && mLastSuccessStruct.description == text_description.Text &&
                mLastSuccessStruct.size == text_size.Text && mLastSuccessStruct.price == text_price.Text && mLastSuccessStruct.photo == mPhotoPath)
            {

            } else
            {
                mDbActions.UpdateItem(itemStruct);
                change = true;
            }

            if (mCurrState != mLastSuccessState)
            {
                mDbActions.UpdateItemTimes(mId, mCurrState);
                mLastSuccessState = mCurrState;
                change = true;
            }

            if (change)
            {
                mLastSuccessStruct = new EditItemStruct
                {
                    description = text_description.Text,
                    photo = mPhotoPath,
                    name = text_name.Text,
                    size = text_size.Text,
                    price = text_price.Text
                };

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

            if (mLastSuccessState != mCurrState)
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
                OnClosePopup.IsOpen = false;
                mLastSuccessStruct.name = text_name.Text;
                mLastSuccessStruct.description = text_description.Text;
                mLastSuccessStruct.size = text_size.Text;
                mLastSuccessStruct.price = text_size.Text;
                mLastSuccessStruct.photo = mPhotoPath;

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

        private void ClearOptions()
        {
            text_edit_err.Text = "Uspesne zmenene";
            text_edit_err.Foreground = Brushes.Green;
            icon_edit_err.Kind = MaterialDesignThemes.Wpf.PackIconKind.Done;
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

            return true;
        }

    }
}
