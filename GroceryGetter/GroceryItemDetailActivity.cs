
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
		private GroceryListItem _groceryListItem;

		private EditText _itemNameText;
		private EditText _itemQtyText;

		private bool isNewListItem;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.GroceryItemDetail);

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

		// Method to update text boxes in the UI
		protected void UpdateUI()
		{
			_itemNameText.Text = _groceryListItem.ItemName;
			_itemQtyText.Text = _groceryListItem.ItemQty;
		}

		// Method to create the menu options
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.GroceryItemDetailMenu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		// Method to prepare the menu options for display
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

		// Method to handle selecting menu options
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.actionSave:
					SaveGroceryListItem ();
					return true;

				case Resource.Id.actionDelete:
					DeleteGroceryListItem ();
					return true;

				case Resource.Id.actionCancel:
					Finish ();
					return true;

				default:
					return base.OnOptionsItemSelected (item);
			}
		}

		// Method to save the list item
		protected void SaveGroceryListItem()
		{
			bool errors = false;

			if (String.IsNullOrEmpty (_itemNameText.Text))
			{
				_itemNameText.Error = "Name cannot be empty";
				errors = true;
			}
			else
			{
				_itemNameText.Error = null;
			}

			if (errors) {
				return;
			}
				
			_groceryListItem.ItemName = _itemNameText.Text;
			_groceryListItem.ItemQty = _itemQtyText.Text;

			if (isNewListItem) {
				DBManager.Instance.CreateGroceryListItem (_groceryListItem);
			} else {
				DBManager.Instance.SaveGroceryListItem (_groceryListItem);
			}

			Toast toast = Toast.MakeText (this, String.Format("{0} saved.",_groceryListItem.ItemName),ToastLength.Short);
			toast.Show ();
			Finish ();
		}

		protected void ConfirmDelete(object sender, EventArgs e)
		{
			Toast toast = Toast.MakeText (this, String.Format("{0} deleted.",_groceryListItem.ItemName),ToastLength.Short);
			toast.Show ();

			DBManager.Instance.DeleteGroceryListItem (_groceryListItem.Id);
			Finish();
		}

		// Method to delete the list item
		protected void DeleteGroceryListItem()
		{
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

