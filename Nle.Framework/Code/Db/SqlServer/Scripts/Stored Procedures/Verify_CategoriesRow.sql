if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Verify_CategoriesRow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Verify_CategoriesRow]
GO

CREATE PROCEDURE dbo.Verify_CategoriesRow
	@Id Int,
	@Name Varchar(50),
	@Description Varchar(500),
	@PageName Varchar(50),
	@ParentCategoryId int,
	@MetaKeywords Varchar(250),
	@MetaDescription Varchar(250),
	@Title Varchar(70)
	
AS

/*

Summary: Verifies the contents of the static data row in the Categories
	table and makes it match the values specified.

*/

If Exists(Select [Id] From Categories Where [Id] = @Id)
	Begin
		Update Categories
		Set [Name] = @Name,		
		[Description] = @Description,
		PageName = @PageName,
		ParentCategoryId = @ParentCategoryId,
		MetaKeywords = @MetaKeywords,
		MetaDescription = @MetaDescription,
		Title = @Title
		Where [Id] = @Id
	End
Else
	Begin
		Set Identity_Insert Categories On

		Insert Into Categories
		([Id], [Name], [Description], PageName, ParentCategoryId, MetaKeywords, MetaDescription, Title)
		Values(@Id, @Name, @Description, @PageName, @ParentCategoryId, @MetaKeywords, @MetaDescription, @Title)

		Set Identity_Insert Categories Off
	End

Go

GRANT EXECUTE ON dbo.Verify_CategoriesRow TO [Public]
Go

/*

sp_columns categories

*/