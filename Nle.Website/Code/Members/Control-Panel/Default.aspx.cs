using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using Nle.Website.Common_Controls;
using Nle.Website.Members.Link_Page_Setup;
using Nle.Website.Members.Manage_Link_Articles;
using Nle.Website.Members.Manage_Sites;
using YTech.General.Web;
using YTech.General.Web.JavaScript;
using log4net;

namespace Nle.Website.Members
{
	/// <summary>
	///		The control panel for users.
	/// </summary>
	public partial class ControlPanel : Page
	{
        public const string MY_PATH = "Members/Control-Panel/";
        public const string MY_FILE_NAME = "";

		private int _userId;
		private Database _db;
		private JavaScriptBlock _scripts;
        private MainMaster _master;
        private StatusHeader header;

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected HtmlGenericControl pnlConfigStatus;

		protected void Page_Load(object sender, EventArgs e)
		{
			_userId = Global.GetCurrentUserId();
			_db = Global.GetDbConnection();

            _master = (MainMaster)Master;
            _master.AddStylesheet("ControlPanel.css");
            header = _master.MasterStatusHeader;

			initPrivledges();
			initJavaScript();
			initConfigStatus();
            initPromotionSection();
			
			if(!Page.IsPostBack)
			{
				//Add a confirmation to the update rss button
				JavaScriptBlock.ConfirmClick(cmdUpdateRss, "Warning: This updates all of the RSS feeds that need to be updated, and can take some time.");
				JavaScriptBlock.ConfirmClick(cmdUpdateLinks, "Warning: This runs a full maintenance cycle on all the links in the system.  Is normally only done daily.");
			}

			cmdUpdateRss.Click += new EventHandler(cmdUpdateRss_Click);
			cmdUpdateLinks.Click += new EventHandler(cmdUpdateLinks_Click);
            cmdUpdateFtp.Click += new EventHandler(cmdUpdateFtp_Click);
            cmdSpiderLinkPage.Click += new EventHandler(cmdSpiderLinkPage_Click);
            cmdSpiderAllLinkPages.Click += new EventHandler(cmdSpiderAllLinkPages_Click);
		}

        void cmdSpiderAllLinkPages_Click(object sender, EventArgs e)
        {
            LinkPage.Spider.MultiSiteSpider ss;
            Site[] sites;

            sites = _db.GetSitesForLinkPageCheck();

            ss = new Nle.LinkPage.Spider.MultiSiteSpider(_db);
            ss.SpiderSites(sites);

            _scripts.ShowAlert("Check Complete");   
        }

        private void cmdSpiderLinkPage_Click(object sender, EventArgs e)
        {
            LinkPage.Spider.MultiSiteSpider ss;
            int siteId;
            Site site;           

            siteId = Global.GetCurrentSiteId();
            site = new Site(siteId);
            _db.PopulateSite(site);

            ss = new Nle.LinkPage.Spider.MultiSiteSpider(_db);
            ss.SpiderSites(new Site[] { site });
                        
            _scripts.ShowAlert("Check Complete");
        }

        void cmdUpdateFtp_Click(object sender, EventArgs e)
        {
            Site[] sites;
            FtpUploadInfo ftpInfo;
            Uploader u;

            //Get all the sites that need to have files FTP'd
            sites = _db.GetAllFtpSites();

            foreach (Site currSite in sites)
            {
                ftpInfo = _db.GetFtpUploadInfo(currSite.Id);

                try
                {
                    //The constructor automatically uploads the link pages
                    u = new Uploader(_db, ftpInfo);
                }
                catch (Exception ex)
                {
                    _log.Debug("Error FTP'ing link pages (Not a big deal, will send an email)", ex);
                    emailFailedFtp(currSite);
                }
            }
        }

        private void emailFailedFtp(Site s)
        {
            EmailMessage msg;
            User u;

            u = new User(s.UserId);
            _db.PopulateUser(u);

            msg = new EmailMessage();
            msg.UserId = s.UserId;
            msg.ToName = u.Name;
            msg.ToAddress = u.EmailAddress;
            msg.From = "Support@NaturalLinkExchange.com";
            msg.Subject = "FTP Upload Failed";
            msg.Message = "There was an error while uploading your link pages for Natural Link Exchange. " +
                "Please login and check your FTP settings at http://www.NaturalLinkExchange.com";

            _log.Debug("Email created, queuing");
            _db.SaveEmail(msg);
            _log.Debug("Email queued");
        }

        private void initPromotionSection()
        {
            Subscription subscription;
            LegalNoticeVersion version;
            bool hasAgreed;

            pnlPromotion.Visible = true;

            if (header.GetSelectedSiteId() > 0)
            {
                subscription = _db.GetSiteSubscription(header.GetSelectedSiteId());
                cpiUpgradeSite.Visible = (subscription == null) || subscription.PlanId == 1;
            }
            else
                cpiUpgradeSite.Visible = false;

            version = _db.GetLatestLegalNoticeVersion(LegalNotices.AffiliateTermsOfService);
            hasAgreed = _db.HasAgreedToNotice(version.Id, _userId);

            cpiAffiliateProgram.Visible = !hasAgreed;

            pnlPromotion.Visible = cpiUpgradeSite.Visible || cpiAffiliateProgram.Visible;
        }

		private void initJavaScript()
		{
			_scripts = new JavaScriptBlock();
			controlPlaceholder.Controls.Add(_scripts);
		}

		private void initPrivledges()
		{
			User user;

			user = new User(_userId);
			_db.PopulateUser(user);

			if(user.AccountType == AccountTypes.Administrator)
			{
				pnlReporting.Visible = true;
				pnlAdministration.Visible = true;
			}
			else
			{
				pnlReporting.Visible = false;
				pnlAdministration.Visible = false;
			}
		}

