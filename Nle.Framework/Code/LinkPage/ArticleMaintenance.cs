using System;
using System.Reflection;
using log4net;
using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.LinkPage
{
	/// <summary>
	///		Used to update the articles that are cached for a user.
	/// </summary>
	public class ArticleMaintenance
	{
		Database _db;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public ArticleMaintenance(Database db)
		{
			_db = db;
		}

		public void Run()
		{
			Site[] sitesToProcess;
			
			sitesToProcess = _db.GetSitesForLinkProcessing();
			foreach(Site currSite in sitesToProcess)
			{
				processSite(currSite);	
			}
		}

		private void processSite(Site s)
		{
			try
			{
				//Todo: Check if there are any articles that belong to old customers

				_log.DebugFormat("Processing site #{0} with the name {1}", s.Id, s.Name);

				addNewLinks(s);
				mapNewFeeds(s);
				updateRelatedCategories(s);
			}
			catch(Exception ex)
			{
				_log.Error("Error Processing Site", ex);
			}
		}

		/// <summary>
		///		Randomly determines if this is a day to add links, and
		///		if it is, it adds a random number of links within the
		///		parameters defined by the site.
		/// </summary>
		/// <param name="s"></param>
		private void addNewLinks(Site s)
		{
			int randomNumber;
			int linksToAdd;
			
			//Generate a random number between 1 and 100
			randomNumber = getRandomIntInRange(1, 100); 

			if(s.AddLinksPercentDays > randomNumber)
			{
				//This is a day we are going to add links

				//Figure out how many links to add
				linksToAdd = getRandomIntInRange(s.MinLinksToAdd, s.MaxLinksToAdd); 

				//Now add the links
				addNewLinks(s, linksToAdd);
			}
		}

		private void mapNewFeeds(Site s)
		{
			_db.UpdateSiteFeeds(s.Id);
		}

		private void updateRelatedCategories(Site s)
		{
			_db.UpdateSiteRelatedCategories(s.Id);
		}

		/// <summary>
		///		Adds the specified number of links to the specified
		///		site.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="linkCount"></param>
		private void addNewLinks(Site s, int linkCount)
		{
			for(int i = 0; i < linkCount; i++)
				_db.AddLink(s);
		}


		/// <summary>
		///		Creates a random number within and including
		///		the specified range.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		private int getRandomIntInRange(int min, int max)
		{
			int range;
			double rangeAmount;
			Random r;
			int result;

			r = new Random();

			range = max - min;
			rangeAmount = (double)range * r.NextDouble();

			result = (int)Math.Round((double)min + rangeAmount);

			return result;
		}
	}
}