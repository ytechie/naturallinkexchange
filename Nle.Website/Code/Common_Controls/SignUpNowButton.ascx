<%@ Control Language="c#" Inherits="Nle.Website.Common_Controls.SignUpNowButton" CodeFile="SignUpNowButton.ascx.cs" %>
<table class="signUpCircleBox">
	<tr>
		<td>
			<a href="<%= Nle.Website.Global.VirtualDirectory %>Sign-Up/" title="Sign Up Now!">
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/Join-Now-Circle.gif" />
			</a>
		</td>
		<td>
			<a href="<%= Nle.Website.Global.VirtualDirectory %>Sign-Up/" class="message">Sign Up for our Free Membership</a>
		</td>
	</tr>
</table>