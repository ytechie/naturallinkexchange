if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Rss_GetRandomCategoryItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Rss_GetRandomCategoryItems]
GO

CREATE Procedure dbo.Rss_GetRandomCategoryItems
(
  @CategoryId Int,
  @ItemCount Int
)
AS

Select *, 0.00000000000 'RandomValue'
Into #FeedItems
From RssFeedItemCache ric
Join CategoryFeedMappings cm On RssFeedId = ric.FeedId
Where cm.CategoryId = @CategoryId

Declare @CurrId Int
Declare ItemCursor Cursor For
Select [Id] From #FeedItems

Open ItemCursor
Fetch Next From ItemCursor Into @CurrId

While @@Fetch_Status = 0
  Begin
    Update #FeedItems
    Set RandomValue = Rand()
    Where [Id] = @CurrId

    Fetch Next From ItemCursor Into @CurrId
  End

Set Rowcount @ItemCount

Select *
From #FeedItems
Order By RandomValue

Set Rowcount 0

Drop Table #FeedItems

GO

GRANT EXECUTE ON dbo.Rss_GetRandomCategoryItems TO [Public]
Go


/*

select * from rssfeeds
select * from CategoryFeedMappings
select Rand()

Rss_GetRandomCategoryItems 72, 5

*/

