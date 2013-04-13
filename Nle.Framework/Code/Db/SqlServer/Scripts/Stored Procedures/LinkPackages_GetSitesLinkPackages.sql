if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LinkPackages_GetSitesLinkPackages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LinkPackages_GetSitesLinkPackages]
GO

CREATE PROCEDURE dbo.LinkPackages_GetSitesLinkPackages
	@SiteId Int
AS

/*

Summary: Retrieves the active link package for the specified site id.

*/

Select Top 1 *
From Subscriptions
Where SiteId = @SiteId
And ((GetUtcDate() > StartTime And EndTime Is Null) Or GetUtcDate() Between StartTime And EndTime)

GO

GRANT EXECUTE ON dbo.LinkPackages_GetSitesLinkPackages TO [Public]
Go

/*
LinkPackages_GetSitesLinkPackages 1

sp_help subscriptions
select * from sitedatasamples

Select * From Subscriptions


*/


