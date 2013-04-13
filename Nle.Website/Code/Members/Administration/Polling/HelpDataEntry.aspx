<%@ Page language="c#" Codebehind="HelpDataEntry.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.HelpDataEntry" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Help: Poll Editor</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
<meta content="Microsoft Visual Studio 7.0" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema><LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
<body>
<form id=HelpDataEntry method=post runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Runat="server" Text="Help: Poll Editor"></ASP:LABEL></div><br>Use the Poll Editor to add and 
edit a poll record. It is a data entry form where you fill in the text boxes. 
Then click <STRONG><FONT color=blue>Submit</FONT></STRONG>       
                 
   to save.<br><br>To 
add a poll, start on the Poll Administration page and click <FONT color=blue><STRONG>Add a Poll</STRONG></FONT>.<br>To edit a poll, start on the Poll Administration page and 
locate the desired poll. Then click <FONT color=blue><STRONG>Edit</STRONG></FONT>.<br>
<br>
<H3>Fields on the Poll Editor</H3>
<h4>Question</h4>
Provide a question which the user is going to answer. It doesn't have to be in the form of a question. 
For example, it can be "What is your favorite color?" or "Pick your favorite color."<br>
This field requires an entry. It is limited to 300 characters.
<h4>Priority</h4>
When you have multiple polls which match the selection criteria of today's date and categories, more than one
poll may be selected by the Poll control. The Poll control can show as many of the polls found as you want, or
just one. It uses the Priority field to determine which poll appears first, second, etc.<br>
 The Priority of 1 is the highest and 100 is the lowest. If several polls have the same
priority, they will appear in order added into the database.<br>
<h4>Start Date and End Date</h4>
A date range is required on each poll record. The Poll control always matches today's date to this date range.<br>
You can have multiple polls with overlapping date ranges. The Poll control can also use categories to narrow the list
and the Priority field to elevate certain polls over others.<br><br>
These date fields support a popup calendar. In addition, they have a number of keyboard shortcuts:<br>
<ol type=disc>
<li> Type T for today
<li> Type '+' or '.' for the next date.
<li> Type '-' or ',' for the previous date.
<li> If the date is in the present year, just type the 
  month and day.
<li> You can type years as two digit or four digit numbers. ('02' vs '2002')
</li>
</ol>
<h4>Categories</h4>
Categories allow the Poll control to find specific poll records. Each Poll control can optionally look
for words found in the Categories field. If there is a match, that poll is selected.<br>
Categories provide enormous flexibility. Some ideas include:
<ol type=disc>
<li> Multiple Poll controls on your site (even on the same page), each showing different polls.
<li> Selecting a poll based on information you've gathered from a user, such as buying preferences or location.
</li> 
</ol>
You should develop a list of categories first. They can be single words like "Entertainment" and "Sports" or
grouped like "Entertainment\Movies\StarWars", "Entertainment\Magazines\Newsweek", and "Sports\Baseball".
You can define as many categories as you want. Just be sure to omit spaces in them.<br>
<br>
When entering categories here, work from your list. Add as many categories as you'd like, separating them with
a space character. The field is limited to 500 characters.<br>
<br>
<b>Example</b><br><FONT face="Courier New">Music\Rock\RollingStones Ages\30-39 
Ages\40-49 Ages\50-59</FONT>
   
<br><br>
<em>Note: Categories have no effect by themselves. You must set up the Poll control to match to certain
categories. The Poll control has a powerful matching system and can match full or partial categories entered here.</em> 
<h4>Answers</h4>Enter the text for each answer here. You 
can add as many answers as you like. By default, there are 10 text boxes. Use 
the <STRONG><FONT color=blue>More Answers</FONT></STRONG>
                    
      button to extend the list by 5 (each time). If you want to delete
an answer, simply remove its text. The next time you edit this records, answers below the deletion
will be moved up automatically.<br>
<br>
<em>Note: Once the poll is used, each answer will maintain a vote count. You can add, edit and delete answers
after this point. However, don't change their order by swapping names. The vote count is maintained in the
original position and will be associated with the wrong answers.</em><br>
<br>
Answers can be up to 500 characters.<br>
</form>
	
  </body>
</HTML>
