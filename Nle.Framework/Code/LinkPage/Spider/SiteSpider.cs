using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web;
using log4net;

namespace Nle.LinkPage.Spider
{
    /// <summary>
    ///     A spider that is capable of visiting customers websites
    ///     and verifying that they have their link pages configured correctly.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The Process:
    /// 
    ///         Since a link page must be on the root of the customers website, that
    ///         is where we start spidering.  We look for the type of link page that
    ///         they have configured, and look for a link to it.
    ///     </para>
    ///     <para>
    ///         How to determine if a link is for a link page:
    /// 
    ///         If the user is using the FTP upload method, we know the name of the file
    ///         that they uploaded.  We will simply look for that file.  If they are using
    ///         a proxy script, we will send over a special category name on all the links
    ///         we find, and look for a special response that the server side link
    ///         generator will need to handle.
    ///     </para>
    /// </remarks>
    public class SiteSpider
    {
        private string _url;
        private GenerateTypes _generateType;
        private Database _db;
        private string _ftpFileName;
        private IPageContentReader _pageReader;

        //href\s*=\s*["'](?<Url>[^"]*)["']
        private const string REGEX_HTML_LINK = "[Hh][Rr][Ee][Ff]\\s*=\\s*[\"'](?<Url>[^\"]*)[\"']";

        /// <summary>
        ///     A special value that can be sent to the category name parameter
        ///     on a link page, and the link page will reply with <see cref="PAGE_CHECK_RESPONSE"/>.
        /// </summary>
        public const string PAGE_CHECK_STRING = "pagecheck456";

        /// <summary>
        ///     The special response given when a category name of
        ///     <see cref="PAGE_CHECK_STRING"/> is used.
        /// </summary>
        public const string PAGE_CHECK_RESPONSE = "success789";

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Creates a new instance of the site spider.
        /// </summary>
        /// <param name="site">
        ///     The site that should be spidered to have it's link page verified.
        /// </param>
        /// <param name="db">
        ///     The database to use to retrieve the necessary information about the site.
        /// </param>
        public SiteSpider(Site site, Database db)
        {
            Nle.Components.LinkPage initialLinkPage;

            _db = db;

            _url = site.Url;

            //For simplicity, we'll create our own page reader
            _pageReader = new PageContentReader();

            _generateType = (GenerateTypes)_db.GetIntSiteSetting(site.Id, 6);

            if (_generateType == GenerateTypes.FtpUpload)
            {
                initialLinkPage = db.GetInitialLinkPage(site.Id);
                _ftpFileName = initialLinkPage.PageName + ".html";
            }
        }

        /// <summary>
        ///     Creates a new instance of the site spider class that can check
        ///     for the existance of a file that has been FTP'd to the users site.
        /// </summary>
        /// <param name="baseUrl">
        ///     The URL of the users site to check for the link page.
        /// </param>
        /// <param name="fileName">
        ///     The name of the file to look for in the content of the <see cref="url"/>
        /// </param>
        public SiteSpider(string baseUrl, string ftpFileName, IPageContentReader pageReader)
        {
            _url = baseUrl;
            _ftpFileName = ftpFileName;
            _generateType = GenerateTypes.FtpUpload;
            _pageReader = pageReader;

            _log.DebugFormat("FTP site spider created for URL '{0}' looking for file '{1}'", baseUrl, ftpFileName);
        }

        /// <summary>
        ///     Creates a new instance of the site spider class that can check
        ///     all the links on a page, and determine if one of them is a valid
        ///     link page.
        /// </summary>
        /// <param name="baseUrl">
        ///     The URL to check the links for.
        /// </param>
        /// <param name="generateType">
        ///     A <see cref="GenerateTypes"/> that specifies either a PHP script
        ///     or an ASPX script to look for.  Any other type will throw an exception.
        /// </param>
        /// <param name="pageReader">
        ///     An <see cref="IPageContentReader"/> capable of requesting Internet URL's.
        /// </param>
        public SiteSpider(string baseUrl, GenerateTypes generateType, IPageContentReader pageReader)
        {
            _url = baseUrl;
            _pageReader = pageReader;
            _generateType = generateType;

            _log.DebugFormat("FTP site spider created for URL '{0}' looking for a valid script", baseUrl);
        }

        /// <summary>
        ///     Searches through a list of links to find a specific file name.
        /// </summary>
        /// <param name="allLinks"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool hasLink(string[] allLinks, string fileName, out string absoluteUrl)
        {
            UrlBuilder parsedUrl;

            foreach (string currLink in allLinks)
            {
                parsedUrl = new UrlBuilder(currLink);
                if (parsedUrl.URLBase.EndsWith(fileName))
                {
                    _log.DebugFormat("Found '{0}' which is the filename '{1}'", currLink, fileName);
                    absoluteUrl = MakeLinkAbsolute(_url, currLink);
                    return true;
                }
            }

            absoluteUrl = null;

            //We didn't find a match
            return false;
        }

        /// <summary>
        ///     Spiders the specified site and determines if it
        ///     has a valid link page linked from the front page.
        /// </summary>
        public bool HasLinkPage(out string linkPageUrl)
        {
            return HasLinkPage(null, out linkPageUrl);
        }

