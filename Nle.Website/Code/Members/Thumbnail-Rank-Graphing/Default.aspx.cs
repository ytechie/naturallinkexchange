using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChartDirector;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Members.Rank_Graphing;
using YTech.General.Charting.Controls;
using YTech.General.Web;
using YTech.General.Web.Controls;

namespace Nle.Website.Members.Thumbnail_Rank_Graphing
{
	/// <summary>
	///		The member page that displays thumbnails for their keywords
	///		in the major search engines.
	/// </summary>
	public partial class ThumbnailRankGraphingDefault : Page
	{
		private int _userId;
		private Database _db;
        private MainMaster _master;

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("ThumbnailRankGraphing.css");
            
			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

			displaySites();
		}

		private void displaySites()
		{
			Site[] sites;
			HtmlGenericControl siteHeader;

			sites = _db.GetUsersSites(_userId, false);

			foreach (Site currSite in sites)
			{
				siteHeader = new HtmlGenericControl("h2");
				ControlPlaceholder.Controls.Add(siteHeader);
				siteHeader.InnerText = currSite.Name;

				displayRankUrls(currSite.Id);
			}
		}

		private void displayRankUrls(int siteId)
		{
			SiteRankUrl[] rankUrls;
			HtmlGenericControl siteRankHeader;

			rankUrls = _db.GetSiteRankUrls(siteId);

			foreach (SiteRankUrl currRankUrl in rankUrls)
			{
				siteRankHeader = new HtmlGenericControl("h3");
				ControlPlaceholder.Controls.Add(siteRankHeader);
				siteRankHeader.InnerText = currRankUrl.Url;

				displayKeyPhrases(currRankUrl.Id);
			}
		}

		private void displayKeyPhrases(int rankUrlId)
		{
			KeyPhrase[] keyPhrases;
			HtmlGenericControl keyPhraseHeader;

			keyPhrases = _db.GetRankKeyPhrases(rankUrlId);

			foreach (KeyPhrase currKeyPhrase in keyPhrases)
			{
				keyPhraseHeader = new HtmlGenericControl("h4");
				ControlPlaceholder.Controls.Add(keyPhraseHeader);
				keyPhraseHeader.InnerText = string.Format("Key Phrase: \"{0}\"", currKeyPhrase.Phrase);

				displayThumbnailCharts(currKeyPhrase, rankUrlId);
			}
		}

		private void displayThumbnailCharts(KeyPhrase keyPhrase, int rankUrlId)
		{
			string logosRoot;
			HtmlGenericControl chartGroup;

			logosRoot = Global.VirtualDirectory + "Images/";

			chartGroup = new HtmlGenericControl("div");
			ControlPlaceholder.Controls.Add(chartGroup);
			chartGroup.Attributes["Class"] = "chartGroup";

			displayThumbnailChart(chartGroup, rankUrlId, keyPhrase, 1, logosRoot + "GoogleTiny.gif", "Google");
			displayThumbnailChart(chartGroup, rankUrlId, keyPhrase, 2, logosRoot + "YahooTiny.gif", "Yahoo!");
			displayThumbnailChart(chartGroup, rankUrlId, keyPhrase, 3, logosRoot + "MSNTiny.gif", "MSN Search");
			displayThumbnailChart(chartGroup, rankUrlId, keyPhrase, 4, logosRoot + "JeevesTiny.gif", "Ask Jeeves");	
		}

		private void displayThumbnailChart(HtmlGenericControl parentControl, int rankUrlId, KeyPhrase keyPhrase,
			int searchEngineId, string searchEngineLogoUrl, string searchEngineName)
		{
			DataTable currRankings;
			DBTable tableUtil;
			double[] yData;
			DateTime[] timestamps;
			ImageWriter img;
			HtmlGenericControl chartSection;
			ThumbnailChart tc;
			Image logo;
			HyperLink thumbLink;
			HyperLink logoLink;
			Label rankNumber;
	
			currRankings = _db.GetRankings(rankUrlId, searchEngineId, keyPhrase.Id, DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow);
			tableUtil = new DBTable(currRankings);
			yData = tableUtil.getCol(1);
			timestamps = tableUtil.getColAsDateTime(0);
	
			//Replace the 0's with 50's
			for(int i = 0; i < yData.Length; i++)
				if(yData[i] == 0)
					yData[i] = 50;

			tc = new ThumbnailChart();
			tc.YValues = yData;
			tc.Timestamps = timestamps;
			tc.YMin = 50;
			tc.YMax = 1;
	
			chartSection = new HtmlGenericControl("div");
			parentControl.Controls.Add(chartSection);
			chartSection.Attributes["Class"] = "thumbnailContainer";
	
			//chartHeader = new HtmlGenericControl("h1");
			//chartSection.Controls.Add(chartHeader);

			logoLink = new HyperLink();
			chartSection.Controls.Add(logoLink);
			logoLink.NavigateUrl = getSearchEngineLink(searchEngineId, keyPhrase.Phrase);

			rankNumber = new Label();
			chartSection.Controls.Add(rankNumber);
			rankNumber.CssClass = "rankNumber";

			//Display the last known rank if possible
			if(yData.Length > 0)
				rankNumber.Text = yData[yData.Length-1].ToString() + "<br />";
			else
				rankNumber.Text = "?<br />";

			rankNumber.ToolTip = "This number represents the last recorded rank of this key phrase";
			
			logo = new Image();
			logoLink.Controls.Add(logo);
			logo.ImageUrl = searchEngineLogoUrl;
			logo.AlternateText = searchEngineName;
			logo.CssClass = "thumbnailHeaderImage";
			

			thumbLink = new HyperLink();
			chartSection.Controls.Add(thumbLink);
			thumbLink.NavigateUrl = RankGraphingDefault.GetLoadUrl(rankUrlId, keyPhrase.Id, searchEngineId);

			img = new ImageWriter();
			thumbLink.Controls.Add(img);
			img.AlternateText = "Click to view details and modify parameters";
			img.ImageBitmap = tc.GenerateBitmap();
		}

		public static string getSearchEngineLink(int engineId, string keyPhrase)
		{
			switch(engineId)
			{
				case 1:
					return SearchEngineSearches.GetGoogleSearchUrl(keyPhrase); 
				case 2:
					return SearchEngineSearches.GetYahooSearchUrl(keyPhrase); 
				case 3:
					return SearchEngineSearches.GetMsnSearchUrl(keyPhrase);
				case 4:
					return SearchEngineSearches.GetJeevesSearchUrl(keyPhrase);
				default:
					return "";
			}
		}

		#region Web Form Designer generated code

		protected override void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

			//Tell the image writer to initialize so that it can
			//handle image requests if necessary.
			ImageWriter.InitForImageHandling(Page);
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