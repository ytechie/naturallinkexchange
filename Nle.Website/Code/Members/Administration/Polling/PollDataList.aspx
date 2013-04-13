<%@ Page language="c#" Codebehind="PollDataList.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataList" clientTarget="Uplevel" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
      <title>Poll Administration</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<META http-equiv=Content-Type content="text/html; charset=windows-1252">
      <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
      <meta name="CODE_LANGUAGE" Content="C#">
      <meta name="vs_defaultClientScript" content="JavaScript">
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
      <LINK rel="stylesheet" href="PollAdminStyleSheet.css" type=text/css>
  </HEAD>
   <body>
      <form id="PollDataList" method="post" runat="server">
         <div class="PageTitle">
            <ASP:LABEL id="Label1" Text="Poll Administration" Runat="server" ></ASP:LABEL>
         </div>
         <br>
         <span class="StndCmds">
         <asp:HyperLink ID="MainPage" Runat=server Visible=False NavigateUrl="Home.aspx">Home Page</asp:HyperLink>
         <asp:Literal ID="CmdSeparator1" Runat="server" Visible=False >&nbsp;|&nbsp;</asp:Literal>
         <asp:HyperLink ID="AddCmd" Runat=server NavigateUrl="PollDataEdit.aspx?PollID=0">Add a Poll</asp:HyperLink>
         &nbsp;|&nbsp;
         <asp:HyperLink ID="DailyPlanner" Runat=server NavigateUrl="PollDataPlanner.aspx">Poll Daily Planner</asp:HyperLink>
         &nbsp;|&nbsp;
         <asp:HyperLink ID="UsageReport" Runat=server NavigateUrl="PollUsageReport.aspx">Usage Report</asp:HyperLink>
         &nbsp;|&nbsp;
         <asp:HyperLink ID="HelpLink" Runat=server NavigateUrl="HelpDataList.aspx" Target="Help">Help</asp:HyperLink>
         </span>
         <br>
<p style="MARGIN-BOTTOM: 6px">
Poll list contains &nbsp;
<asp:DropDownList id="ContainsDropList" runat="server">
   <asp:ListItem Value="1" Selected="True">All</asp:ListItem>
   <asp:ListItem Value="2">Completed</asp:ListItem>
   <asp:ListItem Value="3">Now Active</asp:ListItem>
   <asp:ListItem Value="4">Has not started</asp:ListItem>
   <asp:ListItem Value="5">Started in the last 7 days</asp:ListItem>
   <asp:ListItem Value="6">Started in last 30 days</asp:ListItem>
   <asp:ListItem Value="7">Will start in the next 7 days</asp:ListItem>
</asp:DropDownList>&nbsp;
<asp:Button id="SubmitButton" runat="server" Text="Update List"></asp:Button>&nbsp;&nbsp; 
(Today's date:
<asp:Label ID="TodayLabel" Runat=server />
)
</p>
<asp:Repeater id="PollRepeater" runat="server">
   <HeaderTemplate>
      <table class=PollListTable >
         <tr class=PollListHeader > <!-- header-->
            <td>&nbsp;</td> <!-- commands column-->
            <td>PollID</td>
            <td>Question</td>
            <td>Priority</td>
            <td nowrap>Start Date</td>
            <td nowrap>End Date</td>
            <td>Votes</td>
         </tr>
      <!-- remaining table elements are in other templates -->
   </HeaderTemplate>
   <ItemTemplate>
      <tr class=PollListRow >
         <td class="StndCmds" nowrap><!-- commands: Edit, View, Delete -->
            <a href='PollDataTest.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Test</a>
            &nbsp;
            <a href='PollDataResults.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Results</a>
            &nbsp;
            <a href='PollDataEdit.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Edit</a>
            &nbsp;
            <a href='PollDataDelete.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Delete</a>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "PollID") %>
         </td>
         <td>
            <%# DataBinder.Eval(Container.DataItem, "Question") %>
         </td>
         <td align=right>
            <%# DataBinder.Eval(Container.DataItem, "Priority") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "StartDate", "{0:d}") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:d}") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "TotalVotes") %>
         </td>
      </tr>
   </ItemTemplate>
   <AlternatingItemTemplate>
      <tr class=PollListRowAlt >
         <td class="StndCmds" nowrap><!-- commands: Edit, View, Delete -->
            <a href='PollDataTest.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Test</a>
            &nbsp;
            <a href='PollDataResults.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Results</a>
            &nbsp;
            <a href='PollDataEdit.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Edit</a>
            &nbsp;
            <a href='PollDataDelete.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>'>Delete</a>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "PollID") %>
         </td>
         <td>
            <%# DataBinder.Eval(Container.DataItem, "Question") %>
         </td>
         <td align=right>
            <%# DataBinder.Eval(Container.DataItem, "Priority") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "StartDate", "{0:d}") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:d}") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "TotalVotes") %>
         </td>
      </tr>
   </AlternatingItemTemplate>
   <FooterTemplate>
      </table>
   </FooterTemplate>
</asp:Repeater>
      </form>
   </body>
</HTML>