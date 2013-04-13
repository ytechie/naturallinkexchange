namespace Nle.LinkPage
{
	/// <summary>
	///		Holds the information to be written to a html file such as the
	///		file name and the html.
	/// </summary>
	public class LinkFile
	{
		private string _fileName;
		private string _fileHtml;

		/// <summary>
		///		Creates a new instance of the <see cref="LinkFile"/> class.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="html"></param>
		public LinkFile(string name, string html)
		{
			_fileName = name;
			_fileHtml = html;
		}

		/// <summary>
		///		The name of the link file.
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
		}

		/// <summary>
		///		The HTML of the link file.
		/// </summary>
		public string FileHtml
		{
			get { return _fileHtml; }
		}
	}
}