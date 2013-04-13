if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Email_MarkLastMessageAsBounced]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Email_MarkLastMessageAsBounced]
GO

CREATE PROCEDURE dbo.[Email_MarkLastMessageAsBounced]
  @EmailAddress Varchar(255)
AS

/*

Summary: Marks the last email sent to the specified email address
as bounced.  That way when there are a certain number of bounces to an
address, emails are no longer sent to them.

*/

Declare @EmailId Int

--Get the last email sent to that address
Select Top 1 @EmailId = eq.[Id]
From EmailQueue eq
Where SentOn Is Not Null
And ToAddress = @EmailAddress
Order By SentOn Desc

If @EmailId Is Null
	Begin
		Print 'Couldn''t find an email that matched that address'
		Return
	End

Print 'Found a message that matched that address (ID ' + Cast(@EmailId As Varchar(10)) + ')'

Update EmailQueue
Set Bounced = 1
Where [Id] = @EmailId

GO

GRANT EXECUTE ON dbo.[Email_MarkLastMessageAsBounced] TO [Public]
Go


/*
select * from users
select * from emailqueue

Select *
From EmailQueue eq
Where SentOn Is Not Null
And ToAddress = 'superjason@new.rr.com'
Order By SentOn Desc

--[Email_MarkLastMessageAsBounced] 'superjason@new.rr.com'
*/

