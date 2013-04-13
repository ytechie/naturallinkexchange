using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a specific URL that can be used to
	///		check the rank for.
	/// </summary>
	public class SiteRankUrl
	{
		private int _id;
		private int _siteId;
		private string _url;

		/// <summary>
		///		Gets the unique identifier of the site url.
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		///		Gets or sets the unique identifier of the site
		///		that this url belongs to.
		/// </summary>
		[FieldMapping("SiteId")]
		public int SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		/// <summary>
		///		Gets the url.
		/// </summary>
		[FieldMapping("url")]
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}

		/// <summary>
		///		Creates a new instance of the <see cref="SiteRankUrl"/> class.
		/// </summary>
		/// <param name="urlId"></param>
		public SiteRankUrl(int urlId)
		{
			_id = urlId;
		}


	}
}