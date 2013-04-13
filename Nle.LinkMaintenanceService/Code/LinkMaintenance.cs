using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using Nle.Db;
using Nle.Db.SqlServer;
using Nle.LinkPage;

namespace Nle.Services.LinkMaintenanceService
{
	/// <summary>
	///		A service that periodically runs a maintenance cycle
	///		on the link database.  It does this by using the
	///		<see cref="ArticleMaintenance"/> class.
	/// </summary>
	public class LinkMaintenance : Nle.Services.NlePollingServiceBase
	{
		/// <summary>
		///		The subkey to use to store the data for this service.
		/// </summary>
		public const string REG_SETTINGS_SUB_KEY = "LinkMaintenanceService";

		/// <summary> 
		///		Required designer variable.
		/// </summary>
		private Container components = null;

		private Database _db;

		/// <summary>
		///		Creates a new instance of the <see cref="LinkMaintenance"/> service class.
		/// </summary>
		public LinkMaintenance()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			_db = getDbConnection();
		}
		
		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();

			//This is what shows up in the services list
			ServiceName = "NLE Link Maintenance Service";
		}

		#endregion

		// The main entry point for the process
		private static void Main()
		{
			ServiceBase[] ServicesToRun;

			ServicesToRun = new ServiceBase[] {new LinkMaintenance()};
			ServiceBase.Run(ServicesToRun);
		}

		/// <summary>
		///		Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			base.OnStart(args);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		///		Returns <see cref="REG_SETTINGS_SUB_KEY"/>
		/// </summary>
		protected override string ServiceSubKey
		{
			get
			{
				return REG_SETTINGS_SUB_KEY;
			}
		}

		protected override TimeSpan DefaultPollInterval
		{
			get
			{
				return TimeSpan.FromDays(1.0);
			}
		}

		/// <summary>
		///		This is where we actually do our work.
		/// </summary>
		protected override void RunCycle()
		{
			ArticleMaintenance am;

			am = new ArticleMaintenance(_db);
			am.Run();
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the database.
		/// </summary>
		/// <returns></returns>
		private static Database getDbConnection()
		{
			return ConnectionFactory.GetDbConnection();
		}

	}
}