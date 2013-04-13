using System;
using NUnit.Framework;

namespace Nle.Components
{
	/// <summary>
	/// Summary description for EmailMessage.
	/// </summary>
	[TestFixture]
	public class EmailMessage_Tester
	{
		private const int USER_ID = 999;
		private const string USER_NAME = "Test User";
		private const string USER_EMAIL = "test.user@testDomain.com";
		User user;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			user = new User(999);
			user.EmailAddress = USER_EMAIL;
			user.Name = USER_NAME;
		}

		[Test]
		public void MessageNull()
		{
			EmailMessage msg = new EmailMessage();
			msg.ApplyEmailTo(user);
		}

		[Test]
		public void MessageEmptyString()
		{
			EmailMessage msg = new EmailMessage();
			msg.Message = string.Empty;
			msg.ApplyEmailTo(user);
		}

		[Test]
		public void UserNull()
		{
			EmailMessage msg = new EmailMessage();
			msg.ApplyEmailTo(null);
		}

		[Test]
		public void EmailOverride()
		{
			EmailMessage msg = new EmailMessage();
			msg.ToAddress = "fail@whatever.com";
			msg.ApplyEmailTo(user);
			Assert.IsTrue(msg.ToAddress == user.EmailAddress);
		}

		[Test]
		public void ReplaceUsersNameToken()
		{
			EmailMessage msg = new EmailMessage();
			msg.Message = EmailMessage.TOKEN_USERSNAME;
			msg.ApplyEmailTo(user);
			Assert.IsTrue(msg.Message == user.Name);
		}

		[Test]
		public void ReplaceReferralLinkToken()
		{
			EmailMessage msg = new EmailMessage();
			msg.Message = EmailMessage.TOKEN_REFERRALLINK;
			msg.ApplyEmailTo(user);
			Assert.IsTrue(msg.Message == string.Format(EmailMessage.FORMAT_REFERRAL_LINK, user.Id));
		}

		[Test]
		public void ReplaceReferralUrlToken()
		{
			EmailMessage msg = new EmailMessage();
			msg.Message = EmailMessage.TOKEN_REFERRALURL;
			msg.ApplyEmailTo(user);
			
			Assert.IsTrue(msg.Message == string.Format(EmailMessage.FORMAT_REFERRAL_URL, user.Id));
		}

		[Test]
		public void ComplexMessage()
		{
			string testMsg = "Hello {0},\nHow are you today?  You're referral url is {1}.  Here's a link to it: {2}";
			string url = string.Format(EmailMessage.FORMAT_REFERRAL_URL, user.Id);
			string link = string.Format(EmailMessage.FORMAT_REFERRAL_LINK, user.Id);
			
			EmailMessage msg = new EmailMessage();
			msg.Message = string.Format(testMsg, EmailMessage.TOKEN_USERSNAME, EmailMessage.TOKEN_REFERRALURL, EmailMessage.TOKEN_REFERRALLINK);
			msg.ApplyEmailTo(user);
			
			Assert.IsTrue(msg.Message == string.Format(testMsg, user.Name, url, link));
		}

		[Test]
		public void ComplexMessageMultipleReplacements()
		{
			string testMsg = "Hello {0},\nHow are you today?  You're referral url is {1}.  Here's a link to it: {2}  You're name is {0}, and your referral url is {1} and the link is {2}.";
			string url = string.Format(EmailMessage.FORMAT_REFERRAL_URL, user.Id);
			string link = string.Format(EmailMessage.FORMAT_REFERRAL_LINK, user.Id);

			EmailMessage msg = new EmailMessage();
			msg.Message = string.Format(testMsg, EmailMessage.TOKEN_USERSNAME, EmailMessage.TOKEN_REFERRALURL, EmailMessage.TOKEN_REFERRALLINK);
			msg.ApplyEmailTo(user);
			
			Assert.IsTrue(msg.Message == string.Format(testMsg, user.Name, url, link));
		}

		[Test]
		public void CaseInsensitive()
		{
			string testMsg = "Hello {0},\nHow are you today?  You're referral url is {1}.  Here's a link to it: {2}  You're name is {3}, and your referral link is {4} and the link is {5}.";
			string url = string.Format(EmailMessage.FORMAT_REFERRAL_URL, user.Id);
			string link = string.Format(EmailMessage.FORMAT_REFERRAL_LINK, user.Id);
			
			EmailMessage msg = new EmailMessage();
			msg.Message = string.Format(testMsg, EmailMessage.TOKEN_USERSNAME, EmailMessage.TOKEN_REFERRALURL, EmailMessage.TOKEN_REFERRALLINK, EmailMessage.TOKEN_USERSNAME.ToLower(), EmailMessage.TOKEN_REFERRALURL.ToLower(), EmailMessage.TOKEN_REFERRALLINK.ToLower());
			msg.ApplyEmailTo(user);
			
			Assert.IsTrue(msg.Message == string.Format(testMsg, user.Name, url, link, user.Name, url, link));
		}

		[Test]
		public void DoubleBrace()
		{
			string testMsg = "Test {{{0}}} Test";
			EmailMessage msg = new EmailMessage();;
			Assert.IsTrue(string.Format(testMsg, "{USERSNAME}") == "Test {{USERSNAME}} Test");
			msg.Message = string.Format(testMsg, EmailMessage.TOKEN_USERSNAME);
			msg.ApplyEmailTo(user);
			Assert.IsTrue(msg.Message == string.Format(testMsg, user.Name));
		}
	}
}
