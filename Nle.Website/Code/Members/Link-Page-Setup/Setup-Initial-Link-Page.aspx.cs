using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;
using Nle.Website;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using Nle.Components;

public partial class Members_Link_Page_Setup_Setup_Initial_Link_Page : System.Web.UI.Page
{
    private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private Database _db;
    private int _userId;
    private MainMaster _master;
    private StatusHeader _header;
    private YTech.General.Web.JavaScript.JavaScriptBlock _scripts;

    protected override void OnInit(EventArgs e)
    {
        Load += new EventHandler(Page_Load);
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _master = (MainMaster)Master;
        _header = _master.MasterStatusHeader;
        _db = Global.GetDbConnection();
        _userId = Global.GetCurrentUserId();
        _scripts = new YTech.General.Web.JavaScript.JavaScriptBlock();
        contentPlaceholder.Controls.Add(_scripts);

        cmdReady.Click += new EventHandler(cmdReady_Click);
    }

    void cmdReady_Click(object sender, EventArgs e)
    {
        int siteId; 
        Site setupSite;
        string userAlert;
        Subscription subscription;
        int incomingLinkCount;

        siteId = _header.GetSelectedSiteId();

        _log.DebugFormat("User {0} is running the initial setup cycle for site {1}", _userId, siteId);

        setupSite = new Site(siteId);
        _db.PopulateSite(setupSite);

        //Create the users subscription if needed
        subscription = _db.GetSiteSubscription(siteId);
        if (subscription == null)
        {
            _log.DebugFormat("Creating a free subscription for site {0}", siteId);
            subscription = new Subscription();
            subscription.PlanId = 1; //free plan
            subscription.SiteId = siteId;
            _db.SaveSubscription(subscription);
        }
        else
        {
            _log.DebugFormat("No need to create a subscription for site {0}, since they already have one", siteId);
        }
        
        //Create the initial link page
        //If you just request the initial link page, it will create it if it doesn't exist
        _db.GetInitialLinkPage(siteId);

        incomingLinkCount = _db.GetIncomingLinkCount(siteId);
        if (incomingLinkCount == 0)
        {
            //Give the site a link
            _log.DebugFormat("Adding a link to site {0}", siteId);
            _db.AddLink(setupSite);
            _log.DebugFormat("A link was successfully added to site {0}", siteId);
        }
        else
        {
            _log.DebugFormat("Site {0} already has {0} links", siteId, incomingLinkCount);
        }

        //Update their RSS feeds
        _log.DebugFormat("Updating the RSS feeds for site {0}", siteId);
        _db.UpdateSiteFeeds(siteId);
        _log.DebugFormat("Finished updating the RSS feeds for site {0}", siteId);

        //They're done!
        _log.Debug("The initial setup of the users link page is complete, sending them back to the control panel");
        userAlert = "Your initial link page setup is complete";
        _scripts.AlertAndRedirect(userAlert, Page.ResolveUrl("~/Members/Control-Panel/"));
    }
}
