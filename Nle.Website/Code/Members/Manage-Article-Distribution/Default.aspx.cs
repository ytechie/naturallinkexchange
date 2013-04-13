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
using Nle.Website.Common_Controls;
using YTech.General.Web.JavaScript;
using YTech.General.Web;

namespace Nle.Website.Members.Manage_Article_Distribution
{
    public partial class ManageArticleDistributionDefault : System.Web.UI.Page
    {
        /// <summary>
        ///		The relative path of this page to the root.
        /// </summary>
        public const string MY_PATH = "Members/Manage-Article-Distribution/";
        /// <summary>
        ///		The file name of the Url for this page.
        /// </summary>
        public const string MY_FILE_NAME = "";

        Database _db;
        Site currSite;
        MainMaster _master;
        StatusHeader _header;
        JavaScriptBlock _javascriptBlock;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            _db = Global.GetDbConnection();

            _master = (MainMaster)Master;
            _header = _master.MasterStatusHeader;

            _master.AddStylesheet("ManageDistributions.css");

            _javascriptBlock = new JavaScriptBlock();
            JavascriptPlaceholder.Controls.Add(_javascriptBlock);

            cmdSave.Click += new EventHandler(cmdSave_Click);

            getSiteInformation();
            displayLinkGroups();

            if (!Page.IsPostBack)
            {
                cmdCancel.Attributes["onclick"] = "if(confirm('Are you sure you want to cancel and lose your changes?')) window.location = '" + Global.VirtualDirectory + "Members/Control-Panel/'; return false;";
            }
        }

        /// <summary>
        ///		Gets the URL that can be used to call this page.
        /// </summary>
        /// <returns></returns>
        public static string GetLoadUrl()
        {
            UrlBuilder url;

            url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

            return url.ToString();
        }

        void cmdSave_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/Members/Control-Panel/"));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            updateTotal();
            base.Render(writer);
        }

        void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/Members/Control-Panel/"));
        }

        private void getSiteInformation()
        {
            int siteId;
            siteId = _header.GetSelectedSiteId();

            if (siteId != -1)
            {
                currSite = new Site(siteId);
                _db.PopulateSite(currSite);
            }
        }

        private void displayLinkGroups()
        {
            LinkParagraphGroup[] groups;
            string clientIds = string.Empty;

            TableRow tr;
            TableCell td;
            Label groupName;
            EditBox distribution;
            Literal percent;
            HyperLink a;
            bool altRow = false;
            Subscription subscription;
            LinkPackage package;

            subscription = _db.GetSiteSubscription(currSite.Id);
            if (subscription == null)
                package = _db.GetLinkPackage(1);
            else
                package = _db.GetLinkPackage(subscription.PlanId);

            if (currSite != null)
            {
                groups = _db.GetSiteLinkParagraphGroups(currSite.Id);

                foreach (LinkParagraphGroup currGroup in groups)
                {
                    tr = new TableRow();
                    Distributions.Rows.Add(tr);
                    if (altRow)
                        tr.CssClass = "DistributionTable_AltRow";
                    else
                        tr.CssClass = "DistributionTable_Row";

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.CssClass = "DistributionTable_Name";
                    groupName = new Label();
                    td.Controls.Add(groupName);
                    groupName.Text = currGroup.Title;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.CssClass = "DistributionTable_Link";
                    a = new HyperLink();
                    td.Controls.Add(a);
                    a.NavigateUrl = currSite.Url + currGroup.Url1;
                    a.Text = currGroup.ReplacementText1;

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.CssClass = "DistributionTable_Link";
                    a = new HyperLink();
                    td.Controls.Add(a);
                    if (package.AnchorCount > 1)
                    {
                        a.NavigateUrl = currSite.Url + currGroup.Url2;
                        a.Text = currGroup.ReplacementText2;
                    }
                    else
                    {
                        a.NavigateUrl = string.Empty;
                        a.Text = "-";
                    }

                    td = new TableCell();
                    tr.Cells.Add(td);
                    td.CssClass = "DistributionTable_Distribution";
                    distribution = new EditBox();
                    td.Controls.Add(distribution);
                    distribution.Tag = currGroup.Id;
                    distribution.Text = currGroup.Distribution.ToString();
                    distribution.MaxLength = 4;
                    distribution.Width = new Unit(50);
                    distribution.TextChanged += new EventHandler(distribution_TextChanged);
                    distribution.ClientOnChange = "displayTotal('" + lblTotal.ClientID + "', editBoxes);";
                    distribution.ClientOnKeyDown = "return IsNumeric(event.keyCode) || IsControl(event.keyCode);";
                    distribution.ClientOnKeyUp = "displayTotal('" + lblTotal.ClientID + "', editBoxes);";
                    clientIds += distribution.ClientID + ";";

                    td = new TableCell();
                    tr.Cells.Add(td);
                    percent = new Literal();
                    td.Controls.Add(percent);
                    percent.Text = "%";

                    altRow = !altRow;
                }

                _javascriptBlock.DeclareVariable("editBoxes", "'" + clientIds + "'");
            }
        }

        void updateTotal()
        {
            LinkParagraphGroup[] groups;
            double total = 0;

            if (currSite != null)
            {
                groups = _db.GetSiteLinkParagraphGroups(currSite.Id);

                foreach (LinkParagraphGroup currGroup in groups)
                    total += currGroup.Distribution;
            }

            lblTotal.Text = total.ToString();
            if (total == 100)
                lblTotal.Style["color"] = "Green";
            else
                lblTotal.Style["color"] = "Red";
        }

        void distribution_TextChanged(object sender, EventArgs e)
        {
            EditBox editbox;
            int groupId;
            LinkParagraphGroup group;

            if (_db == null)
                _db = Global.GetDbConnection();

            editbox = (EditBox)sender;
            groupId = (editbox).Tag;

            group = new LinkParagraphGroup(groupId);
            _db.PopulateLinkParagraphGroup(group);

            if (editbox.Text != string.Empty)
                group.Distribution = double.Parse(editbox.Text);
            else
                group.Distribution = 0;
            _db.SaveLinkParagraphGroup(group);
        }
    }
}