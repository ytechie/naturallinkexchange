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
using Nle.Website.Common_Controls;
using Nle.Db.Exceptions;
using Nle.Website;

public partial class MainMaster : System.Web.UI.MasterPage
{
    private string _pageKeywords;
    private string _pageDescription;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
#if SANDBOX
            litSandbox.Text = "Sandbox";
#else
            litSandbox.Visible = false;
#endif
            setUpMemberLink();
            PreRender += new EventHandler(MainMaster_PreRender);

            AddJavascriptFile("Scripts/Global.js");
            AddJavascriptFile("Scripts/DynamicHelp.js");
        }
        catch (UserNotFoundException ex)
        {
            if (ex.UserId == Global.GetCurrentUserId())
                FormsAuthentication.SignOut();
        }
        
    }

    /// <summary>
    ///		Sets up the member link on the menu so that it's text
    ///		changes so that the user knows that they are logged in.
    /// </summary>
    private void setUpMemberLink()
    {
        User user;
        if (Nle.Website.Global.GetCurrentUserId() == -1)
        {
            lnkControlPanel.Text = "Member Login";
            lnkControlPanel.NavigateUrl = "~/Members/";
        }
        else
        {
            lnkControlPanel.Text = "Control Panel";
            lnkControlPanel.NavigateUrl = "~/Members/Control-Panel/";

            user = new User(Nle.Website.Global.GetCurrentUserId());
            Nle.Website.Global.GetDbConnection().PopulateUser(user);
        }
    }
    
    void MainMaster_PreRender(object sender, EventArgs e)
    {
        addSearchMetaTags();
    }

    /// <summary>
    ///     Adds the meta tags for keywords and description if
    ///     they have been set by the page using the <see href="MasterPage" />.
    /// </summary>
    private void addSearchMetaTags()
    {
        HtmlMeta meta;

        if (_pageKeywords != null)
        {
            meta = new HtmlMeta();
            meta.Name = "Keywords";
            meta.Content = _pageKeywords;
            Page.Header.Controls.Add(meta);
        }

        if (_pageDescription != null)
        {
            meta = new HtmlMeta();
            meta.Name = "Description";
            meta.Content = _pageDescription;
            Page.Header.Controls.Add(meta);
        }
    }

    /// <summary>
    ///     Gets or sets the keywords that will be used in the "Head" of
    ///     this page.
    /// </summary>
    public string PageKeywords
    {
        get
        {
            return _pageKeywords;
        }
        set
        {
            _pageKeywords = value;
        }
    }

    /// <summary>
    ///     Gets or sets the description of the page, which is used primarily
    ///     in the "Head" of the page to assist in indexing by search engines.
    /// </summary>
    public string PageDescription
    {
        get
        {
            return _pageDescription;
        }
        set
        {
            _pageDescription = value;
        }
    }

    /// <summary>
    ///     Adds a stylesheet link tag to the head tag of the page.
    /// </summary>
    /// <param name="href">The path to the CSS Stylesheet.</param>
    public void AddStylesheet(string href)
    {
        AddStylesheet(href, false);
    }

    /// <summary>
    ///     Adds a stylesheet link tag to the head tag of the page.
    /// </summary>
    /// <param name="href">The path to the CSS Stylesheet.</param>
    /// <param name="allowHrefRewrite">If true, uses an HtmlLink object to
    /// add the link tag, which the worker process will rewrite URLs if
    /// need be.  If false, an HtmlGenericControl is use which does not
    /// do any URL rewriting.</param>
    public void AddStylesheet(string href, bool allowHrefRewrite)
    {
        const string HREF = "href";
        const string TEXT = "text";
        const string REL = "rel";
        const string TEXT_VALUE = "text/css";
        const string REL_VALUE = "stylesheet";
        HtmlLink htmlLink;
        HtmlGenericControl genericLink;

        if (allowHrefRewrite)
        {
            htmlLink = new HtmlLink();
            htmlLink.Href = href;
            htmlLink.Attributes[TEXT] = TEXT_VALUE;
            htmlLink.Attributes[REL] = REL_VALUE;
            Page.Header.Controls.Add(htmlLink);
        }
        else
        {
            genericLink = new HtmlGenericControl("link");
            genericLink.Attributes[HREF] = href;
            genericLink.Attributes[TEXT] = TEXT_VALUE;
            genericLink.Attributes[REL] = REL_VALUE;
            Page.Header.Controls.Add(genericLink);
        }
    }

    /// <summary>
    ///     Adds a javascript file tag to the head tag of the page.
    /// </summary>
    /// <param name="href"></param>
    public void AddJavascriptFile(string src)
    {
        HtmlGenericControl script;
        script = new HtmlGenericControl("script");
        script.Attributes["src"] = Nle.Website.Global.VirtualDirectory + src;
        script.Attributes["type"] = "text/javascript";
        script.Attributes["language"] = "Javascript";
        Page.Header.Controls.Add(script);
    }

    /// <summary>
    ///     Gets the <see cref="StatusHeader"/> that appears
    ///     on the master page.
    /// </summary>
    public StatusHeader MasterStatusHeader
    {
        get
        {
            return shStatus;
        }
    }
}
