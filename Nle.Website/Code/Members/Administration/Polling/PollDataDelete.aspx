<%@ Page language="c#" Codebehind="PollDataDelete.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataDelete" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
      <title>PollDataDelete</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
      <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
      <meta name="CODE_LANGUAGE" Content="C#">
      <meta name="vs_defaultClientScript" content="JavaScript">
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
      <LINK rel="stylesheet" href="PollAdminStyleSheet.css" type="text/css">
  </HEAD>
   <body>
      <form id="PollDataDelete" method="post" runat="server">
         <div class="PageTitle">
            <ASP:LABEL id="Label1" Runat="server" Text="Delete This Poll ({0})"></ASP:LABEL>
         </div>
         <br>
         Do you want to delete the poll?
         <br>
         <br>
         <asp:Button id="YesButton" runat="server" Text="Yes"></asp:Button>&nbsp;
         <asp:Button id="NoButton" runat="server" Text="No"></asp:Button><br><br>
         <em>NOTE: This action will also clear related data in the Usage Report.</em>         
      </form>
   </body>
</HTML>
