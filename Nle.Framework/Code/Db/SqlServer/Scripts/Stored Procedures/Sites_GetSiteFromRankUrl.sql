if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetSiteFromRankUrl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetSiteFromRankUrl]
GO

CREATE PROCEDURE dbo.Sites_GetSiteFromRankUrl
  @SiteRankUrlId Int
AS

Select SiteId
From SiteUrls
Where [Id] = @SiteRankUrlId

GO
GRANT EXECUTE ON dbo.Sites_GetSiteFromRankUrl TO [Public]
Go

/*
Sites_GetSiteFromRankUrl 2

select * from SiteRankUrls
*/

