using Nle.Client.RssFeeds.Sources;
using NUnit.Framework;

namespace Nle.Client_Tester.RssFeeds.Sources
{
	[TestFixture()]
	public class YahooRssSource_Tester
	{
		[Test()]
		public void RemoveYahooLinkReference()
		{
			string result;

			result = YahooRssSource.RemoveYahooLinkReference("http://us.rd.yahoo.com/dailynews/rss/search/%22United+Parcel+Service%22+OR+USPS/SIG=12ug4e727/*http://www.iii.co.uk/news/?type=afxnews&articleid=5419471&subject=companies&action=article");

			Assert.AreEqual("http://www.iii.co.uk/news/?type=afxnews&articleid=5419471&subject=companies&action=article", result);
		}
	}
}