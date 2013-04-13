if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetDeletedLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetDeletedLinks]
GO

CREATE PROCEDURE dbo.Links_GetDeletedLinks
  @LinkCount Int,
  @LinkPageId Int
AS

Set RowCount @LinkCount

Select *
From LinkParagraphs lp
Join LinkPages_Articles a On lp.[Id] = a.ArticleId
Where MarkedForDeletion = 1
And a.LinkPageId = @LinkPageId
Order By NewId() --Randomly select the links to delete

Set RowCount 0

GO

GRANT EXECUTE ON dbo.Links_GetDeletedLinks TO [Public]
Go


/*
Links_GetDeletedLinks 0

select * from categories
select * from SiteParagraphMappings
select * from LinkParagraphs
sp_columns LinkParagraphs
*/

