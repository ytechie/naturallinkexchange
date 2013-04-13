If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_IsSiteFullyConfigured]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_IsSiteFullyConfigured]
GO

create function dbo.[Sites_IsSiteFullyConfigured] (@SiteId Int)
  Returns Bit
As

/*

Summary:

*/

Begin

Declare @Configured Bit

If dbo.Sites_HasValidSubscription(@SiteId) = 1
  And dbo.Sites_HasPublishedArticles(@SiteId) = 1
  And dbo.Sites_HasLinkPageSetup(@SiteId) = 1
	Set @Configured = 1
Else
	Set @Configured = 0

Return @Configured

End

Go

/*

Select dbo.[Sites_IsSiteFullyConfigured](1)

Select dbo.Sites_HasValidSubscription(1)
Select dbo.Sites_HasPublishedArticles(1)
Select dbo.Sites_HasLinkPageSetup(1)

Select Name, dbo.[Sites_IsSiteFullyConfigured](s.Id)
From Sites s

			Select Top 1 SiteId
			From Subscriptions
			Where Subscriptions.SiteId = 1
				And GetUtcDate() Between StartTime And EndTime

select count(*) from sites where dbo.Sites_HasLinkPageSetup(id) = 1

select * from sitesettings
select * from siteparameters
select * from subscriptions
*/