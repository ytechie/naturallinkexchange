using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Client.RssFeeds;
using Nle.Client.RssFeeds.Controls;
using Nle.Client.RssFeeds.Sources;

namespace Nle.Website.ServerServices.Yahoo_Rss_Feed
{
	/// <summary>
	///		Provides a way to access the RAW Yahoo RSS feed from <see cref="Nle.Client"/>.
	/// </summary>
	public partial class _Default : Page
	{

		private const string PARAM_SEARCH_STRING = "Search-String";

		private string _searchString;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString[PARAM_SEARCH_STRING] == null)
				throw new ApplicationException(PARAM_SEARCH_STRING + " is a required parameter.");
			else
				_searchString = Request.QueryString[PARAM_SEARCH_STRING];

			displayFeed(_searchString);
		}

		private void displayFeed(string keywords)
		{
			FeedDisplay feedDisp;
			IRssFeedSource yahooFeed;

			yahooFeed = new YahooRssSource("Search Engine Optimization");
			feedDisp = new FeedDisplay(yahooFeed);
			feedPlaceHolder.Controls.Add(feedDisp);
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
	}
}