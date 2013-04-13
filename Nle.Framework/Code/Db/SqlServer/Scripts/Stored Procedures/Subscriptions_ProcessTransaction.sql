if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscriptions_ProcessTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Subscriptions_ProcessTransaction]
GO

CREATE PROCEDURE dbo.Subscriptions_ProcessTransaction
	@SubscriptionId UniqueIdentifier
AS

/*

Summary: Processes a subscription transaction, and applies it to the subscriptions table.  It is transaction
	based, so that the tables are not left in a broken state.

*/

Declare @SiteId Int
Declare @PlanId Int
Declare @Processed Bit

Begin Try
	Begin Transaction

	Print 'Looking up subscription transaction information'
	Select @SiteId = st.SiteId, @PlanId = st.PlanId, @Processed = st.Processed
	From SubscriptionTransactions st
	Where st.GuidId = @SubscriptionId
	
	--Make sure this transaction needs processed
	If @Processed = 1
		Begin
			Print 'The transaction has already been processed, aborting'
			Rollback Transaction
			Return
		End
	
	Print 'Looking for an existing subscription for site ' + Cast(@SiteId As Varchar(10))
	If Exists(Select * From Subscriptions Where SiteId = @SiteId)
		Begin
			Print 'Updating the current subscription for the user'

			Update Subscriptions
			Set PlanId = @PlanId,
			EndTime = DateAdd(m, 1, GetUtcDate())
			Where SiteId = @SiteId
		End
	Else
		Begin
			Print 'Creating a new subscription row for the user, why didn''t they have a free subscription?'

			Insert Into Subscriptions
			(SiteId, PlanId, StartTime, EndTime)
			Values(@SiteId, @PlanId, GetUtcDate(), DateAdd(m, 1, GetUtcDate()))
		End

	Print 'Marking the transaction as being processed'
	Update SubscriptionTransactions
	Set Processed = 1
	Where GuidId = @SubscriptionId	

	Commit Transaction
End Try
Begin Catch
	Print 'There was a failure that was caught, the transaction will be rolled back'

	IF XACT_STATE() <> 0
		Rollback Transaction

	Declare @ErrorMsg Varchar(1000)
	Declare @ErrorSeverity Int
	Declare @ErrorState Int

	Set @ErrorMsg = Error_Message()
	Set @ErrorSeverity = Error_severity()
	Set @ErrorState = Error_State()

	RAISERROR(@ErrorMsg, @ErrorSeverity, @ErrorState)
End Catch

GO

GRANT EXECUTE ON dbo.Subscriptions_ProcessTransaction TO [Public]
Go

/*
Subscriptions_ProcessTransaction 'f4e6f4f1-f0c0-4a2c-b67e-1969b70e62f9'

select * from subscriptiontransactions

select * from subscriptions


select xact_state()
*/

