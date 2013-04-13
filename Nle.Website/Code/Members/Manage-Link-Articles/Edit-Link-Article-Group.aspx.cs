using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
	/// Summary description for Edit_Link_Article_Group.
	/// </summary>
	public partial class EditLinkArticleGroup : Page
	{
		public const string PARAM_ARTICLE_GROUP_ID = "ArticleGroupId";
		public const string PARAM_SITE_ID = "SiteId";

		public const string MY_PATH = "Members/Manage-Link-Articles/";
		public const string MY_FILE_NAME = "Edit-Link-Article-Group.aspx";

		private Database _db;
		private int _groupId;
		private int _siteId;
		private Modes _mode;

		private LinkParagraphGroup _group;
		protected Site _site;
		private Subscription _subscription;
		private LinkPackage _linkPackage;
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

			initDeleteConfirm();
			initCancelConfirm();
			initSubscriptionLevel();
            initJavascript();

			cmdSave.Click += new EventHandler(cmdSave_Click);
			cmdDelete.Click += new EventHandler(cmdDelete_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			if(!Page.IsPostBack)
			{
				_header.FilterIncompleteSites = true;

				switch(_mode)
				{
					case Modes.Create:
						initNew();
						break;
					case Modes.Edit:
						initExisting();
						break;
					default:
						throw new NotSupportedException(string.Format("{0} is not supported.", _mode.ToString()));
				}
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

		private void getParameters()
		{
			string paramQuery;

			paramQuery = Request.QueryString[PARAM_ARTICLE_GROUP_ID];
			if(paramQuery != null && paramQuery != string.Empty)
			{
				_groupId = int.Parse(paramQuery);
				_mode = Modes.Edit;

				_group = new LinkParagraphGroup(_groupId);
				_db.PopulateLinkParagraphGroup(_group);

				_site = new Site(_group.SiteId);
				_db.PopulateSite(_site);
			}
			else
			{
				_mode = Modes.Create;

				paramQuery = Request.QueryString[PARAM_SITE_ID];

				if(paramQuery != null && paramQuery != string.Empty)
				{
					_siteId = int.Parse(paramQuery);

					_site = new Site(_siteId);
					_db.PopulateSite(_site);
				}
				else
				{
					throw new ApplicationException(string.Format("Paramter '{0}' is required", PARAM_SITE_ID)); 
				}
			}

			_subscription = _db.GetSiteSubscription(_site.Id);
			if(_subscription == null)
				_linkPackage = _db.GetLinkPackage(1);
			else
				_linkPackage = _db.GetLinkPackage(_subscription.PlanId);
		}

		/// <summary>
		///		Gets the URL that can be used to call this page in edit
		///		mode for the specified article group id.
		/// </summary>
		/// <param name="articleGroupId">The id of the article group to edit.</param>
		/// <returns></returns>
		public static string GetLoadUrl(int articleGroupId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_ARTICLE_GROUP_ID, articleGroupId);
			
			return url.ToString();
		}

		/// <summary>
		/// Gets the URL that can be used to call this page in create new
		/// mode for the specified site id.
		/// </summary>
		/// <param name="siteId">The id of the site to add the new article group to.</param>
		/// <returns></returns>
		public static string GetLoadUrl2(int siteId)
		{
			UrlBuilder url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_SITE_ID, siteId);

			return url.ToString();
		}

		private int getSubscriptionAnchorCount()
		{
			return _linkPackage.AnchorCount;
		}

		private void initDeleteConfirm()
		{
			JavaScriptBlock.ConfirmClick(cmdDelete, "Are you sure you want to delete this article group?  Deleting this group will delete all of its corresponding articles.");
		}

		private void initCancelConfirm()
		{
            JavaScriptBlock.ConfirmClick(lnkDistribution, "Are you sure you want to lose all your changes and go to the article group distribution manager?");
			JavaScriptBlock.ConfirmClick(cmdCancel, "Are you sure you want to lose your changes and return to the link article manager?");
		}

        private void initJavascript()
        {
            divLink1.Attributes["onclick"] = "SetFocus('" + txtUrl1.ClientID + "');" + divLink1.Attributes["onclick"];
            divLink2.Attributes["onclick"] = "SetFocus('" + txtUrl2.ClientID + "');" + divLink2.Attributes["onclick"];

            initAdvancedCheckbox(chkAdvanced1, pnlReplacementText1, txtReplacementText1, txtKeyword1);
            initAdvancedCheckbox(chkAdvanced2, pnlReplacementText2, txtReplacementText2, txtKeyword2);
        }

        private void initAdvancedCheckbox(CheckBox checkbox, Panel panel, TextBox replacementText, TextBox keyword)
        {
            string showhide;
            string makeequal;

            showhide = JavaScriptBlock.GetFunctionCall("ShowHide", false, JavaScriptBlock.SQuote(panel.ClientID));
            makeequal = JavaScriptBlock.GetFunctionCall("MakeEqual", false, JavaScriptBlock.SQuote(checkbox.ClientID),
                JavaScriptBlock.SQuote(replacementText.ClientID), JavaScriptBlock.SQuote(keyword.ClientID));

            checkbox.Attributes.Add("onClick", showhide + makeequal);
        }

		private void initSubscriptionLevel()
		{
            pnlKeyword2.Visible = _linkPackage.AnchorCount > 1;
            pnlUpgradeAccount.Visible = _linkPackage.AnchorCount < 2;
		}

		private void initNew()
		{
			cmdDelete.Enabled = false;

			string baseUrl = GuaranteeTrailingSlash(_site.Url);

            pnlDistribution.Visible = true;

			litSite.Text = _site.Name;

			litBaseUrl1.Text = baseUrl;
            pnlReplacementText1.Style.Add("display", "none");
            chkAdvanced1.Checked = false;
			
            litBaseUrl2.Text = baseUrl;
            pnlReplacementText2.Style.Add("display", "none");
            chkAdvanced2.Checked = false;
		}

		private void initExisting()
		{
			string baseUrl = GuaranteeTrailingSlash(_site.Url);

			litSite.Text = _site.Name;
            txtLinkGroupTitle.Text = _group.Title;

            pnlDistribution.Visible = _group.Distribution == 0;

            // 1st Keyword
			litBaseUrl1.Text = baseUrl;
            txtKeyword1.Text = _group.Keyword1;
            txtReplacementText1.Text = _group.ReplacementText1;
            if (txtKeyword1.Text == txtReplacementText1.Text)
            {
                pnlReplacementText1.Style.Add("display", "none");
                chkAdvanced1.Checked = false;
            }
            else
                chkAdvanced1.Checked = true;
            txtUrl1.Text = _group.Url1;

            // 2nd Keyword
            litBaseUrl2.Text = baseUrl;
            txtKeyword2.Text = _group.Keyword2;
            txtReplacementText2.Text = _group.ReplacementText2;
            if (txtKeyword2.Text == txtReplacementText2.Text)
            {
                pnlReplacementText2.Style.Add("display", "none");
                chkAdvanced2.Checked = false;
            }
            else
                chkAdvanced2.Checked = true;
            txtUrl2.Text = _group.Url2;
		}

		private string GuaranteeTrailingSlash(string path)
		{
			return path.ToCharArray()[path.Length-1] == '/' ? path : path + '/';
		}

		private void redirectToArticleManager()
		{
			string manageArticlesUrl;

            manageArticlesUrl = ResolveUrl("~/Members/Manage-Link-Articles/");
			Response.Redirect(manageArticlesUrl);			
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			LinkParagraphGroup group;
            int siteId;
            string keyword1, keyword2;
            bool pageIsValid = true;

            // Validate Page
            if (txtKeyword1.Text == string.Empty && txtReplacementText1.Text == string.Empty)
            {
                lblKeywordRequired1.Visible = true;
                pageIsValid = false;
            }
            else
                lblKeywordRequired1.Visible = false;

            if (pnlKeyword2.Visible && txtKeyword2.Text == string.Empty && txtReplacementText2.Text == string.Empty)
            {
                lblKeywordRequired2.Visible = true;
                pageIsValid = false;
            }
            else
                lblKeywordRequired2.Visible = false;

			if(!Page.IsValid || !pageIsValid)
				return;

            // Determine Mode
			switch(_mode)
			{
				case Modes.Create:
					group = new LinkParagraphGroup();
					group.SiteId = _siteId;
					siteId = _siteId;
					break;
				case Modes.Edit:
					group = new LinkParagraphGroup(_groupId);
					_db.PopulateLinkParagraphGroup(group);
					siteId = group.SiteId;
					break;
				default:
					throw new NotSupportedException(string.Format("{0} is not supported.", _mode.ToString()));
			}

            // Save
            keyword1 = txtKeyword1.Text == string.Empty ? txtReplacementText1.Text : txtKeyword1.Text;
            keyword2 = txtKeyword2.Text == string.Empty ? txtReplacementText2.Text : txtKeyword2.Text;

            if (group.Paragraphs != null && (group.Keyword1 != keyword1 || group.Keyword2 != keyword2))
                foreach (LinkParagraph paragraph in group.Paragraphs)
                {
                    paragraph.ChangeKeywords(keyword1, keyword2);
                    _db.SaveLinkParagraph(paragraph);
                }

			group.Title = txtLinkGroupTitle.Text;

            // 1st Keyword
            group.Keyword1 = keyword1;
            group.ReplacementText1 = (!chkAdvanced1.Checked || txtReplacementText1.Text == string.Empty) ? txtKeyword1.Text : txtReplacementText1.Text;
            group.Url1 = txtUrl1.Text;

            // 2nd Keyword
            group.Keyword2 = keyword2;
            group.ReplacementText2 = (!chkAdvanced2.Checked || txtReplacementText2.Text == string.Empty) ? txtKeyword2.Text : txtReplacementText2.Text;
            group.Url2 = txtUrl2.Text;

			_db.SaveLinkParagraphGroup(group);

			redirectToArticleManager();
		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			LinkParagraphGroup group;

			if(_mode != Modes.Edit)
			{
				_log.Warn("The delete button was pressed, but we were not in edit mode");
				return;
			}

			group = new LinkParagraphGroup(_groupId);
			_db.DeleteLinkParagraphGroup(group);

			redirectToArticleManager();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			redirectToArticleManager();
		}
	}
}
