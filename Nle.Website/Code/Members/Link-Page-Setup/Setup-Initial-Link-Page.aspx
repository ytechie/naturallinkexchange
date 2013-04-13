<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Setup-Initial-Link-Page.aspx.cs"
    Inherits="Members_Link_Page_Setup_Setup_Initial_Link_Page" Title="Initial Link Page Setup" %>

<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <p>
        Congratulations, you are almost ready to start using your link pages! Now that you have everything
        set up, we're going to do the following:
    </p>
    <ul>
        <li>
            Turn on the linking for your site, which will allow you to start receiving links.
        </li>
        <li>
            Create a link to your site from another members site.
        </li>
        <li>
            An initial link page will be created.  To start, there will be no outgoing link articles.
            You will see a temporary sample article until you have an outgoing link from that page.  It
            may take some time to get an outgoing link on that page, because another members site must
            match the category of that page, and it must be their turn to get a link.
        </li>
        <li>
            Load some RSS feeds on your link page, so you can see what they look like.
        </li>
    </ul>
    <asp:Button runat="server" ID="cmdReady" Text="I'm Ready!" />
    <asp:PlaceHolder runat="server" ID="contentPlaceholder" />
</asp:Content>

