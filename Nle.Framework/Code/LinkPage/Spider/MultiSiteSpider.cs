using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Nle.Db.SqlServer;
using Nle.Components;
using log4net;

namespace Nle.LinkPage.Spider
{
    /// <summary>
    ///     Automates the <see cref="SiteSpider"/> to spider
    ///     all of the sites in the system.
    /// </summary>
    public class MultiSiteSpider
    {
        private Database _db;

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Creates a new instance of the <see cref="MultiSiteSpider"/>.
        /// </summary>
        /// <param name="db"></param>
        public MultiSiteSpider(Database db)
        {
            _db = db;
        }

        /// <summary>
        ///     Spiders all of the sites in the <see cref="Site"/> array, and
        ///     stores the spidering results to the database.
        /// </summary>
        /// <param name="sites"></param>
        public void SpiderSites(Site[] sites)
        {
            SiteSpider spider;
            bool validLinkPage;
            string linkPageUrl;

            foreach (Site currSite in sites)
            {
                spider = new SiteSpider(currSite, _db);
                validLinkPage = spider.HasLinkPage(currSite.LinkPageUrl, out linkPageUrl);
                saveSiteResults(currSite, validLinkPage, linkPageUrl);
            }
        }

        private void saveSiteResults(Site site, bool validLinkPage, string linkPageUrl)
        {
            LinkPageStatus status;

            //First, update the site to cache the link page url
            if(validLinkPage)
            {
                if (linkPageUrl != site.LinkPageUrl)
                {
                    _log.DebugFormat("Found a new link page URL of {0} for site #{1}", linkPageUrl, site.Id);

                    site.LinkPageUrl = linkPageUrl;
                    _db.SaveSite(site);
                }
            }

            status = new LinkPageStatus();
            status.SiteId = site.Id;
            status.Valid = validLinkPage;
            status.CheckedOn = _db.GetServerTime();

            //Save the status to the database
            _db.SaveLinkPageStatus(status);

            _log.DebugFormat("Saved a link page status to the database: {0}", status.ToString());
        }
    }
}
