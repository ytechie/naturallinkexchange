using System;
using System.Threading;
using NUnit.Framework;

namespace Nle.Service
{
	[TestFixture()]
	public class ServiceProcessManager_Tester
	{
		private ServiceProcessManager spm;

		[SetUp()]
		public void Setup()
		{
			spm = new ServiceProcessManager();
		}

		[Test()]
		public void SingleProcess()
		{
			MockProcess mp;

			mp = new MockProcess(5);
			mp.DelayTime = TimeSpan.FromMilliseconds(100);

			spm.AddProcess(mp);
			Thread.Sleep(200);
			Assert.AreEqual(true, mp.WasRun);
		}

		/// <summary>
		///		Add a process that should not be run right away, and
		///		add a second one that should.  Make sure the right one
		///		is run right away, but the other is not.
		/// </summary>
		[Test()]
		public void MultipleProcesses()
		{
			MockProcess mp = new MockProcess(5);
			MockProcess mp2 = new MockProcess(6);

			mp.RunIntervalMS = 0;
			mp2.RunIntervalMS = 50000;
			mp2.SetLastRun(DateTime.UtcNow.AddSeconds(1));

			spm.AddProcess(mp2);
			spm.AddProcess(mp);
			Assert.AreEqual(true, mp.WasRun);
			Assert.AreEqual(false, mp2.WasRun);
		}

		/// <summary>
		///		Verify that the processes are run the the right order.
		///		Multiple processes are added that should be executed
		///		near the same time, but not exactly.
		/// </summary>
		[Test()]
		public void MultipleProcesses2()
		{
			MockProcess mp = new MockProcess(5);
			MockProcess mp2 = new MockProcess(6);

			mp.SetLastRun(DateTime.UtcNow.AddMilliseconds(100));
			mp2.SetLastRun(DateTime.UtcNow.AddMilliseconds(100));

			mp.RunIntervalMS = 1000;
			mp2.RunIntervalMS = 10;

			spm.AddProcess(mp);
			spm.AddProcess(mp2);

			Thread.Sleep(200);

			Assert.AreEqual(false, mp.WasRun);
			Assert.AreEqual(true, mp2.WasRun);
		}
	}
}