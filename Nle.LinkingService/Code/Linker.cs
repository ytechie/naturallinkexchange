using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using Nle.LinkPage;
using Nle.Db;
using Nle.Db.SqlServer;
using log4net;

namespace Nle.LinkingService
{
    public class Linker : Nle.Services.NlePollingServiceBaseV2
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     This corresponds to the ID in the database.
        /// </summary>
        private const int SERVICE_ID = 3;
        
        /// <summary>
        ///     Creates a new instance of the <see cref="Linker"/> class.
        /// </summary>
        public Linker()
        {
            _log.DebugFormat("Linking service created with a service ID of {0}", SERVICE_ID);
        }

        protected override void RunCycle()
        {
            ArticleMaintenance am;
            Database _db;

            _log.Debug("Getting database connection");
            _db = GetDbConnection();

            _log.Debug("Creating an instance of the ArticleMaintenance class");
            am = new ArticleMaintenance(_db);
            _log.Debug("Running the article maintenance cycle");
            am.Run();

            _log.Debug("Article maintenance cycle complete");
        }

        /// <summary>
        ///		Creates and prepares a <see cref="Database"/> object that
        ///		is ready to interact with the database.
        /// </summary>
        /// <returns></returns>
        private static Database GetDbConnection()
        {
            _log.Debug("Getting database connection from the connection factory");
            return ConnectionFactory.GetDbConnection();
            _log.Debug("Database connection retrieved successfully");
        }

        protected override Database GetDatabase()
        {
            return GetDbConnection();
        }

        protected override int ServiceId
        {
            get { return SERVICE_ID; }
        }
    }
}