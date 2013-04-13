using System;
using System.Web.UI.WebControls;
using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.Website.Common_Controls
{
    /// <summary>
    ///		A simple control for selecting a site from a
    ///		list of the users available sites.
    /// </summary>
    public class SiteSelector : WebControl
    {
        private Database _db;

        private Literal _lblSiteLabel;
        private DropDownList _ddlSites;
        private bool _generatedChildren = false;

        private const string SESSION_SITE_ID = "SelectedSiteId";

        /// <summary>
        ///		The signature for the <see cref="SiteChanged"/> event handler.
        /// </summary>
        public delegate void SiteChangedDelegate(object s, SiteSelectionEventArgs e);

        /// <summary>
        ///		Raised when the user has selected a new site
        /// </summary>
        public event SiteChangedDelegate SiteChanged;

        /// <summary>
        ///		Creates a new instance of the <see cref="SiteSelector"/> class.
        /// </summary>
        public SiteSelector()
        {
            _db = Global.GetDbConnection();

            base.Init += new EventHandler(SiteSelector_Init);
        }

        /// <summary>
        ///		A wrapper that throws the <see cref="SiteChanged"/> event
        ///		so that it can handle the event being null, even in multi-threaded
        ///		operations.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void raiseSiteChangedEvent(object source, SiteSelectionEventArgs args)
        {
            SiteChangedDelegate e;

            e = SiteChanged;
            if (e != null)
                e(source, args);
        }

        private void SiteSelector_Init(object sender, EventArgs e)
        {
            createChildControls();
        }

        /// <summary>
        ///		Gets the ID of the site that is selected
        /// </summary>
        /// <returns>
        ///		The unique identifier of the site.  If no site is selected,
        ///		then -1 is returned;
        /// </returns>
        public int GetSelectedSiteId()
        {
            if (_ddlSites.SelectedValue == null || _ddlSites.SelectedValue == "")
                return -1;

            return int.Parse(_ddlSites.SelectedValue);
        }

        /// <summary>
        ///		Creates all the controls needed for the site selector.
        /// </summary>
        private void createChildControls()
        {
            //Don't generate the children more than once
            if (_generatedChildren)
                return;

            _lblSiteLabel = new Literal();
            Controls.Add(_lblSiteLabel);
            _lblSiteLabel.Text = "Currently Managing:";

            _ddlSites = new DropDownList();
            _ddlSites.AutoPostBack = true;
            Controls.Add(_ddlSites);
            _ddlSites.SelectedIndexChanged += new EventHandler(_ddlSites_SelectedIndexChanged);

            _generatedChildren = true;
        }

        public void PopulateSiteList(int userId)
        {
            PopulateSiteList(userId, false);
        }

        public void PopulateSiteList(int userId, bool fullyConfiguredSitesOnly)
        {
            Site[] sites;
            ListItem newItem;

            createChildControls();

            sites = _db.GetUsersSites(userId, fullyConfiguredSitesOnly);

            _ddlSites.Items.Clear();

            foreach (Site currSite in sites)
            {
                newItem = new ListItem();
                newItem.Text = currSite.Name;
                newItem.Value = currSite.Id.ToString();

                _ddlSites.Items.Add(newItem);
            }

            checkCurrentSiteExists();

            restoreSiteSelection();
            saveInitialSiteSelection();
        }

        private void saveInitialSiteSelection()
        {
            int siteId;

            siteId = GetSelectedSiteId();
            if (siteId != -1)
                saveSiteId(siteId);
        }

        private void checkCurrentSiteExists()
        {
            int cookieSiteId;
            int userId;
            bool existsInList = false;
            ListItem newItem;

            cookieSiteId = loadSiteId();

            //If there is a site selection in the cookie, make sure it exists in the list
            if (cookieSiteId != -1)
            {
                foreach (ListItem currItem in _ddlSites.Items)
                {
                    if (currItem.Value == cookieSiteId.ToString())
                    {
                        existsInList = true;
                        break;
                    }
                }

                if (!existsInList)
                {
                    userId = Global.GetCurrentUserId();
                    Site site = new Site(cookieSiteId);
                    _db.PopulateSite(site);
                    //Make sure that the site in session state belongs to the current user
                    if (site.UserId == userId)
                    {
                        //Add the item
                        newItem = new ListItem();
                        newItem.Text = site.Name;
                        newItem.Value = site.Id.ToString();

                        _ddlSites.Items.Add(newItem);
                    }
                }
            }
        }

        private void restoreSiteSelection()
        {
            int cookieSiteId;

            cookieSiteId = loadSiteId();

            //If there is a site selection in the session, try to select that one
            if (cookieSiteId != -1)
            {
                //Loop through all the items to see if the site is in
                //the list
                foreach (ListItem currItem in _ddlSites.Items)
                {
                    if (currItem.Selected) currItem.Selected = false;
                    if (currItem.Value == cookieSiteId.ToString())
                        currItem.Selected = true;
                }

                if (_ddlSites.SelectedValue.Length > 0 && _ddlSites.SelectedValue != cookieSiteId.ToString())
                    saveSiteId(int.Parse(_ddlSites.SelectedValue));
            }
            else
            {
                int selectedId;
                selectedId = this.GetSelectedSiteId();
                if (selectedId != -1)
                    saveSiteId(selectedId);
            }
        }

        /// <summary>
        ///     Saves the current site id to a cookie so that
        ///     we can keep showing the user the same site.
        /// </summary>
        private void saveSiteId(int siteId)
        {
            System.Web.HttpCookie cookie;

            cookie = new System.Web.HttpCookie(Global.COOKIE_SITE_ID, siteId.ToString());
            Page.Response.Cookies.Add(cookie);
        }

        /// <summary>
        ///     Gets the site id from a cookie so that we can show
        ///     the user the same site they were previously looking at.
        /// </summary>
        /// <returns></returns>
        private int loadSiteId()
        {
            return Global.GetCurrentSiteId();
        }

        private void _ddlSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedSiteId;
            SiteSelectionEventArgs args;

            selectedSiteId = GetSelectedSiteId();
            args = new SiteSelectionEventArgs(selectedSiteId);

            //Save the selected site id to the session
            saveSiteId(selectedSiteId);

            raiseSiteChangedEvent(this, args);
        }

        public void ChangeSite(int siteId)
        {
            saveSiteId(siteId);
            restoreSiteSelection();
        }
    }
}