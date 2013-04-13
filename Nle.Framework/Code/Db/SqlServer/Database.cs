using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;
using Nle.Components;
using Nle.Db.Exceptions;
using Rss;
using YTech.General.DataMapping;
using YTech.Db.SqlServer;
using RssFeed = Nle.Components.RssFeed;

namespace Nle.Db.SqlServer
{
	/// <summary>
	///		Provides all of the functionality to use SQL Server 2000
	///		as the data store for the linking application.
	/// </summary>
	public class Database
	{
		#region Private Properties

		private DbConnection _dbConn;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region General

		/// <summary>
		///		Gets the UTC <see cref="DateTime"/> of the database server.
		/// </summary>
		/// <remarks>
		///		It's important to have a single source for the current time. You
		///		should always use this to stay in sync.
		/// </remarks>
		/// <returns></returns>
		public DateTime GetServerTime()
		{
			SqlCommand cmd;
			DataSet ds;
			object cellValue;

			cmd = _dbConn.GetSqlCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = "Select GetUtcDate()";

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				throw new Exception("No data returned when calling the server function to retrieve the date");

			cellValue = ds.Tables[0].Rows[0][0];

			if(cellValue == null || cellValue == DBNull.Value)
				throw new Exception("NULL value returned when calling the server function to retrieve the date");

			return (DateTime)cellValue;
		}

		#endregion

		#region Constructors

    /// <summary>
		///		Creates a new instance of the <see cref="Database"/> class.
		/// </summary>
		/// <param name="dbConn"></param>
		public Database(DbConnection dbConn)
		{
			_dbConn = dbConn;
		}


		#endregion

		#region Global Settings

		/// <summary>
		///		Populates the specified <see cref="GlobalSetting"/>.
		/// </summary>
		/// <param name="globalSetting">The <see cref="GlobalSetting"/> that should be populated.</param>
		/// <exception cref="GlobalSettingNotFound">
		///		Thrown when the identifier of the global setting could not be found
		///		in the database.
		/// </exception>
		public void PopulateGlobalSetting(GlobalSetting globalSetting)
		{
			SqlCommand cmd;
			DataSet ds;

			cmd = _dbConn.GetStoredProcedureCommand("GetGlobalSetting");
			cmd.Parameters.AddWithValue("@GlobalSettingId", globalSetting.Id);

			ds = _dbConn.GetDataSet(cmd);

			GlobalSettingNotFound noDataEx = new GlobalSettingNotFound(globalSetting.Id);

			DataMapper.PopulateObject(globalSetting, ds, noDataEx, null);
		}

		/// <summary>
		///		Loads the setting from the database with the specified ID
		/// </summary>
		///	<param name="id">
		///		The unique identifier of the setting in the database.
		///	</param>
		/// <exception cref="GlobalSettingNotFound">
		///		Thrown when the identifier of the global setting could not be found
		///		in the database.
		/// </exception>
		public GlobalSetting GetGlobalSetting(int id)
		{
			GlobalSetting gs;

			gs = new GlobalSetting(id);
			PopulateGlobalSetting(gs);

			return gs;
		}


		#endregion

		#region Rank Charting

		/// <summary>
		///		Determines the Site Id for the specified site
		///		rank url Id.
		/// </summary>
		/// <param name="siteRankUrlId">
		///		The unique identifier of the site url to look up.
		/// </param>
		/// <returns>
		///		A site Id.
		/// </returns>
		public int GetSiteIdFromRankUrl(int siteRankUrlId)
		{
			SqlCommand cmd;
			DataSet ds;

			cmd = _dbConn.GetSqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Sites_GetSiteFromRankUrl";

			//Add all the parameters
			cmd.Parameters.AddWithValue("@SiteRankUrlId", siteRankUrlId);

			ds = _dbConn.GetDataSet(cmd);

			//Todo: what if the lookup fails?

			return (int) ds.Tables[0].Rows[0][0];
		}


		/// <summary>
		///		Saves the specified <see cref="Nle.Ranking.TopDogPro.Ranking"/>
		///		object from TopDog Pro.
		/// </summary>
		public void SaveRankings(Ranking.TopDogPro.Ranking[] rankings)
		{
			SqlCommand[] commands;
			SqlCommand cmd;
			Ranking.TopDogPro.Ranking currRanking;

			commands = new SqlCommand[rankings.Length];

			for (int i = 0; i < rankings.Length; i++)
			{
				cmd = new SqlCommand();
				currRanking = rankings[i];

				cmd = _dbConn.GetSqlCommand();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = "Ranking_AddTopDogProRankings";

				//Add all the parameters
                cmd.Parameters.AddWithValue("@Timestamp", currRanking.Timestamp);
				cmd.Parameters.AddWithValue("@Rank", currRanking.Rank);
				cmd.Parameters.AddWithValue("@SearchEngineName", currRanking.SearchEngine);
				cmd.Parameters.AddWithValue("@SiteUrl", currRanking.Url);
				cmd.Parameters.AddWithValue("@SearchPhrase", currRanking.SearchString);

				commands[i] = cmd;
			}

			_dbConn.ExecuteNonQueriesAtomic(commands);
		}

		/// <summary>
		///		Retrieves search engine rankings.
		/// </summary>
		public DataTable GetRankings(int siteRankUrlId, int searchEngineId, int keyPhraseId, DateTime startTime, DateTime endTime)
		{
			SqlCommand cmd;
			DataSet ds;

			cmd = _dbConn.GetSqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Ranking_GetRankings";

			//Add all the parameters
			cmd.Parameters.AddWithValue("@SiteRankUrlId", siteRankUrlId);
			cmd.Parameters.AddWithValue("@SearchEngineId", searchEngineId);
			cmd.Parameters.AddWithValue("@KeyPhraseId", keyPhraseId);

			if (startTime != new DateTime())
				cmd.Parameters.AddWithValue("@StartTime", startTime);
			if (endTime != new DateTime())
				cmd.Parameters.AddWithValue("@EndTime", endTime);

			ds = _dbConn.GetDataSet(cmd);

			return ds.Tables[0];
		}



		/// <summary>
		///		Gets the site rank url's for the specified
		///		site.
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public SiteRankUrl[] GetSiteRankUrls(int siteId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			SiteRankUrl currRankUrl;
			SiteRankUrl[] rankUrls;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Sites_GetRankUrls";
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new SiteRankUrl[0]; //Return an empty array to avoid null errors

			rankUrls = new SiteRankUrl[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new site with the Id from the database
				currRankUrl = new SiteRankUrl((int) currRow["Id"]);
				DataMapper.PopulateObject(currRankUrl, currRow, null);
				//Add the current link to the return array
				rankUrls[i] = currRankUrl;
			}

			return rankUrls;
		}

