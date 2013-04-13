using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Controls;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using Nle.Website.Members.Common;
using Telerik.WebControls;
using YTech.General.Web.JavaScript;

namespace Nle.Website.Members
{
	/// <summary>
	///		The category administration page.
	/// </summary>
	public partial class CategoryAdministrationDefault : Page
	{
		public const string MY_PATH = "Members/Administration/Category-Administration/";
		public const string MY_FILE_NAME = "";

		private Database _db;
		private JavaScriptBlock _scriptBlock;


		//Todo: Check if the user is an admin!!!!

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();

			if(!Page.IsPostBack)
				populateCategoryList();

			txtNewRelatedCategoryId.ServerChange += new EventHandler(txtNewRelatedCategoryId_ServerChange);

			initJavaScript();

			setUpTree();
			initAddRelatedCategory();
			
			cmdRemove.Click += new EventHandler(cmdRemove_Click);

            MainMaster mp = (MainMaster)Page.Master;

            mp.AddStylesheet("CategoryAdministration.css");	
		}

		private void initJavaScript()
		{
			_scriptBlock = new JavaScriptBlock();
			controlPlaceholder.Controls.Add(_scriptBlock);
		}

		private void setUpTree()
		{
			categoryTree.RadControlsDir = Page.ResolveUrl("~/ThirdParty/RadControls/");
			categoryTree.NodeClick += new Telerik.WebControls.RadTreeView.RadTreeViewEventHandler(categoryTree_NodeClick);
			categoryTree.AutoPostBack = true;
		}

		/// <summary>
		///		Sets up the "Add" button for the related categories
		/// </summary>
		private void initAddRelatedCategory()
		{
			string addCategoryUrl;

			addCategoryUrl = CategorySelector.GetLoadUrl(txtNewRelatedCategoryId.ClientID);

			cmdAdd.Attributes["onclick"] = JavaScriptBlock.GetFunctionCall("ShowDialog", true, JavaScriptBlock.SQuote(addCategoryUrl) , "500", "400", "true");
		}

		private void populateCategoryList()
		{
			categoryTree.GetChildren = new CategoryList.GetChildCategoriesDelegate(_db.GetLinkCategories);
//			RadTreeNode rootNode;
//
//			rootNode = new RadTreeNode();
//			categoryTree.Nodes.Add(rootNode);
//			rootNode.Expanded = true;
//			rootNode.Text = "All Categories";
//
//			populateChildNodes(null, rootNode);
		}
//
//		private void populateChildNodes(LinkCategory category, RadTreeNode node)
//		{
//			RadTreeNode currNode;
//			LinkCategory[] currCategories;
//
//			currCategories = _db.GetLinkCategories(category) ;
//
//			foreach(LinkCategory currCategory in currCategories)
//			{
//				currNode = new RadTreeNode();
//				node.Nodes.Add(currNode);
//	
//				currNode.Text = currCategory.Name;
//				currNode.Value = currCategory.Id.ToString();
//
//				populateChildNodes(currCategory, currNode);
//			}
//		}

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

		private void populateCategoryDetails(LinkCategory category)
		{
			populateRelatedCategories(category);
			populateAllRelatedCategories(category);
		}

		private void populateRelatedCategories(LinkCategory category)
		{
			LinkCategory[] relatedCategories;

			relatedCategories = _db.GetRelatedCategories(category.Id);
			populateCategoryList(lstRelatedCategories, relatedCategories);
		}

		private void populateAllRelatedCategories(LinkCategory category)
		{
			LinkCategory[] relatedCategories;
		
			relatedCategories = _db.GetAllRelatedCategories(category.Id) ;
			populateCategoryList(lstAllRelatedCategories, relatedCategories);
		}

		private void populateCategoryList(ListBox list, LinkCategory[] categories)
		{
			ListItem newItem;

			list.Items.Clear();
			
			foreach(LinkCategory currCategory in categories)
			{
				newItem = new ListItem();
				newItem.Text = currCategory.Name;
				newItem.Value = currCategory.Id.ToString();

				list.Items.Add(newItem);
			}
		}

		private void categoryTree_NodeClick(object o, RadTreeNodeEventArgs e)
		{
			LinkCategory category;

			category = new LinkCategory(int.Parse(e.NodeClicked.Value));
			_db.PopulateLinkCategory(category);

			populateCategoryDetails(category);
		}

		/// <summary>
		///		Gets the selected category that the user selected from the main list.
		/// </summary>
		/// <returns></returns>
		private LinkCategory getSelectedCategory()
		{
			int selectedId;
			LinkCategory category;

			selectedId = int.Parse(categoryTree.SelectedNode.Value);

			category = new LinkCategory(selectedId);
			_db.PopulateLinkCategory(category);

			return category;
		}

		private void txtNewRelatedCategoryId_ServerChange(object sender, EventArgs e)
		{
			string categoryIdString;
			string[] categoryIdStrings;
			string currIdString;
			int currId;
			LinkCategory selectedCategory;

			categoryIdString = txtNewRelatedCategoryId.Value;

			if(categoryIdString == null || categoryIdString.Length == 0)
			{
				_scriptBlock.ShowAlert("No value received from category selection dialog.  No categories will be added.");
			}

			//Get the Id of the category they are editing
			selectedCategory = getSelectedCategory();

			categoryIdStrings = categoryIdString.Split(',');

			for(int i = 0; i < categoryIdStrings.Length; i++)
			{
				currIdString = categoryIdStrings[i].Trim();
				currId = int.Parse(currIdString);

				_db.AddCategoryRelationship(selectedCategory.Id, currId);
			}

			//Refresh the category details
			populateCategoryDetails(selectedCategory);
		}

		private void cmdRemove_Click(object sender, EventArgs e)
		{
			LinkCategory mainCategory;
			int relatedCategoryId;
			ListItem selectedItem;

			selectedItem = lstRelatedCategories.SelectedItem;

			if(lstRelatedCategories.SelectedItem == null)
				return;

			mainCategory = getSelectedCategory();
			relatedCategoryId = int.Parse(lstRelatedCategories.SelectedValue);

			_db.DeleteCategoryRelationship(mainCategory.Id, relatedCategoryId);

			lstRelatedCategories.Items.Remove(selectedItem);
		}
	}
}