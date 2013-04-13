using System;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rss;
using log4net;

namespace Nle.Client.RssFeeds.Controls
{
	/// <summary>
	///		Shows RSS feeds in a simple format that is
	///		both search engine friendly, and user friendly.
	/// </summary>
	public class FeedDisplay : WebControl
	{
		private IRssFeedSource _feedSource;
		private int _headerLevel = 4;
		private TimeSpan _cacheTime = TimeSpan.FromHours(12.0);
        private bool _showSignature = false;
		private bool _showDescription = true;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		///		The header level to use for the item headers.  For example,
		///		to set the header tags as "h4", set this to 4.
		/// </summary>
		/// <remarks>
		///		The default is 4
		/// </remarks>
		public int HeaderLevel
		{
			get { return _headerLevel; }
			set { _headerLevel = value; }
		}

		/// <summary>
		///		The amount of time to cache the feed items for
		///		after reading them.
		/// </summary>
		public TimeSpan CacheTime
		{
			get { return _cacheTime; }
			set { _cacheTime = value; }
		}

		public bool ShowDescription
		{
			get { return _showDescription; }
			set { _showDescription = value; }
		}

        /// <summary>
        ///     Gets or sets whether the Published date
        ///     should be displayed for each item.
        /// </summary>
        public bool ShowSignature
        {
            get { return _showSignature; }
            set { _showSignature = value; }
        }

		/// <summary>
		///		Creates a new instance of the <see cref="FeedDisplay"/> class.
		/// </summary>
		/// <param name="feedSource">
		///		The <see cref="IRssFeedSource"/> to read the feed from.
		/// </param>
		public FeedDisplay(IRssFeedSource feedSource)
		{
			_feedSource = feedSource;
		}

		/// <summary>
		///		Reads the feed from the cache if it exists, otherwise, it
		///		reloads the feed from the source.
		/// </summary>
		private RssFeed getFeed()
		{
			Cache cache = Page.Cache;
			string cacheKey;
			object cacheObject;
			RssFeed feed;

			cacheKey = _feedSource.GetCacheKey();

			cacheObject = cache[cacheKey];
			if(cacheObject == null)
			{
				_log.Debug("Feed was not cached, reloading from RSS source");
				feed = _feedSource.ReadFeed();
				_log.Debug("Feed successfully loaded") ;
				cache[cacheKey] = feed;
			}
			else
			{
				_log.Debug("Feed cached, using cached copy");
				feed = (RssFeed)cacheObject;
				_log.Debug("Feed successfully loaded from the cache");
			}

			return feed;
		}

		/// <summary>
		///		Renders the feed dislpay to the specified <see cref="HtmlTextWriter"/>.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			RssFeed feed;
			HyperLink titleLink;

			try
			{
				feed = getFeed();

				foreach (RssItem currItem in feed.Channels[0].Items)
				{
					titleLink = new HyperLink();

					writer.WriteFullBeginTag("h" + _headerLevel.ToString());
					titleLink.Text = currItem.Title;
					titleLink.NavigateUrl = currItem.Link.ToString();
					titleLink.RenderControl(writer);
					writer.WriteEndTag("h" + _headerLevel.ToString());

					if (_showDescription)
					{
						writer.WriteFullBeginTag("p");
						writer.Write(currItem.Description);
						writer.WriteEndTag("p");
					}

					if(ShowSignature)
                    {
                        writer.WriteBeginTag("p");
                        writer.WriteAttribute("class", "RssItemSignature");
                        writer.Write(">");
                        writer.Write(string.Format("Published {0}", currItem.PubDate));
                        writer.WriteEndTag("p");
                    }
				}
			}
			catch (WebException wex)
			{
				_log.Warn("Error rendering FeedDisplay control", wex);

				writer.WriteBeginTag("p");
				writer.WriteAttribute("class", "ConnectionError");
				writer.Write(">");
				writer.Write(HttpUtility.HtmlEncode("<!-- RSS connection could not be established. -->"));
				writer.WriteEndTag("p");
			}
		}

	}
}