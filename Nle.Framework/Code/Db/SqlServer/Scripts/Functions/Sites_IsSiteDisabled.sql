If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_IsSiteDisabled]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_IsSiteDisabled]
GO

create function dbo.[Sites_IsSiteDisabled] (@SiteId Int)
  Returns Bit
As

/*

Summary:
Determines if the specified site is disabled, or if the users whose site it belongs
to is disabled.  If so, 1 is returned.

*/

Begin

Declare @EnabledCount Int
Declare @Disabled Bit

Select @EnabledCount = (Coalesce(s.Enabled, 0) + Coalesce(u.Enabled, 0))
From Sites s
Left Outer Join Users u On s.UserId = u.Id
Where s.Id = @SiteId

If @EnabledCount = 2
	Set @Disabled = 0 --The site is not disabled
Else
	Set @Disabled = 1 --The site IS disabled

Return @Disabled

End
Go

/*
--0
Select dbo.[Sites_IsSiteDisabled](1) 
--1
Select dbo.[Sites_IsSiteDisabled](559) 
--1
Select dbo.[Sites_IsSiteDisabled](595) 

*/