		/// <summary>
		///		Gets the <see cref="KeyPhrase"/> objects that exist for
		///		the specified site rank url.
		/// </summary>
		/// <returns></returns>
		public KeyPhrase[] GetRankKeyPhrases(int siteRankUrlId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			KeyPhrase currKeyPhrase;
			KeyPhrase[] keyPhrases;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Ranking_GetUrlKeyPhrases";
			cmd.Parameters.AddWithValue("@SiteRankUrlId", siteRankUrlId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new KeyPhrase[0]; //Return an empty array to avoid null errors

			keyPhrases = new KeyPhrase[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new site with the Id from the database
				currKeyPhrase = new KeyPhrase((int) currRow["Id"]);
				DataMapper.PopulateObject(currKeyPhrase, currRow, null);
				//Add the current link to the return array
				keyPhrases[i] = currKeyPhrase;
			}

			return keyPhrases;
		}


		#endregion

		#region Links

		/// <summary>
		///		Populates the specified link page.
		/// </summary>
		/// <param name="linkPage"></param>
		public void PopulateLinkPage(Components.LinkPage linkPage)
		{
			SqlCommand cmd;
			DataSet ds;
			ApplicationException ae;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetLinkPage");
			cmd.Parameters.AddWithValue("@LinkPageId", linkPage.Id);

			ds = _dbConn.GetDataSet(cmd);

			ae = new ApplicationException("Link Page Not Found");

			DataMapper.PopulateObject(linkPage, ds, ae, null);
		}

		/// <summary>
		///		Gets the initial link page to display for
		///		the specified site id.
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public Components.LinkPage GetInitialLinkPage(int siteId)
		{
			SqlCommand cmd;
			DataSet ds;
			ApplicationException ae;
			Components.LinkPage lp;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetInitialLinkPage");
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			ds = _dbConn.GetDataSet(cmd);

			lp = new Components.LinkPage();

			ae = new ApplicationException("Link Page Not Found");
			DataMapper.PopulateObject(lp, ds, ae, null);

			return lp;
		}

		private Components.LinkPage[] GetLinkPagesFromDataset(DataSet ds)
		{
			DataRow currRow;
			Components.LinkPage currLinkPage;
			Components.LinkPage[] linkPages;

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new Components.LinkPage[0]; //Return an empty array to avoid null errors

			linkPages = new Components.LinkPage[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new site with the Id from the database
				currLinkPage = new Components.LinkPage((int) currRow["Id"]);
				DataMapper.PopulateObject(currLinkPage, currRow, null);
				//Add the current link to the return array
				linkPages[i] = currLinkPage;
			}

			return linkPages;
		}

		public Components.LinkPage[] GetSiteLinkPages(int siteId)
		{
			SqlCommand cmd;
			Components.LinkPage[] linkPages;
			DataSet ds;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetSiteLinkPages");
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			ds = _dbConn.GetDataSet(cmd);

			linkPages = GetLinkPagesFromDataset(ds); 

			return linkPages;

		}

		/// <summary>
		///		Retrieves a list of link pages that have relationships
		///		with the specified <see cref="Components.LinkPage"/>
		/// </summary>
		/// <param name="linkPage">
		///		The link page to find related pages for.
		/// </param>
		/// <returns>
		///		The related link pages.
		/// </returns>
		public Components.LinkPage[] GetRelatedLinkPages(Components.LinkPage linkPage)
		{
			SqlCommand cmd;
			DataSet ds;
			Components.LinkPage[] linkPages;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetRelatedLinkPages");
			cmd.Parameters.AddWithValue("@LinkPageId", linkPage.Id);

			ds = _dbConn.GetDataSet(cmd);

			linkPages = GetLinkPagesFromDataset(ds);

			return linkPages;
		}
		/// <summary>
		///		Populates the specified <see cref="LinkParagraph"/>.
		/// </summary>
		/// <param name="link">
		///		The <see cref="LinkParagraph"/> whose links will be populated.
		/// </param>
		/// <exception cref="LinkParagraphNotFoundException">
		///		Thrown if the identifier of the specified link could not be found.
		/// </exception>
		public void PopulateLinkParagraph(LinkParagraph link)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Links_GetLinkInformation";
			cmd.Parameters.AddWithValue("@LinkId", link.Id);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0)
				throw new LinkParagraphNotFoundException(link.Id);

			if (ds.Tables[0].Rows.Count == 0)
				throw new LinkParagraphNotFoundException(link.Id);

			row = ds.Tables[0].Rows[0];

			DataMapper.PopulateObject(link, row, null);
		}

		/// <summary>
		///		Populates the specified <see cref="LinkParagraphGroup"/>.
		/// </summary>
		/// <param name="linkGroup">
		///		The <see cref="LinkParagraphGroup"/> whose link group will be populated.
		/// </param>
		/// <exception cref="LinkParagraphGroupNotFoundException">
		///		Thrown if the identifier of the specified link group could not be found.
		/// </exception>
		public void PopulateLinkParagraphGroup(LinkParagraphGroup linkGroup)
		{
			SqlCommand cmd;
			DataSet ds;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetLinkGroupInformation");
			cmd.Parameters.AddWithValue("@LinkGroupId", linkGroup.Id);

			ds = _dbConn.GetDataSet(cmd);

			DataMapper.PopulateObject(linkGroup, ds, new LinkParagraphGroupNotFoundException(linkGroup.Id), null);
            linkGroup.Paragraphs = GetParagraphGroupLinks(linkGroup.Id);
		}

		/// <summary>
		///		Saves the specified <see cref="LinkParagraph"/> to the database.
		///		If the link paragraph already exists, it will be updated, otherwise
		///		it will be created.
		/// </summary>
		/// <remarks>
		///		To determine if an insert or an update will be performed, it
		///		looks at the <see cref="LinkParagraph.NewParagraph"/> property.
		/// </remarks>
		/// <param name="link">
		///		The <see cref="LinkParagraph"/> to commit to the database.
		/// </param>
		public void SaveLinkParagraph(LinkParagraph link)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Links_SaveLinkInformation";

			//If it's not new, pass in the id so it can be updated
			if (!link.NewParagraph)
				cmd.Parameters.AddWithValue("@LinkId", link.Id);

			cmd.Parameters.AddWithValue("@Title", link.Title);
			cmd.Parameters.AddWithValue("@Paragraph", link.Paragraph);
			cmd.Parameters.AddWithValue("@Enabled", link.Enabled);
			cmd.Parameters.AddWithValue("@GroupId", link.GroupId);

			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		///		Saves the specified <see cref="LinkParagraphGroup"/> to the database.
		///		If the link paragraph group already exists, it will be updated, otherwise
		///		it will be created.
		/// </summary>
		/// <remarks>
		///		To determine if an insert or an update will be performed, it
		///		looks at the <see cref="LinkParagraphGroup.NewLinkParagraphGroup"/> property.
		/// </remarks>
		/// <param name="group">
		///		The <see cref="LinkParagraphGroup"/> to commit to the database.
		/// </param>
		public void SaveLinkParagraphGroup(LinkParagraphGroup group)
		{
			SqlCommand cmd = _dbConn.GetStoredProcedureCommand("Links_SaveLinkGroupInformation");

			//If it's not new, pass in the id so it can be updated
			if (!group.NewLinkParagraphGroup)
				cmd.Parameters.AddWithValue("@GroupId", group.Id);

			cmd.Parameters.AddWithValue("@SiteId", group.SiteId);
			cmd.Parameters.AddWithValue("@Title", group.Title);
			cmd.Parameters.AddWithValue("@Distribution", group.Distribution);
			cmd.Parameters.AddWithValue("@Url1", group.Url1);
			cmd.Parameters.AddWithValue("@Url2", group.Url2);
			cmd.Parameters.AddWithValue("@AnchorText1", group.ReplacementText1);
			cmd.Parameters.AddWithValue("@AnchorText2", group.ReplacementText2);
            cmd.Parameters.AddWithValue("@Keyword1", group.Keyword1);
            cmd.Parameters.AddWithValue("@Keyword2", group.Keyword2);

			_dbConn.ExecuteNonQuery(cmd);
		}

