using System;
using NUnit.Framework;

namespace Nle.Components
{
	[TestFixture()]
	public class LegalNoticeAgreement_Tester
	{
		private LegalNoticeAgreement lna;


		[SetUp()]
		public void Setup()
		{
			 lna = new LegalNoticeAgreement();			
		}

		[Test]
		public void Id()
		{
			lna = new LegalNoticeAgreement(5);
			Assert.AreEqual(5, lna.Id);
		}

		[Test]
		public void LegalNoticeVersionId()
		{
			lna.LegalNoticeVersionId = 3;
			Assert.AreEqual(3, lna.LegalNoticeVersionId);
		}

		[Test]
		public void UserId()
		{
			lna.UserId = 2;
			Assert.AreEqual(2, lna.UserId);
		}

		[Test]
		public void Agree()
		{
			lna.Agree = true;
			Assert.AreEqual(true, lna.Agree);
		}

		[Test]
		public void Agree2()
		{
			lna.Agree = false;
			Assert.AreEqual(false, lna.Agree);
		}


		[Test]
		public void IsNew()
		{
			Assert.AreEqual(true, lna.IsNew);
		}

		[Test]
		public void IsNew2()
		{
			lna = new LegalNoticeAgreement(8);
			Assert.AreEqual(false, lna.IsNew);
		}
	}
}
