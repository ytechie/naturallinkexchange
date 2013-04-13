<%@ Page language="c#" Inherits="Nle.Website.Members.Forgot_Password.ForgotPasswordDefault"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
	<h1>Password Retrieval</h1>
	<p>
		If you have forgotten your password, you will need to enter the email address that
		you used to create your account.  We will then email you a new temporary password
		that you can use to login.  After you successfully log in, you can use the control
		panel to change your temporary password back to your normal password.
	</p>
	
	Email Address: <asp:TextBox runat="server" ID="txtEmail" />
	<asp:Button runat="server" ID="cmdSend" text="Send" /><br />
	<asp:Literal runat="server" ID="litMessage" />
</asp:Content>