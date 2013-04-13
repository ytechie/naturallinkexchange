if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Payments_GetPayment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Payments_GetPayment]
GO

CREATE PROCEDURE dbo.Payments_GetPayment
  @PaymentId Int
AS

Select p.*
From Payments p
Where p.[Id] = @PaymentId

GO

GRANT EXECUTE ON dbo.Payments_GetPayment TO [Public]
Go

/*
select * from payments

Payments_GetPayment 5
*/

