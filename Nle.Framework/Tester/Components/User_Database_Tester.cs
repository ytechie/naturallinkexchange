using System;
using Nle.Db.SqlServer;
using NUnit.Framework;
using YTech.Db.SqlServer;

namespace Nle.Components
{
	/// <summary>
	/// Summary description for User_Database_Tester.
	/// </summary>
	[TestFixture]
	public class User_Database_Tester
	{
		protected const string USERS_TABLE = "Users";

		string noMatch(string field, object value1, object value2)
		{
			return string.Format("The {0} of the two users did not match.  {1} vs. {2}", field, value1, value2);
		}

		void compareUsers(User user1, User user2)
		{
			Assert.IsTrue(user1.AccountType == user2.AccountType, noMatch("account types", user1.AccountType, user2.AccountType));
			Assert.IsTrue(Global.DatesMatch(user1.CreatedOn, user2.CreatedOn), noMatch("created-on date", user1.CreatedOn, user2.CreatedOn));
			Assert.IsTrue(user1.EmailAddress == user2.EmailAddress, noMatch("email address", user1.EmailAddress, user2.EmailAddress));
			Assert.IsTrue(user1.Enabled == user2.Enabled, noMatch("enabled state", user1.Enabled, user2.Enabled));
			Assert.IsTrue(Global.DatesMatch(user1.LastLogin, user2.LastLogin), noMatch("last-login date", user1.LastLogin, user2.LastLogin));
			Assert.IsTrue(user1.Name == user2.Name, noMatch("name", user1.Name, user2.Name));
			Assert.IsTrue(user1.Password == user2.Password, noMatch("password", user1.Password, user2.Password));
			Assert.IsTrue(user1.ReferrerId == user2.ReferrerId, noMatch("referrer id", user1.ReferrerId,  user2.ReferrerId));
		}

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			Global.DisableConstraints();
		}

		[TestFixtureTearDown]
		public void TestFixtureTeardown()
		{
			Global.EnableConstraints();
		}

        //[Test, Explicit(), Ignore("Fails Build - Old")]
        //public void User_Insert01()
        //{
        //    AccountTypes accountTypes = AccountTypes.StandardUser;
        //    DateTime createdOn = DateTime.Now;
        //    string email = User.GetRandomPassword(75) + "@nle.com";			
        //    bool enabled = true;
        //    string name = "Test User";
        //    DateTime lastLogin = DateTime.Now;
        //    string password = "blah";
        //    int referrerId = 0;

        //    AccountTypes accountTypes2 = AccountTypes.Administrator;
        //    DateTime createdOn2 = DateTime.Now;
        //    string email2 = "2" + email;
        //    bool enabled2 = false;
        //    string name2 = "Test User Second";
        //    DateTime lastLogin2 = DateTime.Now;
        //    string password2 = "pwd2";
        //    int referrerId2 = 1;

        //    User created, retrieved;

        //    if(Global.Db.UserExists(email))
        //        Assert.Fail("{0} already exists in the database, can not create.", email);

        //    System.Console.WriteLine("Using first email address {0}.", email);
        //    System.Console.WriteLine("Using second email address {0}.", email2);

        //    //Create the new user and populate his fields
        //    created = new User();
        //    created.Name = name;
        //    created.EmailAddress = email;
        //    created.AccountType = accountTypes;
        //    created.Enabled = enabled;
        //    created.Password = password;
        //    created.ReferrerId = referrerId;
        //    created.CreatedOn = createdOn;
        //    created.LastLogin = lastLogin;

        //    //Save the user to the database
        //    Global.Db.SaveUser(created);

        //    //Make sure that the database class returned the appropriate information
        //    Assert.IsTrue(created.Id > 0, "No Id was set after the user was saved.");
        //    Assert.IsFalse(created.CreateNew, "The user object is still flagged as new after the save.");

        //    //Retrieve the new user from the database into a second object
        //    retrieved = new User(created.Id);
        //    Global.Db.PopulateUser(retrieved);

        //    //Make sure the two objects are the same
        //    compareUsers(created, retrieved);

        //    //Update all the fields in the user
        //    created.Name = name2;
        //    created.EmailAddress = email2;
        //    created.AccountType = accountTypes2;
        //    created.Enabled = enabled2;
        //    created.Password = password2;
        //    created.ReferrerId = referrerId2;
        //    created.CreatedOn = createdOn2;
        //    created.LastLogin = lastLogin2;

        //    //Save the changes to the database
        //    Global.Db.SaveUser(created);

        //    //Retrieve the updated user from the database into a second object
        //    retrieved = new User(created.Id);
        //    Global.Db.PopulateUser(retrieved);

        //    //Make sure the two objects are the same
        //    compareUsers(created, retrieved);
        //}
	}
}
