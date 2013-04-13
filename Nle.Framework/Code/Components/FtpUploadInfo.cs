using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	/// Summary description for FtpUploadInfo.
	/// </summary>
	public class FtpUploadInfo
	{
		private int _id;
		private int _siteId;
		private bool _enabled;
		private string _url;
		private string _userName;
		private string _password;
		private DateTime _lastUpload;
		private DateTime _dailyUploadTime;
		private string _ftpPath;
        private bool _activeMode;

		private bool _createdNew;

		#region Simple Public Properties

		/// <summary>
		///		Gets the unique identifier for this ftp upload information.
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		///		The unique identifier of the site that this ftp information
		///		belongs to.
		/// </summary>
		[FieldMapping("SiteId")]
		public int SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		/// <summary>
		///		Gets or sets whether or not the ftp upload for this
		///		site is enabled or disabled.
		/// </summary>
		[FieldMapping("Enabled")]
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		///		Gets or sets the FTP url for the site.
		/// </summary>
		[FieldMapping("Url")]
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		/// <summary>
		///		Gets or sets the FTP username for this site.
		/// </summary>
		[FieldMapping("UserName")]
		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}

		/// <summary>
		///		Gets or sets the FTP password for this site.
		/// </summary>
		[FieldMapping("Password")]
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		/// <summary>
		///		Gets or sets the time the last upload was successfully
		///		processed for this site.
		/// </summary>
		[FieldMapping("LastUpload")]
		public DateTime LastUpload
		{
			get { return _lastUpload; }
			set { _lastUpload = value; }
		}

		/// <summary>
		///		If true, it means that this is a new set of options.  If false, it mean
		///		that the options were loaded from a specific ID in the database.
		/// </summary>
		public bool CreatedNew
		{
			get { return _createdNew; }
		}

		/// <summary>
		///		Not sure if this will be used.
		/// </summary>
		public DateTime DailyUploadTime
		{
			get { return _dailyUploadTime; }
			set { _dailyUploadTime = value; }
		}

		/// <summary>
		///		The path to CWD to before uploading the files
		///		to the FTP server.
		/// </summary>
		[FieldMapping("FtpPath")]
		public string FtpPath
		{
			get	{	return _ftpPath;	}
			set	{	_ftpPath = value;	}
		}

        /// <summary>
        ///     Gets or sets whether this server requires active
        ///     mode. (non-passive)
        /// </summary>
        [FieldMapping("ActiveMode")]
        public bool ActiveMode
        {
            get { return _activeMode; }
            set { _activeMode = value; }
        }

		#endregion

		/// <summary>
		///		Creates a new instance of the <see cref="FtpUploadInfo"/> class.
		/// </summary>
		/// <param name="ftpUploadId"></param>
		public FtpUploadInfo(int ftpUploadId)
		{
			_id = ftpUploadId;
			_createdNew = false;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="FtpUploadInfo"/> class.
		/// </summary>
		public FtpUploadInfo()
		{
			_id = -1;
			_createdNew = true;
		}
	}
}