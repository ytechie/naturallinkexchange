if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_RelateCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_RelateCategory]
GO

CREATE PROCEDURE dbo.Links_RelateCategory
  @LinkPageId Int
AS

/*

Summary:
Relates the specified link page with an exising 

- Note: Always make sure that a new link page has a reciprocal link with
				another link page, so that we can be sure that all link pages are
				accessible.

*/

Declare @SiteId Int
Declare @CategoryId Int
Declare @RelatedCount Int
Declare @DeltaCount Int
Declare @LinkPageCount Int

Print 'Attempting to find another link page to link with link page #' + Cast(@LinkPageId As Varchar(10))

--Look up the category and site
Select @CategoryId = lp.CategoryId, @SiteId = lp.SiteId
From LinkPages lp
Where lp.[Id] = @LinkPageId

Print 'Link page belongs to site #' + Cast(@SiteId As Varchar(10))

--Check if this is the only link page for this site
Select @LinkPageCount = Count(*)
From LinkPages lp
Where lp.SiteId = @SiteId

If @LinkPageCount = 1
	Begin
		Print 'This is the only link page for the site, no relationships can be added'
		Return
	End

--Figure out how many related categories we already have
Select @RelatedCount = Count(*)
From LinkPages_RelatedCategories
Where LinkPageId = @LinkPageId

--If we already have 10 more more related categories, that is enough
If @RelatedCount >= 10
	Return

Set @DeltaCount = 10 - @RelatedCount

--This can't be a table variable because of the insert line below
Create Table #RelatedCategories
(
  [Id] Int
)

--Get a list of all the possible related categories
Insert #RelatedCategories
Exec Categories_GetAllRelatedCategories @CategoryId

Declare @RelatedLinkPages Table
(
	[Id] Int
)

--Find link pages that are for the same site that are for related categories
Insert Into @RelatedLinkPages
Select lp.[Id]
From LinkPages lp
Where CategoryId In (Select rc.[Id] From #RelatedCategories rc)
And SiteId = @SiteId

--Relate the categories
Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Select @LinkPageId, rlp.[Id]
From @RelatedLinkPages rlp

If @@RowCount = 0
  Begin
		Print 'No related categories were able to linked'

		Declare @PartnerId Int

		--Find the category with the least number of relationships
		Select Top 1 @PartnerId = lp.[Id]--, Count(lprc.[LinkPageId]) 'RelationshipCount'
		From LinkPages lp
		Left Outer Join LinkPages_RelatedCategories lprc On lprc.LinkPageId = lp.[Id]
		Where lp.SiteId = @SiteId
		And lp.[Id] <> @LinkPageId
		Group By lp.[Id]
		Order By Count(lprc.[LinkPageId]) Asc

		If @PartnerId Is Null
			Begin
				--We have to throw an error because it's bad if a page doesn't
				--get related to the other pages.
				Raiserror('Count not find a link page to partner with', 16, 1)
				Return
			End
		
		Exec Links_RelateCategories @LinkPageId, @PartnerId
	End

Drop Table #RelatedCategories

GO

GRANT EXECUTE ON dbo.Links_RelateCategory TO [Public]
Go


/*
select * from LinkPages_RelatedCategories
select * from linkpages where siteid = 133

delete from linkpages_relatedCategories

Links_RelateCategory 275
*/

