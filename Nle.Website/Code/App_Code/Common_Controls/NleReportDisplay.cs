using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Nle.Db.SqlServer;
using Nle.Website;
using System.Collections;
using Nle.Components.Reporting;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Nle.Components;

namespace Nle.Website.Common_Controls
{

    public class NleReportDisplayControlBuilder : ControlBuilder
    /* Class name: DynamicHelpControlBuilder.
     * Defines the functions and data to be used in building custom controls. 
     * This class is referenced using the ControlBuilderAttribute class. See class below.
     */
    {
        public override Type GetChildControlType(String tagName, IDictionary attributes)
        {
            if (String.Compare(tagName, "Parameters", true) == 0)
            {
                return typeof(NleReportParameter);
            }
            return null;
        }
    }

    [ToolboxData("<{0}:NleReportParameter></{0}:NleReportParameter>")]
    public class NleReportParameter
    {
        private string _name;
        private object _value;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    /// <summary>
    /// Summary description for NleReport
    /// </summary>
    [ToolboxData("<{0}:NleReport></{0}:NleReport>"),
     ParseChildren(false),
     ControlBuilderAttribute(typeof(NleReportDisplayControlBuilder))]
    public class NleReportDisplay : WebControl, INamingContainer
    {
        private const string COL_TYPE = "ReportType";
        private const string COL_SUBSCRIPTION_LEVEL = "SubscriptionLevel";
        private const string COL_DESCRIPTION = "ReportLongDescription";
        private const string COL_TITLE = "Title";
        private const string PATTERN_ADMIN_REPORT = "Report_.*";

        private ArrayList _items = new ArrayList();
        string _report_name;

        public string ReportName
        {
            get { return _report_name; }
            set { _report_name = value; }
        }

        public NleReportDisplay()
        {

        }

        protected override void AddParsedSubObject(Object obj)
        /* Function name: AddParsedSubObject.
         * Updates the array list with the allowed child objects.
         * This function is called during the parsing of the child controls and 
         * after the GetChildControlType function defined in the associated control 
         * builder class.
         */
        {
            if (obj is NleReportParameter)
            {
                _items.Add(obj);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string planId;
            string description;
            int siteId;

            Database db;
            DataSet data;
            Subscription subscription;
            User user;

            //If no report, nothing to render
            if (string.IsNullOrEmpty(_report_name))
                return;

            db = Global.GetDbConnection();
            siteId = Global.GetCurrentSiteId();
            user = new User(Global.GetCurrentUserId());
            db.PopulateUser(user);

            data = db.GetReport(_report_name);
            planId = getValue(data.Tables[0].Rows[0], COL_SUBSCRIPTION_LEVEL);
            description = getValue(data.Tables[0].Rows[0], COL_DESCRIPTION);

            if (siteId != -1)
            {
                subscription = db.GetSiteSubscription(siteId);
                //If the current user is an Administrator, no subscription is specified for the report or the current site's subscription
                //is greater than or equal to the subscription level of the report.
                if (user.AccountType == AccountTypes.Administrator ||
                   (isUserReport(_report_name) && subscription != null && planId != null && int.Parse(planId) <= subscription.PlanId))
                    displayReports(writer, getReportData(_report_name, _items));
                else
                    displayUnauthorized(writer);
            }
            else
                displayUnauthorized(writer);
        }

        void displayUnauthorized(HtmlTextWriter writer)
        {
            HtmlGenericControl h1;
            h1 = new HtmlGenericControl("h1");
            h1.Attributes.Add("class", "Unauthorized");
            h1.InnerText = "You are not authorized to view this report.";
            h1.RenderControl(writer);
        }

        void displayReports(HtmlTextWriter writer, DataSet data)
        {
            INleReport report = null;
            DataTable dt;
            DataSet subset = null;

            for(int i = 0; i < data.Tables.Count; i = 0)
            {
                dt = data.Tables[0];

                //If table is report definition table
                if (dt.Columns.Contains(COL_TYPE))
                {
                    addReport(report, subset, writer);

                    //Prepare a new subset and report
                    subset = new DataSet();
                    report = initReport(dt);
                    if (report == null)
                    {
                        displayBadConfig(writer);
                        return;
                    }
                    data.Tables.Remove(dt);
                }
                else
                {
                    //If here and subset has not been initialized, a report definition table was missing - bad config
                    if (subset == null)
                    {
                        displayBadConfig(writer);
                        return;
                    }

                    //Move table to subset
                    data.Tables.Remove(dt);
                    subset.Tables.Add(dt);
                }
            }
            addReport(report, subset, writer);
        }

        void addReport(INleReport report, DataSet data, HtmlTextWriter writer)
        {
            //If subset contains tables, set the report's data and add to page
            if (report != null && data != null)
            {
                report.SetData(data);
                report.GetControl().RenderControl(writer);
            }
        }

        void displayBadConfig(HtmlTextWriter writer)
        {
            Label badConfig;

            badConfig = new Label();
            badConfig.Text = string.Format("The data returned from {0} was not in the correct format.", _report_name);
            badConfig.RenderControl(writer);
        }

        INleReport initReport(DataTable dt)
        {
            DataRow reportProperties;
            Type t;
            string type;
            string title;
            string description;
            INleReport report;

            if (dt.Rows.Count == 0)
                return null;

            reportProperties = dt.Rows[0];
            type = getValue(reportProperties, COL_TYPE);

            if (type == null)
                return null;

            t = Type.GetType(type);
            if (type == null)
                return null;

            title = getValue(reportProperties, COL_TITLE);
            description = getValue(reportProperties, COL_DESCRIPTION);

            report = (INleReport)Activator.CreateInstance(t);
            report.Title = title;
            report.Description = description;

            return report;
        }

        static string getValue(DataRow dr, string name)
        {
            if (dr.Table.Columns.Contains(name) && !(dr[name] is DBNull) && (string)dr[name] != string.Empty)
                return (string)dr[name];
            else
                return null;
        }

        static bool isUserReport(string reportName)
        {
            return Regex.IsMatch(reportName, "UserReport_.*", RegexOptions.IgnoreCase);
        }

        static DataSet getReportData(string reportName, ArrayList parameters)
        {
            Database db;
            ArrayList sqlParameters;
            SqlParameter sqlParam;
            bool userReport, userIdAdded;
            string paramName;
            
            db = Global.GetDbConnection();

            userReport = isUserReport(reportName);
            userIdAdded = false;

            sqlParameters = new ArrayList();
            foreach (NleReportParameter param in parameters)
            {
                paramName = param.Name;
                if(paramName.Substring(0, 1) != "@") paramName = "@" + paramName;
                if (userReport && paramName.ToLower() == "@userid") userIdAdded = true;
                sqlParam = new SqlParameter(param.Name, param.Value);
                sqlParameters.Add(sqlParam);
            }
            if (userReport && !userIdAdded)
                sqlParameters.Add(new SqlParameter("@UserId", Global.GetCurrentUserId()));

            return db.ExecuteReport(reportName, (SqlParameter[])sqlParameters.ToArray(typeof(SqlParameter)));
        }
    }
}