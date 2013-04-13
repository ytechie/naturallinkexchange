using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a cached RSS feed item.  In other words,
	///		a single post.
	/// </summary>
	public class RssFeedItem
	{
		private int _id;
		private string _itemTitle;
		private string _itemText;
		private DateTime _readTime;
		private int _feedId;
		private int _revision;
		private bool _delete;

		#region Public Properties

		[FieldMapping("Id")]
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		///		Gets or sets the title of the RSS feed item.
		/// </summary>
		[FieldMapping("ItemTitle")]
		public string ItemTitle
		{
			get { return _itemTitle; }
			set { _itemTitle = value; }
		}

		/// <summary>
		///		Gets or sets the item text or description.
		/// </summary>
		[FieldMapping("ItemText")]
		public string ItemText
		{
			get { return _itemText; }
			set { _itemText = value; }
		}

		/// <summary>
		///		Gets or sets the time that the feed item was read.
		/// </summary>
		[FieldMapping("ReadTime")]
		public DateTime ReadTime
		{
			get { return _readTime; }
			set { _readTime = value; }
		}

		/// <summary>
		///		Gets the ID of the <see cref="RssFeed"/>.
		/// </summary>
		[FieldMapping("FeedId")]
		public int FeedId
		{
			get { return _feedId; }
			set { _feedId = value; }
		}

		/// <summary>
		///		The revision, or version of the RSS feed item.
		/// </summary>
		[FieldMapping("Revision")]
		public int Revision
		{
			get { return _revision; }
			set { _revision = value; }
		}

		/// <summary>
		///		Gets or sets a value indicating if the feed item
		///		is marked for deletion once it is no longer used.
		/// </summary>
		[FieldMapping("Delete")]
		public bool Delete
		{
			get { return _delete; }
			set { _delete = value; }
		}

		#endregion

		/// <summary>
		///		Creates a new instance of the <see cref="RssFeedItem"/> class
		/// </summary>
		public RssFeedItem(int itemId)
		{
			_id = itemId;	
		}
	}
}
