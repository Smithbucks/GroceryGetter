
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace GroceryGetter
{
	[Activity (Label = "GroceryItemDetailActivity")]			
	public class GroceryItemDetailActivity : Activity
	{
		/***********************************/
		/** Method properties and objects **/
		/***********************************/
		private GroceryListItem _groceryListItem;

		private EditText _itemNameText;
		private EditText _itemQtyText;

		private bool isNewListItem;

		/*********************/
		/** Activity States **/
		/*********************/
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.GroceryItemDetail);

			// Bind properties to text fields
			_itemNameText = FindViewById<EditText> (Resource.Id.txtGroceryItemName);
			_itemQtyText = FindViewById<EditText> (Resource.Id.txtGroceryQtyValue);

			// Determine if the input Intent has any extra content, and if so, deserialize it into a new GroceryListItem object.
			// Otherwise, create a new GroceryListItem.
			if (Intent.HasExtra ("listItem")) {
				string listItemJson = Intent.GetStringExtra ("listItem");
				_groceryListItem = JsonConvert.DeserializeObject<GroceryListItem> (listItemJson);

				isNewListItem = false;
			} else
			{
				_groceryListItem = new GroceryListItem ();
				isNewListItem = true;
			}	

			// Update the user interface with the appropriate input data
			UpdateUI();
		}

		/*******************************************/
		/** Method to update text boxes in the UI **/
		/*******************************************/
		protected void UpdateUI()
		{
			_itemNameText.Text = _groceryListItem.ItemName;
			_itemQtyText.Text = _groceryListItem.ItemQty;
		}

		/***************************************/
		/** Method to create the menu options **/
		/***************************************/
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.GroceryItemDetailMenu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		/****************************************************/
		/** Method to prepare the menu options for display **/
		/****************************************************/
		public override bool OnPrepareOptionsMenu (IMenu menu)
		{
			base.OnPrepareOptionsMenu (menu);

			// Disable delete for a new GroceryListItem
			if (isNewListItem)
			{
				IMenuItem item = menu.FindItem (Resource.Id.actionDelete);
				item.SetEnabled (false);
				item.SetVisible (false);
			}

			return true;
		}

		/*********************************************/
		/** Method to handle selecting menu options **/
		/*********************************************/
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				// Save button (disk)
				case Resource.Id.actionSave:
					SaveGroceryListItem ();
					return true;
				// Delete button (garbge can)
				case Resource.Id.actionDelete:
					DeleteGroceryListItem ();
					return true;
				// Cancel button (large X)
				case Resource.Id.actionCancel:
					Finish ();
					return true;

				default:
					return base.OnOptionsItemSelected (item);
			}
		}

		/******************************************/
		/** Method to save the current list item **/
		/******************************************/
		protected void SaveGroceryListItem()
		{
			bool errors = false;

			// Display error if the Name is empty
			if (String.IsNullOrEmpty (_itemNameText.Text))
			{
				_itemNameText.Error = "Name cannot be empty";
				errors = true;
			}
			else
			{
				_itemNameText.Error = null;
			}

			// If an error was found, exit the method without saving
			if (errors) {
				return;
			}
				
			// Update the GroceryListItem object with data from the text fields
			_groceryListItem.ItemName = _itemNameText.Text;
			_groceryListItem.ItemQty = _itemQtyText.Text;

			// If the current list item is new, perform the "create" database action.
			// Otherwise, perform the "save" (update) database action.
			if (isNewListItem) {
				DBManager.Instance.CreateGroceryListItem (_groceryListItem);
			} else {
				DBManager.Instance.SaveGroceryListItem (_groceryListItem);
			}

			// Display a Toast message to confirm the save action.
			Toast toast = Toast.MakeText (this, String.Format("{0} saved.",_groceryListItem.ItemName),ToastLength.Short);
			toast.Show ();

			// Finish the activity
			Finish ();
		}

		/******************************************************************/
		/** If a delete action is confirmed, delete the item from the DB **/
		/******************************************************************/
		protected void ConfirmDelete(object sender, EventArgs e)
		{
			// Delete the current list item from the DB
			DBManager.Instance.DeleteGroceryListItem (_groceryListItem.Id);

			// Display a Toast message to confirm the delete action 
			Toast toast = Toast.MakeText (this, String.Format("{0} deleted.",_groceryListItem.ItemName),ToastLength.Short);
			toast.Show ();

			// Finish the activity
			Finish();
		}

		/*****************************************************/
		/** Method called when the delete button is clicked **/
		/*****************************************************/
		protected void DeleteGroceryListItem()
		{
			// Set up and display an alert dialog to confirm the delete.
			// If the OK button is cliked, call ConfirmDelete() to complete the delete action.
			AlertDialog.Builder alertConfirm = new AlertDialog.Builder (this);
			alertConfirm.SetTitle ("Confirm delete");

			alertConfirm.SetCancelable (false);
			alertConfirm.SetPositiveButton ("OK", ConfirmDelete);
			alertConfirm.SetNegativeButton ("Cancel", delegate {});
			alertConfirm.SetMessage (String.Format ("Are you sure you want to delete {0}?", _groceryListItem.ItemName));
			alertConfirm.Show ();
		}
	}
}

