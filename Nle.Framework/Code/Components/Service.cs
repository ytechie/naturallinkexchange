using System;
using System.Collections.Generic;
using System.Text;
using YTech.Db.SqlServer;
using YTech.General.DataMapping;

namespace Nle.Components
{
    /// <summary>
    ///     Represents the information about a particular NLE service such
    ///     as the email engine, or RSS fead reader.
    /// </summary>
    public class Service
    {
        private int _id;
        private string _description;
        private DateTime _lastHeartbeat;
        private int _runIntervalMinutes;
        private bool _enabled;
        private DateTime _lastRunTime;
        private bool _reloadConfiguration;
        private bool _forceRun;

        private bool _hasId = false;

        /// <summary>
        ///     Creates a new instance of the <see cref="Service"/> class.
        /// </summary>
        public Service()
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="Service"/> class that
        ///     represents the service with the specified Id.
        /// </summary>
        /// <param name="id">
        ///     The ID of the service being created.
        /// </param>
        public Service(int id)
        {
            Id = id;
        }

        /// <summary>
        ///     If true, this object has been assigned a database ID.
        /// </summary>
        public bool HasId
        {
            get { return _hasId; }
        }

        /// <summary>
        ///     The unique ID of the service.
        /// </summary>
        [FieldMapping("Id")]
        public int Id
        {
            get 
            {
                return _id; 
            }
            set 
            {
                _id = value;
                _hasId = true;
            }
        }

        /// <summary>
        ///     A short description of this service.
        /// </summary>
        [FieldMapping("Description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     The last time that the service "checked-in" to
        ///     tell the database that it was working alright.
        /// </summary>
        [FieldMapping("LastHeartbeat")]
        public DateTime LastHeartbeat
        {
            get { return _lastHeartbeat; }
            set { _lastHeartbeat = value; }
        }

        /// <summary>
        ///     If this is a polling service, the number of minutes
        ///     between each sucessive working cycle.
        /// </summary>
        [FieldMapping("RunIntervalMinutes")]
        public int RunIntervalMinutes
        {
            get { return _runIntervalMinutes; }
            set { _runIntervalMinutes = value; }
        }

        /// <summary>
        ///     Gets or sets whether or not the service should be doing
        ///     work.
        /// </summary>
        /// <remarks>
        ///     If a service is not enabled (enabled=false), then the service should
        ///     look at this field, and wait for this value to change.
        /// </remarks>
        [FieldMapping("Enabled")]
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        ///     Gets or sets the time that the service last ran.
        /// </summary>
        [FieldMapping("LastRunTime")]
        public DateTime LastRunTime
        {
            get { return _lastRunTime; }
            set { _lastRunTime = value; }
        }

        /// <summary>
        ///     If true, the service will reload its configuration during the next heartbeat.
        /// </summary>
        [FieldMapping("ReloadConfiguration")]
        public Boolean ReloadConfiguration
        {
            get { return _reloadConfiguration; }
            set { _reloadConfiguration = value; }
        }

        /// <summary>
        ///     If true, a cycle will be run during the next configuration load.
        /// </summary>
        /// <remarks>
        ///     Configuration reloads typically happen during a heartbeat cycle.
        /// </remarks>
        [FieldMapping("ForceRun")]
        public Boolean ForceRun
        {
            get { return _forceRun; }
            set { _forceRun = value; }
        }
    }
}
