using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web.JavaScript;

namespace Nle.Website.Members.Manage_Site_Parameters
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class ManageSiteParameters : Page
	{
		private int _userId;
		private Database _db;
		Site _site;
		Subscription _subscription;
		LinkPackage _linkPackage;
        MainMaster _master;
        StatusHeader header;

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("ManageLinkArticles.css");
            header = _master.MasterStatusHeader;

			_userId = Global.GetCurrentUserId();
			_db = Global.GetDbConnection();

			getSiteInformation();

			header.SiteChanged += new Nle.Website.Common_Controls.SiteSelector.SiteChangedDelegate(siteSelector_SiteChanged);
			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			initCancelConfirm();

			if(!Page.IsPostBack)
			{
				header.FilterIncompleteSites = true;

				loadSettings();
			}
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

		private void getSiteInformation()
		{
			Site site;
			site = new Site(header.GetSelectedSiteId());
			_db.PopulateSite(site);
			_site = site;

			_subscription = _db.GetSiteSubscription(_site.Id);

			if(_subscription == null)
				_linkPackage = _db.GetLinkPackage(1);
			else
				_linkPackage = _db.GetLinkPackage(_subscription.PlanId);
		}

		private void initCancelConfirm()
		{
			JavaScriptBlock.ConfirmClick(cmdCancel, "Are you sure you want to lose your changes and return to the control panel?");
		}

		private void loadSettings()
		{
		    int days;

            pnlFreeSilverDesc1.Visible = false;
            pnlFreeSilverDesc2.Visible = false;

            if (_linkPackage.Id == 1)
            {
                txtId1.Enabled = false;
                txtId2.Enabled = false;
                litLinksPerUpdate.Text = string.Format("Your Free site subscription level allows you only <b>{0}</b> link add per day.", _linkPackage.MaxLinksPerCycle);
                descFreeSilver_NextLevel.Text = "Silver or Gold";
                pnlFreeSilverDesc1.Visible = true;
                pnlFreeSilverDesc2.Visible = true;
                //descFreeSilver_NextLevel.Text = "Silver or Gold";
            }
            else 
            {
                litLinksPerUpdate.Text = string.Format("Your site subscription level allows to set your minimum links to add per day to be as low as <b>{0}</b> and as high as <b>{1}</b>.", _linkPackage.MinLinksPerCycle, _linkPackage.MaxLinksPerCycle);
            }

            litArticlesPerPage.Text = string.Format("This option lets you set the number of RSS feed items that will be displayed on your links pages. RSS feeds bring fresh data to the links pages and therefore attract the search engine spiders. The more items you display the higher the chances that the feed will supply fresh data. Your site subscription level allows to set your feeds items per link page between <b>{0}</b> and <b>{1}</b>.", _linkPackage.MinFeedsPerLinkPage, _linkPackage.MaxFeedsPerLinkPage);

            days = (int)Math.Round(((double)_linkPackage.LinkPercentDays / 100.0) * 30.0, 0);
            litFrequency.Text = string.Format("This option is used to determine how often links " + 
            "that point to your site are added to other sites. This is a key feature of Natural Link " + 
            "Exchange because it guarantees that the frequency will be random so the additional links " + 
            "look like they were added naturally. Your site subscription level allows you to set this option " + 
            "to a maximum of <b>{0}%</b>. So you would receive a link approximately every {1} {2} out of 30. ",
            _linkPackage.LinkPercentDays, days, days>1? "days":"day");

			txtId1.Text = _db.GetIntSiteSetting(_site.Id, 1).ToString();
			txtId2.Text = _db.GetIntSiteSetting(_site.Id, 2).ToString();
			txtId3.Text = _db.GetIntSiteSetting(_site.Id, 3).ToString();
			txtId4.Text = _db.GetIntSiteSetting(_site.Id, 4).ToString();
			txtId5.Text = _db.GetIntSiteSetting(_site.Id, 5).ToString();

			string funcCall = JavaScriptBlock.GetFunctionCall("Validate", false,
                JavaScriptBlock.SQuote(txtId1.ClientID), _linkPackage.MinLinksPerCycle.ToString(),
                JavaScriptBlock.SQuote(txtId2.ClientID), _linkPackage.MaxLinksPerCycle.ToString(),
                JavaScriptBlock.SQuote(txtId3.ClientID), _linkPackage.MinFeedsPerLinkPage.ToString(),
                JavaScriptBlock.SQuote(txtId4.ClientID), _linkPackage.MaxFeedsPerLinkPage.ToString(),
                JavaScriptBlock.SQuote(txtId5.ClientID), _linkPackage.LinkPercentDays.ToString());
			cmdSave.Attributes.Add("onclick", "return " + funcCall);
		}

		private void saveSettings()
		{
			if(_site == null) getSiteInformation();

			if(!Page.IsValid)
				return;

			_db.SaveIntSiteSetting(_site.Id,  1, int.Parse(txtId1.Text));
			_db.SaveIntSiteSetting(_site.Id,  2, int.Parse(txtId2.Text));
			_db.SaveIntSiteSetting(_site.Id,  3, int.Parse(txtId3.Text));
			_db.SaveIntSiteSetting(_site.Id,  4, int.Parse(txtId4.Text));
			_db.SaveIntSiteSetting(_site.Id,  5, int.Parse(txtId5.Text));

            Response.Redirect(ResolveUrl("~/Members/Control-Panel/"));
		}

		private void siteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			loadSettings();
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			saveSettings();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
            Response.Redirect(ResolveUrl("~/Members/Control-Panel/"));
		}
	}
}
