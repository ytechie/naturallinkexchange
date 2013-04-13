if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Verify_SiteParameterRow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Verify_SiteParameterRow]
GO

CREATE PROCEDURE dbo.[Verify_SiteParameterRow]
	@Id Int,
	@Description Varchar(255),
	@DefaultInt Int,
	@DefaultText Text,
	@UserVisible Bit,
	@UserEditable Bit
AS

/*

Summary: Verifies the contents of the static data row in the SiteParameters
	table and makes it match the values specified.

*/

If Exists(Select [Id] From SiteParameters Where [Id] = @Id)
	Begin
		Update SiteParameters
		Set Description = @Description,
		DefaultInt = @DefaultInt,
		DefaultText = @DefaultText,
		UserVisible = @UserVisible,
		UserEditable = @UserEditable
		Where [Id] = @Id
	End
Else
	Begin
		Set Identity_Insert SiteParameters On

		Insert Into SiteParameters
		([Id], Description, DefaultInt, DefaultText, UserVisible, UserEditable)
		Values(@Id, @Description, @DefaultInt, @DefaultText, @UserVisible, @UserEditable)

		Set Identity_Insert SearchEngines Off
	End

Go

GRANT EXECUTE ON dbo.[Verify_SiteParameterRow] TO [Public]
Go

/*

sp_columns SiteParameters

*/