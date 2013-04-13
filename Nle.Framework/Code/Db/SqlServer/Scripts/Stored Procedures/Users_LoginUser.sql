if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_LoginUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_LoginUser]
GO

CREATE PROCEDURE dbo.Users_LoginUser
  @EmailAddress Varchar(100),
  @Password Varchar(50)
AS

Select *
From Users
Where EmailAddress = @EmailAddress
And [Password] = @Password

GO
GRANT EXECUTE ON dbo.Users_LoginUser TO [Public]


/*
Users_LoginUser 'superjason@new.rr.com', 'xxx'
Users_LoginUser 'superjason@new.rr.com', 'asdfasd'
Users_LoginUser 'SuperJason@new.rr.com', ''
select * from users
sp_columns users

update users
set password = 'xxx'
where id = 4
*/

