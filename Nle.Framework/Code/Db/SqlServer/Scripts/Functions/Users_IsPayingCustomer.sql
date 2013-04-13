If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_IsPayingCustomer]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Users_IsPayingCustomer]
GO

create function dbo.[Users_IsPayingCustomer] (@UserId Int)
  Returns Bit
As

/*

Summary: Determines if the user has at least one site that has an upgraded subscription.

*/

Begin

Declare @PayingCount Int
Declare @HasPayingAccount Bit

Select @PayingCount = Count(s.Id)
From Sites s
Left Outer Join Subscriptions subs On subs.SiteId = s.Id
Where PlanId > 1
And s.UserId = @UserId

If(@PayingCount > 0)
	Set @HasPayingAccount = 1
Else
	Set @HasPayingAccount = 0

Return @HasPayingAccount

End

Go

/*
select * from subscriptions


select dbo.Users_IsPayingCustomer(1)
select dbo.Users_IsPayingCustomer(30)
select * from subscriptions where planid > 1
select * from sites where id = 73
*/