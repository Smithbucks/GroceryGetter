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
		/***********************************/
		/** Method properties and objects **/
		/***********************************/
		private ListView groceryListView;
		private List<GroceryListItem> groceryListData;
		private GroceryListViewAdapter groceryListAdapter;

		// Variable to store the scroll position when updating activity state **/
		int scrollPosition;

		/*********************/
		/** Activity States **/
		/*********************/
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.GroceryList);

			// Bind properties and objects 
			groceryListView = FindViewById<ListView> (Resource.Id.groceryListView);
			groceryListView.ItemClick += GroceryItemClicked;

			// Create a new database table to store the grocery list items
			DBManager.Instance.CreateTable ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			// Update the grocery list
			getGroceryList ();
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);

			// Save the current scroll position
			int currentPosition = groceryListView.FirstVisiblePosition;
			outState.PutInt ("scroll_position", currentPosition);
		}



		protected override void OnRestoreInstanceState (Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState (savedInstanceState);

			// Set the last know scroll position
			scrollPosition = savedInstanceState.GetInt ("scroll_position");
		}

		/***************************************/
		/** Method to create the menu options **/
		/***************************************/
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			// Inflate menus to be shown on ActionBar
			MenuInflater.Inflate(Resource.Menu.GroceryListViewMenu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		/****************************************************/
		/** Method to prepare the menu options for display **/
		/****************************************************/
		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			base.OnPrepareOptionsMenu (menu);
			return true;
		}

		/*********************************************/
		/** Method to handle selecting menu options **/
		/*********************************************/
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				// New button (plus sign)
				case Resource.Id.actionNew:
					Intent intent = new Intent (this, typeof(GroceryItemDetailActivity));
					StartActivity (intent);
					return true;
				default:
					return base.OnOptionsItemSelected (item);
			}
		}

		/*******************************************/
		/** Method to handle clicking a list item **/
		/*******************************************/
		protected void GroceryItemClicked(object sender, ListView.ItemClickEventArgs e)
		{
			GroceryListItem listItem = groceryListData [(int)e.Id];
			int listIndex = (int)e.Id;

			// Create a new Intent object to pass to GroceryItemDetailActivity
			Intent listItemDetailIntent = new Intent(this, typeof(GroceryItemDetailActivity));

			// Serialize the selected item into a Json string
			string listItemJson = JsonConvert.SerializeObject (listItem);

			// Add the Json object to the Intent
			listItemDetailIntent.PutExtra ("listItem", listItemJson);

			// Call GroceryItemDetailActivity and pass the selected list item
			StartActivity (listItemDetailIntent);
		}

		/********************************************************/
		/** Method to load grocery item data from the database **/
		/********************************************************/
		public void getGroceryList() {
			groceryListData = DBManager.Instance.getGroceryListFromDatabase ();

			groceryListAdapter = new GroceryListViewAdapter (this, groceryListData);
			groceryListView.Adapter = groceryListAdapter;
		}
			
	}
}


