if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Administration_SendEmailToAllUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Administration_SendEmailToAllUsers]
GO

CREATE PROCEDURE dbo.Administration_SendEmailToAllUsers
  @Subject Nvarchar(100),
  @Message Text,
	@Filter NVarchar(100) = Null
AS

Declare @FromAddress Varchar(100)

Select @FromAddress = TextValue
From GlobalSettings
Where [Id] = 5

Declare @FilteredEmailUsers table(
	[UserId] Int )

If @Filter Is Not Null And @Filter != ''
	Insert Into @FilteredEmailUsers (UserId)
		Exec @Filter

Insert Into EMailQueue
	Select @FromAddress [From], u.[Name] [ToName], u.EmailAddress [ToAddress],
		@Subject [Subject], @Message [Message], 1 [Html], null [SentOn], GETUTCDATE() [QueuedOn],
		Null [LastTry], 0 [NumberOfTries], u.[Id] [UserId], 0 Bounced
	From Users u
	Where ((@Filter Is Null Or @Filter = '') Or [Id] In ( Select UserId From @FilteredEmailUsers )) And
		[Id] Not In ( Select UserId From UsersBouncedEmails Where BounceCount > 1 ) And
		u.Enabled = 1

GO

GRANT EXECUTE ON dbo.Administration_SendEmailToAllUsers TO [Public]
Go

/*

select * from emailqueue where userid = 11
select * from users
update emailqueue set bounced = 2 where id in ( 233, 274 )
delete from emailqueue
Administration_SendEmailToAllUsers 'Test Message', 'Hello to all.', 'EmailFilter_Administrators'

*/