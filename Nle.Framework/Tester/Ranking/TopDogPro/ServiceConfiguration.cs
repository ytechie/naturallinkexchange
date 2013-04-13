using System;
using System.IO;
using NUnit.Framework;

namespace Nle.Ranking.TopDogPro
{
	[TestFixture()]
	public class ServiceConfiguration_Tester
	{
		private ServiceConfiguration sc;
		private ServiceConfiguration sc2;

		[SetUp]
		public void Setup()
		{
			sc = new ServiceConfiguration();
		}

		[Test()]
		public void MonitorFolder()
		{
			string xml;

			sc.MonitorFolder = "C:\\test";

			xml = sc.GetConfigXml();
			sc2 = ServiceConfiguration.ReadConfigXml(xml);

			Assert.AreEqual("C:\\test", sc2.MonitorFolder);
		}

		[Test()]
		public void DbConnectionString()
		{
			string xml;

			sc.DbConnectionString = "test string";

			xml = sc.GetConfigXml();
			sc2 = ServiceConfiguration.ReadConfigXml(xml);

			Assert.AreEqual("test string", sc2.DbConnectionString);
		}

		[Test()]
		public void SaveReadFile()
		{
			string tempFile;

			sc.MonitorFolder = "C:\\test";
			sc.DbConnectionString = "db conn";

			tempFile = getTempFilePath();

			sc.SaveToFile(tempFile);
			sc2 = ServiceConfiguration.ReadFromFile(tempFile);

			Assert.AreEqual("C:\\test", sc2.MonitorFolder);
			Assert.AreEqual("db conn", sc2.DbConnectionString);
		}

		private string getTempFilePath()
		{
			string tempFolder;
			string tempFile;

			tempFolder = Environment.GetEnvironmentVariable("TEMP");
			tempFile = Path.Combine(tempFolder, "sc.xml");

			return tempFile;
		}

	}
}