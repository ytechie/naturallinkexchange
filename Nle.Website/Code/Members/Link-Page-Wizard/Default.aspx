<%@ Reference Page="~/members/link-page-design/default.aspx" %>

<%@ Page Language="c#" Inherits="Nle.Website.Members.Link_Page_Wizard._Default" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
<asp:Content runat="Server" ContentPlaceHolderID="mainContent">

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/DynamicHelp.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Link-Page-Design/LinkPageDesign.js"></script>

    <Nle:DynamicHelp ID="dynamicHelp" runat="Server">
		<nle:DynamicHelpText runat="server" ID="dhtRadWizardMode_Import" HelpId="1" Title="Wizard Mode" Text="<p>Allows you to choose how to set up your link page.</p><p><b>Import</b> (Recommended) Allows you to specify a URL of a web page (such as one on your website) that you want your link page to look like and the wizard will import that web page.</p><p><b>Upload</b> Allows you to upload a web page file as the template.</p><p><b>Template</b> Allows you to choose from a predefined template list to choose a link page design already created by us.  <i>It is recommened that you create your own template though to avoid leaving a footprint.</i></p>" />
		<nle:DynamicHelpText runat="server" ID="dhtTxtImportUrl" HelpId="2" Title="Import Url" Text="<p>Specify the URL of the webpage that you would like to use as a starting point for your link page.  You will be given the chance to edit the source code after the upload.</p><p><b>Example:</b> http://www.NaturalLinkExchange.com/default.aspx</p>" />
		<nle:DynamicHelpText runat="server" ID="dhtDdlStylesheets" HelpId="3" Title="Predefined Templates" Text="<p>Choose a theme for you link page.  You will be given a chance to customize the source code after you choose the theme.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhtCmdUpload" HelpId="4" Title="Upload File" Text="<p>Upload a template webpage file that you have created.  You will be given a change to customize the source code after the upload.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhtSiteSelector" HelpId="999" Title="Site Selector" Text="<p>Use the Site Selector to easily switch between sites to configure that site.</p>" />
    </Nle:DynamicHelp>
    <div class="StepText">
        <asp:Panel runat="server" ID="wizardPanel1">
            <!-- -->
            <h1>
                3 Easy Steps To Setup The Link Page To Host On Your Site</h1>
            <p>
                You need to host links on your site before you will begin receiving links to your
                site. This wizard will help you build this page - it's a very simple task. All you
                have to do is determine what type of template you want to use, select where to pull
                the source HTML, then fill in the token or placeholder to indicate where the system
                should place the articles to other sites as well as the categories and RSS feeds.
            </p>
            <p>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/step1.gif" CssClass="StepImage" />
                First, deterimine how you will build your links page. We stongly recommend that
                you use the <i>Import Template From A URL</i> option because it allows you to simply
                point to one of your own webpages which the system will use to build a starting
                template from. Then you just edit the HTML and take out the parts that are specific
                to the page you pointed to leaving only those parts that you want to be common on
                all of your pages. This is the easiest way to make your links page look like the
                rest of your site. Remember: you don't want to leave a footprint so making your
                links page look like the other pages on your site is important. After you select
                this option you will be prompted for a URL.</p>
            <p>
                If you decide to go with the <i>Upload a Template</i> option you will be prompted
                for an HTML file to upload. Likewise if you choose the <i>Use A Predefined Template</i>
                option you will be allowed to choose from one of our standard templates. We strongly
                suggest that you do not use this last option because it can leave a prominent footprint.
            </p>
            <br />
            <!--onmouseover="return ShowDynamicHelp('dynamicHelp', 1);"
                onclick="return ShowDynamicHelp('dynamicHelp', 1);" -->
            <!--<asp:ListItem Value="Predefined">Use A Predefined Template (only use if necessary - <a id="A1" runat="server" href="#">Why?</a>)</asp:ListItem> -->
            <asp:RadioButtonList runat="server" ID="radWizardMode">
                <asp:ListItem Value="Import" Selected="True">Import Template From A URL</asp:ListItem>
                <asp:ListItem Value="Upload">Upload A Template</asp:ListItem>
                <asp:ListItem Value="Predefined">Use A Predefined Template (only use if no other option is available)</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
        <asp:Panel runat="server" ID="importPanel">
            <p>
                <asp:Image ID="Image2a" runat="server" ImageUrl="~/Images/step2.gif" CssClass="StepImage" />Point
                to a page on your site that is a good representation of what your site looks and
                feels like. You might consider selecting a page that is one click from your index
                page so your template will contain a link back to your index page.
            </p>
            <br />
            <h2>
                Import Template From A URL</h2>
            URL:
            <asp:TextBox runat="server" ID="txtImportUrl" class="ImportUpload" />
        </asp:Panel>
        <asp:Panel runat="server" ID="predefinedPanel">
            <p>
                <asp:Image ID="Image2b" runat="server" ImageUrl="~/Images/step2.gif" CssClass="StepImage" /><b>We
                    strongly discourage you from using this option because all the sites that use the
                    same template will leave a footprint for the search engine spiders to see.</b>
                If you feel you have no other choice, just select one of our predefined templates.
                If you have questions <a id="A2" runat="server" href="~/Support/">contact us by opening
                    a support case.</a>.
            </p>
            <h2>
                Select a Predefined Template</h2>
            Theme:
            <asp:DropDownList runat="server" ID="ddlStylesheets">
                <asp:ListItem Value="00" Selected="True">Plain (no style)</asp:ListItem>
                <asp:ListItem Value="01">Red, White and Blue</asp:ListItem>
                <asp:ListItem Value="02">Newspaper</asp:ListItem>
            </asp:DropDownList>
        </asp:Panel>
        <asp:Panel runat="server" ID="uploadPanel">
            <p>
                <asp:Image ID="Image2c" runat="server" ImageUrl="~/Images/step2.gif" CssClass="StepImage" />Many
                webmasters already have a template that they use for pages they add to their website.
                If that is true your case, you can select that HTML file here. The wizard will upload
                it and create the template to be used as your links page.
            </p>
            <h2>
                Upload Template</h2>
            File:
            <input type="file" runat="server" id="cmdUpload" size="50" />
        </asp:Panel>
    </div>
    <br />
    <div class="wizardNavigation">
        <asp:Button runat="server" ID="cmdCancel" Text="Cancel" />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button runat="server" ID="cmdPrevious" Text="Previous" />
        <asp:Button runat="server" ID="cmdNext" Text="Next" />
    </div>
    <asp:PlaceHolder runat="server" ID="javaScriptPlaceholder" />
</asp:Content>
