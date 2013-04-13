using System;

namespace Nle.Website.Common_Controls
{
	/// <summary>
	///		Used to indicate that the user has selected a new site.
	/// </summary>
	public class SiteSelectionEventArgs : EventArgs
	{
		private int _siteId;

		/// <summary>
		///		The ID of the site that is now selected.
		/// </summary>
		public int SiteId
		{
			get { return _siteId; }
		}

		/// <summary>
		///		Creates a new instance of the <see cref="SiteSelectionEventArgs"/> class.
		/// </summary>
		public SiteSelectionEventArgs(int siteId)
		{
			_siteId = siteId;
		}


	}
}
