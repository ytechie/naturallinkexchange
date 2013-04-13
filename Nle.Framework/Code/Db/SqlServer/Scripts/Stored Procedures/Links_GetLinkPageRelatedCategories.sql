if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPageRelatedCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetLinkPageRelatedCategories]
GO

CREATE PROCEDURE dbo.Links_GetLinkPageRelatedCategories
  @LinkPageId Int
AS

/*

Summary:
Gets the data for the link pages and categories that
are related to the specified link page id.

*/

Select lprc.RelatedLinkPageId 'LinkPageId', c.*
From LinkPages_RelatedCategories lprc
Join LinkPages lp On lp.[Id] = lprc.RelatedLinkPageId
Join Categories c On c.[Id] = lp.CategoryId
Where lprc.LinkPageId = @LinkPageId

GO

GRANT EXECUTE ON dbo.Links_GetLinkPageRelatedCategories TO [Public]
Go


/*

Links_GetLinkPageRelatedCategories 25

select * from linkpages_relatedcategories

delete from linkpages_relatedcategories

sp_columns categories

*/

