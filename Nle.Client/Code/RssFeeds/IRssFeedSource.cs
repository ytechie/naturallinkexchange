using System;
using Rss;

namespace Nle.Client.RssFeeds
{
	/// <summary>
	///		Defines a data source that can be used to read an RSS feed.
	/// </summary>
	public interface IRssFeedSource
	{
		/// <summary>
		///		Reads the rss feed from the source
		/// </summary>
		/// <returns></returns>
		RssFeed ReadFeed();

		/// <summary>
		///		Gets a unique cache key based on the feed criteria.
		/// </summary>
		/// <returns></returns>
		string GetCacheKey();
	}
}
