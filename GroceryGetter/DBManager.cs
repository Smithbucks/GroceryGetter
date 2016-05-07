using System;
using System.Collections.Generic;
using SQLite;

namespace GroceryGetter
{
	public class DBManager
	{
		/***************************************/
		/** Define database object properties **/
		/***************************************/
		private const string DB_NAME = "GroceryList_DB.db3";

		// Initialize the DBManager object early to force it as a singleton class
		// (Prevents more than one instance from being defined)
		private static readonly DBManager instance = new DBManager();

		SQLiteConnection dbConn;

		// Constructor (
		private DBManager ()
		{
		}

		/*******************************************************************/
		/** Static method required to access the private DBManager object **/
		/*******************************************************************/
		public static DBManager Instance {
			get { return instance; }
		}

		/*****************************************************************/
		/** Method to create a new database table of grocery list items **/
		/*****************************************************************/
		public void CreateTable()
		{
			var path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			dbConn = new SQLiteConnection (System.IO.Path.Combine (path, DB_NAME));

			dbConn.CreateTable<GroceryListItem> ();
		}

		/***********************************************************/
		/** Method to insert a new grocery item into the database **/
		/***********************************************************/
		public int CreateGroceryListItem (GroceryListItem item)
		{
			int result = dbConn.Insert (item);
			Console.WriteLine ("{0} record updated!", result);

			return result;
		}

		/***********************************************/
		/** Method to update an existing grocery item **/
		/***********************************************/
		public int SaveGroceryListItem (GroceryListItem item)
		{
			int result = dbConn.Update (item);
			//int result = dbConn.InsertOrReplace (item);
			Console.WriteLine ("{0} record updated!", result);

			return result;
		}

		/***********************************************/
		/** Method to retreive all grocery list items **/
		/***********************************************/
		public List<GroceryListItem> getGroceryListFromDatabase()
		{
			var groceryListData = new List<GroceryListItem> ();
			IEnumerable<GroceryListItem> table = dbConn.Table<GroceryListItem> ();

			foreach (GroceryListItem groceryListItem in table)
			{
				groceryListData.Add (groceryListItem);
			}

			return groceryListData;
		}
			
		/***************************************************/
		/** Method to retreive a single grocery list item **/
		/***************************************************/
		public GroceryListItem getListItem(int itemId)
		{
			GroceryListItem listItem = dbConn.Table<GroceryListItem> ().Where (a => a.Id.Equals(itemId)).FirstOrDefault();

			return listItem;
		}

		/*************************************************/
		/** Method to delete a single grocery list item **/
		/*************************************************/
		public int DeleteGroceryListItem (int itemId)
		{
			int result = dbConn.Delete<GroceryListItem> (itemId);
			Console.WriteLine ("{0} records affected!", result);

			return result;
		}

		/**********************************************/
		/** Method to delete the entire grocery list **/
		/**********************************************/
		public int clearGroceryListDB()
		{
			int result = dbConn.DeleteAll<GroceryListItem> ();
			Console.WriteLine ("{0} records affected!", result);

			return result;
		}
	}
}

