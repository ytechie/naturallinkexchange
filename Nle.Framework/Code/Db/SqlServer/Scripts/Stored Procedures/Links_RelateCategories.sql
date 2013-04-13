if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_RelateCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_RelateCategories]
GO

CREATE PROCEDURE dbo.Links_RelateCategories
  @LinkPageId1 Int,
	@LinkPageId2 Int
AS

/*

Summary:
Creates a relationship between 2 link pages so that the generated
pages can be navigated.

- Note: Always make sure that a new link page has a reciprocal link with
				another link page, so that we can be sure that all link pages are
				accessible.

*/

Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Values(@LinkPageId1, @LinkPageId2)

--Insert the reciprocal relationship
Insert Into LinkPages_RelatedCategories
(LinkPageId, RelatedLinkPageId)
Values(@LinkPageId2, @LinkPageId1)

GO

GRANT EXECUTE ON dbo.Links_RelateCategories TO [Public]
Go


/*
select * from LinkPages_RelatedCategories

*/

