<%@ Page language="c#" Inherits="Nle.Website.Members.Payment_Settings.Account_Upgrade"
    CodeFile="Account-Upgrade.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/payment-settings/paypal-return.aspx" %>
<%@ Reference Page="~/serverservices/paypalipn/default.aspx" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<%@ Register TagPrefix="Nle" TagName="LinkPackagesTable" Src="LinkPackagesTable.ascx" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
	<h1>Account Upgrade</h1>
	
	<asp:Panel runat="server" ID="pnlUpgradeFlag" Visible="false">
	    <p>
	        If you do not which to upgrade your site at this time, please
	        <asp:LinkButton runat="server" ID="lnkCancelUpgrade" Text="click here" />.  You can come
	        back to this page at any time.  Otherwise, follow
	        the instructions below to set up your payments through PayPal.
	    </p>
	</asp:Panel>
	
	<p>
		Upgrading your account is very simple, and will take effect almost immediately. Currently, we
		only support PayPal Subscriptions as a payment method. During the checkout process, you will
		be taken to the PayPal secure payment site to select your payment options.
	</p>
	<p>
		All pay plans must be prepaid, and cancellations will only affect future payments. Please choose
		your payment terms accordingly. See the table below for plan details and pricing.
	</p>
	<p>
		If you upgrade your plan while paying for an existing plan, the new plan will start when
		the next payment scheduled.  If you would like to upgrade between paying plans immediately, you need
		to cancel your account, and then upgrade.
	</p>
	<p>
		If you have any questions or concerns, please
		<a runat="server" href="~/Support/">contact support</a>.
	</p>
	
	<h3>Payment Method</h3>
	<asp:RadioButton Runat="server" ID="rdoPayPal" Text="PayPal" Enabled="False" Checked="True" />
	
	<h3>Available Link Plans</h3>
	<asp:RadioButton Runat="server" ID="rdoSilver" Text="Silver Package" GroupName="LinkPlans" Checked="True"/>
	<asp:RadioButton Runat="server" ID="rdoGold" Text="Gold Package" GroupName="LinkPlans" />
	
	<h3>Payment Interval</h3>
	<asp:RadioButton Runat="server" ID="rdoMonthly" Text="Pay Monthly" GroupName="PaymentIntervals" Checked="True" />
	<asp:RadioButton Runat="server" ID="rdoYearly" Text="Pay Yearly" GroupName="PaymentIntervals" /><br />
	
	<asp:Button Runat="server" Text="Pay Now!" ID="cmdPayNow" /><br />
	
	<h2>Cancel Account</h2>
	<p>
		If you would like to cancel your account, please make sure that you no longer want to
		participate in our network. We recommend at least participating in our FREE subscription level.
		To cancel your payments, log into your <a href="http://www.PayPal.com">PayPal</a> account, and
		cancel it from there.  That	will automatically cancel your subscription in our system.
	</p>
					
	<h2>Account Package Details</h2>
					
	<!-- The table that lists the available link packages -->
	<Nle:LinkPackagesTable runat="server" id="tblLinkPackages" />
</asp:Content>