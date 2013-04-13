if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetRelatedCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_GetRelatedCategories]
GO

--Retrieves the related categories from the 'CategoryRelationships' table
CREATE PROCEDURE dbo.Categories_GetRelatedCategories
  @CategoryId Int
AS

Select c.*
From Categories c
Join CategoryRelationships cr On cr.ToCategoryId = c.[Id]
Where cr.FromCategoryId = @CategoryId

GO

GRANT EXECUTE ON dbo.Categories_GetRelatedCategories TO [Public]
Go

/*
Categories_GetRelatedCategories 57
Categories_GetRelatedCategories 8

select * from categories
select * from categoryrelationships
sp_columns categories
*/

