using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Net.Mail;
using System.Threading;
using Microsoft.Win32;
using Nle.Components;
using Nle.Db;
using Nle.Db.SqlServer;
using YTech.General;
using Nle.Services;
using log4net;
using System.Diagnostics;

namespace Nle.EmailEngine
{
	/// <summary>
	///		The service that sends queued emails from the system.
	/// </summary>
    public class Emailer : Nle.Services.NlePollingServiceBaseV2
	{
        private const int SERVICE_ID = 1;
		private Database _db;
		private string _mailServer;

        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		///		Creates a new instance of the emailer service.
		/// </summary>
		public Emailer()
		{
		}

        protected override int ServiceId
        {
            get { return SERVICE_ID; }
        }

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			GlobalSetting gs;
			Exception ex;

			_log.Debug("Email engine OnStart called");

			_db = GetDatabase();

			_log.Debug("Database connection created");
			
			gs = _db.GetGlobalSetting(8);
			if (gs == null || gs.TextValue == null)
			{
				ex = new Exception("Could not find the SMTP server name in the database");
				_log.Error("The SMTP server name could not be found in the database");
				throw ex;
			}

			_mailServer = gs.TextValue;

            _log.DebugFormat("Loaded mail server address from database: '{0}'", _mailServer);

			base.OnStart(args);
		}

		protected override void RunCycle()
		{
			pollForMail();
		}

		private void pollForMail()
		{
			EmailMessage queuedMessage;
			MailMessage msg;
			DateTime serverTime;
			bool sendFailure;
			User user;
			SmtpClient _smtpServer;

            _log.Debug("Polling for a queued email");
			
			//Load the messages one at a time, send them, and mark them as processed.
			queuedMessage = _db.GetNextQueuedEmail();
			if(queuedMessage == null)
			{
                _log.Debug("No messages found in the queue that meet the requirements");
				//there are no messages to send
				return;
			}

			//Get the date from the database server
			serverTime = _db.GetServerTime();
						
			//Set the name of the mail server to send through
			_smtpServer = new SmtpClient(_mailServer);
			_smtpServer.Credentials = new System.Net.NetworkCredential("SystemMailer@NaturalLinkExchange.com", "mailer1");
						
			sendFailure = false;

            try
            {
                if (queuedMessage.UserId > 0)
                {
                    _log.DebugFormat("Formatting the email message for user {0}", queuedMessage.UserId);
                    user = new User(queuedMessage.UserId);
                    _db.PopulateUser(user);
                    queuedMessage.ApplyEmailTo(user);
                }

                //Get the MailMessage object to send
                msg = queuedMessage.GetMailMessage();

                _log.Debug("About to send the message");
                _smtpServer.Send(msg);
            }
            catch (Exception ex)
            {
                _log.Error("There was an error sending the message", ex);

                queuedMessage.LastTry = serverTime;
                queuedMessage.NumberOfTries++;

                sendFailure = true;

                //Note: we don't need to return here, because this message should not
                //show up in the list of emails that needs to be sent for a while.
            }
            finally
            {
                //Put in an artificial delay so that we don't send out emails too fast
                System.Threading.Thread.Sleep(3000);
            }

			if(!sendFailure)
			{
				queuedMessage.SentOn = serverTime;	
			}

			//In any case, we need to save the email with the new information
			_db.SaveEmail(queuedMessage);
			
			//Since we found an email the last time, lets check to see if there is another
			pollForMail();
		}

        /// <summary>
        ///		Creates and prepares a <see cref="Database"/> object that
        ///		is ready to interact with the database.
        /// </summary>
        /// <returns></returns>
        protected override Database GetDatabase()
        {
            return ConnectionFactory.GetDbConnection();
        }
    }
}