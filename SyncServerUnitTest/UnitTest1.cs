using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SyncServerUnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void SqlTest()
		{
			sync_server.SyncSQLite syncSql = new sync_server.SyncSQLite();
			
		}
	}
}
