<%@ Page language="c#" Inherits="Nle.Website.Members.Manage_Sites.ManageSitesDefault"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/payment-settings/default.aspx" %>
<%@ Reference Page="~/members/manage-sites/edit-site.aspx" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
	<h1>Manage Sites</h1>
	
	<asp:Panel runat="server" ID="pnlNoSites" Visible="false">
	    <p>
	        You have not yet set up any sites.  You will need to set up at least
	        one site before you can manage your account.  Once you set up at least one
	        site, you will be able to use the control panel (use the menu on the left). 
	        You can use this page at any time to add, remove, or change the websites that you manage.
	    </p>
	</asp:Panel>
	
	<asp:PlaceHolder Runat="server" ID="SitesList" /><br />
	<asp:HyperLink Runat="server" ID="lnkAddSite" Text="Add Site" />
</asp:Content>