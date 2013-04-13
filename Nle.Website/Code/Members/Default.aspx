<%@ Page language="c#" Inherits="Nle.Website.Members.MemberDefault" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>
    
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
    
<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <asp:Panel runat="server" DefaultButton="cmdLogin">
	    <h1>Members Area</h1>
	    <p>
		    Welcome to the members area.
	    </p>
	    <div class="standardFieldLabel">Email Address: </div><asp:TextBox Runat="server" ID="txtEmailAddress" /><br />
	    <div class="standardFieldLabel">Password: </div><asp:TextBox Runat="server" ID="txtPassword" TextMode="Password" /><br />
	    <div class="standardFieldLabel">&nbsp;</div><asp:Button Runat="server" ID="cmdLogin" Text="Log in" /><br />
	    <asp:Literal Runat="server" ID="lblLoginMessage" />
	    <br />
	    <a href="<%= Nle.Website.Global.VirtualDirectory %>Sign-Up/">Not a member yet? Sign up here.</a><br />
	    <a runat="server" href="~/Members/Forgot-Password/">Forgot your password?</a>
	</asp:Panel>
</asp:Content>