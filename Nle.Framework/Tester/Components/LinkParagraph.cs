using NUnit.Framework;

namespace Nle.Components
{
	[TestFixture()]
	public class LinkParagraph_Tester
	{
		private LinkParagraph lp;

		[SetUp()]
		public void Setup()
		{
			lp = new LinkParagraph();
            lp.Keyword1 = "{anchor1}";
            lp.Keyword2 = "{anchor2}";
		}

		[Test()]
		public void GetFormattedParagraph()
		{
			const string PARAGRAPH = "My Paragraph";

			lp.UrlBase = "";
			lp.Paragraph = PARAGRAPH;
			Assert.AreEqual(PARAGRAPH, lp.GetFormattedParagraph());
		}

		[Test()]
		public void GetFormattedParagraph2()
		{
			const string PARAGRAPH = "My Paragraph";

			lp.UrlBase = null;
			lp.Paragraph = PARAGRAPH;
			Assert.AreEqual(PARAGRAPH, lp.GetFormattedParagraph());
		}

		[Test()]
		public void GetFormattedParagraph3()
		{
			string p;

			lp.Url1 = "path/";
			lp.ReplacementText1 = "testUrl";
			lp.Paragraph = "My {anchor1} Paragraph";

			lp.UrlBase = "http://base/";
			p = lp.GetFormattedParagraph();

			Assert.AreEqual("My <a href=\"http://base/path/\">testUrl</a> Paragraph", p);
		}

        [Test()]
        public void GetFormattedParagraph4()
        {
            string p;

            lp.Url1 = "";
            lp.ReplacementText1 = "testUrl";
            lp.Paragraph = "My {anchor1} Paragraph";

            lp.UrlBase = "http://www.Test.com";
            p = lp.GetFormattedParagraph();

            Assert.AreEqual("My <a href=\"http://www.Test.com/\">testUrl</a> Paragraph", p);
        }

        [Test()]
        public void GetFormattedParagraph_NoUrl()
        {
            string p;

            lp.UrlBase = "";
            lp.Url1 = "";
            lp.ReplacementText1 = "Test";
            lp.Paragraph = "My {anchor1} Paragraph";
                        
            p = lp.GetFormattedParagraph();

            Assert.AreEqual("My Test Paragraph", p);
        }
	}
}