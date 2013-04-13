using System;
using System.Text;
using System.Reflection;
using Nle.Db.SqlServer;
using YTech.Db.SqlServer;
using log4net;

namespace Nle.Db
{
	/// <summary>
	///		Provides an easy way to create database connections to the
	///		NaturalLinkExchange database.
	/// </summary>
	public class ConnectionFactory
	{
		private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private ConnectionFactory()
		{
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the production database.
		/// </summary>
		/// <returns></returns>
		public static Database GetDbConnection()
		{
			Database db;
			string connectionString;

			_log.Debug("Reading connection string from the database");

			connectionString = Nle.NleRegistry.DbConnection;

			if(connectionString == null || connectionString.Length == 0)
			{
				throw new Exception("Unable to find a valid database connection string in the registry");
			}

			_log.DebugFormat("Connection string successfully loaded as {0}", connectionString);

			db = GetDbConnectionFromConnectionString(connectionString);

			_log.Debug("Successfully created connection");

			return db;
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the production database.
		/// </summary>
		/// <returns></returns>
		[System.Obsolete("The connection information will no longer be stored here")]
		public static Database GetDbConnection(string password)
		{
			Database db;

			db = GetDbConnection("209.132.211.159", "Links", "SuperJason", password);

			return db;
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the specified database.
		/// </summary>
		/// <returns></returns>
		public static Database GetDbConnection(string databaseServerName, string databaseName, string userName, string password)
		{
			StringBuilder sb;

			sb = new StringBuilder();

			sb.Append("Network Library=DBMSSOCN;");
			sb.AppendFormat("Data Source={0};", databaseServerName);
			sb.AppendFormat("Initial Catalog={0};", databaseName);
			sb.AppendFormat("User ID={0};", userName);
			sb.AppendFormat("Password={0};", password);

			return GetDbConnectionFromConnectionString(sb.ToString());
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the specified database.
		/// </summary>
		/// <param name="connectionString">
		///		The full connection string that can be used to connect to
		///		the database.
		///	</param>
		public static Database GetDbConnectionFromConnectionString(string connectionString)
		{
			Database db;
			DbConnection dbConn;

			_log.Debug("Creating database connection");

			dbConn = new DbConnection(connectionString);

			_log.Debug("Initializing database functions");

			db = new Database(dbConn);

			_log.Debug("Database initialization complete");

			return db;
		}
	}
}