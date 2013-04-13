using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChartDirector;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using DataSet = ChartDirector.DataSet;

namespace Nle.Website.Members.Rank_Graphing
{
	/// <summary>
	///		Displays an interactive chart application that allows the user to
	///		graph their ranking history.
	/// </summary>
	public partial class RankGraphingDefault : Page
	{
		/// <summary>The path of this page</summary>
		public const string MY_PATH = "Members/Rank-Graphing/";
		/// <summary>The name of this page</summary>
		public const string MY_FILE_NAME = "";

		public const string PARAM_RANK_URL_ID = "RankUrlId";
		public const string PARAM_KEY_PHRASE_ID = "KeyPhraseId";
		public const string PARAM_SEARCH_ENGINE_ID = "SearchEngineId";

		private int _userId;
		private XYChart _chart;
		private Database _db;
		private WebChartViewer _viewer;
        private MainMaster _master;
        private StatusHeader header;

		//Initial properties from the URL
		private int _rankUrlId = 0;
		private int _keyPhraseId = 0;
		private int _searchEngineId = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
            _master = (MainMaster)Master;
            _master.AddStylesheet("RankGraphing.css");
            header = _master.MasterStatusHeader;

			_db = Global.GetDbConnection();
			_userId = Global.GetCurrentUserId();

			setUpChart();

			if (!Page.IsPostBack)
			{
				getParameters();
				setInitialSearchEngineSelection();
				populateUrlAndPhrasesGroup();
				renderChart();
			}

			//Set up the control event handlers
			header.SiteChanged += new Nle.Website.Common_Controls.SiteSelector.SiteChangedDelegate(header_SiteChanged);
			ddlSiteRankUrls.SelectedIndexChanged += new EventHandler(ddlSiteRankUrls_SelectedIndexChanged);

			cmdRefresh.Click += new EventHandler(refreshEventHandler);
			base.PreRender += new EventHandler(refreshEventHandler);
		}

		private void getParameters()
		{
			string currVal;

			//Todo: How should I handle invalid values in the URL?

			currVal = Request.QueryString[PARAM_RANK_URL_ID];
			if(currVal != null)
				_rankUrlId = int.Parse(currVal);

			currVal = Request.QueryString[PARAM_KEY_PHRASE_ID];
			if(currVal != null)
				_keyPhraseId = int.Parse(currVal);

			currVal = Request.QueryString[PARAM_SEARCH_ENGINE_ID];
			if(currVal != null)
				_searchEngineId = int.Parse(currVal);
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
			this.Init += new EventHandler(RankGraphingDefault_Init);
		}

		#endregion

		private void getDimensions(out int height, out int width)
		{
			string selValue;
			string[] valueComponents;

			selValue = ddlChartSize.SelectedValue;
			valueComponents = selValue.Split("x".ToCharArray());

			if (valueComponents.Length != 2)
				throw new ApplicationException("Invalid Size Value Selected In Size Drop Down List");

			height = int.Parse(valueComponents[0]);
			width = int.Parse(valueComponents[1]);
		}

		/// <summary>
		///		Generic error handler for events that should trigger
		///		a refresh of the chart.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void refreshEventHandler(object s, EventArgs e)
		{
			renderChart();
		}

		/// <summary>
		///		Sets the initial search engine selection for the graph
		///		and the trending based on the search engine Id found
		///		in the querystring.
		/// </summary>
		private void setInitialSearchEngineSelection()
		{
			if(_searchEngineId == 0)
				return;

			chkPlotGoogle.Checked = false;
			chkPlotYahoo.Checked = false;
			chkPlotMSN.Checked = false;
			chkPlotAskJeeves.Checked = false;

			chkTrendGoogle.Checked = false;
			chkTrendYahoo.Checked = false;
			chkTrendMSN.Checked = false;
			chkTrendAskJeeves.Checked = false;

			switch(_searchEngineId)
			{
				case 1:
					chkPlotGoogle.Checked = true;
					chkTrendGoogle.Checked = true;
					break;
				case 2:
					chkPlotYahoo.Checked = true;
					chkTrendYahoo.Checked = true;
					break;
				case 3:
					chkPlotMSN.Checked = true;
					chkTrendMSN.Checked = true;
					break;
				case 4:
					chkPlotAskJeeves.Checked = true;
					chkTrendAskJeeves.Checked = true;
					break;
			}

		}

