if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_SelectInitialLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_SelectInitialLinkPage]
GO

CREATE PROCEDURE dbo.[Links_SelectInitialLinkPage]
  @SiteId Int,
  @InitialLinkPageId Int Output
AS

/*

Summary:
Selects a link page for the site that is the most appropriate for the intitial link page.

The following criteria are used
- Look for a link page in the same category
- Look for any link page

Todo: Look for an initial category that is just related to our main category

*/

Declare @SiteCategoryId Int

Select @InitialLinkPageId = s.StartLinkPageId, @SiteCategoryId = s.InitialCategoryId
From Sites s
Where s.[Id] = @SiteId

--Make sure we don't already have an initial link page
If @InitialLinkPageId Is Not Null
	Begin
		Print 'There is already an initial link page'
		Return @InitialLinkPageId
	End

--Try to find a site with the same category
Select @InitialLinkPageId = lp.[Id]
From LinkPages lp
Where SiteId = @SiteId
And CategoryId = @SiteCategoryId

If @InitialLinkPageId Is Not Null
	Begin
		Print 'Chose a link page that was in the same category as the site'
		Exec Links_SetInitialLinkPage @SiteId, @InitialLinkPageId
		Return
	End
Else
	Begin
		Print 'A link page could not be found in the same category as this site'
	End

--Try to find any link page
Select @InitialLinkPageId = lp.[Id]
From LinkPages lp
Where SiteId = @SiteId

If @InitialLinkPageId Is Not Null
	Begin
		Print 'Randomly chose a link page to use as the initial link page'
		Exec Links_SetInitialLinkPage @SiteId, @InitialLinkPageId
		Return
	End
Else
	Begin
		Print 'Could not find any pages to use as the link page'
	End

Print 'No link page could be found to set at the initial page'
Set @InitialLinkPageId = Null
Return

GO

GRANT EXECUTE ON dbo.[Links_SelectInitialLinkPage] TO [Public]
Go


/*
Declare @Id Int
Exec @Id = [Links_SelectInitialLinkPage] 1
print @Id


select * from sites
select * from linkpages
*/

