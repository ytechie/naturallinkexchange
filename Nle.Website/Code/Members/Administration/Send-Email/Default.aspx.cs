using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web;
using YTech.General.Web.JavaScript;
using System.Web.UI.HtmlControls;
using System.Data;

namespace Nle.Website.Members.Administration.Send_Email
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class SendEmailDefault : Page
	{
		public const string MY_PATH = "Members/Administration/Send-Email/";
		public const string MY_FILE_NAME = "";

        private const string COL_NAME = "Name";
        private const string COL_DISPLAYNAME = "DisplayName";
        private const string COL_DESCRIPTION = "Description";

        private string _termsUrl;
		private string _replaceTerms;

		private int _userId;
		private Database _db;


		private JavaScriptBlock _scripts;

		User _user;

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

		protected void Page_Load(object sender, EventArgs e)
		{
			_userId = Global.GetCurrentUserId();
			_db = Global.GetDbConnection();

			_user = new User(_userId);
			_db.PopulateUser(_user);

            if (_user.AccountType != AccountTypes.Administrator)
                Response.Redirect(Page.ResolveUrl("~/Members/Control-Panel/"));

            tblReplacements.Attributes.Add("class", "Replacements");
            _termsUrl = "http://" + Global.Domain + this.ResolveUrl("~/Terms-Of-Service/");
            _replaceTerms = string.Format("<a href=\"{0}\">Terms of Service</a>", _termsUrl);

            initFilters();
			initJavaScript();
            initStylesheets();
            initEventHandlers();
            initTokenList();
		}

        private void initFilters()
        {
            DataTable dt;
            ListItem li;

            ddlFilters.Items.Add(new ListItem("All Users - No Filter", string.Empty));

            dt = _db.GetEmailFilters();
            foreach (DataRow dr in dt.Rows)
            {
                li = new ListItem();
                li.Text = (string)dr[COL_DISPLAYNAME];
                if (!(dr[COL_DESCRIPTION] is DBNull)) li.Text += " - " + (string)dr[COL_DESCRIPTION];
                li.Value = (string)dr[COL_NAME];
                ddlFilters.Items.Add(li);
            }
        }

		private void initJavaScript()
		{
			_scripts = new JavaScriptBlock();
			_scripts.Defer = true;
			controlPlaceholder.Controls.Add(_scripts);

            if (!Page.IsPostBack)
                JavaScriptBlock.ConfirmClick(cmdSend, "Are you sure you want to send this email to the distribution list selected?");
		}

        private void initStylesheets()
        {
            MainMaster mp = (MainMaster)Page.Master; 
            mp.AddStylesheet("SendEmail.css");
        }

        private void initEventHandlers()
        {
            cmdPreview.Click += new EventHandler(cmdPreview_Click);
            cmdSendPreview.Click += new EventHandler(cmdSendPreview_Click);
            cmdSend.Click += new EventHandler(cmdSend_Click);
            cmdCancel.Click += new EventHandler(cmdCancel_Click);
        }

        private void initTokenList()
        {
            addReplacement("<b>Token</b>", "<b>Description</b>", "<b>Example</b>");
            addReplacement(EmailMessage.TOKEN_REFERRALLINK, "Referral link", string.Format(EmailMessage.FORMAT_REFERRAL_LINK, _user.Id));
            addReplacement(EmailMessage.TOKEN_REFERRALURL, "Referral url", string.Format(EmailMessage.FORMAT_REFERRAL_URL, _user.Id));
            addReplacement(EmailMessage.TOKEN_HOMEPAGELINK, "Home page link", EmailMessage.HOMEPAGE_LINK);
            addReplacement(EmailMessage.TOKEN_HOMEPAGEURL, "Home page url", EmailMessage.HOMEPAGE_URL);
            addReplacement(EmailMessage.TOKEN_TERMSOFSERVICE, "Link to Terms of Service", _replaceTerms);
            addReplacement(EmailMessage.TOKEN_LOGO, "Logo (linked to NaturalLinkExchange.com)", EmailMessage.LOGO);
            addReplacement(EmailMessage.TOKEN_USERSFULLNAME, "User's full name", _user.Name);
            addReplacement(EmailMessage.TOKEN_USERSFIRSTNAME, "User's first name", _user.FirstName);
        }

		private void addReplacement(string token, string value, string example)
		{
			TableRow tr;
			TableCell td_token, td_value, td_example;

			tr = new TableRow();
			td_token = new TableCell();
			td_value = new TableCell();
			td_example = new TableCell();

			td_token.Text = token;
			td_value.Text = value;
			td_example.Text = example;

			tr.Cells.Add(td_token);
			tr.Cells.Add(td_value);
			tr.Cells.Add(td_example);

			tblReplacements.Rows.Add(tr);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
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

        void cmdSendPreview_Click(object sender, EventArgs e)
        {
            EmailMessage newMsg;
            User currUser;
            GlobalSetting gsFromEmail;

            if (!Page.IsValid)
                return;

            currUser = new User(_userId);
            _db.PopulateUser(currUser);

            gsFromEmail = _db.GetGlobalSetting(5);

            newMsg = new EmailMessage();
            newMsg.Message = getFormattedMessage(txtBody.Text);
            newMsg.UserId = currUser.Id;
            newMsg.ToAddress = currUser.EmailAddress;
            newMsg.ToName = currUser.Name;
            newMsg.Html = true;
            newMsg.From = gsFromEmail.TextValue;
            newMsg.Subject = "NLE PREVIEW: " + txtSubject.Text;

            _db.SaveEmail(newMsg);
        }

		private void cmdSend_Click(object sender, EventArgs e)
		{
			if(!Page.IsValid)
				return;

			_db.SendEmailToAllUsers(txtSubject.Text, getFormattedMessage(txtBody.Text), ddlFilters.SelectedValue);
            Response.Redirect(Page.ResolveUrl("~/Members/Control-Panel/"));
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
            Response.Redirect(Page.ResolveUrl("~/Members/Control-Panel/"));
		}

		private string getFormattedMessage(string message)
		{
			EmailMessage msg;

			msg = new EmailMessage();
			msg.Message = message;
			msg.ReplaceInMessageBody(EmailMessage.TOKEN_HOMEPAGELINK, EmailMessage.HOMEPAGE_LINK);
			msg.ReplaceInMessageBody(EmailMessage.TOKEN_HOMEPAGEURL, EmailMessage.HOMEPAGE_URL);
			msg.ReplaceInMessageBody(EmailMessage.TOKEN_LOGO, EmailMessage.LOGO);
			msg.ReplaceInMessageBody(EmailMessage.TOKEN_TERMSOFSERVICE, _replaceTerms);

			return msg.Message;
		}

		private void cmdPreview_Click(object sender, EventArgs e)
		{
			string url, name, features;
			EmailMessage msg;

			msg = new EmailMessage();
			msg.Message = getFormattedMessage(txtBody.Text);
			msg.ApplyEmailTo(_user);

			Session["EMAIL_PREVIEW_SOURCE"] = msg.Message;

            //TODO: Change the hard coded URL to programmatically generated URL.
            url = JavaScriptBlock.SQuote(Global.VirtualDirectory + "Members/Administration/Send-Email/" + "EmailPreview.aspx");
			name = JavaScriptBlock.SQuote("EmailPreview");
			features = JavaScriptBlock.SQuote("location=no,toolbar=no,status=no,menubar=no,width=400,height=500,resizable=yes,scrollbars=yes");

			_scripts.CallFunction("window.open", url, name, features);
		}
	}
}
