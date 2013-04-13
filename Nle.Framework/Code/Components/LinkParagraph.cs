using System.Web;
using System.Web.UI.WebControls;
using YTech.General.DataMapping;
using YTech.General.Web.Controls;
using System;
using System.Text.RegularExpressions;

namespace Nle.Components
{
	/// <summary>
	///		Represents a paragraph that is used as a link.
	/// </summary>
	public class LinkParagraph
	{
        const string GROUP_KEYWORD = "Keyword";
        const string GROUP_PRE = "Pre";
        const string GROUP_POST = "Post";

		private int _id;
		private string _urlBase;
        private string _keyword1;
		private string _url1;
		private string _replacementText1;
        private string _keyword2;
		private string _url2;
		private string _replacementText2;
		private string _title;
		private string _paragraph;
		private bool _enabled = false;
		private int _groupId;
        private int _linkCount;

		private bool _newParagraph;

		/// <summary>
		///		Gets the unique identifier of this <see cref="LinkParagraph"/>
		/// </summary>
		public int Id
		{
			get { return _id; }
		}


		/// <summary>
		///		The URL that this link should navigate to.
		/// </summary>
		[FieldMapping("LinkUrl")]
		public string Url1
		{
			get { return _url1; }
			set { _url1 = value; }
		}

		/// <summary>
		///		The display text to use for the URL.
		/// </summary>
		[FieldMapping("LinkText")]
        public string ReplacementText1
		{
            // If an account was upgraded, the replacement text could be empty string, give back the keyword in this case.
			get { return string.IsNullOrEmpty(_replacementText1) ? Keyword1 : _replacementText1; }
			set { _replacementText1 = value; }
		}

        /// <summary>
        ///     The text to search for in the article.
        /// </summary>
        [FieldMapping("Keyword1")]
        public string Keyword1
        {
            get { return _keyword1; }
            set { _keyword1 = value; }
        }

		/// <summary>
		///		The paragraph that is the link body where the
		///		link(s) will eventually go.
		/// </summary>
		[FieldMapping("Paragraph")]
		public string Paragraph
		{
			get { return _paragraph; }
			set { _paragraph = value; }
		}

		/// <summary>
		///		The title of the article that will be displayed above it.
		/// </summary>
		[FieldMapping("Title")]
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		/// <summary>
		///		If true, then the article will be used on link
		///		pages.
		/// </summary>
		[FieldMapping("Enabled")]
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		/// <summary>
		///		Indicates if this link paragraph is new, or is one
		///		based off an exising id.
		/// </summary>
		/// <remarks>
		///		This is used to determine if the paragraph should be
		///		inserted or updated in the database.
		/// </remarks>
		public bool NewParagraph
		{
			get { return _newParagraph; }
		}

		/// <summary>
		/// Gets and sets the Id of the <see cref="LinkParagraphGroup"/> that this Link Paragraph belongs to.
		/// </summary>
		[FieldMapping("LinkParagraphGroupId")]
		public int GroupId
		{
			get { return _groupId; }
			set { _groupId = value; }
		}

		/// <summary>
		/// Gets and sets the secondary URL that this link should navigate to.
		/// </summary>
		[FieldMapping("LinkUrl2")]
		public string Url2
		{
			get { return _url2; }
			set { _url2 = value; }
		}

		/// <summary>
		/// Gets and sets the display text to use for the secondary URL.
		/// </summary>
		[FieldMapping("LinkText2")]
        public string ReplacementText2
		{
            // If an account was upgraded, the replacement text could be empty string, give back the keyword in this case.
			get { return string.IsNullOrEmpty(_replacementText2) ? Keyword2 : _replacementText2; } 
			set { _replacementText2 = value; }
		}

        /// <summary>
        ///     The text to search for in the article.
        /// </summary>
        [FieldMapping("Keyword2")]
        public string Keyword2
        {
            get { return _keyword2; }
            set { _keyword2 = value; }
        }

		/// <summary>
		///		The base website that the URL's should be appended to.
		/// </summary>
		[FieldMapping("UrlBase")]
		public string UrlBase
		{
			get { return _urlBase; }
			set { _urlBase = value; }
		}

        /// <summary>
        ///     The number of keywords that will be replaced in the formatted article.
        /// </summary>
        [FieldMapping("LinkCount")]
        public int LinkCount
        {
            get { return _linkCount; }
            set { _linkCount = value; }
        }

