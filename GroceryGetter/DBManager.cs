using System;
using System.Collections.Generic;
using SQLite;

namespace GroceryGetter
{
	public class DBManager
	{
		private const string DB_NAME = "GroceryList_DB.db3";

		private static readonly DBManager instance = new DBManager();

		SQLiteConnection dbConn;

		private DBManager ()
		{
		}

		public static DBManager Instance {
			get { return instance; }
		}

		public void CreateTable()
		{
			var path = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			dbConn = new SQLiteConnection (System.IO.Path.Combine (path, DB_NAME));

			dbConn.CreateTable<GroceryListItem> ();
		}

		public int CreateGroceryListItem (GroceryListItem item)
		{
			int result = dbConn.Insert (item);
			//int result = dbConn.InsertOrReplace (item);
			Console.WriteLine ("{0} record updated!", result);

			return result;
		}

		public int SaveGroceryListItem (GroceryListItem item)
		{
			int result = dbConn.Update (item);
			//int result = dbConn.InsertOrReplace (item);
			Console.WriteLine ("{0} record updated!", result);

			return result;
		}

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
			
		public GroceryListItem getListItem(int itemId)
		{
			GroceryListItem listItem = dbConn.Table<GroceryListItem> ().Where (a => a.Id.Equals(itemId)).FirstOrDefault();

			return listItem;
		}

		public int DeleteGroceryListItem (int itemId)
		{
			int result = dbConn.Delete<GroceryListItem> (itemId);
			Console.WriteLine ("{0} records affected!", result);

			return result;
		}

		public int clearGroceryListDB()
		{
			int result = dbConn.DeleteAll<GroceryListItem> ();
			Console.WriteLine ("{0} records affected!", result);

			return result;
		}
	}
}

