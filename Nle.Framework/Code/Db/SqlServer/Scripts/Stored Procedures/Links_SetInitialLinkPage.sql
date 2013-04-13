if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_SetInitialLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_SetInitialLinkPage]
GO

CREATE PROCEDURE dbo.[Links_SetInitialLinkPage]
  @SiteId Int,
  @LinkPageId Int
AS

Update Sites
Set StartLinkPageId = @LinkPageId
Where [Id] = @Siteid

GO

GRANT EXECUTE ON dbo.[Links_SetInitialLinkPage] TO [Public]
Go


/*
select * from sites
select * from linkpages
*/

