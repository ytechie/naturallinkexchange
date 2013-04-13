<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
	Inherits="Members_Recent_Polls_Default" Title="Recent Polls" %>

<%@ Register tagprefix="Poll" namespace="PeterBlum.PollControl" Assembly="PollControl" %>

<asp:Content ContentPlaceHolderID="mainContent" Runat="Server">
	<Poll:RecentPolls runat="server">
	</Poll:RecentPolls>
</asp:Content>

