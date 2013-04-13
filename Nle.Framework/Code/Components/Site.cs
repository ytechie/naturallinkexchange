using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a site that is configured to use the
	///		linking service.
	/// </summary>
	public class Site
	{
		private int _id;
		private string _name;
		private int _userId;
		private string _url;
		private bool _enabled;
		private int _addLinksPercentDays;
		private string _pageTemplate;
		private int _initialCategoryId;
		private int _minLinksToAdd;
		private int _maxLinksToAdd;
		private Guid _siteGuid;
		private bool _createNew = false;
		private int _startLinkPageId;
        private int _upgradeFlag;
        private string _linkPageUrl;
		private bool _hideInitialSetupMessage;
		
		private bool _categorySet = false;

		#region Public Properties

		/// <summary>
		///		Gets the unique identifier for the site.
		/// </summary>
        [FieldMapping("Id")]
		public int Id
		{
			get{ return _id; }
			set{	_id = value;	}
		}

		/// <summary>
		///		The title of the web site
		/// </summary>
		[FieldMapping("Name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		///		Gets the Id of the user that the site belongs to.
		/// </summary>
		[FieldMapping("UserId")]
		public int UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		/// <summary>
		///		The url of the web site
		/// </summary>
		[FieldMapping("Url")]
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		/// <summary>
		///		If true, the site is enabled.  If false, link pages will not
		///		be pushed to them, and they cannot request page updates.
		/// </summary>
		[FieldMapping("Enabled")]
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		///		The percentage of days that backlinks will be added from other sites.
		/// </summary>
		/// <remarks>
		///		This does not affect the update time for the outbound links on this site.
		/// </remarks>
		[FieldMapping("AddLinksPercentDays")]
		public int AddLinksPercentDays
		{
			get { return _addLinksPercentDays; }
			set { _addLinksPercentDays = value; }
		}

		/// <summary>
		///		The template that specifies where our HTML will be inserted.
		///		This is what customers can set so that the pages match the rest
		///		of their site.
		/// </summary>
		[FieldMapping("PageTemplate")]
		public string PageTemplate
		{
			get { return _pageTemplate; }
			set { _pageTemplate = value; }
		}

		/// <summary>
		///		The initial category to display when the user
		///		first displays the links for this site.
		/// </summary>
		[FieldMapping("InitialCategoryId")]
		public int InitialCategoryId
		{
			get
			{
				if(!_categorySet)
					throw new ApplicationException("Cannot read sites category Id before setting it") ;

				return _initialCategoryId;
			}
			set
			{
				_initialCategoryId = value;
				_categorySet = true;
			}
		}

		/// <summary>
		///		If true, the category Id has been populated.
		/// </summary>
		public bool CategoryIdSet
		{
			get
			{
				return _categorySet;
			}
		}

		/// <summary>
		///		The minium number of links to add during
		///		an update cycle.
		/// </summary>
		[FieldMapping("MinLinksToAdd")]
		public int MinLinksToAdd
		{
			get { return _minLinksToAdd; }
			set { _minLinksToAdd = value; }
		}

		/// <summary>
		///		The maximum number of links to add during
		///		an update cycle.
		/// </summary>
		[FieldMapping("MaxLinksToAdd")]
		public int MaxLinksToAdd
		{
			get { return _maxLinksToAdd; }
			set { _maxLinksToAdd = value; }
		}

		/// <summary>
		///		A unique identifier for a site that is
		///		given out as a key to request link pages.
		/// </summary>
		[FieldMapping("SiteGuid")]
		public Guid SiteGuid
		{
			get { return _siteGuid; }
			set { _siteGuid = value; }
		}

		/// <summary>
		///		If true, the site does not represent an existing site in
		///		the database.
		/// </summary>
		public bool CreateNew
		{
			get { return _createNew; }
			set { _createNew = value; }
		}

		/// <summary>
		///		The initial link page that should be displayed for the
		///		site.
		/// </summary>
		[FieldMapping("StartLinkPageId")]
		public int StartLinkPageId
		{
			get{	return _startLinkPageId;	}
			set{	_startLinkPageId = value;	}
		}

        /// <summary>
        ///     If set, the next time the user logs on, they will be presented
        ///     with the account upgrade page for this site.
        /// </summary>
        [FieldMapping("UpgradeFlag")]
        public int UpgradeFlag
        {
            get { return _upgradeFlag; }
            set { _upgradeFlag = value; }
        }

        /// <summary>
        ///     The cached URL of the last known URL for the site's
        ///     link page.
        /// </summary>
        [FieldMapping("LinkPageUrl")]
        public string LinkPageUrl
        {
            get { return _linkPageUrl; }
            set { _linkPageUrl = value; }
        }

		/// <summary>
		///		If true, the inital setup message that gets displayed on users initial link pages will be hidden.
		/// </summary>
		[FieldMapping("HideInitialSetupMessage")]
		public bool HideInitialSetupMessage
		{
			get { return _hideInitialSetupMessage; }
			set { _hideInitialSetupMessage = value; }
		}

		#endregion 

		/// <summary>
		///		Creates a new instance of the <see cref="Site"/> class that
		///		represents the site with the specified ID.
		/// </summary>
		public Site(int siteId)
		{
			_createNew = false;
			_id = siteId;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="Site"/> class.
		/// </summary>
		public Site()
		{
			CreateNew = true;
		}
	}
}
