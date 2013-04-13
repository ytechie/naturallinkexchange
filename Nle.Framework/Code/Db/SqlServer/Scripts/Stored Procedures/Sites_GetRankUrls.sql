if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetRankUrls]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetRankUrls]
GO

CREATE PROCEDURE dbo.Sites_GetRankUrls
  @SiteId Int = Null
AS

Select *
From SiteUrls ru
Where (@SiteId Is Null Or ru.SiteId = @SiteId)

GO
GRANT EXECUTE ON dbo.Sites_GetRankUrls TO [Public]


/*
Sites_GetRankUrls
Sites_GetRankUrls 1

select * from Sites
sp_columns sites
*/

