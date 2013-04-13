if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_DeleteSite]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_SaveSite]
GO

CREATE PROCEDURE dbo.Sites_DeleteSite
	@SiteId Int = Null
AS

Update Sites
Set
	MarkedForDeletion = 1
Where Id = @SiteId

GO

GRANT EXECUTE ON dbo.Sites_DeleteSite TO [Public]
Go

/*
sp_columns sites
*/

