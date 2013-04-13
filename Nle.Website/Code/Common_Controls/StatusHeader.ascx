<%@ Control Language="c#" Inherits="Nle.Website.Common_Controls.StatusHeader" CodeFile="StatusHeader.ascx.cs" %>

<%@ Register TagPrefix="ss" Namespace="Nle.Website.Common_Controls" %>

<div class="topRight">
	<asp:Panel Runat="server" ID="pnlNotLoggedIn">
		<a href="<%= Nle.Website.Global.VirtualDirectory %>Members/">You are currently NOT logged in.</a>
	</asp:Panel>
	<asp:Panel Runat="server" id="pnlLoggedIn" Visible="False">
		You are currently logged in as <asp:Literal Runat="server" ID="lblUser" />.
		<asp:LinkButton Runat="server" ID="cmdLogOff" text="(log off)" ValidationGroup="None" /> <br />
		
		<ss:SiteSelector runat="Server" id="ssSiteSelector" />
	</asp:Panel>
</div>