        enum ConfigTasks
        {
            CreateSite = 1,
            ChooseSubscription = 2,
            CreateArticles = 4,
            FullArticleDistribution = 8,
            CreateArticlePageTemplate = 16,
            ChoosePublishMethod = 32,
            ActivateLinking = 64,
            VerifyLinkPage = 128
        }

		private void initConfigStatus()
		{
            ConfigTasks configMask;
            ConfigTasks baseMask;

            baseMask = ConfigTasks.CreateSite;

            configMask = getConfigurationMask();

            enableIf(cpiSiteSettings, configMask, ConfigTasks.CreateSite, ConfigTasks.CreateSite);
            enableIf(cpiManageArticles, configMask, ConfigTasks.CreateArticles, baseMask);
            enableIf(this.cpiManageDistribution, configMask, baseMask | ConfigTasks.FullArticleDistribution, baseMask | ConfigTasks.CreateArticles);
            enableIf(cpiArticlePageWizard, configMask, ConfigTasks.CreateArticlePageTemplate, baseMask | ConfigTasks.CreateArticles);
            enableIf(cpiArticlePageEdit, configMask, ConfigTasks.CreateArticlePageTemplate, baseMask | ConfigTasks.CreateArticles);
            enableIf(cpiArticlePageConfig, configMask, ConfigTasks.ChoosePublishMethod, baseMask | ConfigTasks.CreateArticles | ConfigTasks.CreateArticlePageTemplate);
            enableIf(cpiActivateLinking, configMask, ConfigTasks.ActivateLinking,
                baseMask | ConfigTasks.CreateArticles | ConfigTasks.CreateArticlePageTemplate | ConfigTasks.ChoosePublishMethod);

            //Once they activate the linking, they shouldn't be able to do it again.
            if ((configMask & ConfigTasks.ActivateLinking) == ConfigTasks.ActivateLinking)
            {
                cpiActivateLinking.Enabled = false;
                cpiActivateLinking.ToolTip = "You have already activated this site for linking";
            }
            
            enableIf(cpiVerifyLinkPage, configMask, ConfigTasks.VerifyLinkPage, baseMask | ConfigTasks.ActivateLinking);
		}
               
        void enableIf(ControlPanelItem cpi, ConfigTasks configMask, ConfigTasks completeMask, ConfigTasks enabledMask)
        {
            cpi.CheckedMode = (configMask & completeMask) == completeMask && (configMask & enabledMask) == enabledMask ? ControlPanelItem.CheckedModes.Checked : ControlPanelItem.CheckedModes.Unchecked;
            cpi.Enabled = (configMask & enabledMask) == enabledMask;
            if (cpi.Enabled) cpi.ToolTip = string.Empty; else cpi.ToolTip = "This item is disabled until you have completed the above tasks.";
        }

        ConfigTasks getConfigurationMask()
        {
            int selectedSiteId;
            Site site;
            Subscription subscription;
            ConfigTasks configMask = 0;
            LinkPageStatus linkPageStatus;

            //Prepare for check to make sure they have a site configured
            selectedSiteId = header.GetSelectedSiteId();
            //Prepare for check for Subscription
            if (selectedSiteId != -1)
            {
                site = new Site(selectedSiteId);
                _db.PopulateSite(site);
            }
            else
                site = null;

            if (site != null) 
            {
                subscription = _db.GetSiteSubscription(site.Id);
                //Prepare for check for articles
                bool containsArticles = false;
                double distribution = 0;
                bool allGroupsDistributed = true;
                if (site != null)
                    foreach (LinkParagraphGroup group in _db.GetSiteLinkParagraphGroups(site.Id))
                    {
                        allGroupsDistributed = allGroupsDistributed && (group.Distribution > 0);
                        distribution += group.Distribution;
                        foreach (LinkParagraph paragraph in group.Paragraphs)
                            if (paragraph.Enabled) containsArticles = true;
                    }

                //Check to make sure they have a site configured
                if (selectedSiteId != -1) configMask = configMask | ConfigTasks.CreateSite;
                //Check for Subscription
                if (subscription != null) configMask = configMask | ConfigTasks.ChooseSubscription;
                //Check for articles
                if (containsArticles) configMask = configMask | ConfigTasks.CreateArticles;
                //Check for 100% Distribution
                if (allGroupsDistributed && distribution == 100.0) configMask = configMask | ConfigTasks.FullArticleDistribution;
                //Check for Article Page Template
                if (site.PageTemplate != null && site.PageTemplate != string.Empty) configMask = configMask | ConfigTasks.CreateArticlePageTemplate;
                //Check for Article Page Publish Method
                if (_db.GetIntSiteSetting(site.Id, 6) != 0) configMask = configMask | ConfigTasks.ChoosePublishMethod;
                //Check if they have any incoming links
                if (_db.GetIncomingLinkCount(site.Id) > 0) configMask = configMask | ConfigTasks.ActivateLinking;

                //Check if they have successfully verified their link page
                linkPageStatus = _db.GetCurrentLinkPageStatus(site.Id);
                if (linkPageStatus != null)
                {
                    if (linkPageStatus.Valid)
                        configMask = configMask | ConfigTasks.VerifyLinkPage;
                }
            }

            return configMask;
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

		private void cmdUpdateRss_Click(object sender, EventArgs e)
		{
			RssFeedMaintenance fm;

			fm = new RssFeedMaintenance(_db);
			fm.Run();
		}

		private void cmdUpdateLinks_Click(object sender, EventArgs e)
		{
			ArticleMaintenance am;

			am = new ArticleMaintenance(_db);
			am.Run();
		}

        public static string GetLoadUrl()
        {
            UrlBuilder url;

            url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

            return url.ToString();
        }
	}
}