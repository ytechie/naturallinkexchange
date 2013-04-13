if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetRelatedLinkPages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetRelatedLinkPages]
GO

CREATE PROCEDURE dbo.Links_GetRelatedLinkPages
  @LinkPageId Int
AS

/*

Summary:
Gets the link pages that are related to the specified link page.

*/

Select lp.*
From LinkPages_RelatedCategories lprc
Join LinkPages lp On lp.[Id] = lprc.RelatedLinkPageId
Where lprc.LinkPageId = @LinkPageId

GO

GRANT EXECUTE ON dbo.Links_GetRelatedLinkPages TO [Public]
Go


/*

Links_GetRelatedLinkPages 55

select * from linkpages
select * from linkpages_relatedcategories

delete from linkpages_relatedcategories

sp_columns categories

*/

