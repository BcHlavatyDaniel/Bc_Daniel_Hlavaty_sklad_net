using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data;
using System.Linq;
using Emgu.CV;
using Emgu.CV.UI;
using System.ComponentModel;
using DatabaseProj;

namespace materialApp
{
    public partial class UsersPage : Page, INotifyPropertyChanged
    {
        private DbActions mDbActions;
        VideoCapture mCapture;
        ImageViewer mViewer;
        public List<User> UserList { get; set; }
        public User mUser { get; set; }

        public List<string> idCmbList { get; set; } = new List<string>();
        public List<string> fNameCmbList { get; set; } = new List<string>();
        public List<string> sNameCmbList { get; set; } = new List<string>();
        public List<string> addressCmbList { get; set; } = new List<string>();
        public List<string> phoneCmbList { get; set; } = new List<string>();
        private int mSelectedId;
        public int selectedId
        {
            get
            {
                return mSelectedId;
            }
            set
            {
                mSelectedId = value;
                NotifyPropertyChanged("selectedId");
            }
        }
        private int mSelectedFName;
        public int selectedFName
        {
            get
            {
                return mSelectedFName;
            }
            set
            {
                mSelectedFName = value;
                NotifyPropertyChanged("selectedFName");
            }
        }
        private int mSelectedSName;
        public int selectedSName
        {
            get
            {
                return mSelectedSName;
            }
            set
            {
                mSelectedSName = value;
                NotifyPropertyChanged("selectedSName");
            }
        }
        private int mSelectedAddress;
        public int selectedAddress
        {
            get
            {
                return mSelectedAddress;
            }
            set
            {
                mSelectedAddress = value;
                NotifyPropertyChanged("selectedAddress");
            }
        }

