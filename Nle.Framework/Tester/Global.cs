using System;
using Nle.Db.SqlServer;
using NUnit.Framework;
using YTech.Db.SqlServer;

namespace Nle
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global
	{
		static DbConnection _dbconn;
		static Database _db;

		public static string DatabaseConnectionString
		{
			get
			{
				string dbconnstring = NleRegistry.DbConnection;
				if(dbconnstring == null || dbconnstring == string.Empty)
				{
					NleRegistry.DbConnection = string.Empty;
					Assert.Fail("Missing the registry key {0} for the database connection string.", "HKEY_LOCAL_MACHINE\\" + NleRegistry.REGKEY_ROOT + "\\" + NleRegistry.REGKEY_DB_CONNECTION_STRING);
				}
				return dbconnstring;
			}
		}

		public static DbConnection DbConn
		{
			get
			{
				if(_dbconn == null) _dbconn = new DbConnection(DatabaseConnectionString);
				return _dbconn;
			}
		}

		public static Database Db
		{
			get
			{
				if(_db == null) _db = new Database(DbConn);
				return _db;
			}	
		}

		public static void DisableConstraints()
		{
			if(DbConn != null)
			{
				DbConn.DisableContraints();
				DbConn.DisableTriggers();
			}
		}

		public static void EnableConstraints()
		{
			if(DbConn != null)
			{
				DbConn.EnableContraints();
				DbConn.EnableTriggers();
			}
		}

		public static bool DatesMatch(DateTime d1, DateTime d2)
		{
			return d1.Subtract(d2).TotalSeconds < 1 && d1.Subtract(d2).TotalSeconds > -1;
		}
	}
}
