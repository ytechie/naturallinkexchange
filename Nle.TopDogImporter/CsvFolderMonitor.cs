using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using Microsoft.Win32;
using Nle.Db;
using Nle.Db.SqlServer;
using Nle.Ranking.TopDogPro;
using YTech.General;

namespace Nle.Services.TopDogImporter
{
	/// <summary>
	///		Monitors a folder for CSV files, and consumes them when they appear.
	/// </summary>
	/// <remarks>
	///		To install use the following command line:
	///			installutil Nle.TopDogImporter.exe
	///		To uninstall use the following command line:
	///			installutil /u Nle.TopDogImporter.exe
	///	</remarks>
	public class CsvFolderMonitor : NlePollingServiceBase
	{
		private FileSystemWatcher _folderWatcher;
		private Database _db;
		private EventLog _eventLog;
		private string _watchFolder;

		private const string EVENT_LOG_SOURCE = SERVICE_NAME;
		private const string REG_APP_NAME = "TopDogImportService";
		private const string REG_MONITOR_FOLDER = "MonitorFolder";

		public const string SERVICE_NAME = "NLE TopDog CSV Importer";
		public const string SERVICE_DESCRIPTION = "Monitors a folder for CSV output files from TopDog Pro, and imports them into NLE";

		/// <summary> 
		///		Required designer variable.
		/// </summary>
		private Container components = null;

		public CsvFolderMonitor()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.CanShutdown = true;
			this.ServiceName = SERVICE_NAME;
		}

		#endregion

		// The main entry point for the process
		private static void Main()
		{
			ServiceBase[] ServicesToRun;

			ServicesToRun = new ServiceBase[] {new CsvFolderMonitor()};
			ServiceBase.Run(ServicesToRun);	
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
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			base.RunOnStart = true; //Set this before calling the base method
			base.OnStart(args);

			setUpEventLog();
			createDbConnection();
			loadConfiguration();
			startMonitoring();
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
			//Check if there are files in the folder that didn't get
			//caught by the file system watcher
			processFiles();
		}

		protected override TimeSpan DefaultPollInterval
		{
			get
			{
				return TimeSpan.FromHours(1);
			}
		}

		private void loadConfiguration()
		{
			_watchFolder = base.ReadRegistrySetting(REG_MONITOR_FOLDER);

			if(_watchFolder == null)
				throw new ApplicationException("Could not load watch folder path from the registry, it is required");
		}

		private void setUpEventLog()
		{
			if (!EventLog.SourceExists(EVENT_LOG_SOURCE))
				EventLog.CreateEventSource(EVENT_LOG_SOURCE, "Application");

			_eventLog = new EventLog();
			_eventLog.Source = EVENT_LOG_SOURCE;
		}

		private void startMonitoring()
		{
			_folderWatcher = new FileSystemWatcher(_watchFolder, "*.csv");
			_folderWatcher.EnableRaisingEvents = true;
			_folderWatcher.Created += new FileSystemEventHandler(folderWatcher_Created);
			_folderWatcher.Changed += new FileSystemEventHandler(folderWatcher_Changed);

			_eventLog.WriteEntry("File Monitoring Is Now Enabled");
		}

		/// <summary>
		///		Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			// TODO: Add code here to perform any tear-down necessary to stop your service.
		}

		private void processFiles()
		{
			string[] fileNames;

			_eventLog.WriteEntry("Processing Files");
			fileNames = Directory.GetFiles(_watchFolder, "*.csv");

			foreach (string currFileName in fileNames)
			{
				processFile(currFileName);
			}
		}

		private void processFile(string fileName)
		{
			FileStream fs;
			byte[] buffer;
			string fileText;

			try
			{
				fs = File.OpenRead(fileName);
				try
				{
					buffer = new byte[fs.Length];
					fs.Read(buffer, 0, (int) fs.Length);
				}
				finally
				{
					fs.Close();
				}

				//Covert the bytes to text
				fileText = ASCIIEncoding.ASCII.GetString(buffer, 0, buffer.Length);
				processFileData(fileText);

				//Delete the file now that we are done
				File.Delete(fileName);
			}
			catch (Exception ex)
			{
				_eventLog.WriteEntry(string.Format("Error Processing File '{0}', Reason: {1}", fileName, ex.Message));
			}
		}

		private void processFileData(string fileText)
		{
			Ranking.TopDogPro.Ranking[] rankings;

			rankings = CsvParser.ParseResults(fileText);
			_db.SaveRankings(rankings);
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the database.
		/// </summary>
		private void createDbConnection()
		{
			_db = ConnectionFactory.GetDbConnection("xxx");
		}

		#region File System Events

		private void folderWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			_eventLog.WriteEntry("File Change Detected");

			processFiles();
		}

		private void folderWatcher_Created(object sender, FileSystemEventArgs e)
		{
			_eventLog.WriteEntry("A New CSV File Has Been Detected");

			//A file has been created, load it.
			processFiles();
		}

		#endregion

	}
}