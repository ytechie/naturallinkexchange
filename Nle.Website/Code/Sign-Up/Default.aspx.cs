using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Components.ABTesting;
using Nle.Db.SqlServer;
using Nle.Website.Members.Forgot_Password;
using YTech.General.Web.JavaScript;
using YTech.General.Web.Controls.ContentRotation;
using System.Collections.Generic;

namespace Nle.Website.Sign_Up
{
	/// <summary>
	///		The form that people can use to create an account.
	/// </summary>
	public partial class SignUpDefault : Page
	{
		private Database _db;
        private JavaScriptBlock _scripts;

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();
			cmdCreateUser.Click += new EventHandler(cmdCreateUser_Click);

            MainMaster mp = (MainMaster)Page.Master;

            mp.PageKeywords = "exchange, natural, naturally, link, link farm";
            mp.PageDescription = "Exchange links naturally over time through our enhanced link exchange system";
            mp.AddStylesheet("SignUp.css");

            initValidators();

            _scripts = new JavaScriptBlock();
            contentPlaceholder.Controls.Add(_scripts);
		}

        private void initValidators()
        {
            revEmail.ValidationExpression = YTech.General.Email.EmailValidation.EMAIL_REGEX;
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

		/// <summary>
		///		Called when the user has clicked the submit button to
		///		submit their information.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdCreateUser_Click(object sender, EventArgs e)
		{
			User u;
			HttpCookie cookie;
			int referrerId = 0;
			
			//Don't do anything if the page didn't pass validation
			if(!Page.IsValid)
				return;

            if (_db.UserExists(txtEmail.Text))
            {
                pnlUserExists.Visible = true;
                hypForgotPassword.NavigateUrl = Members.Forgot_Password.ForgotPasswordDefault.GetLoadUrl();
                return;
            }
            else
            {
                pnlUserExists.Visible = false;
            }

			//Check to see if the user's computer has a referrer's id
			cookie = Request.Cookies[Global.COOKIE_REFERRAL];
			if(cookie != null && cookie.Value != null && cookie.Value != string.Empty)
				referrerId = int.Parse(cookie.Value);

			//Create a new user, the password will be auto-generated
			u = new User(txtEmail.Text);
			u.Name = txtName.Text;
			u.AccountType = AccountTypes.StandardUser;
			u.ReferrerId = referrerId;
            recordLead(u);
			
			if(!chkAgreeToTerms.Checked)
			{
               _scripts.ShowAlert("You must agree to the terms of service to create an account, please check the terms of service box stating that you agree and then try again.");
				return;
			}
			
			//Add the user to the database
			_db.SaveUser(u);

			//Record that the user agreed to our terms
			saveUserAgreement(u);
			
			//Email the user their password
			emailUserWelcome(u);

            //Save the content that the user saw with the A/B testing
            saveContentKeys(u);

			//Redirect to the signup completed page
			Response.Redirect("Sign-Up-Complete.aspx");
		}

        private void recordLead(User u)
        {
            int leadKey;

            leadKey = LeadTracking.GetLead(Request.Cookies);

            if (leadKey != 0)
                u.LeadId = leadKey;
        }

        private void saveContentKeys(User u)
        {
            Dictionary<string, string> keyPairs;
            ABContent contentShown;
            DateTime serverTime;

            //We're using a try..catch so that errors here to crash the sign-up
            try
            {
                keyPairs = ServerContentRotator.GetSavedContentKeys(Request.Cookies);
                serverTime = _db.GetServerTime();

                foreach(string currRotatorKey in keyPairs.Keys)
                {
                    contentShown = new ABContent();
                    contentShown.RotatorKey = currRotatorKey;
                    contentShown.ContentKey = keyPairs[currRotatorKey];
                    contentShown.Action = Actions.AccountSignUp;
                    contentShown.Timestamp = serverTime;
                    contentShown.UserId = u.Id;

                    _db.SaveABContent(contentShown);
                }
            }
            catch (Exception)
            {
                //Todo: log the exception
            }
        }

		private void saveUserAgreement(User u)
		{
			LegalNoticeAgreement agreement;

			agreement = new LegalNoticeAgreement();
			agreement.Agree = true;
			agreement.UserId = u.Id;
			//Todo: Isn't this ID going to change if we have a new notice version?
			agreement.LegalNoticeVersionId = (int)LegalNotices.TermsOfService;

			_db.SaveLegalNoticeAgreement(agreement);
		}

		/// <summary>
		///		Queues a welcome message for the new user.
		/// </summary>
		private void emailUserWelcome(User u)
		{
			EmailMessage newMsg;
			GlobalSetting gsMessage;
			GlobalSetting gsFromEmail;

			gsMessage = _db.GetGlobalSetting(10);
			gsFromEmail = _db.GetGlobalSetting(5);
			
			newMsg = new EmailMessage();
			newMsg.Message = gsMessage.TextValue;
			newMsg.UserId = u.Id;
			newMsg.ToAddress = u.EmailAddress;
			newMsg.ToName = u.Name;
			newMsg.Html = true;
			newMsg.From = gsFromEmail.TextValue;
			newMsg.Subject = "Welcome To NaturalLinkExchange";

			_db.SaveEmail(newMsg);
		}
	}
}