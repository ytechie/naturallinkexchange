using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;

namespace Nle.Website.Members.Affiliate_Agreement
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class AffiliateAgreementDefault : System.Web.UI.Page
	{
		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Affilate-Agreement/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "";


		private int _userId;
		private Database _db;
		LegalNoticeVersion _version;
		bool _hasAgreed;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_userId = Global.GetCurrentUserId();
			_db = Global.GetDbConnection();
			_version = _db.GetLatestLegalNoticeVersion(LegalNotices.AffiliateTermsOfService);
			_hasAgreed = _db.HasAgreedToNotice(_version.Id, _userId);

			cmdAgree.Click += new EventHandler(cmdAgree_Click);
			cmdDisagree.Click += new EventHandler(cmdDisagree_Click);

			if(!Page.IsPostBack)
			{
				loadPage();
			}
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

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
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
            this.Load += new EventHandler(Page_Load);
		}
		#endregion

		private void cmdAgree_Click(object sender, EventArgs e)
		{
			setAgreement(true);
			_hasAgreed = true;
			loadPage();
		}

		private void cmdDisagree_Click(object sender, EventArgs e)
		{
			setAgreement(false);
			Response.Redirect(ResolveUrl("~/Members/Control-Panel/"));
		}

		private void setAgreement(bool agree)
		{
			LegalNoticeAgreement agreement;

			agreement = new LegalNoticeAgreement();
			agreement.UserId = _userId;
			agreement.LegalNoticeVersionId = (int)LegalNotices.AffiliateTermsOfService;
			agreement.Agree = agree;

			_db.SaveLegalNoticeAgreement(agreement);
		}

		private void loadPage()
		{
			pnlAffiliate.Visible = _hasAgreed;
			pnlNonAffiliate.Visible = !_hasAgreed;
			if(_hasAgreed)
				loadAffiliateStuff();
			else
				loadTerms();
		}

		private void loadAffiliateStuff()
		{
			hypAffiliateLink.Text = string.Format(EmailMessage.FORMAT_REFERRAL_URL, _userId);
			hypAffiliateLink.NavigateUrl = RootDefault.GetLoadUrl(_userId);
		}

		private void loadTerms()
		{
			litTerms.Text = _version.Notice;
		}
	}
}