using System;
using Nle.Components;
using Nle.Db;
using Nle.Db.SqlServer;
using NUnit.Framework;

namespace Nle.DatabaseTester
{
	[TestFixture()]
	public class SiteTester
	{
		private Database _db;
		private Site _s;

		[SetUp()]
		public void Setup()
		{
			//hardcode this for now, but eventually read it from a config file
			_db = ConnectionFactory.GetDbConnection("server01", "NLETest", "NleTestUser", "");
		}

		[Test()]
		public void Test1()
		{
			string siteName;
			int siteId;

			siteName = "Test Site " + DateTime.Now.ToString();

			_s = new Site();

			_s.Name = siteName;
			_s.Url = "http://www.Google.com";

			_db.SaveSite(_s);
			siteId = _s.Id;

			_s = null;
			_s = new Site(siteId);
			_db.PopulateSite(_s);

			Assert.AreEqual(siteName, _s.Name);
			Assert.AreEqual("http://www.Google.com", _s.Url);
		}
	}
}
