using System;
using NUnit.Framework;

namespace Nle.Components
{
	[TestFixture()]
	public class KeyPhrase_Tester
	{
		private KeyPhrase kp;


		[SetUp()]
		public void Setup()
		{
			kp = new KeyPhrase(7);			
		}

		[Test]
		public void Phrase()
		{
			kp.Phrase = "go";
			Assert.AreEqual("go", kp.Phrase);
		}

		[Test]
		public void ID()
		{
			Assert.AreEqual(7, kp.Id);
		}
		
	}
}
