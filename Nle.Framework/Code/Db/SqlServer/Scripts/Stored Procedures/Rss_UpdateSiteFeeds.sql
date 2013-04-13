if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_UpdateSiteFeeds]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Rss_UpdateSiteFeeds]
GO

CREATE PROCEDURE dbo.Rss_UpdateSiteFeeds
  @SiteId Int
AS

/*

Summary:
Loops through all of the link pages for a site and updates their RSS feeds.

*/

Declare @CurrId Int
Declare LinkPageCursor Cursor For
Select [Id] From LinkPages Where SiteId = @SiteId

Open LinkPageCursor
Fetch Next From LinkPageCursor Into @CurrId

While @@Fetch_Status = 0
  Begin
    Exec Rss_UpdateLinkPageFeeds @CurrId
    Print 'Updated Link Page #' + Cast(@CurrId As Varchar(10))
    Fetch Next From LinkPageCursor Into @CurrId
  End

Close LinkPageCursor
Deallocate LinkPageCursor

GO

GRANT EXECUTE ON dbo.Rss_UpdateSiteFeeds TO [Public]
Go


/*
Rss_UpdateSiteFeeds 46
Rss_UpdateSiteFeeds 37

select * from linkpages
select * from linkpages_rssItems

*/

