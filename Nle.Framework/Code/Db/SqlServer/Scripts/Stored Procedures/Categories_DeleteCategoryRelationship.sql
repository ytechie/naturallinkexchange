if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_DeleteCategoryRelationship]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_DeleteCategoryRelationship]
GO

CREATE PROCEDURE dbo.Categories_DeleteCategoryRelationship
  @FromCategoryId Int,
  @ToCategoryId Int
AS

Delete From CategoryRelationships
Where FromCategoryId = @FromCategoryId
And ToCategoryId = @ToCategoryId

GO

GRANT EXECUTE ON dbo.Categories_DeleteCategoryRelationship TO [Public]
Go

/*
select * from CategoryRelationships
sp_columns CategoryRelationships
*/

