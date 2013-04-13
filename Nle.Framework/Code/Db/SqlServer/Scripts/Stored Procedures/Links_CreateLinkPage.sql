if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_CreateLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_CreateLinkPage]
Go

CREATE PROCEDURE dbo.Links_CreateLinkPage
  @SiteId Int,
  @CategoryId Int
AS

/*

Summary: Creates a new link page for the specified site and category.

- For the article target, we need to generate a random number
  within the users parameters.
- To make sure that all link pages are accessible, we have to make sure
	that all new link pages trade a link with an exising page.

*/

Declare @LinkPageId Int
Declare @ArticleTarget Int

Set @ArticleTarget = Round((Rand() * 9 + 1), 0)

Declare @PageName Varchar(50)
Declare @PageTitle Varchar(70)

Set @PageName = dbo.Links_GetUniquePageName(@SiteId, @CategoryId)
Set @PageTitle = dbo.Links_GetUniquePageTitle(@SiteId, @CategoryId)

--Insert the new link page
Insert Into LinkPages
(SiteId, CategoryId, ArticleTarget, PageName, PageTitle)
Values(@SiteId, @CategoryId, @ArticleTarget, @PageName, @PageTitle)

Set @LinkPageId = @@Identity

--Make sure that the category is linked to one that is already
--in the tree
Exec Links_RelateCategory @LinkPageId

Return @LinkPageId

Go

GRANT EXECUTE ON dbo.Links_CreateLinkPage TO [Public]
Go

/*
select rand()

sp_columns categories

select * from linkpages
select * from linkpages_relatedcategories
select * from sites

Links_CreateLinkPage 1, 57

*/

