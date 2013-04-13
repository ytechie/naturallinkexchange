if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetSitesForLinkPageCheck]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetSitesForLinkPageCheck]
GO

CREATE PROCEDURE dbo.[Sites_GetSitesForLinkPageCheck]
AS

Select s.*, su.Url, lps.*--, lps.Valid
From Sites s
Left Outer Join SiteUrls su On s.RootUrlId = su.[Id]
Left Outer Join LinkPageStatuses lps On lps.Id = dbo.Sites_GetLastLinkPageCheck(s.Id)
Where (CheckedOn Is Null Or CheckedOn < DateAdd(d, -1, CheckedOn))
And dbo.Sites_IsSiteFullyConfigured(s.Id) = 1

GO
GRANT EXECUTE ON dbo.[Sites_GetSitesForLinkPageCheck] TO [Public]


/*
Sites_GetSitesForLinkPageCheck

select * from Sites
sp_columns sites

select * from siteparameters

select * from linkpagestatuses

Select SiteId, Valid
From LinkPageStatuses
having SiteId = Max(SiteId)

*/

