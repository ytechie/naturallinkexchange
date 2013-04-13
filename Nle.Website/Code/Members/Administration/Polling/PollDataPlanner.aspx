<%@ Register TagPrefix="Poll" Namespace="PeterBlum.PollControl" Assembly="PollControl" %>
<%@ Register TagPrefix="date" Namespace="PeterBlum.DateTextBoxControls" Assembly="DateTextBoxControls" %>
<%@ Page language="c#" Codebehind="PollDataPlanner.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataPlanner" %>
<%@ Register TagPrefix="uc1" TagName="PollSelectCategories" Src="PollSelectCategories.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Poll Daily Planner</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
  <body MS_POSITIONING="FlowLayout">
	
    <form id="PollDataPlanner" method="post" runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Runat="server" Text="Poll Daily Planner"></ASP:LABEL></div>
<P><br>
<span class=StndCmds><asp:hyperlink id=PollList Runat="server" NavigateUrl="PollDataList.aspx">Poll Administration</asp:hyperlink>
&nbsp;|&nbsp;
<asp:HyperLink ID="AddCmd" Runat=server NavigateUrl="PollDataEdit.aspx?PollID=0&amp;ReturnUrl={0}">Add a Poll</asp:HyperLink>
&nbsp;|&nbsp;
<asp:HyperLink ID="HelpLink" Runat=server NavigateUrl="HelpDataPlanner.aspx" Target="Help">Help</asp:HyperLink>
</span><br><br>
<asp:panel id=SettingsPanel Runat="server" CssClass=SelectionCriteriaPanel >
<asp:Table id=SettingsTable Runat="server" Width="100%">
<asp:TableRow>
<asp:TableCell>
<asp:Label id=SettingsHeader Runat="server" CssClass=SelectionCriteriaTitle >Data Settings</asp:Label>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
<asp:TableCell Width=20%>
Date to view
</asp:TableCell>
<asp:TableCell>
<Date:DateTextBox id=DateTextBox1 runat="server" Columns="10" xImageURL="../DateTextBox/calendar.jpg" xPopupURL="../DateTextBox/popupcalendar.aspx" xEndRangeControlID="DateTextBox2" xInvalidDateMsg="The date is invalid." EnableViewState=false ></Date:DateTextBox>
&nbsp;
<Date:DateTextBoxValidator id=DTBV1 runat="server" Display=Dynamic ControlToValidate=DateTextBox1 EnableViewState=false />
<asp:RequiredFieldValidator ID=DTBRV1 Runat=server Display=Dynamic ControlToValidate=DateTextBox1 EnableViewState=False ErrorMessage="You must assign a date." />
</asp:TableCell> 
</asp:TableRow>
</asp:Table>
<uc1:PollSelectCategories id=PollSelectCategories1 runat="server"></uc1:PollSelectCategories>
<DIV align=right>
<asp:Button id=SubmitButton Text="Show The Polls" Runat="server"></asp:Button>&nbsp;&nbsp;</DIV>
</asp:panel>
<br>
<p style="MARGIN-BOTTOM: 6px">
These polls match the above criteria. They appear in the order the Poll Control will retrieve them.
</p>
<asp:Repeater id="PollRepeater" runat="server">
   <HeaderTemplate>
      <table class=PollListTable >
         <tr class=PollListHeader> <!-- header-->
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
            <a href='PollDataTest.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Test</a>
            &nbsp;
            <a href='PollDataResults.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Results</a>
            &nbsp;
            <a href='PollDataEdit.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Edit</a>
            &nbsp;
            <a href='PollDataDelete.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Delete</a>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "PollID") %>
         </td>
         <td>
            <%# DataBinder.Eval(Container.DataItem, "Question") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "Priority") %>
         </td>
         <td align=right>
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
            <a href='PollDataTest.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Test</a>
            &nbsp;
            <a href='PollDataResults.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Results</a>
            &nbsp;
            <a href='PollDataEdit.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Edit</a>
            &nbsp;
            <a href='PollDataDelete.aspx?pollid=<%#DataBinder.Eval(Container.DataItem, "PollID") %>&ReturnUrl=<%# ReturnUrl %>'>Delete</a>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "PollID") %>
         </td>
         <td>
            <%# DataBinder.Eval(Container.DataItem, "Question") %>
         </td>
         <td align=right nowrap>
            <%# DataBinder.Eval(Container.DataItem, "Priority") %>
         </td>
         <td align=right>
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
