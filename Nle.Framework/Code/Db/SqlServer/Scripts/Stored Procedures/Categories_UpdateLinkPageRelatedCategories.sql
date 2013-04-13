if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_UpdateLinkPageRelatedCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_UpdateLinkPageRelatedCategories]
GO

CREATE PROCEDURE dbo.Categories_UpdateLinkPageRelatedCategories
  @LinkPageId Int
AS

/*

Summary:
Updates the related categories for the specified link page.

Process:
- Link child categories
- Link sibling categories
- Link parent category
- Link manually related categories
- If more links are necessary, load categories that are related to our
  related categories
- If still more links are necessary, choose some at random.

*/

--Todo: Look up the limit
Declare @CategoryLimit Int
Set @CategoryLimit = 5

Declare @CategoryId Int
Select @CategoryId = CategoryId
From LinkPages
Where [Id] = @LinkPageId

--Insert child categories if possible
Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Select @LinkPageId, lp2.[Id]
From LinkPages lp
Join Categories sc On sc.ParentCategoryId = lp.CategoryId --Get child categories
Join LinkPages lp2 On lp2.SiteId = lp.SiteId And lp2.CategoryId = sc.[Id] --Find the link pages that meet the criteria
Where lp.[Id] = @LinkPageId
And sc.[Id] <> @CategoryId --Don't add a link to ourself
And sc.[Id] Not In (Select [Id] From Categories_GetLinkPageRelatedCategories(@LinkPageId))

Print Cast(@@Rowcount As Varchar(10)) + ' Child Category Relationships Were Added For Link Page #' + Cast(@LinkPageId As Varchar(10))

--Insert parent category
Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Select @LinkPageId, lp2.[Id]
From LinkPages lp
Join Categories c On c.[Id] = lp.CategoryId --Get our category
Join Categories c2 On c2.[Id] = c.ParentCategoryId --Get our parent category
Join LinkPages lp2 On lp2.SiteId = lp.SiteId And lp2.CategoryId = c2.[Id] --Find the link pages that meet the criteria
Where lp.[Id] = @LinkPageId
And c2.[Id] Not In (Select [Id] From Categories_GetLinkPageRelatedCategories(@LinkPageId)) --Don't add an existing link

Print Cast(@@Rowcount As Varchar(10)) + ' Parent Category Relationships Were Added For Link Page #' + Cast(@LinkPageId As Varchar(10))

--Check if we have enough categories yet
If (Select Count(*) From Categories_GetLinkPageRelatedCategories(@LinkPageId)) >= @CategoryLimit
  Goto EnoughCategories

--Insert siblings
Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Select @LinkPageId, lp2.[Id]
From LinkPages lp
Join Categories c On c.[Id] = lp.CategoryId --Get our category
Join Categories c2 On c2.ParentCategoryId = c.ParentCategoryId --Get sibling categories
Join LinkPages lp2 On lp2.SiteId = lp.SiteId And lp2.CategoryId = c2.[Id] --Find the link pages that meet the criteria
Where lp.[Id] = @LinkPageId
And c2.[Id] <> @CategoryId --Don't add a link to ourself
And c2.[Id] Not In (Select [Id] From Categories_GetLinkPageRelatedCategories(@LinkPageId))

Print Cast(@@Rowcount As Varchar(10)) + ' Child Sibling Relationships Were Added For Link Page #' + Cast(@LinkPageId As Varchar(10))

--Check if we have enough categories yet
If (Select Count(*) From LinkPages_RelatedCategories Where LinkPageId = @LinkPageId) >= @CategoryLimit
  Goto EnoughCategories

--Insert categories that are in the relations table
Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Select @LinkPageId, lp2.[Id]
From LinkPages lp
Join CategoryRelationships cr On cr.FromCategoryId = lp.CategoryId
Join Categories c On c.[Id] = cr.ToCategoryId
Join LinkPages lp2 On lp2.SiteId = lp.SiteId And lp2.CategoryId = c.[Id] --Find the link pages that meet the criteria
Where lp.[Id] = @LinkPageId
And c.[Id] Not In (Select [Id] From Categories_GetLinkPageRelatedCategories(@LinkPageId))

Print Cast(@@Rowcount As Varchar(10)) + ' Specified Relationships Were Added For Link Page #' + Cast(@LinkPageId As Varchar(10))

EnoughCategories:

GO

GRANT EXECUTE ON dbo.Categories_UpdateLinkPageRelatedCategories TO [Public]
Go

/*

Categories_UpdateLinkPageRelatedCategories 37

select * from linkpages_relatedcategories
select * from linkpages
sp_columns categories
*/


