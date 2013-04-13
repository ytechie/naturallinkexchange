<%@ Page language="c#" Inherits="Nle.Website.Members.Manage_Site_Parameters.ManageSiteParameters"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>   
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
    
<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/DynamicHelp.js"></script>
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Manage-Site-Parameters/ManageSiteParameters.js"></script>

	<nle:DynamicHelp ID="dynamicHelp" runat="server">
		<nle:DynamicHelpText runat="server" ID="dhtSiteSelector" HelpId="999" Title="Site Selector" Text="<p>Use the Site Selector to easily switch between sites to configure that site.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhLinksToAdd" HelpId="1" Title="Links To Add" Text="<p>During one update cycle, we will add your links to a random number of users sites.  You can specify the minumum and maximum number of sites you want your links added to in one update cycle.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhArticlesPerPage" HelpId="2" Title="Articles Per Page" Text="<p>When we generate your link pages, we will add a random number of links to each page.  You can specify the minimum and the maximum number of links you want on a page.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhFrequency" HelpId="3" Title="Frequency" Text="<p>You can specify how often you want links added.</p><p><b>Example:</b> A frequency of 50% will add links every other day.  A frequency of 33% will add links every third day.</p>" />
	</nle:DynamicHelp>
		
	<h2>Links to add during an update cycle</h2>
	<div class="SiteParameters_Width">
	    <asp:Literal ID="litLinksPerUpdate" Runat="server" />
	    <asp:Panel runat="server" ID="pnlFreeSilverDesc1" CssClass="inline" >
            You can
            <a id="A1" runat="server" href="~/Members/Payment-Settings/Account-Upgrade.aspx">upgrade to a <asp:Literal runat="server" ID="descFreeSilver_NextLevel" /> account</a>
             to be able to obtain more links per day.
	    </asp:Panel> 
	    <br />
	    <br />
	    <div class="standardFieldLabel">Minimum:</div>
	    <asp:TextBox Runat="server" ID="txtId1" onclick="ShowDynamicHelp('dynamicHelp', 1);" onkeypress="return IsNumeric(event.which == null ? event.keyCode : event.which) || IsControl(event.which == null ? event.keyCode : event.which);" /><br />
	    <div class="standardFieldLabel">Maxiumum:</div>
	    <asp:TextBox Runat="server" ID="txtId2" onclick="ShowDynamicHelp('dynamicHelp', 1);" onkeypress="return IsNumeric(event.which == null ? event.keyCode : event.which) || IsControl(event.which == null ? event.keyCode : event.which);" /><br />
    	
	    <h2>Number of RSS feeds per page</h2>
	    <asp:Literal ID="litArticlesPerPage" Runat="server" />
	    <br />
	    <br />
	    <div class="standardFieldLabel">Minimum:</div>
	    <asp:TextBox Runat="server" ID="txtId3" onclick="ShowDynamicHelp('dynamicHelp', 2);" onkeypress="return IsNumeric(event.which == null ? event.keyCode : event.which) || IsControl(event.which == null ? event.keyCode : event.which);" /><br />
	    <div class="standardFieldLabel">Maximum:</div>
	    <asp:TextBox Runat="server" ID="txtId4" onclick="ShowDynamicHelp('dynamicHelp', 2);" onkeypress="return IsNumeric(event.which == null ? event.keyCode : event.which) || IsControl(event.which == null ? event.keyCode : event.which);" /><br />
    	
	    <h2>Frequency to add links</h2>
	    <asp:Literal ID="litFrequency" Runat="server" />
	    <asp:Panel runat="server" ID="pnlFreeSilverDesc2" CssClass="inline">
            Get even more links by 
            <a id="A2" runat="server" href="~/Members/Payment-Settings/Account-Upgrade.aspx">upgrading to a Silver or Gold account</a>.
	    </asp:Panel> 
	    <br />
	    <br />
	    <div class="standardFieldLabel">Percentage of days:</div>
	    <asp:TextBox Runat="server" ID="txtId5" onclick="ShowDynamicHelp('dynamicHelp', 3);" onkeypress="return IsNumeric(event.which == null ? event.keyCode : event.which) || IsControl(event.which == null ? event.keyCode : event.which);" /><br /><br />
	    <asp:Button Runat="server" ID="cmdSave" Text="Save" />
	    <asp:Button Runat="server" ID="cmdCancel" Text="Cancel" />
    </div>
</asp:Content>