namespace DatabaseProj
{
	public class Item
	{
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
		public string Name { get; set; }
		public string Size { get; set; }
		public double Price { get; set; }
		public bool Archived { get; set; }
		public int State { get; set; }
		public string Description { get; set; }
		public string Photo { get; set; }

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
