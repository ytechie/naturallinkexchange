using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.JavaScript;
using YTech.Web.Ftp;

namespace Nle.Website.Members.Link_Page_Setup
{
	/// <summary>
	///		The page for setting up your link page generation settings.
	/// </summary>
	public partial class LinkPageSetupDefault : Page
	{
		/// <summary>
		///		The relative path of this page to the root.
		/// </summary>
		public const string MY_PATH = "Members/Link-Page-Setup/";
		/// <summary>
		///		The file name of the Url for this page.
		/// </summary>
		public const string MY_FILE_NAME = "";

		private Database _db;
		private int _userId;
		private int _selectedSiteId;
		private JavaScriptBlock _scripts;
        private StatusHeader _status;
        private MainMaster _master;
      
		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

            _master = ((MainMaster)this.Master);
            _status = _master.MasterStatusHeader;

			initJavaScript();

            _selectedSiteId = _status.GetSelectedSiteId();
			if (_selectedSiteId == -1)
				throw new NotImplementedException("No sites have been configured for this user.");

			chkHideSetupMessage.CheckedChanged += new EventHandler(chkHideSetupMessage_CheckedChanged);

			rdoFtpUpload.CheckedChanged += new EventHandler(rdoSectionSelection_CheckedChanged);
			rdoPhpScript.CheckedChanged += new EventHandler(rdoSectionSelection_CheckedChanged);
			rdoNetScript.CheckedChanged += new EventHandler(rdoSectionSelection_CheckedChanged);
			rdoNetControlScript.CheckedChanged += new EventHandler(rdoSectionSelection_CheckedChanged);

			_status.SiteChanged += new SiteSelector.SiteChangedDelegate(ssSiteSelector_SiteChanged);

			//Add handlers for the FTP section
			cmdGenerateAndFtp.Click += new EventHandler(cmdGenerateAndFtp_Click);
			cmdSaveFtpInfo.Click += new EventHandler(cmdSaveFtpInfo_Click);

			if(!Page.IsPostBack)
			{
				_status.FilterIncompleteSites = true;
				initialSiteLoad();
			}
		}

		void chkHideSetupMessage_CheckedChanged(object sender, EventArgs e)
		{
			Site s;
			lblHideSetupMsg.Visible = true;

			s = new Site(_selectedSiteId);
			_db.PopulateSite(s);

			s.HideInitialSetupMessage = chkHideSetupMessage.Checked;
			_db.SaveSite(s);
		}

		private void initialSiteLoad()
		{
			GenerateTypes generateType;
			Site s;

			//Load the users method of generating the link pages
			generateType = (GenerateTypes)_db.GetIntSiteSetting(_selectedSiteId, 6);
			setSection(generateType);

			//Load the users setting for their link page setup message
			s = new Site(_selectedSiteId);
			_db.PopulateSite(s);
			chkHideSetupMessage.Checked = s.HideInitialSetupMessage;
		}

		/// <summary>
		///		Shows the specified section
		/// </summary>
		/// <param name="generateType"></param>
		private void setSection(GenerateTypes generateType)
		{
			//Note: even when radio buttons are in a group, you need to
			//explicitly uncheck them when checking something else.

			uncheckAllGenerateTypes();

			switch(generateType)
			{
				case GenerateTypes.FtpUpload:
					rdoFtpUpload.Checked = true;
					break;
				case GenerateTypes.PhpScript:
					rdoPhpScript.Checked = true;
					break;
				case GenerateTypes.NetScript:
					rdoNetScript.Checked = true;
					break;
				case GenerateTypes.NetControlScript:
					rdoNetControlScript.Checked = true;
					break;
				default:
                    rdoPhpScript.Checked = true;
                    break;
			}

			//Now updated the section based on the selection
			rdoSectionSelection_CheckedChanged(null, null);
		}

		private void uncheckAllGenerateTypes()
		{
			rdoFtpUpload.Checked = false;
			rdoPhpScript.Checked = false;
			rdoNetScript.Checked = false;
			rdoNetControlScript.Checked = false;
		}

		private void hideAllGenerateSectionPanels()
		{
			pnlFtpSettings.Visible = false;
			pnlPhpSettings.Visible = false;
			pnlNetSettings.Visible = false;			
		}

		private void rdoSectionSelection_CheckedChanged(object s, EventArgs e)
		{
			hideAllGenerateSectionPanels();

			if(rdoFtpUpload.Checked)
			{
				showFtpSection();
				_db.SaveIntSiteSetting(_selectedSiteId, 6, (int)GenerateTypes.FtpUpload);
			}
			else if(rdoPhpScript.Checked)
			{
				showPhpSection();
				_db.SaveIntSiteSetting(_selectedSiteId, 6, (int)GenerateTypes.PhpScript);
			}
			else if(rdoNetScript.Checked)
			{
				showNetSection();
				_db.SaveIntSiteSetting(_selectedSiteId, 6, (int)GenerateTypes.NetScript);
			}
			else
				throw new NotImplementedException("This selection is not yet implemented.");
		}

		private void initJavaScript()
		{
			_scripts = new JavaScriptBlock();
			javaScriptPlaceholder.Controls.Add(_scripts);
		}

		private void showFtpSection()
		{
			string saveConfirmMsg;

			pnlFtpSettings.Visible = true;

			//Don't waste time populating this if it's not slected
			if(!rdoFtpUpload.Checked)
				return;

			if(cmdSaveFtpInfo.Attributes["onclick"] == null)
			{
				saveConfirmMsg = "Warning! It may take a few moments to test and save the FTP settings!";

				//Confirm saving the FTP info since it can take a while to test the connection
				JavaScriptBlock.ConfirmClick(cmdSaveFtpInfo, saveConfirmMsg);
			}

			displayInitialPageName();
			initFtpSectionSiteInfo();
		}

