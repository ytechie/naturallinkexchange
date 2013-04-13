<%@ Page language="c#" Inherits="Nle.Website.Members.Link_Page_Setup.LinkPageSetupDefault"
		CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/link-page-setup/download-link-page-script.aspx" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<asp:Content runat="Server" ContentPlaceHolderID="mainContent">
	<nle:DynamicHelp ID="dynamicHelp" runat="Server">
		<nle:DynamicHelpText runat="server" HelpId="1" Title="FTP Path"
				Text="<p>This field is the full FTP path where the files should be uploaded on the server you specified.</p><p>To determine the base path of your website, use an FTP client and log in using the username and password you entered here.</p><p>The folder that you start in should be your default path.  It might look something like '/UserFiles/Site1/'.  If you want the link pages in a subfolder under that, you would enter a path of '/UserFiles/Site1/SubfolderName/'.</p>" />
		<nle:DynamicHelpText runat="server" HelpId="2" Title="FTP Server Name"
				Text="<p>In this field you should enter the name of the FTP server you use to upload your web files. This usually looks like <i>ftp.YourSite.com</i></p>" />
		<nle:DynamicHelpText runat="server" HelpId="3" Title="FTP User Name"
				Text="<p>In this field you should enter the FTP user account that has permission to upload files to your website. Contact your hosting provider if you need this set up, or if you don't know what it is.</p>" />
		<nle:DynamicHelpText runat="server" HelpId="4" Title="FTP Password"
				Text="<p>In this field you should enter the password for the user name that you entered above.</p>" />  	  
	</nle:DynamicHelp>

	<h3>Overview</h3>
	<p class="rightPaddedText">
		Use this page to configure how your article pages will be updated.
	</p>
	<p class="rightPaddedText">
		There are two primary methods of getting the article pages on your site.  The simplest way is
		to use a script that you place on your webserver that handles all of the article page requests.
		Every time someone visits one of your article pages, it requests the most current data
		from our server.
	</p>
	<p class="rightPaddedText">
		The other method is to have us FTP the files into your account, so that all of the link pages
		are static, and do not rely on server side scripting abilities.
		This has the advange of lowering the load on our server as well as yours because the pages
		are plain HTML, and do not have to be generated on the fly.
	</p>

	<ajax:AjaxPanel runat="server">
		<asp:CheckBox runat="server" ID="chkHideSetupMessage" AutoPostBack="true"
			Text="Hide initial setup message" />
			- Check this after your link page has been set up and
			you have confirmed that it's working properly.  When this option is unchecked, you
			will get a message on your link page that tells you that your link page is
			correctly configured.
		<b>
			<asp:Literal runat="server" ID="lblHideSetupMsg" Text="<br />Your setting has been saved" Visible="false" />
		</b>
	</ajax:AjaxPanel>

	<h3>Update Method</h3>
	<ajax:AjaxPanel runat="server">
		<asp:RadioButton Runat="server" ID="rdoFtpUpload" GroupName="UpdateMethod" Text="FTP Upload" Checked="False" AutoPostBack="True" /><br />
		<asp:RadioButton Runat="server" ID="rdoPhpScript" GroupName="UpdateMethod" Text="PHP Script" Checked="False" AutoPostBack="True" /><br />
		<asp:RadioButton Runat="server" ID="rdoNetScript" GroupName="UpdateMethod" Text="ASP.NET Script" Checked="False" AutoPostBack="True" /><br />
		<asp:RadioButton Runat="server" ID="rdoNetControlScript" GroupName="UpdateMethod" Text=".NET Control" Checked="False" Enabled="False" AutoPostBack="True" Visible="False" />
		
		<asp:Panel Runat="server" ID="pnlFtpSettings" Visible="False">
			<h3>FTP Settings</h3>
			<p>
				Your account is currently configured to use the FTP upload method. To use this method,
				you'll need to have an FTP account set up so that we can upload your article pages.  We
				need to know the username and password for the FTP account.  If you require assistance,
				please do not hesitate to contact <a href="../../Support/">support</a>.
			</p>
			<p>
				After saving your settings, upload your pages using the button below.  You will then need to link
				to the inital link page.  Listed below is the file name of the file you will link to.
			</p>
			<p>
				Initial Link Page Name:
				<b><asp:Literal Runat="server" ID="lblFtpInitialName" /></b>
			</p>
				
			<div class="standardFieldLabel">Last upload:</div> <asp:Label Runat="server" ID="lblLastUpload" /><br />
			<div class="standardFieldLabel">Server Name:</div> <asp:TextBox Runat="server" ID="txtFtpServer" onmouseout="return HideDynamicHelp('dynamicHelp');" onmouseover="return ShowDynamicHelp('dynamicHelp', 2)" /><br />
			<div class="standardFieldLabel">Path (if not default):</div> <asp:TextBox Runat="server" ID="txtFtpPath" onmouseout="return HideDynamicHelp('dynamicHelp');" onmouseover="return ShowDynamicHelp('dynamicHelp', 1)" /><br />
			<div class="standardFieldLabel">Username:</div> <asp:TextBox Runat="server" ID="txtFtpUserName" onmouseout="return HideDynamicHelp('dynamicHelp');" onmouseover="return ShowDynamicHelp('dynamicHelp', 3)" /><br />
			<div class="standardFieldLabel">Password:</div> <asp:TextBox Runat="server" ID="txtFtpPassword" TextMode="Password" onmouseout="return HideDynamicHelp('dynamicHelp');" onmouseover="return ShowDynamicHelp('dynamicHelp', 4)" /><br />
			<asp:CheckBox runat="server" ID="chkActiveMode" Checked="false" Text="Active Mode" /> (most servers require this to be unchecked, try if you have problems uploading)<br />
			<asp:Button Runat="server" ID="cmdSaveFtpInfo" Text="Test &amp; Save Settings" /><br />
			<asp:Button Runat="server" ID="cmdGenerateAndFtp" Text="Upload Pages Now"
				ToolTip="Click to force an update of your link pages" />
			
		</asp:Panel>
		
		<asp:Panel Runat="server" ID="pnlPhpSettings" Visible="False">
			<h3>PHP Script Settings</h3>

			<h4>Step by step instructions</h4>
			<ol>
				<li>
					Download the script below (right click and choose &quot;Save Target As&quot;)
				</li>
				<li>
					(Optional) Rename the script to whatever name you like, just be sure to keep
					the extension &quot;php&quot;.
				</li>
				<li>
					Upload the file to your webserver in the location of your choosing.
				</li>
				<li>
					Go to the URL that corresponds to the location that you placed your file.  Verify
					that the link page is working.  If you require assistance, please contact
					<a href="../../Support/">our support team</a>.
				</li>
			</ol>

			<asp:HyperLink Runat="server" ID="lnkPhpScript"
				text="Download the PHP Script (Right-click and choose 'Save Target As')" />
		</asp:Panel>
		
		<asp:Panel Runat="server" ID="pnlNetSettings" Visible="False">
			<h3>ASP.NET Script Settings</h3>
			
			<h4>Step by step instructions</h4>
			<ol>
				<li>
					Download the script below (right click and choose &quot;Save Target As&quot;)
				</li>
				<li>
					(Optional) Rename the script to whatever name you like, just be sure to keep
					the extension &quot;aspx&quot;.
				</li>
				<li>
					Upload the file to your webserver in the location of your choosing.
				</li>
				<li>
					Go to the URL that corresponds to the location that you placed your file.  Verify
					that the link page is working.  If you require assistance, please contact
					<a href="../../Support/">our support team</a>.
				</li>
			</ol>

			<asp:HyperLink Runat="server" ID="lnkNetScript"
				text="Download the ASP.NET Script (Right-click and choose 'Save Target As')" />
		</asp:Panel>
	</ajax:AjaxPanel>

	<asp:PlaceHolder Runat="server" ID="javaScriptPlaceholder" />
</asp:Content>