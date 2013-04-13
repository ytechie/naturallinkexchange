using System;

namespace Nle.Db.Exceptions
{
	/// <summary>
	///		Used to signal that a <see cref="Site"/> could not be found.
	/// </summary>
	public class SiteNotFoundException : ApplicationException
	{
		private int _siteId;

		/// <summary>
		///		Creates a new instance of the <see cref="SiteNotFoundException"/> exception.
		/// </summary>
		/// <param name="siteId">
		///		The unique identifier of the site that could not be found.
		/// </param>
		public SiteNotFoundException(int siteId)
		{
			_siteId = siteId;
		}
	}
}