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
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website;
using YTech.General.Web.JavaScript;

public partial class Members_Link_Page_Setup_Verify_Link_Page : System.Web.UI.Page
{
    private int _siteId;
    private Database _db;
    private Site _site;
    private JavaScriptBlock _scripts;

    protected void Page_Load(object sender, EventArgs e)
    {
        _siteId = Global.GetCurrentSiteId();
        _db = Global.GetDbConnection();

        initJavaScript();

        //Load their site info since we're going to need it
        _site = new Site(_siteId);
        _db.PopulateSite(_site);

        if(!Page.IsPostBack)
            JavaScriptBlock.ConfirmClick(cmdVerify, "This may take a few minutes the first time it is run");

        cmdVerify.Click += new EventHandler(cmdVerify_Click);

        showCurrentStatus();
    }

    void initJavaScript()
    {
        _scripts = new JavaScriptBlock();

        JavaScriptPlaceholder.Controls.Add(_scripts);
    }

    void cmdVerify_Click(object sender, EventArgs e)
    {
        Nle.LinkPage.Spider.MultiSiteSpider ss;

        ss = new Nle.LinkPage.Spider.MultiSiteSpider(_db);
        ss.SpiderSites(new Site[] { _site });

        showCurrentStatus();

        _scripts.ShowAlert("Check Complete");
    }

    private void showCurrentStatus()
    {
        LinkPageStatus linkPageStatus;

        //Check if they have successfully verified their link page
        linkPageStatus = _db.GetCurrentLinkPageStatus(_site.Id);
        if (linkPageStatus != null)
        {
            if (linkPageStatus.Valid)
            {
                litLastStatus.Text = "Verified as Working";
                pnlCongratulations.Visible = true;
            }
            else
            {
                litLastStatus.Text = "Invalid Link Page";
            }
        }
        else
        {
            litLastStatus.Text = "Never Checked";
        }

        if (_site.LinkPageUrl == null)
            txtLinkPageUrl.Text = "";
        else
            txtLinkPageUrl.Text = _site.LinkPageUrl;
    }
}
