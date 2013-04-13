using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Nle.Components.Reporting
{
    public class TableReport : INleReport
    {
        const string COL_DESCRIPTION = "ReportLongDescription";
        DataSet _data;
        string _title;
        string _description;

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

        public bool SetData(DataSet data)
        {
            if (data.Tables.Count < 1)
                return false;

            _data = data;

            return true;
        }

        public Control GetControl()
        {
            HtmlGenericControl div;
            HtmlGenericControl title;
            Label description;
            GridView gv;

            div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "TableReport");

            if (_title != null)
            {
                title = new HtmlGenericControl("h1");
                title.InnerText = _title;
                div.Controls.Add(title);
            }

            if(_description != null)
            {
                description = new Label();
                description.Text = _description;
                div.Controls.Add(description);
            }

            if(_data != null)
                foreach (DataTable dt in _data.Tables)
                {
                    gv = new GridView();
                    gv.AllowSorting = true;
                    gv.CssClass = "TableReport_Table";
                    gv.HeaderStyle.CssClass = "TableReport_HeaderStyle";
                    gv.RowStyle.CssClass = "TableReport_ItemStyle";
                    gv.AlternatingRowStyle.CssClass = "TableReport_AlternatingItemStyle";
                    gv.DataSource = dt;
                    gv.DataBind();
                    div.Controls.Add(gv);
                }

            return div;
        }
    }
}
