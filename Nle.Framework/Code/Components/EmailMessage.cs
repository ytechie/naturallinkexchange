using System;
using System.Text.RegularExpressions;
using System.Web.Mail;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents an email message from or for the email queue.
	/// </summary>
	public class EmailMessage
	{
		public const string TOKEN_REFERRALLINK = "{REFERRALLINK}";
		public const string TOKEN_REFERRALURL = "{REFERRALURL}";
		public const string TOKEN_HOMEPAGELINK = "{HOMELINK}";
		public const string TOKEN_HOMEPAGEURL = "{HOMEURL}";
		public const string TOKEN_TERMSOFSERVICE = "{TERMSOFSERVICE}";
		public const string TOKEN_LOGO = "{LOGO}";
		public const string TOKEN_USERSNAME = "{USERSNAME}";
        public const string TOKEN_USERSFULLNAME = "{USERSFULLNAME}";
        public const string TOKEN_USERSFIRSTNAME = "{USERSFIRSTNAME}";
		public const string TOKEN_USERSPASSWORD = "{USERSPASSWORD}";

		protected const string FORMAT_LOGOLINK = "<a href=\"{0}\"><img src=\"{1}\" alt=\"NaturalLinkExchange Logo\" style=\"Border:none\" /></a>";

		public const string HOMEPAGE_URL = "http://www.NaturalLinkExchange.com/";
		public static readonly string HOMEPAGE_LINK = string.Format("<a href=\"{0}\">Natural Link Exchange</a>", HOMEPAGE_URL);
		public static readonly string LOGO = string.Format(FORMAT_LOGOLINK, HOMEPAGE_URL, HOMEPAGE_URL + "Images/MainLogo.gif");
		public const string FORMAT_REFERRAL_URL = HOMEPAGE_URL + "?ReferralId={0}";
		public static readonly string FORMAT_REFERRAL_LINK = string.Format("<a href=\"{0}\">{0}</a>", FORMAT_REFERRAL_URL);

		private int _id;
		private string _from;
		private string _toName;
		private string _toAddress;
		private string _subject;
		private string _message;
		private bool _html;
		private DateTime _sentOn;
		private DateTime _queuedOn;
		private DateTime _lastTry;
		private int _numberOfTries;
		private int _userId;

		private bool _hasId;

		#region Public Properties

		/// <summary>
		///		The unique identifier for this object in
		///		the database.  If this object was not loaded
		///		from the database, an exception will be thrown.
		/// </summary>
		[FieldMapping("Id")]
		public int Id
		{
			get
			{
				if(!_hasId)
					throw new Exception("The ID has not yet been set on this email object.");

				return _id;
			}
			set
			{
				_id = value;
				_hasId = true;
			}
		}

		/// <summary>
		///		The "From" field to use when sending the email.
		/// </summary>
		[FieldMapping("From")]
		public string From
		{
			get { return _from; }
			set { _from = value; }
		}

		/// <summary>
		///		The name of the person receiving the email.  This
		///		can be left NULL if it is not known.
		/// </summary>
		[FieldMapping("ToName")]
		public string ToName
		{
			get { return _toName; }
			set { _toName = value; }
		}

		/// <summary>
		///		The address to send the email to.
		/// </summary>
		[FieldMapping("ToAddress")]
		public string ToAddress
		{
			get { return _toAddress; }
			set { _toAddress = value; }
		}

		/// <summary>
		///		The subject of the email.
		/// </summary>
		[FieldMapping("Subject")]
		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		/// <summary>
		///		If true, the email will be treated as an HTML email.  Only
		///		set this to true if you are using HTML.  Text is generally
		///		accepted better.
		/// </summary>
		[FieldMapping("Html")]
		public bool Html
		{
			get { return _html; }
			set { _html = value; }
		}

		/// <summary>
		///		The <see cref="DateTime"/> that the email was sent, if at all.
		/// </summary>
		[FieldMapping("SentOn")]
		public DateTime SentOn
		{
			get { return _sentOn; }
			set { _sentOn = value; }
		}

		/// <summary>
		///		The <see cref="DateTime"/> that the email was queued, if it
		///		was queued.
		/// </summary>
		[FieldMapping("QueuedOn")]
		public DateTime QueuedOn
		{
			get { return _queuedOn; }
			set { _queuedOn = value; }
		}

		/// <summary>
		///		The <see cref="DateTime"/> that the last email attempt was made.
		/// </summary>
		[FieldMapping("LastTry")]
		public DateTime LastTry
		{
			get { return _lastTry; }
			set { _lastTry = value; }
		}

		/// <summary>
		///		The number of attempts that have been made to send this email.
		/// </summary
		[FieldMapping("NumberOfTries")]
		public int NumberOfTries
		{
			get { return _numberOfTries; }
			set { _numberOfTries = value; }
		}

		/// <summary>
		///		If true, a unique database identifier has been assigned
		///		to this email.
		/// </summary>
		public bool HasId
		{
			get { return _hasId; }
		}

		/// <summary>
		///		The text or HTML that goes in the body of the email
		/// </summary>
		[FieldMapping("Message")]
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		/// <summary>
		/// The user who's intended to receive this message.
		/// </summary>
		[FieldMapping("UserId")]
		public int UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		#endregion

		/// <summary>
		///		Creates a new instance of the <see cref="EmailMessage"/> class.
		/// </summary>
		public EmailMessage()
		{
			_hasId = false;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="EmailMessage"/> class.
		/// </summary>
		public EmailMessage(int id)
		{
			_id = id;
			_hasId = true;
		}

		/// <summary>
		///		Gets a <see cref="MailMessage"/> that contains the values
		///		from this <see cref="EmailMessage"/>.
		/// </summary>
		/// <returns></returns>
        public System.Net.Mail.MailMessage GetMailMessage()
		{
			System.Net.Mail.MailMessage msg;

            msg = new System.Net.Mail.MailMessage();

			//From
			if(_from == null || _from.Length == 0)
				throw new Exception("No From address available");

			msg.From = new System.Net.Mail.MailAddress(_from);

			//To
			if(_toAddress == null || _toAddress.Length == 0)
				throw new Exception("No To address available");

			if(_toName == null || _toName.Length == 0)
				msg.To.Add(_toAddress);
			else
				msg.To.Add(string.Format("<{0}> {1}", _toAddress, _toName));

			//Subject
			msg.Subject = _subject;

			//Format
            msg.IsBodyHtml = _html;

			msg.Body = _message;

			return msg;
		}

		public void ApplyEmailTo(User user)
		{
            Regex regex; 
			if(user != null)
			{
				ToAddress = user.EmailAddress;
				if(Message != null)
				{
					ReplaceInMessageBody(EmailMessage.TOKEN_USERSNAME, user.Name);
                    ReplaceInMessageBody(EmailMessage.TOKEN_USERSFULLNAME, user.Name);
                    ReplaceInMessageBody(EmailMessage.TOKEN_USERSFIRSTNAME, user.FirstName);
					ReplaceInMessageBody(EmailMessage.TOKEN_USERSPASSWORD, user.Password);
					ReplaceInMessageBody(EmailMessage.TOKEN_REFERRALURL, string.Format(FORMAT_REFERRAL_URL, user.Id));
					ReplaceInMessageBody(EmailMessage.TOKEN_REFERRALLINK, string.Format(FORMAT_REFERRAL_LINK, user.Id));
				}
			}
		}

		public void ReplaceInMessageBody(string token, string replacementString)
		{
			if(Message != null) 
                Message = Regex.Replace(Message, token, replacementString == null ? string.Empty : replacementString, RegexOptions.IgnoreCase);
		}
	}
}
