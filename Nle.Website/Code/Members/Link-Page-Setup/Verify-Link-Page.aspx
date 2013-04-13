<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Verify-Link-Page.aspx.cs" Inherits="Members_Link_Page_Setup_Verify_Link_Page"
    Title="Verify Link Page" %>

<asp:Content ContentPlaceHolderID="mainContent" Runat="Server">
    <asp:PlaceHolder runat="server" ID="JavaScriptPlaceholder" />
    <p>
        This page can be used to verify that you link page is working correctly.
    </p>
    
    Last Known Status: <b><asp:Literal runat="server" ID="litLastStatus" /></b><br />
    Last Known Link Page URL: <asp:TextBox runat="server" ID="txtLinkPageUrl" Width="300" ReadOnly="true" /><br />
    <br />
    <asp:Button runat="server" Text="Check Now" ID="cmdVerify" /><br />
    <asp:Panel runat="Server" ID="pnlCongratulations" Visible="false">
        <h2>Congratulations on getting your site set up!</h2>
        <p>
            For all of your hard work, we're pleased to offer you some
            <a href="../Tools/">free resources</a> that we've compiled that we
            know you'll be interested in.
        </p>
        <p>
            <a href="../Tools/">Click here for the list of free tools</a>
        </p>
    </asp:Panel>
</asp:Content>

