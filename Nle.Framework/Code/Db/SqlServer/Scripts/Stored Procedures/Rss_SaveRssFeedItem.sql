if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_SaveRssFeedItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Rss_SaveRssFeedItem]
GO

CREATE PROCEDURE dbo.Rss_SaveRssFeedItem
  @FeedId Int,
  @ItemTitle Varchar(100),
  @ItemText Text,
  @ReadTime DateTime,
  @Revision Int = Null Output
AS

--Assign a new revision number if we didn't get one
If @Revision Is Null
  Select @Revision = Max(Revision) + 1
  From RssFeedItemCache
  Where FeedId = @FeedId

--If the revision is still null, assign it #1
If @Revision Is Null
  Set @Revision = 1

--Add the new feed item
Insert Into RssFeedItemCache
(ItemTitle, ItemText, ReadTime, FeedId, Revision)
Values(@ItemTitle, @ItemText, @ReadTime, @FeedId, @Revision)

--Mark old items for eventual deletion
Update RssFeedItemCache
Set [Delete] = 1
Where Revision <> @Revision
And FeedId = @FeedId

--Clean up the old revisions
Delete From RssFeedItemCache
Where [Delete] = 1
And [Id] Not In (Select RssItemId From LinkPages_RssItems)

--Save the time of this update
Update RssFeeds
Set LastUpdate = @ReadTime
Where [Id] = @FeedId

GO

GRANT EXECUTE ON dbo.Rss_SaveRssFeedItem TO [Public]
Go

/*
select * from rssfeeditemcache
sp_columns rssfeeditemcache

select * from rssfeeds

Rss_GetFeedsToUpdate
*/

