using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a subscription level for the link
	///		system.
	/// </summary>
	public class LinkPackage
	{
		#region Private Properties

		private int _id;
		private string _friendlyName;
		private int _linkGroups;
		private int _anchorCount;
		private int _articlesPerGroup;
		private double _monthlyPrice;
		private double _yearlyPrice;
		private int _linkPercentDays;
		private int _bans;
		private int _feedUpdateDays;
		private int _minFeedsPerLinkPage;
		private int _maxFeedsPerLinkPage;
		private int _minLinksPerCycle;
		private int _maxLinksPerCycle;
		private double _outInRatio;
        private double _monthlyPriceMultiple;
        private double _yearlyPriceMultiple;

		private bool _idSet = false;

		#endregion

		#region Public Properties

		/// <summary>
		///		This objects unique identifier in the database
		/// </summary>
		[FieldMapping("Id")]
		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				_idSet = true;
			}
		}

		/// <summary>
		///		The friendly name that can be displayed to the user.
		/// </summary>
		[FieldMapping("FriendlyName")]
		public string FriendlyName
		{
			get { return _friendlyName; }
			set { _friendlyName = value; }
		}

		/// <summary>
		///		The number of link groups that the user is allowed to have
		/// </summary>
		[FieldMapping("LinkGroups")]
		public int LinkGroups
		{
			get { return _linkGroups; }
			set { _linkGroups = value; }
		}

		/// <summary>
		///		Gets the text to display for the number of link
		///		groups. This is needed because the value could
		///		be "Unlimited".
		/// </summary>
		public string LinkGroupsDisplayText
		{
			get
			{
				if(_linkGroups >= 999999)
					return "Unlimited";
				else
					return _linkGroups.ToString();
			}
		}

		/// <summary>
		///		The number of hyperlinks that the user can have in their
		///		link paragraphs.
		/// </summary>
		[FieldMapping("AnchorCount")]
		public int AnchorCount
		{
			get { return _anchorCount; }
			set { _anchorCount = value; }
		}

		/// <summary>
		///		The number of articles the user can have in each article group.
		/// </summary>
		[FieldMapping("ArticlesPerGroup")]
		public int ArticlesPerGroup
		{
			get { return _articlesPerGroup; }
			set { _articlesPerGroup = value; }
		}

		/// <summary>
		///		Gets the text to display for the number of articles
		///		per group. This is needed because the value could
		///		be "Unlimited".
		/// </summary>
		public string ArticlesPerGroupDisplayText
		{
			get
			{
				if(_articlesPerGroup >= 999999)
					return "Unlimited";
				else
					return _articlesPerGroup.ToString();
			}
		}

		/// <summary>
		///		The price if the user chooses a month-to-month plan.
		/// </summary>
		[FieldMapping("MonthlyPrice")]
		public double MonthlyPrice
		{
			get { return _monthlyPrice; }
			set { _monthlyPrice = value; }
		}

		/// <summary>
		///		The price if the user pays a year in advance
		/// </summary>
		[FieldMapping("YearlyPrice")]
		public double YearlyPrice
		{
			get { return _yearlyPrice; }
			set { _yearlyPrice = value; }
		}

        /// <summary>
        ///		The price if the user chooses a month-to-month plan, and already
        ///     has another site that is paying full price.
        /// </summary>
        [FieldMapping("MonthlyPriceMultiple")]
        public double MonthlyPriceMultiple
        {
            get { return _monthlyPriceMultiple; }
            set { _monthlyPriceMultiple = value; }
        }

        /// <summary>
        ///		The price if the user pays a year in advance, and already
        ///     has another site that is paying full price.
        /// </summary>
        [FieldMapping("YearlyPriceMultiple")]
        public double YearlyPriceMultiple
        {
            get { return _yearlyPriceMultiple; }
            set { _yearlyPriceMultiple = value; }
        }

		/// <summary>
		///		The maximum percent of days that links will be added.
		/// </summary>
		[FieldMapping("LinkPercentDays")]
		public int LinkPercentDays
		{
			get { return _linkPercentDays; }
			set { _linkPercentDays = value; }
		}

		/// <summary>
		///		The number of competing links that the user
		///		is able to ban.
		/// </summary>
		[FieldMapping("Bans")]
		public int Bans
		{
			get { return _bans; }
			set { _bans = value; }
		}

		/// <summary>
		///		Gets the text to display for the number of bans.
		///		This is needed because the value could
		///		be "Unlimited".
		/// </summary>
		public string BansDisplayText
		{
			get
			{
				if(_bans >= 999999)
					return "Unlimited";
				else
					return _bans.ToString();
			}
		}

		/// <summary>
		///		The minimum number of days between each RSS
		///		feed update.
		/// </summary>
		[FieldMapping("FeedUpdateDays")]
		public int FeedUpdateDays
		{
			get { return _feedUpdateDays; }
			set { _feedUpdateDays = value; }
		}

		/// <summary>
		///		The minimum number of RSS feed items allowed on
		///		each link page.
		/// </summary>
		[FieldMapping("MinFeedsPerLinkPage")]
		public int MinFeedsPerLinkPage
		{
			get { return _minFeedsPerLinkPage; }
			set { _minFeedsPerLinkPage = value; }
		}

		/// <summary>
		///		The maximum number of RSS feed items allowed
		///		on each link page.
		/// </summary>
		[FieldMapping("MaxFeedsPerLinkPage")]
		public int MaxFeedsPerLinkPage
		{
			get { return _maxFeedsPerLinkPage; }
			set { _maxFeedsPerLinkPage = value; }
		}

		/// <summary>
		///		The minimum number of links to add
		///		during an update cycle.
		/// </summary>
		[FieldMapping("MinLinksPerCycle")]
		public int MinLinksPerCycle
		{
			get { return _minLinksPerCycle; }
			set { _minLinksPerCycle = value; }
		}

		/// <summary>
		///		The maximum number of links to add
		///		during an update cycle.
		/// </summary>
		[FieldMapping("MaxLinksPerCycle")]
		public int MaxLinksPerCycle
		{
			get { return _maxLinksPerCycle; }
			set { _maxLinksPerCycle = value; }
		}

		/// <summary>
		///		The ratio of outgoing links to incoming links.
		/// </summary>
		[FieldMapping("OutInRatio")]
		public double OutInRatio
		{
			get { return _outInRatio; }
			set { _outInRatio = value; }
		}

		/// <summary>
		///		Gets the text to display for the
		///		<see cref="OutInRatio"/> property in
		///		a "friendly" format.
		/// </summary>
		public string OutInRatioDisplayText
		{
			get
			{
				if(_outInRatio == 2)
					return "2:1";
				else if(_outInRatio == 1)
					return "1:1";
				else if(_outInRatio == 0.5)
					return "1:2";
				else
					return "???";
			}
		}

		/// <summary>
		///		If true, then this object has had its <see cref="Id"/>
		///		property set.
		/// </summary>
		public bool IdSet
		{
			get { return _idSet; }
		}

		#endregion 

		/// <summary>
		///		Creates a new instance of the <see cref="LinkPackage"/>.
		/// </summary>
		public LinkPackage()
		{
			
		}

		/// <summary>
		///		Creates a new instance of the <see cref="LinkPackage"/>.
		/// </summary>
		/// <param name="id">
		///		The unique identifier assigned to this object
		///		from the database.
		/// </param>
		public LinkPackage(int id)
		{
			_id = id;
			_idSet = true;
		}
	}
}
