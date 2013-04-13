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
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;

namespace Nle.Website.Members.Manage_Sites
{
	/// <summary>
	/// Summary description for Edit_Site.
	/// </summary>
	public partial class EditSite : System.Web.UI.Page
	{
		/// <summary>
		///		The path of this page.
		/// </summary>
		public const string MY_PATH = "Members/Manage-Sites/";
		/// <summary>
		///		The file name of this page.
		/// </summary>
		public const string MY_FILE_NAME = "Edit-Site.aspx";

		private const string PARAM_SITE_ID = "SiteId";

		private Database _db;
		private int _userId;
		private int _siteId;
		private Modes _mode;
        private MainMaster _master;
        private StatusHeader _header;

		private enum Modes
		{
			Edit,
			Create
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("~/Members/Manage-Sites/Site.css");
            _header = _master.MasterStatusHeader;

			getParameters();

			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

			cmdSave.Click += new EventHandler(cmdSave_Click);
            cmdDelete.Click += new EventHandler(cmdDelete_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

			if(!Page.IsPostBack)
			{
				initCancelConfirm();
				initSaveConfirm();

				switch(_mode)
				{
					case Modes.Edit:
						loadSite();
						break;
					case Modes.Create:
						initNew();
						break;
					default:
						throw new NotSupportedException(string.Format("{0} is not supported.", _mode.ToString()));
				}
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

		/// <summary>
		/// Gets the URL that can be used to call this page.
		/// </summary>
		/// <param name="siteId">The Id of the Site to be edited.</param>
		/// <returns></returns>
		public static string GetLoadUrl(int siteId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_SITE_ID, siteId);

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
            this.Load += new EventHandler(Page_Load);
		}
		#endregion

		private void getParameters()
		{
			string paramString;

			paramString = Request.QueryString[PARAM_SITE_ID];
			if (paramString != null && paramString.Length > 0)
			{
				_siteId = int.Parse(paramString);
				_mode = Modes.Edit;
			}
			else
			{
				_mode = Modes.Create;
			}
		}

		private void initCancelConfirm()
		{
			JavaScriptBlock.ConfirmClick(cmdCancel, "Are you sure you want to lose your changes and return to the link site manager?");
		}

		private void initSaveConfirm()
		{
			if(_mode == Modes.Create)
				JavaScriptBlock.ConfirmClick(cmdSave, "Are you sure you are ready to save?  The Site Url and Category can not be changed once the site has been added.");
		}

        private void initDeleteConfirm()
        {
            JavaScriptBlock.ConfirmClick(cmdDelete, "Are you sure you want to delete this site?  This action can not be undone.");
        }

		private void initNew()
		{
			foreach(LinkCategory category in _db.GetSiteCategories())
			{
				ListItem li = new ListItem(category.Name, category.Id.ToString());
				ddlCategories.Items.Add(li);
			}

			hypSiteUrl.Visible = false;
			litCategory.Visible = false;

            cmdDelete.Visible = false;
		}

		private void loadSite()
		{
			Site site;
			LinkCategory category;
			
			site = new Site(_siteId);
			_db.PopulateSite(site);

			category = new LinkCategory(site.InitialCategoryId);
			_db.PopulateLinkCategory(category);

			txtSiteName.Text = site.Name;
			hypSiteUrl.Text = site.Url;
			hypSiteUrl.NavigateUrl = site.Url;
			litCategory.Text = category.Name;
            txtSiteUrlRequiredValidator.Enabled = false;
            ddlCategoriesRequiredValidator.Enabled = false;

			txtSiteUrl.Visible = false;
			ddlCategories.Visible = false;

            cmdDelete.Visible = true;
            initDeleteConfirm();
		}

		private void redirectToSiteManager()
		{
			string manageSiteUrl;

			manageSiteUrl = ResolveUrl("~/Members/Manage-Sites/");
			Response.Redirect(manageSiteUrl);			
		}

        void cmdDelete_Click(object sender, EventArgs e)
        {
            _db.DeleteSite(_siteId);
            if(_siteId == Global.GetCurrentSiteId()) _header.SetSelectedSiteId(-1);
            redirectToSiteManager();
        }

		private void cmdSave_Click(object sender, EventArgs e)
		{
			Site site;
            JavaScriptBlock jsb;
            string url;

            if (!Page.IsValid)
                return;

			switch(_mode)
			{
				case Modes.Create:
                    url = WebUtilities.GetProperUrl(txtSiteUrl.Text);
                    if (WebUtilities.ReadWebPage(url, false) == null)
                    {
                        jsb = new JavaScriptBlock();
                        JavascriptPlaceholder.Controls.Add(jsb);
                        jsb.ShowAlert(string.Format("The page you specified ({0}) could not be found or the URL was not in the correct format.", url));
                        txtSiteUrl.Text = url;
                        return;
                    }

					site = new Site();
					site.InitialCategoryId = int.Parse(ddlCategories.SelectedValue);
					site.Url = url;
					site.UserId = Global.GetCurrentUserId();
					site.Enabled = true;
					break;
				case Modes.Edit:
					site = new Site(_siteId);
					_db.PopulateSite(site);
					break;
				default:
					throw new NotSupportedException(string.Format("{0} is not supported.", _mode.ToString()));
			}

			site.Name = txtSiteName.Text;

			_db.SaveSite(site);
			redirectToSiteManager();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			redirectToSiteManager();
		}
	}
}
