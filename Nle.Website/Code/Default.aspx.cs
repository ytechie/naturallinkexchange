using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Nle.Client.RssFeeds;
using Nle.Client.RssFeeds.Controls;
using Nle.Client.RssFeeds.Sources;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web;
using YTech.General.Web.Forms;

namespace Nle.Website
{
	/// <summary>
	///		The main default page of the application.
	/// </summary>
	public partial class RootDefault : Page
	{
		public const string MY_PATH = "";
		public const string MY_FILE_NAME = "";

		protected const string PARAM_REFERRAL_ID = "ReferralId";
        private const string META_KEYWORDS = "exchange, natural, naturally, link, link farm, reciprocal";
        private const string META_DESCRIPTION = "Add Links Naturally - Increasing Your Impact on Search Engine Rankings and Bringing Your Site More Traffic";
        
		protected RequiredFieldValidator RequiredFieldValidator1;
		protected RequiredFieldValidator RequiredFieldValidator2;
		protected RegularExpressionValidator RegularExpressionValidator1;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private Database _db;

		protected void Page_Load(object sender, EventArgs e)
		{
			string referalId;
			HttpCookie cookie;
			LegalNoticeVersion version;

            setupMetaTags();

			_db = Global.GetDbConnection();

			if (!Page.IsPostBack)
				updateRssNews();

			//Check for Referal Id
			referalId = Request.QueryString[PARAM_REFERRAL_ID];
			if(referalId != null)
			{
				version = _db.GetLatestLegalNoticeVersion(LegalNotices.AffiliateTermsOfService);
				if(_db.HasAgreedToNotice(version.LegalNoticeId, int.Parse(referalId)))
				{
					cookie = Request.Cookies[Global.COOKIE_REFERRAL];
					if(cookie == null || cookie.Value == null || cookie.Value == string.Empty)
					{
						cookie = new HttpCookie(Global.COOKIE_REFERRAL, referalId);
						cookie.Expires = DateTime.Now.AddDays(30);
						Response.Cookies.Add(cookie);
					}
				}
			}

			cmdSignupList.Click += new EventHandler(cmdSignupList_Click);
		}

        private void setupMetaTags()
        {
            MainMaster mp = (MainMaster)Page.Master;

            mp.PageKeywords = META_KEYWORDS;
            mp.PageDescription = META_DESCRIPTION;
        }

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl()
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

			return url.ToString();
		}

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl(int referralId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_REFERRAL_ID, referralId);

			return url.ToString();
		}

		#region Web Form Designer generated code

		protected override void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);

		}

		#endregion

		private void cmdSignupList_Click(object sender, EventArgs e)
		{
			FieldPoster poster;
			NameValueCollection postValues;

			//Make sure it passed server side validation
			if (!Page.IsValid)
				return;

			//We are going to post to the AWeber script
			poster = new FieldPoster("http://www.aweber.com/scripts/addlead.pl");

			postValues = new NameValueCollection();
			postValues.Add("PHPSESSID", "a0f2fa9b3c8a0068622af9fcfd976bf4");
			postValues.Add("meta_web_form_id", "658045002");
			postValues.Add("meta_split_id", "");
			postValues.Add("unit", "natural7");
			postValues.Add("meta_adtracking", "NLEFrontPage");
			postValues.Add("meta_message", "1");
			postValues.Add("meta_required", "from");
			postValues.Add("meta_forward_vars", "0");
			postValues.Add("name", txtName.Text);
			postValues.Add("from", txtEmail.Text);

			poster.Post(postValues);

			//Disable the form so they don't try to sign up again.
			txtName.Enabled = false;
			txtEmail.Enabled = false;
			cmdSignupList.Text = "Already Signed Up";
			cmdSignupList.Enabled = false;
		}

		private void updateRssNews()
		{
			FeedDisplay feedDisp;
			IRssFeedSource yahooFeed;

			yahooFeed = new YahooRssSource("Search Engine Optimization");
			feedDisp = new FeedDisplay(yahooFeed);
			NewsPlaceholder.Controls.Add(feedDisp);
		}
	}
}