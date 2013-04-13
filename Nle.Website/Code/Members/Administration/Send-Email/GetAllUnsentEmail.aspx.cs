using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.Website.Members.Administration.Send_Email
{
	/// <summary>
	///		The page used to display all the unsent email in the email queue.
	/// </summary>
	public partial class GetAllUnsentEmail : Page
	{
		Database _db;

		private void GetAllUnsentEmail_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();

			displayUnsentEmail();

            MainMaster mp = (MainMaster)Page.Master;

            mp.AddStylesheet("SendEmail.css");
		}

		private void displayUnsentEmail()
		{
			DataTable dt;

			dt = _db.GetAllUnsentEmail(new DateTime());
			dgEmailTable.DataSource = dt;
			dgEmailTable.DataBind();
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
			this.Load +=new EventHandler(GetAllUnsentEmail_Load);
		}
		#endregion
	}
}
