if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_AddUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_AddUser]
GO

CREATE PROCEDURE dbo.Users_AddUser
  @EmailAddress Varchar(100),
  @Password Varchar(50),
  @AccountType Int,
  @CreatedOn DateTime,
  @Name Varchar(50),
	@ReferrerId Int = 0,
	@LeadId Int = Null
AS

Insert Into Users
(EmailAddress, [Password], AccountType, Enabled, CreatedOn, [Name], ReferrerId, LeadId)
Values(@EmailAddress, @Password, @AccountType, 1, @CreatedOn, @Name, @ReferrerId, @LeadId)

Return @@Identity

GO

GRANT EXECUTE ON dbo.Users_AddUser TO [Public]
Go

/*
select * from users
sp_columns users
*/

