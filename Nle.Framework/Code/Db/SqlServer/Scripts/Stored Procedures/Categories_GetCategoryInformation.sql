if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetCategoryInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_GetCategoryInformation]
GO

CREATE PROCEDURE dbo.Categories_GetCategoryInformation
  @CategoryId Int = Null
AS

Select *
From Categories
Where @CategoryId Is Null Or [Id] = @CategoryId

GO
GRANT EXECUTE ON dbo.Categories_GetCategoryInformation TO [Public]


/*
Categories_GetCategoryInformation 2

select * from Sites
sp_columns sites
*/

