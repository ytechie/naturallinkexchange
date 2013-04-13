if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscriptions_GetTransaction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Subscriptions_GetTransaction]
GO

CREATE PROCEDURE dbo.Subscriptions_GetTransaction
	@SubscriptionId UniqueIdentifier
AS

Select *
From SubscriptionTransactions
Where GuidId = @SubscriptionId

GO

GRANT EXECUTE ON dbo.Subscriptions_GetTransaction TO [Public]
Go

/*
Subscriptions_GetTransaction 'ee54eb31-6250-4f3d-bad4-cec6a4634663'
*/

