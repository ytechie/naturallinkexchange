using System;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;
using Nle.Db;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using YTech.General;
using System.IO;
using log4net;
using log4net.Config;
using System.Reflection;

namespace Nle.Services
{
	/// <summary>
	///		Provides a base for all of the services that use
	///		a pattern of polling.
	/// </summary>
	public abstract class NlePollingServiceBase : ServiceBase
	{
		/// <summary>
		///		The base registry key to use for all settings that are for
		///		the NLE services.
		/// </summary>
		public const string REG_SETTINGS_ROOT_BASE = "Software\\YTech\\NLE\\";

		/// <summary>
		///		The standard value name for the setting that stores the last time
		///		the service was run.  If your service specifies a <see cref="ServiceSubKey"/>,
		///		this value will automatically be populated for you.
		/// </summary>
		public const string REG_LAST_RUNTIME = "LastRuntime";

		/// <summary>
		///		The standard value name for the setting that stores the number
		///		of minutes between each call of <see cref="RunCycle"/>.
		/// </summary>
		public const string REG_POLL_INTERVAL_MINUTES = "PollIntervalMinutes";
        
		/// <summary>
		///		The standard value name for the setting that stores whether
		///		or not the service will run <see cref="RunCycle"/> when it is
		///		first started.
		/// </summary>
		public const string REG_RUN_ON_STARTUP = "RunOnStartup";

		private Timer _pollTimer;
		private bool _runOnStart;
		private TimeSpan _pollInterval;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public NlePollingServiceBase()
		{
            initLogging();
		}

        private void initLogging()
        {
            Uri uri;
            string filePath;
            FileInfo logConfig;

            uri = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
            filePath = Path.GetDirectoryName(uri.LocalPath);

            logConfig = new FileInfo(filePath + System.IO.Path.DirectorySeparatorChar + "Logging.config");
            XmlConfigurator.ConfigureAndWatch(logConfig);

            _log.Info("Logging initialized");
        }

		/// <summary>
		///		Retrieves the subkey to look for the settings for
		///		this particluar service.  This key name will be appended
		///		to the root key name for all services.
		/// </summary>
		/// <example>
		///		If you specify "RssFeedService", the full key name will be:
		///		Software\YTech\Nle\RssFeedService\
		/// </example>
		protected abstract string ServiceSubKey
		{
			get;
		}

		/// <summary>
		///		The default amount of time between calling <see cref="RunCycle"/>.
		/// </summary>
		protected abstract TimeSpan DefaultPollInterval
		{
			get;
		}

		/// <summary>
		///		Gets called when the service should perform the action
		///		that it needs to run.
		/// </summary>
		protected abstract void RunCycle();

		protected bool RunOnStart
		{
			get{	return _runOnStart;	}
			set{	_runOnStart = value;	}
		}
		
		#region Registry Settings

		/// <summary>
		///		Gets the registry key to store all of the settings in.
		/// </summary>
        private string getSettingsRootKey()
        {
            string regLocation;

            if (ServiceSubKey == null || ServiceSubKey.Length == 0)
            {
                _log.Debug("No root path was specified for the server, so settings can't be looked up from the registry");
                return null;
            }

            regLocation = REG_SETTINGS_ROOT_BASE + ServiceSubKey + "\\";
            _log.DebugFormat("Service settings will be loaded from '{0}'", regLocation);

            return regLocation;
        }

		/// <summary>
		///		Reads a value from the registry that is in the section of the registry
		///		specific to this service.
		/// </summary>
		/// <remarks>
		///		The base registry location is determined from a combination of the
		///		<see cref="REG_SETTINGS_ROOT_BASE"/> and the <see cref="SubKey"/>.
		/// </remarks>
		/// <param name="valueName"></param>
		/// <returns>
		///		The value, if found, otherwise NULL.
		/// </returns>
		public string ReadRegistrySetting(string valueName)
		{
			string regVal;
			string rootKey;

			rootKey = getSettingsRootKey();

			regVal = RegistryUtilities.ReadRegistryKey(Registry.LocalMachine, rootKey, valueName);

            _log.DebugFormat("Read registry key '{0}' as value '{1}'", valueName, regVal);

			return regVal;
		}

