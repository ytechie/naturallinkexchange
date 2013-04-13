if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetInitialLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetInitialLinkPage]
GO

CREATE PROCEDURE dbo.Links_GetInitialLinkPage
  @SiteId Int
AS

/*

Summary:
Retrieves the initial link page for a site.  If there is no initial
link page, a blank one is created.

*/

Declare @LinkPageId Int

Exec @LinkPageId = Links_GetInitialLinkPageId @SiteId

Select lp.*
From LinkPages lp
Where lp.[Id] = @LinkPageId

GO

GRANT EXECUTE ON dbo.Links_GetInitialLinkPage TO [Public]
Go


/*
Links_GetInitialLinkPage 48

Links_GetAllSiteLinks 1
Links_GetAllSiteLinks 1, 57

select * from sites
select * from linkpages
sp_columns LinkParagraphs
*/

