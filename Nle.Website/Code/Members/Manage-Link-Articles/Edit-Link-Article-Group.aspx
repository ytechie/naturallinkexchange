<%@ Page language="c#" Inherits="Nle.Website.Members.Manage_Link_Articles.EditLinkArticleGroup"
    CodeFile="Edit-Link-Article-Group.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/payment-settings/default.aspx" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Manage-Link-Articles/ManageLinkArticles.js"></script>
    
	<nle:DynamicHelp ID="dynamicHelp" runat="server">
		<nle:DynamicHelpText runat="server" ID="dhtxtLinkGroupTitle" HelpId="1" Title="Article Group Name" Text="<p>Just enter a name for this group here so you can remember what it's about.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhLinkTitle" HelpId="3" Title="Keyword" Text="<p>Enter the keyword here.  This is the word or phrase that our system searches for in your article that indicates where you want your link created.</p><p><b>Example:</b> To create the link <a href='http://www.natruallinkexchange.com'>Natural Link Exchange</a>, 'Natural Link Exchange' would be your keyword.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhLinkUrl" HelpId="4" Title="Link URL" Text="<p>Enter the address (i.e. URL) of a page on your site that you want this link to point to. The address defaults to and must begin with your site's home page but you can do some deep linking by adding a specific page here. (e.g /bassguitar.html)</p>" />
		<nle:DynamicHelpText runat="server" ID="dhcmdDelete" HelpId="5" Title="Delete" Text="<p>Deletes the current article group and all of its articles. To avoid raising search engine suspicion, when an article group has been deleted it gradually ripples out to take affect on other sites.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhcmdSave" HelpId="6" Title="Save" Text="<p><i>Try to avoid changing the links in your groups after you publish them. Make sure they are the links you want and they are active.</i> The ability to change the link anchor text and the URL is a valuable feature of the system but you should try not to change them too often so they don't look unnatural.</p>" />
		<nle:DynamicHelpText runat="server" ID="dhKeywordReplacementText" HelpId="7" Title="Replacement Text" Text="<p>If you would like the link text of the generated link to be different from your keyword, enter that link text here.</p><p><b>Example:</b> To create the link <a href='http://www.natruallinkexchange.com'>Natural Link Exchange</a> where the keyword NLE appears in your article, 'Natural Link Exchange' would be your replacement text.</p>" />
	</nle:DynamicHelp>

	<h1>Article Group Editor</h1>
	
	<asp:Panel runat="server" ID="pnlDistribution" CssClass="LargTextBlock Blue">
	    <p>
	        This group is currently configured to have a 0% distribution.  This means that articles that are defined in this
	        group will not be used by our system.  By default when a new article group is created, it has a 0% distribution,
	        that way you decide when you are ready for it to be used in the system.  If you would like to modify the distrubution
	        or learn more about distribution, visit the <asp:HyperLink runat="server" ID="lnkDistribution" NavigateUrl="~/Members/Manage-Article-Distribution/">
	        Manage Article Distribution</asp:HyperLink> section on the Control Panel.  We will also display
	        a shortcut to this section by your article group definition once you hit the Save button.
	    </p>
	</asp:Panel>
	
	<div class="standardFieldLabel">Site:</div>
	<asp:Literal ID="litSite" Runat="server" />
	<br />
	<br />
	
	<div class="standardFieldLabel">Article Group Name:</div>
	<asp:TextBox Runat="server" ID="txtLinkGroupTitle" MaxLength="50" CssClass="groupTitle" onclick="return ShowDynamicHelp('dynamicHelp', 1);" />
	<asp:RequiredFieldValidator ID="txtLinkGroupTitleValidator" Runat="server" ControlToValidate="txtLinkGroupTitle" Display="Dynamic" ErrorMessage="You must give your article group a name" />
	<br />
	
	<div style="font-size: 12pt;" class="LargTextBlock">
        <p>
	        Below is an area that you can define up to 2 keywords to use in your link articles that you later define in this group.
	        Keywords are special words or phrases that our system will search for in your articles contained in this group and turn them
	        into links for you.	    
	    </p>
	    <p>
	        <b>Example:</b> Say you define your first keyword as <i>Natural Link Exchange</i> and your URL as <i>http://www.naturallinkexchange.com</i>.
	        Then our system will search for the first instance of <i>Natural Link Exchange</i> and turn it into a link that links to <i>http://www.naturallinkexchange.com</i>.
	    </p>
	    <p style="font-size: 11pt;">
	        <b>Before:</b> Visit Natural Link Exchange to save time, money, and headaches while safely getting the search engines to love your website - for free.
	        <br />
	        <b>After:</b> Visit <a href="http://www.naturallinkexchange.com">Natural Link Exchange</a> to save time, money, and headaches while safely getting the search engines to love your website - for free.
	    </p>
	</div>
	
	<b>1st Keyword:</b>
	<br />
	<div class="standardFieldLabel">Keyword:</div>
	<asp:TextBox runat="Server" ID="txtKeyword1" MaxLength="50" CssClass="linkTitle" onclick="return ShowDynamicHelp('dynamicHelp', 3);" />
	<asp:RegularExpressionValidator Runat="server" ControlToValidate="txtKeyword1" Display="Dynamic" ValidationExpression=".{0,50}" ErrorMessage="Keyword is more than 50 characters" />
	<asp:CheckBox runat="server" ID="chkAdvanced1" Text="Advanced" CssClass="AdvancedCheckbox" />
	<asp:Label runat="server" ID="lblKeywordRequired1" Text="The keyword is required." CssClass="Red" Visible="false" />
	<br />
	
	<asp:Panel runat="server" ID="pnlReplacementText1">
	    <div class="standardFieldLabel">Replacement Text:</div>
	    <asp:TextBox runat="Server" ID="txtReplacementText1" MaxLength="50" CssClass="linkTitle" onclick="return ShowDynamicHelp('dynamicHelp', 7);" />
	    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtReplacementText1" Display="Dynamic" ValidationExpression=".{0,50}" ErrorMessage="Replacement text is more than 50 characters" />
	    <br />
	</asp:Panel>

	<div class="standardFieldLabel">URL:</div>
	<div runat="server" id="divLink1" class="Url" onclick="ShowDynamicHelp('dynamicHelp', 4);">
	<asp:Literal Runat="server" ID="litBaseUrl1" /><asp:TextBox Runat="server" ID="txtUrl1" MaxLength="100" CssClass="linkUrl" onclick="return ShowDynamicHelp('dynamicHelp', 4);" />
	</div>
	<asp:RegularExpressionValidator Runat="server" ID="txtLink1DestinationValidator1" ControlToValidate="txtUrl1" Display="Dynamic" ValidationExpression=".{0,100}" ErrorMessage="URL is more than 100 characters" />
	<br />
	<br />
	
	<b>2nd Keyword:</b>
	<br />
	<asp:Panel runat="server" ID="pnlKeyword2">
	    <div class="standardFieldLabel">Keyword:</div>
	    <asp:TextBox Runat="server" ID="txtKeyword2" MaxLength="50" CssClass="linkTitle" onclick="return ShowDynamicHelp('dynamicHelp', 3);" />
	    <asp:RegularExpressionValidator Runat="server" ControlToValidate="txtKeyword2" Display="Dynamic" ValidationExpression=".{0,50}" ErrorMessage="Keyword is more than 50 characters" />
	    <asp:CheckBox runat="server" ID="chkAdvanced2" Text="Advanced" CssClass="AdvancedCheckbox" />
	    <asp:Label runat="server" ID="lblKeywordRequired2" Text="The keyword is required." CssClass="Red" Visible="false" />
	    <br />
    	
    	<asp:Panel runat="server" ID="pnlReplacementText2">
	        <div class="standardFieldLabel">Replacement Text:</div>
	        <asp:TextBox runat="Server" ID="txtReplacementText2" MaxLength="50" CssClass="linkTitle" onclick="return ShowDynamicHelp('dynamicHelp', 7);" />
	        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtReplacementText2" Display="Dynamic" ValidationExpression=".{0,50}" ErrorMessage="Replacement text is more than 50 characters" />
	        <br />
	    </asp:Panel>

	    <div class="standardFieldLabel">URL:</div>
	    <div runat="server" id="divLink2" class="Url" onclick="ShowDynamicHelp('dynamicHelp', 4);">
	    <asp:Literal Runat="server" ID="litBaseUrl2" /><asp:TextBox Runat="server" ID="txtUrl2" MaxLength="100" CssClass="linkUrl" onclick="return ShowDynamicHelp('dynamicHelp', 4);" />
	    </div>
	    <asp:RegularExpressionValidator Runat="server" ID="txtLink2DestinationValidator1" ControlToValidate="txtUrl2" Display="Dynamic" ValidationExpression=".{0,100}" ErrorMessage="URL is more than 100 characters" />
	    <br />
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlUpgradeAccount" CssClass="Information">
	    <asp:Image Runat="server" ID="imgInfo" AlternateText="Information" ImageUrl="~/Images/iCase.gif" />
	    <p>
	        Your subscription level only permits you to utilize 1 link in your articles.
	        If you would like to utilize both links, you can upgrade your subscription level.
	        <br />
	        <a href="../Payment-Settings/">Click here</a> to upgrade your subscription.
	    </p>
	</asp:Panel>
	<br />
	
	<asp:Button Runat="server" ID="cmdSave" Text="Save" onmouseover="ShowDynamicHelp('dynamicHelp', 6);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:Button Runat="server" ID="cmdDelete" Text="Delete" onmouseover="ShowDynamicHelp('dynamicHelp', 5);" onmouseout="HideDynamicHelp('dynamicHelp');" />
	<asp:Button Runat="server" ID="cmdCancel" Text="Cancel" />
</asp:Content>