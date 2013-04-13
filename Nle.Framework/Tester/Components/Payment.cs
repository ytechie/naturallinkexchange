using System;
using NUnit.Framework;

namespace Nle.Components
{
	[TestFixture()]
	public class Payment_Tester
	{
		private Payment p;


		[SetUp()]
		public void Setup()
		{
			p = new Payment();			
		}

		[Test]
		public void HasId()
		{	
			Assert.AreEqual(false, p.HasId);
		}

		[Test]
		public void HasId2()
		{
			p = new Payment(8);
			Assert.AreEqual(true, p.HasId);
		}
		
		[Test]
		public void HasId3()
		{
			p.HasId = true;
			Assert.AreEqual(true, p.HasId);
		}

		[Test]
		public void Id()
		{
			p.Id = 10;
			Assert.AreEqual(10, p.Id);
			Assert.AreEqual(true, p.HasId);
		}

		//Not done.  Continue from here

	}
}