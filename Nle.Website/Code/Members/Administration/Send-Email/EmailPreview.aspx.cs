using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using YTech.General.Web;

namespace Nle.Website.Members.Administration.Send_Email
{
	/// <summary>
	/// Summary description for EmailPreview.
	/// </summary>
	public partial class EmailPreview : System.Web.UI.Page
	{
		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Administration/Send-Email/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "EmailPreview.aspx";


		public static string GetFullUrl()
		{
			UrlBuilder url;

			//url = new UrlBuilder("http://" + Global.Domain + Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

			return url.ToString();
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string source = (string)Session["EMAIL_PREVIEW_SOURCE"];
			Debug.WriteLine(source);
			if (source != null) litPage.Text = source;
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
            this.Load += new EventHandler(Page_Load);
		}
		#endregion
	}
}
