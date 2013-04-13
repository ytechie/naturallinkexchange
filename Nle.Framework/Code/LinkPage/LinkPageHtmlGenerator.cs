using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web.Controls;

namespace Nle.LinkPage
{
	/// <summary>
	///		Generates the link pages.
	/// </summary>
	public class LinkPageHtmlGenerator
	{
		/// <summary>The string to replace with the actual title when generating a link page.</summary>
		public const string LPTOKEN_TITLE = "{title}";
		/// <summary>The string to replace with the meta keywords when generating a link page.</summary>
		public const string LPTOKEN_META_KEYWORDS = "{metaKeywords}";
		/// <summary>The string to replace with the meta description when generating a link page.</summary>
		public const string LPTOKEN_META_DESCRIPTION = "{metaDescription}";
		/// <summary>The string to replace with the related categories when generating a link page.</summary>
		public const string LPTOKEN_RELATED_CATEGORIES = "{relatedCategories}";
		/// <summary>The string to replace with the RSS feeds when generating a link page.</summary>
		public const string LPTOKEN_RSS_FEEDS = "{rssFeeds}";
		/// <summary>The string to replace with the articles when generating a link page.</summary>
		public const string LPTOKEN_ARTICLES = "{articles}";

		private string _template;
		private Components.LinkPage[] _relatedCategories;
		private LinkParagraph[] _articles;
		private RssFeedItem[] _rssFeedItems;
		private string _pageTitle;
		private string _categoryUrlSignature;
		private LinkCategory _categoryData;
		private bool _hideInitialSetupMessage = false;

		#region Public Properties

		/// <summary>
		///		The template design for the webpage.
		/// </summary>
		public string Template
		{
			get { return _template; }
			set { _template = value; }
		}

		/// <summary>
		///		An array of <see cref="Components.LinkPage"/> objects that represent
		///		the other categories that will be linked to.
		/// </summary>
		public Components.LinkPage[] RelatedCategories
		{
			get { return _relatedCategories; }
			set { _relatedCategories = value; }
		}

		/// <summary>
		///		The articles that belong to this link page.  They will automatically
		///		be formatted using their URL's and their URL text.
		/// </summary>
		public LinkParagraph[] Articles
		{
			get { return _articles; }
			set { _articles = value; }
		}

		/// <summary>
		///		The RSS items to display on the link page.
		/// </summary>
		public RssFeedItem[] RssFeedItems
		{
			get { return _rssFeedItems; }
			set { _rssFeedItems = value; }
		}

		/// <summary>
		///		The title of the link page.
		/// </summary>
		public string PageTitle
		{
			get { return _pageTitle; }
			set { _pageTitle = value; }
		}

		/// <summary>
		///		The URL signature of the links.  Ex: http://mysite.com/dir/page.aspx?Cat={0}.
		///		Use "{0}" to insert the category Id, and "{1}" to insert the page name key.
		/// </summary>
		public string CategoryUrlSignature
		{
			get { return _categoryUrlSignature; }
			set { _categoryUrlSignature = value; }
		}

		/// <summary>
		///		The <see cref="LinkCategory"/> object that contains
		///		the specific data for the category.
		/// </summary>
		public LinkCategory CategoryData
		{
			get { return _categoryData; }
			set { _categoryData = value; }
		}

		#endregion

