using Rss;

namespace Nle.Client.RssFeeds.Sources
{
	public class GenericFeed : IRssFeedSource
	{
		private string _rssUrl;

		public GenericFeed(string rssUrl)
		{
			_rssUrl = rssUrl;
		}

		#region IRssFeedSource Members

		public RssFeed ReadFeed()
		{
			return RssFeed.Read(_rssUrl);
		}

		public string GetCacheKey()
		{
			return "Generic_" + _rssUrl;
		}

		#endregion
	}
}