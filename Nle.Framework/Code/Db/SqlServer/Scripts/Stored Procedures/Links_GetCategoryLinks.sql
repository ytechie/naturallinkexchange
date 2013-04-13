if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetCategoryLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetCategoryLinks]
GO

CREATE PROCEDURE dbo.Links_GetCategoryLinks
  @SiteId Int,
  @CategoryId Int
AS

Select lp.Id, lp.Title, lp.Paragraph, lp.Enabled, lpg.Url1 LinkUrl, lpg.AnchorText1 LinkText, lpg.Url2 LinkUrl2, lpg.AnchorText2 LinkText2, lp.SiteId, lp.MarkedForDeletion, lp.LinkParagraphGroupId
From SiteLinkCache slc
Join LinkParagraphs lp On slc.ParagraphId = lp.[Id]
Join LinkParagraphGroups lpg On lp.LinkParagraphGroupId = lpg.Id
Where slc.SiteId = @SiteId
And slc.CategoryId = @CategoryId

GO

GRANT EXECUTE ON dbo.Links_GetCategoryLinks TO [Public]
Go


/*
Links_GetCategoryLinks 1, 57

select * from categories
select * from SiteParagraphMappings
select * from LinkParagraphs
sp_columns LinkParagraphs
*/

