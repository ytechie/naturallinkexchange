if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetAllRelatedCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_GetAllRelatedCategories]
GO

CREATE PROCEDURE dbo.Categories_GetAllRelatedCategories
  @CategoryId Int,
  @CategoryLimit Int = 10
AS

Declare @RelatedCategories Table
(
  [Id] Int
)

--Insert child categories
Insert Into @RelatedCategories
Select c.[Id]
From Categories c
Where c.ParentCategoryId = @CategoryId
And c.[Id] <> @CategoryId

--Insert parent category
Insert Into @RelatedCategories
Select c2.[Id]
From Categories c
Join Categories c2 On c.ParentCategoryId = c2.[Id]
Where c.[Id] = @CategoryId

--Check if we have enough categories yet
If (Select Count(*) From @RelatedCategories) >= @CategoryLimit
  Goto EnoughCategories

--Insert categories with the same parent
Insert Into @RelatedCategories
Select c.[Id]
From Categories c
Where c.ParentCategoryId = (Select ParentCategoryId From Categories Where [Id] = @CategoryId)
And c.[Id] <> @CategoryId

--Check if we have enough categories yet
If (Select Count(*) From @RelatedCategories) >= @CategoryLimit
  Goto EnoughCategories

--Insert categories that are in the relations table
Insert Into @RelatedCategories
Select c.[Id]
From Categories c
Join CategoryRelationships cr On cr.ToCategoryId = c.[Id]
Where c.[Id] Not In (Select [Id] From @RelatedCategories)
And cr.FromCategoryId = @CategoryId
And c.[Id] <> @CategoryId

EnoughCategories:

Select *
From @RelatedCategories

GO

GRANT EXECUTE ON dbo.Categories_GetAllRelatedCategories TO [Public]
Go

/*
Categories_GetAllRelatedCategories 57, 1
Categories_GetAllRelatedCategories 57, 3
Categories_GetAllRelatedCategories 57, 10
Categories_GetAllRelatedCategories 8
Categories_GetAllRelatedCategories 4

select * from categories
select * from categoryrelationships
sp_columns categories

insert into #relatedcategories
Select * from categories
*/


