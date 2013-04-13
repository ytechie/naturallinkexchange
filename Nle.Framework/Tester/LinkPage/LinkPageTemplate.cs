using System;
using NUnit.Framework;
using YTech.General.General.Reflection;

namespace Nle.LinkPage
{
	/// <summary>
	/// Summary description for LinkPageTemplate.
	/// </summary>
	[TestFixture]
	public class LinkPageTemplate_Tester
	{
		private const string TAG_METAKEYWORDS = "<meta name=\"keywords\" content=\"{metaKeywords}\" />";
		private const string TAG_METADESCRIPTION = "<meta name=\"description\" content=\"{metaDescription}\" />";

		[Test]
		public void constr_Test01()
		{
			string before = "<html><head><title>My title</title></head><body></body></html>";

			LinkPageTemplate pt = new LinkPageTemplate(before);
			Assert.IsTrue(pt.SourceCode == before);
		}

		[Test]
		public void constr_Test02()
		{
			string before = "<html><head><title>My title</title></head><body></body></html>";

			LinkPageTemplate pt = new LinkPageTemplate(string.Empty);
			Assert.IsTrue(pt.SourceCode == string.Empty);
			pt.SourceCode = before;
			Assert.IsTrue(pt.SourceCode == before);
		}

		[Test]
		public void InsertNleStuff_Test01()
		{
			string before = "<html><head><title>My title</title></head><body></body></html>";
			string after = string.Format("<html><head>{0}{1}{0}{2}<title>{{title}}</title></head><body></body></html>", Environment.NewLine, TAG_METAKEYWORDS, TAG_METADESCRIPTION);

			LinkPageTemplate pt = new LinkPageTemplate(before);
			pt.InsertNleStuff();
			Assert.IsTrue(pt.SourceCode == after, string.Format("\nSourceCode: {0}\nExpected: {1}", pt.SourceCode, after));
		}

		/// <summary>
		/// <b>Purpose:</b> Test bug in which InsertNleStuff locks up the processor
		/// when attempting to work with the source code from SuperJason.com.
		/// <b>Method:</b>
		/// <ul>
		///		<li>Read in source code from an embedded resource (source code in question
		///		from SuperJason.com has been saved into an embedded resource).</li>
		///		<li>Create a <see cref="LinkPageTemplate"/> object supplying the SuperJason.com
		///		source code.</li>
		///		<li>Execute <see cref="LinkPageTemplate.InsertNleStuff()"/>.</li>
		/// </ul>
		/// <b>Expected Result:</b> <see cref="LinkPageTemplate.InsertNleStuff()"/> should finish relatively quickely.
		/// If not, the test will seem to lock up.
		/// </summary>
		[Test]
		public void InsertNleStuff_Test02()
		{
			string source = EmbeddedFileUtilities.ReadEmbeddedTextFile(this.GetType().Assembly, this.GetType().Namespace, "SuperJason_com_Source.txt");
			LinkPageTemplate pt = new LinkPageTemplate(source);
			pt.InsertNleStuff();
			Assert.IsTrue(true);
		}

		/// <summary>
		/// <b>Purpose:</b> Test to make sure that if any of the Nle stuff exists, <see cref="LinkPageTemplate.InsertNleStuff()"/>
		/// won't re-insert it.
		/// <b>Method:</b>
		/// <ul>
		///		<li>Creae a <see cref="LinkPageTemplate"/> object supplying source code that
		///		already contains some of the Nle stuff.</li>
		///		<li>Execute <see cref="LinkPageTemplate.InsertNleStuff()"/>.</li>
		/// </ul>
		/// <b>Expected Result:</b> <see cref="LinkPageTemplate.InsertNleStuff()"/> should not re-insert the Nle stuff
		/// that already exists.
		/// </summary>
		[Test]
		public void InsertNleStuff_Test03()
		{
			string before = string.Format("<html><head><title>My title</title>{0}</head><body></body></html>", TAG_METADESCRIPTION);
			string after = string.Format("<html><head>{0}{1}<title>{{title}}</title>{2}</head><body></body></html>", Environment.NewLine, TAG_METAKEYWORDS, TAG_METADESCRIPTION);

			LinkPageTemplate pt = new LinkPageTemplate(before);
			pt.InsertNleStuff();
			Assert.IsTrue(pt.SourceCode == after, string.Format("\nSourceCode: {0}\nExpected: {1}", pt.SourceCode, after));
		}

		[Test]
		public void ForceNleTitle_Test01()
		{
			string before = "<html><head><title>My title</title></head><body></body></html>";
			string after = "<html><head><title>{title}</title></head><body></body></html>";
			
			LinkPageTemplate pt = new LinkPageTemplate(before);
			pt.ForceNleTitle();
			Assert.IsTrue(pt.SourceCode == after, string.Format("\nSourceCode: {0}\nExpected: {1}", pt.SourceCode, after));
		}

		[Test]
		public void ForceNleTitle_Test02()
		{
			string before = "<html><head></head><body></body></html>";
			string after = string.Format("<html><head>{0}<title>{{title}}</title></head><body></body></html>", Environment.NewLine);
			
			LinkPageTemplate pt = new LinkPageTemplate(before);
			pt.ForceNleTitle();
			Assert.IsTrue(pt.SourceCode == after, string.Format("\nSourceCode: {0}\nExpected: {1}", pt.SourceCode, after));
		}

		[Test]
		public void InsertStyleSheet_Test01()
		{
			string before = "<html><head><title>My title</title></head><body></body></html>";
			string css_path = "http://www.naturallinkexchange.com/stylesheets/template_css01.css";
			string after = string.Format("<html><head>{0}<link rel=\"stylesheet\" href=\"{1}\" /><title>My title</title></head><body></body></html>", Environment.NewLine, css_path);

			LinkPageTemplate pt = new LinkPageTemplate(before);
			pt.InsertStyleSheet(css_path);
			Assert.IsTrue(pt.SourceCode == after, string.Format("\nSourceCode: {0}\nExpected{1}", pt.SourceCode, after));
		}
	}
}
