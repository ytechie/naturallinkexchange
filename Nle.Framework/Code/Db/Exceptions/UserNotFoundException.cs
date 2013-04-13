using System;

namespace Nle.Db.Exceptions
{
	/// <summary>
	///		Used to signal that a <see cref="User"/> could not be found.
	/// </summary>
	public class UserNotFoundException : ApplicationException
	{
		private int _userId;

        public int UserId
        {
            get { return _userId; }
        }

		/// <summary>
		///		Creates a new instance of the <see cref="SiteNotFoundException"/> exception.
		/// </summary>
		/// <param name="userId">
		///		The unique identifier of the user that could not be found.
		/// </param>
		public UserNotFoundException(int userId)
		{
			_userId = userId;
		}
	}
}