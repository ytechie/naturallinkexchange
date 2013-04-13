using System;
using System.Text.RegularExpressions;

namespace Nle.LinkPage
{
	/// <summary>
	/// Summary description for LinkPageTemplate.
	/// </summary>
	public class LinkPageTemplate
	{
		private const string REGEX_HEAD = @"[\s\S]*<html>[\s\S]*<head>";
		private const string REGEX_POSTHEAD = @"[\s\S]*</head>[\s\S]*<body>[\s\S]*</body>[\s\S]*</html>";
		private const string REGEX_TITLE = @"<title>[\s\S]*</title>";
		private const string TAG_TITLE = "<title>{title}</title>";
		private const string TAG_STYLESHEET = "<link rel=\"stylesheet\" href=\"{0}\" />";
		private const string TAG_METAKEYWORDS = "<meta name=\"keywords\" content=\"{metaKeywords}\" />";
		private const string TAG_METADESCRIPTION = "<meta name=\"description\" content=\"{metaDescription}\" />";

		private const RegexOptions REGEXOPTIONS = RegexOptions.Compiled | RegexOptions.IgnoreCase;

		private string _source;

		public string SourceCode
		{
			get { return _source; }
			set { _source = value; }
		}

		public LinkPageTemplate(string sourceCode)
		{
			SourceCode = sourceCode;
		}

		/// <summary>
		/// Overrides the <title> tag with the Nle title tag.
		/// </summary>
		public void ForceNleTitle()
		{
			if(this.headContainsTag(REGEX_TITLE))
				replaceTagInHead(REGEX_TITLE, TAG_TITLE);
			else
				insertTagIntoHead(TAG_TITLE);
		}

		/// <summary>
		/// Inserts the Nle Meta Tags and executes <see cref="LinkPageTemplate.ForceNleTitle()"/>
		/// to override the <title> tag with the Nle title tag.
		/// </summary>
		public void InsertNleStuff()
		{
			ForceNleTitle();
			verifyTagExists(TAG_METADESCRIPTION);
			verifyTagExists(TAG_METAKEYWORDS);
		}

		public void InsertStyleSheet(string stylesheetPath)
		{
			string tag = string.Format(TAG_STYLESHEET, stylesheetPath);
			verifyTagExists(tag);
		}

		private void verifyTagExists(string tag)
		{
			if(!headContainsTag(tag)) insertTagIntoHead(tag);
		}

		private string getHeadSection()
		{
			string headEndTag = "</head>";
			int subsourceEnd = _source.ToLower().IndexOf(headEndTag) + headEndTag.Length;
			if(subsourceEnd > 0)
				return _source.Substring(0, subsourceEnd);
			else
				return _source;
		}

		private string getPostHeadSection()
		{
			string headEndTag = "</head>";
			int subsourceEnd = _source.ToLower().IndexOf(headEndTag) + headEndTag.Length;
			if(subsourceEnd > 0)
				return _source.Substring(subsourceEnd);
			else
				return _source;
		}

		private bool headContainsTag(string tag)
		{
			// Have to split for really long source code - regex does not work well with that source.
			Regex regex = new Regex(REGEX_HEAD + @"[\s\S]*" + tag, REGEXOPTIONS);
			return regex.IsMatch(getHeadSection());
		}

		private void insertTagIntoHead(string tag)
		{
			// Have to split for really long source code - regex does not work well with that source.
			string headSection = getHeadSection();
			string postheadSection = getPostHeadSection();

			Regex regex = new Regex(REGEX_HEAD, REGEXOPTIONS);
			Match head = regex.Match(headSection);
			if(head != null) headSection = regex.Replace(headSection, head.Value + Environment.NewLine + tag);

			_source = headSection + postheadSection;
		}

		private void replaceTagInHead(string tag, string replaceWith)
		{
			string headSection = getHeadSection();
			string postheadSection = getPostHeadSection();

			Regex regex = new Regex(tag, REGEXOPTIONS);
			headSection = regex.Replace(headSection, replaceWith);

			_source = headSection + postheadSection;
		}
	}
}
