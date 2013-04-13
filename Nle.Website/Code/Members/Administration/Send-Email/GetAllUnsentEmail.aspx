<%@ Page language="c#" Inherits="Nle.Website.Members.Administration.Send_Email.GetAllUnsentEmail"
    validateRequest="false" CodeFile="GetAllUnsentEmail.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
			<asp:PlaceHolder Runat="server" ID="controlPlaceholder" />
			<asp:DataGrid Runat="server" ID="dgEmailTable" />
</asp:Content>