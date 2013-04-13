using System;
using System.Text;
using YTech.General.DataMapping;
using System.Text.RegularExpressions;

namespace Nle.Components
{
	/// <summary>
	///		Represents a user of the system.
	/// </summary>
	public class User
	{
        public const string REGEX_USERSFIRSTNAME = @"^\w+";

		private int _id;
		private string _emailAddress;
		private string _password;
		private bool _authenticated;
		private int _accountTypeValue = (int) AccountTypes.StandardUser;
		private bool _enabled = true;
		private DateTime _createdOn;
		private string _name;
		private DateTime _lastLogin;
		private int _referrerId;
        private int _leadId;

		private bool _createNew;

		/// <summary>
		///		The length that new randomly generated passwords will be
		/// </summary>
		public const int RANDOM_PASSWORD_LENGTH = 10;

		/// <summary>
		///		Create a new instance of the <see cref="User"/> class.
		/// </summary>
		public User()
		{
			_createNew = true;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="User"/> class
		///		for the user with the specified unique identifier.
		/// </summary>
		/// <param name="userId"></param>
		public User(int userId)
		{
			_createNew = false;
			_id = userId;
		}

		/// <summary>
		///		Creates a user with a random password that is not yet
		///		authenticated.  This would be used when a user is first
		///		created.
		/// </summary>
		/// <param name="emailAddress">
		///		The email address of the new user.
		/// </param>
		public User(string emailAddress)
		{
			_createNew = true;
			_emailAddress = emailAddress;
			_password = GetRandomPassword(RANDOM_PASSWORD_LENGTH);
		}

		/// <summary>
		///		Creates a user that is not yet authenticated.  This
		///		constructor should be used when logging on a <see cref="User"/>
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <param name="password"></param>
		public User(string emailAddress, string password)
		{
			_createNew = false;
			_emailAddress = emailAddress;
			_password = password;
		}

		/// <summary>
		///		Gets or sets the unique identifier for this user.
		/// </summary>
		[FieldMapping("Id")]
		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				_createNew = false;
			}
		}

		/// <summary>
		///		Gets or sets the email address for the user.  The password
		///		is not automatically set if the <see cref="User"/> object 
		///		was created from the user identifier.
		/// </summary>
		[FieldMapping("EmailAddress")]
		public string EmailAddress
		{
			get { return _emailAddress; }
			set { _emailAddress = value; }
		}

		/// <summary>
		///		Gets or sets the password for the user.  The password
		///		is not automatically set if the <see cref="User"/> object
		///		was created from the user Id.
		/// </summary>
		[FieldMapping("Password")]
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		/// <summary>
		///		Gets or sets whether or not the user has been authenticated
		///		using their email address and password.
		/// </summary>
		public bool Authenticated
		{
			get { return _authenticated; }
			set { _authenticated = value; }
		}

		/// <summary>
		///		Gets or sets the account status Id of the user.
		/// </summary>
		[FieldMapping("AccountType")]
		public int AccountTypeValue
		{
			get { return _accountTypeValue; }
			set { _accountTypeValue = value; }
		}

		/// <summary>
		///		Gets or sets the type of the account.
		/// </summary>
		public AccountTypes AccountType
		{
			get { return (AccountTypes) _accountTypeValue; }
			set { _accountTypeValue = (int) value; }
		}

		/// <summary>
		///		Gets or sets a boolean value indicating if the account
		///		is currently enabled.
		/// </summary>
		[FieldMapping("Enabled")]
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		///		Gets or sets the <see cref="DateTime"/> that the
		///		account was created on.
		/// </summary>
		[FieldMapping("CreatedOn")]
		public DateTime CreatedOn
		{
			get { return _createdOn; }
			set { _createdOn = value; }
		}

		/// <summary>
		///		Gets or set the name of the user.
		/// </summary>
		[FieldMapping("Name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		///		Gets or sets the last time that the
		///		user logged on.
		/// </summary>
		[FieldMapping("LastLogin")]
		public DateTime LastLogin
		{
			get { return _lastLogin; }
			set { _lastLogin = value; }
		}

		/// <summary>
		/// Gets or sets the Id of the
		/// User that referred this user.
		/// </summary>
		[FieldMapping("ReferrerId")]
		public int ReferrerId
		{
			get { return _referrerId; }
			set { _referrerId = value; }
		}

        /// <summary>
        ///     Gets or sets the lead that the user
        ///     used when they signed up.
        /// </summary>
        [FieldMapping("LeadId")]
        public int LeadId
        {
            get { return _leadId; }
            set { _leadId = value; }
        }

		/// <summary>
		///		If true, this User was created as a new user and
		///		was not loaded from the database.
		/// </summary>
		public bool CreateNew
		{
			get	{	return _createNew;	}
			set	{	_createNew = value;	}
		}

        /// <summary>
        /// Uses Regular Expressions to extract the first word/name of the user's
        /// full name.
        /// </summary>
        public string FirstName
        {
            get { return Regex.Match(Name, User.REGEX_USERSFIRSTNAME, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture).Value; }
        }

		/// <summary>
		///		Gets a randomly generated password.
		/// </summary>
		/// <param name="passwordLength">
		///		The length that the generated password should be.
		/// </param>
		/// <returns></returns>
		public static string GetRandomPassword(int passwordLength)
		{
			StringBuilder randomString;
			Random randomNumber;
			Char appendedChar; //Holds the generated character

			randomString = new StringBuilder();
			randomNumber = new Random();

			for (int i = 0; i < passwordLength; i++)
			{
				//Generate the char and assign it to appendedChar
				appendedChar = Convert.ToChar(Convert.ToInt32(26*randomNumber.NextDouble()) + 65);

				//Append appendedChar to randomString
				randomString.Append(appendedChar);
			}

			return randomString.ToString();
		}

	}
}