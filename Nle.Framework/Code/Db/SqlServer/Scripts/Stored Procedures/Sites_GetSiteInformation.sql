if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetSiteInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetSiteInformation]
GO

CREATE PROCEDURE dbo.Sites_GetSiteInformation
  @SiteId Int = Null
AS

Select s.[Name], su.Url, s.Enabled, s.UserId,
	s.InitialCategoryId, s.SiteGuid,
	Coalesce(s.PageTemplate, GlobalTemplate.[TextValue]) PageTemplate,
	dbo.Sites_GetIntSiteSetting(@SiteId, 5) 'AddLinksPercentDays',
	dbo.Sites_GetIntSiteSetting(@SiteId, 1) 'MinLinksToAdd',
	dbo.Sites_GetIntSiteSetting(@SiteId, 2) 'MaxLinksToAdd',
	StartLinkPageId, LastManualCheck, UpgradeFlag, LinkPageUrl,
	HideInitialSetupMessage
From Sites s
Left Outer Join SiteUrls su On s.RootUrlId = su.[Id]
Left Outer Join GlobalSettings GlobalTemplate On GlobalTemplate.[Id] = 2
Where @SiteId Is Null Or s.[Id] = @SiteId

GO
GRANT EXECUTE ON dbo.Sites_GetSiteInformation TO [Public]


/*
Sites_GetSiteInformation 1

select * from Sites
sp_columns sites

select * from siteparameters
*/