        /// <summary>
        ///     Spiders the specified site and determines if it
        ///     has a valid link page linked from the front page.
        /// </summary>
        public bool HasLinkPage(string lastLinkPageUrl, out string linkPageUrl)
        {
            string pageSource;
            string[] links;
            bool found = false;
            string absoluteUrl = null;

            try
            {
                pageSource = requestPage(_url);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error reading a main site URL: {0}", _url, ex);
                linkPageUrl = null;
                return false;
            }

            links = getAllPageLinks(pageSource);

            if (_generateType == GenerateTypes.FtpUpload)
            {
                _log.Debug("Site uses the FTP upload method, so I'm looking for a link to the FTP file name");
                found = hasLink(links, _ftpFileName, out absoluteUrl);
            }
            else if(_generateType == GenerateTypes.NetScript || _generateType == GenerateTypes.PhpScript)
            {
                _log.Debug("Site uses a script method of generating link pages, I'll have to check them all");

                //Check if the old link is still valid, if so, we have a major shortcut
                if (oldLinkStillValid(links, lastLinkPageUrl))
                {
                    found = true;
                    absoluteUrl = lastLinkPageUrl;
                }
                else
                {
                    if(_generateType == GenerateTypes.NetScript)
                        found = checkSubPages(links, "aspx", out absoluteUrl);
                    if(_generateType == GenerateTypes.PhpScript)
                        found = checkSubPages(links, "php", out absoluteUrl);
                }
            }
            else
            {
                _log.InfoFormat("Unsupported link page generation type {0} for site with URL {1}", (int)_generateType, _url); 
            }

            linkPageUrl = absoluteUrl;
            
            return found;
        }

        /// <summary>
        ///     Checks if the specified link is still valid.
        /// </summary>
        /// <param name="lastLinkPageUrl"></param>
        /// <returns></returns>
        private bool oldLinkStillValid(string[] relativeLinks, string lastLinkPageUrl)
        {
            bool found;
            string absoluteUrl;

            if (lastLinkPageUrl == null)
                return false;
            if (lastLinkPageUrl.Length == 0)
                return false;

            found = checkLinkPage(lastLinkPageUrl);

            if (found)
            {
                _log.Debug("Their old link page is still working, let's check to make sure it's linked to");

                //Check the main page for a link to the link page we found
                foreach (string currLink in relativeLinks)
                {
                    absoluteUrl = MakeLinkAbsolute(_url, currLink);
                    if (absoluteUrl == lastLinkPageUrl)
                    {
                        _log.Debug("The old link page is valid, and is linked from the front page, we are done");
                        return true;
                    }
                }

                _log.Debug("The link page is still valid, but they are not linking to it, we'll have to keep looking");
                return false;
            }
            else
            {
                //The link page URL doesn't appear to be valid
                return false;
            }            
        }

        public static string MakeLinkAbsolute(string urlBase, string relativeUrl)
        {
            Uri uri1;
            Uri uri2;

            uri1 = new Uri(urlBase);
            uri2 = new Uri(uri1, relativeUrl);

            return uri2.ToString();
        }

        private bool checkSubPages(string[] links, string scriptExtension, out string absoluteUrl)
        {
            foreach (string currLink in links)
            {
                if (checkLinkPage(currLink, scriptExtension, out absoluteUrl))
                    return true;
            }

            absoluteUrl = null;

            //We didn't find any that passed our test
            return false;
        }

        /// <summary>
        ///     Sends a special request to the specified page to
        ///     determine if it's one of our link pages.
        /// </summary>
        /// <param name="url">
        ///     The URL to request to see if it's one of our link pages.
        /// </param>
        /// <param name="scriptExtension">
        ///     The extension of the link page script that the the site
        ///     is using.  This helps make a faster decision on whether this
        ///     is a valid link page.  It is NOT required.
        /// </param>
        /// <returns>
        ///     True if the page appears to be a link page, otherwise false.
        /// </returns>
        private bool checkLinkPage(string url, string scriptExtension, out string absoluteUrl)
        {
            try
            {
                absoluteUrl = MakeLinkAbsolute(_url, url);
                return checkLinkPage(absoluteUrl);
            }
            catch
            {
                absoluteUrl = null;
                return false;
            }
        }

        private bool checkLinkPage(string absoluteUrl)
        {
            return checkLinkPage(absoluteUrl, null);
        }

        private bool checkLinkPage(string absoluteUrl, string scriptExtension)
        {
            UrlBuilder parsedUrl;
            string response;

            try
            {
                parsedUrl = new UrlBuilder(absoluteUrl);

                //Only look for pages with the right extension
                if (scriptExtension != null && !parsedUrl.URLBase.EndsWith(scriptExtension))
                    return false;

                parsedUrl.Parameters.AddParameter("Category-Name", PAGE_CHECK_STRING);

                response = requestPage(parsedUrl.ToString());
                if (response != null && response.StartsWith(PAGE_CHECK_RESPONSE))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private string[] getAllPageLinks(string pageSource)
        {
            Regex rx;
            MatchCollection matches;
            string[] links;
            
            rx = new Regex(REGEX_HTML_LINK);
            matches = rx.Matches(pageSource);

            links = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                links[i] = matches[i].Groups["Url"].Value;
            }

            _log.DebugFormat("Read {0} links from the source", links.Length);

            return links;
        }

        private string requestPage(string url)
        {
            return _pageReader.GetPageContents(url);
        }
    }
}
