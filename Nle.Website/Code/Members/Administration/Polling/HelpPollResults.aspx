<%@ Page language="c#" Codebehind="HelpPollResults.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.HelpPollResults" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Help: Poll Results</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
  <body >
	
    <form id="HelpPollResults" method="post" runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Runat="server" Text="Help: Poll Results"></ASP:LABEL></div><br>
The Poll Results page shows how users have been using a specific poll.<br><br>
To open the Poll Results page, start on the Poll Administration page and 
locate the desired poll. Then click <FONT color=blue><STRONG>Results</STRONG></FONT>.<br>
<br>
There are three elements on this page.
<ol type=disc>
<li>The central element is a Poll control showing the Results graph
with the total votes on each answer. This information may be useful to your marketing efforts.
<li>Use the <FONT color=blue><STRONG>Usage Report</STRONG></FONT> command to see a
Usage Report on this one poll. The Usage Report will help you evaluate what dates and times this poll
attracted the most and least traffic.
<li>If you want to reset the vote counts to zero, use the <FONT color=blue><STRONG>Clear Votes</STRONG></FONT>
command at the top. This command will also clear Usage Report data related to this poll.
</li>
</ol>
<br>

     </form>
	
  </body>
</HTML>
