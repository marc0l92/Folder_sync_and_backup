using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SyncServerUnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void SqlTest_newUser()
		{
			sync_server.SyncSQLite syncSql = new sync_server.SyncSQLite();
			syncSql.destroyDatabase();
			syncSql = new sync_server.SyncSQLite();
			Assert.AreEqual(true, syncSql.newUser("admin", "admin", "/"));
			Assert.AreEqual(false, syncSql.newUser("admin", "admin", "/"));
			syncSql.destroyDatabase();
		}

		[TestMethod]
		public void SqlTest_deleteUser()
		{
			sync_server.SyncSQLite syncSql = new sync_server.SyncSQLite();
			syncSql.destroyDatabase();
			syncSql = new sync_server.SyncSQLite();
			Assert.AreEqual(true, syncSql.newUser("admin", "admin", "/"));
			Assert.AreEqual(true, syncSql.deleteUser("admin"));
			syncSql.destroyDatabase();
		}
	}
}
