if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_UpdateLinkPageFeeds]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Rss_UpdateLinkPageFeeds]
GO

CREATE PROCEDURE dbo.Rss_UpdateLinkPageFeeds
  @LinkPageId Int
AS

/*

Summary:
Randomly assigns RSS feed items from the feed cache to the specified
link page.  The feed items are assigned by category.

*/

Declare @LastRowCount Int

Declare @ItemCount Int
Set @ItemCount = 5 --Choose this many items for the link page

--Look up the category ID
Declare @LinkPageCategoryId Int
Select @LinkPageCategoryId = CategoryId
From LinkPages
Where [Id] = @LinkPageId

Print 'Page category Id is ' + Cast(@LinkPageCategoryId As Varchar(10))

--Create a table of the available feed items, and add a random
--column so that we can randomly choose n number of items.
Select [Id], 0.00000000000 'RandomValue'
Into #FeedItems
From RssFeedItemCache ric
Join CategoryFeedMappings cm On RssFeedId = ric.FeedId
Where cm.CategoryId = @LinkPageCategoryId
And ric.[Delete] = 0 --Only use active items

Declare @AvailableCount Int
Select @AvailableCount = Count(*)
From #FeedItems

Print Cast(@AvailableCount As Varchar(10)) + ' feed items to choose from'

Declare @CurrId Int
Declare ItemCursor Cursor For
Select [Id] From #FeedItems

Open ItemCursor
Fetch Next From ItemCursor Into @CurrId

--Loop through each item and assign a random number
While @@Fetch_Status = 0
  Begin
    Update #FeedItems
    Set RandomValue = Rand()
    Where [Id] = @CurrId

    Fetch Next From ItemCursor Into @CurrId
  End

--We'll use a transaction for this so that we are
--not left without RSS items if we have a problem getting
--new ones.
Begin Transaction

Delete From LinkPages_RssItems
Where LinkPageId = @LinkPageId

If @@Error <> 0
  Goto Failure

Print 'Successfully Deleted Old RSS Items'

Set Rowcount @ItemCount

Insert Into LinkPages_RssItems
(LinkPageId, RssItemId)
Select @LinkPageId, [Id]
From #FeedItems
Order By RandomValue

If @@Error <> 0
  Goto Failure

Print 'Successfully Inserted New RSS Item Mappings'

Commit Transaction
Goto Finalize

Failure:
Rollback Transaction
Print 'Transaction Was Rolled Back Because Of An Error'

Finalize:

--Reset the rowcount
Set Rowcount 0

--We're done with this table
Drop Table #FeedItems

GO

GRANT EXECUTE ON dbo.Rss_UpdateLinkPageFeeds TO [Public]
Go


/*

Select ri.*, ic.itemtext From LinkPages_RssItems ri
join RssFeedItemCache ic On ic.[Id] = ri.RssItemId

select * from rssfeeditemcache
select * from LinkPages_RssItems

Rss_UpdateLinkPageFeeds 121
Rss_UpdateLinkPageFeeds 37


*/

