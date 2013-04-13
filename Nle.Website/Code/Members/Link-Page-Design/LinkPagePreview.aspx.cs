using System;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using YTech.General.Web;

namespace Nle.Website.Members.Link_Page_Design
{
	/// <summary>
	/// Summary description for LinkPagePreview.
	/// </summary>
	public partial class LinkPagePreview : Page
	{
		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Link-Page-Design/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "LinkPagePreview.aspx";


		protected void Page_Load(object sender, EventArgs e)
		{
			string source = (string)Session["LINK_PAGE_PREVIEW_SOURCE"];
			Debug.WriteLine(source);
			if (source != null) litPage.Text = source;
		}

		public static string GetUrl()
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

			return url.ToString();
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
	}
}
