namespace DatabaseProj
{
    public class User
    {
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
        public string Id { get; set; }
        public int IdYear { get; set; }
        public int IdNumber { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }

        public User Copy()
        {
            User newUser = new User()
            {
                FName = FName,
                SName = SName,
                IdName = IdName,
                Address = Address,
                Phone = Phone
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
