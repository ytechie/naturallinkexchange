using System;
using System.Web.UI;

namespace Nle.Website.How_Does_This_Work
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class _Default : Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            MainMaster mp = (MainMaster)Page.Master;

            mp.PageKeywords = "link, exchange, naturally, how, work";
            mp.PageDescription = "How the Natural Link Exchange system works.";
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