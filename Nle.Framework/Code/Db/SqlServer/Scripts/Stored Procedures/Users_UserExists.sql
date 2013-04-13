if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_UserExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_UserExists]
GO

CREATE PROCEDURE dbo.Users_UserExists
  @EmailAddress VARCHAR(100)
AS

If Exists(Select * From Users Where EmailAddress = @EmailAddress)
	Select 1
Else
	Select 0

GO
GRANT EXECUTE ON dbo.Users_UserExists TO [Public]


/*
Declare @Return Int
Exec @Return = Users_UserExists 'xxx@tds.net'
Print @Return
*/

