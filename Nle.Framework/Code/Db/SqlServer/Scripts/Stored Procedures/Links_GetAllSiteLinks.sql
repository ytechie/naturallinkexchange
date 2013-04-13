if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetAllSiteLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetAllSiteLinks]
GO

CREATE PROCEDURE dbo.Links_GetAllSiteLinks
  @SiteId Int
AS

Select p.Id, p.Title, p.Paragraph, p.Enabled, g.Url1 LinkUrl, g.AnchorText1 LinkText, g.Url2 LinkUrl2, g.AnchorText2 LinkText2, g.SiteId, p.MarkedForDeletion, p.LinkParagraphGroupId
From LinkParagraphs p
Join LinkParagraphGroups g On p.LinkParagraphGroupId = g.Id
Where g.SiteId = @SiteId 

GO

GRANT EXECUTE ON dbo.Links_GetAllSiteLinks TO [Public]
Go


/*
Links_GetAllSiteLinks 1
Links_GetAllSiteLinks 1, 57

select * from categories
select * from SiteParagraphMappings
select * from LinkParagraphs
sp_columns LinkParagraphs
*/

