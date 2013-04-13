if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_UpdateSiteRelatedCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_UpdateSiteRelatedCategories]
GO

CREATE PROCEDURE dbo.Categories_UpdateSiteRelatedCategories
  @SiteId Int
AS

/*

Summary:
Updates the categories for all of the link pages belonging to
the specified site.  This SP simply loops through all of the link
pages for the specified site, and calls another stored procedure
to update that specific link page.

*/

Declare @CurrId Int
Declare LinkPageCursor Cursor For
Select [Id] From LinkPages Where SiteId = @SiteId

Open LinkPageCursor
Fetch Next From LinkPageCursor Into @CurrId

While @@Fetch_Status = 0
  Begin
    Exec Categories_UpdateLinkPageRelatedCategories @CurrId
    Print 'Updated Related Categories For Link Page #' + Cast(@CurrId As Varchar(10))
    Fetch Next From LinkPageCursor Into @CurrId
  End

Close LinkPageCursor
Deallocate LinkPageCursor

GO

GRANT EXECUTE ON dbo.Categories_UpdateSiteRelatedCategories TO [Public]
Go

/*
Categories_UpdateSiteRelatedCategories 37

select * from linkpages
select * from linkpages_relatedcategories

delete from linkpages_relatedcategories

*/


