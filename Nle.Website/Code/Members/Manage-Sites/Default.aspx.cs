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
using YTech.General.Web;

namespace Nle.Website.Members.Manage_Sites
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class ManageSitesDefault : Page
	{
		/// <summary>
		///		The path of this page.
		/// </summary>
        public const string MY_PATH = "Members/Manage-Sites/";
        
		/// <summary>
		///		The file name of this page.
		/// </summary>
        public const string MY_FILE_NAME = "";

		private Database _db;
		private int _userId;
        private MainMaster _master;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("Site.css");          

			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

			if(!Page.IsPostBack)
			{
				lnkAddSite.NavigateUrl = EditSite.GetLoadUrl();
				displaySites();
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
            this.Load += new EventHandler(this.Page_Load);
		}
		#endregion

		private void displaySites()
		{
			Site[] sites;

			sites = _db.GetUsersSites(_userId, false);

            if (sites.Length == 0)
                pnlNoSites.Visible = true;

			foreach(Site site in sites)
				displaySite(SitesList, site);
		}

		private void displaySite(PlaceHolder parentControl, Site site)
		{
			HtmlGenericControl siteContainer;
			HtmlGenericControl siteName;
			HtmlGenericControl subscriptionLevel;
			HyperLink siteUrl;
			HyperLink editLink;
			HyperLink upgradeLink;
			Literal br;

			Subscription subscription = _db.GetSiteSubscription(site.Id);
			LinkPackage linkPackage = _db.GetLinkPackage(subscription == null ? 1 : subscription.PlanId);

			siteContainer = new HtmlGenericControl("div");
			parentControl.Controls.Add(siteContainer);
			siteContainer.Attributes.Add("class", "siteContainer");

			siteName = new HtmlGenericControl("h2");
			siteContainer.Controls.Add(siteName);
			siteName.InnerText = site.Name;

			editLink = new HyperLink();
			siteContainer.Controls.Add(editLink);
			editLink.NavigateUrl = EditSite.GetLoadUrl(site.Id);
			editLink.Text = "Edit";
			editLink.CssClass = "siteActionCommand";

			br = new Literal();
			siteContainer.Controls.Add(br);
			br.Text = "<br />";

			siteUrl = new HyperLink();
			siteContainer.Controls.Add(siteUrl);
			siteUrl.NavigateUrl = site.Url;
			siteUrl.Text = site.Url;

			br = new Literal();
			siteContainer.Controls.Add(br);
			br.Text = "<br />";

			subscriptionLevel = new HtmlGenericControl("h5");
			siteContainer.Controls.Add(subscriptionLevel);
			subscriptionLevel.Attributes.Add("class", "linkPackage" + linkPackage.Id);
			subscriptionLevel.InnerText = string.Format("{0} Site", linkPackage.FriendlyName);

			if(linkPackage.Id < 3)
			{
				upgradeLink = new HyperLink();
				siteContainer.Controls.Add(upgradeLink);
				upgradeLink.Text = "Upgrade";
				upgradeLink.NavigateUrl = Payment_Settings._Default.GetLoadUrl(site.Id);
			}
			

			br = new Literal();
			siteContainer.Controls.Add(br);
			br.Text = "<br /><br />";
		}
	}
}
