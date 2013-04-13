using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using YTech.General.DataMapping;
using System.Web;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Nle.Website.Common_Controls
{
    public class DynamicHelpControlBuilder : ControlBuilder
    /* Class name: DynamicHelpControlBuilder.
     * Defines the functions and data to be used in building custom controls. 
     * This class is referenced using the ControlBuilderAttribute class. See class below.
     */
    {
        public override Type GetChildControlType(String tagName, IDictionary attributes)
        {
            if (String.Compare(tagName, "DynamicHelpItems", true) == 0)
            {
                return typeof(DynamicHelpText);
            }
            return null;
        }
    }

	/// <summary>
	/// Summary description for DynamicHelp.
	/// </summary>
    [ToolboxData("<{0}:DynamicHelp></{0}:DynamicHelp>"),
	 ParseChildren(false),
    ControlBuilderAttribute(typeof(DynamicHelpControlBuilder))]
    public sealed class DynamicHelp : WebControl, INamingContainer
	{
		private ArrayList _items = new ArrayList();
		private string _cssClass;
		private string _helpIds;
		private string _style;
		private bool _hidden = true;

		public new string CssClass
		{
			get { return _cssClass; }
			set { _cssClass = value; }
		}

		public ArrayList DynamicHelpItems
		{
			get { return _items; }
		}

		public string HelpIds
		{
			get { return _helpIds; }
			set { _helpIds = value; }
		}

		public new string Style
		{
			get { return _style; }
			set { _style = value; }
		}

		public bool Hidden
		{
			get { return _hidden; }
			set { _hidden = value; }
		}

        protected override void AddParsedSubObject(Object obj)
        /* Function name: AddParsedSubObject.
         * Updates the array list with the allowed child objects.
         * This function is called during the parsing of the child controls and 
         * after the GetChildControlType function defined in the associated control 
         * builder class.
         */
        {
            if (obj is DynamicHelpText)
            {
                _items.Add(obj);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ///This function manually writes the HTML to avoid .NET's ID naming
            ///convention.  To simplify the use of DynamicHelp, the ID is used
            ///over the ClientId.
            
            HtmlGenericControl h1, hr;

            //Create Container Div
            writer.WriteBeginTag("div");
            writer.WriteAttribute("id", this.ID + "_Container");
            writer.WriteAttribute("style", Hidden ? "display:none" : string.Empty);
            writer.WriteAttribute("class", string.Format("DynamicHelp{0}", _cssClass == null ? string.Empty : " " + _cssClass));
            writer.Write(">");
            writer.WriteLine();

            //Create Title placeholder
            h1 = new HtmlGenericControl("h1");
            //div.Controls.Add(h1);
            h1.ID = this.ID + "_Title";
            h1.RenderControl(writer);
            writer.WriteLine();

            //Create Title-Text divider
            hr = new HtmlGenericControl("hr");
            hr.RenderControl(writer);
            writer.WriteLine();

            //Create Text placeholder
            writer.WriteBeginTag("div");
            writer.WriteAttribute("id", this.ID + "_Text");
            writer.Write(">");
            writer.WriteEndTag("div");  //Content
            writer.WriteLine();

            //Create hidden inputs that hold the dynamic help items
            foreach (DynamicHelpText c in DynamicHelpItems)
            {
                //Create Title hidden input
                writer.WriteBeginTag("input");
                writer.WriteAttribute("type", "hidden");
                writer.WriteAttribute("id", string.Format("{0}_Title{1}", this.ID, c.HelpId));
                writer.WriteAttribute("value", c.Title);
                writer.Write(" />");

                //Create Text hidden input
                writer.WriteBeginTag("input");
                writer.WriteAttribute("type", "hidden");
                writer.WriteAttribute("id", string.Format("{0}_Text{1}", this.ID, c.HelpId));
                writer.WriteAttribute("value", HttpUtility.HtmlEncode(c.Text));
                writer.Write(" />");
                writer.WriteLine();
            }

            writer.WriteEndTag("div");  //Container
        }
    }
}
