<%@ Page language="c#" Inherits="Nle.Website.Members.Manage_Sites.EditSite" CodeFile="Edit-Site.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <nle:DynamicHelp ID="dynamicHelp" runat="server">
	    <nle:DynamicHelpText runat="server" ID="dhtxtSiteName" HelpId="1" Title="Site Name" Text="<p>Specify the name of the site for your reference.</p>" />
	    <nle:DynamicHelpText runat="server" ID="dhtxtSiteUrl" HelpId="2" Title="Site Url" Text="<p>The address of your website.  This address will be used as your base address for any links used for this site.</p>" />
	    <nle:DynamicHelpText runat="server" ID="dhddlCategories" HelpId="3" Title="Initial Category" Text="<p>The category in which your site best fits.</p>" />
    </nle:DynamicHelp>
    
    <asp:PlaceHolder runat="server" ID="JavascriptPlaceholder" />
		
	<h1>Site Editor</h1>
	<div class="standardFieldLabel">Site Name:</div>
	<asp:TextBox Runat="server" ID="txtSiteName" CssClass="SiteName" onclick="ShowDynamicHelp('dynamicHelp',1);" MaxLength="50" />
	<asp:RegularExpressionValidator ID="txtSiteNameRegExValidator" Runat="server" ControlToValidate="txtSiteName" Display="Dynamic" ValidationExpression=".{0,50}" ErrorMessage="Site name can not be more than 50 characters" />
	<asp:RequiredFieldValidator ID="txtSiteNameRequiredValidator" Runat="server" ControlToValidate="txtSiteName"
	  Display="Dynamic" ErrorMessage="Site name is required" /><br />
	
	<div class="standardFieldLabel">Site Url:</div>
	<asp:TextBox Runat="server" ID="txtSiteUrl" CssClass="SiteUrl" onclick="ShowDynamicHelp('dynamicHelp',2);" /><asp:HyperLink Runat="server" ID="hypSiteUrl" />
	<asp:RequiredFieldValidator ID="txtSiteUrlRequiredValidator" Runat="server" ControlToValidate="txtSiteUrl"
	  Display="Dynamic" ErrorMessage="Site URL is required" /><br />
	
	<div class="standardFieldLabel">Initial Category:</div>
	<asp:DropDownList Runat="server" ID="ddlCategories" onmouseover="ShowDynamicHelp('dynamicHelp',3)" onmouseout="HideDynamicHelp('dynamicHelp');" /><asp:Literal Runat="server" ID="litCategory" />
	<asp:RequiredFieldValidator ID="ddlCategoriesRequiredValidator" Runat="server" ControlToValidate="ddlCategories"
	  Display="Dynamic" ErrorMessage="Initial category is required" /><br />
	
	<asp:Button Runat="server" ID="cmdSave" Text="Save" />
	<asp:Button runat="server" ID="cmdDelete" Text="Delete" />
	<asp:Button Runat="server" ID="cmdCancel" Text="Cancel" />
</asp:Content>