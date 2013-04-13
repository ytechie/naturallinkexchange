using System;
using Nle.Components;
using Telerik.WebControls;

namespace Nle.Controls
{
	/// <summary>
	///		A <see cref="RadTreeView"/> that can auto populate itself on
	///		the <see cref="PreRender"/> event.  You just need to supply a
	///		<see cref="GetChildCategoriesDelegate"/> so that it can retrieve
	///		the nodes.
	/// </summary>
	public class CategoryList : RadTreeView
	{
		/// <summary>
		///		The required delegate so that child categories can be retrieved.  It
		///		should support a paramter of NULL so that the parent categories can
		///		be retrieved.
		/// </summary>
		public delegate LinkCategory[] GetChildCategoriesDelegate(LinkCategory parent);

		private GetChildCategoriesDelegate _getChildren;

		private LinkCategory _topCategory;

		/// <summary>
		///		Gets or sets the method used to populate the category tree.
		/// </summary>
		public GetChildCategoriesDelegate GetChildren
		{
			get { return _getChildren; }
			set { _getChildren = value; }
		}

		/// <summary>
		///		Creates a new instance of the <see cref="CategoryList"/> class.  This
		///		is primarily used when using the control on a page.
		/// </summary>
		public CategoryList() : this(null)
		{
		}

		/// <summary>
		///		Creates a new instancce of the <see cref="CategoryList"/> class.
		/// </summary>
		/// <param name="getChildren"></param>
		public CategoryList(GetChildCategoriesDelegate getChildren) : this(getChildren, null)
		{
		}

		/// <summary>
		///		Creates a new instancce of the <see cref="CategoryList"/> class.
		/// </summary>
		public CategoryList(GetChildCategoriesDelegate getChildren, LinkCategory topCategory)
		{
			getChildren = GetChildren;
			_topCategory = topCategory;

			base.PreRender += new EventHandler(CategoryList_PreRender);
		}

		private void populateChildNodes(LinkCategory category, RadTreeNode node)
		{
			RadTreeNode currNode;
			LinkCategory[] currCategories;

			if (_getChildren == null)
				return;

			currCategories = _getChildren(category);

			foreach (LinkCategory currCategory in currCategories)
			{
				currNode = new RadTreeNode();
				node.Nodes.Add(currNode);

				currNode.Text = currCategory.Name;
				currNode.Value = currCategory.Id.ToString();

				populateChildNodes(currCategory, currNode);
			}
		}

		private void CategoryList_PreRender(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				RadTreeNode rootNode;

				rootNode = new RadTreeNode();
				this.Nodes.Add(rootNode);
				rootNode.Expanded = true;

				rootNode.Text = "All Categories";
				populateChildNodes(_topCategory, rootNode);
			}
		}
	}
}