		private void populateUrlAndPhrasesGroup()
		{
			populateSiteUrls();
		}

		/// <summary>
		///		Occurrs when the user selects a different site
		/// </summary>
		private void header_SiteChanged(object s, SiteSelectionEventArgs e)
		{
			populateSiteUrls();
		}

		private void populateSiteUrls()
		{
			int siteId;
			SiteRankUrl[] siteRankUrls;
			ListItem currListItem;
			
			siteId = header.GetSelectedSiteId();

			siteRankUrls = _db.GetSiteRankUrls(siteId);
			ddlSiteRankUrls.Items.Clear();
			foreach (SiteRankUrl currSiteRankUrl in siteRankUrls)
			{
				currListItem = new ListItem();
				//Todo: Should we shorten this if it gets too long?
				currListItem.Text = currSiteRankUrl.Url;
				currListItem.Value = currSiteRankUrl.Id.ToString();

				if(_rankUrlId != 0 && currSiteRankUrl.Id == _rankUrlId)
				{
					currListItem.Selected = true;
				}

				ddlSiteRankUrls.Items.Add(currListItem);
			}

			populateSearchPhrases();
		}

		private void ddlSiteRankUrls_SelectedIndexChanged(object sender, EventArgs e)
		{
			populateSearchPhrases();
		}

		private void populateSearchPhrases()
		{
			string rankUrlIdString;
			int rankUrlId;
			KeyPhrase[] keyPhrases;
			ListItem currListItem;

			if (ddlSiteRankUrls.SelectedIndex == -1)
				return;

			rankUrlIdString = ddlSiteRankUrls.SelectedValue;
			rankUrlId = int.Parse(rankUrlIdString);

			keyPhrases = _db.GetRankKeyPhrases(rankUrlId);
			ddlSearchPhrases.Items.Clear();
			foreach (KeyPhrase currKeyPhrase in keyPhrases)
			{
				currListItem = new ListItem();
				currListItem.Text = currKeyPhrase.Phrase;
				currListItem.Value = currKeyPhrase.Id.ToString();

				if(_keyPhraseId != 0 && currKeyPhrase.Id == _keyPhraseId)
				{
					currListItem.Selected = true;
				}

				ddlSearchPhrases.Items.Add(currListItem);
			}
		}

		#region Charting

		private void renderChart()
		{
			string imageMap;

			plotData();
			imageMap = getImageMap();

			_viewer.Image = _chart.makeWebImage(Chart.PNG);
			_viewer.ImageMap = imageMap;
		}

		private void RankGraphingDefault_Init(object sender, EventArgs e)
		{
			_viewer = new WebChartViewer();
			ControlPlaceHolder.Controls.Add(_viewer);
		}

		private void plotData()
		{
			addSearchEnginePlot(1, "<*img=GoogleTiny.gif*>", chkPlotGoogle.Checked, chkTrendGoogle.Checked);
			addSearchEnginePlot(2, "<*img=YahooTiny.gif*>", chkPlotYahoo.Checked, chkTrendYahoo.Checked);
			addSearchEnginePlot(3, "<*img=MSNTiny.gif*>", chkPlotMSN.Checked, chkTrendMSN.Checked);
			addSearchEnginePlot(4, "<*img=JeevesTiny.gif*>", chkPlotAskJeeves.Checked, chkTrendAskJeeves.Checked);
		}

