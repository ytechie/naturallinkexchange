using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using Nle.Website.Common_Controls;
using Nle.Website.Members.Link_Page_Design;
using YTech.General.Web;
using YTech.General.Web.JavaScript;
using log4net;
using System.Reflection;
using System.Web;

namespace Nle.Website.Members.Link_Page_Wizard
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class _Default : Page
	{
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Link-Page-Wizard/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "";
        
		private Database _db;
        private MainMaster _master;
        private StatusHeader header;

		private int page
		{
			get{ return (int)ViewState["Page"]; }
			set{ ViewState["Page"] = value; }
		}

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl()
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

			return url.ToString();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();

            _master = (MainMaster)Master;
            header = _master.MasterStatusHeader;

			header.SiteChanged += new SiteSelector.SiteChangedDelegate(siteSelector_SiteChanged);
			cmdNext.Click += new EventHandler(cmdNext_Click);
			cmdPrevious.Click += new EventHandler(cmdPrevious_Click);
			cmdCancel.Click += new EventHandler(cmdCancel_Click);

            MainMaster mp = (MainMaster)Page.Master;
            mp.AddStylesheet("LinkPageWizard.css");
            mp.AddStylesheet("~/Members/Link-Page-Design/LinkPageDesign.css");
        
			if(!Page.IsPostBack)
			{
				header.FilterIncompleteSites = true;

				initCancelConfirm();
				initWizard();
			}
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

		private void initCancelConfirm()
		{
			JavaScriptBlock.ConfirmClick(cmdCancel, "Are you sure you want to lose your changes and return to the control panel?");
		}

        private void initFinishConfirm()
        {
            JavaScriptBlock.ConfirmClick(cmdNext, "This will overwrite any template that you may have already defined (or the default template if you have not specifically defined a template yet).  Are you sure you want to save?");
        }

		private void hideAllPanels()
		{
			wizardPanel1.Visible = false;
			importPanel.Visible = false;
			predefinedPanel.Visible = false;
			uploadPanel.Visible = false;
		}

		private void initWizard()
		{
			switchToPage1();
		}

		private void switchToPage1()
		{
			hideAllPanels();
			wizardPanel1.Visible = true;

			JavaScriptBlock jsb = new JavaScriptBlock();
			javaScriptPlaceholder.Controls.Add(jsb);
			jsb.Defer = true;
			//jsb.CallFunction("ShowDynamicHelp", "'dynamicHelp'", "1");

			cmdPrevious.Enabled = false;
			cmdNext.Text = "Next";

			page = 1;
		}

		private void switchToPage2()
		{
			hideAllPanels();

			cmdPrevious.Enabled = true;
			cmdNext.Text = "Finish";

			JavaScriptBlock jsb = new JavaScriptBlock();
			javaScriptPlaceholder.Controls.Add(jsb);
			jsb.Defer = true;

			switch(radWizardMode.SelectedValue)
			{
				case("Import"):
					importPanel.Visible = true;
					//jsb.CallFunction("ShowDynamicHelp", "'dynamicHelp'", "2");
					break;
				case("Predefined"):
					predefinedPanel.Visible = true;
					//jsb.CallFunction("ShowDynamicHelp", "'dynamicHelp'", "3");
					break;
				case("Upload"):
					uploadPanel.Visible = true;
					//jsb.CallFunction("ShowDynamicHelp", "'dynamicHelp'", "4");
					break;
				default:
					throw new NotImplementedException(string.Format("{0} is not implemented.", radWizardMode.SelectedValue));
			}

            initFinishConfirm();

			page = 2;
		}

		private string getStylesheetName()
		{
			int stylesheetIndex = int.Parse(ddlStylesheets.SelectedValue);
			if(stylesheetIndex == 0)
				return null;
			else
				return "http://" + Global.Domain + Global.VirtualDirectory + string.Format("Link-Page-Template-Styles/StyleSheet{0}.css", stylesheetIndex);
		}

		private void finish()
		{
			string source = null;
			string stylesheet = null;

			switch(radWizardMode.SelectedValue)
			{
				case("Import"):
					source = readRemoteSite(txtImportUrl.Text);
					break;
				case("Predefined"):
					GlobalSetting gSetting = new GlobalSetting(2);
					_db.PopulateGlobalSetting(gSetting);
					source = gSetting.TextValue;
					stylesheet = getStylesheetName();
					break;
				case("Upload"):
					source = readFile(cmdUpload);
					break;
				default:
					throw new NotImplementedException(string.Format("{0} is not implemented.", radWizardMode.SelectedValue));
			}

			if(source != null)
			{
				LinkPageTemplate pt = new LinkPageTemplate(source);
				pt.InsertNleStuff();

				if(stylesheet != null) pt.InsertStyleSheet(stylesheet);

				Session["LINK_PAGE_TEMPLATE_SOURCE"] = pt.SourceCode;
			}

			if(source != null) redirectToTemplateDesign();
		}

		private void redirectToTemplateDesign()
		{
			string url;

			url = LinkPageDesignDefault.GetLoadUrl(true);
			Response.Redirect(url);			
		}

		private void redirectToControlPanel()
		{
			string url;

			url = ResolveUrl("~/Members/Control-Panel/");
			Response.Redirect(url);			
		}

		private string readFile(HtmlInputFile upload)
		{
			string source;
			StreamReader reader = new StreamReader(upload.PostedFile.InputStream);
			source = reader.ReadToEnd();

			if(source.Length == 0)
			{
				JavaScriptBlock jsb;
				jsb = new JavaScriptBlock();
				javaScriptPlaceholder.Controls.Add(jsb);
				jsb.ShowAlert("The file you specified contained no information.");

				return null;
			}

			return source;
		}

		private string readRemoteSite(string url) 
		{ 
			string correctedUrl;
			WebRequest req;
			WebResponse resp;
			StreamReader sr;
			string html;
			Regex regex = new Regex(@"\w+://.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			JavaScriptBlock jsb;
		 
			if(!regex.IsMatch(url)) correctedUrl = "http://" + url; else correctedUrl = url;
            _log.Debug(string.Format("Attempting to read {0}.", correctedUrl));
			try
			{
				req = WebRequest.Create(correctedUrl);
			}
			catch(System.UriFormatException)
			{
				jsb = new JavaScriptBlock();
				javaScriptPlaceholder.Controls.Add(jsb);
				jsb.ShowAlert(string.Format("The URL you supplied ({0}) was not in the correct format.", url));
				return null;
			}
            try
            {
                resp = req.GetResponse();
            }
            catch (WebException)
            {
                jsb = new JavaScriptBlock();
                javaScriptPlaceholder.Controls.Add(jsb);
                jsb.ShowAlert(string.Format("The page you specified ({0}) could not be found.", url));
                return null;
            }
            catch (HttpException)
            {
                jsb = new JavaScriptBlock();
                javaScriptPlaceholder.Controls.Add(jsb);
                jsb.ShowAlert(string.Format("The page you specified ({0}) could not be found.", url));
                return null;
            }
			sr = new StreamReader(resp.GetResponseStream());
			html = sr.ReadToEnd();
			resp.Close();
			sr.Close();	
	
			return html;
		}

		private void siteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			initWizard();
		}

		private void cmdNext_Click(object sender, EventArgs e)
		{
			switch(page)
			{
				case(1):
					switchToPage2();
					break;
				case(2):
					finish();
					break;
				default:
					throw new NotSupportedException(string.Format("Page {0} is not supported.", page));
			}
		}

		private void cmdPrevious_Click(object sender, EventArgs e)
		{
			switch(page)
			{
				case(2):
					switchToPage1();
					break;
				default:
					throw new NotSupportedException(string.Format("Page {0} is not supported.", page));
			}
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			redirectToControlPanel();
		}
	}
}
