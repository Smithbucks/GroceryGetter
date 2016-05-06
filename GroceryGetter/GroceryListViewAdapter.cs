using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;


namespace GroceryGetter
{
	public class GroceryListViewAdapter : BaseAdapter<GroceryListItem>
	{
		private readonly Activity context;
		private List<GroceryListItem> groceryListData;

		public GroceryListViewAdapter (Activity _context, List<GroceryListItem> _groceryListData) : base()
		{
			this.context = _context;
			this.groceryListData = _groceryListData;
		}

		public override int Count
		{
			get { return groceryListData.Count; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override GroceryListItem this[int index]
		{
			get { return groceryListData[index]; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView;

			if (view == null) {
				view = context.LayoutInflater.Inflate (Resource.Layout.GroceryListItem, null);
			}

			GroceryListItem groceryItem = this[position];

			return view;
		}
	}
}

