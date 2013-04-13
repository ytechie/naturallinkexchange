if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_GetUserInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_GetUserInfo]
GO

CREATE PROCEDURE dbo.Users_GetUserInfo
  @UserId Int
AS

Select u.*
From Users u
Where u.[Id] = @UserId

GO

GRANT EXECUTE ON dbo.Users_GetUserInfo TO [Public]
Go

/*
Users_GetUserInfo 4

select * from users
sp_columns users
*/

