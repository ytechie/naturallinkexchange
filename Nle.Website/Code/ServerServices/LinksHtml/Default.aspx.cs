using System;
using System.Web.UI;
using Nle.Db.SqlServer;
using Nle.LinkPage;
using YTech.General.Web;

namespace Nle.Website.ServerServices.LinksHtml
{
	/// <summary>
	///		This service generates the HTML for the link pages.
	/// </summary>
	public partial class LinksHtmlDefault : Page
	{
		/// <summary>
		///		The URL parameter that specifies a specific link page
		///		to display.
		/// </summary>
		public const string PARAM_PAGE_ID = "PageId";

		/// <summary>
		///		The URL parameter that specifies the clients site
		///		key which can be used to uniquely identify them.
		/// </summary>
		public const string PARAM_SITE_KEY = "sk";

		/// <summary>
		///		The URL parameter that specifies the name of the link page to display.
		/// </summary>
		public const string PARAM_LINK_PAGE_NAME = "cn";

		/// <summary>
		///		The value from the client for the SCRIPT_NAME server variable.
		/// </summary>
		public const string PARAM_SCRIPT_NAME = "sn";

		private int _pageId;
		private Database _db;
		private Modes _mode;
		private string _linkPageName;
		private string _sourceKey;
		private string _scriptName;

		private enum Modes
		{
			/// <summary>
			///		The page mode could not be determined.
			/// </summary>
			Unknown,
			/// <summary>
			///		The request was for a specific link page ID
			/// </summary>
			PageId,
			/// <summary>
			///		The request was from a client website.
			/// </summary>
			Client
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();
			getParameters();

			if (_mode == Modes.Unknown)
			{
				throw new ApplicationException("The page mode could not be determined from the specified parameters.");
			}

			if (_mode == Modes.Client)
			{
                //Check if the page is being tested as a link page
                if (_linkPageName == Nle.LinkPage.Spider.SiteSpider.PAGE_CHECK_STRING)
                {
                    sendPageCheckResponse();
                    return;
                }

				lookupPageIdFromSourceUrl();
			}

			generatePage();
		}

        /// <summary>
        ///     Sends a special response to the requestor as a response to a page
        ///     check request.  This is for the <see cref="Nle.LinkPage.Spider.SiteSpider"/>
        /// </summary>
        private void sendPageCheckResponse()
        {
            Response.Clear();
            Response.Write(Nle.LinkPage.Spider.SiteSpider.PAGE_CHECK_RESPONSE);
            Response.Flush();
            Response.End();
        }

		/// <summary>
		///		Uses the source URL and the database to determine the page
		///		Id that is being requested.
		/// </summary>
		private void lookupPageIdFromSourceUrl()
		{
			Components.LinkPage lp;
			string pageName;

			//Make sure the category name is NULL if we don't know what it is
			if(_linkPageName == null || _linkPageName.Length == 0)
				pageName = null;
			else
				pageName = _linkPageName;

			lp = _db.GetLinkPageForClient(new Guid(_sourceKey), pageName);

			if(lp == null)
				_pageId = -1;
			else
				_pageId = lp.Id;
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
		///		Retrives the URL parameters.
		/// </summary>
		/// <remarks>
		///		If a source URL and a page Id are both found in the URL,
		///		both values are loaded, but the page will be in page Id mode.
		/// </remarks>
		private void getParameters()
		{
			string currVal;

			//Set the mode to "Unknown" in case we fail to figure it out
			_mode = Modes.Unknown;

			//See if it's a request from a customer
			currVal = Request.QueryString[PARAM_SITE_KEY];
			if(currVal != null && currVal.Length > 0)
			{
				_mode = Modes.Client;
				_linkPageName = Request.QueryString[PARAM_LINK_PAGE_NAME];
				_sourceKey = currVal;
				_scriptName = Request.QueryString[PARAM_SCRIPT_NAME];
			}

			//Get the page Id
			currVal = Request.QueryString[PARAM_PAGE_ID];
			if(currVal != null && currVal.Length > 0)
			{
				_mode = Modes.PageId;
				_pageId = int.Parse(currVal);
				_scriptName = Request.ServerVariables["SCRIPT_NAME"];
			}
		}

		private void generatePage()
		{
			LinkPageHtmlGenerator lpg;
			Components.LinkPage lp;
			string pageHtml;

			lp = new Components.LinkPage(_pageId);
			_db.PopulateLinkPage(lp);

			lpg = new LinkPageHtmlGenerator();
			lpg.CategoryUrlSignature = getCategoryUrlSignature();
			pageHtml = lpg.GetPageHtml(_db, lp);

			Response.Clear();
			Response.Write(pageHtml);
			Response.End();
		}

		/// <summary>
		///		Gets the URL signature that can be used for linked link pages.
		/// </summary>
		/// <returns>
		///		A string URL.
		/// </returns>
		private string getCategoryUrlSignature()
		{
			UrlBuilder url;

			url = new UrlBuilder(_scriptName);

			if(_mode == Modes.Client)
			{
				url.Parameters.AddParameterNoEncode("Category-Name", "{1}");
				return url.ToString();
			}
			else if(_mode == Modes.PageId)
			{
				url.Parameters.AddParameterNoEncode(PARAM_PAGE_ID, "{0}");
				return url.ToString();
			}
			else
			{
				throw new NotImplementedException("Unable to determine category URL signature from this mode") ;
			}
		}

	}
}