using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a link page for a site.
	/// </summary>
	public class LinkPage
	{
		private int _id;
		private int _siteId;
		private int _categoryId;
		private int _articleTarget;
		private string _pageName;
		private string _pageTitle;

		/// <summary>
		///		Tracks whether or not the ID field has been set.
		/// </summary>
		private bool _idSet = false;

		/// <summary>
		///		The identifier for the site that this link page belongs to.
		/// </summary>
		[FieldMapping("SiteId")]
		public int SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		/// <summary>
		///		The category that this link page is for.
		/// </summary>
		[FieldMapping("CategoryId")]
		public int CategoryId
		{
			get { return _categoryId; }
			set { _categoryId = value; }
		}

		/// <summary>
		///		The unique identifier for this site.
		/// </summary>
		[FieldMapping("Id")]
		public int Id
		{
			set
			{
				_id = value;
				_idSet = true;
			}
			get
			{
				if(!_idSet)
					throw new ApplicationException("Link page ID field cannot be read before setting");

				return _id;
			}
		}

		/// <summary>
		///		The number of articles that this link
		///		page should eventually contain.
		/// </summary>
		[FieldMapping("ArticleTarget")]
		public int ArticleTarget
		{
			get { return _articleTarget; }
			set { _articleTarget = value; }
		}

		/// <summary>
		///		The file name of the page, and also the name
		///		used when referencing it in the URL.
		/// </summary>
		[FieldMapping("PageName")]
		public string PageName
		{
			get { return _pageName; }
			set { _pageName = value; }
		}

		/// <summary>
		///		The title of this link page.
		/// </summary>
		[FieldMapping("PageTitle")]
		public string PageTitle
		{
			get { return _pageTitle; }
			set { _pageTitle = value; }
		}

		/// <summary>
		///		Creates a new instance of the <see cref="LinkPage"/> class.
		/// </summary>
		public LinkPage()
		{
		}

		/// <summary>
		///		Creates a new instance of the <see cref="LinkPage"/> class.
		/// </summary>
		/// <param name="linkPageId"></param>
		public LinkPage(int linkPageId)
		{
			_id = linkPageId;
			_idSet = true;
		}
	}
}