		private void displayInitialPageName()
		{
			Nle.Components.Site s;
			Nle.Components.LinkPage lp;

			s = new Site(_selectedSiteId);
			_db.PopulateSite(s);

			if(s.StartLinkPageId == 0)
			{
				lblFtpInitialName.Text = "Not Yet Set";
			}
			else
			{
				lp = new Nle.Components.LinkPage(s.StartLinkPageId);
				_db.PopulateLinkPage(lp);

				lblFtpInitialName.Text = System.Web.HttpUtility.HtmlEncode(lp.PageName) + ".html";
			}
		}

		private void showPhpSection()
		{
			Site s;

			pnlPhpSettings.Visible = true;
			
			s = loadCurrentSiteInfo();

			//Set up the script link
			lnkPhpScript.NavigateUrl = Download_Link_Page_Script.GetLoadUrl(s.SiteGuid.ToString(), Download_Link_Page_Script.ScriptTypes.Php);
		}

		private Site loadCurrentSiteInfo()
		{
			Site s;

			s = new Site(_selectedSiteId);
			_db.PopulateSite(s);

			return s;
		}

		private void showNetSection()
		{
			Site s;

			pnlNetSettings.Visible = true;

			s = loadCurrentSiteInfo();

			lnkNetScript.NavigateUrl = Download_Link_Page_Script.GetLoadUrl(s.SiteGuid.ToString(), Download_Link_Page_Script.ScriptTypes.Net);
		}

		private void initFtpSectionSiteInfo()
		{
			FtpUploadInfo ftpInfo;

			ftpInfo = getFtpInfo();

			txtFtpServer.Text = ftpInfo.Url;
			txtFtpPath.Text = ftpInfo.FtpPath;
			txtFtpUserName.Text = ftpInfo.UserName;
			txtFtpPassword.Attributes["Value"] = ftpInfo.Password;
            chkActiveMode.Checked = ftpInfo.ActiveMode;

			if(ftpInfo.LastUpload.Ticks == new DateTime().Ticks)
				lblLastUpload.Text = "Never";
			else
				lblLastUpload.Text = ftpInfo.LastUpload.ToLocalTime().ToString();
		}

		private FtpUploadInfo getFtpInfo()
		{
			FtpUploadInfo ftpInfo;

			ftpInfo = _db.GetFtpUploadInfo(_selectedSiteId);
			if(ftpInfo == null)
			{
				ftpInfo = new FtpUploadInfo();
				ftpInfo.SiteId = _selectedSiteId;
				ftpInfo.Enabled = true;
			}

			return ftpInfo;
		}

		private void ssSiteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			//Update everything that is site dependent
			initialSiteLoad();
		}

		private void cmdGenerateAndFtp_Click(object sender, EventArgs e)
		{
			Uploader u;
			FtpUploadInfo ftpInfo;
            string errMsg;

			ftpInfo = getFtpInfo();

            try
            {
                u = new Uploader(_db, ftpInfo);
            }
            catch (EnterpriseDT.Net.Ftp.FTPException ftpEx)
            {
                errMsg = string.Format("There was an error uploading your files, please make sure that you entered in valid information.  Error: {0}", ftpEx.Message);
                _scripts.ShowAlert(errMsg);
                return;
            }

			//Reload the ftp details so that the last update time changes
			initFtpSectionSiteInfo();

			_scripts.ShowAlert("Upload Complete, Please Manually Verify That They Uploaded Correctly");
		}

		private void cmdSaveFtpInfo_Click(object sender, EventArgs e)
		{
            FtpUploadInfo testInfo;
			bool testResult;
			string msg;
			FtpUploadInfo ftpInfo;

            testInfo = new FtpUploadInfo();

			testInfo.Url = txtFtpServer.Text;
			testInfo.FtpPath = txtFtpPath.Text;
			testInfo.UserName = txtFtpUserName.Text;
			testInfo.Password = txtFtpPassword.Text;
            testInfo.ActiveMode = chkActiveMode.Checked;

            testResult = testFtpInfo(testInfo, out msg);

			if (!testResult)
			{
				_scripts.ShowAlert("FTP Connection test failed, your settings will NOT be saved. Message: " + msg);
				return;
			}

			ftpInfo = getFtpInfo();

            ftpInfo.Url = testInfo.Url;
            ftpInfo.FtpPath = testInfo.FtpPath;
            ftpInfo.UserName = testInfo.UserName;
            ftpInfo.Password = testInfo.Password;
            ftpInfo.ActiveMode = testInfo.ActiveMode;
			
			//The connection is valid, save the info
			_db.SaveFtpUploadInfo(ftpInfo);

			_scripts.ShowAlert("The FTP settings were verified, and they have been saved successfully.");
		}

		/// <summary>
		///		Tests the credentials to see if they are valid.
		/// </summary>
		/// <param name="server"></param>
		/// <param name="user"></param>
		/// <param name="pass"></param>
		/// <param name="path">
		///		The path to change to, if supplied.
		/// </param>
		/// <returns>
		///		True if the FTP information works, otherwise false.
		/// </returns>
		private bool testFtpInfo(FtpUploadInfo testInfo, out string msg)
		{
            Uploader u;

            msg = null;

            u = new Uploader(_db);

            try
            {
                u.TestFtpInfo(testInfo);
            }
            catch (EnterpriseDT.Net.Ftp.FTPException ftpe)
            {
                msg = ftpe.GetBaseException().Message;
                return false;
            }

            return true;
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


	}
}