using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Nle.Services.TopDogImporter
{
	/// <summary>
	///		The installer for the NLE TopDog Pro CSV importer
	/// </summary>
	[RunInstaller(true)]
	public class CsvFolderMonitorInstaller : Installer
	{
		private ServiceProcessInstaller serviceProcessInstaller;
		private ServiceInstaller serviceInstaller;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public CsvFolderMonitorInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// serviceProcessInstaller
			// 
			this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.serviceProcessInstaller.Password = null;
			this.serviceProcessInstaller.Username = null; 
			
			//This is how it knows what service to install
			this.serviceInstaller.ServiceName = CsvFolderMonitor.SERVICE_NAME;
			this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
						
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[]
				{
					this.serviceProcessInstaller,
					this.serviceInstaller
				});

		}

		#endregion
	}
}