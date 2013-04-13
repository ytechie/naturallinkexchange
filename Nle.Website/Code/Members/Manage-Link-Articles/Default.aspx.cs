using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;
using Nle.Website.Members.Manage_Article_Distribution;

namespace Nle.Website.Members.Manage_Link_Articles
{
	/// <summary>
	///		Allows the users to manage (add, delete, edit) their link articles.
	/// </summary>
	public partial class ManageLinkArticlesDefault : Page
	{
        public const string MY_PATH = "Members/Manage-Link-Articles/";
        public const string MY_FILE_NAME = "";

		/// <summary>
		///		The path of this page.
		/// </summary>
		/// <summary>
		///		The file name of this page.
		/// </summary>

		private Database _db;

		private Site _currSite;
		private LinkParagraphGroup[] _groups;
		Subscription _subscription;
		LinkPackage _linkPackage;
        MainMaster _master;
        StatusHeader _header;

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();

            _master = (MainMaster)Master;
            _master.AddStylesheet("ManageLinkArticles.css");
            _header = _master.MasterStatusHeader;
	
			getSiteInformation();
            setDescription();

			_header.SiteChanged += new Nle.Website.Common_Controls.SiteSelector.SiteChangedDelegate(siteSelector_SiteChanged);

            if (!Page.IsPostBack)
			{
				_header.FilterIncompleteSites = true;

				lnkAddNewGroup.NavigateUrl = EditLinkArticleGroup.GetLoadUrl2(_header.GetSelectedSiteId());

				displayLinkGroups();
			}
		}

		private void setDescription()
		{
            if (_linkPackage.ArticlesPerGroup > 1)
            {
                descFreeSilver_NumOfArticles.Text = _linkPackage.ArticlesPerGroupDisplayText + " articles";
            }
            else
            {
                descFreeSilver_NumOfArticles.Text = _linkPackage.ArticlesPerGroupDisplayText + " article";
            }
            if (_linkPackage.Id == 1)
            {
                pnlFreeSilverDesc1.Visible = true;
                descFreeSilver_NextLevel.Text = "Silver or Gold";
            }
            else if (_linkPackage.Id == 2)
            {
                pnlFreeSilverDesc1.Visible = true;
                descFreeSilver_NextLevel.Text = "Gold";
            }
            else
            {
                pnlFreeSilverDesc1.Visible = false;
            }

		}

        private void getSiteInformation()
        {
            int siteId;
            siteId = _header.GetSelectedSiteId();

            _currSite = new Site(siteId);
            _db.PopulateSite(_currSite);

            _groups = _db.GetSiteLinkParagraphGroups(siteId);

            _subscription = _db.GetSiteSubscription(_currSite.Id);

            if (_subscription == null)
                _linkPackage = _db.GetLinkPackage(1);
            else
                _linkPackage = _db.GetLinkPackage(_subscription.PlanId);
        }
        
        private int getSubscriptionArticleCount()
		{
			return _linkPackage.ArticlesPerGroup;
		}

		private int getSubscriptionArticleGroupCount()
		{
			
            return _linkPackage.LinkGroups;
		}

		private void displayLinkGroups()
		{
			//Update the "Add Link Group" button
			Image imgAddGroup = new Image();
			lnkAddNewGroup.Controls.Add(imgAddGroup);
			imgAddGroup.ImageUrl = Global.VirtualDirectory + "Images/AddGroup.gif";
			imgAddGroup.AlternateText = "Add Group";
			imgAddGroup.ToolTip = "Add Group";
			if(_groups.Length < getSubscriptionArticleGroupCount())
				lnkAddNewGroup.NavigateUrl = EditLinkArticleGroup.GetLoadUrl2(_currSite.Id);
			else
			{
				string funcCall = JavaScriptBlock.GetFunctionCall("ShowArticleGroupCountMessage", false, getSubscriptionArticleGroupCount().ToString(), JavaScriptBlock.SQuote(Payment_Settings._Default.GetLoadUrl()));
				lnkAddNewGroup.NavigateUrl="#";
				lnkAddNewGroup.Attributes.Add("onclick", "return " + funcCall);
			}

			foreach (LinkParagraphGroup currGroup in _groups)
			{
				LinkParagraph[] articles = currGroup.Paragraphs;
				Control ctrlGroup = newArticleGroup(_currSite, currGroup, articles);
				controlPlaceholder.Controls.Add(ctrlGroup);
			}
		}

		#region build controls
		private HtmlGenericControl newButtonContainer()
		{
			HtmlGenericControl buttonContainer = new HtmlGenericControl("div");
			buttonContainer.Attributes["Class"] = "buttonContainer";
			return buttonContainer;
		}

		private HyperLink newImageLink(string imageUrl, string navigateUrl, string altText)
		{
			HyperLink link = new HyperLink();
			link.NavigateUrl = navigateUrl;

			Image imgButton = new Image();
			link.Controls.Add(imgButton);
			imgButton.ImageUrl = imageUrl;
			imgButton.AlternateText = altText;
			imgButton.ToolTip = altText;

			return link;
		}

		private Control newHeader(int level, string text)
		{
			HtmlGenericControl h = new HtmlGenericControl("h" + level.ToString());
			h.InnerText = text;
			return h;
		}

