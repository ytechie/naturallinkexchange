<%@ Page language="c#" Codebehind="HelpPollUsageReport.aspx.cs" AutoEventWireup="false" Inherits="PeterBlum.Poll_WebAdmin.HelpPollUsageReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
    <title>Help: Usage Report</title>
<!--- Copyright 2002 Peter L. Blum, All Rights Reserved. www.PeterBlum.com -->
    <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK href="PollAdminStyleSheet.css" type=text/css rel=stylesheet >
  </HEAD>
  <body >
	
    <form id="HelpPollUsageReport" method="post" runat="server">
<div class=PageTitle><ASP:LABEL id=PageTitle Runat="server" Text="Help: Usage Report"></ASP:LABEL></div><br>
The Usage Report shows how votes were received over time. Each time a vote is submitted, your database is updated
to reflect that vote along with its date and the associated poll. Use the Usage Report to determine usage patterns.
Some examples include:
<ol type=disc>
<li>Times of the day and week which get responses
<li>See how individual polls perform
<li>See how categories perform</li>
</ol>
With this knowledge, you can fine tune the polls you offer to entice response and returns to your site.<br>
<br>
The 
Usage Report is available for individual polls from the Poll Results page and for groups of polls from the Poll Administration page. It
generates a bar graph showing the total votes submitted for each period of time. You select the period size, which
can be from every half hour to one week. You also can limit the records into a particular date range. The
resulting graph shows a bar for each period which has data. If a period had no data, it will be omitted
and the next bar will show its label in a special color to highlight the gap.<br>
<br>
<h3>Setting Up The Report</h3>
Use the fields at the top of the page to establish any selection criteria, 
such as categories and dates, and formatting such as bar colors and period size. 
Then click <STRONG><FONT color=blue>Draw The Graph</FONT></STRONG>. 
<h4>Select By Categories</h4>
<em>The Categories fields are not offered when you run a Usage Report from the Poll Results page.</em>
<br>
<br>
Initially, the Usage Report will include all polls. Use the Categories fields to narrow the data to polls
associated with one or more categories. You can enter up to four categories and select whether each poll
must match any or all with the radio buttons to the left.<br><br>
Since you can define complex categories like "Entertainment\Music\RollingStones", the drop down lists
next to each text box let you determine where your text is found in those categories.
<ol type=disc>
<li>Exact - the text must match the full category.
<li>Contains - the text can be any part of the category.
<li>Starts with - the text must begin a category.
<li>Ends with - the text must be at the end of the category.
</li>
</ol>
Matching is always case insensitive.<br>
<h5>Category Commands</h5>
    The fields of this row make it easier to work with categories. After all, who wants to
set up the category fields each time?<br><br>
This system retains your category fields using cookies in your browser. So if you have disabled cookies, these
commands will not work. There are 10 slots which can retain your settings. They are named "Categories 1" through
"Categories 10" and shown in the <b><FONT color=blue>Quick Lookup</FONT></b> drop down list. You may want to 
associate them with individual Poll controls so you can report on how each Poll control did. 
Here's how to save and retrieve category settings:<br><br>
To save, choose the 
slot where you want to save from the drop down list then click 
<b><FONT color=blue>Save</FONT></b>.<br>
To retrieve, choose the slot containing the saved settings from the drop down list and click 
<b><FONT color=blue>Get</FONT></b>.<br>
<br>
Use the <b><FONT color=blue>Clear Categories</FONT></b> button to set the category fields to their defaults.<br>
<br>
<em> While this page doesn't let you change the names of the slots, here is a trick to do so. Edit the file Poll_WebAdmin\CategoryQuickLookup.xml. It houses
the names of the slots and is designed so you can edit it, so
long as you maintain the XML structure it uses.</em><br>
<em>NOTE: 
The Quick Lookup's data is shared with other features in this program.</em> 
<h4>Date Range</h4>
Limit the report to votes within this date range. If the start date text box is blank, all records from the beginning
are retrieved. If the end date text box is blank, all records through the current date are retrieved.<br>
<h4>Period Size</h4>
Use the Period Size drop down list to determine the size of a period represented by each bar. Choose from
sizes of half hour to one week.
<h4>Odd Bar Color and Even Bar Color</h4>
Use these drop down lists to choose the colors of bars. The Odd Bar Color is applied to the first, third, fifth, and so on
bars while the Even Bar Color is applied to the rest.
<h4>Bar Height</h4>
Use the Bar Height drop down list to reduce or enlargen the bar height and associated font size.
<h4>Color following a gap in the data</h4>
When a period has no data, the next bar shown will show its label in this color to point out the gap.
    </form>
	
  </body>
</HTML>
