if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPageStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetLinkPageStatus]
GO

CREATE PROCEDURE dbo.[Links_GetLinkPageStatus]
	@SiteId Int
AS

Select Top 1 *
From LinkPageStatuses
Where SiteId = @SiteId
Order By CheckedOn Desc

GO

GRANT EXECUTE ON dbo.[Links_GetLinkPageStatus] TO [Public]
Go

/*
exec Links_GetLinkPageStatus 1
*/

