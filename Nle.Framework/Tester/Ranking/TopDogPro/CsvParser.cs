using System;
using System.Reflection;
using NUnit.Framework;
using YTech.General.General.Reflection;

namespace Nle.Ranking.TopDogPro
{
	[TestFixture()]
	public class CsvParser_Tester
	{
		[SetUp()]
		public void Setup()
		{
		}

		[Test()]
		public void ParseResults()
		{
			string sampleFile;
			Ranking[] ranks;

			sampleFile = loadSampleCsv("SampleCSV1.csv");
			ranks = CsvParser.ParseResults(sampleFile);

			Assert.AreEqual(3, ranks.Length);

			Assert.AreEqual("MSN", ranks[0].SearchEngine);
			Assert.AreEqual("usps Tracking", ranks[0].SearchString);
			Assert.AreEqual("http://www.Young-Technologies.com", ranks[0].Url);
			Assert.AreEqual(7, ranks[0].Rank);
			Assert.AreEqual(DateTime.Parse("5/10/2005"), ranks[0].Timestamp);

			Assert.AreEqual("Lycos", ranks[1].SearchEngine);
			Assert.AreEqual("ups tracking", ranks[1].SearchString);
			Assert.AreEqual("http://www.Young-Technologies.com", ranks[1].Url);
			Assert.AreEqual(0, ranks[1].Rank);
			Assert.AreEqual(DateTime.Parse("5/10/2005"), ranks[1].Timestamp);

			Assert.AreEqual("MSN", ranks[2].SearchEngine);
			Assert.AreEqual("ups tracking", ranks[2].SearchString);
			Assert.AreEqual("http://www.Young-Technologies.com", ranks[2].Url);
			Assert.AreEqual(13, ranks[2].Rank);
			Assert.AreEqual(DateTime.Parse("5/10/2005"), ranks[2].Timestamp);
		}

		[Test()]
		public void ParseResults2()
		{
			string sampleFile;
			Ranking[] ranks;

			sampleFile = loadSampleCsv("SampleCSV2.csv");
			ranks = CsvParser.ParseResults(sampleFile);

			Assert.AreEqual(20, ranks.Length);
		}

		private string loadSampleCsv(string fileName)
		{
			string fileContents;
			string fileNamespace;

			fileNamespace = typeof (CsvParser).Namespace + ".SampleCsvs";

			fileContents = EmbeddedFileUtilities.ReadEmbeddedTextFile(Assembly.GetExecutingAssembly(), fileNamespace, fileName);

			return fileContents;
		}
	}
}