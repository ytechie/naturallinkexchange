if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_AddCategoryRelationship]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_AddCategoryRelationship]
GO

CREATE PROCEDURE dbo.Categories_AddCategoryRelationship
  @FromCategoryId Int,
  @ToCategoryId Int
AS

--Check if the relationship already exists.
If (Select Count(*) From CategoryRelationships Where FromCategoryId = @FromCategoryId And ToCategoryId = @ToCategoryId) > 0
  Return

Insert Into CategoryRelationships(FromCategoryId, ToCategoryId)
Values(@FromCategoryId, @ToCategoryId)

GO

GRANT EXECUTE ON dbo.Categories_AddCategoryRelationship TO [Public]
Go

/*
select * from CategoryRelationships
sp_columns CategoryRelationships
*/

