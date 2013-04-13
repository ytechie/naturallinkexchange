using YTech.General.DataMapping;
using YTech.General.Collections;

namespace Nle.Components
{
	/// <summary>
	///		Represents a category that an article can belong to.
	/// </summary>
	public class LinkCategory
	{
		private int _id;
		private string _name;
		private string _description;
		private string _pageName;
		private int _parentCategoryId;
		private StringObjectCollection _extendedProperties;
		private string _metaKeywords;
		private string _metaDescription;
		private string _title;

		#region Public Properties

		/// <summary>
		///		The name of the category.
		/// </summary>
		[FieldMapping("Name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		///		A short description about the site.
		/// </summary>
		[FieldMapping("Description")]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		/// <summary>
		///		A unique name that is assigned to static link
		///		pages, and is also used as a key for dynamic
		///		link pages.
		/// </summary>
		[FieldMapping("PageName")]
		public string PageName
		{
			get { return _pageName; }
			set { _pageName = value; }
		}

		/// <summary>
		///		The ID of the parent category for this category.
		/// </summary>
		[FieldMapping("ParentCategoryId")]
		public int ParentCategoryId
		{
			get { return _parentCategoryId; }
			set { _parentCategoryId = value; }
		}

		/// <summary>
		///		The unique identifier for this category.
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		///		Additional non-standard properties.
		/// </summary>
		public StringObjectCollection ExtendedProperties
		{
			get { return _extendedProperties; }
		}

		/// <summary>
		///		The keywords to substitute into the meta
		///		tag in the header of the page.
		/// </summary>
		[FieldMapping("MetaKeywords")]
		public string MetaKeywords
		{
			get { return _metaKeywords; }
			set { _metaKeywords = value; }
		}

		/// <summary>
		///		The description to substitute into the meta
		///		tag in the header of the page.
		/// </summary>
		[FieldMapping("MetaDescription")]
		public string MetaDescription
		{
			get { return _metaDescription; }
			set { _metaDescription = value; }
		}

		/// <summary>
		///		The title for the link page.
		/// </summary>
		[FieldMapping("Title")]
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		#endregion

		/// <summary>
		///		Creates a new instance of the <see cref="Category"/> class.
		/// </summary>
		/// <param name="categoryId"></param>
		public LinkCategory(int categoryId)
		{
			_id = categoryId;
			_extendedProperties = new StringObjectCollection();
		}
	}
}