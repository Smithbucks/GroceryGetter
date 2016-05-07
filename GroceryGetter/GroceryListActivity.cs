using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;



namespace GroceryGetter
{
	[Activity (Label = "GroceryGetter", MainLauncher = true, Icon = "@mipmap/icon")]
	public class GroceryListActivity : Activity
	{
		private ListView groceryListView;
		private List<GroceryListItem> groceryListData;
		private GroceryListViewAdapter groceryListAdapter;
		// Create 
		int scrollPosition;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.GroceryList);

			groceryListView = FindViewById<ListView> (Resource.Id.groceryListView);
			groceryListView.ItemClick += GroceryItemClicked;

			DBManager.Instance.CreateTable ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			getGroceryList ();
		}

		// Method to generate options menu
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			// Inflate menus to be shown on ActionBar
			MenuInflater.Inflate(Resource.Menu.GroceryListViewMenu, menu);
			return base.OnPrepareOptionsMenu (menu);
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			int currentPosition = groceryListView.FirstVisiblePosition;
			outState.PutInt ("scroll_position", currentPosition);
		}



		protected override void OnRestoreInstanceState (Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState (savedInstanceState);
			scrollPosition = savedInstanceState.GetInt ("scroll_position");
		}



		// Method to handle selecting a menu option
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.actionNew:
					Intent intent = new Intent (this, typeof(GroceryItemDetailActivity));
					StartActivity (intent);
					return true;
				default:
					return base.OnOptionsItemSelected (item);
			}
		}

		// Method to handle clicking a list item
		protected void GroceryItemClicked(object sender, ListView.ItemClickEventArgs e)
		{
			GroceryListItem listItem = groceryListData [(int)e.Id];
			int listIndex = (int)e.Id;

			Intent listItemDetailIntent = new Intent(this, typeof(GroceryItemDetailActivity));

			string listItemJson = JsonConvert.SerializeObject (listItem);

			listItemDetailIntent.PutExtra ("listItem", listItemJson);

			StartActivity (listItemDetailIntent);


			/*
			// Fetching the object at user clicked position
			GroceryListItem item = groceryListData[(int)e.Id];

			// Showing log result

			Console.Out.WriteLine("Grocery item clicked: Name is {0}", item.ItemName);
			*/
		}

		// Method to load data for the grocery list
		public void getGroceryList() {
			groceryListData = DBManager.Instance.getGroceryListFromDatabase ();

			groceryListAdapter = new GroceryListViewAdapter (this, groceryListData);
			groceryListView.Adapter = groceryListAdapter;
		}
			
	}
}