		/// <summary>
		///		Interacts with the database to load the link page
		///		information, and generate the link page.
		/// </summary>
		/// <param name="db">
		///		The database connection to use to retrieve the link page data.
		/// </param>
		/// <param name="lp">
		///		The link page whose HTML should be generated.
		/// </param>
		/// <param name="overrideTemplate">
		///		If not NULL, this overrides the saved template for
		///		the user.
		/// </param>
		/// <remarks>
		///		<see cref="CategoryUrlSignature"/> is the only public property
		///		that needs to be set for this to function properly.
		/// </remarks>
		/// <returns>
		///		The HTML representing the link page.
		/// </returns>
		public string GetPageHtml(Database db, Components.LinkPage lp, string overrideTemplate)
		{
			string pageHtml;
			Site s;
			LinkCategory categoryData;

			//Load the articles for this link page
			Articles = db.GetLinkPageArticles(lp.Id);

			//Load the related categories for this page
			RelatedCategories = db.GetRelatedLinkPages(lp);

			//Load the RSS feeds
			RssFeedItems = db.GetRssItemsForLinkPage(lp.Id);

			//Look up the site information
			s = new Site(lp.SiteId);
			db.PopulateSite(s);

			//Check if their initial setup message should be hidden
			_hideInitialSetupMessage = s.HideInitialSetupMessage;

			//Load the template
			if(overrideTemplate == null)
				Template = s.PageTemplate;

			//Load the category information for the link page
			categoryData = new LinkCategory(lp.CategoryId);
			db.PopulateLinkCategory(categoryData);
			CategoryData = categoryData;

			//Get the HTML
			pageHtml = GetPageHtml();

			return pageHtml;
		}

		/// <summary>
		///		Interacts with the database to load the link page
		///		information, and generate the link page.
		/// </summary>
		/// <param name="db">
		///		The database connection to use to retrieve the link page data.
		/// </param>
		/// <param name="lp">
		///		The link page whose HTML should be generated.
		/// </param>
		/// <remarks>
		///		<see cref="CategoryUrlSignature"/> is the only public property
		///		that needs to be set for this to function properly.
		/// </remarks>
		/// <returns>
		///		The HTML representing the link page.
		/// </returns>
		public string GetPageHtml(Database db, Components.LinkPage lp)
		{
			return GetPageHtml(db, lp, null);
		}

		/// <summary>
		///		Generates HTML for a specific link page.
		/// </summary>
		/// <returns>
		///		The HTML for the generated page.
		/// </returns>
		public string GetPageHtml()
		{
			string categoryListHtml;
			string linkParagraphsHtml;
			string rssHtml;
			BulletList categoriesList;
			string pageHtml;

			pageHtml = Template;

			//Todo: load the default template if there is none
			if (pageHtml == null)
				pageHtml = "";

			//Title
			pageHtml = Regex.Replace(pageHtml, LPTOKEN_TITLE, getPageTitle(), RegexOptions.IgnoreCase);

			//Meta keywords/description
			pageHtml = Regex.Replace(pageHtml, LPTOKEN_META_KEYWORDS, getMetaKeywords(), RegexOptions.IgnoreCase);
			pageHtml = Regex.Replace(pageHtml, LPTOKEN_META_DESCRIPTION, getMetaDescription(), RegexOptions.IgnoreCase);

			//Retrieve the list of related category links
			categoriesList = getCategoryLinksList(RelatedCategories, CategoryUrlSignature);
			categoryListHtml = ControlUtilities.GetControlHtml(categoriesList);
			pageHtml = Regex.Replace(pageHtml, LPTOKEN_RELATED_CATEGORIES, categoryListHtml, RegexOptions.IgnoreCase);

			//Get the HTML for the link paragraphs
			linkParagraphsHtml = getLinkParagraphs(Articles);
			pageHtml = Regex.Replace(pageHtml, LPTOKEN_ARTICLES, linkParagraphsHtml, RegexOptions.IgnoreCase);

			//Get the RSS feed HTML
			rssHtml = getRssFeedHtml(RssFeedItems);
			pageHtml = Regex.Replace(pageHtml, LPTOKEN_RSS_FEEDS, rssHtml, RegexOptions.IgnoreCase);

			return pageHtml;
		}

		private string getPageTitle()
		{
			//Use the override if available
			if (PageTitle != null)
				return PageTitle;

			//Use the category title if available
			if (CategoryData != null && CategoryData.Title != null)
				return CategoryData.Title;

			//No title was found
			return "";
		}

		private string getMetaKeywords()
		{
			if (CategoryData != null && CategoryData.MetaKeywords != null)
				return CategoryData.MetaKeywords;
			else
				return "";
		}

