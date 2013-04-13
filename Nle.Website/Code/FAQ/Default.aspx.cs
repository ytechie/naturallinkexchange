using System;
using System.Web.UI;

namespace Nle.Website.FAQ
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class _Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			MainMaster mp = (MainMaster)Page.Master;

            mp.PageKeywords = "exchange, natural, naturally, link, link farm";
            mp.PageDescription = "Frequently asked questions about the Natural Link Exchange system";
	
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