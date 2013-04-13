using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents an RSS feed that will be monitored and
	///		whose items will be cached in the Database.
	/// </summary>
	public class RssFeed
	{
		private int _id;
		private string _rssUrl;
		private string _name;
		private string _siteUrl;
		private TimeSpan _updateInterval;
		private DateTime _lastUpdate;

		/// <summary>
		///		The unique identifier of this Feed.
		/// </summary>
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		///		The URL of the RSS feed
		/// </summary>
		[FieldMapping("RssUrl")]
		public string RssUrl
		{
			get { return _rssUrl; }
			set { _rssUrl = value; }
		}

		/// <summary>
		///		The name of the RSS feed
		/// </summary>
		[FieldMapping("Name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		///		The URL of the site that the RSS
		///		feed belongs to.
		/// </summary>
		[FieldMapping("SiteUrl")]
		public string SiteUrl
		{
			get { return _siteUrl; }
			set { _siteUrl = value; }
		}

		/// <summary>
		///		The time span, in minutes, between each required update
		///		of the RSS feed items.
		/// </summary>
		[FieldMapping("UpdateInterval")]
		public int UpdateIntervalMinutes
		{
			get { return (int)_updateInterval.TotalMinutes; }
			set { _updateInterval = TimeSpan.FromMinutes(value); }
		}

		/// <summary>
		///		The <see cref="TimeSpan"/> between each required update
		///		of the caches RSS feed items.
		/// </summary>
		public TimeSpan UpdateInterval
		{
			get	{	return _updateInterval;	}
			set	{	_updateInterval = value;	}
		}

		/// <summary>
		///		The <see cref="DateTime"/> of the last retrieval
		///		of the RSS feeds.
		/// </summary>
		[FieldMapping("LastUpdate")]
		public DateTime LastUpdate
		{
			get { return _lastUpdate; }
			set { _lastUpdate = value; }
		}

		/// <summary>
		///		Creates a new instance of a specific RSS feed
		///		with the specified feed Id.
		/// </summary>
		/// <param name="feedId"></param>
		public RssFeed(int feedId)
		{
			Id = feedId;
		}
	}
}
