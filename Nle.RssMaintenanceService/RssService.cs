using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;
using Nle.Db;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using YTech.General;
using Nle.Services;

namespace Nle.RssMaintenanceService
{
	public class RssService : NlePollingServiceBase
	{
		public const string REG_APP_NAME = "RssFeedService";
		private const int DEFAULT_POLL_INTERVAL_MINUTES = 120;

		private Database _db;

		/// <summary> 
		///		Required designer variable.
		/// </summary>
		private Container components = null;

		public RssService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();
		}

		// The main entry point for the process
		private static void Main()
		{
			ServiceBase[] ServicesToRun;

			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new ServiceBase[] {new RssService()};

			ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new Container();

			//This is what shows up in the services list
			ServiceName = "NLE RSS Maintenance Service";
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
		///		Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			base.OnStart(args);

			_db = GetDbConnection();
		}

		protected override TimeSpan DefaultPollInterval
		{
			get
			{
				return TimeSpan.FromMinutes(DEFAULT_POLL_INTERVAL_MINUTES);
			}
		}

		protected override string ServiceSubKey
		{
			get
			{
				return REG_APP_NAME;
			}
		}

		protected override void RunCycle()
		{
			runMaintenanceCycle();
		}

		
		private void runMaintenanceCycle()
		{
			RssFeedMaintenance svc;

			svc = new RssFeedMaintenance(_db);
			svc.Run();
		}

		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the database.
		/// </summary>
		/// <returns></returns>
		public static Database GetDbConnection()
		{
			return ConnectionFactory.GetDbConnection();
		}
	}
}