		public void DeleteLinkParagraph(LinkParagraph link)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Links_DeleteLinkInformation";
			cmd.Parameters.AddWithValue("@LinkId", link.Id);

			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		/// Deletes the specified <see cref="LinkParagraphGroup"/> from the database.
		/// </summary>
		/// <param name="group">The id of the <see cref="LinkParagraphGroup"/> to delete.</param>
		public void DeleteLinkParagraphGroup(LinkParagraphGroup group)
		{
			SqlCommand cmd = _dbConn.GetStoredProcedureCommand("Links_RemoveLinkGroup");
			cmd.Parameters.AddWithValue("@LinkGroupId", group.Id);

			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		///		Retrieves a list of site paragraphs for the specified
		///		site identifier.
		/// </summary>
		/// <param name="groupId">
		///		The identifier of the site to retrieve the links for.
		/// </param>
		/// <returns>
		///		An array of the link paragraphs for the specified site
		///		and category.
		/// </returns>
		public LinkParagraph[] GetParagraphGroupLinks(int groupId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			LinkParagraph currLink;
			LinkParagraph[] links;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Links_GetAllParagraphGroupLinks";
			cmd.Parameters.AddWithValue("@GroupId", groupId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkParagraph[0]; //Return an empty array to avoid null errors

			links = new LinkParagraph[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new link paragraph with the Id from the database
				currLink = new LinkParagraph((int) currRow["Id"]);
				DataMapper.PopulateObject(currLink, currRow, null);
				//Add the current link to the return array
				links[i] = currLink;
			}

			return links;
		}

		/// <summary>
		///		Retrieves a list of site paragraphs for the specified
		///		site identifier.
		/// </summary>
		/// <param name="linkPageId">
		///		The identifier of the link page to retrieve the links for.
		/// </param>
		/// <returns>
		///		An array of the link paragraphs for the specified site
		///		and category.
		/// </returns>
		public LinkParagraph[] GetLinkPageArticles(int linkPageId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			LinkParagraph currLink;
			LinkParagraph[] links;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetLinkPageArticles");
			cmd.Parameters.AddWithValue("@LinkPageId", linkPageId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkParagraph[0]; //Return an empty array to avoid null errors

			links = new LinkParagraph[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new link paragraph with the Id from the database
				currLink = new LinkParagraph((int) currRow["Id"]);
				DataMapper.PopulateObject(currLink, currRow, null);
				//Add the current link to the return array
				links[i] = currLink;
			}

			return links;
		}

		/// <summary>
		///		Retrieves a list of site paragraph groups for the specified
		///		site identifier.
		/// </summary>
		/// <param name="siteId">
		///		The identifier of the site to retrieve the link pargraphs groups for.
		/// </param>
		/// <returns>
		///		An array of the link paragraph groups for the specified site.
		/// </returns>
		public LinkParagraphGroup[] GetSiteLinkParagraphGroups(int siteId)
		{
			SqlCommand cmd = _dbConn.GetStoredProcedureCommand("Links_GetAllSiteParagraphGroups");

			cmd.Parameters.AddWithValue("@SiteId", siteId);

			DataSet ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkParagraphGroup[0];

			LinkParagraphGroup[] groups = new LinkParagraphGroup[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				DataRow currRow = ds.Tables[0].Rows[i];
				LinkParagraphGroup currGroup = new LinkParagraphGroup((int) currRow["Id"]);
				DataMapper.PopulateObject(currGroup, currRow, null);
				currGroup.Paragraphs = GetParagraphGroupLinks(currGroup.Id);
				groups[i] = currGroup;
			}

			return groups;
		}

		/// <summary>
		///		Retrieves a list of site paragraphs that should be linked
		///		from the specified site in the specified category.
		/// </summary>
		/// <param name="siteId">
		///		The identifier of the site to retrieve the links for.
		/// </param>
		/// <param name="categoryId">
		///		The identifier of the category to retrieve the sites for.
		/// </param>
		/// <returns>
		///		An array of the link paragraphs for the specified site
		///		and category.
		/// </returns>
		public LinkParagraph[] GetCategoryLinks(int siteId, int categoryId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			LinkParagraph currLink;
			LinkParagraph[] links;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Links_GetCategoryLinks";
			cmd.Parameters.AddWithValue("@SiteId", siteId);
			cmd.Parameters.AddWithValue("@CategoryId", categoryId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkParagraph[0]; //Return an empty array to avoid null errors

			links = new LinkParagraph[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new link paragraph with the Id from the database
				currLink = new LinkParagraph((int) currRow["Id"]);
				DataMapper.PopulateObject(currLink, currRow, null);
				//Add the current link to the return array
				links[i] = currLink;
			}

			return links;
		}

		/// <summary>
		///		Add a link to the specified site using the standard
		///		algorithm for adding links.
		/// </summary>
		/// <param name="s">
		///		The site to add an incoming link to.
		/// </param>
		public void AddLink(Site s)
		{
			SqlCommand cmd;

			if (s == null)
			{
				_log.Warn("NULL Site Passed Into 'AddLink' To Add A Link, No Links Were Added");
				return;
			}

			cmd = _dbConn.GetStoredProcedureCommand("Links_AddLink");
			cmd.Parameters.AddWithValue("@SiteId", s.Id);
			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		///		Gets a list of sites that are ready to have a
		///		maintenance cycle run on them.
		/// </summary>
		/// <returns></returns>
		public Site[] GetSitesForLinkProcessing()
		{
			SqlCommand cmd;
			DataSet ds;
			Site[] sites;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetSitesForLinkProcessing");
			ds = _dbConn.GetDataSet(cmd);

			sites = GetSitesFromDataset(ds);

			return sites;
		}


		/// <summary>
		///		Calls a stored procedure to update all the related categories
		///		for all of the link pages for the specified site.
		/// </summary>
		/// <param name="siteId">
		///		The site to update the related categories for.
		/// </param>
		public void UpdateSiteRelatedCategories(int siteId)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetStoredProcedureCommand("Categories_UpdateSiteRelatedCategories");
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			_dbConn.ExecuteNonQuery(cmd);
		}


		/// <summary>
		///		Retrieves the related categories that are mapped
		///		to the specified link page.
		/// </summary>
		/// <param name="linkPageId"></param>
		/// <returns></returns>
		public LinkCategory[] GetLinkPageRelatedCategories(int linkPageId)
		{
			SqlCommand cmd;
			DataSet ds;
			LinkCategory[] categories;
			DataRow currRow;
			LinkCategory currCategory;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetLinkPageRelatedCategories");
			cmd.Parameters.AddWithValue("@LinkPageId", linkPageId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkCategory[0];

			categories = new LinkCategory[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				currCategory = new LinkCategory((int) currRow["Id"]);
				DataMapper.PopulateObject(currCategory, currRow, currCategory.ExtendedProperties);
				categories[i] = currCategory;
			}

			return categories;
		}


		/// <summary>
		///		Gets the link page for the specified domain for the
		///		specified category.
		/// </summary>
		/// <param name="linkPageName">
		///		The name of the link page to retrieve.
		/// </param>
		/// <returns></returns>
		public Components.LinkPage GetLinkPageForClient(Guid siteGuid, string linkPageName)
		{
			SqlCommand cmd;
			DataSet ds;
			Components.LinkPage lp;

			cmd = _dbConn.GetStoredProcedureCommand("Links_GetLinkPageIdForClient");
			cmd.Parameters.AddWithValue("@SiteGuid", siteGuid);
			cmd.Parameters.AddWithValue("@LinkPageName", linkPageName);

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return null;

			lp = new Components.LinkPage();
			DataMapper.PopulateObject(lp, ds, null, null);

			return lp;
		}

        /// <summary>
        ///     Gets the number of incoming links that the site has.
        /// </summary>
        /// <param name="siteId">
        ///     The site to count links for.
        /// </param>
        /// <returns>
        ///     The number of incoming links.
        /// </returns>
        public int GetIncomingLinkCount(int siteId)
        {
            SqlCommand cmd;
            DataSet ds;

            cmd = _dbConn.GetStoredProcedureCommand("Sites_GetIncomingLinkCount");
            cmd.Parameters.AddWithValue("@SiteId", siteId);

            ds = _dbConn.GetDataSet(cmd);

            return (int)ds.Tables[0].Rows[0][0];
        }
        
		/// <summary>
		///		Saves the specified <see cref="LinkPageStatus" /> to the database.
		/// </summary>
		/// <param name="LinkPageStatus">
		///		The <see cref="LinkPageStatus" /> to save to the database.
		/// </param>
		public void SaveLinkPageStatus(LinkPageStatus status)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("Links_SaveLinkPageStatus");

            if (status.HasId)
                cmd.Parameters.AddWithValue("@StatusId", status.Id);

            cmd.Parameters.AddWithValue("@CheckedOn", status.CheckedOn);
            cmd.Parameters.AddWithValue("@Valid", status.Valid);
            cmd.Parameters.AddWithValue("@SiteId", status.SiteId);

			//Set up the parameter to retrieve the return value, which is the ID in the database
			returnParam = cmd.Parameters.Add("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			//If we did an insert, grab the new id
            if (!status.HasId)
                status.Id = (int)returnParam.Value;
		}

        
        /// <summary>
	    ///   Retreives the last <see cref="LinkPageStatus" /> for the site.
	    /// </summary>
        /// <param name="siteId">
        ///     The ID of the site to get the status for.
        /// </param>
        /// <returns>
        ///     The last status of the site from when it was last spidered.
        /// </returns>
        public LinkPageStatus GetCurrentLinkPageStatus(int siteId)
        {
            SqlCommand cmd;
            DataSet ds;
            LinkPageStatus getObj;

            cmd = _dbConn.GetStoredProcedureCommand("Links_GetLinkPageStatus");
            cmd.Parameters.AddWithValue("@SiteId", siteId);

            ds = _dbConn.GetDataSet(cmd);

            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;

            getObj = new LinkPageStatus();
            DataMapper.PopulateObject(getObj, ds, null, null);

            return getObj;
        }
	

		#endregion

		#region Sites

		/// <summary>
		///		Populates the specified <see cref="Site"/>.
		/// </summary>
		/// <param name="site">
		///		The <see cref="Site"/> that should be populated.
		/// </param>
		/// <exception cref="SiteNotFoundException">
		///		Thrown when the the identifier of the site could not be
		///		found in the database.
		/// </exception>
		public void PopulateSite(Site site)
		{
			SqlCommand cmd;
			DataSet ds;
			SiteNotFoundException noDataException;

			cmd = _dbConn.GetStoredProcedureCommand("Sites_GetSiteInformation");
			cmd.Parameters.AddWithValue("@SiteId", site.Id);

			ds = _dbConn.GetDataSet(cmd);

			noDataException = new SiteNotFoundException(site.Id);

			DataMapper.PopulateObject(site, ds, noDataException, null);
		}

        /// <summary>
        ///     Gets a list of all the sites that need to have their link
        ///     pages verified to make sure they are valid.
        /// </summary>
        /// <returns></returns>
        public Site[] GetSitesForLinkPageCheck()
        {
            SqlCommand cmd;
            DataSet ds;
            object[] objArr;
            Site[] sites;

            cmd = _dbConn.GetStoredProcedureCommand("Sites_GetSitesForLinkPageCheck");
            ds = _dbConn.GetDataSet(cmd);
            objArr = DataMapper.CreateObjects(ds, typeof(Site));
            sites = new Site[objArr.Length];
            objArr.CopyTo(sites, 0);

            return sites;
        }

		/// <summary>
		///		Saves the specified <see cref="Site"/> into the database.
		/// </summary>
		/// <param name="site">The <see cref="Site"/> that should be saved.</param>
		/// <remarks>Does not handle new sites.</remarks>
		public void SaveSite(Site site)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("Sites_SaveSite");
			
			if(!site.CreateNew)
				cmd.Parameters.AddWithValue("@SiteId", site.Id);

			cmd.Parameters.AddWithValue("@Name", site.Name);
			cmd.Parameters.AddWithValue("@UserId", site.UserId);
			cmd.Parameters.AddWithValue("@Url", site.Url);
			cmd.Parameters.AddWithValue("@Enabled", site.Enabled);
			cmd.Parameters.AddWithValue("@InitialCategoryId", site.InitialCategoryId);
			cmd.Parameters.AddWithValue("@PageTemplate", site.PageTemplate);
            cmd.Parameters.AddWithValue("@LinkPageUrl", site.LinkPageUrl);
						cmd.Parameters.AddWithValue("@HideInitialSetupMessage", site.HideInitialSetupMessage);

			if(site.StartLinkPageId != 0)
				cmd.Parameters.AddWithValue("@StartLinkPageId", site.StartLinkPageId);

            if(site.UpgradeFlag != 0)
                cmd.Parameters.AddWithValue("@UpgradeFlag", site.UpgradeFlag);

			//Return value
			returnParam = cmd.Parameters.AddWithValue("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			if(site.CreateNew)
			{
				site.Id = (int)returnParam.Value;
				site.CreateNew = false;
			}
		}

		
		private Site[] GetSitesFromDataset(DataSet ds)
		{
			DataRow currRow;
			Site currSite;
			Site[] sites;

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new Site[0]; //Return an empty array to avoid null errors

			sites = new Site[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new site with the Id from the database
				currSite = new Site((int) currRow["Id"]);
				DataMapper.PopulateObject(currSite, currRow, null);
				//Add the current link to the return array
				sites[i] = currSite;
			}

			return sites;
		}


		/// <summary>
		///		Retrieves a parameter for the specified site.
		/// </summary>
		/// <param name="siteId">
		///		The site to find the setting for.
		/// </param>
		/// <param name="parameterId">
		///		The parameter to load for the site.
		/// </param>
		/// <returns>
		///		The value of the parameter from the database.
		/// </returns>
		/// <exception cref="ApplicationException">
		///		Thrown if the value cannot be found, and there is no default.
		///	</exception>
		public int GetIntSiteSetting(int siteId, int parameterId)
		{
			SqlCommand cmd;
			DataSet ds;
			
			cmd = _dbConn.GetSqlCommand();
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = "Select dbo.Sites_GetIntSiteSetting(@SiteId, @ParameterId)";

			cmd.Parameters.AddWithValue("@SiteId", siteId);
			cmd.Parameters.AddWithValue("@ParameterId", parameterId);

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				throw new ApplicationException("The value could not be found"); 
			}

			return (int)ds.Tables[0].Rows[0][0];
		}

		/// <summary>
		///		Saves a site setting to the database.
		/// </summary>
		/// <param name="siteId">
		///		The id of the site to save the setting for.
		/// </param>
		/// <param name="parameterId">
		///		The id of the parameter you are setting.
		/// </param>
		/// <param name="settingValue">
		///		The value to save.
		/// </param>
		public void SaveIntSiteSetting(int siteId, int parameterId, int settingValue)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetStoredProcedureCommand("Sites_SaveIntSetting");
			cmd.Parameters.AddWithValue("@SiteId", siteId);
			cmd.Parameters.AddWithValue("@ParameterId", parameterId);
			cmd.Parameters.AddWithValue("@Value", settingValue);

			_dbConn.ExecuteNonQuery(cmd);
		}

        /// <summary>
        ///		Marks the specified <see cref="Site"/> for deletion.
        /// </summary>
        /// <param name="site">The <see cref="Site"/> that should be marked for deletion.</param>
        public void DeleteSite(int siteId)
        {
            SqlCommand cmd;
            cmd = _dbConn.GetStoredProcedureCommand("Sites_DeleteSite");
            cmd.Parameters.AddWithValue("@SiteId", siteId);
            _dbConn.ExecuteNonQuery(cmd);
        }
		#endregion

		#region Categories

		/// <summary>
		///		Populates the specified <see cref="LinkCategory"/>.
		/// </summary>
		/// <param name="category">
		///		The <see cref="LinkCategory"/> that should be populated.
		/// </param>
		/// <exception cref="LinkCategoryNotFoundException">
		///		Thrown when the the identifier of the category could not be
		///		found in the database.
		/// </exception>
		public void PopulateLinkCategory(LinkCategory category)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Categories_GetCategoryInformation";
			cmd.Parameters.AddWithValue("@CategoryId", category.Id);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0)
				throw new LinkCategoryNotFoundException(category.Id);

			if (ds.Tables[0].Rows.Count == 0)
				throw new LinkCategoryNotFoundException(category.Id);

			row = ds.Tables[0].Rows[0];

			DataMapper.PopulateObject(category, row, null);
		}


		/// <summary>
		///	Gets the list of categories from the database.
		/// </summary>
		/// <returns>An array of <see cref="LinkCategory"/> objects.</returns>
		public LinkCategory[] GetSiteCategories()
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;
			LinkCategory[] categories;
			LinkCategory newCategory;

			cmd = _dbConn.GetStoredProcedureCommand("Categories_GetSiteCategories");

			ds = _dbConn.GetDataSet(cmd);

			categories = new LinkCategory[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				row = ds.Tables[0].Rows[i];

				newCategory = new LinkCategory((int) row["Id"]);
				DataMapper.PopulateObject(newCategory, row, null);

				categories[i] = newCategory;
			}

			return categories;
		}

