If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_HasLinkPageSetup]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_HasLinkPageSetup]
GO

create function dbo.[Sites_HasLinkPageSetup] (@SiteId Int)
  Returns Bit
As

/*

Summary: Determines if the site is ready to publish link pages.

*/

Begin

Declare @Configured Bit

If Exists
		(Select SiteId
		From SiteSettings
		Where SiteSettings.SiteId = @SiteId
			And SiteParameterId = 6 And IntValue <> 0 And IntValue Is Not Null)
	Set @Configured = 1
Else
	Set @Configured = 0

Return @Configured

End

Go

/*
select * from sitesettings
*/