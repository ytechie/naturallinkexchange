if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetNextSiteLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_GetNextSiteLink]
Go

CREATE PROCEDURE dbo.Links_GetNextSiteLink
  @SiteId Int
AS

/*

Summary:
Retrieves the link for the specified site.

Process:
- Choose a group that is not "meeting" it's percentage goal
- From that group, choose the article that is used the least

*/

Declare @Group Int --this is the group we choose to get the link from
Declare @Link Int --this will be our return value
Declare @GroupPercent Float
Declare @LinkCount Int

Print 'Looking for the link group that is farthest behind it''s percentage goal'

Select lgd.[Id], lgd.LinkCount, lg.Distribution 'DistributionTarget',
  Case Coalesce(Sum(LinkCount), 0) When 0 Then 0 Else LinkCount / Sum(LinkCount) * 100 End 'OverallPercent'
Into #GroupStats
From Links_GetLinkGroupDistribution(@SiteId) lgd
Join LinkParagraphGroups lg On lgd.[Id] = lg.[Id]
Where lg.Distribution > 0
Group By lgd.[Id], lgd.LinkCount, lg.Distribution
Order By lgd.LinkCount

Select Top 1 @Group = [Id], @GroupPercent = (OverallPercent / DistributionTarget) * 100
From #GroupStats
Where DistributionTarget > 0
Order By (OverallPercent / DistributionTarget) Asc, --Order by how far off target we are
DistributionTarget Desc --Also order by the distribution percent as a tie breaker

Drop Table #GroupStats

If @Group Is Null
  Begin
    Print 'No Valid Group Was Found'
    Return 0
  End

Print 'Group #' + Cast(@Group As Varchar(10)) + ' Is At ' + Cast(@GroupPercent As Varchar(100)) + '% Target'

--We we need to choose an article from the group that has been used the least
Select Top 1 @Link = lp.[Id], @LinkCount = Count(lpa.[Id])
From LinkParagraphs lp
Left Outer Join LinkPages_Articles lpa On lpa.ArticleId = lp.[Id]
Where lp.LinkParagraphGroupId = @Group --Filter the articles by the group we're looking for
Group By lp.[Id]

Print 'Choosing Article #' + Cast(@Link As Varchar(10)) + '.  It Is Used ' + Cast(@LinkCount As Varchar(10)) + ' Times'

Return @Link

Go

GRANT EXECUTE ON dbo.Links_GetNextSiteLink TO [Public]
Go

/*
select * from linkpages
select * from linkparagraphs
select * from linkpages_articles
select * from linkparagraphgroups

Declare @ReturnVal Int
Exec @ReturnVal = Links_GetNextSiteLink 1
print 'Return Value: ' + Cast(@ReturnVal As Varchar(100))

*/

