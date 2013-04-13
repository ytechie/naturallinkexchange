<%@ Page Language="C#" %>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title></title>
    </head>
    <body>
    </body>
</html>

<script runat="Server">
    /// <summary>
    ///     The lead key that uniquely identifies users that came
    ///     in through this link.
    /// </summary>
    public const int LEAD_KEY = 7;

    private Nle.Db.SqlServer.Database _db;

    protected override void OnInit(EventArgs e)
    {
        Page.Load += new EventHandler(Page_Load);
        base.OnInit(e);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        _db = Nle.Website.Global.GetDbConnection();
        
        //Record this as a hit for this lead
        _db.IncrementLeadHit(LEAD_KEY);
        
        //Record the lead key for this page
        Nle.Website.LeadTracking.SaveLead(Response.Cookies, LEAD_KEY); 
        
        //Send them back to the home page now
        Response.Redirect(ResolveUrl("~/"));
    }

</script>