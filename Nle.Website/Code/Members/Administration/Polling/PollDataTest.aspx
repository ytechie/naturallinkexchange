<%@ Page language="c#" Codebehind="PollDataTest.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataTest" %>
<%@ Register tagprefix="Poll" namespace="PeterBlum.PollControl" Assembly="PollControl" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Poll Test Data</title>
 <!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    
    <LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
  <body >
	
    <form id="PollDataTest" method="post" runat="server">
<div class=PageTitle>
<ASP:LABEL id=Label1 Runat="server">Poll Test Data (PollID: {0})</ASP:LABEL></div><br>
<span class=StndCmds><asp:hyperlink id=BackLink Runat="server" NavigateUrl="PollDataList.aspx">Back to List</asp:hyperlink>&nbsp;|&nbsp; 
<asp:hyperlink id=ShowResults Runat="server" NavigateUrl="PollDataResults.aspx?PollID={0}">Show Results</asp:hyperlink>
</span>
<br><br>
Use this page to evaluate the poll's data. It is shown without any of the elegant formatting that you 
may be using when displaying it to the user.<br>
<hr color=blue size=2 >
<POLL:POLL id=Poll1 runat="server" xResultsButton=" " xPollUsedMode="eNone" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" BackColor="AntiqueWhite"></POLL:POLL>
<hr color=blue size=2 >
<em> Note:This page will not switch to the Results view when
you click Vote. Click Show Results to see the results.</em>
     </form>
	
  </body>
</HTML>
