using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	/// Summary description for LegalNoticeAgreement.
	/// </summary>
	public class LegalNoticeAgreement
	{
		private int _id;
		private int _legalNoticeVersionId;
		private int _userId;
		private bool _agree;
		private DateTime _timestamp;

		private bool _isNew;

		[FieldMapping("Id")]
		public int Id
		{
			get { return _id; }
		}

		[FieldMapping("LegalNoticeId")]
		public int LegalNoticeVersionId
		{
			get { return _legalNoticeVersionId; }
			set { _legalNoticeVersionId = value; }
		}

		[FieldMapping("UserId")]
		public int UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		[FieldMapping("Agree")]
		public bool Agree
		{
			get { return _agree; }
			set { _agree = value; }
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

		public LegalNoticeAgreement()
		{
			_isNew = true;
		}

		public LegalNoticeAgreement(int id)
		{
			_id = id;
			_isNew = false;
		}
	}
}
