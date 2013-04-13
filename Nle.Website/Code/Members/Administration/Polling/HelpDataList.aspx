<%@ Page language="c#" Codebehind="HelpDataList.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.HelpDataList" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Help: Poll Administration</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
<LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
<body>
<form id=HelpDataList method=post runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Text="Help: Poll Administration" Runat="server"></ASP:LABEL></div><br>
     The Poll Administration page is the main page of the Poll Administration system. From here, you can take virtually
     any action from editing the data to running reports.<br><br>
     The centerpiece of this page is a <FONT color=blue><STRONG>list</STRONG></FONT> of the polls in your database. Along 
the left of each poll record are commands to <STRONG><FONT 
color=blue>Test</FONT></STRONG> the poll's data, show <FONT color=blue><STRONG> 
Results</STRONG></FONT>, <FONT 
color=blue><STRONG>Edit</STRONG></FONT>, and <FONT color=blue><STRONG>Delete</STRONG></FONT>. 
The <FONT color=blue><STRONG>Add a Poll</STRONG></FONT> 
command is above the list. <EM>Each of these commands open to a new page with 
its own documenation.</EM>
                  
              
          <br><br>
Your database can maintain 
an unlimited number of polls, many running simultaneously (when you use 
categories). The Poll List becomes unmanageble with too much data. So use the 
<FONT color=blue><STRONG>Poll list contains</STRONG></FONT>
               
               drop down list to narrow the list
to those currently active, already ran and yet to run.<br><br>
You can run a Usage Report against all or selected polls with the 
<FONT color=blue><STRONG>Usage Report</STRONG></FONT> command at the top of the page.<br></form>
	
  </body>
</HTML>
