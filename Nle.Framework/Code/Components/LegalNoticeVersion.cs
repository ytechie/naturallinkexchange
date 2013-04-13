using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	/// Summary description for LegalNoticeVersion.
	/// </summary>
	public class LegalNoticeVersion
	{
		private int _id;
		private int _legalNoticeId;
		private string _notice;
		private DateTime _timestamp;

		private bool _isNew;

		[FieldMapping("Id")]
		public int Id
		{
			get { return _id; }
		}

		[FieldMapping("LegalNoticeId")]
		public int LegalNoticeId
		{
			get { return _legalNoticeId; }
			set { _legalNoticeId = value; }
		}

		[FieldMapping("Notice")]
		public string Notice
		{
			get { return _notice; }
			set { _notice = value; }
		}

		[FieldMapping("Timestamp")]
		public DateTime Timestamp
		{
			get { return _timestamp; }
            set { _timestamp = value; }
		}

		public bool IsNew
		{
			get { return _isNew; }
		}

		public LegalNoticeVersion()
		{
			_isNew = true;
		}

		public LegalNoticeVersion(int id)
		{
			_id = id;
			_isNew = false;
		}
	}
}
