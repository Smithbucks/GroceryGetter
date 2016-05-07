using System;
using Android.App;
using Android.Widget;
using Android.OS;
using SQLite;

namespace GroceryGetter
{
	[Table("GroceryListItemTable")]

	public class GroceryListItem
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }

		[NotNull]
		public string ItemName { get; set; }

		[MaxLength(100)]
		public string ItemQty { get; set; }

		public GroceryListItem ()
		{
			
		}



	}
}

