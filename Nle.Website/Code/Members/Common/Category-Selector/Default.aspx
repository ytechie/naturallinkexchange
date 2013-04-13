<%@ Page language="c#" Inherits="Nle.Website.Members.Common.CategorySelector" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="catt" Namespace="Nle.Controls" Assembly="Nle.Framework" %>

<asp:Content runat="Server" ContentPlaceHolderID="mainContent">
	<asp:PlaceHolder Runat="server" ID="controlPlaceholder" />		

	<div class="tree">
		<catt:CategoryList id="categoryTree" runat="server" />
	</div>
	
	<div>
		<asp:Button runat="server" ID="cmdOk" />
		<input type="button" onclick="window.close();" value="Cancel" />
	</div>
</asp:Content>