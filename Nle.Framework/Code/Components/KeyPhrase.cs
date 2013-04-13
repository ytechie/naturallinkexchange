using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a keyword phrase that is used to perform a search
	///		in search engines.
	/// </summary>
	public class KeyPhrase
	{
		private int _id;
		private string _phrase;

		/// <summary>
		///		The phrase that can be searched for in search engines.
		/// </summary>
		[FieldMapping("Phrase")]
		public string Phrase
		{
			get { return _phrase; }
			set { _phrase = value; }
		}

		/// <summary>
		///		Gets the unique identifier for this key phrase.
		/// </summary>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		///		Creates a new instance of the <see cref="KeyPhrase"/> class.
		/// </summary>
		public KeyPhrase(int phraseId)
		{
			_id = phraseId;
		}

	}
}