if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_SaveUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_SaveUser]
GO

CREATE PROCEDURE dbo.Users_SaveUser
	@UserId Int = Null,
  @EmailAddress Varchar(100),
  @Password Varchar(50),
  @AccountType Int,
  @Name Varchar(50),
	@CreatedOn DateTime = Null,
	@LastLogin DateTime = Null,
  @Enabled Bit,
	@ReferrerId Int = 0,
	@LeadId Int = Null
AS

If @UserId Is Null
  Begin
		If @CreatedOn Is Null
			Set @CreatedOn = GetUtcDate()

		Insert Into Users
		(EmailAddress, [Password], AccountType, Enabled, CreatedOn, [Name], LastLogin, ReferrerId, LeadId)
		Values(@EmailAddress, @Password, @AccountType, @Enabled, GetUtcDate(), @Name, @LastLogin, @ReferrerId, @LeadId)

		Return @@Identity
	End
Else
	Begin
		Update Users
		Set EmailAddress = @EmailAddress,
		[Password] = @Password,
		AccountType = @AccountType,
		Enabled = @Enabled,
		CreatedOn = @CreatedOn,
		[Name] = @Name,
		LastLogin = @LastLogin,
		ReferrerId = @ReferrerId,
		LeadId = @LeadId
		Where [Id] = @UserId

		Return @UserId
	End


GO

GRANT EXECUTE ON dbo.Users_SaveUser TO [Public]
Go

/*
select * from users
sp_columns users
*/

