using System;
using System.Threading;

namespace Nle.Service
{
	public class MockProcess : ServiceProcess
	{
		public TimeSpan DelayTime = TimeSpan.FromMilliseconds(0);
		public bool SimulateFailure = false;
		public bool WasRun = false;
		public bool RunOnce = true;

		public MockProcess(int serviceId) : base(serviceId)
		{
			base._lastRun = DateTime.UtcNow;
		}

		public override void Run()
		{
			base.Run();

			Thread.Sleep(DelayTime);

			if (SimulateFailure)
				throw new ApplicationException("Simulated Process Failure");

			WasRun = true;

			//If the process should only run once, then we'll change the
			//last run time to the future so that it is not due to be run.
			if (RunOnce)
				base._lastRun = DateTime.UtcNow.AddYears(10);
		}

		public void SetLastRun(DateTime newVal)
		{
			_lastRun = newVal;
		}
	}
}