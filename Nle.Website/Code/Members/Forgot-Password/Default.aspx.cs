using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web;

namespace Nle.Website.Members.Forgot_Password
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class ForgotPasswordDefault : System.Web.UI.Page
	{
		/// <summary>
		///		The path of this page.
		/// </summary>
		public const string MY_PATH = "Members/Forgot-Password/";
		/// <summary>
		///		The file name of this page.
		/// </summary>
		public const string MY_FILE_NAME = "";

        private Database _db;

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Load += new EventHandler(Page_Load);
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            _db = Global.GetDbConnection();
            cmdSend.Click += new EventHandler(cmdSend_Click);
		}

        void cmdSend_Click(object sender, EventArgs e)
        {
            string emailBody;
            EmailMessage msg;
            User u;
            string fromAddress;

            u = _db.GetUserByEmail(txtEmail.Text);

            if(u == null)
            {
                litMessage.Text = "That email address was not found in our system";
                return;
            }
            
            emailBody = _db.GetGlobalSetting(11).TextValue;
            fromAddress = _db.GetGlobalSetting(5).TextValue;

            msg = new EmailMessage();
            msg.Html = true;
            msg.From = fromAddress;
            msg.Message = emailBody;
            msg.Subject = "Your Natural Link Exchange Password";
            msg.ToAddress = u.EmailAddress;
            msg.ToName = u.Name;
            msg.UserId = u.Id;

            _db.SaveEmail(msg);

            litMessage.Text = "Your password should be arriving in an email shortly.";
        }

	}
}
