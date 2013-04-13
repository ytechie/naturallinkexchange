using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.PayPal;

namespace Nle.Website.Members.Payment_Settings
{
	/// <summary>
	///		This is the page that users are taken to after completing a transaction
	///		through the PayPal system.
	/// </summary>
	public partial class PayPal_Return : Page
	{
		/// <summary>
		///		The relative path to this page.
		/// </summary>
		public const string MY_PATH = "Members/Payment-Settings/";
		/// <summary>
		///		The file name of this page.
		/// </summary>
		public const string MY_FILE_NAME = "PayPal-Return.aspx";

		private Database _db;
        private MainMaster _master;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected void Page_Load(object sender, EventArgs e)
		{
			_log.Debug("Loading PayPal return page");

            _master = (MainMaster)Master;
            _master.AddStylesheet("Members.css");

			_db = Global.GetDbConnection();		
		}

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <param name="full">
		///		If true, the host name and virtual directory are appended to the URL
		///		so that it can be used externally.
		/// </param>
		/// <returns></returns>
		public static string GetLoadUrl(bool full)
		{
			UrlBuilder url;

			if(full)
				url = new UrlBuilder(Global.Domain + Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			else
				url = new UrlBuilder(MY_PATH + MY_FILE_NAME);

			return url.ToString();
		}
	}
}