		/// <summary>
		///		Writes a value to the registry in the section of the registry specific
		///		to this service.
		/// </summary>
		/// <param name="valueName"></param>
		/// <param name="registryValue"></param>
		public void WriteRegistrySetting(string valueName, string registryValue)
		{
			string rootKey;

			rootKey = getSettingsRootKey();

			RegistryUtilities.WriteRegistryKey(Registry.LocalMachine, rootKey, valueName, registryValue);

            _log.DebugFormat("Write registry value '{0}' to key '{1}'", registryValue, valueName);
		}

		private void readRegistrySettings()
		{
			readPollInterval();
			readRunOnStart();
		}

		private void readPollInterval()
		{
			string minuteString;
			int minutes;

            _log.Debug("Reading poll interval");

			minuteString = ReadRegistrySetting(REG_POLL_INTERVAL_MINUTES);

			if(minuteString == null)
			{
				_pollInterval = DefaultPollInterval;
                _log.DebugFormat("The poll interval was not found in the registry, so the default of {0} minutes will be used", DefaultPollInterval.TotalMinutes);
				return;
			}

			try
			{
				minutes = int.Parse(minuteString);
                _log.DebugFormat("Poll interval of '{0}' minutes was found in the registry", minutes);
			}
			catch(Exception)
			{
				_pollInterval = DefaultPollInterval;
                _log.Error("Error parsing the poll interval value from the registry, so the default will be used");
				return;
			}

			_pollInterval = TimeSpan.FromMinutes(minutes);
		}

		private void readRunOnStart()
		{
			string runOnStartString;

			runOnStartString = ReadRegistrySetting(REG_RUN_ON_STARTUP);

			if(runOnStartString == null || runOnStartString.Length == 0)
				_runOnStart = false;
			else if(runOnStartString == "1")
				_runOnStart = true;
			else if(runOnStartString == "0")
				_runOnStart = false;
			else
			{
				try
				{
					_runOnStart = bool.Parse(runOnStartString);
				}
				catch(Exception)
				{
					_runOnStart = false;
				}
			}
		}

		#endregion

		/// <summary>
		///		Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			base.OnStart(args);

            readRegistrySettings();

			//throw new Exception("Poll Interval: " + _pollInterval.TotalMinutes);

			initTimer();
		}

		private void runFullCycle()
		{
			stopPollTimer();
            try
            {
                RunCycle();
                writeLastRunTime();
            }
            catch (Exception ex)
            {
                _log.Error("Critical error has occurred", ex);
            }
            finally
            {
                resetPollTimer();
            }
		}

		/// <summary>
		///		Writes the last time that the service ran to the registry.
		/// </summary>
		/// <remarks>
		///		The time is stored in local time because the services are just run
		///		locally.  If it needs to be stored in UTC, this should be the only
		///		place that you would need to change it.
		/// </remarks>
		private void writeLastRunTime()
		{
            _log.Debug("Writing the last runtime back to the registry");
			WriteRegistrySetting(REG_LAST_RUNTIME, DateTime.Now.ToString());
		}

		private void initTimer()
		{
            _log.Debug("Initializing the polling timer");

            if (_runOnStart)
            {
#if DEBUG
                _pollTimer = new Timer(new TimerCallback(_pollTimer_Tick), null, TimeSpan.FromSeconds(30), _pollInterval);
                _log.Debug("The service is a debug build, so the initial start will be delayed so debuggers can attach");
#else
                _pollTimer = new Timer(new TimerCallback(_pollTimer_Tick), null, TimeSpan.Zero, _pollInterval);
                _log.Debug("The service is a non-debug build, so the polling will begin immediately.");
#endif
            }
            else
            {
                _log.Debug("The RunOnStartup flag was not set, so the polling will begin after the first interval");
                _pollTimer = new Timer(new TimerCallback(_pollTimer_Tick), null, _pollInterval, _pollInterval);
            }
		}

		/// <summary>
		///		Starts the timer back up.
		/// </summary>
		private void resetPollTimer()
		{
            _log.Debug("Resetting the poll timer");
			_pollTimer.Change(_pollInterval, _pollInterval);
		}

		/// <summary>
		///		Stops the timer until further notice.
		/// </summary>
		private void stopPollTimer()
		{
            _log.Debug("Stopping the poll timer");
			_pollTimer.Change(Timeout.Infinite, Timeout.Infinite);
		}

		private void _pollTimer_Tick(object stateData)
		{
            _log.Debug("Poll timer has ticked");
			runFullCycle();
		}
	}
}
