<%@ Page language="c#" Codebehind="PollDataClearVotes.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataClearVotes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
      <title>Clear Votes</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
      <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
      <meta name="CODE_LANGUAGE" Content="C#">
      <meta name="vs_defaultClientScript" content="JavaScript">
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
      <LINK rel="stylesheet" href="PollAdminStyleSheet.css" type="text/css">
  </HEAD>
   <body>
      <form id="PollDataClearVotes" method="post" runat="server">
         <div class="PageTitle">
            <ASP:LABEL id="Label1" Runat="server" Text="Clear Votes On This Poll ({0})"></ASP:LABEL>
         </div>
         <br>
         This action will remove the vote counts on each answer while preserving the poll definition. It is intended to assist in trial runs of your poll whose votes you don't want when the poll is live.<br><br>
         Do you want to clear the votes on the poll?
         <br>
         <br>
         <asp:Button id="YesButton" runat="server" Text="Yes"></asp:Button>&nbsp;
         <asp:Button id="NoButton" runat="server" Text="No"></asp:Button><br><br>
         <em>NOTE: This action will also clear related data in the Usage Report.</em>
      </form>
   </body>
</HTML>