		private string getMetaDescription()
		{
			if (CategoryData != null && CategoryData.Description != null)
				return CategoryData.MetaKeywords;
			else
				return "";
		}

		/// <summary>
		///		Gets a <see cref="BulletList"/> that contains the links
		///		to the specified categories.
		/// </summary>
		/// <param name="linkPages">
		///		An array of <see cref="Components.LinkPage"/> objects that will be linked to.
		/// </param>
		/// <param name="categoryUrlSignature">
		///		The URL signature of the links.  Ex: http://mysite.com/dir/page.aspx?Cat={0}.
		///		Use "{0}" to insert the category Id, and "{1}" to insert the page name key.
		/// </param>
		/// <returns>
		///		A <see cref="BulletList"/> of links to the specified categories.
		/// </returns>
		private BulletList getCategoryLinksList(Components.LinkPage[] linkPages, string categoryUrlSignature)
		{
			BulletList list;
			HyperLink currLink;
			BulletListItem currListItem;

			if (categoryUrlSignature == null)
				throw new NullReferenceException("categoryUrlSignature Cannot Be Null");

			list = new BulletList();

			if (linkPages != null)
			{
				foreach (Components.LinkPage currLinkPage in linkPages)
				{
					currLink = new HyperLink();
					currLink.NavigateUrl = string.Format(categoryUrlSignature, currLinkPage.Id, currLinkPage.PageName);
					currLink.Text = currLinkPage.PageTitle;

					currListItem = new BulletListItem();
					currListItem.Controls.Add(currLink);

					list.Controls.Add(currListItem);
				}
			}

			return list;
		}

		/// <summary>
		///		Processes the <see cref="LinkParagraph"/> objects, and generates
		///		the HTML that represents the final HTML paragraphs.
		/// </summary>
		/// <param name="links">
		///		The links to generate the HTML for.
		/// </param>
		/// <returns>
		///		The final HTML ready to be output to a page.
		/// </returns>
		private string getLinkParagraphs(LinkParagraph[] links)
		{
			StringBuilder outputText;
			string currLinkText;

			outputText = new StringBuilder();

			//They have no articles, we'll just generate a default article
			if ((links == null || links.Length == 0) && !_hideInitialSetupMessage)
			{
				links = new LinkParagraph[1];
				links[0] = new LinkParagraph();
				links[0].Title = "Congratulations";
				links[0].UrlBase = "";
				links[0].Url1 = "";
				links[0].ReplacementText1 = "NaturalLinkExchange.com";
				links[0].Keyword1 = "{anchor1}";
				links[0].Paragraph = "Congratulations.  Your link page has been set up sucessfully. " +
					"This article will not appear once a link is created to another site from this page. " +
					"This may take some time, because the site that will be linked will match the category of this page. " +
					"Visit {anchor1} if you require assistance. If you would like this message removed, go to the " +
					"'Link Page Setup' section on the control panel.";
			}

				foreach (LinkParagraph currLink in links)
				{
					currLinkText = currLink.GetFormattedParagraph();

					outputText.Append("<p>");
					outputText.Append(currLinkText);
					outputText.Append("</p>");
				}

			return outputText.ToString();
		}

		/// <summary>
		///		Gets the formatted paragraphs for the specified RSS
		///		feed items.
		/// </summary>
		/// <param name="feedItems"></param>
		/// <returns></returns>
		private string getRssFeedHtml(RssFeedItem[] feedItems)
		{
			StringBuilder html;

			html = new StringBuilder();

			if (feedItems != null && feedItems.Length > 0)
			{
				foreach (RssFeedItem currFeedItem in feedItems)
				{
					html.Append("<h4>");
					html.Append(currFeedItem.ItemTitle);
					html.Append("</h4>");
					html.Append("<p>");
					html.Append(currFeedItem.ItemText);
					html.Append("</p>");
				}
			}

			return html.ToString();
		}
	}
}