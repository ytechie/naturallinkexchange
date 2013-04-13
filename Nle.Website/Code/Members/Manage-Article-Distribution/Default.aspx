<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Default.aspx.cs"
    Inherits="Nle.Website.Members.Manage_Article_Distribution.ManageArticleDistributionDefault" %>

<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="Server">
    <h1>
        Set the Distribution Values For Your Article Groups</h1>
        <div class="StepText">
    <p>
        You've set up your article groups, your keywords and your articles. Now you
        just need to indicate how often you want each keyword/article group to be distributed. If
        all of your links pointed back to your site using the same keywords, the search
        engines would be suspicious. This feature helps make sure your link distribution
        from NaturalLinkExchange.com looks natural by allowing you to indicate how
        often each keyword phrase(s) should be used.
    </p>
    <p>
        Just set the distribution by article group below by entering a percentage in the
        grid next to each keyword/article group. Your total must add up to 100% or you will not
        be able to move to the next step in the Control Panel.
    </p>
    <p>
        If you need help setting this up, check out our video on <a id="A4" runat="server"
            href="~/Videos/Article Group Distribution.htm">Setting Your Article Group Distributions</a> as well as <a id="A5" runat="server"
                href="~/Videos/">other helpful videos</a>. If you still need help please <a id="A2" runat="server"
                    href="~/Support/">contact us by opening a support case.</a>
    </p>
        </div>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Manage-Article-Distribution/ManageDistributions.js"></script>

    <asp:PlaceHolder runat="server" ID="JavascriptPlaceholder" />
    <asp:Table runat="server" ID="Distributions" CssClass="DistributionTable" CellPadding="0"
        CellSpacing="0">
        <asp:TableHeaderRow CssClass="DistributionTable_HeaderRow">
            <asp:TableHeaderCell>Article Group</asp:TableHeaderCell>
            <asp:TableHeaderCell>Link 1</asp:TableHeaderCell>
            <asp:TableHeaderCell>Link 2</asp:TableHeaderCell>
            <asp:TableHeaderCell ColumnSpan="2">Distribution</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
    <div class="DistributionTable">
        Total:
        <asp:Label ID="lblTotal" runat="server" /><br />
    </div>
    <asp:Button runat="server" ID="cmdSave" Text="Save" />
    <asp:Button runat="server" ID="cmdCancel" Text="Cancel" />
</asp:Content>
