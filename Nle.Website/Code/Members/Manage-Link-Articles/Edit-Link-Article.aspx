<%@ Page language="c#" Inherits="Nle.Website.Members.Manage_Link_Articles.EditLinkArticle"
    validateRequest="false" CodeFile="Edit-Link-Article.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/payment-settings/default.aspx" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/DynamicHelp.js"></script>
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Manage-Link-Articles/ManageLinkArticles.js"></script>

    <nle:DynamicHelp ID="dynamicHelp" runat="server">
	    <nle:DynamicHelpText runat="server" ID="dhtxtArticleTitle" HelpId="1" Title="Link Article Title" Text="<p>Your article will rotate with other articles as it's placed on other sites. If this article is the first article on the page, this value will be placed in the TITLE tag of that page. This will give your link text more value in the current search engine alorithms.</p><p>Your article title can be up to 80 characters long.</p><p><b>Tips:</b></p><p>We strongly suggest the following guidelines to avoid spammy title tags:</p><ul><li>A title tag should contain specific phrases related to the text on the page if possible</li><li>Do not use ALL CAPS</li><li>Do not repeat a word in sequence ('buy my widget widget sellers') if you can avoid it</li><li>If you must repeat a word sequencially use a pipe symbol ('|') not a comma - commas in the TITLE tag are considered spammy by the search engines</li><li>Just to be safe do not use the same word more than twice in the title.</li></ul>" />
	    <nle:DynamicHelpText runat="server" ID="dhInsertAnchor1" HelpId="2" Title="Insert 1st Keyword" Text="<p>Easily insert the keyword into your article paragraph. Remember, these keywords are replaced with the specified link (anchor) when we generate your link page.</p><p>Insert your first keyword at the location you want your first link to be positioned in your link paragraph. To get the most value out of your link try to place the link inside your paragraph as opposed to the beginning or the end.</p>" />
	    <nle:DynamicHelpText runat="server" ID="dhInsertAnchor2" HelpId="3" Title="Insert 2nd Keyword" Text="<p>Easily insert the keyword into your article paragraph. Remember, these keywords are replaced with the specified link (anchor) when we generate your link page.</p><p>Insert your second keyword at the location you want your second link to be positioned in your link paragraph. To get the most value out of your link try to place the link inside your paragraph as opposed to the beginning or the end.</p>" />
	    <nle:DynamicHelpText runat="server" ID="dhcmdSaveDraft" HelpId="4" Title="Save as Draft" Text="<p>This will save the article for you, but the article will not be activated for use until you have choose to Save and Publish.</p><p><i>It is recommended that you use 'Save as Draft' until you are certain that the article is just the way you want it.  Once the article is in use, changes made to it will ripple through other users' site faster than during the initial Publishing and may not appear as natural to search engines.</i></p>" />
	    <nle:DynamicHelpText runat="server" ID="dhcmdPublish" HelpId="5" Title="Save and Publish" Text="<p>This will save the article and publish the article for use.</p><p><i>It is not recommended to modify the article once it has published as the modifications ripple through other users' sites faster than during initial publication and may not appear as natural to search engines.  It is instead recommended that you not publish until you are sure you have the article the way you want it.</i></p>" />
	    <nle:DynamicHelpText runat="server" ID="dhtxtArticle" HelpId="6" Title="Link Article" Text="<p>Use this area to define the articles that will be used on other sites' link pages to talk about and link to your site.</p>" />
	    <nle:DynamicHelpText runat="server" ID="dhcmdDelete" HelpId="7" Title="Delete" Text="<p>Deletes the current article.  Once an article has been deleted, it gradually ripples out to take affect on other sites as in order to appear more natural.</p>" />
    </nle:DynamicHelp>

	<h1>Article Editor</h1>
	
	<asp:Panel runat="server" ID="pnlDistribution" CssClass="LargTextBlock Blue">
	    <p>
	        This group that this article belongs to is currently configured to have a 0% distribution.  This means that this article
	        will not be used by our system.  By default when a new article group is created, it has a 0% distribution,
	        that way you decide when you are ready for it to be used in the system.  If you would like to modify the distrubution
	        or learn more about distribution, visit the <asp:HyperLink runat="server" ID="lnkDistribution" NavigateUrl="~/Members/Manage-Article-Distribution/">
	        Manage Article Distribution</asp:HyperLink> section on the Control Panel.  We will also display
	        a shortcut to this section by your article group definition once you hit the Save button.
	    </p>
	</asp:Panel>
	
	<div class="standardFieldLabel">Site:</div>
	<asp:Literal ID="litSite" Runat="server" />
	<br />
	
	<div class="standardFieldLabel">Article Group:</div>
	<asp:Literal Runat="server" ID="litArticleGroups" />
	<br />
	
	<div class="standardFieldLabel">1st: Keyword:</div>
	<asp:Literal runat="server" ID="litKeyword1" />
	<asp:HyperLink runat="server" ID="lnkKeyword1" />
	<br />
	
	<asp:Panel runat="server" ID="pnlKeyword2">
	    <div class="standardFieldLabel">2nd: Keyword:</div>
	    <asp:Literal runat="server" ID="litKeyword2" />
	    <asp:HyperLink runat="server" ID="lnkKeyword2" />
	    <br />
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlUpgradeAccount">
	    <br />
	    <div class="Information">
	        <asp:Image Runat="server" ID="imgInfo" AlternateText="Information" ImageUrl="~/Images/iCase.gif" />
	        <p>
	            Your subscription level only permits you to utilize 1 link in your articles.
	            If you would like to utilize both links, you can upgrade your subscription level.
	            <br />
	            <a href="../Payment-Settings/">Click here</a> to upgrade your subscription.
	        </p>
	    </div>
	</asp:Panel>
    <br />
	
	<div class="standardFieldLabel">Article Title:</div>
	<asp:TextBox Runat="server" ID="txtArticleTitle" MaxLength="80" CssClass="articleTitle" onclick="ShowDynamicHelp('dynamicHelp', 1);" />
	<asp:RegularExpressionValidator Runat="server" ControlToValidate="txtArticleTitle"
	  Display="Dynamic" ValidationExpression="[\S\s]{0,80}" ErrorMessage="Article title must be 80 characters or less." />
	<asp:RequiredFieldValidator Runat="server" ControlToValidate="txtArticleTitle"
	  Display="Dynamic" ErrorMessage="Article title is required" />
	<br />
	<br />

	<asp:Button ID="cmdInsertKeyword1" Runat="server" Text="Insert 1st Keyword" CssClass="SourceCodeButton" onmouseover="ShowDynamicHelp('dynamicHelp', 2);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:Button ID="cmdInsertKeyword2" runat="server" Text="Insert 2nd Keyword" CssClass="SourceCodeButton" onmouseover="ShowDynamicHelp('dynamicHelp', 3);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:RegularExpressionValidator Runat="server" ValidationExpression="[\S\s]{0,500}" Display="Dynamic"
	  ControlToValidate="txtArticle" ErrorMessage="Article must be 500 characters or less." />
	<br />
	<asp:TextBox Runat="server" ID="txtArticle" TextMode="MultiLine" Rows="10" MaxLength="500" CssClass="articleBox" onclick="ShowDynamicHelp('dynamicHelp', 6);" /><br />
	<asp:Button Runat="server" ID="cmdSaveDraft" Text="Save as Draft" onmouseover="ShowDynamicHelp('dynamicHelp', 4);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:Button Runat="server" ID="cmdPublish" Text="Save and Publish" onmouseover="ShowDynamicHelp('dynamicHelp', 5);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:Button Runat="server" ID="cmdDelete" Text="Delete" onmouseover="ShowDynamicHelp('dynamicHelp', 7);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:Button Runat="server" ID="cmdCancel" Text="Cancel" />
</asp:Content>