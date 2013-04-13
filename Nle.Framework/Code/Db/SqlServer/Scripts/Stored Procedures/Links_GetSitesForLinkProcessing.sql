if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetSitesForLinkProcessing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_GetSitesForLinkProcessing]
Go

CREATE PROCEDURE dbo.Links_GetSitesForLinkProcessing
AS

/*

Summary:
Returns a list of sites that should have a procesing cycle run
on them.  This basically includes all sites that are fully configured, and not
disabled.

*/

Select s.*, dbo.Sites_GetIntSiteSetting(s.[Id], 5) 'AddLinksPercentDays',
	Coalesce(s.PageTemplate, GlobalTemplate.[TextValue]) PageTemplate,
	dbo.Sites_GetIntSiteSetting(s.[Id], 1) 'MinLinksToAdd',
	dbo.Sites_GetIntSiteSetting(s.[Id], 2) 'MaxLinksToAdd'
From Sites s
Left Outer Join GlobalSettings GlobalTemplate On GlobalTemplate.[Id] = 2
Where dbo.Sites_IsSiteDisabled(s.Id) = 0
And dbo.Sites_IsSiteFullyConfigured(s.[Id]) = 1


Go

GRANT EXECUTE ON dbo.Links_GetSitesForLinkProcessing TO [Public]
Go

/*
select * from linkpages
select * from linkpages_articles

sp_columns linkpages

Links_GetSitesForLinkProcessing

*/

