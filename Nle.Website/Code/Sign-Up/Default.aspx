<%@ Page language="c#" Inherits="Nle.Website.Sign_Up.SignUpDefault" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Members/Payment-Settings/LinkPackagesTable.ascx" TagPrefix="LP" TagName="LinkPackagesTable" %>

<%@ Reference Page="~/members/forgot-password/default.aspx" %>
 
 <asp:Content runat="Server" ContentPlaceHolderID="mainContent">
    <asp:PlaceHolder runat="server" ID="contentPlaceholder" />
 
	<h1>Sign Up</h1>
	<p>
		Use this form to create an account.  Please <b>make sure that
		your email address is correct</b> because we will send your password to that address.
		If you do not receive the welcome email, please
		<a runat="server" href="~/Support/">contact support</a>.
	</p>
					
	<div class="standardFieldLabel">Name:</div>
	<asp:TextBox Runat="server" ID="txtName" />
	<asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" Display="Dynamic"
		ErrorMessage="You Must Enter Your Name" />
	<br />
	
	<div class="standardFieldLabel">Email Address:</div>
	<asp:TextBox Runat="server" ID="txtEmail" Columns="50" />
	<asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic"
		ErrorMessage="You Must Enter Your Email Address" />
	<asp:RegularExpressionValidator Runat="server" ID="revEmail"
		ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Invalid Email Address" />
	<br />
	
	<asp:Panel ID="pnlUserExists" Runat="server" Visible="False">
		<div class="UserExists">
			The email address specified already exists.  If you forgot your password, please visit
			the <asp:HyperLink id="hypForgotPassword" Runat="server">forgot password page</asp:HyperLink>.
		</div>
	</asp:Panel>
		
	<p>
		After you sign up, a temporary password will be sent to the email address
		you provided.  You can use that temporary password, or you can choose
		a new password by using the control panel.  You account will be available instantly.
	</p>
	
	<p>
		Accounts will be available instantly but links will not be published until your site 
		is approved by our editors. This process will take less than 24 hours and if during 
		business hours may happen much sooner. Unless you recieve an email or call from us, consider your links approved. 
	</p>
	
	<!-- The table that lists the available link packages -->
	<LP:LinkPackagesTable runat="server" id="tblLinkPackages" />
	
	<p>
	    Please select which plan you would like to sign up for.  If you sign up for a silver or gold
	    account, you will be asked to set up your payments through PayPal when you first log in.  There is
	    no obligation if you choose silver or gold.
	</p>
	
	<asp:RadioButtonList runat="server" ID="rdoAccountLevel">
	    <asp:ListItem Text="Free Account" Value="1" Selected="true" />
	    <asp:ListItem Text="Silver Account" Value="1" />
	    <asp:ListItem Text="Gold Account" Value="1" />
	</asp:RadioButtonList>
	<br />
	    	
	<asp:CheckBox Runat="server" ID="chkAgreeToTerms" />
	I agree to the <a href="../Terms-of-Service/">terms of service</a>
    <br />
	
	<asp:Button Runat="server" ID="cmdCreateUser" Text="Sign Up!" /><br />
	
</asp:Content>