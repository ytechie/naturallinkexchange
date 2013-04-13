if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_GetExpiredFeeds]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Rss_GetExpiredFeeds]
GO

CREATE PROCEDURE dbo.Rss_GetExpiredFeeds
  
AS

Select *
From RssFeedItemCache ri
Join RssFeeds rf On ri.FeedId = rf.[Id]
Where DateAdd(s, rf.UpdateIntervalMinutes, ri.ReadTime) > GetUtcDate()

GO

GRANT EXECUTE ON dbo.Rss_GetExpiredFeeds TO [Public]
Go

/*
select * from rssfeeds
*/

