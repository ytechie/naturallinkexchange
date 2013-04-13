using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Controls;
using Nle.Db.SqlServer;
using Telerik.WebControls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;

namespace Nle.Website.Members.Common
{
	/// <summary>
	///		A common dialog for selecting a category.
	/// </summary>
	public partial class CategorySelector : Page
	{
		public const string MY_PATH = "Members/Common/Category-Selector/";
		public const string MY_FILE_NAME = "";

		/// <summary>
		///		The name of the parameter used to pass in the id of the target textbox
		/// </summary>
		public const string PARAM_TARGET_TEXTBOX_ID = "TargetTextboxId";

		private Database _db;
		private string _targetTextboxId;


		JavaScriptBlock _scriptBlock;

        protected void Page_Load(object sender, EventArgs e)
        {
            _db = Global.GetDbConnection();

            getParameters();

            if (!Page.IsPostBack)
                setUpCategoryTree();

            initJavaScript();

            cmdOk.Click += new EventHandler(cmdOk_Click);

            MainMaster mp = (MainMaster)Page.Master;
            mp.AddStylesheet("CategorySelector.css");
        }

		private void initJavaScript()
		{
			_scriptBlock = new JavaScriptBlock();
			controlPlaceholder.Controls.Add(_scriptBlock);
		}

		private void getParameters()
		{
			_targetTextboxId = Request.QueryString[PARAM_TARGET_TEXTBOX_ID];

			if (_targetTextboxId == null || _targetTextboxId.Length == 0)
				throw new ApplicationException("Missing Parameter '" + PARAM_TARGET_TEXTBOX_ID + "'");
		}

		private void setUpCategoryTree()
		{
			categoryTree.RadControlsDir = Page.ResolveUrl("~/ThirdParty/RadControls/");
			categoryTree.MultipleSelect = false;

			categoryTree.GetChildren = new CategoryList.GetChildCategoriesDelegate(_db.GetLinkCategories);
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
		///		Generates the URL that can be used to call this page.
		/// </summary>
		/// <param name="targetTextboxId"></param>
		/// <returns></returns>
		public static string GetLoadUrl(string targetTextboxId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_TARGET_TEXTBOX_ID, targetTextboxId);

			return url.ToString();
		}

		private void cmdOk_Click(object sender, EventArgs e)
		{
			RadTreeNode selectedNode;

			selectedNode = categoryTree.SelectedNode;

			if(selectedNode == null)
			{
				_scriptBlock.ShowAlert("You Must Select A Category");
				return;
			}

			//Send the value to our parent form
			_scriptBlock.CallFunction("SendValueToParent", JavaScriptBlock.SQuote(_targetTextboxId) , selectedNode.Value);
			_scriptBlock.CloseWindow();
		}
	}
}