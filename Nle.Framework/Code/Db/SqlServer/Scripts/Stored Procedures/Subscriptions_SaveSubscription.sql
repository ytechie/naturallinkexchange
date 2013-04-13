if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscriptions_SaveSubscription]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Subscriptions_SaveSubscription]
GO

CREATE PROCEDURE dbo.Subscriptions_SaveSubscription
	@SubscriptionId Int = Null,
	@SiteId Int,
	@PlanId Int,
	@StartTime DateTime = Null,
	@EndTime DateTime = Null
AS

If @StartTime Is Null
	Set @StartTime = GetUtcDate()

If @SubscriptionId Is Null
	Begin
		Insert Into Subscriptions
		(SiteId, PlanId, StartTime, EndTime)
		Values(@SiteId, @PlanId, @StartTime, @EndTime)
	
		Return @@Identity
	End
Else
	Begin
	  Update Subscriptions
	  Set SiteId = @SiteId,
		PlanId = @PlanId,
		StartTime = @StartTime,
		EndTime = @EndTime
	  Where [Id] = @SubscriptionId

		Return @SubscriptionId
	End



GO

GRANT EXECUTE ON dbo.Subscriptions_SaveSubscription TO [Public]
Go

/*
select * from subscriptions
sp_columns subscriptions
*/

