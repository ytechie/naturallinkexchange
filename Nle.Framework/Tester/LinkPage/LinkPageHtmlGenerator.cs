using Nle.Components;
using NUnit.Framework;

namespace Nle.LinkPage
{
	/// <summary>
	///		Tester for the <see cref="LinkPageGenerator"/> class.
	/// </summary>
	[TestFixture()]
	public class LinkPageGenerator_Tester
	{
		private LinkPageHtmlGenerator _lpg;

		/// <summary>
		///		Use NULL for everything and make sure it can handle it.  We
		///		can then be fairly confident that it will work if only
		///		some of the values are null.
		/// </summary>
		[Test()]
		public void NullHandling()
		{
			_lpg = new LinkPageHtmlGenerator();
			_lpg.CategoryUrlSignature = "{0}";
			_lpg.Template = LinkPageHtmlGenerator.LPTOKEN_RELATED_CATEGORIES;
			Assert.AreEqual("<ul></ul>", _lpg.GetPageHtml());
		}

		///// <summary>
		/////		Verify that the related categories HTML is generated correctly.
		///// </summary>
		//[Test()]
		//public void GetPageHtmlRelatedCategories()
		//{
		//  string html;

		//  relatedCategories[0] = new LinkCategory(5);
		//  relatedCategories[0].Name = "cat1 name";
		//  relatedCategories[0].PageName = "cat1";
		//  relatedCategories[1] = new LinkCategory(6);
		//  relatedCategories[1].Name = "cat2 name";
		//  relatedCategories[1].PageName = "cat2";

		//  _lpg = new LinkPageHtmlGenerator();
		//  _lpg.Template = LinkPageHtmlGenerator.LPTOKEN_RELATED_CATEGORIES;
		//  _lpg.CategoryUrlSignature = "{1}";
		//  _lpg.RelatedCategories = relatedCategories;

		//  html = _lpg.GetPageHtml();

		//  Assert.AreEqual("<ul><li><a href=\"5-cat1\">cat1 name</a></li><li><a href=\"6-cat2\">cat2 name</a></li></ul>", html);
		//}

		///// <summary>
		/////		Verify that the link paragraph HTML is generated correctly.
		///// </summary>
		//[Test()]
		//public void GetPageHtmlParagraphs()
		//{
		//  LinkParagraph[] paragraphs = new LinkParagraph[2];
		//  string html;
		//  LinkPageData pageData;

		//  pageData = new LinkPageData();

		//  paragraphs[0] = new LinkParagraph(27);
		//  paragraphs[0].Paragraph = "My {0} Paragraph";
		//  paragraphs[0].Url = "http://mysite.com";
		//  paragraphs[0].UrlText = "cool";

		//  paragraphs[1] = new LinkParagraph(27);
		//  paragraphs[1].Paragraph = "Hi {0} There";
		//  paragraphs[1].Url = "http://mysite2.com";
		//  paragraphs[1].UrlText = "awesome";

		//  pageData.Articles = paragraphs;

		//  _lpg = new LinkPageHtmlGenerator();
		//  html = _lpg.GetPageHtml(pageData);

		//  Assert.AreEqual("<ul></ul><p>My <a href=\"http://mysite.com\">cool</a> Paragraph</p><p>Hi <a href=\"http://mysite2.com\">awesome</a> There</p>", html);
		//}
		////
		////		/// <summary>
		////		///		Verify that the RSS feed HTML is generated correctly.
		////		/// </summary>
		////		[Test()]
		////		public void GetPageHtmlRssFeeds()
		////		{
		////			string[] rssItems = new string[2];
		////			string html;
		////
		////			rssItems[0] = "RSS Item 1";
		////			rssItems[1] = "RSS Item 2";
		////
		////			_lpg = new LinkPageHtmlGenerator();
		////			html = _lpg.GetPageHtml(null, null, null, null, rssItems);
		////
		////			Assert.AreEqual("<ul></ul><p>RSS Item 1</p><p>RSS Item 2</p>", html);
		////		}


	}
}