		/// <summary>
		///		Gets an array of link <see cref="LinkCategory"/> objects.
		/// </summary>
		/// <param name="parentCategory">
		///		The parent category to get child categories for.
		/// </param>
		public LinkCategory[] GetLinkCategories(LinkCategory parentCategory)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;
			LinkCategory newCategory;
			LinkCategory[] categories;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Categories_GetChildCategories";

			if (parentCategory != null)
				cmd.Parameters.AddWithValue("@ParentCategoryId", parentCategory.Id);

			ds = _dbConn.GetDataSet(cmd);

			categories = new LinkCategory[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				row = ds.Tables[0].Rows[i];

				newCategory = new LinkCategory((int) row["Id"]);
				DataMapper.PopulateObject(newCategory, row, null);

				categories[i] = newCategory;
			}

			return categories;
		}


		/// <summary>
		///		Adds a new relationship between 2 categories.
		/// </summary>
		public void AddCategoryRelationship(int from, int to)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetSqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Categories_AddCategoryRelationship";

			//Add all the parameters
			cmd.Parameters.AddWithValue("@FromCategoryId", from);
			cmd.Parameters.AddWithValue("@ToCategoryId", to);

			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		///		Deletes the relationship between 2 categories.
		/// </summary>
		public void DeleteCategoryRelationship(int from, int to)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetSqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Categories_DeleteCategoryRelationship";

			//Add all the parameters
			cmd.Parameters.AddWithValue("@FromCategoryId", from);
			cmd.Parameters.AddWithValue("@ToCategoryId", to);

			_dbConn.ExecuteNonQuery(cmd);
		}


		/// <summary>
		///		Retrieves a list of categories that are related
		///		to the specified category for various reasons.
		/// </summary>
		/// <remarks>
		///		Categories can be related by sibling relationships,
		///		parent/child relationships, or explicit relationships.
		/// </remarks>
		/// <param name="categoryId">
		///		The category to find related categories for.
		/// </param>
		/// <returns>
		///		Categories that are related to the specified category.
		/// </returns>
		public LinkCategory[] GetAllRelatedCategories(int categoryId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			LinkCategory currCategory;
			LinkCategory[] categories;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Categories_GetAllRelatedCategories";
			cmd.Parameters.AddWithValue("@CategoryId", categoryId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkCategory[0]; //Return an empty array to avoid null errors

			categories = new LinkCategory[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new link paragraph with the Id from the database
				currCategory = new LinkCategory((int) currRow["Id"]);
				DataMapper.PopulateObject(currCategory, currRow, null);
				//Add the current link to the return array
				categories[i] = currCategory;
			}

			return categories;
		}

		/// <summary>
		///		Gets a list of the explicitly related categories from
		///		the CategoryRelations table.
		/// </summary>
		/// <remarks>
		///		This is used to view only the explicit relationships between
		///		categories.  If you need to generate related categories for
		///		a content page, use <see cref="GetAllRelatedCategories"/>.
		/// </remarks>
		/// <param name="categoryId">
		///		The category to find related categories for.
		/// </param>
		/// <returns>
		///		Categories that are related to the specified category.
		/// </returns>
		public LinkCategory[] GetRelatedCategories(int categoryId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow currRow;
			LinkCategory currCategory;
			LinkCategory[] categories;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Categories_GetRelatedCategories";
			cmd.Parameters.AddWithValue("@CategoryId", categoryId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new LinkCategory[0]; //Return an empty array to avoid null errors

			categories = new LinkCategory[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];
				//Create a new link paragraph with the Id from the database
				currCategory = new LinkCategory((int) currRow["Id"]);
				DataMapper.PopulateObject(currCategory, currRow, null);
				//Add the current link to the return array
				categories[i] = currCategory;
			}

			return categories;
		}


		#endregion
		
		#region Users

		/// <summary>
		///		Gets all of the users in the database that
		///		are currently enabled.
		/// </summary>
		/// <returns></returns>
		public User[] GetAllEnabledUsers()
		{
			SqlCommand cmd;
			DataSet ds;
			User[] users;
			User currUser;
			DataRow currRow;

			cmd = _dbConn.GetStoredProcedureCommand("Users_GetAllEnabledUsers");
			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new User[0]; //Return an empty array to avoid null errors

			users = new User[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];

				//Create a new user with the Id from the database
				currUser = new User((int) currRow["Id"]);
				DataMapper.PopulateObject(currUser, currRow, null);
				//Add the current user to the return array
				users[i] = currUser;
			}

			return users;
		}


		/// <summary>
		///		Retrieves all of the sites for the specified
		///		user identifier.
		/// </summary>
		/// <param name="userId"></param>
		public Site[] GetUsersSites(int userId, bool fullyConfiguredSitesOnly)
		{
			SqlCommand cmd;
			DataSet ds;
			Site[] sites;

			cmd = _dbConn.GetStoredProcedureCommand("Sites_GetUsersSites");
			cmd.Parameters.AddWithValue("@UserId", userId);
			cmd.Parameters.AddWithValue("@FullyConfiguredSitesOnly", fullyConfiguredSitesOnly);

			ds = _dbConn.GetDataSet(cmd);

			sites = GetSitesFromDataset(ds);

			return sites;
		}

		public bool UserExists(string email)
		{
			SqlCommand cmd;
			DataSet ds;

			cmd = _dbConn.GetStoredProcedureCommand("Users_UserExists");
			cmd.Parameters.AddWithValue("@EmailAddress", email);

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
			{
				throw new ApplicationException("The value could not be found"); 
			}

			return (int)ds.Tables[0].Rows[0][0] == 1 ? true : false;
		}

		/// <summary>
		///		Checks the users credentials against the list of valid users.
		/// </summary>
		/// <returns>
		///		A positive integer that is the unique identifier for the user if
		///		the username and password are valid.  Otherwise, -1 is returned.
		/// </returns>
		public void AuthenticateUser(User authUser)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow userRow;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Users_LoginUser";
			cmd.Parameters.AddWithValue("@EmailAddress", authUser.EmailAddress);
			cmd.Parameters.AddWithValue("@Password", authUser.Password);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
			{
				authUser.Authenticated = false;
				return;
			}

			userRow = ds.Tables[0].Rows[0];
			authUser.Authenticated = true;

			//Populate the other fields
			DataMapper.PopulateObject(authUser, userRow, null);
		}

        /// <summary>
        ///		Gets a user from the database based on their email address.
        /// </summary>
        /// <returns>
        ///		The user with the specified email address, or NULL if a user
        ///     with that email address does not exist.
        /// </returns>
        public User GetUserByEmail(string emailAddress)
        {
            SqlCommand cmd;
            DataSet ds;
            User u;

            cmd = _dbConn.GetStoredProcedureCommand("Users_GetUserByEmail");
            cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);

            ds = _dbConn.GetDataSet(cmd);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            u = new User();
            DataMapper.PopulateObject(u, ds, null, null);

            return u;
        }

		/// <summary>
		///		Populates the specified <see cref="User"/>.
		/// </summary>
		/// <param name="user">
		///		The <see cref="User"/> that should be populated.
		/// </param>
		/// <exception cref="UserNotFoundException">
		///		Thrown when the the identifier of the site could not be
		///		found in the database.
		/// </exception>
		public void PopulateUser(User user)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Users_GetUserInfo";
			cmd.Parameters.AddWithValue("@UserId", user.Id);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0)
				throw new UserNotFoundException(user.Id);

			if (ds.Tables[0].Rows.Count == 0)
				throw new UserNotFoundException(user.Id);

			row = ds.Tables[0].Rows[0];

			DataMapper.PopulateObject(user, row, null);
		}


		/// <summary>
		///		Saves the specified <see cref="User"/> object to the database.
		/// </summary>
		/// <param name="user">
		///		The user to persist in the database.
		/// </param>
		public void SaveUser(User user)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("Users_SaveUser");

			if(!user.CreateNew)
				cmd.Parameters.AddWithValue("@UserId", user.Id);

			cmd.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
			cmd.Parameters.AddWithValue("@Password", user.Password);
			cmd.Parameters.AddWithValue("@AccountType", user.AccountType);
			cmd.Parameters.AddWithValue("@Enabled", user.Enabled);
			cmd.Parameters.AddWithValue("@Name", user.Name);
			cmd.Parameters.AddWithValue("@ReferrerId", user.ReferrerId);

            if(user.LeadId > 0)
                cmd.Parameters.AddWithValue("@LeadId", user.LeadId);

			if(user.CreatedOn != new DateTime())
				cmd.Parameters.AddWithValue("@CreatedOn", user.CreatedOn);

			if(user.LastLogin != new DateTime())
				cmd.Parameters.AddWithValue("@LastLogin", user.LastLogin);

			//Return value
			returnParam = cmd.Parameters.AddWithValue("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			if(user.CreateNew)
			{
				user.Id = (int)returnParam.Value;
				user.CreateNew = false;
			}
		}

		#endregion

		#region FTP Uploads

		/// <summary>
		///		Gets the next FTP upload job that should be processed
		///		and uploaded to the client.
		/// </summary>
		/// <param name="intervalSinceLast">
		///		The amount of time to wait since the last run.  For example,
		///		if you specify 12 hours, you will get all sites that have
		///		been updated more than 12 hours ago.
		/// </param>
		public FtpUploadInfo GetNextFtpUploadInfo(TimeSpan intervalSinceLast)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;
			FtpUploadInfo ftpInfo;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Sites_GetNextFtpUploadInfo";
			cmd.Parameters.AddWithValue("@CurrentTime", DateTime.UtcNow);
			cmd.Parameters.AddWithValue("@IntervalMS", intervalSinceLast.TotalMilliseconds);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0)
				return null;

			if (ds.Tables[0].Rows.Count == 0)
				return null;

			row = ds.Tables[0].Rows[0];

			ftpInfo = new FtpUploadInfo((int) row["Id"]);
			DataMapper.PopulateObject(ftpInfo, row, null);

			return ftpInfo;
		}

		/// <summary>
		///		Gets the FTP upload information for the specified site.
		/// </summary>
		public FtpUploadInfo GetFtpUploadInfo(int siteId)
		{
			SqlCommand cmd;
			DataSet ds;
			DataRow row;
			FtpUploadInfo ftpInfo;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Sites_GetFtpUploadInfo";
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0)
				return null;

			if (ds.Tables[0].Rows.Count == 0)
				return null;

			row = ds.Tables[0].Rows[0];

			ftpInfo = new FtpUploadInfo((int) row["Id"]);
			DataMapper.PopulateObject(ftpInfo, row, null);

			return ftpInfo;
		}

		/// <summary>
		///		Saves the specified FTP information to the databas.
		/// </summary>
		/// <remarks>
		///		To determine if an insert or an update will be performed, it
		///		looks at the <see cref="FtpUploadInfo.CreatedNew"/> property.
		/// </remarks>
		/// <param name="ftpInfo">
		///		The <see cref="FtpUploadInfo"/> to commit to the database.
		/// </param>
		public void SaveFtpUploadInfo(FtpUploadInfo ftpInfo)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Sites_SaveFtpUploadInfo";

			//If it's not new, pass in the id so it can be updated
			if (!ftpInfo.CreatedNew)
				cmd.Parameters.AddWithValue("@FtpInfoId", ftpInfo.Id);

			cmd.Parameters.AddWithValue("@SiteId", ftpInfo.SiteId);
			cmd.Parameters.AddWithValue("@Enabled", ftpInfo.Enabled);
			cmd.Parameters.AddWithValue("@Url", ftpInfo.Url);
			cmd.Parameters.AddWithValue("@Username", ftpInfo.UserName);
			cmd.Parameters.AddWithValue("@Password", ftpInfo.Password);
			cmd.Parameters.AddWithValue("@FtpPath", ftpInfo.FtpPath);
            cmd.Parameters.AddWithValue("@ActiveMode", ftpInfo.ActiveMode);

			if(ftpInfo.LastUpload == new DateTime())
				cmd.Parameters.AddWithValue("@LastUpload", null);
			else
				cmd.Parameters.AddWithValue("@LastUpload", ftpInfo.LastUpload);

			if(ftpInfo.DailyUploadTime == new DateTime())
				cmd.Parameters.AddWithValue("@DailyUploadTime", null);
			else
				cmd.Parameters.AddWithValue("@DailyUploadTime", ftpInfo.DailyUploadTime);

			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		///		Marks FTP upload information as being processed.  This will
		///		move it to the bottom of the list to be processed.
		/// </summary>
		public void SetFtpUploadInfoProcessed(int id, DateTime lastUpload)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetSqlCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = "Sites_SetFtpInfoProcessed";
			cmd.Parameters.AddWithValue("@Id", id);
			cmd.Parameters.AddWithValue("@LastUpload", lastUpload);

			_dbConn.ExecuteNonQuery(cmd);
		}

        /// <summary>
        ///     Gets all of the sites that are configured to upload
        ///     their link pages using FTP.
        /// </summary>
        /// <returns></returns>
        public Site[] GetAllFtpSites()
        {
            SqlCommand cmd;
            DataSet ds;
            object[] objArr;
            Site[] sites;

            cmd = _dbConn.GetStoredProcedureCommand("Sites_GetAllFtpSites");
            ds = _dbConn.GetDataSet(cmd);

            objArr = DataMapper.CreateObjects(ds, typeof(Site));
            sites = new Site[objArr.Length];
            objArr.CopyTo(sites, 0);

            return sites;
        }


		#endregion

		#region RSS Feeds

		/// <summary>
		///		Retrieves the RSS feeds that need to have their
		///		feed items updated.
		/// </summary>
		/// <returns></returns>
		public RssFeed[] GetFeedsToUpdate()
		{
			SqlCommand cmd;
			DataSet ds;
			RssFeed[] feeds;
			RssFeed currFeed;
			DataRow currRow;

			cmd = _dbConn.GetStoredProcedureCommand("Rss_GetFeedsToUpdate");
			ds = _dbConn.GetDataSet(cmd);

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new RssFeed[0]; //Return an empty array to avoid null errors

			feeds = new RssFeed[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];

				//Create a new feed with the Id from the database
				currFeed = new RssFeed((int) currRow["Id"]);
				DataMapper.PopulateObject(currFeed, currRow, null);
				//Add the current user to the return array
				feeds[i] = currFeed;
			}

			return feeds;
		}

		private static RssFeedItem[] getRssItemsFromDataSet(DataSet ds)
		{
			RssFeedItem[] items;
			RssFeedItem currItem;
			DataRow currRow;

			if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return new RssFeedItem[0]; //Return an empty array to avoid null errors

			items = new RssFeedItem[ds.Tables[0].Rows.Count];

			for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
			{
				currRow = ds.Tables[0].Rows[i];

				//Create a new feed with the Id from the database
				currItem = new RssFeedItem((int) currRow["Id"]);
				DataMapper.PopulateObject(currItem, currRow, null);
				//Add the current user to the return array
				items[i] = currItem;
			}

			return items;
		}

		/// <summary>
		///		Saves the RSS feed item to the RSS item cache.
		/// </summary>
		/// <param name="feedId">
		///		The ID of the RSS feed that this feed item belongs to.
		/// </param>
		/// <param name="rssItems">
		///		The RSS items to save.
		/// </param>
		public void SaveRssFeedItems(int feedId, RssItemCollection rssItems)
		{
			SqlCommand cmd;
			RssItem currItem;
			SqlParameter parameter;
			int revision = 0;

			for (int i = 0; i < rssItems.Count; i++)
			{
				currItem = rssItems[i];

				cmd = _dbConn.GetStoredProcedureCommand("Rss_SaveRssFeedItem");
				cmd.Parameters.AddWithValue("@FeedId", feedId);
				cmd.Parameters.AddWithValue("@ItemTitle", currItem.Title);
				cmd.Parameters.AddWithValue("@ItemText", currItem.Description);
				cmd.Parameters.AddWithValue("@ReadTime", DateTime.UtcNow);

				if (i == 0)
				{
					parameter = cmd.Parameters.AddWithValue("@Revision", SqlDbType.Int);
					parameter.Direction = ParameterDirection.Output;

					_dbConn.ExecuteNonQuery(cmd);

					revision = (int) parameter.Value;
				}
				else
				{
					cmd.Parameters.AddWithValue("@Revision", revision);
					_dbConn.ExecuteNonQuery(cmd);
				}
			}
		}

		/// <summary>
		///		Loops through all the link pages in the specified
		///		site and chooses new RSS feed items to map.
		/// </summary>
		/// <param name="siteId"></param>
		public void UpdateSiteFeeds(int siteId)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetStoredProcedureCommand("Rss_UpdateSiteFeeds");
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			_dbConn.ExecuteNonQuery(cmd);
		}


		/// <summary>
		///		Retrieves the RSS items for the specified link page.
		/// </summary>
		/// <param name="linkPageId">
		///		The link page to retrieve the items for.
		/// </param>
		public RssFeedItem[] GetRssItemsForLinkPage(int linkPageId)
		{
			SqlCommand cmd;
			DataSet ds;
			RssFeedItem[] items;

			cmd = _dbConn.GetStoredProcedureCommand("Rss_GetItemsForLinkPage");
			cmd.Parameters.AddWithValue("@LinkPageId", linkPageId);
			ds = _dbConn.GetDataSet(cmd);
			items = getRssItemsFromDataSet(ds);

			return items;
		}

		#endregion

		#region Payments & Subscriptions

		/// <summary>
		///		Get the active link package  <see cref="Subscription"/> for
		///		the specified site.
		/// </summary>
		/// <param name="siteId">
		///		The ID of the site to get the subscription for.
		/// </param>
		/// <returns>
		///		The link package subscription for the site if it is currently active,
		///		otherwise NULL.
		///	</returns>
		public Subscription GetSiteSubscription(int siteId)
		{
			SqlCommand cmd;
			DataSet ds;
			Subscription subscription;

			cmd = _dbConn.GetStoredProcedureCommand("LinkPackages_GetSitesLinkPackages");
			cmd.Parameters.AddWithValue("@SiteId", siteId);

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return null;

			subscription = new Subscription();
			DataMapper.PopulateObject(subscription, ds, null, null);
			
			return subscription;		
		}

		public void SaveSubscription(Subscription s)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("Subscriptions_SaveSubscription");

			if(s.IdSet)
				cmd.Parameters.AddWithValue("@SubscriptionId", s.Id);

			cmd.Parameters.AddWithValue("@SiteId", s.SiteId);
			cmd.Parameters.AddWithValue("@PlanId", s.PlanId);

			if(s.StartTime != new DateTime())
				cmd.Parameters.AddWithValue("@StartTime", s.StartTime);

			if(s.EndTime != new DateTime())
				cmd.Parameters.AddWithValue("@EndTime", s.EndTime);

			//Return value
			returnParam = cmd.Parameters.AddWithValue("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			//If we did an insert, grab the new id
			if(!s.IdSet)
				s.Id = (int)returnParam.Value;
		}

		/// <summary>
		///		Retreives the <see cref="LinkPackage"/> with the
		///		specified database ID
		/// </summary>
		public LinkPackage GetLinkPackage(int linkPackageId)
		{
			SqlCommand cmd;
			DataSet ds;
			LinkPackage linkPackage;

			cmd = _dbConn.GetStoredProcedureCommand("LinkPackages_GetLinkPackage");
			cmd.Parameters.AddWithValue("@LinkPackageId", linkPackageId);

			ds = _dbConn.GetDataSet(cmd);

			linkPackage = new LinkPackage();
			DataMapper.PopulateObject(linkPackage, ds, null, null);

			return linkPackage;
		}

		/// <summary>
		///		Gets all of the link packages from the database.
		/// </summary>
		/// <returns>
		///		An array of <see cref="LinkPackage"/> objects for the
		///		link packages in the database.
		/// </returns>
		public LinkPackage[] GetAllLinkPackages()
		{
			SqlCommand cmd;
			DataSet ds;
			object[] objArr;
			LinkPackage[] linkPackages;

			cmd = _dbConn.GetStoredProcedureCommand("LinkPackages_GetAllLinkPackages");
			ds = _dbConn.GetDataSet(cmd);
			objArr = DataMapper.CreateObjects(ds, typeof(LinkPackage)); 
			linkPackages = new LinkPackage[objArr.Length];
			objArr.CopyTo(linkPackages, 0);

			return linkPackages;
		}

		/// <summary>
		///		Saves the specified payment to the database.
		/// </summary>
		/// <param name="p">
		///		The payment to save.
		/// </param>
		public void SavePayment(Payment p)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("Payments_SavePayment");

			if(p.HasId)
				cmd.Parameters.AddWithValue("@PaymentId", p.Id);

			cmd.Parameters.AddWithValue("@Amount", p.Amount);
			cmd.Parameters.AddWithValue("@Applied", p.Applied);
			cmd.Parameters.AddWithValue("@PostData", p.PostData);
			cmd.Parameters.AddWithValue("@SubscriptionTransactionId", p.SubscriptionTransactionId);
			cmd.Parameters.AddWithValue("@PayPal_SubscriptionId", p.PayPal_SubscriptionId);
			cmd.Parameters.AddWithValue("@PayPal_VerifySign", p.PayPal_VerifySign);
			cmd.Parameters.AddWithValue("@PayPal_TransactionId", p.PayPal_TransactionId);
			cmd.Parameters.AddWithValue("@PayPal_PayerId", p.PayPal_PayerId);
			cmd.Parameters.AddWithValue("@PayPal_Fee", p.PayPal_Fee);
			cmd.Parameters.AddWithValue("@PayPal_PayerEmail", p.PayPal_PayerEmail);
			
			//Return value
			returnParam = cmd.Parameters.AddWithValue("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			//If we did an insert, grab the new id
			if(!p.HasId)
				p.Id = (int)returnParam.Value;
		}

		/// <summary>
		///		Retrieves the payment with the specified payment Id.
		/// </summary>
		/// <param name="paymentId"></param>
		/// <returns></returns>
		public Payment GetPayment(int paymentId)
		{
			SqlCommand cmd;
			DataSet ds;
			Payment p;

			cmd = _dbConn.GetStoredProcedureCommand("Payments_GetPayment");
			cmd.Parameters.AddWithValue("@PaymentId", paymentId);

			ds = _dbConn.GetDataSet(cmd);

			p = new Payment();
			DataMapper.PopulateObject(p, ds, null, null);

			return p;
		}

		/// <summary>
		///		Saves the transaction to the database.
		/// </summary>
		/// <param name="st">
		///		The <see cref="SubscriptionTransaction"/> to save to the database.
		/// </param>
		public void SaveSubscriptionTransaction(SubscriptionTransaction st)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("Subscriptions_SaveTransaction");

			returnParam =	cmd.Parameters.AddWithValue("@SubscriptionId", st.GuidId);
			if(!st.HasId)
				returnParam.Direction = ParameterDirection.Output;

			cmd.Parameters.AddWithValue("@SiteId", st.SiteId);
			cmd.Parameters.AddWithValue("@PlanId", st.PlanId);
			cmd.Parameters.AddWithValue("@Processed", st.Processed);
			cmd.Parameters.AddWithValue("@PaymentAmount", st.PaymentAmount);
			cmd.Parameters.AddWithValue("@PaymentInterval", st.PaymentInterval);

			_dbConn.ExecuteNonQuery(cmd);

			//If we did an insert, grab the new id
			if(!st.HasId)
				st.GuidId = (Guid)returnParam.Value;
		}

		/// <summary>
		///		Retrieves the <see cref="SubscriptionTransaction"/> from the database
		///		that has the specified transaction key.
		/// </summary>
		/// <param name="subscriptionId">
		///		The unique GUID that identifies the subscription to retrieve.
		/// </param>
		/// <returns>
		///		A <see cref="SubscriptionTransaction"/> with the specified ID, or
        ///     NULL if a transaction with that ID cannot be found.
		/// </returns>
		public SubscriptionTransaction GetSubscriptionTransaction(string subscriptionId)
		{
			SqlCommand cmd;
			DataSet ds;
			SubscriptionTransaction st;

			cmd = _dbConn.GetStoredProcedureCommand("Subscriptions_GetTransaction");
			cmd.Parameters.AddWithValue("@SubscriptionId", subscriptionId);

			ds = _dbConn.GetDataSet(cmd);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

			st = new SubscriptionTransaction();
			DataMapper.PopulateObject(st, ds, null, null);

			return st;
		}

        /// <summary>
        ///     Determines if the specified site is eligible for a multi-site discount for
        ///     the account level requested.
        /// </summary>
        /// <param name="siteId">
        ///     The ID of the site to check for eligibility.
        /// </param>
        /// <param name="requestedLevel">
        ///     The plan ID to check for discount eligibility.
        /// </param>
        /// <returns>
        ///     True if the site is eligible for a multi-site discount,
        ///     otherwise false.
        /// </returns>
        public bool EligibleForMultiSiteDiscount(int siteId, int requestedLevel)
        {
            SqlCommand cmd;
            DataSet ds;

            cmd = _dbConn.GetStoredProcedureCommand("Sites_EligibleForMultiSiteDiscount");
            cmd.Parameters.AddWithValue("@SiteId", siteId);
            cmd.Parameters.AddWithValue("@RequestedLevel", requestedLevel);

            ds = _dbConn.GetDataSet(cmd);

            return (bool)ds.Tables[0].Rows[0][0];
        }

		#endregion

		#region Email

		/// <summary>
		///		Adds a message to the email queue for every user in the system.  Be very
		///		careful when calling this so that you avoid spamming people.
		/// </summary>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		public void SendEmailToAllUsers(string subject, string message, string filter)
		{
			SqlCommand cmd = _dbConn.GetStoredProcedureCommand("Administration_SendEmailToAllUsers");

			cmd.Parameters.AddWithValue("@Subject", subject);
			cmd.Parameters.AddWithValue("@Message", message);
            cmd.Parameters.AddWithValue("@Filter", filter);

			_dbConn.ExecuteNonQuery(cmd);
		}

		/// <summary>
		///		Gets the next email from the queue that should be sent.
		/// </summary>
		/// <returns></returns>
		public EmailMessage GetNextQueuedEmail()
		{
			SqlCommand cmd;
			DataSet ds;
			EmailMessage msg;

			cmd = _dbConn.GetStoredProcedureCommand("Email_GetNextMessageFromQueue");
			ds = _dbConn.GetDataSet(cmd);

			//If we can't find an email, just return null
			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return null;

			msg = new EmailMessage();
			DataMapper.PopulateObject(msg, ds, null, null);

			return msg;
		}

		/// <summary>
		///		Saves the specified message to the database.
		/// </summary>
		/// <param name="msg">
		///		The message to save to the database.
		/// </param>
		public void SaveEmail(EmailMessage msg)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			DateTime nullDate;
			
			cmd = _dbConn.GetStoredProcedureCommand("Email_SaveEmailMessage");

			if(msg.HasId)
				cmd.Parameters.AddWithValue("@EmailId", msg.Id);

			nullDate = new DateTime();

			cmd.Parameters.AddWithValue("@From", msg.From);
			cmd.Parameters.AddWithValue("@ToName", msg.ToName);
			cmd.Parameters.AddWithValue("@ToAddress", msg.ToAddress);
			cmd.Parameters.AddWithValue("@Subject", msg.Subject);
			cmd.Parameters.AddWithValue("@Message", msg.Message);
			cmd.Parameters.AddWithValue("@Html", msg.Html);
			if(msg.SentOn != nullDate)
				cmd.Parameters.AddWithValue("@SentOn", msg.SentOn);
			if(msg.QueuedOn != nullDate)
				cmd.Parameters.AddWithValue("@QueuedOn", msg.QueuedOn);
			if(msg.LastTry != nullDate)
				cmd.Parameters.AddWithValue("@LastTry", msg.LastTry);
			cmd.Parameters.AddWithValue("@NumberOfTries", msg.NumberOfTries);
			cmd.Parameters.AddWithValue("@UserId", msg.UserId);

			//Return value
			returnParam = cmd.Parameters.AddWithValue("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			//If we did an insert, grab the new id
			if(!msg.HasId)
				msg.Id = (int)returnParam.Value;
		}

		/// <summary>
		///		Gets all the email from the email queue that
		///		has not been sent yet.
		/// </summary>
		/// <returns></returns>
		public DataTable GetAllUnsentEmail(DateTime startTime)
		{
			SqlCommand cmd;
			SqlParameter param;

			cmd = _dbConn.GetStoredProcedureCommand("Report_GetAllUnsentEmail");

			if(startTime != new DateTime())
			{
				param = cmd.Parameters.AddWithValue("@StartTime", SqlDbType.DateTime);
				param.Value = startTime;
			}

			return _dbConn.GetDataSet(cmd).Tables[0];
		}

		#endregion

		#region Legal Notices
		public LegalNoticeVersion GetLatestLegalNoticeVersion(LegalNotices legalNotice)
		{
			SqlCommand cmd;
			DataSet ds;
			LegalNoticeVersion version;

			cmd = this._dbConn.GetStoredProcedureCommand("Legal_GetNoticeVersion");
			cmd.Parameters.AddWithValue("@NoticeId", (int)legalNotice);

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
				version = new LegalNoticeVersion((int)ds.Tables[0].Rows[0]["Id"]);
			else
				throw new ApplicationException("There are no versions for the specified legal notice.");
			DataMapper.PopulateObject(version, ds, null, null);

			return version;
		}

		public bool HasAgreedToNotice(int legalNoticeVersionId, int userId)
		{
			SqlCommand cmd;
			DataSet ds;
			LegalNoticeAgreement agreement;

			cmd = _dbConn.GetStoredProcedureCommand("Legal_GetNoticeAgreement");
			cmd.Parameters.AddWithValue("@NoticeVersionId", legalNoticeVersionId);
			cmd.Parameters.AddWithValue("@UserId", userId);

			ds = _dbConn.GetDataSet(cmd);

			if(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
				return false;
			else
			{
				agreement = new LegalNoticeAgreement();
				DataMapper.PopulateObject(agreement, ds, null, null);
				return agreement.Agree;
			}
		}

		public void SaveLegalNoticeAgreement(LegalNoticeAgreement agreement)
		{
			SqlCommand cmd;

			cmd = _dbConn.GetStoredProcedureCommand("Legal_SaveNoticeAgreement");
			cmd.Parameters.AddWithValue("@NoticeVersionId", agreement.LegalNoticeVersionId);
			cmd.Parameters.AddWithValue("@UserId", agreement.UserId);
			cmd.Parameters.AddWithValue("@Agree", agreement.Agree);

			_dbConn.ExecuteNonQuery(cmd);
		}
		#endregion

        #region Leads

        /// <summary>
        ///     Increments the counter that tracks how many hits came
        ///     from the specifiec lead.
        /// </summary>
        /// <param name="leadId">
        ///     The ID of the lead that should be incremented.
        /// </param>
        public void IncrementLeadHit(int leadId)
        {
            SqlCommand cmd;

            cmd = _dbConn.GetStoredProcedureCommand("Leads_IncrementLeadHit");
            cmd.Parameters.AddWithValue("@LeadId", leadId);

            _dbConn.ExecuteNonQuery(cmd);
        }
        


        #endregion

        #region Reporting
        public DataSet GetReport(string reportName)
        {
            SqlCommand cmd;

            cmd = _dbConn.GetStoredProcedureCommand("Reporting_GetReport");
            cmd.Parameters.AddWithValue("@ReportName", reportName);

            return _dbConn.GetDataSet(cmd);
        }

        public DataSet ExecuteReport(string reportName, params SqlParameter[] parameters)
        {
            SqlCommand cmd;

            cmd = _dbConn.GetStoredProcedureCommand(reportName);
            foreach (SqlParameter param in parameters)
                cmd.Parameters.Add(param);

            return _dbConn.GetDataSet(cmd);
        }

        [Obsolete("Use GetReports(int)")]
        public DataSet GetReports(bool userReportsOnly)
        {
            SqlCommand cmd;
            DataSet ds;

            cmd = _dbConn.GetStoredProcedureCommand("Reporting_GetReports");
            cmd.Parameters.AddWithValue("@UserReportsOnly", userReportsOnly);

            ds = _dbConn.GetDataSet(cmd);

            return ds;
        }

        public DataSet GetReports(int siteId)
        {
            SqlCommand cmd;
            DataSet ds;

            cmd = _dbConn.GetStoredProcedureCommand("Reporting_GetReports");
            cmd.Parameters.AddWithValue("@SiteId", siteId);

            ds = _dbConn.GetDataSet(cmd);

            return ds;
        }
        #endregion

        #region EmailFilters
        public DataTable GetEmailFilters()
        {
            SqlCommand cmd;
            DataTable dt;

            cmd = _dbConn.GetStoredProcedureCommand("EmailFiltering_GetFilters");

            dt = _dbConn.GetDataSet(cmd).Tables[0];

            return dt;
        }
        #endregion

        #region Services

        /// <summary>
        ///     Gets the information about a particular Windows service.
        /// </summary>
        /// <param name="serviceId">
        ///     The unique ID of the service whose information you would like to load.
        /// </param>
        /// <returns>
        ///     A <see cref="Service"/> object with the ID of the service you are
        ///     requesting.
        /// </returns>
        public Service GetService(int serviceId)
        {
            SqlCommand cmd;
            DataSet ds;
            Service svc;

            cmd = _dbConn.GetStoredProcedureCommand("Services_GetService");
            cmd.Parameters.AddWithValue("@ServiceId", serviceId);

            ds = _dbConn.GetDataSet(cmd);

            svc = new Service();
            DataMapper.PopulateObject(svc, ds, null, null);

            return svc;
        }

        /// <summary>
        ///     Adds or updates the <see cref="Service"/> in the database.
        /// </summary>
        /// <param name="svc"></param>
        public void SaveService(Service svc)
        {
            SqlCommand cmd;
            SqlParameter returnParam;

            cmd = _dbConn.GetStoredProcedureCommand("Services_SaveService");

            if (svc.HasId)
                cmd.Parameters.AddWithValue("@ServiceId", svc.Id);

            cmd.Parameters.AddWithValue("@Description", svc.Description);
            cmd.Parameters.AddWithValue("@LastHeartbeat", svc.LastHeartbeat);
            cmd.Parameters.AddWithValue("@RunIntervalMinutes", svc.RunIntervalMinutes);
            cmd.Parameters.AddWithValue("@Enabled", svc.Enabled);
            cmd.Parameters.AddWithValue("@LastRunTime", svc.LastRunTime);
            cmd.Parameters.AddWithValue("@ReloadConfiguration", svc.ReloadConfiguration);
            cmd.Parameters.AddWithValue("@ForceRun", svc.ForceRun);
            
            //Return value
            returnParam = cmd.Parameters.AddWithValue("@Return_Value", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            _dbConn.ExecuteNonQuery(cmd);

            //If we did an insert, grab the new id
            if (!svc.HasId)
                svc.Id = (int)returnParam.Value;
        }

        /// <summary>
        ///     Updates the last heartbeat time for a service.
        /// </summary>
        /// <param name="serviceId">
        ///     The ID of the service to update.
        /// </param>
        /// <param name="reloadConfiguration">
        ///     A boolean indicating wheter or not the service information
        ///     needs to be reloaded from the database.  Once this value is read,
        ///     it gets reset to 0, so you better handle it.
        /// </param>
        public void SaveServiceHeartbeat(int serviceId, out bool reloadConfiguration)
        {
            SqlCommand cmd;
            SqlParameter reloadParam;

            cmd = _dbConn.GetStoredProcedureCommand("Services_SaveHeartbeat");
            cmd.Parameters.AddWithValue("@ServiceId", serviceId);

            reloadParam = new SqlParameter("@ReloadConfiguration", SqlDbType.Bit);
            reloadParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(reloadParam);

            _dbConn.ExecuteNonQuery(cmd);

            reloadConfiguration = (bool)reloadParam.Value;
        }

        #endregion

        #region A/B Testing

        
		/// <summary>
		///		Saves the specified <see cref="ABContent" /> to the database.
		/// </summary>
		/// <param name="content">
		///		The <see cref="ABContent" /> to save to the database.
		/// </param>
		public void SaveABContent(Nle.Components.ABTesting.ABContent content)
		{
			SqlCommand cmd;
			SqlParameter returnParam;
			
			cmd = _dbConn.GetStoredProcedureCommand("ABTesting_SaveViewedContent");

			if(content.HasId)
				cmd.Parameters.AddWithValue("@ContentId", content.Id);

			//Add your parameters here that save the properties of the component
            cmd.Parameters.AddWithValue("@UserId", content.UserId);
            cmd.Parameters.AddWithValue("@RotatorKey", content.RotatorKey);
            cmd.Parameters.AddWithValue("@ContentKey", content.ContentKey);
            cmd.Parameters.AddWithValue("@Timestamp", content.Timestamp);
            cmd.Parameters.AddWithValue("@Action", content.Action);

			//Set up the parameter to retrieve the return value, which is the ID in the database
			returnParam = cmd.Parameters.Add("@Return_Value", SqlDbType.Int);
			returnParam.Direction = ParameterDirection.ReturnValue;

			_dbConn.ExecuteNonQuery(cmd);

			//If we did an insert, grab the new id
			if(!content.HasId)
				content.Id = (int)returnParam.Value;
		}
	

        #endregion
    }
}
