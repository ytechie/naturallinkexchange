<%@ Reference Page="~/members/link-page-design/default.aspx" %>

<%@ Page Language="c#" MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
<asp:Content runat="Server" ContentPlaceHolderID="mainContent">

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/DynamicHelp.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Link-Page-Design/LinkPageDesign.js"></script>

    <div class="StepText">
        <h1 style="text-align: center">
            NaturalLinkExchange Tutorial Videos</h1>
        <p>
            Here are the tutorial videos on using NaturalLinkExchange.com. These all require
            the free <a href="http://www.macromedia.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash&promoid=BIOX">
                Macromedia Flash player</a>. Enjoy and if you have any questions or even better,
            suggestions, <a href="../../Support/Default.aspx">let us know</a>.</p>
        <p>
            <a href="Control Panel & Site Settings.htm">Using the Control Panel & Configuring Site
                Settings</a></p>
        <p>
            <a href="Article Group Setup.htm">Setting Up Your Articles and Article Groups Settings</a></p>
        <p>
            <a href="Article Distribution Editor.htm">Setting Your Article Group Distributions Settings</a></p>
        <p>
            <a href="Editing Your Link Page.htm">Creating and Editing Your Link Page (Using the Template Wizard)</a></p>
        <!--
    <blockquote>
        <p>
            You can now access your favorites from any computer that has Internet access. It
            also tells you how popular the links you store are. Very mellow interface but powerful.</p>
    </blockquote>
    -->
    </div>
    <asp:PlaceHolder runat="server" ID="javaScriptPlaceholder" />
</asp:Content>
