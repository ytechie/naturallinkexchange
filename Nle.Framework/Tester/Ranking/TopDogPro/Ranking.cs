using NUnit.Framework;

namespace Nle.Ranking.TopDogPro
{
	[TestFixture()]
	public class Ranking_Tester
	{
		private Ranking r;

		[SetUp()]
		public void Setup()
		{
			r = new Ranking();
		}

		[Test()]
		public void SearchEngine()
		{
			r.SearchEngine = "test engine";
			Assert.AreEqual("test engine", r.SearchEngine);
		}

		[Test()]
		public void SearchString()
		{
			r.SearchString = "test string";
			Assert.AreEqual("test string", r.SearchString);
		}

		[Test()]
		public void Url()
		{
			r.Url = "test url";
			Assert.AreEqual("test url", r.Url);
		}

		[Test()]
		public void Rank()
		{
			r.Rank = 27;
			Assert.AreEqual(27, r.Rank);
		}
	}
}