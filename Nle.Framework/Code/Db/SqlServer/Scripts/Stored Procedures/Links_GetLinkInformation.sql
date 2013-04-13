if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetLinkInformation]
GO

CREATE PROCEDURE dbo.Links_GetLinkInformation
  @LinkId Int = Null
AS

Declare @LinkCount Int

Select Top 1 @LinkCount = AnchorCount
From LinkParagraphs p
Join LinkParagraphGroups g On p.LinkParagraphGroupId = g.Id
Join Subscriptions s On s.SiteId = g.SiteId
Join LinkPackages lp On s.PlanId = lp.Id
Where p.Id = @LinkId
And ((GetUtcDate() > s.StartTime And s.EndTime Is Null) Or GetUtcDate() Between s.StartTime And s.EndTime)

Select p.Id, p.Title, p.Paragraph, p.Enabled, g.Url1 LinkUrl, g.AnchorText1 LinkText, g.Url2 LinkUrl2, g.AnchorText2 LinkText2,
	p.MarkedForDeletion, p.LinkParagraphGroupId, Coalesce(g.Keyword1, '{anchor1}') [Keyword1], Coalesce(g.Keyword2, '{anchor2}') [Keyword2], @LinkCount LinkCount
From LinkParagraphs p
Join LinkParagraphGroups g On p.LinkParagraphGroupId = g.Id
Where @LinkId Is Null Or p.[Id] = @LinkId

GO
GRANT EXECUTE ON dbo.Links_GetLinkInformation TO [Public]


/*
Links_GetLinkInformation 1
Links_GetLinkInformation 13

select * from LinkParagraphs
sp_columns LinkParagraphs
*/

