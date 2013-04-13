if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscriptions_SaveTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Subscriptions_SaveTransaction]
GO

CREATE PROCEDURE dbo.Subscriptions_SaveTransaction
	@SubscriptionId UniqueIdentifier = Null Output,
	@SiteId Int,
	@PlanId Int,
	@Processed Bit,
	@PaymentAmount Float,
	@PaymentInterval Varchar(10)
AS

If @SubscriptionId Is Null
	Begin
		Set @SubscriptionId = NewId()

		Insert Into SubscriptionTransactions
		(GuidId, SiteId, PlanId, Processed, PaymentAmount, PaymentInterval)
		Values(@SubscriptionId, @SiteId, @PlanId, @Processed, @PaymentAmount, @PaymentInterval)
	End
Else
  Update SubscriptionTransactions
	Set SiteId = @SiteId,
	PlanId = @PlanId,
	Processed = @Processed,
	PaymentAmount = @PaymentAmount,
	PaymentInterval = @PaymentInterval
  Where GuidId = @SubscriptionId

GO

GRANT EXECUTE ON dbo.Subscriptions_SaveTransaction TO [Public]
Go

/*
delete from subscriptiontransactions
select * from subscriptions
select * from SubscriptionTransactions
sp_columns SubscriptionTransactions
*/

