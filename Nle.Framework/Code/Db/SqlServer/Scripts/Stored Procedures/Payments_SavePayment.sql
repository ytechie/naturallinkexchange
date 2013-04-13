if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Payments_SavePayment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Payments_SavePayment]
GO

CREATE PROCEDURE dbo.Payments_SavePayment
	@PaymentId Int = Null,
	@Amount Float,
	@Applied Bit,
	@PostData Text,
	@SubscriptionTransactionId UniqueIdentifier,
	@PayPal_SubscriptionId Varchar(50),
	@PayPal_VerifySign Varchar(50),
	@PayPal_TransactionId Varchar(50),
	@PayPal_PayerId Varchar(50),
	@PayPal_Fee Float,
	@PayPal_PayerEmail Varchar(50)
AS


If @PaymentId Is Null
	Begin
		Insert Into Payments
		(Amount, Applied, PostData, SubscriptionTransactionId, PayPal_SubscriptionId,
			PayPal_VerifySign, PayPal_TransactionId,
			PayPal_PayerId, PayPal_Fee, PayPal_PayerEmail)
		Values(@Amount, @Applied, @PostData, @SubscriptionTransactionId, 
			@PayPal_SubscriptionId, @PayPal_VerifySign, @PayPal_TransactionId,
			@PayPal_PayerId, @PayPal_Fee, @PayPal_PayerEmail)

		Return @@Identity
	End
Else
	Begin
	  Update Payments
	  Set Amount = @Amount,
		Applied = @Applied,
		PostData = @PostData,
		SubscriptionTransactionId = @SubscriptionTransactionId,
		PayPal_SubscriptionId = @PayPal_SubscriptionId,
		PayPal_VerifySign = @PayPal_VerifySign,
		PayPal_TransactionId = @PayPal_TransactionId,
		PayPal_PayerId = @PayPal_PayerId,
		PayPal_Fee = @PayPal_Fee,
		PayPal_PayerEmail = @PayPal_PayerEmail
	  Where [Id] = @PaymentId

		Return @PaymentId
	End

GO

GRANT EXECUTE ON dbo.Payments_SavePayment TO [Public]
Go

/*
select * from payments
sp_columns payments
*/

