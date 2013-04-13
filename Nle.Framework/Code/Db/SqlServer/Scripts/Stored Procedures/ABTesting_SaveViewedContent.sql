if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ABTesting_SaveViewedContent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ABTesting_SaveViewedContent]
GO

CREATE PROCEDURE dbo.[ABTesting_SaveViewedContent]
  @ContentId Int = Null,
	@UserId Int,
	@RotatorKey NVarchar(Max),
	@ContentKey NVarchar(Max),
	@Timestamp DateTime,
	@Action Int
AS

If @ContentId Is Null
	Begin
		Insert Into ABContentShown
		(UserId, RotatorKey, ContentKey, [Timestamp], ActionId)
		Values(@UserId, @RotatorKey, @ContentKey, @Timestamp, @Action)

		Set @ContentId = @@Identity

		Return @ContentId
	End
Else
	Begin
		Update ABContentShown
		Set UserId = @UserId,
		RotatorKey = @RotatorKey,
		ContentKey = @ContentKey,
		[Timestamp] = @Timestamp,
		ActionId = @Action
		Where [Id] = @ContentId

		Return @ContentId
	End

GO

GRANT EXECUTE ON dbo.[ABTesting_SaveViewedContent] TO [Public]
Go

/*
select * from ABContentShown
*/

