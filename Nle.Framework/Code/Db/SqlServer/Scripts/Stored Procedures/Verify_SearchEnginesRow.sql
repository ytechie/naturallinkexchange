if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Verify_SearchEnginesRow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Verify_SearchEnginesRow]
GO

CREATE PROCEDURE dbo.Verify_SearchEnginesRow
	@Id Int,
	@Name Varchar(50),
	@Url Varchar(50),
	@PopularityKey int,
	@TopDogName Varchar(50)
AS

/*

Summary: Verifies the contents of the static data row in the SearchEngines
	table and makes it match the values specified.

*/

If Exists(Select [Id] From SearchEngines Where [Id] = @Id)
	Begin
		Update SearchEngines
		Set [Name] = @Name,
		Url = @Url,
		PopularityKey = @PopularityKey,
		TopDogName = @TopDogName
		Where [Id] = @Id
	End
Else
	Begin
		Set Identity_Insert SearchEngines On

		Insert Into SearchEngines
		([Id], [Name], Url, PopularityKey, TopDogName)
		Values(@Id, @Name, @Url, @PopularityKey, @TopDogName)

		Set Identity_Insert SearchEngines Off
	End

Go

GRANT EXECUTE ON dbo.Verify_SearchEnginesRow TO [Public]
Go

/*

sp_columns searchengines

*/