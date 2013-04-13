using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.LinkPage
{
	/// <summary>
	///		Generates the static HTML link pages for a particular site.
	/// </summary>
	public class StaticLinkFileGenerator
	{
		private Database _db;
		private Site _site;

		/// <summary>
		///		Creates a new instance of the <see cref="StaticLinkFileGenerator"/> class.
		/// </summary>
		public StaticLinkFileGenerator(Database db, Site site)
		{
			_db = db;
			_site = site;
		}

		/// <summary>
		///		Generates all of the static link pages
		/// </summary>
		public LinkFile[] GeneratePages()
		{
			LinkFile[] fileArr;
			Components.LinkPage[] linkPages;

			linkPages = _db.GetSiteLinkPages(_site.Id);
			fileArr = new LinkFile[linkPages.Length];

			for (int i = 0; i < linkPages.Length; i++)
			{
				fileArr[i] = createLinkFile(linkPages[i]);
			}

			return fileArr;
		}

		private LinkFile createLinkFile(Components.LinkPage linkPage)
		{
			LinkPageHtmlGenerator generator;
			string pageName;
			string pageHtml;
			LinkFile currFile;

			//Todo: pass in the site template
			generator = new LinkPageHtmlGenerator();

			//Todo: Load the page extension for the site
			generator.CategoryUrlSignature = "{1}.html";
			pageHtml = generator.GetPageHtml(_db, linkPage);
			pageName = string.Format("{0}.html", linkPage.PageName);

			currFile = new LinkFile(pageName, pageHtml);

			return currFile;
		}

	}
}