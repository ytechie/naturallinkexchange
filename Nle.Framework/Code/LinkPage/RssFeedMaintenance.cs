using System.Reflection;
using log4net;
using Nle.Db.SqlServer;
using Rss;

namespace Nle.LinkPage
{
	/// <summary>
	///		Updates the RSS feed items cached in the database.
	/// </summary>
	public class RssFeedMaintenance
	{
		private Database _db;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		///		Creates a new instance of the <see cref="RssFeedMaintenance"/>
		/// </summary>
		/// <param name="db"></param>
		public RssFeedMaintenance(Database db)
		{
			_db = db;
		}

		/// <summary>
		///		Retrieves all of the feeds that need to be updated, and
		///		saves the current items for those feeds.
		/// </summary>
		public void Run()
		{
			Components.RssFeed[] feeds;

			//Retrieve a list of the feeds to be updated.
			feeds = _db.GetFeedsToUpdate();

			foreach (Components.RssFeed currFeed in feeds)
			{
				UpdateFeed(currFeed);
			}
		}

		/// <summary>
		///		Retrieves the latest data for the specified RSS
		///		feed, and saves those items to the database.
		/// </summary>
		/// <param name="feed"></param>
		public void UpdateFeed(Components.RssFeed feed)
		{
			RssFeed feedData;
			RssItemCollection feedItems;

			feedData = RssFeed.Read(feed.RssUrl);

			if (feedData.Channels.Count == 0)
			{
				_log.DebugFormat("No channels found while retrieving feed from '{0}'", feed.RssUrl);
				return;
			}

			feedItems = feedData.Channels[0].Items;

			if (feedItems.Count == 0)
			{
				_log.DebugFormat("No items found while retrieving feed from '{0}'", feed.RssUrl);
				return;
			}

			_log.DebugFormat("Found {0} RSS feed items while retrieving feed from '{1}'", feedItems.Count, feed.RssUrl);

			_db.SaveRssFeedItems(feed.Id, feedItems);
		}
	}
}