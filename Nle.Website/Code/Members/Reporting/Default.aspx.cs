using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Nle.Db.SqlServer;
using Nle.Components;
using Nle.Website;
using YTech.General.Web;
using System.Text.RegularExpressions;
using Nle.Website.Common_Controls;

public partial class Members_Reporting_Default : System.Web.UI.Page
{
    const string COL_NAME = "Name";
    const string COL_CATEGORY = "ReportCategory";
    const string COL_DISPLAYNAME = "DisplayName";
    const string COL_DESCRIPTION = "Description";
    const string PARAM_NAME = "ReportName";

    public const string MY_PATH = "Members/Reporting/";
    public const string MY_FILE_NAME = "";

    Database _db;
    MainMaster _master;
    StatusHeader _header;

    protected void Page_Load(object sender, EventArgs e)
    {
        _db = Global.GetDbConnection();
        _master = (MainMaster)Page.Master;
        _header = _master.MasterStatusHeader;

        _master.AddStylesheet("Reporting.css");

        loadReports();
        loadRequestedReport();
    }

    public static string GetLoadUrl()
    {
        UrlBuilder url;

        url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

        return url.ToString();
    }

    public static string GetLoadUrl(string reportName)
    {
        UrlBuilder url;

        url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
        url.Parameters.AddParameter(PARAM_NAME, reportName);

        return url.ToString();
    }

    #region Web Form Designer generated code

    protected override void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Load += new EventHandler(this.Page_Load);
    }

    #endregion

    void loadReports()
    {
        User u;
        string name;
        HyperLink link;
        HtmlGenericControl header;
        HtmlGenericControl hr;
        string value;
        string description;

        u = new User(Global.GetCurrentUserId());
        _db.PopulateUser(u);

        foreach(DataTable dt in _db.GetReports(_header.GetSelectedSiteId()).Tables)
        {
            if (dt.Rows.Count > 0)
            {
                //If the table contains a category
                value = getValue(dt.Rows[0], COL_CATEGORY);
                if(value != null)
                {
                    header = new HtmlGenericControl("h1");
                    header.Attributes.Add("class", "ReportList");
                    header.InnerText = value + ":";
                    ReportLinks.Controls.Add(header);
                    hr = new HtmlGenericControl("hr");
                    ReportLinks.Controls.Add(hr);
                }

                //Create a hyperlink for each report
                foreach (DataRow dr in dt.Rows)
                {
                    name = (string)dr[COL_NAME];
                    description = getValue(dr, COL_DESCRIPTION);
                    link = new HyperLink();
                    value = getValue(dr, COL_DISPLAYNAME);
                    link.Text = value == null ? name : value;
                    link.ToolTip = description;
                    link.NavigateUrl = GetLoadUrl(name);
                    link.CssClass = "ReportLink";
                    ReportLinks.Controls.Add(link);
                }
            }
        }
    }

    string getValue(DataRow dr, string valName)
    {
        if (dr.Table.Columns.Contains(valName) && !(dr[valName] is DBNull) && (string)dr[valName] != string.Empty)
            return (string)dr[valName];
        else
            return null;
    }

    void loadRequestedReport()
    {
        if(!string.IsNullOrEmpty(Request.QueryString[PARAM_NAME]))
            report.ReportName = Request.QueryString[PARAM_NAME];
    }
}
