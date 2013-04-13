if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_LinkArticle]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_LinkArticle]
Go

CREATE PROCEDURE dbo.Links_LinkArticle
  @SiteId Int, --The site to link from
  @ArticleId Int  --The article to link to
AS

/*

Summary: This SP creates a link from the specified site to
  the specified link article.


- If the current position is a #1 spot, determine if the user has
  already been put into a number #1 position.  If they have, assign
  them the #2 position, which will be first for now.

*/

Declare @CategoryId Int
Declare @LinkPageId Int
Declare @ToSiteId Int --The site we are linking to

--Look up the category and site of the article
Select @CategoryId = s.InitialCategoryId, @ToSiteId = lpg.SiteId
From LinkParagraphs lp
Join LinkParagraphGroups lpg On lp.LinkParagraphGroupId = lpg.[Id]
Join Sites s On lpg.SiteId = s.[Id]
Where lp.[Id] = @ArticleId

Declare @FirstPositionCount Int
Declare @CanUseFirstPosition Bit

--Determine if we can take the first position
Select @FirstPositionCount = Count(*)
From LinkPages_Articles lpa
Join LinkPages lp On lpa.LinkPageId = lp.[Id]
Where lp.CategoryId = @CategoryId
And lp.SiteId = @SiteId
And lpa.PositionOrder = 1

If @FirstPositionCount = 0
	Set @CanUseFirstPosition = 1
Else
	Set @CanUseFirstPosition = 0

--Get the link page to insert the article into
Exec @LinkPageId = Links_GetLinkPageForArticleInsert @SiteId, @CategoryId, @CanUseFirstPosition

Declare @PositionOrder Int

--See if the first position is available on the link page we found
If (Select Count(lpa.[Id])
		From LinkPages_Articles lpa
		Where lpa.LinkPageId = @LinkPageId
		And lpa.PositionOrder = 1) = 0
	Set @PositionOrder = 1 --we'll take the first spot	
Else
	Select @PositionOrder = Max(lpa.PositionOrder) + 1
	From LinkPages_Articles lpa
	Where lpa.LinkPageId = @LinkPageId

Insert Into LinkPages_Articles
(LinkPageId, ArticleId, Added, PositionOrder)
Values(@LinkPageId, @ArticleId, GetUtcDate(), @PositionOrder)

Print 'Added A Link From Site #' + Cast(@SiteId As Varchar(10)) + ' To Article #' + Cast(@ArticleId As Varchar(10))

Go

GRANT EXECUTE ON dbo.Links_LinkArticle TO [Public]
Go

/*
select * from linkpages
select * from linkpages_articles
select * from  linkparagraphs
select * from  linkparagraphgroups
select * from sites

Update linkpages
set articletarget = 5

Links_AddLinks 1, 5

select * from linkparagraphgroups

*/

