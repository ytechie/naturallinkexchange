using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	/// Summary description for LinkParagraphGroup.
	/// </summary>
	public class LinkParagraphGroup
	{
		private int _id;

		private int _siteId;
		private string _title;
		private double _distribution;
		private string _ulr1;
		private string _url2;
		private string _anchorText1;
		private string _anchorText2;
        private string _keyword1;
        private string _keyword2;
		private LinkParagraph[] _paragraphs;

		private bool _newLinkParagraphGroup;

		public int Id
		{
			get { return _id; }
		}

		[FieldMapping("Title")]
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		[FieldMapping("Distribution")]
		public double Distribution
		{
			get { return _distribution; }
			set { _distribution = value; }
		}

		[FieldMapping("Url1")]
		public string Url1
		{
			get { return _ulr1; }
			set { _ulr1 = value; }
		}

		[FieldMapping("Url2")]
		public string Url2
		{
			get { return _url2; }
			set { _url2 = value; }
		}

		[FieldMapping("AnchorText1")]
		public string ReplacementText1
		{
            // If an account was upgraded, the replacement text could be empty string, give back the keyword in this case.
			get { return string.IsNullOrEmpty(_anchorText1) ? Keyword1 : _anchorText1; }
			set { _anchorText1 = value; }
		}

		[FieldMapping("AnchorText2")]
		public string ReplacementText2
		{
            // If an account was upgraded, the replacement text could be empty string, give back the keyword in this case.
			get { return string.IsNullOrEmpty(_anchorText2) ? Keyword2 : _anchorText2; }
			set { _anchorText2 = value; }
		}

        /// <summary>
        ///     The text to search for in the articles.
        /// </summary>
        [FieldMapping("Keyword1")]
        public string Keyword1
        {
            get { return _keyword1; }
            set { _keyword1 = value; }
        }

        /// <summary>
        ///     The text to search for in the articles.
        /// </summary>
        [FieldMapping("Keyword2")]
        public string Keyword2
        {
            get { return _keyword2; }
            set { _keyword2 = value; }
        }

		public LinkParagraph[] Paragraphs
		{
			get { return _paragraphs; }
			set { _paragraphs = value; }
		}

		public bool NewLinkParagraphGroup
		{
			get { return _newLinkParagraphGroup; }
		}

		[FieldMapping("SiteId")]
		public int SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		public LinkParagraphGroup()
		{
			_newLinkParagraphGroup = true;
		}

		public LinkParagraphGroup(int id)
		{
			_newLinkParagraphGroup = false;

			_id = id;
		}
	}
}
