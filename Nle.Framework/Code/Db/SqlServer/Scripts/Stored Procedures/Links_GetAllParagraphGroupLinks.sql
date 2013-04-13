if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetAllParagraphGroupLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetAllParagraphGroupLinks]
GO

CREATE PROCEDURE dbo.Links_GetAllParagraphGroupLinks
  @GroupId Int
AS

Declare @LinkCount Int

Select Top 1 @LinkCount = AnchorCount
From LinkParagraphGroups g
Join Subscriptions s On s.SiteId = g.SiteId
Join LinkPackages lp On s.PlanId = lp.Id
Where g.Id = @GroupId
And ((GetUtcDate() > s.StartTime And s.EndTime Is Null) Or GetUtcDate() Between s.StartTime And s.EndTime)

Select p.Id, p.Title, p.Paragraph, p.Enabled, g.Url1 LinkUrl, g.AnchorText1 LinkText, g.Url2 LinkUrl2, g.AnchorText2 LinkText2, p.MarkedForDeletion, p.LinkParagraphGroupId,
	Coalesce(g.Keyword1, '{anchor1}') [Keyword1], Coalesce(g.Keyword2, '{anchor2}') [Keyword2], @LinkCount LinkCount
From LinkParagraphs p
Join LinkParagraphGroups g On p.LinkParagraphGroupId = g.Id
Where p.LinkParagraphGroupId = @GroupId And p.MarkedForDeletion = 0

GO

GRANT EXECUTE ON dbo.Links_GetAllParagraphGroupLinks TO [Public]
Go


/*
Links_GetAllParagraphGroupLinks 8
select * from linkparagraphgroups
*/

