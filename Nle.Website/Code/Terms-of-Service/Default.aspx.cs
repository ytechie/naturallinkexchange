using System;
using System.Web.UI;
using YTech.General.Web;

namespace Nle.Website
{
	/// <summary>
	///		The terms of service page.
	/// </summary>
	public partial class TermsOfServiceDefault : Page
	{
		/// <summary>
		///		The path to this page.
		/// </summary>
		public const string MY_PATH = "Terms-Of-Service/";
		/// <summary>
		///		The file name of this page.
		/// </summary>
		public const string MY_FILE_NAME = "";

		protected void Page_Load(object sender, EventArgs e)
		{
            MainMaster mp = (MainMaster)Page.Master;

            mp.PageKeywords = "terms, service, agreement";
            mp.PageDescription = "Terms of service agreement";
		}
		
		/// <summary>
		///		Gets the URL to load this page.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl()
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

			return url.ToString();
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