<%@ Page language="c#" Inherits="Nle.Website.Members.CategoryAdministrationDefault" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/common/category-selector/default.aspx" %>
<%@ Register TagPrefix="catt" Namespace="Nle.Controls" Assembly="Nle.Framework" %>
  
<asp:Content runat="Server" ContentPlaceHolderID="mainContent">
	Categories:<br />
	<div id="sourceTree">
		<catt:CategoryList id="categoryTree" runat="server" />
	</div>
	
	<div class="divPanel">
		Category Details<br />
		<br />
		Category Name: <asp:TextBox Runat="server" ID="txtCategoryName" /><br />
		<br />
		Related categories<br />
		<asp:ListBox Runat="server" ID="lstRelatedCategories" /><br />
		<input type="button" id="cmdAdd" runat="server" value="Add" />
		<asp:Button Runat="server" ID="cmdRemove" Text="Remove" />
		<input type="hidden" id="txtNewRelatedCategoryId" runat="server" />
		<br />
		All Related Categories (Preview)<br />
		<asp:ListBox Runat="server" ID="lstAllRelatedCategories" />
    </div>
    
    <asp:PlaceHolder runat="Server" ID="controlPlaceholder" />
</asp:Content>