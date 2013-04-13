using Nle.Components;

namespace Nle.Db.Exceptions
{
	/// <summary>
	///		Used to signal that an <see cref="FtpUploadInfo"/> object
	///		could not be found.
	/// </summary>
	public class FtpUploadInfoNotFoundException
	{
		private int _id;

		/// <summary>
		///		Creates a new instance of the <see cref="FtpUploadInfoNotFoundException"/> class
		///		with the specified Id.
		/// </summary>
		/// <param name="ftpId">
		///		The unique identifier of the ftp upload information.
		/// </param>
		public FtpUploadInfoNotFoundException(int ftpId)
		{
			_id = ftpId;
		}
	}
}