using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.Website.Common_Controls
{
	/// <summary>
	///		The common header for the pages.
	/// </summary>
	public partial class StatusHeader : UserControl
	{
		#region Form Controls



		
		#endregion

		private Database _db;
		private int _userId = -1;
		private bool _siteSelectorLoaded = false;
		private bool _filterIncompleteSites = false;

		public bool FilterIncompleteSites
		{
			get { return _filterIncompleteSites; }
			set
			{
				_filterIncompleteSites = value;
				loadSiteSelector();
			}
		}

		/// <summary>
		///		This event occurrs when the site selector control
		///		has been changed to a different site.
		/// </summary>
		public event SiteSelector.SiteChangedDelegate SiteChanged;

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

			if(_userId != -1)
			{
				cmdLogOff.Click += new EventHandler(cmdLogOff_Click);
				showLoggedOnInfo();

				if(!_siteSelectorLoaded && !Page.IsPostBack)
					loadSiteSelector();

				ssSiteSelector.SiteChanged += new Nle.Website.Common_Controls.SiteSelector.SiteChangedDelegate(ssSiteSelector_SiteChanged);
			}
		}

		private void loadSiteSelector()
		{
			if(_userId == -1)
				_userId = Global.GetCurrentUserId();

			ssSiteSelector.PopulateSiteList(_userId, FilterIncompleteSites);

			_siteSelectorLoaded = true;
		}

		private void showLoggedOnInfo()
		{
			User user;

			pnlNotLoggedIn.Visible = false;
			pnlLoggedIn.Visible = true;

			//Load the user information
			user = new User(_userId);
			_db.PopulateUser(user);

			lblUser.Text = user.Name;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion

		private void cmdLogOff_Click(object sender, EventArgs e)
		{
			FormsAuthentication.SignOut();
			Response.Redirect(Global.VirtualDirectory);
		}

		/// <summary>
		///		Gets the site ID of the site that is currently
		///		selected in the	header.
		/// </summary>
		public int GetSelectedSiteId()
		{
			if(!_siteSelectorLoaded && !Page.IsPostBack)
				loadSiteSelector();

			return ssSiteSelector.GetSelectedSiteId();
		}

		public void SetSelectedSiteId(int siteId)
		{
			if(!_siteSelectorLoaded && !Page.IsPostBack)
				loadSiteSelector();

			ssSiteSelector.ChangeSite(siteId);
		}

		private void ssSiteSelector_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			SiteSelector.SiteChangedDelegate sce;

			sce = SiteChanged;
			if(sce != null)
				sce(s, e);
		}
	}
}