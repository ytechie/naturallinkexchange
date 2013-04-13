using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock;
using YTech.General.Web;
using YTech.General.General.Reflection;

namespace Nle.LinkPage.Spider
{
    [TestFixture]
    public class SiteSpider_Tester
    {
        SiteSpider ss;
        Mock mockReader;
        IPageContentReader reader;
        string linkPageUrl;

        [SetUp]
        public void Setup()
        {
            mockReader = new DynamicMock(typeof(IPageContentReader));
            reader = (IPageContentReader)mockReader.MockInstance;
        }

        private string getSpiderFile(string fileName)
        {
            return EmbeddedFileUtilities.ReadEmbeddedTextFile(this.GetType().Assembly, this.GetType().Namespace, fileName);
        }

        [Test]
        public void FtpFileCheck()
        {
            mockReader.ExpectAndReturn("GetPageContents", getSpiderFile("SpiderTest1.htm"), "testurl");

            ss = new SiteSpider("testurl", "testfile.htm", reader);
            Assert.AreEqual(false, ss.HasLinkPage(out linkPageUrl));
            
            //Make sure that the page contents were requested
            mockReader.Verify();
        }

        [Test]
        public void FtpFileCheck2()
        {
            mockReader.ExpectAndReturn("GetPageContents", getSpiderFile("SpiderTest2.htm"), "http://www.test.com");

            ss = new SiteSpider("http://www.test.com", "findme.htm", reader);
            Assert.AreEqual(true, ss.HasLinkPage(out linkPageUrl));

            //Make sure that the page contents were requested
            mockReader.Verify();
        }

        [Test]
        public void PhpScriptCheck()
        {
            mockReader.ExpectAndReturn("GetPageContents", getSpiderFile("SpiderTest3.htm"), "http://www.test.com");
            mockReader.ExpectAndReturn("GetPageContents", "move along", string.Format("http://www.test.com/link1.php?Category-Name={0}", SiteSpider.PAGE_CHECK_STRING));
            mockReader.ExpectAndReturn("GetPageContents", SiteSpider.PAGE_CHECK_RESPONSE, string.Format("http://www.test.com/link2.php?Category-Name={0}", SiteSpider.PAGE_CHECK_STRING));

            ss = new SiteSpider("http://www.test.com", GenerateTypes.PhpScript, reader);
            Assert.AreEqual(true, ss.HasLinkPage(out linkPageUrl));

            //Make sure that the page contents were requested
            mockReader.Verify();
        }

        [Test]
        public void PhpScriptCheck2()
        {
            mockReader.ExpectAndReturn("GetPageContents", getSpiderFile("SpiderTest3.htm"), "http://www.test.com");
            mockReader.ExpectAndReturn("GetPageContents", "bad request", string.Format("http://www.test.com/link2.php?Category-Name={0}", SiteSpider.PAGE_CHECK_STRING));

            ss = new SiteSpider("http://www.test.com", GenerateTypes.PhpScript, reader);
            Assert.AreEqual(false, ss.HasLinkPage(out linkPageUrl));

            //Make sure that the page contents were requested
            mockReader.Verify();
        }

        [Test]
        public void AspxScriptCheck()
        {
            mockReader.ExpectAndReturn("GetPageContents", getSpiderFile("SpiderTest3.htm"), "http://www.test.com");
            //Requests that aren't link pages
            mockReader.ExpectAndReturn("GetPageContents", "move along", string.Format("http://www.test.com/link4.aspx?Category-Name={0}", SiteSpider.PAGE_CHECK_STRING));
            mockReader.ExpectAndReturn("GetPageContents", "move along", string.Format("http://www.test.com/link5.aspx?Category-Name={0}", SiteSpider.PAGE_CHECK_STRING));
            mockReader.ExpectAndReturn("GetPageContents", "move along", string.Format("http://www.test.com/link6.aspx?Category-Name={0}", SiteSpider.PAGE_CHECK_STRING));
                        
            ss = new SiteSpider("http://www.test.com", GenerateTypes.NetScript, reader);
            Assert.AreEqual(false, ss.HasLinkPage(out linkPageUrl));

            //Make sure that the page contents were requested
            mockReader.Verify();
        }

        [Test]
        public void MakeLinkAbsolute()
        {
            string result;

            result = SiteSpider.MakeLinkAbsolute("http://www.SuperJason.com", "/test/");
            Assert.AreEqual("http://www.superjason.com/test/", result);
        }

        [Test]
        public void MakeLinkAbsolute2()
        {
            string result;

            result = SiteSpider.MakeLinkAbsolute("http://www.SuperJason.com/test/test2.htm", "../test2/");
            Assert.AreEqual("http://www.superjason.com/test2/", result);
        }

        [Test]
        public void MakeLinkAbsolute3()
        {
            string result;

            result = SiteSpider.MakeLinkAbsolute("http://www.SuperJason.com/test/test2.htm", "http://www.yahoo.com");
            Assert.AreEqual("http://www.yahoo.com/", result);
        }
    }
}
