using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Nle.Ranking.TopDogPro
{
	/// <summary>
	///		Parses the CSV output file generated by TopDog pro.
	/// </summary>
	public class CsvParser
	{
		public const string CSV_REGEX = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";

		private CsvParser()
		{
		}

		/// <summary>
		///		Parses the file and reads all of the ranking results.
		/// </summary>
		public static Ranking[] ParseResults(string csvContents)
		{
			Ranking[] rankArr;
			ArrayList ranks;
			Ranking currRanking;

			string[] fileLines;

			fileLines = getFileLines(csvContents);

			ranks = new ArrayList();

			for (int i = 0; i < fileLines.Length; i++)
			{
				currRanking = parseRankLine(fileLines[i]);

				if (currRanking != null)
					ranks.Add(currRanking);
			}

			rankArr = new Ranking[ranks.Count];
			ranks.CopyTo(rankArr);

			return rankArr;
		}

		/// <summary>
		///		Parses the input file into it's lines.
		/// </summary>
		/// <returns></returns>
		private static string[] getFileLines(string fileText)
		{
			string[] lines;

			lines = fileText.Split('\n');

			return lines;
		}

		private static string[] getRankComponents(string fileLine)
		{
			string[] components;

			components = Regex.Split(fileLine, CSV_REGEX);

			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].StartsWith("\"") && components[i].EndsWith("\""))
					components[i] = components[i].Substring(1, components[i].Length - 2);
			}

			return components;
		}

		/// <summary>
		///		Parses a line from the CSV file and populates a 
		///		<see cref="Ranking"/> object.
		/// </summary>
		/// <param name="rankLine"></param>
		/// <returns></returns>
		private static Ranking parseRankLine(string rankLine)
		{
			Ranking rank;
			string[] rankComponents;

			if (rankLine.Length == 0)
				return null;

			rankComponents = getRankComponents(rankLine);
			if (rankComponents == null)
				return null;
			if (rankComponents.Length != 12)
				throw new ApplicationException("Error Parsing Input File, Incorrect Number Of Items On Line");

			rank = new Ranking();
			rank.TimestampString = rankComponents[8];
			rank.SearchEngine = rankComponents[0];
			rank.Rank = int.Parse(rankComponents[2]);
			rank.Url = rankComponents[9];
			rank.SearchString = rankComponents[10];

			return rank;
		}
	}
}