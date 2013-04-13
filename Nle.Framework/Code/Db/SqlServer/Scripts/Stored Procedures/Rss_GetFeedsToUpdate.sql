if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_GetFeedsToUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Rss_GetFeedsToUpdate]
GO

CREATE PROCEDURE dbo.Rss_GetFeedsToUpdate
  
AS

--Check for new items for all feeds, just order them by priority

Select *
From RssFeeds rf
Order By LastUpdate Desc

--Where (Select Count(*)
--       From RssFeedItemCache ri
--       Where ri.FeedId = rf.[Id]) = 0


GO

GRANT EXECUTE ON dbo.Rss_GetFeedsToUpdate TO [Public]
Go

/*
select * from rssfeeds
sp_columns rssfeeds

Rss_GetFeedsToUpdate
*/

