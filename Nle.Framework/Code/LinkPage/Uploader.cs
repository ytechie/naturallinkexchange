using System;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using Nle.Components;
using Nle.Db.SqlServer;
//using YTech.Web.Ftp;
using EnterpriseDT.Net.Ftp;

namespace Nle.LinkPage
{
	/// <summary>
	///		Uses an FTP connection to generate and upload link
	///		pages to a client's website.
	/// </summary>
	public class Uploader
	{
		private Database _db;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		///		Creates a new instance of the <see cref="Uploader"/> class.
		/// </summary>
		/// <param name="dbConn">
		///		The database connection to use to retrieve the job information.
		/// </param>
		public Uploader(Database dbConn)
		{
			_db = dbConn;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="Uploader"/> class, and
		///		runs the specified FTP job immediately.
		/// </summary>
		/// <param name="dbConn">
		///		The database connection to use to retrieve the job information.
		/// </param>
		/// <param name="ftpInfo">
		///		The FTP info to update immediately.
		/// </param>
		public Uploader(Database dbConn, FtpUploadInfo ftpInfo) : this(dbConn)
		{
			processFtpInfo(ftpInfo);
		}

		/// <summary>
		///		Checks the database to see if there are any link pages that
		///		are due for generation and upload.
		/// </summary>
		/// <returns>
		///		True if a link page was generated and uploaded.
		/// </returns>
		public bool RunNextJob()
		{
			FtpUploadInfo ftpInfo;

			_log.Debug("Checking for next scheduled FTP upload");

			ftpInfo = _db.GetNextFtpUploadInfo(TimeSpan.FromHours(12.0));

			if (ftpInfo == null)
			{
				_log.Debug("No jobs are waiting to be processed");

				return false;
			}
			else
			{
				_log.InfoFormat("Found ftp info job #{0}, so it is being processed", ftpInfo.Id);
				processFtpInfo(ftpInfo);

				return true;
			}
		}

		/// <summary>
		///		Processes the specified FTP information.
		/// </summary>
		/// <param name="ftpInfo"></param>
		private void processFtpInfo(FtpUploadInfo ftpInfo)
		{
			StaticLinkFileGenerator pageGenerator;
			Site site;
			LinkFile[] linkFiles;
			FTPClient ftp;
			MemoryStream ms;
			//StringReader sr;
			byte[] htmlBytes;

			_log.DebugFormat("Processing FTP Information for upload Id #{0}, Url: {1}", ftpInfo.Id, ftpInfo.Url);

            //Make sure our information is valid
            TestFtpInfo(ftpInfo);
            			
			//Load the site information
			site = new Site(ftpInfo.SiteId);
			_db.PopulateSite(site);

			pageGenerator = new StaticLinkFileGenerator(_db, site);
			linkFiles = pageGenerator.GeneratePages();

            _log.DebugFormat("Generated {0} link pages to upload", linkFiles.Length);

			if(linkFiles.Length > 0)
			{             
                ftp = new FTPClient();
                ftp.RemoteHost = ftpInfo.Url;
                _log.DebugFormat("Connecting to FTP server '{0}'", ftp.RemoteHost);
                ftp.Connect();
                if (ftpInfo.ActiveMode)
                    ftp.ConnectMode = FTPConnectMode.ACTIVE;
                else
                    ftp.ConnectMode = FTPConnectMode.PASV;
                _log.DebugFormat("Logging into FTP server with username '{0}', password '{1}'", ftpInfo.UserName, ftpInfo.Password);
				ftp.Login(ftpInfo.UserName, ftpInfo.Password);
				try
				{
					if(ftpInfo.FtpPath != null && ftpInfo.FtpPath.Length > 0)
					{                          
						//Switch to the right directory
						ftp.ChDir(ftpInfo.FtpPath);
					}

					//Loop through all the files that we need to upload
					foreach (LinkFile currLinkFile in linkFiles)
					{
						htmlBytes = Encoding.UTF8.GetBytes(currLinkFile.FileHtml);
						ms = new MemoryStream(htmlBytes);

						ftp.Put(ms, currLinkFile.FileName);
                        _log.DebugFormat("Successfully uploaded '{0}'", currLinkFile.FileName);
					}
				}
				finally
				{
					ftp.Quit();
				}
			}

			//Mark the ftp info as processed
            _log.Debug("Marking FTP info as processed");
			_db.SetFtpUploadInfoProcessed(ftpInfo.Id, DateTime.UtcNow);
		}

        /// <summary>
        ///     Tests the FTP information, and throws an exception if
        ///     there is a problem.
        /// </summary>
        /// <remarks>
        ///     Todo: Is there is a good way to check if the current mode (active/passive) works?
        /// </remarks>
        /// <param name="testInfo"></param>
        /// <returns></returns>
        public void TestFtpInfo(FtpUploadInfo testInfo)
        {
            FTPClient ftp;

            //Test our FTP connection
            ftp = new FTPClient();
            ftp.RemoteHost = testInfo.Url;

            ftp.Connect();

            if (testInfo.ActiveMode)
                ftp.ConnectMode = FTPConnectMode.ACTIVE;
            else
                ftp.ConnectMode = FTPConnectMode.PASV;

            ftp.Login(testInfo.UserName, testInfo.Password);
            if (!ftp.IsConnected)
                throw new FTPException("Could Not Connect To Ftp Server");
            ftp.Quit();
        }
	}
}