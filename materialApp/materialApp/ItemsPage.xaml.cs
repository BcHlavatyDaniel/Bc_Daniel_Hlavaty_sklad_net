using DatabaseProj;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Emgu.CV;
using Emgu.CV.UI;
using System.ComponentModel;

namespace materialApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 

	public partial class ItemsPage : Page, INotifyPropertyChanged
	{
        private DbActions mDbActions;
        enum mState { Einit_state, Esold_card, Esold_cash, Ereturned, Epaid_card, Epaid_cash, Earchived };
        VideoCapture mCapture;
        ImageViewer mViewer;
        public List<Item> ItemList { get; set; }

        public List<string> idCmbList { get; set; } = new List<string>();
        public List<string> fNameCmbList { get; set; } = new List<string>();
        public List<string> sNameCmbList { get; set; } = new List<string>();
        public List<string> iNameCmbList { get; set; } = new List<string>();

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
        private int mSelectedIName;
        public int selectedIName
        {
            get
            {
                return mSelectedIName;
            }
            set
            {
                mSelectedIName = value;
                NotifyPropertyChanged("selectedIName");
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

        public ItemsPage(DbActions mDbActions, ImageViewer view, VideoCapture capture)
        {
            InitializeComponent();
			this.mDbActions = mDbActions;
            DataContext = this;
            Init(view, capture);
        }

        private void Init(ImageViewer view, VideoCapture capture)
        {
            ItemList = new List<Item>();
            mCapture = capture;
            mViewer = view;
            DataSet itData = mDbActions.LoadAllItems();
            LoadItemsGrid(itData, mDbActions.LoadAllUsers());
            idCmbList.Insert(0, "");
            fNameCmbList.Insert(0, "");
            sNameCmbList.Insert(0, "");
            iNameCmbList.Insert(0, "");
        }

        private void LoadItemsGrid(DataSet allItems, DataSet allUsers)
        {
            allItems.Tables[0].Columns.Add("rok-id", typeof(string));
            allItems.Tables[0].Columns.Add("Prve meno", typeof(string));
            allItems.Tables[0].Columns.Add("Druhe meno", typeof(string));

            for (int i = allItems.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = allItems.Tables[0].Rows[i];
                row["rok-id"] = row["user_year"] + "-" + row["user_numbers"];
                foreach (DataRow uRow in allUsers.Tables[0].Rows)
                {
                    if (row["user_year"].ToString() == uRow["year"].ToString())
                    {
                        if (row["user_numbers"].ToString() == uRow["_numbers"].ToString())
                        {
                            row["Prve meno"] = uRow["first_name"].ToString();
                            row["Druhe meno"] = uRow["second_name"].ToString();
                            break;
                        }
                    }
                }
                if (row["Prve meno"].ToString() == "")
                {
                    row.Delete();
                }
            }
            allItems.AcceptChanges();

            ItemList.Clear();
            string curState = "";
            foreach (DataRow row in allItems.Tables[0].Rows)
            {
                if (!idCmbList.Contains(row["rok-id"].ToString()))
                    idCmbList.Add(row["rok-id"].ToString());
                if (!fNameCmbList.Contains(row["Prve meno"].ToString()))
                    fNameCmbList.Add(row["Prve meno"].ToString());
                if (!sNameCmbList.Contains(row["Druhe meno"].ToString()))
                    sNameCmbList.Add(row["Druhe meno"].ToString());
                if (!iNameCmbList.Contains(row["name"].ToString()))
                    iNameCmbList.Add(row["name"].ToString());

                if (row["archived"].ToString() == "True")
                {
                    curState = "OrangeRed";
                } 
                else
                {
                    switch (int.Parse(row["stav"].ToString()))
                    {
                        case 1:
                            {
                                curState = "LimeGreen";
                                break;
                            }
                        case 2:
                            {
                                curState = "LightYellow";
                                break;
                            }
                        case 3:
                            {
                                curState = "LightGray";
                                break;
                            }
                        case 4:
                            {
                                curState = "Green";
                                break;
                            }
                        case 5:
                            {
                                curState = "Yellow";
                                break;
                            }
                    }
                }
                ItemList.Add(new Item
                {
                    UserId = row["rok-id"].ToString(),
                    UserFName = row["Prve meno"].ToString(),
                    UserSName = row["Druhe meno"].ToString(),
                    Id = int.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    Size = row["size"].ToString(),
                    Price = Double.Parse(row["price"].ToString()),
                    Description = row["description"].ToString(),
                    Color = curState
                });
            }
            itemsDataGrid.Items.Refresh();
            itemsDataGrid.UpdateLayout();
        }

		private void Item_Open(object sender, RoutedEventArgs e)
		{
            Item currItem = ((Item)itemsDataGrid.SelectedItem);
            Item_details mItemDWindow = new Item_details(currItem.Id.ToString(),currItem.UserFName, currItem.UserSName, currItem.UserId, mViewer, mCapture); 
            mItemDWindow.Owner = Window.GetWindow(this);
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            ResetCmbs();
            LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
        }
		private void Profile_Item_Open(object sender, RoutedEventArgs e)
		{
            Item currItem = ((Item)itemsDataGrid.SelectedItem);
            DataSet userRow = mDbActions.LoadSpecificUser(currItem.UserYear.ToString(), currItem.UserNumber.ToString());

            User userStruct = new User
            {
                IdYear = currItem.UserYear,
                IdNumber = currItem.UserNumber,
                FName = userRow.Tables[0].Rows[0]["first_name"].ToString(),
                SName = userRow.Tables[0].Rows[0]["second_name"].ToString(),
                Address = userRow.Tables[0].Rows[0]["address"].ToString(),
                Phone = int.Parse(userRow.Tables[0].Rows[0]["telephone"].ToString())
            };

            User_details mUserDWindow = new User_details(userStruct, mViewer, mCapture);
            mUserDWindow.Owner = Window.GetWindow(this);
            mUserDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mUserDWindow.ShowDialog();
            ResetCmbs();
            LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
        }

        private void ResetCmbs()
        {
            selectedFName = 0;
            selectedId = 0;
            selectedIName = 0;
            selectedSName = 0;
        }

		private void SearchItems(object sender, RoutedEventArgs e)
		{
            if (selectedFName == 0 && selectedId == 0 && selectedIName == 0 && selectedSName == 0)
            {
                LoadItemsGrid(mDbActions.LoadAllItems(), mDbActions.LoadAllUsers());
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
                IdYear = int.Parse(year),
                IdNumber = int.Parse(number),
                Address = iNameCmbList.ElementAt(selectedIName)
            };

            DataSet userData = new DataSet();
            DataSet data = mDbActions.SearchForItems(userStruct, ref userData);
            LoadItemsGrid(data, userData);
        }
	}
}

