using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using YTech.General.DataMapping;
using System.Security.Permissions;

namespace Nle.Website.Common_Controls
{
	/// <summary>
	/// Summary description for DynamicHelpText.
	/// </summary>
    [ToolboxData("<{0}:DynamicHelpText HelpId='0' Title='Sample' Text='Sample Text' />")]
	public sealed class DynamicHelpText : Control
	{
        private string _id;
		private string _title;
		private string _text;
		private int _helpId;

        public new string ID
        {
            get { return _id; }
            set { _id = value; }
        }

		[FieldMapping("Text")]
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		[FieldMapping("Title")]
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		[FieldMapping("Id")]
		public int HelpId
		{
			get { return _helpId; }
			set { _helpId = value; }
		}

		public DynamicHelpText()
		{
		}

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    HtmlInputHidden title, text;
        //    string parentId;

        //    parentId = Parent == null ? string.Empty : Parent.ID;

        //    title = new HtmlInputHidden();
        //    Controls.Add(title);
        //    title.Value = Title;
        //    title.ID = string.Format("Title{0}", HelpId);

        //    text = new HtmlInputHidden();
        //    Controls.Add(text);
        //    text.Value = HttpUtility.HtmlEncode(Text);
        //    text.ID = string.Format("Text{0}", HelpId);

        //    base.Render(writer);
        //}

	}
}
