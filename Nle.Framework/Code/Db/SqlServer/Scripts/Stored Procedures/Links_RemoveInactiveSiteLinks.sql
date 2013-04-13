if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_RemoveInactiveSiteLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_RemoveInactiveSiteLinks]
GO

--Removes links for sites that no longer exist
CREATE PROCEDURE dbo.Links_RemoveInactiveSiteLinks
AS

--Todo

GO
GRANT EXECUTE ON dbo.Links_RemoveInactiveSiteLinks TO [Public]


/*
Links_RemoveInactiveSiteLinks

select * from LinkParagraphs
sp_columns LinkParagraphs
*/

