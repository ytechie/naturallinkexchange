using System;

namespace Nle.Ranking.TopDogPro
{
	/// <summary>
	/// Summary description for Ranking.
	/// </summary>
	public class Ranking
	{
		private DateTime _timestamp;
		private string _searchEngine;
		private int _rank;
		private string _url;
		private string _searchString;

		/// <summary>
		///		Creates a new instance of the <see cref="Ranking"/> class.
		/// </summary>
		public Ranking()
		{
		}

		/// <summary>
		///		Gets or set the name of the search engine that
		///		the sample was taken from.
		/// </summary>
		public string SearchEngine
		{
			get { return _searchEngine; }
			set { _searchEngine = value; }
		}

		/// <summary>
		///		Gets or sets the rank for this sample.
		/// </summary>
		public int Rank
		{
			get { return _rank; }
			set { _rank = value; }
		}

		/// <summary>
		///		Gets or sets the URL whose rank was checked in this sample.
		/// </summary>
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		/// <summary>
		///		Gets or sets the search string that was tested to get this sample.
		/// </summary>
		public string SearchString
		{
			get { return _searchString; }
			set { _searchString = value; }
		}

		/// <summary>
		///		Gets or sets the timestamp of the sample.
		/// </summary>
		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}

		/// <summary>
		///		Sets the string representation of the timestamp of the sample.
		///		This is used to convert the string timestamps from the CSV file.
		/// </summary>
		public string TimestampString
		{
			set { _timestamp = DateTime.Parse(value); }
		}
	}
}