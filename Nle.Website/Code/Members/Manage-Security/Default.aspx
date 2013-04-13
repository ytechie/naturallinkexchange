<%@ Page language="c#" Inherits="Nle.Website.Members.Manage_Security.ManageSecurityDefault"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
	<h1>Manage Security</h1>

	<h2>Change Your Password</h2>
	<div class="standardFieldLabel">New Password:</div>
	<asp:TextBox Runat="server" ID="txtPassword" TextMode="Password" />
	<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
	    Display="Dynamic" ErrorMessage="You must enter a non-blank password" />
	<br />
	<div class="standardFieldLabel">New Password (verify):</div>
	<asp:TextBox Runat="server" ID="txtPasswordVerify" TextMode="Password" />
	<asp:CompareValidator Runat="server" ControlToValidate="txtPassword" ControlToCompare="txtPasswordVerify"
	  Display="Dynamic" ErrorMessage="Passwords do not match!" Operator="Equal" />
	<br />
	<asp:Button Runat="server" ID="cmdChangePassword" Text="Change Password" /><br />
	
	<b><asp:Literal Runat="server" ID="lblChangeStatus" /></b>
</asp:Content>