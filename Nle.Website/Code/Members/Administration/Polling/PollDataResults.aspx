<%@ Register tagprefix="Poll" namespace="PeterBlum.PollControl" Assembly="PollControl" %>
<%@ Page language="c#" Codebehind="PollDataResults.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataResults" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
      <title>Poll Results</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema><LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
<body>
<form id=PollDataResults method=post runat="server">
<div class=PageTitle><ASP:LABEL id=Label1 Runat="server">Poll Results (PollID: {0})</ASP:LABEL></div><br>
<span class=StndCmds><asp:hyperlink id=BackLink Runat="server" NavigateUrl="PollDataList.aspx">Back to List</asp:hyperlink>&nbsp;|&nbsp; 
<asp:hyperlink id=UsageReport Runat="server" NavigateUrl="PollUsageReport.aspx?PollID={0}">Usage Report</asp:hyperlink>&nbsp;|&nbsp; 
<asp:hyperlink id="ClearResults" Runat="server" NavigateUrl="PollDataClearVotes.aspx?PollID={0}">Clear Votes</asp:hyperlink> 
&nbsp;|&nbsp;
<asp:HyperLink ID="HelpLink" Runat=server NavigateUrl="HelpPollResults.aspx" Target="Help">Help</asp:HyperLink>
</span>
<br><br>
<POLL:POLL id=Poll1 runat="server" xBarRightColor="OldLace" xBarBorderColor="DarkKhaki" xBarLeftColor="Green" xResultElementR1C3="Votes" xResultElementR1C2="Percentage" xTotalVotesFormatting="Total votes: {0:G}" xQuestionStyle-Font-Italic="true" xViewMode="Results" Width="500px" xTotalVotesOnResultsHAlign="Right" BorderColor="Black">
<xTotalVotesOnResults>
Total Votes: <%# Container.xTotalVotes %>
</xTotalVotesOnResults>

<xResultAnswerElements>
<poll:ResultElement xCellWidth="100%"></poll:ResultElement>
<poll:ResultElement xResultElementType="NewRow"></poll:ResultElement>
<poll:ResultElement xResultElementType="Bar" xCellWidth="350px"></poll:ResultElement>
<poll:ResultElement xTextFormat="{0:P}" xHorizontalAlign="Right" xResultElementType="Percentage" xCellWidth="80px"></poll:ResultElement>
<poll:ResultElement xTextFormat="{0:G}" xHorizontalAlign="Right" xResultElementType="Votes" xCellWidth="70px"></poll:ResultElement>
</xResultAnswerElements>
</POLL:POLL>
</form></SPAN>
   </body>
</HTML>
