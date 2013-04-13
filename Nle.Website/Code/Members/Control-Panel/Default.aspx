<%@ Page language="c#" Inherits="Nle.Website.Members.ControlPanel" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<%@ Reference Page="~/members/link-page-setup/default.aspx" %>
<%@ Reference Page="~/members/link-page-wizard/default.aspx" %>
<%@ Reference Page="~/members/manage-link-articles/default.aspx" %>
<%@ Reference Page="~/members/manage-sites/default.aspx" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content runat="Server" ContentPlaceHolderID="mainContent">
	<asp:PlaceHolder Runat="server" ID="controlPlaceholder" />

    <asp:Panel ID="pnlPromotion" runat="server">
	    <h2>
		    <img src="Promotion.gif" alt="Account Logo" />
		    Promotions			
	    </h2>
	    <ul class="menuList">
		    <li>
		        <Nle:ControlPanelItem ID="cpiUpgradeSite" runat="server" NavigateUrl="../Payment-Settings/Account-Upgrade.aspx"
		            Title="Upgrade Your Site" Description="Upgrade your free account to a Silver or Gold account and get more links and more features." />
		    </li>
		    <li>
		        <Nle:ControlPanelItem ID="cpiAffiliateProgram" runat="server" NavigateUrl="../Affiliate-Program/"
		            Title="Promote & Profit" Description="Promote Natural Link Exchange and we'll pay you up to $120/site." />
		    </li>
	    </ul>
	</asp:Panel>

	<asp:Panel ID="pnlLinkingPanel" Runat="server">
		<h2>
			<img src="Links.gif" alt="Linking Logo" />
			Site Configuration
		</h2>
		<ul class="menuList">
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiSiteSettings" NavigateUrl="../Manage-Site-Parameters" Title="Site Settings"
			    Description="Configure individual settings on your sites.  Use this to define settings such as how often the service should add links.  These settings are pre-populated with default values for your convinience when you add site, but we encourage you to review and modify the settings to your liking." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiManageArticles" NavigateUrl="../Manage-Link-Articles/" Title="Manage My Articles" Description="Set up your article groups, articles, incoming link text, and article distribution." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiManageDistribution" NavigateUrl="../Manage-Article-Distribution/" Title="Manage Article Distribution" Description="Once your article groups and articles are set up, choose how often you want articles from each group being used." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiArticlePageWizard" NavigateUrl="../Link-Page-Wizard/" Title="Article Page Template Wizard"
			    Description="Quickly and easily set up your article pages with easy to use guided help.  Design your article pages to match the look and feel of your website, or choose a new unique look. This step is optional as you can manually modify the page template below." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiArticlePageEdit" NavigateUrl="../Link-Page-Design/" Title="Manually Edit My Article Page Template"
			    Description="Manually edit the HTML template that is used when generating the article pages on your site. A default page template is created for your convinience when when you add a site, but we encourage you to customize it to match your own site's design." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiArticlePageConfig" NavigateUrl="../Link-Page-Setup/" Title="Article Page Configuration" Description="Set up how link pages will be used on your website." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiActivateLinking"
			        NavigateUrl="../Link-Page-Setup/Setup-Initial-Link-Page.aspx"
			        Title="Activate Linking"
			        Description="Create your initial link page and enable your account for linking." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" ID="cpiVerifyLinkPage"
			        NavigateUrl="../Link-Page-Setup/Verify-Link-Page.aspx"
			        Title="Verify Link Page"
			        Description="Instantly check if your link page has been set up correctly.  You may re-verify your link page at any time." />
			</li>
		</ul>
	</asp:Panel>
	
	<h2>
		<img src="Account.gif" alt="Account Logo" />
		Account Maintenance		
	</h2>
	<ul class="menuList">
		<li>
		    <Nle:ControlPanelItem runat="server" NavigateUrl="../Payment-Settings/" Title="Payment Settings" Description="Set up the type of payment used, and your payment plan.  You can also see when your account expires, and needs renewal." />
		</li>
		<li>
		    <Nle:ControlPanelItem runat="Server" NavigateUrl="../Manage-Sites/" Title="Site Management" Description="Set up your sites.  Use this to set the category that your site belongs to, and enter web addresses." />
		</li>
		<li>
		    <Nle:ControlPanelItem runat="server" NavigateUrl="../Manage-Security/" Title="Change Password" Description="This is where you can change your account password." />
		</li>
		<li>
		    <Nle:ControlPanelItem runat="server" NavigateUrl="../Affiliate-Program/" Title="Affiliate Program" Description="Sign up and receive a link that will allow you to refer users to our site and earn credit with our site." />
		</li>
	</ul>

	<h2>
		<img src="Reporting.gif" alt="Reporting Icon" />
		Reporting
	</h2>
	<ul class="menuList">
	    <li>
	        <Nle:ControlPanelItem runat="server" NavigateUrl="../Reporting/" Title="Reports" />
	    </li>
	</ul>

	<asp:panel Runat="server" ID="pnlReporting" Visible="True">
		<h2>
			<img src="Reporting.gif" alt="Reporting Icon" />
			Administrative Reporting
		</h2>
		<ul class="menuList">
		    <li>
		        <Nle:ControlPanelItem runat="server" NavigateUrl="../Reporting/" Title="Reports" />
		    </li>
			<li>
			    <Nle:ControlPanelItem runat="server" NavigateUrl="../Thumbnail-Rank-Graphing/" Title="Ranking Dashboard" />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" NavigateUrl="../Rank-Graphing/" Title="Rank Charting Application" />
			</li>
		</ul>
	</asp:panel>
					
	<asp:Panel Runat="server" ID="pnlAdministration" Visible="False">
		<h2>
			<img src="Administration.gif" alt="Administration Icon" />
			Administration
		</h2>
		<ul class="menuList">
			<li>
			    <Nle:ControlPanelItem runat="server" NavigateUrl="http://209.132.213.151/cp/awstats/awstats.pl?config=naturallinkexchange.com" Title="AWStats Statistics" />
			</li>
			<li>
				<asp:Button Runat="server" ID="cmdUpdateRss" Text="Run RSS Maintenance Cycle" />
			</li>
			<li>
				<asp:Button Runat="server" ID="cmdUpdateLinks" Text="Run Site Link Maintenance Cycle" />
			</li>
			<li>
				<asp:Button Runat="server" ID="cmdUpdateFtp" Text="Run all FTP Updates" />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" NavigateUrl="../Administration/Category-Administration/" Title="Category Administration" Description="Edit categories as configure the relationships between them." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" NavigateUrl="../Administration/Send-Email/" Title="Send Email" Description="Send emails to the customers via an online form." />
			</li>
			<li>
			    <Nle:ControlPanelItem runat="server" NavigateUrl="../Administration/Send-Email/GetAllUnsentEmail.aspx" Title="View unsent email queue" />
			</li>
			<li>
			    <asp:Button Runat="server" id="cmdSpiderLinkPage" Text="Spider Link Page (Experimental!)" />
			</li>
			<li>
			    <asp:Button Runat="server" id="cmdSpiderAllLinkPages" Text="Spider All Link Pages (Experimental!)" />
			</li>
		</ul>
	</asp:Panel>
</asp:Content>