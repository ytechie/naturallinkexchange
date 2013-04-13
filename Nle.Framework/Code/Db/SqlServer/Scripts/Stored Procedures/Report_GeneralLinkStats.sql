if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Report_GeneralLinkStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Report_GeneralLinkStats]
Go

CREATE PROCEDURE dbo.Report_GeneralLinkStats
AS

/*

Summary: This report displays the following statistics:
	- Number of link pages in the system
	- Number of links in the system
	- Number of sites in the system
	- Percentage of sites with links
	- Average number of incoming links/site

*/

Exec Reporting_GetReportDefinition 1, ''

Declare @FloatVal Float
Declare @TotalSites Int

Declare @Summary Table([Description] Varchar(100), Value Float)

--Total link pages in the system
Select @FloatVal = Count(*)
From LinkPages

Insert Into @Summary
Values('Number of link pages in the system', @FloatVal)

--Number of links in the system
Select @FloatVal = Count(*)
From LinkPages_Articles

Insert Into @Summary
Values('Number of links in the system', @FloatVal)

--Number of sites in the system
Select @TotalSites = Count(*)
From Sites

Insert Into @Summary
Values('Number of sites in the system', @TotalSites)

--Percentage of sites with links
Select @FloatVal = Case Count(lpa.[Id]) When 0 Then 0 Else Count(lpa.[Id]) / Cast(@TotalSites As Float) * 100 End
From Sites s
Inner Join LinkPages lp On lp.SiteId = s.[Id]
Inner Join LinkPages_Articles lpa On lpa.LinkPageId = lp.[Id]

Insert Into @Summary
Values('Percentage of sites with links', @FloatVal)

--Average number of incoming links/site
Select @FloatVal = Avg(Articles)
From
(Select Cast(Count(*) As Float) As 'Articles' --Get as a float for the average
From LinkPages_Articles lpa
Join LinkParagraphs lp On lp.[Id] = lpa.ArticleId
Join LinkParagraphGroups lpg On lpg.[Id] = lp.LinkParagraphGroupId
Group By lpg.SiteId) As x(Articles)

If @FloatVal Is Null
	Set @FloatVal = 0

Insert Into @Summary
Values('Average number of incoming links/site', @FloatVal)

--Return the report table
Select *
From @Summary

Go

GRANT EXECUTE ON dbo.Report_GeneralLinkStats TO [Public]
Go

/*
select * from linkpages
select * from linkpages_articles
select * from  linkparagraphs
select * from  linkparagraphgroups
select * from sites

select distinct siteid from linkpages
select * from sites

Select Count(s.[Id]), (Select [Id] From LinkPages lp Where lp.SiteId = s.[Id])
From Sites s
Group By s.[Id]


Report_GeneralLinkStats

*/