		/// <summary>
		///		Creates a new instance of the <see cref="LinkParagraph"/> that
		///		represents an existing paragraph.
		/// </summary>
		/// <param name="id">
		///		The unique identifier assigned to this <see cref="LinkParagraph"/>
		/// </param>
		public LinkParagraph(int id)
		{
			_id = id;

			//They are loading an existing paragraph
			_newParagraph = false;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="LinkParagraph"/>
		///		that represents a new paragraph.
		/// </summary>
		public LinkParagraph()
		{
			_newParagraph = true;
		}

		/// <summary>
		///		Retrieves the paragraph text with the links inserted.
		///		
		/// </summary>
		/// <returns>
		///		Text that is ready to insert into a <p></p>
		///		on a page.
		///	</returns>
		///	<remarks>
		///		The text is encoded to avoid having the user insert
		///		their own HTML.
		///	</remarks>
		public string GetFormattedParagraph()
        {
            string formattedParagraph;
            string encodedParagraph;
            string urlBase;

            if (_paragraph == null)
                return null;

            encodedParagraph = HttpUtility.HtmlEncode(_paragraph);

            if (_urlBase != null && _urlBase.Length > 0)
            {
                //Guarantee trailing slash on siteUrl
                if (_urlBase.ToCharArray()[_urlBase.Length - 1] != '/')
                    urlBase = _urlBase + '/';
                else
                    urlBase = _urlBase;
            }
            else
            {
                urlBase = _urlBase;
            }

            formattedParagraph = encodedParagraph;
            /// The keywords are first replaced with standard tokens.  This is to help prevent the replacement
            /// of the second keyword over top of the link in the first keywords replacement.
            /// Example: Site = NaturalLinkExchange.com, Keyword1 = Something, Keyword2 = NaturalLinkExchange.com
            /// The URL in the first keyword would be corrupted by the second replacement since
            /// NaturalLinkExchange appears in the URL of the first link.
            formattedParagraph = replaceKeyword(formattedParagraph, Keyword1, "{keyword1}", null, LinkCount, true);
            formattedParagraph = replaceKeyword(formattedParagraph, Keyword2, "{keyword2}", null, LinkCount, true);
            formattedParagraph = replaceKeyword(formattedParagraph, Keyword1, ReplacementText1, null, LinkCount, false);
            formattedParagraph = replaceKeyword(formattedParagraph, Keyword2, ReplacementText2, null, LinkCount, false);
            formattedParagraph = replaceKeyword(formattedParagraph, "{keyword1}", HttpUtility.HtmlEncode(ReplacementText1), urlBase + Url1, LinkCount, true);
            formattedParagraph = replaceKeyword(formattedParagraph, "{keyword2}", HttpUtility.HtmlEncode(ReplacementText2), urlBase + Url2, LinkCount, true);

            return formattedParagraph;
        }

        public void ChangeKeywords(string newKeyword1, string newKeyword2)
        {
            if (newKeyword1 != Keyword1) Paragraph = replaceKeyword(Paragraph, Keyword1, newKeyword1, null, LinkCount, true);
            if (newKeyword2 != Keyword2) Paragraph = replaceKeyword(Paragraph, Keyword2, newKeyword2, null, LinkCount, true);
        }

        private string regexEncode(string value)
        {
            string temp;

            temp = value.Replace(@"{", @"\{");

            return temp;
        }

        private string replaceKeyword(string source, string keyword, string replacementText, string url, int count, bool firstAndLastOnly)
        {  
            string pattern;
            Regex regex;
            MatchCollection matches;
            Match first, last;
            string temp;

            if (count > 2) throw new NotImplementedException(string.Format("Replacement of {0} instances of keyword has not been implemented yet.", count));

            temp = source;
            if (!string.IsNullOrEmpty(keyword))
            {
                pattern = string.Format(@"(?<{0}>^|\W)(?<{1}>{2})(?<{3}>\W|$)", GROUP_PRE, GROUP_KEYWORD, regexEncode(keyword), GROUP_POST);
                regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                matches = regex.Matches(temp);
                if (firstAndLastOnly && matches.Count > 0)
                {
                    first = matches[0];
                    temp = regex.Replace(temp, getReplacementText(first, replacementText, url, keyword), 1);

                    if (count == 2)
                    {
                        matches = regex.Matches(temp);
                        if (matches.Count > 0)
                        {
                            last = matches[matches.Count - 1];
                            temp = regex.Replace(temp, getReplacementText(last, replacementText, url, keyword), 1, last.Index);
                        }
                    }
                }
                else
                {
                    foreach (Match m in matches)
                        temp = regex.Replace(temp, getReplacementText(m, replacementText, url, keyword), 1, m.Index);
                }
            }
            return temp;
        }

        private string getReplacementText(Match m, string replacementText, string url, string keyword)
        {
            HyperLink link = new HyperLink();
            string linkHtml;

            if (string.IsNullOrEmpty(url))
            {
                if (string.IsNullOrEmpty(replacementText))
                    linkHtml = keyword;         // Incomplete keyword definition - can happen on an account upgrade
                else
                    linkHtml = replacementText; // Just switches the keyword to the replacement text
            }
            else
            {
                if (string.IsNullOrEmpty(replacementText))
                {
                    // Incomplete keyword definition - can happen on an account upgrade
                    link.NavigateUrl = url;
                    link.Text = keyword;
                    linkHtml = ControlUtilities.GetControlHtml(link);
                }
                else
                {
                    link.NavigateUrl = url;
                    link.Text = replacementText;
                    linkHtml = ControlUtilities.GetControlHtml(link);
                }
            }

            return m.Groups[GROUP_PRE].Value + linkHtml + m.Groups[GROUP_POST].Value;
        }

		/// <summary>
		/// Replaces the {anchor#} tag in the specified stirng with the specified url and Url and Url display text.
		/// </summary>
		/// <param name="source">The source string to that contains the {anchor#} tag.</param>
		/// <param name="anchorIndex">The index number on the {anchor#} tag.</param>
		/// <param name="url">The Url that the {anchor#} tag should be replaced with.</param>
		/// <param name="urlText">The text that should display on the anchor that will replace the {anchor#} tag.</param>
		/// <returns></returns>
		private string ReplaceAnchor(string source, int anchorIndex, string url, string urlText)
		{
			HyperLink link = new HyperLink();
            string linkHtml;
            string anchorTag;


            if (string.IsNullOrEmpty(url))
            {
                if (string.IsNullOrEmpty(urlText))
                    linkHtml = "";
                else
                    linkHtml = urlText;
            }
            else
            {
                if (string.IsNullOrEmpty(urlText))
                    linkHtml = url;
                else
                {
                    link.NavigateUrl = url;
                    link.Text = urlText;
                    linkHtml = ControlUtilities.GetControlHtml(link);
                }
            }

			anchorTag = string.Format("{{anchor{0}}}", anchorIndex);

			return source.Replace(anchorTag, linkHtml);
		}
	}
}