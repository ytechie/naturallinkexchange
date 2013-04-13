using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web.Controls;
using Nle.Website.Common_Controls;

namespace Nle.Website.Members
{
	/// <summary>
	///		The main page for the members area.
	/// </summary>
	public partial class MemberDefault : Page
	{
		private User _user;
		private Database _db;
        private MainMaster _master;
        private StatusHeader header;

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("Members.css");
            header = _master.MasterStatusHeader;

			_db = Global.GetDbConnection();

			//ButtonUtilities.SetButtonClickDisable(cmdLogin, "Please Wait...");

            if (!Page.IsPostBack)
            {
                checkLogin();
            }

			cmdLogin.Click += new EventHandler(cmdLogin_Click);
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
		///		Checks if the user is already logged in, and if they are,
		///		they are automatically directed to their destination.
		/// </summary>
		private void checkLogin()
		{
            if (Global.GetCurrentUserId() != -1)
            {
                _user = new User(Global.GetCurrentUserId());
                _db.PopulateUser(_user);
                redirectToDestination();
            }
		}

		private void cmdLogin_Click(object sender, EventArgs e)
		{
			HttpCookie authCookie;
            
			_user = new User(txtEmailAddress.Text, txtPassword.Text);

			_db.AuthenticateUser(_user);

            if (!_user.Authenticated)
            {
                lblLoginMessage.Text = "Invalid Email Address/Password Combination";
                return;
            }

            if (!_user.Enabled)
            {
                lblLoginMessage.Text = "Sorry, Your Account Has Been Disabled, Please Contact Support";
                return;
            }

			authCookie = FormsAuthentication.GetAuthCookie(_user.Id.ToString(), false);
			authCookie.Expires = DateTime.Now.AddMonths(1);

			Response.Cookies.Add(authCookie);

			try
			{
				recordLastLoginTime(_user);
			}
			catch(Exception)
			{
				//We don't to bomb if we only failed at recording the last login time.
				throw;
			}
			finally
			{
				redirectToDestination();
			}		
		}

		private void recordLastLoginTime(User u)
		{
				u.LastLogin = _db.GetServerTime();
				_db.SaveUser(u);
		}

		private void redirectToDestination()
		{
			string redirectUrl;
            Site[] sites;

			//Check if there is a return URL in the querystring
			redirectUrl = Request.QueryString["ReturnUrl"];

			if (redirectUrl == null)
                redirectUrl = ResolveUrl("~/Members/Control-Panel/");

            sites = _db.GetUsersSites(_user.Id, false);

            //See if they need to set up a site
            checkSiteSetup(sites);
            
            //See if they need to go to the account upgrade page
            checkUpgradeFlag(sites);

			Response.Redirect(redirectUrl);
		}

        /// <summary>
        ///     Checks if they don't have any sites set up.  If they don't, they
        ///     are directed to the site setup page.
        /// </summary>
        /// <param name="sites"></param>
        private void checkSiteSetup(Site[] sites)
        {
            //Check if they need to set up their sites, and take them to where they need to be
            if (sites.Length == 0)
            {
                Response.Redirect(ResolveUrl("~/Members/Manage-Sites/"));
            }
        }

        /// <summary>
        ///     Checks if any of the users sites have the upgrade flag set.  If any
        ///     of them do, the user is redirected to the account
        ///     upgrade page.
        /// </summary>
        /// <param name="sites">
        ///     The sites to enumerate to check for the upgrade flag.
        /// </param>
        private void checkUpgradeFlag(Site[] sites)
        {
            string redirectUrl;
             
            sites = _db.GetUsersSites(_user.Id, false);

            foreach (Site currSite in sites)
            {
                if (currSite.UpgradeFlag > 0)
                {
                    //Switch to the site that had the flag and send them over to upgrade it
                    header.SetSelectedSiteId(currSite.Id);
                    redirectUrl = string.Format("~/Members/Payment-Settings/Account-Upgrade.aspx?UpgradeFlag={0}", currSite.UpgradeFlag);
                    Response.Redirect(redirectUrl);
                }
            }
        }
	}
}