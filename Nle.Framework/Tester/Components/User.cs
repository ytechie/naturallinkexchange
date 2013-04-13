using System;
using NUnit.Framework;

namespace Nle.Components
{
	[TestFixture()]
	public class User_Tester
	{
		private User u;

		[SetUp()]
		public void Setup()
		{
			u = new User(10);
		}

		[Test()]
		public void Ctor()
		{
			u = new User("a@b.com");
			Assert.AreEqual("a@b.com", u.EmailAddress);
			Assert.AreEqual(User.RANDOM_PASSWORD_LENGTH, u.Password.Length);
		}

		[Test()]
		public void AccountType()
		{
			u.AccountType = AccountTypes.StandardUser;
			Assert.AreEqual(AccountTypes.StandardUser, u.AccountType);
		}

		[Test()]
		public void AccountType2()
		{
			u.AccountType = AccountTypes.Administrator;
			Assert.AreEqual(AccountTypes.Administrator, u.AccountType);
		}

		[Test()]
		public void AccountTypeValue()
		{
			u.AccountTypeValue = 5;
			Assert.AreEqual(5, u.AccountTypeValue);
		}

		[Test()]
		public void Authenticated()
		{
			u.Authenticated = false;
			Assert.AreEqual(false, u.Authenticated);
		}

		[Test()]
		public void Authenticated2()
		{
			u.Authenticated = true;
			Assert.AreEqual(true, u.Authenticated);
		}

		[Test()]
		public void EmailAddress()
		{
			u.EmailAddress = "test email";
			Assert.AreEqual("test email", u.EmailAddress);
		}

		[Test()]
		public void Enabled()
		{
			u.Enabled = false;
			Assert.AreEqual(false, u.Enabled);
		}

		[Test()]
		public void Enabled2()
		{
			u.Enabled = true;
			Assert.AreEqual(true, u.Enabled);
		}

		[Test()]
		public void Password()
		{
			u.Password = "test pass";
			Assert.AreEqual("test pass", u.Password);
		}

		[Test()]
		public void Name()
		{
			u.Name = "test user";
			Assert.AreEqual("test user", u.Name);
		}

		[Test()]
		public void CreateNew()
		{
			u = new User("test@test.com");
			Assert.AreEqual(true, u.CreateNew);
		}

		[Test()]
		public void CreateNew2()
		{
			u = new User();
			Assert.AreEqual(true, u.CreateNew);
		}

		[Test()]
		public void CreateNew3()
		{
			u = new User(10);
			Assert.AreEqual(false, u.CreateNew);
		}
		
		[Test()]
		public void CreateNew4()
		{
			u = new User("test@test.com", "Test");
			Assert.AreEqual(false, u.CreateNew);
		}


	}
}