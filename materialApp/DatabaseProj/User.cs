using System.ComponentModel;

namespace DatabaseProj
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Name
		{
			get
			{
				return $"{FName} {SName}";
			}
		}
		public string IdName
		{
			get
			{
				return $"{IdYear}-{IdNumber}";
			}
			set
			{
				string[] x = value.Split('-');
				IdYear = int.Parse(x[0]);
				IdNumber = int.Parse(x[1]);
			}
		}
        private string mId;
        public string Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
                NotifyPropertyChanged("Id");
            }
        }
        public int IdYear { get; set; }
        public int IdNumber { get; set; }
        private string mFName;
        public string FName
        {
            get
            {
                return mFName;
            }
            set
            {
                mFName = value;
                NotifyPropertyChanged("FName");
            }
        }

        private string mSName;
        public string SName
        {
            get
            {
                return mSName;
            }
            set
            {
                mSName = value;
                NotifyPropertyChanged("SName");
            }
        }
        private string mAddress;
        public string Address
        {
            get
            {
                return mAddress;
            }
            set
            {
                mAddress = value;
                NotifyPropertyChanged("Address");
            }
        }
        private int mPhone;
        public int Phone
        {
            get
            {
                return mPhone;
            }
            set
            {
                mPhone = value;
                NotifyPropertyChanged("Phone");
            }
        }
        public int Item_Count { get; set; }


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
        private string mEdit_Text;
        public string Edit_Text
        {
            get
            {
                return mEdit_Text;
            }
            set
            {
                mEdit_Text = value;
                NotifyPropertyChanged("Edit_Text");
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


        public User Copy()
        {
            User newUser = new User()
            {
                FName = FName,
                SName = SName,
                IdName = IdName,
                Address = Address,
                Phone = Phone,
                Item_Count = Item_Count,
                Icon_Visibility = Icon_Visibility,
                Edit_Text = Edit_Text,
                Color = Color,
                Kind = Kind,
                Id = Id
            };
            return newUser;
        }

        public bool Compare(User other)
        {
            if (other.FName != FName)
            {
                return false;
            }
            if (other.SName != SName)
            {
                return false;
            }
            if (other.Address != Address)
            {
                return false;
            }
            if (other.Phone != Phone)
            {
                return false;
            }

            return true;
        }
    }
}
