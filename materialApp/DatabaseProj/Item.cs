using System.ComponentModel;

namespace DatabaseProj
{
	public class Item : INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int Id { get; set; }
		public string UserId
		{
			get
			{
				return $"{UserYear}-{UserNumber}";
			}
			set
			{
				string[] x = value.Split('-');
				UserYear = int.Parse(x[0]);
				UserNumber = int.Parse(x[1]);
			}
		}
		public int UserYear { get; set; }
		public int UserNumber { get; set; }

        private string mUserFName;
        public string UserFName
        {
            get
            {
                return mUserFName;
            }
            set
            {
                mUserFName = value;
                NotifyPropertyChanged("UserFName");
            }
        }
        public string UserSName { get; set; }

        private string mName;
		public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
                NotifyPropertyChanged("Name");
            }
        }
        private string mSize;
		public string Size
        {
            get
            {
                return mSize;
            }
            set
            {
                mSize = value;
                NotifyPropertyChanged("Size");
            }
        }
        private double mPrice;
		public double Price
        {
            get
            {
                return mPrice;
            }
            set
            {
                mPrice = value;
                NotifyPropertyChanged("Price");
            }
        }

        private string mDescription;
        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
                NotifyPropertyChanged("Description");
            }
        }
        private string mPhoto;
        public string Photo
        {
            get
            {
                return mPhoto;
            }
            set
            {
                mPhoto = value;
                NotifyPropertyChanged("Photo");
            }
        }

        public bool Archived { get; set; }
		public int State { get; set; }

        private string mIcon_Visibility;
        public string Icon_Visibility
        {
            get
            {
                return mIcon_Visibility;
            }
            set
            {
                mIcon_Visibility = value;
                NotifyPropertyChanged("Icon_Visibility");
            }
        }
        private string mAdd_Text;
        public string Add_Text
        {
            get
            {
                return mAdd_Text;
            }
            set
            {
                mAdd_Text = value;
                NotifyPropertyChanged("Add_Text");
            }
        }
        private string mColor;
        public string Color
        {
            get
            {
                return mColor;
            }
            set
            {
                mColor = value;
                NotifyPropertyChanged("Color");
            }
        }
        private string mKind;
        public string Kind
        {
            get
            {
                return mKind;
            }
            set
            {
                mKind = value;
                NotifyPropertyChanged("Kind");
            }
        }

        private string mStateText;
        public string StateText
        {
            get
            {
                return mStateText;
            }
            set
            {
                mStateText = value;
                NotifyPropertyChanged("StateText");
            }
        }

        private string mStateColor;
        public string StateColor
        {
            get
            {
                return mStateColor;
            }
            set
            {
                mStateColor = value;
                NotifyPropertyChanged("StateColor");
            }
        }

        private string mSellCashVisibility;
        public string SellCashVisibility
        {
            get
            {
                return mSellCashVisibility;
            }
            set
            {
                mSellCashVisibility = value;
                NotifyPropertyChanged("SellCashVisibility");
            }
        }

        private double mSellCashWidth;
        public double SellCashWidth
        {
            get
            {
                return mSellCashWidth;
            }
            set
            {
                mSellCashWidth = value;
                NotifyPropertyChanged("SellCashWidth");
            }
        }

        private string mSellCardVisibility;
        public string SellCardVisibility
        {
            get
            {
                return mSellCardVisibility;
            }
            set
            {
                mSellCardVisibility = value;
                NotifyPropertyChanged("SellCardVisibility");
            }
        }

        private double mSellCardWidth;
        public double SellCardWidth
        {
            get
            {
                return mSellCardWidth;
            }
            set
            {
                mSellCardWidth = value;
                NotifyPropertyChanged("SellCardWidth");
            }
        }

        private string mReturnVisibility;
        public string ReturnVisibility
        {
            get
            {
                return mReturnVisibility;
            }
            set
            {
                mReturnVisibility = value;
                NotifyPropertyChanged("ReturnVisibility");
            }
        }

        private double mReturnWidth;
        public double ReturnWidth
        {
            get
            {
                return mReturnWidth;
            }
            set
            {
                mReturnWidth = value;
                NotifyPropertyChanged("ReturnWidth");
            }
        }

        private string mPayVisibility;
        public string PayVisibility
        {
            get
            {
                return mPayVisibility;
            }
            set
            {
                mPayVisibility = value;
                NotifyPropertyChanged("PayVisibility");
            }
        }

        private double mPayWidth;
        public double PayWidth
        {
            get
            {
                return mPayWidth;
            }
            set
            {
                mPayWidth = value;
                NotifyPropertyChanged("PayWidth");
            }
        }

        public Item Copy()
		{
			Item newItem = new Item()
			{
				Id = Id,
				UserYear = UserYear,
				UserNumber = UserNumber,
				Name = Name,
				Size = Size,
				Price = Price,
				Archived = Archived,
				State = State,
				Description = Description,
				Photo = Photo,
                UserFName = UserFName
			};
			return newItem;
		}


		public bool Compare(Item other)
		{
			if (other.Name != Name)
			{
				return false;
			}
			if (other.Description != Description)
			{
				return false;
			}
			if (other.Size != Size)
			{
				return false;
			}
			if (other.Price != Price)
			{
				return false;
			}
			if (other.Photo != Photo)
			{
				return false;
			}
			if (other.State != State)
			{
				return false;
			}
			if (other.Archived != Archived)
			{
				return false;
			}

			return true;
		}
	}
}
