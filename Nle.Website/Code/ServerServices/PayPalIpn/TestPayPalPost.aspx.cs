using System;
using System.Net;
using System.Text;
using System.Web.UI;

namespace Nle.Website.ServerServices.PayPalIpn
{
	/// <summary>
	/// Summary description for TestPayPalPost.
	/// </summary>
	public partial class TestPayPalPost : Page
	{
		private const string POST_STRING = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			string postUrl;

			postUrl = "http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["Server_Port"];
			postUrl += PayPalIpnPage.GetLoadUrl() + "Default.aspx";

			makePost(postUrl, POST_STRING);
		}

		private void makePost(string postUrl, string postString)
		{
			WebClient client = new WebClient();
			client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			byte[] postByteArray = Encoding.ASCII.GetBytes(postString);
			client.UploadData(postUrl, "POST", postByteArray);
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