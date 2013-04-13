using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Exception_Default : System.Web.UI.Page
{
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
        Page.ErrorPage = "~/Exception/Error.htm";
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        MainMaster master;
        master = (MainMaster)Page.Master;
        master.AddStylesheet("~/Exception/Exception.css", true);
    }
}
