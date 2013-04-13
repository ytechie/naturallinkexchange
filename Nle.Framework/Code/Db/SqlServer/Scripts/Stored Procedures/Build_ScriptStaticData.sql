if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Build_ScriptStaticData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Build_ScriptStaticData]
GO

CREATE PROCEDURE dbo.Build_ScriptStaticData
AS

/*

Summary: Scripts out the static data required for the database
	to operate properly.

*/


GO

GRANT EXECUTE ON dbo.Build_ScriptStaticData TO [Public]
Go

/*


*/