		private void addSearchEnginePlot(int searchEngineId, string plotName, bool plot, bool trend)
		{
			DBTable tableUtil;
			LineLayer layer;
			double[] timestampValues;
			double[] yValues;
			DataSet ds;
			DataTable rankData;
			TrendLayer trendLayer;

			if (!plot && !trend)
				return;

			rankData = getRankData(searchEngineId);

			if (rankData.Rows.Count == 0)
			{
				timestampValues = new double[0];
				yValues = new double[0];

				//Don't trend if there is no data
				trend = false;
			}
			else
			{
				tableUtil = new DBTable(rankData);
				timestampValues = tableUtil.getCol(0);
				yValues = tableUtil.getCol(1);

				//Change the yValues that are 0
				for (int i = 0; i < yValues.Length; i++)
				{
					if (yValues[i] == 0)
						yValues[i] = Chart.NoValue;
				}
			}

			if (plot)
			{
				layer = _chart.addLineLayer();
				layer.setLineWidth(2);
				layer.setXData(timestampValues);

				ds = layer.addDataSet(yValues);
				ds.setDataName(plotName);
			}

			if (trend)
			{
				trendLayer = _chart.addTrendLayer(yValues, -1, string.Format("{0} Trend", plotName));
				trendLayer.setXData(timestampValues);
			}
		}

		private void setUpChart()
		{
			int height;
			int width;
			string imagePath;

			getDimensions(out height, out width);

			_chart = new XYChart(width, height);

			//Set the plotarea and turn on both horizontal and vertical grid lines with light grey
			//color (0xc0c0c0)
			_chart.setPlotArea(55, 45, width - 80, height - 90, 0xffffff, -1, -1, 0xc0c0c0, -1);

			//Add a legend box at (55, 25) (top of the chart) with horizontal layout. Use 8
			//pts Arial font. Set the background and border color to Transparent.
			_chart.addLegend(55, 5, false, "", 8).setBackground(Chart.Transparent);

			//Add a title to the y axis
			_chart.yAxis().setTitle("Rank");

			//Set the y scale
			_chart.yAxis().setLinearScale(50.0, 1.0, -10.0);
			//_chart.yAxis().setMargin(0, 0);
			//_chart.yAxis().setAutoScale(0, 0, 0) ;

			//Set the path to look for images
			imagePath = Server.MapPath("../../Images");
			_chart.setSearchPath(imagePath);

			//Add a zone to show where the top 10 is
			_chart.yAxis().addZone(1, 10, 0x99ff99);
		}

		private string getImageMap()
		{
			string imageMap;

			imageMap = _chart.getHTMLImageMap("", "", "title=\"Rank on {x|mmm d} was {value}\"");

			return imageMap;
		}

		#endregion

		/// <summary>
		///		Uses the currently selected options to determine the data
		///		that needs to be retrieved, and then retrieves it.
		/// </summary>
		/// <returns></returns>
		private DataTable getRankData(int searchEngineId)
		{
			DataTable rankData;
			int siteRankUrlId;
			int keyPhraseId;
			DateTime startTime;
			int monthsRange;

			if (ddlSiteRankUrls.SelectedIndex == -1)
				return new DataTable();
			else
				siteRankUrlId = int.Parse(ddlSiteRankUrls.SelectedValue);

			if (ddlSearchPhrases.SelectedIndex == -1)
				return new DataTable();
			else
				keyPhraseId = int.Parse(ddlSearchPhrases.SelectedValue);

			monthsRange = int.Parse(rdoTimeRange.SelectedValue);

			if (monthsRange == -1)
				startTime = new DateTime();
			else
				startTime = DateTime.UtcNow.AddMonths(-monthsRange);

			rankData = _db.GetRankings(siteRankUrlId, searchEngineId, keyPhraseId, startTime, new DateTime());

			return rankData;
		}

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl(int rankUrlId, int keyPhraseId, int searchEngineId)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_RANK_URL_ID, rankUrlId);
			url.Parameters.AddParameter(PARAM_KEY_PHRASE_ID, keyPhraseId);
			url.Parameters.AddParameter(PARAM_SEARCH_ENGINE_ID, searchEngineId);

			return url.ToString();
		}
	}
}