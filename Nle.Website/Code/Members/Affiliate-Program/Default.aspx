<%@ Page language="c#" Inherits="Nle.Website.Members.Affiliate_Agreement.AffiliateAgreementDefault"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/Default.aspx" %>
  
<asp:Content runat="Server" ContentPlaceHolderID="mainContent">
	<asp:Panel Runat="server" ID="pnlAffiliate">
		<p>To participate in our Affiliate Program, simply place the link below on your website.  When a user follows that link,
		if you were the first person to refer the user to our site and that user then signs up within 30 days of following your
		link, you will be credited for a referral.</p>
		<br />
		<asp:HyperLink Runat="server" ID="hypAffiliateLink" />
	</asp:Panel>
	<asp:Panel Runat="server" ID="pnlNonAffiliate">
		<asp:Literal Runat="server" ID="litTerms" />
		<br />
		<p align="center">
			<asp:Button Runat="server" ID="cmdAgree" Text="Accept" />
			<asp:Button Runat="server" ID="cmdDisagree" Text="Decline" />
		</p>
	</asp:Panel>
</asp:Content>