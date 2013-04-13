using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;
using System.Text.RegularExpressions;
using System.Collections;
using System.Text;

namespace Nle.Website.Members.Link_Page_Design
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class LinkPageDesignDefault : Page
	{
		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Link-Page-Design/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "";

        public const string PARAM_ISWIZARD = "IsWizard";

        const string TOKEN_TITLE = "{title}";
        const string TOKEN_METAKEYWORDS = "{metaKeywords}";
        const string TOKEN_METADESCRIPTION = "{metaDescription}";
        const string TOKEN_RELATEDCATEGORIES = "{relatedCategories}";
        const string TOKEN_RSSFEEDS = "{RssFeeds}";
        const string TOKEN_ARTICLES = "{Articles}";

		private Database _db;
		private int _userId;
        private Site _site;
        private MainMaster _master;
        private StatusHeader header;
        private JavaScriptBlock _scripts;
        private bool _isWizard = false;

        string _missingTokens;

        private HtmlGenericControl previewFrame;

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

            _master = (MainMaster)Master;
            header = _master.MasterStatusHeader;

            _site = new Site(header.GetSelectedSiteId());
            _db.PopulateSite(_site);

            if(Request.QueryString[PARAM_ISWIZARD] != null)
                _isWizard = bool.Parse(Request.QueryString[PARAM_ISWIZARD]);

			header.SiteChanged += new Nle.Website.Common_Controls.SiteSelector.SiteChangedDelegate(siteSelector_SiteChanged);
			cmdPreview.Click += new EventHandler(cmdPreview_Click);
			cmdSource.Click += new EventHandler(cmdSource_Click);
			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

            MainMaster mp = (MainMaster)Page.Master;
            mp.AddStylesheet("LinkPageDesign.css");
            _scripts = new JavaScriptBlock();
            controlplaceholder.Controls.Add(_scripts); 

			if(!Page.IsPostBack)
			{
                litPageSourceClientId.Text = txtLinkPageSource.ClientID;
				header.FilterIncompleteSites = true;

				initGlobalTemplate();
				initCancelConfirm();
                initJavascript();
				loadLinkPageDesign();

                pnlNoWizard.Visible = !_isWizard;
                pnlWizardStep1.Visible = _isWizard;
                pnlWizardStep2.Visible = _isWizard;
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

        public static string GetLoadUrl(bool isWizard)
        {
            UrlBuilder url;

            url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
            url.Parameters.AddParameter(PARAM_ISWIZARD, isWizard);

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

        void initJavascript()
        {
            txtInsertText.Attributes.Add("onclick", "InsertAtCursor(document.getElementById('" + txtLinkPageSource.ClientID + "'), '{title}'); return false;");
            txtInsertMetaKeywords.Attributes.Add("onclick", "InsertAtCursor(document.getElementById('" + txtLinkPageSource.ClientID + "'), '{metaKeywords}'); return false;");
            txtInsertMetaDescription.Attributes.Add("onclick", "InsertAtCursor(document.getElementById('" + txtLinkPageSource.ClientID + "'), '{metaDescription}'); return false;");
            txtInsertCategories.Attributes.Add("onclick", "InsertAtCursor(document.getElementById('" + txtLinkPageSource.ClientID + "'), '{relatedCategories}'); return false;");
            txtInsertRssFeeds.Attributes.Add("onclick", "InsertAtCursor(document.getElementById('" + txtLinkPageSource.ClientID + "'), '{RssFeeds}'); return false;");
            txtInsertArticles.Attributes.Add("onclick", "InsertAtCursor(document.getElementById('" + txtLinkPageSource.ClientID + "'), '{Articles}'); return false;");
        }

		private void initGlobalTemplate()
		{
			GlobalSetting gs = new GlobalSetting(2);
			_db.PopulateGlobalSetting(gs);
			string source = gs.TextValue;
			source = source.Replace("\n", "\\n");
			source = source.Replace("\r", "\\r");
			source = source.Replace("\t", "\\t");
			source = source.Replace("\"", "\\\"");
			litDefaultTemplate.Text = source;
		}

		private void initCancelConfirm()
		{
			JavaScriptBlock.ConfirmClick(cmdCancel, "Are you sure you want to lose your changes and return to the control panel?");
		}

		private void loadLinkPageDesign()
		{
			txtLinkPageSource.Text = _site.PageTemplate;

			if(Session["LINK_PAGE_TEMPLATE_SOURCE"] != null)
			{
				txtLinkPageSource.Text = (string)Session["LINK_PAGE_TEMPLATE_SOURCE"];
				Session["LINK_PAGE_TEMPLATE_SOURCE"] = null;
			}
		}

		private void redirectToControlPanel()
		{
			string controlPanelUrl;

            controlPanelUrl = ResolveUrl("~/Members/Control-Panel/");
			Response.Redirect(controlPanelUrl);			
		}

		private LinkParagraph[] genericParagraphs()
		{
			LinkParagraph[] lps = new LinkParagraph[2];
			LinkParagraph lp;

			lp = new LinkParagraph();
			lp.Title = "Article 1";
			lp.UrlBase = "http://www.naturallinkexchange.com";
			lp.Keyword1 = "{anchor1}";
            lp.ReplacementText1 = "Natural Link Exchange";
			lp.Paragraph = "This is just a sample link article to show you how this link article page will look.  " +
				"{anchor1} adds other sites' link articles to your site in order to generate incoming " +
				"links for them and adds your link articles to other sites' article pages to generate incoming links for you.";
			lps[0] = lp;

			lp = new LinkParagraph();
			lp.Title = "Article 2";
			lp.UrlBase = "http://www.google.com";
			lp.Keyword1 = "{anchor1}";
            lp.ReplacementText1 = "Google";
			lp.Paragraph = "{anchor1} is a search engine that Natural Link Exchange researches to make sure that the service " +
				"provided works in such a way to appear natural to {anchor1}.";
			lps[1] = lp;

			return lps;
		}

		private string getSourceCode()
		{
			LinkPageHtmlGenerator generator;
            LinkCategory category;
            Components.LinkPage lp;

            //Create generic LinkPage
            category = new LinkCategory(_site.InitialCategoryId);
            lp = new Components.LinkPage(0);
            lp.ArticleTarget = 5;
            lp.CategoryId = category.Id;
            lp.PageName = category.PageName;
            lp.PageTitle = category.Title;
            lp.SiteId = _site.Id;

			generator = new LinkPageHtmlGenerator();
			generator.CategoryUrlSignature = "http://www.naturallinkexchange.com";
			generator.GetPageHtml(_db, lp);
			generator.Articles = genericParagraphs();
			generator.Template = txtLinkPageSource.Text;
			return generator.GetPageHtml();
		}

		private void cmdPreview_Click(object sender, EventArgs e)
		{
			txtLinkPageSource.Style["display"] = "none";
			Session["LINK_PAGE_PREVIEW_SOURCE"] = getSourceCode();

			previewFrame = new HtmlGenericControl("iframe");
			previewPlaceholder.Controls.Add(previewFrame);
			previewFrame.Attributes.Add("class", "PreviewFrame");
			previewFrame.Attributes.Add("src", LinkPagePreview.GetUrl());

			sourceToolbar.Visible = false;
		}

		private void cmdSource_Click(object sender, EventArgs e)
		{
			previewPlaceholder.Controls.Clear();
			txtLinkPageSource.Style["display"] = string.Empty;
			sourceToolbar.Visible = true;
		}

		private void cmdSave_Click(object sender, EventArgs e)

		{
            string msg; 
            if(!templateIsValid())
            {
                msg = string.Format("The Link Page could not be save because it is missing 1 or more tokens.  Please add the following tokens: {0}.", _missingTokens);
                _scripts.ShowAlert(msg); 
                lblMissingTokens.Text = msg ;
                lblMissingTokens.Visible = true;
            }
            else
            {
			_site.PageTemplate = txtLinkPageSource.Text;
			_db.SaveSite(_site);
			redirectToControlPanel();
            }
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			redirectToControlPanel();
		}

		private void siteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			loadLinkPageDesign();
			
			previewPlaceholder.Controls.Clear();
			txtLinkPageSource.Style["display"] = string.Empty;
			sourceToolbar.Visible = true;
		}

        bool templateIsValid()
        {
            ArrayList missing;
            string template;
            StringBuilder missingTokens;

            missing = new ArrayList();
            template = txtLinkPageSource.Text;

            
            requireToken(template, TOKEN_ARTICLES, missing);
            requireToken(template, TOKEN_METADESCRIPTION, missing);
            requireToken(template, TOKEN_METAKEYWORDS, missing);
            requireToken(template, TOKEN_RELATEDCATEGORIES, missing);
            requireToken(template, TOKEN_RSSFEEDS, missing);
            requireToken(template, TOKEN_TITLE, missing);

            if(missing.Count == 0)
                return true;

            missingTokens = new StringBuilder();
            for(int i = 0; i < missing.Count; i++)
            {
                missingTokens.Append(missing[i]);
                if(missing.Count > 1 && i < missing.Count - 1)
                    missingTokens.Append(i == missing.Count - 2 ? " and " :  ", ");
            }

            _missingTokens = missingTokens.ToString();
            return false;
        }

        void requireToken(string template, string token, ArrayList missing)
        {
            if(!Regex.IsMatch(template, string.Format(".*{0}.*", token), RegexOptions.IgnoreCase))
                missing.Add(token);
        }
	}
}
