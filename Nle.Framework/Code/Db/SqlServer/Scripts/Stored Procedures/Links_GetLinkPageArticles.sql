if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPageArticles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetLinkPageArticles]
GO

CREATE PROCEDURE dbo.Links_GetLinkPageArticles
  @LinkPageId Int
AS

Declare @LinkCount Int
Declare @SiteId Int

Select @SiteId = SiteId From LinkPages Where Id = @LinkPageId

Select Top 1 @LinkCount = lp.AnchorCount
From Subscriptions s
Join LinkPackages lp On lp.Id = s.PlanId
Where s.SiteId = @SiteId
And ((GetUtcDate() > s.StartTime And s.EndTime Is Null) Or GetUtcDate() Between s.StartTime And s.EndTime)

Select lp.Id, lp.Title, lp.Paragraph, lp.Enabled, lp.MarkedForDeletion, lp.LinkParagraphGroupId, 
	su.Url 'UrlBase', lpg.Url1 'LinkUrl', lpg.Url2 'LinkUrl2', lpg.AnchorText1 'LinkText', lpg.AnchorText2 'LinkText2',
	Coalesce(lpg.Keyword1, '{anchor1}') [Keyword1], Coalesce(lpg.Keyword2, '{anchor2}') [Keyword2], @LinkCount LinkCount
From LinkPages_Articles lpa
Join LinkParagraphs lp On lpa.ArticleId = lp.[Id]
Join LinkParagraphGroups lpg On lpg.[Id] = lp.LinkParagraphGroupId --Find the group
Join Sites s On s.[Id] = lpg.SiteId
Join SiteUrls su On su.[Id] = s.RootUrlId
Where lpa.[LinkPageId] = @LinkPageId

Go

GRANT EXECUTE ON dbo.Links_GetLinkPageArticles TO [Public]
Go


/*
Links_GetLinkPageArticles 1082

select * from linkpages where siteid = 804
select * from sites where name like 'obi%'

select * from categories
select * from SiteParagraphMappings
select * from LinkParagraphs
select * from LinkParagraphgroups
sp_columns LinkParagraphs
*/