        private int mSelectedPhone;
        public int selectedPhone
        {
            get
            {
                return mSelectedPhone;
            }
            set
            {
                mSelectedPhone = value;
                NotifyPropertyChanged("selectedPhone");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public UsersPage(DbActions mDbActions, ImageViewer view, VideoCapture capture)
        {
            InitializeComponent();
            DataContext = this;
            DialogHost.DataContext = this;
			this.mDbActions = mDbActions;
            Init(view, capture);
        }

        private void Init(ImageViewer view, VideoCapture capture)
        {
            UserList = new List<User>();
            mUser = new User();
            DataSet data = mDbActions.LoadAllUsers();
            LoadGrid(data);
            mCapture = capture;
            mViewer = view;

            idCmbList.Insert(0, "");
            fNameCmbList.Insert(0, "");
            sNameCmbList.Insert(0, "");
            addressCmbList.Insert(0, "");
            phoneCmbList.Insert(0, "");
        }

        private void LoadGrid(DataSet gridData)
        {
            gridData.Tables[0].Columns.Add("rok-id", typeof(string));
            gridData.Tables[0].Columns.Add("pocet tovaru", typeof(int));

            foreach (DataRow row in gridData.Tables[0].Rows)
            {
                row["rok-id"] = row["year"] + "-" + row["_numbers"];
                row["pocet tovaru"] = mDbActions.GetNumberOfItemsForUser(row["year"].ToString(), row["_numbers"].ToString());
            }

            UserList.Clear();
            foreach(DataRow row in gridData.Tables[0].Rows)
            {
                if (!idCmbList.Contains(row["rok-id"].ToString()))
                    idCmbList.Add(row["rok-id"].ToString());
                if (!fNameCmbList.Contains(row["first_name"].ToString()))
                    fNameCmbList.Add(row["first_name"].ToString());
                if (!sNameCmbList.Contains(row["second_name"].ToString()))
                    sNameCmbList.Add(row["second_name"].ToString());
                if (!addressCmbList.Contains(row["address"].ToString()))
                    addressCmbList.Add(row["address"].ToString());
                if (!phoneCmbList.Contains(row["telephone"].ToString()))
                    phoneCmbList.Add(row["telephone"].ToString());

                UserList.Add(new User
                {
                    FName = row["first_name"].ToString(),
                    SName = row["second_name"].ToString(),
                    IdName = row["rok-id"].ToString(),
                    Item_Count = int.Parse(row["pocet tovaru"].ToString()),
                    Phone = int.Parse(row["telephone"].ToString()),
                    Address = row["address"].ToString(),
                });
            }
            dataGrid.Items.Refresh();
            dataGrid.UpdateLayout();
            updateTooltips();
        }

        private void updateTooltips()
        {
            int max = dataGrid.Items.Count;
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
            updateTooltips();
        }

        private void Profile_Open(object sender, RoutedEventArgs e)
		{
            User curUser = ((User)dataGrid.SelectedItem);
            if (curUser == null) return;

            User userStruct = new User
            {
                IdYear = curUser.IdYear,
                IdNumber = curUser.IdNumber,
                FName = curUser.FName,
                SName = curUser.SName,
                Address = curUser.Address,
                Phone = curUser.Phone
            };

            User_details mUserDWindow = new User_details(userStruct, mViewer, mCapture);
            mUserDWindow.Owner = Window.GetWindow(this);
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            ResetCmbs();
            LoadGrid(mDbActions.LoadAllUsers());
        }

        private void ResetCmbs()
        {
            selectedAddress = 0;
            selectedFName = 0;
            selectedSName = 0;
            selectedPhone = 0;
            selectedId = 0;
        }

        private void Search(object sender, SelectionChangedEventArgs e)
		{
            if (selectedId == 0 && selectedFName == 0 && selectedSName == 0 && selectedPhone == 0 && selectedAddress == 0)
            {
                LoadGrid(mDbActions.LoadAllUsers());
                return;
            }

            string year;
            string number;
            if (selectedId == 0)
            {
                year = "";
                number = "";
            } else
            {
                year = idCmbList.ElementAt(selectedId).Substring(0, 2);
                number = idCmbList.ElementAt(selectedId).Substring(3, 3);
            }

            User userStruct = new User
            {
                SName = sNameCmbList.ElementAt(selectedSName),
                FName = fNameCmbList.ElementAt(selectedFName),
                Address = addressCmbList.ElementAt(selectedAddress),
                Phone = int.Parse(phoneCmbList.ElementAt(selectedPhone)),
                IdYear = int.Parse(year),
                IdNumber = int.Parse(number)
            };

            DataSet data = mDbActions.SearchForUserNames(userStruct);
            LoadGrid(data);
        }

        public void ModalUserAddInit(object sender, RoutedEventArgs e)
        {
            mUser.Color = "Green";
            mUser.Kind = "Error";
            mUser.Icon_Visibility = "Hidden";
            mUser.Edit_Text = "";
            mUser.Address = "";
            mUser.Id = "";
            mUser.FName = "";
            mUser.SName = "";
            DialogHost.IsOpen = true;
        }

        public void ModalBack(object sender, RoutedEventArgs e)
        {
            DialogHost.IsOpen = false;
            LoadGrid(mDbActions.LoadAllUsers());
            ResetCmbs();
        }

        public void ModalAdd(object sender, RoutedEventArgs e)
        {
            bool err = false;
            mUser.Icon_Visibility = "Visible";
            User curUser = mUser.Copy();

            if (curUser.FName == "")
            {
                err = true;
                mUser.Color = "Red";
                mUser.Kind = "Error";
                mUser.Edit_Text = "Dopln prve meno";
            }
            if (curUser.SName == "")
            {
                err = true;
                mUser.Color = "Red";
                mUser.Kind = "Error";
                mUser.Edit_Text = "Dopln druhe meno";
            }
            if (curUser.Address == "")
            {
                err = true;
                mUser.Color = "Red";
                mUser.Kind = "Error";
                mUser.Edit_Text = "Dopln adresu";
            }
            if (curUser.Id == "")
            {
                err = true;
                mUser.Color = "Red";
                mUser.Kind = "Error";
                mUser.Edit_Text = "Dopln tel. cislo";
            }

            if (err) return;

            mDbActions.AddUser(curUser);
            ResetCmbs();
            mUser.Edit_Text = "Uspesne pridane";
            mUser.Address = "";
            mUser.Id = "";
            mUser.FName = "";
            mUser.SName = "";
            mUser.Color = "Green";
            mUser.Kind = "Done";
            CloseModalAfterAdd();
        }
        private async void CloseModalAfterAdd()
        {
            await Task.Delay(1000);
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