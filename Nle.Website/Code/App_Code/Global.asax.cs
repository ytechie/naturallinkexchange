using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Configuration;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.Db.SqlServer;
using log4net;
using YTech.General.Web;

namespace Nle.Website
{
	/// <summary>
	///		The centralized <see cref="HttpApplication"/> object
	///		for this application.
	/// </summary>
	public class Global : HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

        public const string COOKIE_SITE_ID = "SelectedSiteId";

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		//User roles
		public const string ROLE_USER = "User";
		public const string ROLE_ADMINISTRATOR = "Administrator";

		public const string SESSION_USER = "ur";

		public const string COOKIE_REFERRAL = "NaturalLinkExchangeReferralId";

#if SANDBOX
		public const string CONFIG_DB_CONNECTION_STRING = "DbSandboxConnectionString";
#else
		public const string CONFIG_DB_CONNECTION_STRING = "DbConnectionString";
#endif		

		private static string _dbConnectionString;

		public Global()
		{
			InitializeComponent();
		}

		protected void Application_Start(Object sender, EventArgs e)
		{
            ConnectionStringSettings connStringSettings;

			_log.Info("Application Logging Started");

            connStringSettings = ConfigurationManager.ConnectionStrings[CONFIG_DB_CONNECTION_STRING];
            _dbConnectionString = connStringSettings.ConnectionString;

			_log.DebugFormat("Loaded database connection string: '{0}'", _dbConnectionString);
		}

		protected void Session_Start(Object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			RedirectUtilities.EnforceWwwPrefix("http://NaturalLinkExchange.com");
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
//			User sessionUser;
//			IPrincipal formsAuthUser;
//			int userId;
//			Database db;
//
//			if(HttpContext.Current == null)
//				return;
//
//			if(HttpContext.Current.Session == null)
//				return;
// 
//			//Get the user from the cookie
//			formsAuthUser = HttpContext.Current.User;
//
//			//Get the user from the session
//			sessionUser = GetCurrentUser();
//
//			//If there is no user in the session, but there is one in the cookie,
//			//they must be a returning visitor
//			if(sessionUser == null && formsAuthUser.Identity.IsAuthenticated)
//			{
//				_log.Debug("Credentials found in cookie, but not in the session; populating session.") ;
//
//				//Load the user name into the cookie to populate the session
//				userId = int.Parse(formsAuthUser.Identity.Name);
//
//				sessionUser = new User(userId);
//				db = Global.GetDbConnection();
//				db.PopulateUser(sessionUser);
//
//				HttpContext.Current.Session.Add(Global.SESSION_USER, sessionUser);
//			}
		}

		/// <summary>
		///		Handles errors that are not handled at the page level.
		/// </summary>
		/// <remarks>
		///		If an error occurs localally while in debug mode, then the logging
		///		level of the exception is decreased to "Info" instead of "Fatal".
		///		That saves the system from using email notifications to contact
		///		the system administrators.
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Error(Object sender, EventArgs e)
		{
			Exception lastError;
			bool local = false;
            string personalization;

			lastError = Server.GetLastError();
				
#if DEBUG
			if(Request != null)
			{
				if(Request.ServerVariables["REMOTE_HOST"] != null)	
				{
					if(Request.ServerVariables["REMOTE_HOST"].ToLower() == "localhost" || Request.ServerVariables["REMOTE_HOST"].ToLower() == "127.0.0.1")
					{
						local = true;
					}
				}
			}
#endif

            personalization = GetCurrentUserId() > 0 ? " for user " + GetCurrentUserId().ToString() : string.Empty;

			if(lastError == null)
			{
				if(local)
					_log.Info("An Application Error Has Occurred, and I can't retrieve it" + personalization);
				else
					_log.Fatal("An Application Error Has Occurred, and I can't retrieve it" + personalization);
			}
			else
			{
				lastError = lastError.GetBaseException();
			}

			if(local)
				_log.Info("An Application Error Has Occurred" + personalization, lastError);
			else
				_log.Fatal("An Application Error Has Occurred" + personalization, lastError);

            if(!local && !Request.RawUrl.ToLower().Contains("/exception/"))
            {
                try
                {
                    Server.ClearError();
                    Server.Transfer("~/Exception/default.aspx");
                }
                catch
                {
                    Response.Redirect("~/Exception/");
                }
            }
		}

		protected void Session_End(Object sender, EventArgs e)
		{
		}

		protected void Application_End(Object sender, EventArgs e)
		{
		}

		#region Web Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		#endregion

		/// <summary>
		///		Gets the virtual directory of the current application.
		/// </summary>
		public static string VirtualDirectory
		{
			get
			{
				if (HttpContext.Current.Request.ApplicationPath == "/")
					return "/";
				else
					return HttpContext.Current.Request.ApplicationPath + "/";
			}
		}

		/// <summary>
		///		Gets the domain of the current application.
		/// </summary>
		public static string Domain
		{
			get { return HttpContext.Current.Request.Url.Host; }
		}

		/// <summary>
		///		Creates and prepares a <see cref="Database"/> object that
		///		is ready to interact with the database.
		/// </summary>
		/// <returns></returns>
		public static Database GetDbConnection()
		{            
			return Nle.Db.ConnectionFactory.GetDbConnectionFromConnectionString(_dbConnectionString);
		}

		/// <summary>
		///		Gets the user of the currently authenticated user, if the
		///		<see cref="HttpContext"/> is available for the request.
		/// </summary>
		/// <returns>
		///		The user Id of the logged in user.  If the user is not logged
		///		in, -1 is returned.
		/// </returns>
		public static int GetCurrentUserId()
		{
			string userIdString;
			HttpContext httpContext;
			IPrincipal user;

			httpContext = HttpContext.Current;

			if(httpContext == null)
				return -1;

			user = HttpContext.Current.User;

			if(!user.Identity.IsAuthenticated)
				return -1;

			userIdString = user.Identity.Name;
			
			return int.Parse(userIdString);
		}

        /// <summary>
        ///     Gets the currently selected site Id for the currently
        ///     logged on user, using the current <see cref="HttpContext"/>.
        /// </summary>
        /// <returns>
        ///     The Id of the site that is selected, otherwise -1 if the
        ///     context could not be loaded, or the cookie was not found.
        /// </returns>
        public static int GetCurrentSiteId()
        {
            System.Web.HttpCookie cookie;
            HttpContext context;

            context = HttpContext.Current;

            if (context == null)
                return -1;

            cookie = context.Request.Cookies[COOKIE_SITE_ID];
            if (cookie == null)
                return -1;
            else
                return int.Parse(cookie.Value);
        }

//		public static User GetCurrentUser()
//		{
//			object userObj;
//
//			if(HttpContext.Current == null)
//				return null;
//
//			if(HttpContext.Current.Session == null)
//				return null;
//
//			userObj = HttpContext.Current.Session[Global.SESSION_USER];
//
//			if(userObj == null)
//			{
//				return null;
//			}
//			else
//			{
//				return (User)userObj;
//			}
//		}
	}
}