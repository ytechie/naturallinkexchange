<%@ Page language="c#" Codebehind="HelpDataPlanner.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.HelpDataPlanner" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Help: Poll Daily Planner</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema><LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
<body MS_POSITIONING="FlowLayout">
<form id=HelpDataPlanner method=post runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Text="Help: Poll Daily Planner" Runat="server"></ASP:LABEL></div>
<br>Use 
the Poll Daily Planner to see which polls will appear on a Poll control for any 
given date. The Poll control has access to your entire poll database. It uses 
today's date and any categories that you've assigned to narrow the list. It uses 
the Priority field on each Poll record to order them. It can display all of them 
or shorten the list further. The Poll Daily Planner clarifies which records will 
appear by showing a list of polls, based on date and category criteria that you 
supply and ordered by priority.<br><br>To use the Poll Daily Planner, fill in the Data Settings 
and click <FONT color=blue><b>Show The 
Polls</b></FONT>. At minimum, enter the date which you are planning into the <b><FONT color=blue>Date to 
View</FONT></b> field. Once the list is displayed, it has the same commands as 
the Poll Administration page where you can add, edit, delete, and view entry and 
results.<br><br>When you have a Poll 
control that uses categories, you should enter the same criteria in <b><FONT color=blue>Select by 
Categories</FONT></b>. To save you some time, the Poll Daily Planner includes 
the <b><FONT color=blue>Quick 
Lookup</FONT></b> fields to save and retrieve your criteria. These two concepts 
are detailed below.
<h4>Select By Categories</h4>Use the <b><FONT color=blue>Select by 
Categories</FONT></b> fields to narrow the data to the same categories found on 
one of your Poll control. You must first determine those categories. 
Unfortunately, this page cannot retrieve them from the Poll control because each 
poll control has its settings on other pages. The category data is found on the 
Poll control's <b>xCategories</b> property.<br><br>Use the <b><FONT color=blue>Any</FONT></b> and <b><FONT color=blue>All</FONT></b> radio 
buttons to match the <b>xCategories-xOrConditionB</b> 
properties. When <b>xOrConditionB</b> is <b>true</b>, check the <b><FONT 
color=blue>Any</FONT></b> radio button.<br><br>The remaining settings of <b>xCategories</b> are the <b>xMatchList</b> property which determine the text and rule 
to find the text applies. The rules are: 
<ul type=disc>
  <li>Exact - the text must match the full category. 
  <li>Contains - the text can be any part of the category. 
  <li>Starts with - the text must begin a category. 
  <li>Ends with - the text must be at the end of the 
  category. </li></ul>Matching is always case insensitive.<br><br>
<h5>Example</h5>Suppose your Poll control has this within 
it's ASP.NET page:<br>&lt;Poll:Poll id=Poll1 runat=server 
xCategories-xOrConditionB=false [many other attributes]&gt;<br>&nbsp;&nbsp;&lt;xCategories&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;Poll:MatchListItem 
xTextToMatch="Food" xTextMatchRule="Exact" /&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;Poll:MatchListItem 
xTextToMatch="NewEngland" xTextMatchRule="EndsWith" /&gt;<br>&nbsp;&nbsp;&lt;/xCategories&gt;<br>&lt;/Poll:Poll&gt;<br><br>Enter it as follows:<br>Mark the <b><FONT color=blue>All</FONT></b> radio 
button because <b>xOrConditionB</b> is false.<br>
Set the first category drop down list to <b>Exact</b> and the text to "Food".<br>
Set the second category drop down list to <b>EndsWith</b> and the text to "NewEngland".<br>
<h4>Category Commands</h4>
     The fields of this row make it easier to work with categories. After all, who wants
set up the category fields each time?<br><br>
 This system retains your category fields using cookies in your browser. So if you have disabled cookies, these
commands will not work. There are 10 slots which can retain your settings. They are named "Categories 1" through
"Categories 10" and shown in the <b><FONT color=blue>Quick Lookup</FONT></b> drop down list. You should associate 
them with individual Poll controls. Here's how to save and retrieve
category settings:<br><br>
To save, choose the 
slot where you want to save from the drop down list then click 
<b><FONT color=blue>Save</FONT></b>.<br>
To retrieve, choose the slot containing the saved settings from the drop down list and click 
<b><FONT color=blue>Get</FONT></b>.<br>
<br>
Use the <b><FONT color=blue>Clear Categories</FONT></b> button to set the category fields to their defaults.<br>
<br>
<em> While this page doesn't let you change the names of the slots,&nbsp;here is a trick to do so. Edit the file Poll_WebAdmin\CategoryQuickLookup.xml. It houses
the names of the slots and is designed so you can edit it, so
long as you maintain the XML structure it uses.</em><br>
<em>NOTE: The Quick Lookup's data is shared with other features in this program.</em> 
</form>
	
  </body>
</HTML>
