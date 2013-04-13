using System;

namespace Nle.Db.Exceptions
{
	/// <summary>
	/// Summary description for LinkParagraphGroupNotFoundException.
	/// </summary>
	public class LinkParagraphGroupNotFoundException : ApplicationException
	{
		private int _id;

		/// <summary>
		///		Creates a new instance of the <see cref="LinkParagraphGroupNotFoundException"/>.
		/// </summary>
		/// <param name="linkGroupId">
		///		The unique identifier of the link group that could not be found.
		/// </param>
		public LinkParagraphGroupNotFoundException(int linkGroupId)
		{
			_id = linkGroupId;
		}
	}
}
