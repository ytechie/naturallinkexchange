<%@ Page language="c#" Inherits="Nle.Website.Members.Payment_Settings._Default" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Register Src="~/Members/Payment-Settings/LinkPackagesTable.ascx" TagPrefix="LP" TagName="LinkPackagesTable" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content runat="Server" ContentPlaceHolderID="mainContent">
	<h1>Subscription Information</h1>
	
	<asp:PlaceHolder Runat="server" ID="ControlPlaceholder" />
	
	<div class="standardFieldLabel">Plan Name:</div> <asp:literal Runat="server" ID="lblPlanName" /><br />
	<div class="standardFieldLabel">Plan Start:</div> <asp:literal Runat="server" ID="lblPlanStart" /><br />
	<div class="standardFieldLabel">Plan End:</div> <asp:literal Runat="server" ID="lblPlanEnd" /><br />
					
	<p>
		If you would like to upgrade your account, change the details of an existing subscription,
		or cancel your account, please visit our <a href="Account-Upgrade.aspx">account upgrade page</a>.
	</p>
	
	<!-- The table that lists the available link packages -->
	<LP:LinkPackagesTable runat="server" id="tblLinkPackages" />
</asp:Content>