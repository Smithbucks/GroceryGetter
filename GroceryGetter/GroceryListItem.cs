using System;
using Android.App;
using Android.Widget;
using Android.OS;
using SQLite;

namespace GroceryGetter
{
	// Link this class to a database table
	[Table("GroceryListItemTable")]

	/*******************************************************************/
	/** GroceryListItem: A class to define a single grocery list item **/
	/*******************************************************************/
	public class GroceryListItem
	{
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }

		[NotNull]
		public string ItemName { get; set; }

		[MaxLength(100)]
		public string ItemQty { get; set; }

		/******************************/
		/** Constructor (no actions) **/
		/******************************/
		public GroceryListItem ()
		{
			
		}
	}
}

