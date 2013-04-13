<%@ Register TagPrefix="LVal" Namespace="PeterBlum.LengthValidators" Assembly="LengthValidators" %>
<%@ Page language="c#" Codebehind="PollDataEdit.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.PollDataEdit" clientTarget="Uplevel" %>
<%@ Register TagPrefix="Date" Assembly="DateTextBoxControls" Namespace="PeterBlum.DateTextBoxControls" %>
<%@ Register TagPrefix="val" Assembly="GroupValidator" Namespace="PeterBlum.GroupValidator" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
      <title>Poll Editor</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
<LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
<script language=javascript>
  function SetInitialFocus()
  {
   document.getElementById('QuestionText').focus();
  }
  window.onload = SetInitialFocus;  // called on startup
  </script>
</HEAD>
<body>
<form id=PollDataEdit method=post runat="server">
<div class=PageTitle><ASP:LABEL id=Label1 Runat="server">Poll Editor</ASP:LABEL></div><br>
<span class=StndCmds>
<asp:hyperlink id=BackLink Runat="server" NavigateUrl="PollDataList.aspx">Back to List</asp:hyperlink>&nbsp;|&nbsp;<asp:HyperLink ID="HelpLink" Runat=server NavigateUrl="HelpDataEntry.aspx" Target="Help">Help</asp:HyperLink>
</span><br><br>
<table>
  <tr>
    <td vAlign=top class=PollEditorFieldTitle>Question</td>
    <td vAlign=top>
    <asp:textbox id=QuestionText runat="server" Width="525px" Height="55px" EnableViewState="False" MaxLength="300"></asp:textbox>
    <asp:requiredfieldvalidator id=RequiredFieldValidator1 runat="server" ErrorMessage="The question is required." ControlToValidate="QuestionText" EnableViewState="False" CssClass=PollEditorValidationMessage ForeColor=" "></asp:requiredfieldvalidator></td></tr>
  <tr>
    <td class=PollEditorFieldTitle>Priority </td>
    <td><asp:textbox id=PriorityTextBox runat="server" Width="78px" EnableViewState="False"></asp:textbox>&nbsp;
    <span class=PollEditorHint>(1 is the highest; 100 is the lowest)</span> 
      <asp:RangeValidator ID=PriorityValidator1 Runat=server ControlToValidate=PriorityTextBox ErrorMessage="Please enter a number from 1 to 100." Type=Integer MinimumValue=1 MaximumValue=100 EnableViewState="False" CssClass=PollEditorValidationMessage ForeColor=" "/> </td></tr>
  <tr>
    <td class=PollEditorFieldTitle>Start Date </td>
    <td><Date:DateTextBox id=StartDateTextBox runat="server" Width="60pt" xErrorForeColor="Red" xShowErrorOnChangeB="True" xEndRangeControlID="EndDateTextBox" xCenturyBreak="0" xToolTipInStatusB="true" xInvalidDateMsg="The start date is invalid." xImageURL="../DateTextBox/calendar.jpg" xPopupURL="../DateTextBox/PopupCalendar.aspx" EnableViewState="False"></Date:DateTextBox>
    <Date:DateTextBoxVALIDATOR id=StartDateValidator runat="server" ErrorMessage="Invalid start" ControlToValidate="StartDateTextBox" EnableViewState="False" Display=Dynamic CssClass=PollEditorValidationMessage ForeColor=" "></Date:DateTextBoxVALIDATOR>
    <asp:RequiredFieldValidator ID=StartDateReqValidator Runat=server ControlToValidate=StartDateTextBox ErrorMessage="Please enter a start date." Display=Dynamic CssClass=PollEditorValidationMessage ForeColor=" "/>
    </td></tr>
  <tr>
    <td class=PollEditorFieldTitle>End Date</td>
    <td><Date:DateTextBox id=EndDateTextBox runat="server" Width="60pt" xErrorForeColor="Red" xShowErrorOnChangeB="True" xCenturyBreak="0" xToolTipInStatusB="true" xStartRangeControlID="StartDateTextBox" xInvalidDateMsg="The end date is invalid." xImageURL="../DateTextBox/calendar.jpg" xPopupURL="../DateTextBox/popupcalendar.aspx" EnableViewState="False"></Date:DateTextBox>
    <Date:DateTextBoxVALIDATOR id=EndDateValidator runat="server" ErrorMessage="Invalid end" ControlToValidate="EndDateTextBox" EnableViewState="False" Display=Dynamic CssClass=PollEditorValidationMessage ForeColor=" "></Date:DateTextBoxVALIDATOR>
    <asp:RequiredFieldValidator ID="EndDateTextBoxReqValidator" Runat=server ControlToValidate=EndDateTextBox ErrorMessage="Please enter an end date." Display=Dynamic CssClass=PollEditorValidationMessage ForeColor=" "/>
    </td></tr>
  <tr>
    <td vAlign=top class=PollEditorFieldTitle>Categories</td>
    <td vAlign=top><asp:textbox id=CategoriesTextBox Runat="server" Width="525px" Height="55px" TextMode="MultiLine" EnableViewState="False" MaxLength="50"></asp:textbox>
<br><span class=PollEditorHint>
Optional. Allows for very user specific poll 
      selection.</span>
<LVal:MaxLengthValidator id=MaxLengthValidator1 runat="server" ControlToValidate="CategoriesTextBox" ErrorMessage="Reduce this by {2} characters." CssClass=PollEditorValidationMessage ForeColor=" "></LVal:MaxLengthValidator> </td></tr></table>
<h3>Answers</h3>
<asp:PlaceHolder ID="AnswerGroup" Runat=server />  <!-- for the answer table -->
<br><asp:button id=SubmitButton runat="server" Text="Submit"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<asp:Button id=MoreButton runat="server" Text="More Answers" CausesValidation="False"></asp:Button></form>
   </body>
</HTML>
