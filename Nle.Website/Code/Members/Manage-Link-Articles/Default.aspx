<%@ Page Language="c#" Inherits="Nle.Website.Members.Manage_Link_Articles.ManageLinkArticlesDefault"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/manage-link-articles/edit-link-article.aspx" %>
<%@ Reference Page="~/members/payment-settings/default.aspx" %>
<%@ Reference Page="~/members/manage-article-distribution/default.aspx" %>
<%@ Reference Page="~/members/manage-link-articles/edit-link-article-group.aspx" %>
<%@ Register TagPrefix="nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
<asp:Content runat="server" ContentPlaceHolderID="mainContent">

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/DynamicHelp.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Manage-Link-Articles/ManageLinkArticles.js"></script>

    <nle:DynamicHelp ID="dynamicHelp" runat="Server">
		<nle:DynamicHelpText runat="server" ID="dhtSiteSelector" HelpId="999" Title="Site Selector" Text="<p>Use the Site Selector to easily switch between sites to configure that site.</p>" />
    </nle:DynamicHelp>
    <h1>
        3 Easy Steps To Setup Your Articles</h1>
    <div id="PageInfo" class="StepText">
        <p>
            <asp:Image runat="server" ImageUrl="~/Images/step1.gif" CssClass="StepImage" />First
            determine your top 2 or 3 keywords that you want your site to rank for. For example
            if you sell guitars on your site, you might want to be found for "electric guitar"
            most of the time and if possible "bass guitar" and "acoustic guitar" too. <a id="A6"
                runat="server" href="~/Support/">Need help determining your keywords?</a><!-- (point to an article here - possibly offer a keyword report)-->
        </p>
        <br />
        <p>
            <asp:Image runat="server" ImageUrl="~/Images/step2.gif" CssClass="StepImage" />Now
            create 2 or 3 article groups; one for each of the keyword phrases you selected.
            Just click the "New Group" button located below to start. Then name each group so
            you know what keywords the group is about. Enter the keyword as the link text and
            enter the address of the page (also known as the URL) you want that keyword to point
            to.
        </p>
        <br />
        <p>
            <asp:Image runat="server" ImageUrl="~/Images/step3.gif" CssClass="StepImage" />Then
            just write a short paragraph (100-200 words at the most) about each keyword phrase
            you selected. Try to use the keyword phrase inside of the paragraph; this will give
            it more value to the search engines. Use a word processor so you can check the spelling
            and grammar. Then under the appropriate group just click the "Add Article" button
            to add this new article.
        </p>
        <br />
        <p>
            That's it! If you still need help, check out our video on <a id="A4" runat="server"
                href="~/Videos/Article Group Setup.htm">Setting Up Your Article Groups</a> as
            well as <a id="A5" runat="server" href="~/Videos/">other helpful videos</a>. If
            you still need help please <a id="A2" runat="server" href="~/Support/">contact us by
                opening a support case.</a>
        </p>
        <asp:Panel runat="server" ID="pnlFreeSilverDesc1">
            <h3>
                Upgrade and Get More</h3>
            <p>
                Your account only allows 3 groups with only 1 keyword phrase and
                <asp:Literal runat="server" ID="descFreeSilver_NumOfArticles" />
                per group. The more groups you have the more keywords you can focus on. The more
                keywords per group that you have the more links you will get and the more you will
                be able to vary your link text. Varying your keywords is a neccesity to get good
                rankings. The more articles you have the better because it makes your text unique
                on multiple sites and it helps you avoid having duplicate anchor text. <a id="A1"
                    runat="server" href="~/Members/Payment-Settings/Account-Upgrade.aspx">Upgrade to
                    a
                    <asp:Literal runat="server" ID="descFreeSilver_NextLevel" />
                    account</a> to more out of NaturalLinkExchange.
            </p>
        </asp:Panel>
    </div>
    <asp:HyperLink runat="server" ID="lnkAddNewGroup" /><br />
    <asp:PlaceHolder runat="server" ID="controlPlaceholder" />
    <h3>
        A Note On Changing and Deleting Articles</h3>
    <p class="StepText">
        If you edit an article, the changes will <b>immediately show up</b> on the link
        pages in the system. So use the Save As Draft button in the Article Editor until
        you are sure you wish to publish the article. If you delete an article, the delete
        happens slowly, so that you do not suffer penalties in the search engines. Keep
        this in mind if you need to change an article. You may want to delete it and then
        create a new one in its place to minimize the impact on the search engines.
    </p>
</asp:Content>
