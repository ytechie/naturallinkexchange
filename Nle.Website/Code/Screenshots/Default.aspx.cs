using System;
using System.Web.UI;

namespace Nle.Website.MoreScreenshots
{
	/// <summary>
	///		The Screenshots page.
	/// </summary>
	public partial class ScreenshotsDefault : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			MainMaster mp = (MainMaster)Page.Master;

            mp.PageKeywords = "screenshots, screenshot";
            mp.PageDescription = "Screenshots of the Natural Links system";
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