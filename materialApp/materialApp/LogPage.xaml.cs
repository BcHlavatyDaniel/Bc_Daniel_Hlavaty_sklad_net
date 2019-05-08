using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Data;
using Emgu.CV;
using Emgu.CV.UI;
using System.ComponentModel;
using DatabaseProj;

namespace materialApp
{

    public partial class LogPage : Page, INotifyPropertyChanged
    {
		private DbActions mDbActions;
        CommonActions mCommonActions;
        VideoCapture mCapture;
        ImageViewer mViewer;
        public List<LogTableItem> ItemList { get; set; }

        public List<string> nameCmbList { get; set; } = new List<string>(); 
        public List<string> itemCmbList { get; set; } = new List<string>();
        public List<string> typCmbList { get; set; } = new List<string>();
        private int mSelectedItem;
        public int selectedItem
        {
            get
            {
                return mSelectedItem;
            }
            set
            {
                mSelectedItem = value;
                NotifyPropertyChanged("selectedItem");
            }
        }
        private int mSelectedType;
        public int selectedType
        {
            get
            {
                return mSelectedType;
            }
            set
            {
                mSelectedType = value;
                NotifyPropertyChanged("selectedType");
            }
        }
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

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public LogPage(DbActions mDbActions, ImageViewer view, VideoCapture capture)
        {
            InitializeComponent();
            DataContext = this;
			this.mDbActions = mDbActions;
            Init(view, capture);
        }

        private void Init(ImageViewer view, VideoCapture capture)
        {
            mCommonActions = new CommonActions();
            ItemList = new List<LogTableItem>();
            nameCmbList = new List<string>();
            itemCmbList = new List<string>();
            typCmbList = new List<string>();
            DataSet logData = mDbActions.LoadAllLogs();
            LoadLogGrid(logData);
            nameCmbList.Insert(0, "");
            itemCmbList.Insert(0, "");
            typCmbList.Insert(0, "");
            mCapture = capture;
            mViewer = view;
        }

        private void LoadLogGrid(DataSet data)
        {
            ItemList.Clear();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                if (!nameCmbList.Contains(row["user_id"].ToString()))
                    nameCmbList.Add(row["user_id"].ToString());
                if (!itemCmbList.Contains(row["item_id"].ToString()))
                    itemCmbList.Add(row["item_id"].ToString());
                if (!typCmbList.Contains(row["type"].ToString()))
                    typCmbList.Add(row["type"].ToString());

                ItemList.Add(new LogTableItem
                {
                    id_tovar = row["item_id"].ToString(),
                    id_uzivatela = row["user_id"].ToString(),
                    typ_zmeny = row["type"].ToString(),
                    popis = row["change_text"].ToString(),
                    time = row["time"].ToString(),
                });
            }
            logDataGrid.Items.Refresh();
        }

        private void Item_Log_Open(object sender, RoutedEventArgs e)
		{
            LogTableItem item = ((LogTableItem)logDataGrid.SelectedItem);
            DataSet itemRow = mDbActions.LoadSpecificItem(item.id_tovar);
            DataSet userRow = mDbActions.LoadSpecificUser(itemRow.Tables[0].Rows[0]["user_year"].ToString(), itemRow.Tables[0].Rows[0]["user_numbers"].ToString());
            Item_details mItemDWindow = new Item_details(item.id_tovar, userRow.Tables[0].Rows[0]["first_name"].ToString(), userRow.Tables[0].Rows[0]["second_name"].ToString(), userRow.Tables[0].Rows[0]["year"].ToString() + "-" + userRow.Tables[0].Rows[0]["_numbers"].ToString(), mViewer, mCapture);
            mItemDWindow.Owner = Window.GetWindow(this);
            mItemDWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mItemDWindow.ShowDialog();
            picker.Text = "";
            selectedItem = 0;
            selectedType = 0; 
            selectedName = 0; 
            LoadLogGrid(mDbActions.LoadAllLogs());
        }

        private void SearchLogs(object sender, RoutedEventArgs e)
		{
            DateTime day = new DateTime();
            bool pickerEmpty = true;

            if (picker.ToString() != "")
            {
                DateTime.TryParse(picker.ToString(), out day);
                pickerEmpty = false;
            }

            if (pickerEmpty && selectedItem == 0 && selectedName == 0 && selectedType == 0)
            {
                LoadLogGrid(mDbActions.LoadAllLogs());
                return;
            }

            LoadLogGrid(mDbActions.SearchForLogs(itemCmbList.ElementAt(selectedItem),nameCmbList.ElementAt(selectedName) , typCmbList.ElementAt(selectedType), day));
        }
    }
}


