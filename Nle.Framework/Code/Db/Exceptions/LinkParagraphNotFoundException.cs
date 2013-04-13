using System;

namespace Nle.Db.Exceptions
{
	/// <summary>
	///		Used to signal that a <see cref="LinkParagraph"/> could not be found.
	/// </summary>
	public class LinkParagraphNotFoundException : ApplicationException
	{
		private int _linkId;

		/// <summary>
		///		Creates a new instance of the <see cref="LinkParagraphNotFoundException"/>.
		/// </summary>
		/// <param name="linkId">
		///		The unique identifier of the link that could not be found.
		/// </param>
		public LinkParagraphNotFoundException(int linkId)
		{
			_linkId = linkId;
		}
	}
}