		private HtmlGenericControl newUrl(string keywordNumber, string keyword, string replacementText, string url)
		{
			HtmlGenericControl container = new HtmlGenericControl("p");
            HtmlGenericControl h4 = new HtmlGenericControl("h4");
            HyperLink lnkUrl = new HyperLink();

            container.Controls.Add(h4);
            container.Controls.Add(newFieldValue("Keyword", keyword));
            if(!string.IsNullOrEmpty(replacementText) && keyword != replacementText) container.Controls.Add(newFieldValue("Replacement Text", replacementText));
            container.Controls.Add(newFieldValue("URL", lnkUrl));

            h4.InnerText = string.Format("{0} Keyword", keywordNumber);
            lnkUrl.Text = url;
            lnkUrl.NavigateUrl = url;

			return container;
		}

        private HtmlGenericControl newFieldValue(string label, string value)
        {
            Literal litValue = new Literal();
            litValue.Text = value;
            return newFieldValue(label, litValue);
        }

        private HtmlGenericControl newFieldValue(string label, Control value)
        {
            HtmlGenericControl container = new HtmlGenericControl("div");
            Label lblLabel = new Label();
            HtmlGenericControl br = new HtmlGenericControl("br");

            container.Controls.Add(lblLabel);
            container.Controls.Add(value);
            container.Controls.Add(br);

            lblLabel.CssClass = "standardFieldLabel";
            lblLabel.Text = label + ":";

            return container;
        }

		private HtmlGenericControl newParagraph(string html)
		{
			HtmlGenericControl p = new HtmlGenericControl("p");
			p.InnerHtml = html;
			return p;
		}

		private Control newArticleGroup(Site site, LinkParagraphGroup group, LinkParagraph[] articles)
		{
			HtmlGenericControl container = new HtmlGenericControl("div");
			HtmlGenericControl buttonContainer = newButtonContainer();

			container.Attributes["Class"] = "paragraphGroupContainer";
			container.Controls.Add(newHeader(3, group.Title));
			container.Controls.Add(buttonContainer);
            buttonContainer.Controls.Add(newImageLink(Global.VirtualDirectory + "Images/EditDistribution.gif", ManageArticleDistributionDefault.GetLoadUrl(), "Edit Distribution"));
			buttonContainer.Controls.Add(newImageLink(Global.VirtualDirectory + "Images/EditGroup.gif", EditLinkArticleGroup.GetLoadUrl(group.Id), "Edit Article Group"));
			if(articles.Length < getSubscriptionArticleCount())
			{
				buttonContainer.Controls.Add(newImageLink( Global.VirtualDirectory + "Images/AddArticle.gif", EditLinkArticle.GetLoadUrl2(group.Id), "Add Article"));
			}
			else
			{
				HyperLink link = newImageLink(Global.VirtualDirectory + "Images/AddArticle.gif", "#", "Add Article");
				buttonContainer.Controls.Add(link);
				string funcCall = JavaScriptBlock.GetFunctionCall("ShowArticleCountMessage", false, getSubscriptionArticleCount().ToString(), JavaScriptBlock.SQuote(Payment_Settings._Default.GetLoadUrl()));
				link.Attributes.Add("onclick", "return " + funcCall);
			}
			container.Controls.Add(newUrl("1st", group.Keyword1, group.ReplacementText1, GuaranteeTrailingSlash(site.Url) + group.Url1));
            if(_linkPackage.AnchorCount > 1)
                container.Controls.Add(newUrl("2nd", group.Keyword2, group.ReplacementText2, GuaranteeTrailingSlash(site.Url) + group.Url2));
			container.Controls.Add(newArticles(site, articles));

			return container;
		}

		private Control newArticles(Site site, LinkParagraph[] articles)
		{
			HtmlGenericControl container = new HtmlGenericControl("div");
			container.Attributes.Add("class", "ArticlesContainer");
			foreach(LinkParagraph currArticle in articles)
			{
				Control ctrlArticle = newArticle(site, currArticle);
				container.Controls.Add(ctrlArticle);
			}
			return container;
		}

		private Control newArticle(Site site, LinkParagraph article)
		{
			HtmlGenericControl container = new HtmlGenericControl("div");
			HtmlGenericControl buttonContainer = newButtonContainer();

			article.UrlBase = site.Url;
			
			container.Attributes["Class"] = "articleContainer";
			container.Controls.Add(this.newHeader(4, string.Format("{0}{1}", article.Title, article.Enabled ? string.Empty : " (Not Active)")));
			container.Controls.Add(buttonContainer);
			buttonContainer.Controls.Add(newImageLink(Global.VirtualDirectory + "Images/EditArticle.gif", EditLinkArticle.GetLoadUrl(article.Id), "Edit Article"));
			container.Controls.Add(newParagraph(article.GetFormattedParagraph()));

			return container;
		}
		#endregion

		private string GuaranteeTrailingSlash(string path)
		{
			return path.ToCharArray()[path.Length-1] == '/' ? path : path + '/';
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

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <returns></returns>
//		public static string GetLoadUrl()
//		{
//			UrlBuilder url;
//
//			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
//
//			return url.ToString();
//		}

		private void siteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			displayLinkGroups();
		}

        public static string GetLoadUrl()
        {
            UrlBuilder url;

            url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

            return url.ToString();
        }
	}
}