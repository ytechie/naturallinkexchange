using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;

namespace Nle.Website.Members.Payment_Settings
{
	/// <summary>
	///		The payment settings page.
	/// </summary>
	public partial class _Default : Page
	{
		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Payment-Settings/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "";

		private const string PARAM_SITE_ID = "SiteId";
        
        private Database _db;
		private JavaScriptBlock _scripts;
        private MainMaster _master;
        private StatusHeader header;

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
		public static string GetLoadUrl(int siteId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_SITE_ID, siteId);

			return url.ToString();
		}

		private void getParameters()
		{
			string paramString;

			paramString = Request.QueryString[PARAM_SITE_ID];
			if (paramString != null && paramString.Length > 0)
			{
				int siteId = int.Parse(paramString);
				if (!Page.IsPostBack) header.SetSelectedSiteId(siteId);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("Members.css");
            header = _master.MasterStatusHeader;
            
			getParameters();

			_db = Global.GetDbConnection();

			initJavaScript();

			header.SiteChanged += new SiteSelector.SiteChangedDelegate(siteSelector_SiteChanged);

			if (!Page.IsPostBack)
			{
				populatePlanDetails();
			}
		}

		private void initJavaScript()
		{
			_scripts = new JavaScriptBlock();
			ControlPlaceholder.Controls.Add(_scripts);
		}

		/// <summary>
		///		Populates the labels that display the details
		///		of the plan that the specified site is currently on.
		/// </summary>
		private void populatePlanDetails()
		{
			int siteId;
			Subscription subscription;
			LinkPackage plan;

			siteId = header.GetSelectedSiteId();

			subscription = _db.GetSiteSubscription(siteId);

			//Make sure they even have a subscription
			if (subscription == null)
			{
				lblPlanName.Text = "N/A";
				lblPlanStart.Text = "N/A";
				lblPlanEnd.Text = "N/A";

				//Check for null in case of caching
				if (tblLinkPackages != null)
					tblLinkPackages.HighlightPlan = -1;

				return;
			}

			plan = _db.GetLinkPackage(subscription.PlanId);

			//Populate the form values
			lblPlanName.Text = plan.FriendlyName;
			lblPlanStart.Text = subscription.StartTime.ToLocalTime().ToString();

			if(subscription.EndTime == new DateTime())
				lblPlanEnd.Text = "Unlimited";
			else
				lblPlanEnd.Text = subscription.EndTime.ToLocalTime().ToString();

			if (tblLinkPackages != null)
				tblLinkPackages.HighlightPlan = subscription.PlanId;
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

		private void siteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			populatePlanDetails();
		}
	}
}