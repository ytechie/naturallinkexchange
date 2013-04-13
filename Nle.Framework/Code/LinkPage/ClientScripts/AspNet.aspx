<% @Import Namespace="System.Net" %>
<% @Import Namespace="System.IO" %>

<script runat="server" Language="C#">
	const string SITE_KEY = "{siteKey}";
	const string BASE_URL = "http://www.NaturalLinkExchange.com/";
	//For local testing
	//const string BASE_URL = "http://localhost/NLE/";
	
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
	}

	private void Page_Load(object sender, System.EventArgs e)
	{
		string html;

		html = readRemoteSite();
		Response.Clear();
		Response.Write(html);
		Response.End();
	}

	private string readRemoteSite()
	{
		WebRequest req;
		WebResponse resp;
		Stream responseStream;
		StreamReader sr;
		string html;
		string categoryName;
		string scriptName;
		
		categoryName = Request.QueryString["Category-Name"];
		scriptName = Request.ServerVariables["SCRIPT_NAME"];
		
		req = WebRequest.Create(BASE_URL + "ServerServices/LinksHtml/Default.aspx?cn=" +
			categoryName + "&sk=" + SITE_KEY + "&sn=" + scriptName);
		resp = req.GetResponse();
		
		responseStream = resp.GetResponseStream();
		sr = new StreamReader(resp.GetResponseStream());

		html = sr.ReadToEnd();
		resp.Close();
		sr.Close();

		return html;
	}
</script>