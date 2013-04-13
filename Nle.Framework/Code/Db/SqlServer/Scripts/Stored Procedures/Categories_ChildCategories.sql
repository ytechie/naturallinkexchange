if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetChildCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_GetChildCategories]
GO

CREATE PROCEDURE dbo.Categories_GetChildCategories
  @ParentCategoryId Int = Null
AS

Select *
From Categories
Where (ParentCategoryId = @ParentCategoryId Or (@ParentCategoryId Is Null And ParentCategoryId Is Null))

GO
GRANT EXECUTE ON dbo.Categories_GetChildCategories TO [Public]


/*
Categories_GetChildCategories null
Categories_GetChildCategories 2

select * from Categories
sp_columns Categories
*/

