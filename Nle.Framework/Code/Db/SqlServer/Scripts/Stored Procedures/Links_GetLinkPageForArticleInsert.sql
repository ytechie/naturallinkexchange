if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPageForArticleInsert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_GetLinkPageForArticleInsert]
Go

CREATE PROCEDURE dbo.Links_GetLinkPageForArticleInsert
  @SiteId Int,
  @CategoryId Int,
  @CanUseFirstPosition Bit --If 1, then this article can fill a #1 slot
AS

/*

Summary: Gets the ID of a link page matching the specified criteria that is ready to have an article
					added.  If a link page is full, a new one is created.

- If the link page can use the first position, see if we can find one
- Find the last link page matching the criteria
- See if the link page is full, if so, create a new one

Returns: The ID of the link page matching the criteria
*/

Print 'Getting a link page to insert a new link for site #' + Cast(@SiteId As Varchar(10)) + ', Category #' + Cast(@CategoryId As Varchar(10))

Declare @LinkPageId Int

If @CanUseFirstPosition = 1
	--if the article can take the first position, let's see if we can find a link page
	--that doesn't have an article in the first position
	Select Top 1 @LinkPageId = lpa.LinkPageId --, lp.PageNumber, Count(lpa.[Id]) - lp.ArticleTarget 'DistOffTarget'
	From LinkPages lp	
	Join LinkPages_Articles lpa On lpa.LinkPageId = lp.[Id]
	Where lp.SiteId = @SiteId
	And lp.CategoryId = @CategoryId
	And 1 Not In (Select PositionOrder From LinkPages_Articles lpai Where lpai.[Id] = lpa.[Id])
	Group By lpa.LinkPageId, lp.ArticleTarget
	Having Count(lpa.[Id]) < lp.ArticleTarget --The number of articles should be less than the target
	Order By Count(lpa.[Id]) - lp.ArticleTarget Asc

--Check if we found a suitable link page
If @LinkPageId Is Not Null
	Begin
		Print 'Found a link page that has the first slot available'
		Return @LinkPageId
	End

--Order the link pages by how far off target they are, and get the top one
Select Top 1 @LinkPageId = lpa.LinkPageId --, lp.PageNumber, Count(lpa.[Id]) - lp.ArticleTarget 'DistOffTarget'
From LinkPages lp	
Join LinkPages_Articles lpa On lpa.LinkPageId = lp.[Id]
Where lp.SiteId = @SiteId
And lp.CategoryId = @CategoryId
Group By lpa.LinkPageId, lp.ArticleTarget
Having Count(lpa.[Id]) < lp.ArticleTarget --The number of articles should be less than the target
Order By Count(lpa.[Id]) - lp.ArticleTarget Asc

--Check if we found a suitable link page
If @LinkPageId Is Not Null
	Begin
		Print 'Found a suitable link page (' + Cast(@LinkPageId As Varchar(Max)) + ')'
		Return @LinkPageId
	End

Print 'A suitable link page could not be found to put the link on, so one will be created'

--If we get here, it means there are no available link pages, so we
--need to create one.
Exec @LinkPageId = Links_CreateLinkPage @SiteId, @CategoryId

Return @LinkPageId

Go

GRANT EXECUTE ON dbo.Links_GetLinkPageForArticleInsert TO [Public]
Go

/*
select rand()

select * from linkpages
select * from linkpages_articles

update linkpages_articles
set positionorder = 1
where id= 77

Links_GetLinkPageForArticleInsert 675, 23, 1
Links_GetLinkPageForArticleInsert 675, 23, 0

*/

