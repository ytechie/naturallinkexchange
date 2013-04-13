using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using YTech.General.General.Reflection;
using YTech.General.Web;

namespace Nle.Website.Members.Link_Page_Setup
{
	/// <summary>
	///		This page is used to stream the link page
	///		script to the users browser for downloading.
	/// </summary>
	public partial class Download_Link_Page_Script : Page
	{
		public const string MY_PATH = "Members/Link-Page-Setup/";
		public const string MY_FILE_NAME = "Download-Link-Page-Script.aspx";

		/// <summary>
		///		The name of the script that the user downloads to
		///		put on their website.  The appropriate suffix will
		///		be added.
		/// </summary>
		public const string CLIENT_SCRIPT_NAME = "Natural-Link-Exchange-Directory";

		private const string TOKEN_SITE_KEY = "{siteKey}";

		/// <summary>
		///		The name of the parameter for the site key
		///		of the script.
		/// </summary>
		public const string PARAM_SITE_KEY = "Site-Key";

		/// <summary>
		///		The name of the parameter for the type of
		///		script that should be generated.
		/// </summary>
		public const string PARAM_SCRIPT_TYPE = "Script-Type";

    public enum ScriptTypes
		{
			Php = 0,
			Net = 1
		}

		private string _siteKey;
		private ScriptTypes _scriptType;

		protected void Page_Load(object sender, EventArgs e)
		{
			getParameters();
			writeScript();
		}

		private void writeScript()
		{
			string script;
			string scriptName;

			script = getScript(_scriptType, out scriptName);
			script = replaceTokens(script);
			streamFile(Encoding.UTF8.GetBytes(script), scriptName);
		}

		private string replaceTokens(string script)
		{
			string outScript;

			outScript = Regex.Replace(script, TOKEN_SITE_KEY, _siteKey, RegexOptions.IgnoreCase);

			return outScript;
		}

		private string getScript(ScriptTypes scriptType, out string friendlyScriptName)
		{
			Assembly assm;
			string fileNamespace;
			string fileName;
			string script;

            //Get at the Nle.Framework dll by using a type inside it
			assm = Assembly.GetAssembly(typeof(Nle.Db.SqlServer.Database));
            fileNamespace = "Nle.LinkPage.ClientScripts";

			if(scriptType == ScriptTypes.Net)
			{
				friendlyScriptName = CLIENT_SCRIPT_NAME + ".aspx";
				fileName = "AspNet.aspx";
			}
			else if(scriptType == ScriptTypes.Php)
			{	
				friendlyScriptName = CLIENT_SCRIPT_NAME + ".php";
				fileName = "PHP.php";
			}
			else
				throw new NotSupportedException("Unrecognized Script Type");

			script = EmbeddedFileUtilities.ReadEmbeddedTextFile(assm, fileNamespace, fileName);

			return script;
		}

		private void getParameters()
		{
			string currVal;

			currVal = Request.QueryString[PARAM_SITE_KEY];
			if(currVal == null || currVal.Length == 0)
				throw new ApplicationException(PARAM_SITE_KEY + " is a required parameter");
			else
				_siteKey = currVal;

			currVal = Request.QueryString[PARAM_SCRIPT_TYPE];
			if(currVal == null || currVal.Length == 0)
				throw new ApplicationException(PARAM_SCRIPT_TYPE + " is a required parameter");
			else
				_scriptType = (ScriptTypes)int.Parse(currVal);

		}

		/// <summary>
		///		Gets the URL that can be used to call this page.
		/// </summary>
		/// <param name="siteKey"></param>
		/// <param name="scriptType"></param>
		/// <returns></returns>
		public static string GetLoadUrl(string siteKey, ScriptTypes scriptType)
		{
			UrlBuilder url;

			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);
			url.Parameters.AddParameter(PARAM_SITE_KEY, siteKey);
			url.Parameters.AddParameter(PARAM_SCRIPT_TYPE, (int)scriptType);

			return url.ToString();
		}

		/// <summary>
		///		Streams the byte representation of a file to the client browser.
		/// </summary>
		/// <param name="file">
		///		The file bytes to stream.
		/// </param>
		/// <param name="fileName">
		///		The file name that should be saved on the client's machine.
		/// </param>
		private void streamFile(byte[] file, string fileName)
		{
			Response.Clear();
			Response.ContentType = "application/octet-stream";
			Response.AppendHeader("content-disposition", "filename=\"" + fileName + "\"");
			Response.BinaryWrite(file);
			Response.End();
		}

		#region Web Form Designer generated code

		protected override void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion
	}
}