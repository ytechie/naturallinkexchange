<%@ Register TagPrefix="uc1" TagName="PollSelectCategories" Src="PollSelectCategories.ascx" %>
<%@ Page language="c#" trace="false" Codebehind="PollUsageReport.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollUsageReport" %>
<%@ Register TagPrefix="date" Namespace="PeterBlum.DateTextBoxControls" Assembly="DateTextBoxControls" %>
<%@ Register TagPrefix="Poll" Namespace="PeterBlum.PollControl" Assembly="PollControl" %>
<%@ Register TagPrefix="CDDL" Namespace="PeterBlum.ColorWebControls" Assembly="ColorWebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Poll Usage Report</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema><LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
<body>
<form id=PollUsageReport method=post runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Runat="server" Text="Usage Report {0}"></ASP:LABEL></div><br>
<span class=StndCmds><asp:hyperlink id=PollList Runat="server" NavigateUrl="PollDataList.aspx">Poll List</asp:hyperlink>
<asp:literal id=BackToResultsSep Runat="server" Text="&nbsp;|&nbsp;"></asp:literal>
<asp:hyperlink id=BackToResults Runat="server" NavigateUrl="PollDataResults.aspx?PollID={0}">Back To Poll Results</asp:hyperlink>
&nbsp;|&nbsp;
<asp:HyperLink ID="HelpLink" Runat=server NavigateUrl="HelpPollUsageReport.aspx" Target="Help">Help</asp:HyperLink>
</span><br><br>
<asp:panel id=SettingsPanel Runat="server" CssClass=SelectionCriteriaPanel Width="100%">
<asp:Label id=SettingsHeader Runat="server" CssClass=SelectionCriteriaTitle >Data Settings</asp:Label><br>
<uc1:PollSelectCategories id=PollSelectCategories1 runat="server"></uc1:PollSelectCategories>
<asp:Table id=RemainingControls Runat="server" Width="100%">
<asp:TableRow>
<asp:TableCell Width=20%>
Date range
</asp:TableCell>
<asp:TableCell>
<Date:DateTextBox id=DateTextBox1 runat="server" Columns="10" xImageURL="../DateTextBox/calendar.jpg" xPopupURL="../DateTextBox/popupcalendar.aspx" xEndRangeControlID="DateTextBox2" xInvalidDateMsg="The start date is invalid." EnableViewState=false ></Date:DateTextBox>&nbsp;thru 
<date:DateTextBox id=DateTextBox2 runat="server" Columns="10" xImageURL="../DateTextBox/calendar.jpg" xPopupURL="../DateTextBox/popupcalendar.aspx" xStartRangeControlID="DateTextBox1" xInvalidDateMsg="The end date is invalid." EnableViewState=false ></date:DateTextBox>
&nbsp;
<Date:DateTextBoxValidator id=DTBV1 runat="server" Display=Dynamic ControlToValidate=DateTextBox1 EnableViewState=false />
<Date:DateTextBoxValidator id="DTBV2" runat="server" Display=Dynamic ControlToValidate=DateTextBox2 EnableViewState=false />
</asp:TableCell> 
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
Period size
</asp:TableCell>
<asp:TableCell>
<asp:DropDownList ID=PeriodSizeDDL Runat=server EnableViewState=false >
<asp:ListItem Value="1" >Half hour</asp:ListItem>
<asp:ListItem Value="2" >1 Hour</asp:ListItem>
<asp:ListItem Value="8" >4 hours</asp:ListItem>
<asp:ListItem Value="16" >8 hours</asp:ListItem>
<asp:ListItem Value="24" >Half day</asp:ListItem>
<asp:ListItem Value="48" >1 day</asp:ListItem>
<asp:ListItem Value="336" >1 week</asp:ListItem>
</asp:DropDownList>
</asp:TableCell> 
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
<asp:Label id="Label1" Runat="server" Font-Size="Smaller" Font-Bold="True">Graph formatting</asp:Label>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
Odd bar color
</asp:TableCell>
<asp:TableCell>
   <asp:Table runat=server CellPadding=0 CellSpacing=0 >
   <asp:TableRow>
   <asp:TableCell>
   <CDDL:ExtColorDropDownList ID=OddBarColorDDL Runat=server />
   </asp:TableCell>
   <asp:TableCell>
   &nbsp;&nbsp;Even bar color&nbsp;
   </asp:TableCell>
   <asp:TableCell>
   <CDDL:ExtColorDropDownList ID="EvenBarColorDDL" Runat=server />
   </asp:TableCell>
   </asp:TableRow>
   </asp:Table>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
Bar height
</asp:TableCell>
<asp:TableCell>
   <asp:Table runat=server CellPadding=0 CellSpacing=0 ID="Table1">
   <asp:TableRow>
   <asp:TableCell>
   <asp:DropDownList ID=BarHeightDDL Runat=server EnableViewState=false >
      <asp:ListItem Value="8">Small</asp:ListItem>
      <asp:ListItem Value="10">Medium</asp:ListItem>
      <asp:ListItem Value="12">Large</asp:ListItem>
   </asp:DropDownList>
   </asp:TableCell>
   <asp:TableCell>
   &nbsp;&nbsp;Color following a gap in the data&nbsp;
   </asp:TableCell>
   <asp:TableCell>
   <CDDL:ExtColorDropDownList ID="GapForeColorDDL" Runat=server />
   </asp:TableCell>
   </asp:TableRow>
   </asp:Table>
</asp:TableCell> 
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
&nbsp;
</asp:TableCell>
<asp:TableCell HorizontalAlign=Right>
<asp:Button id=SubmitButton Runat="server" Text="Draw The Graph"></asp:Button>
</asp:TableCell>
</asp:TableRow>
</asp:Table></asp:panel><br><POLL:HTMLBARGRAPH id=HBG runat="server"></POLL:HTMLBARGRAPH>
<br>
<asp:Label ID=TotalVotes Runat=server  EnableViewState="False">Total votes: {0}</asp:Label><br>
<asp:Label ID="MostVotes" Runat=server  EnableViewState="False">Most votes in a period: {0}</asp:Label><br>
</form>
	
  </body>
</HTML>
