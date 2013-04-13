using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;

namespace Nle.Website.Members.Manage_Link_Articles
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class EditLinkArticle : Page
	{
		/// <summary>
		///		The name of the URL parameter to use to pass in the id of the article
		/// </summary>
		public const string PARAM_ARTICLE_ID = "ArticleId";
		/// <summary>
		///		The id of the site to use if the article is to be created.
		/// </summary>
		public const string PARAM_GROUP_ID = "GroupId";

		public const string MY_PATH = "Members/Manage-Link-Articles/";
		public const string MY_FILE_NAME = "Edit-Link-Article.aspx";

		private Database _db;
		private int _articleId;
		private int _groupId;
		private Modes _mode;

		private LinkParagraph _article;
		private LinkParagraphGroup _group;
		private Site _site;
        private MainMaster _master;
        private StatusHeader _header;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private enum Modes
		{
			Edit,
			Create
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("EditLinkArticles.css");
            _header = _master.MasterStatusHeader;

			_db = Global.GetDbConnection();
			getParameters();

			cmdSaveDraft.Click += new EventHandler(cmdSaveDraft_Click);
			cmdPublish.Click += new EventHandler(cmdPublish_Click);
			cmdDelete.Click += new EventHandler(cmdDelete_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			if(!Page.IsPostBack)
			{
                initInsertAnchors();
                initDeleteConfirm();
                initCancelConfirm();
                initSubscriptionLevel();
                initGroupInformation();

				_header.FilterIncompleteSites = true;

				if(_mode == Modes.Edit)
					loadExistingArticle();
				else if(_mode == Modes.Create)
					initForCreate();
			}
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

		private void initDeleteConfirm()
		{
			JavaScriptBlock.ConfirmClick(cmdDelete, "Are you sure you want to delete this article?");
		}

		private void initCancelConfirm()
		{
            JavaScriptBlock.ConfirmClick(lnkDistribution, "Are you sure you want to lose all your changes and go to the article group distribution manager?");
			JavaScriptBlock.ConfirmClick(cmdCancel, "Are you sure you want to lose your changes and return to the link article manager?");
		}

		private void initInsertAnchors()
		{
			cmdInsertKeyword1.Attributes.Add("onclick", JavaScriptBlock.GetFunctionCall("InsertAtCursor", false, "document.getElementById('" + txtArticle.ClientID + "')", JavaScriptBlock.SQuote(_group.Keyword1)) + "return false;");
            cmdInsertKeyword2.Attributes.Add("onclick", JavaScriptBlock.GetFunctionCall("InsertAtCursor", false, "document.getElementById('" + txtArticle.ClientID + "')", JavaScriptBlock.SQuote(_group.Keyword2)) + "return false;"); 
		}

		private void initSubscriptionLevel()
		{
			int linkCount;
			string funcCall;
			
			linkCount = getSubscriptionLinkCount();
			funcCall = JavaScriptBlock.GetFunctionCall("ConfirmLinkCount", false, JavaScriptBlock.SQuote(txtArticle.ClientID), linkCount.ToString(),
                JavaScriptBlock.SQuote(_group.Keyword1), JavaScriptBlock.SQuote(_group.Keyword2));
			cmdSaveDraft.Attributes.Add("onclick", "return " + funcCall);
			cmdPublish.Attributes.Add("onclick", "return " + funcCall);

            pnlKeyword2.Visible = linkCount > 1;
            cmdInsertKeyword2.Visible = linkCount > 1;
            pnlUpgradeAccount.Visible = linkCount < 2;
		}

		private void getParameters()
		{
			string paramString;

			paramString = Request.QueryString[PARAM_ARTICLE_ID];
			if (paramString != null && paramString.Length > 0)
			{
				_articleId = int.Parse(paramString);
				_mode = Modes.Edit;

				_article = new LinkParagraph(_articleId);
				_db.PopulateLinkParagraph(_article);

				_group = new LinkParagraphGroup(_article.GroupId);
				_db.PopulateLinkParagraphGroup(_group);
			}
			else
			{
				_mode = Modes.Create;

				//If we are in create mode, we need to know the site id
				paramString = Request.QueryString[PARAM_GROUP_ID];
				if (paramString != null && paramString.Length > 0)
				{
					_groupId = int.Parse(paramString);

					_group = new LinkParagraphGroup(_groupId);
					_db.PopulateLinkParagraphGroup(_group);
				}
				else
				{
					throw new ApplicationException(string.Format("Paramter '{0}' is required", PARAM_GROUP_ID)); 
				}
			}

			_site = new Site(_group.SiteId);
			_db.PopulateSite(_site);
		}

		private int getSubscriptionLinkCount()
		{
			Subscription subscription;
			LinkPackage linkPackage;

			subscription = _db.GetSiteSubscription(_site.Id);

			if(subscription == null)
				linkPackage = _db.GetLinkPackage(1);
			else
				 linkPackage = _db.GetLinkPackage(subscription.PlanId);

			return linkPackage.AnchorCount;
		}

		private void loadExistingArticle()
		{
            txtArticleTitle.Text = _article.Title;
			txtArticle.Text = _article.Paragraph;

			cmdSaveDraft.Visible = !_article.Enabled;
		}

		private string GuaranteeTrailingSlash(string path)
		{
			return path.ToCharArray()[path.Length-1] == '/' ? path : path + '/';
		}

		private void initForCreate()
		{
			//disable the delete button since we haven't created anything
			//to delete yet.
			cmdDelete.Enabled = false;
		}

        private void initGroupInformation()
        {
            string baseUrl = GuaranteeTrailingSlash(_site.Url);

            litSite.Text = _site.Name;
            litArticleGroups.Text = _group.Title;

            // 1st Keyword
            litKeyword1.Text = _group.Keyword1 + ":";
            lnkKeyword1.Text = _group.ReplacementText1;
            lnkKeyword1.NavigateUrl = baseUrl + _group.Url1;

            // 2nd Keyword
            litKeyword2.Text = _group.Keyword2 + ":";
            lnkKeyword2.Text = _group.ReplacementText2;
            lnkKeyword2.NavigateUrl = baseUrl + _group.Url2;

            pnlDistribution.Visible = _group.Distribution == 0;
        }

		#region Button Event Handlers
		
		private void cmdSaveDraft_Click(object sender, EventArgs e)
		{
			LinkParagraph article;

			if(!Page.IsValid)
				return;

			if(_mode == Modes.Create)
			{
				article = new LinkParagraph();
				article.GroupId = _groupId;
			}
			else
			{
				article = new LinkParagraph(_articleId);

				//Load all the fields so we only have to change what we need
				_db.PopulateLinkParagraph(article);
			}
			article.Title = txtArticleTitle.Text;
			article.Paragraph = txtArticle.Text;
			article.Enabled = false;
			
			_db.SaveLinkParagraph(article);

			//Now redirect back to the link manager
			redirectToArticleManager();
		}

		private void cmdPublish_Click(object sender, EventArgs e)
		{
			LinkParagraph article;

			if(!Page.IsValid)
				return;

			if(_mode == Modes.Create)
			{
				article = new LinkParagraph();
				article.GroupId = _groupId;
			}
			else
			{
				article = new LinkParagraph(_articleId);

				//Load all the fields so we only have to change what we need
				_db.PopulateLinkParagraph(article);
			}

			article.Title = txtArticleTitle.Text;
			article.Paragraph = txtArticle.Text;
			article.Enabled = true;
			
			_db.SaveLinkParagraph(article);

			//Now redirect back to the link manager
			redirectToArticleManager();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			redirectToArticleManager();
		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			LinkParagraph article;

			if(_mode != Modes.Edit)
			{
				_log.Warn("The delete button was pressed, but we were not in edit mode");
				return;
			}

			article = new LinkParagraph(_articleId);
			_db.DeleteLinkParagraph(article);

			redirectToArticleManager();
		}

		#endregion  

		/// <summary>
		///		Gets the URL that can be used to call this page in edit
		///		mode for the specified article id.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl(int articleId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_ARTICLE_ID, articleId);
			
			return url.ToString();
		}

		/// <summary>
		///		Gets the URL that can be used to call this page in add
		///		mode, so that it's ready to add a new article.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl2(int siteId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_GROUP_ID, siteId);
			
			return url.ToString();
		}

		private void redirectToArticleManager()
		{
			string manageArticlesUrl;

			manageArticlesUrl = ResolveUrl("~/Members/Manage-Link-Articles/");
			Response.Redirect(manageArticlesUrl);			
		}
	}
}