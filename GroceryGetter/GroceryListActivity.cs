using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;


namespace GroceryGetter
{
	[Activity (Label = "GroceryGetter", MainLauncher = true, Icon = "@mipmap/icon")]
	public class GroceryListActivity : Activity
	{
		private ListView groceryListView;
		private ProgressBar progressBar;
		private List<GroceryListItem> groceryListData;
		private GroceryListViewAdapter groceryListAdapter;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.GroceryList);

			groceryListView = FindViewById<ListView> (Resource.Id.groceryListView);
			groceryListView.ItemClick += GroceryItemClicked;

			progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);

			DownloadGroceryListAsync ();
		}

		// Method to generate options menu
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			// Inflate menus to be shown on ActionBar
			MenuInflater.Inflate(Resource.Menu.GroceryListViewMenu, menu);
			return base.OnPrepareOptionsMenu (menu);
		}

		// Method to handle selecting a menu option
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.actionNew:
					// placeholder for creating new grocery item
					return true;
				case  Resource.Id.actionDelete:
					// placeholder for deleting a grocery item
					return true;
				default:
					return base.OnOptionsItemSelected (item);
			}
		}

		// Method to handle clicking a list item
		protected void GroceryItemClicked(object sender, ListView.ItemClickEventArgs e)
		{
			// Fetching the object at user clicked position
			GroceryListItem item = groceryListData[(int)e.Id];

			// Showing log result

			Console.Out.WriteLine("Grocery item clicked: Name is {0}", item.ItemName);
		}

		// Temporary method to "download" data for the list items
		public void DownloadGroceryListAsync() {
			progressBar.Visibility = Android.Views.ViewStates.Visible;
			groceryListData = GetGroceryListTestData ();
			progressBar.Visibility = Android.Views.ViewStates.Gone;

			groceryListAdapter = new GroceryListViewAdapter (this, groceryListData);
			groceryListView.Adapter = groceryListAdapter;
		}

		private List<GroceryListItem> GetGroceryListTestData ()
		{
			List <GroceryListItem> listData = new List<GroceryListItem> ();

			for (int i = 0; i < 20; i++) {
				GroceryListItem GroceryListItem = new GroceryListItem ();
				GroceryListItem.Id = i;
				GroceryListItem.ItemName = "Name " + i;
				GroceryListItem.ItemQty = "Qty: " + i.ToString();

				listData.Add (GroceryListItem);
			}

			return listData;
		}
			
	}
}


