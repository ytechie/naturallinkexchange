if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_GetUserByEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_GetUserByEmail]
GO

CREATE PROCEDURE dbo.[Users_GetUserByEmail]
  @EmailAddress Varchar(Max)
AS

Select *
From Users
Where EmailAddress = @EmailAddress

GO
GRANT EXECUTE ON dbo.[Users_GetUserByEmail] TO [Public]


/*
[Users_GetUserByEmail] 'SuperJason@new.rr.com'
*/

