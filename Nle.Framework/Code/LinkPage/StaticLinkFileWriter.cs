using System;
using System.IO;
using System.Text;

namespace Nle.LinkPage
{
	/// <summary>
	///		Uses the <see cref="StaticLinkFileGenerator"/> to generate the static
	///		link file pages, and then writes them to the file system.
	/// </summary>
	public class StaticLinkFileWriter
	{
		private StaticLinkFileGenerator _generator;
		private string _directory;

		/// <summary>
		///		Creates a new instance of the <see cref="StaticLinkFileWriter"/> class.
		/// </summary>
		public StaticLinkFileWriter(StaticLinkFileGenerator generator, string directory)
		{
			_generator = generator;

			if (directory == null)
				throw new NullReferenceException("Directory Cannot Be Null");

			_directory = directory;
		}

		/// <summary>
		///		Generates the link pages and writes them out
		///		to the file system.
		/// </summary>
		public void WriteFiles()
		{
			LinkFile[] linkFiles;
			string currFileName;
			FileStream fs;
			byte[] fileBytes;

			if (!Directory.Exists(_directory))
				throw new DirectoryNotFoundException(string.Format("{0} Was Not Found", _directory));

			linkFiles = _generator.GeneratePages();

			foreach (LinkFile currFile in linkFiles)
			{
				currFileName = Path.Combine(_directory, currFile.FileName);
				fs = File.OpenWrite(currFileName);
				fileBytes = ASCIIEncoding.ASCII.GetBytes(currFile.FileHtml);
				fs.Write(fileBytes, 0, fileBytes.Length);
				fs.Close();
			}
		}
	}
}