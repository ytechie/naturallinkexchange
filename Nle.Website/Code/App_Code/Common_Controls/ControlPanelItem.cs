using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Nle.Website.Common_Controls
{
    /// <summary>
    /// Summary description for ControlPanelItem
    /// </summary>
    public class ControlPanelItem : WebControl
    {
        private bool _enabled;
        private CheckedModes _checkedMode;
        private string _title;
        private string _description;
        private string _navigateUrl;
        private string _toolTip;

        public new bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public CheckedModes CheckedMode
        {
            get { return _checkedMode; }
            set { _checkedMode = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string NavigateUrl
        {
            get { return _navigateUrl; }
            set { _navigateUrl = value; }
        }

        public new string ToolTip
        {
            get { return _toolTip; }
            set { _toolTip = value; }
        }

        public enum CheckedModes
        {
            NoCheckbox,
            Unchecked,
            Checked
        }

        public ControlPanelItem()
        {
            Enabled = true;
            CheckedMode = CheckedModes.NoCheckbox;
        }

        protected override void  Render(HtmlTextWriter writer)
        {
            Panel control;
            Image checkbox;
            HtmlGenericControl title;
            HyperLink a;
            Literal description;

            control = new Panel();
            base.Controls.Add(control);
            control.ToolTip = ToolTip;

            switch (CheckedMode)
            {
                case CheckedModes.Checked:
                    checkbox = new Image();
                    control.Controls.Add(checkbox);
                    checkbox.ImageUrl = Global.VirtualDirectory + "Members/Control-Panel/checked.gif";
                    checkbox.AlternateText = "Checked";
                    break;
                case CheckedModes.Unchecked:
                    checkbox = new Image();
                    control.Controls.Add(checkbox);
                    checkbox.ImageUrl = Global.VirtualDirectory + "Members/Control-Panel/unchecked.gif";
                    checkbox.AlternateText = "Unchecked";
                    break;
            }

            title = new HtmlGenericControl("h5");
            control.Controls.Add(title);

            a = new HyperLink();
            title.Controls.Add(a);
            a.NavigateUrl = NavigateUrl;
            a.Text = Title;

            description = new Literal();
            control.Controls.Add(description);
            description.Text = Description;

            if (!Enabled)
            {
                control.CssClass = "Disabled";
                a.NavigateUrl = string.Empty;
            }

            base.Render(writer);
        }
    }
}