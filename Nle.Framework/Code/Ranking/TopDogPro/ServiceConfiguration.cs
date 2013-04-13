using System.IO;
using System.Text;
using YTech.General.Serialization;

namespace Nle.Ranking.TopDogPro
{
	/// <summary>
	///		Reads and writes the configuration data for the TopDogImporter service.
	/// </summary>
	public class ServiceConfiguration
	{
		private string _monitorFolder;
		private string _dbConnectionString;

		/// <summary>
		///		The folder to monitor for CSV files.
		/// </summary>
		public string MonitorFolder
		{
			get { return _monitorFolder; }
			set { _monitorFolder = value; }
		}

		/// <summary>
		///		Gets or sets the connection string to connect to the database.
		/// </summary>
		public string DbConnectionString
		{
			get { return _dbConnectionString; }
			set { _dbConnectionString = value; }
		}

		/// <summary>
		///		Creates a new instance of the <see cref="Configuration"/> class;
		/// </summary>
		public ServiceConfiguration()
		{
		}

		/// <summary>
		///		Gets the complete XML string representation of all the 
		///		settings.
		/// </summary>
		/// <returns></returns>
		public string GetConfigXml()
		{
			return SerializationUtilities.XmlSerialize(this);
		}

		/// <summary>
		///		Reads the configuration data from the string, and creates
		///		a <see cref="ServiceConfiguration"/> object from it.
		/// </summary>
		public static ServiceConfiguration ReadConfigXml(string configXml)
		{
			ServiceConfiguration sc;
			object scObject;

			scObject = SerializationUtilities.XmlDeserialize(configXml, typeof (ServiceConfiguration));

			sc = (ServiceConfiguration) scObject;

			return sc;
		}

		/// <summary>
		///		Saves the configuration data to the specified file.
		/// </summary>
		/// <remarks>
		///		If the file already exists, it is replaced.  If it does
		///		not exist, it is created.
		/// </remarks>
		/// <param name="path">
		///		The complete path and file name of the file to write.
		/// </param>
		public void SaveToFile(string path)
		{
			string fileContents;
			FileStream fs;
			byte[] fileBytes;

			fileContents = GetConfigXml();
			fileBytes = Encoding.UTF8.GetBytes(fileContents);

			fs = File.Open(path, FileMode.OpenOrCreate);
			try
			{
				fs.Write(fileBytes, 0, fileBytes.Length);
			}
			finally
			{
				fs.Close();
			}
		}

		/// <summary>
		///		Reads the file with the specified path, and loads the
		///		configuration data.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static ServiceConfiguration ReadFromFile(string path)
		{
			FileStream fs;
			byte[] fileBytes;
			string fileXml;

			fs = File.Open(path, FileMode.Open);
			try
			{
				fileBytes = new byte[fs.Length];
				fs.Read(fileBytes, 0, (int) fs.Length);
			}
			finally
			{
				fs.Close();
			}

			fileXml = Encoding.UTF8.GetString(fileBytes);

			return ReadConfigXml(fileXml);
		}
	}
}