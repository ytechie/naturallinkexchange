using System;
using System.Text.RegularExpressions;
using Rss;

namespace Nle.Client.RssFeeds.Sources
{
	/// <summary>
	///		Provides a simple way to retrieve Rss feeds from Yahoo
	///		news based on keyword searches.
	/// </summary>
	public class YahooRssSource : IRssFeedSource
	{
		/// <summary>
		///		The feed signature for the Yahoo news RSS source
		/// </summary>
        public const string FEED_SIGNATURE = "http://news.search.yahoo.com/news/rss?p={0}&ei=UTF-8&fl=0&x=wrt";

		/// <summary>
		///		The regular expression that parses out the true URL from the URL's that
		///		are in the titles of the RSS items.
		/// </summary>
		private const string REGEX_TRUE_URL = @"(?<=/\*).+";

		private string _searchPhrase;

		/// <summary>
		///		Creates a new instance of the <see cref="YahooRssSource"/> class.
		/// </summary>
		public YahooRssSource(string searchPhrase)
		{
			_searchPhrase = searchPhrase;
		}

		/// <summary>
		///		Gets the <see cref="RssFeed"/> for the specified
		///		search terms.
		/// </summary>
		/// <returns></returns>
		public RssFeed ReadFeed()
		{
			string rssUrl;
			RssFeed feed;

			rssUrl = GenerateRequestUrl(_searchPhrase);
			feed = RssFeed.Read(rssUrl);

			removeYahooLinkReferences(feed);

			return feed;
		}

		/// <summary>
		///		Removes all of the Yahoo redirector links from the URL's
		///		in the titles so that the links go directly to the right pages.
		/// </summary>
		private void removeYahooLinkReferences(RssFeed feed)
		{
			if(feed.Channels.Count == 0)
				return;

			foreach(RssItem currItem in feed.Channels[0].Items)
			{
                try
                {
                    currItem.Link = new Uri(RemoveYahooLinkReference(currItem.Link.ToString()));
                }
                catch (UriFormatException)
                {
                    //Sometimes Yahoo gives a bad URL, we'll just ignore them
                }
			}
		}

		/// <summary>
		///		Extracts the real URL embedded in the Yahoo title URL
		/// </summary>
		/// <param name="link">
		///		The URL contained in the header of the Yahoo news item.
		/// </param>
		/// <returns>
		///		The embedded URL
		/// </returns>
		public static string RemoveYahooLinkReference(string link)
		{
			Match match;
            string validUrl;

			match = Regex.Match(link, REGEX_TRUE_URL);

            if (match != null)
                validUrl = match.Value.Replace("http:/", "http://");
            else
                validUrl = null;

            return validUrl;
		}

		/// <summary>
		///		Generates a cache key that can be used when storing the feed in
		///		a cache.  The key is unique based on the search phrase.
		/// </summary>
		/// <remarks>
		///		This is used so that you can cache a feed for a certain amount of time
		///		on a page, so that you don't keep re-requesting it.  It also allows you
		///		to reuse the feed on multiple pages without reloading.
		/// </remarks>
		/// <returns></returns>
		public string GetCacheKey()
		{
			return "YahooRssSourceCacheKey_" + _searchPhrase;
		}

		/// <summary>
		///		Generates the URL that can be used to request an RSS feed
		///		from Yahoo news.
		/// </summary>
		/// <param name="searchPhrase">
		///		The phrase to generate an RSS feed for.  The feed items that are
		///		found will have the specified search terms in them.
		/// </param>
		/// <returns></returns>
		public static string GenerateRequestUrl(string searchPhrase)
		{
			if (searchPhrase == null)
				throw new NullReferenceException("Search phrase for Yahoo RSS feed cannot be null");

			searchPhrase = searchPhrase.Replace(' ', '+');

			return string.Format(FEED_SIGNATURE, searchPhrase);
		}
	}
}