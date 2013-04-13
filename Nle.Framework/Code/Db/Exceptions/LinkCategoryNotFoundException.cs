using System;
using Nle.Components;

namespace Nle.Db.Exceptions
{
	/// <summary>
	///		Used to signal that a <see cref="LinkCategory"/> could not be found.
	/// </summary>
	public class LinkCategoryNotFoundException : ApplicationException
	{
		private int _categoryId;

		/// <summary>
		///		Creates a new instance of the <see cref="LinkCategoryNotFoundException"/> exception.
		/// </summary>
		/// <param name="categoryId">
		///		The unique identifier of the category that could not be found.
		/// </param>
		public LinkCategoryNotFoundException(int categoryId)
		{
			_categoryId = categoryId;
		}
	}
}