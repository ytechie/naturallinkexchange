if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_FillFeedsWithYahoo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Rss_FillFeedsWithYahoo]
GO

CREATE PROCEDURE dbo.Rss_FillFeedsWithYahoo  
AS

/*

Summary:
Loops through all of the categories that do not have RSS feeds, and adds a default
feed of Yahoo! news to them.

*/

Declare categoryCursor Cursor For
Select [Id], [Name]
From Categories c
Left Outer Join CategoryFeedMappings cfm On c.[Id] = cfm.CategoryId
Where cfm.CategoryId Is Null

OPEN categoryCursor

Declare @CurrCategoryId Int
Declare @CurrCategoryName Varchar(100)

-- Perform the first fetch.
FETCH NEXT FROM categoryCursor Into @CurrCategoryId, @CurrCategoryName

-- Check @@FETCH_STATUS to see if there are any more rows to fetch.
While @@FETCH_STATUS = 0
Begin
  Insert Into RssFeeds
  (RssUrl)
  Values('http://news.search.yahoo.com/news/rss?p=' + @CurrCategoryName + '&ei=UTF-8&fl=0&x=wrt')

  Insert Into CategoryFeedMappings
  (CategoryId, RssFeedId)
  Values(@CurrCategoryId, @@Identity)

   -- This is executed as long as the previous fetch succeeds.
   FETCH NEXT FROM categoryCursor Into @CurrCategoryId, @CurrCategoryName
End

CLOSE categoryCursor
DEALLOCATE categoryCursor

GO

GRANT EXECUTE ON dbo.Rss_FillFeedsWithYahoo TO [Public]
Go

/*
select * from rssfeeds
sp_columns rssfeeds

select * from rssfeedItemcache

select * from CategoryFeedMappings

Rss_FillFeedsWithYahoo
*/

