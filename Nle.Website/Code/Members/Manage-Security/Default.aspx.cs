using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.Website.Members.Manage_Security
{
	/// <summary>
	///		The security management page.  This is where the users
	///		can change their account password.
	/// </summary>
	public partial class ManageSecurityDefault : Page
	{
		private Database _db;
        private MainMaster _master;

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("Members.css");

			_db = Global.GetDbConnection();

			cmdChangePassword.Click += new EventHandler(cmdChangePassword_Click);
		}

		private void cmdChangePassword_Click(object sender, EventArgs e)
		{
			User u;

			if (!Page.IsValid)
				return;

			u = new User(Global.GetCurrentUserId());
			_db.PopulateUser(u);

			u.Password = txtPassword.Text;

			_db.SaveUser(u);

			lblChangeStatus.Text = "Your password has been changed.";
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
	}
}