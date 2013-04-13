<%@ Page language="c#" Inherits="Nle.Website.Members.Payment_Settings.PayPal_Return"
    CodeFile="PayPal-Return.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" TagName="LinkPackagesTable" Src="LinkPackagesTable.ascx" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
  
<asp:Content runat="server" ContentPlaceHolderID="mainContent">
<!--
Note: This page needs to follow certain PayPal guidelines:
- Per the user agreement, you must provide verbiage on the page displayed by 
the Return URL that will help the buyer understand that the payment has been 
made and that the transaction has been completed. 
- You must provide verbiage on the page displayed by the Return URL that 
explains that payment transaction details will be emailed to the buyer. 
- Example: Thank you for your payment. Your transaction has been completed, 
and a receipt for your purchase has been emailed to you. You may log into your 
account at PayPal to view details of this transaction. 
-->

	<h3>Thank you for your payment.</h3>
	<p>
		Your transaction has been completed. It may take some time for your payment to take effect. If
		your account has not been upgraded within the next hour, please
		<a href="<%= Nle.Website.Global.VirtualDirectory %>Support/">contact support.</a>
	</p>
	<p>
		You will also receive an email confirming this transaction.
	</p>
	<p>
		You can log in to your <a href="http://www.PayPal.com">PayPal</a> account for more information about
		your subscription.
	</p>
	<p>
		<a href="./">Return to payment settings</a><br />
		<a href="../Control-Panel/">Return to control panel</a>
	</p>
</asp:Content>