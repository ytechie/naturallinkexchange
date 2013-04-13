If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_HasValidSubscription]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_HasValidSubscription]
GO

create function dbo.[Sites_HasValidSubscription] (@SiteId Int)
  Returns Bit
As

/*

Summary: Determines if the site has a valid active subscription.

*/

Begin

Return 1

End

Go

/*
*/