if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_GetItemsForLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Rss_GetItemsForLinkPage]
GO

CREATE PROCEDURE dbo.Rss_GetItemsForLinkPage
  @LinkPageId Int  
AS

/*

Summary:
Retrieves the RSS items that are linked to the specified link page.

*/

Select ic.*
From LinkPages_RssItems ri
Join RssFeedItemCache ic On ic.[Id] = ri.RssItemId
Where ri.LinkPageId = @LinkPageId


GO

GRANT EXECUTE ON dbo.Rss_GetItemsForLinkPage TO [Public]
Go

/*
select * from rssfeeds
sp_columns rssfeeds

sp_columns rssfeeditemcache

Rss_GetItemsForLinkPage 25
*/

