if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetSiteLinkPages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_GetSiteLinkPages]
Go

CREATE PROCEDURE dbo.Links_GetSiteLinkPages
  @SiteId Int
AS

/*

Summary:
Retrieves all of the link pages for the specified site

*/

Select *
From LinkPages
Where SiteId = @SiteId

Go

GRANT EXECUTE ON dbo.Links_GetSiteLinkPages TO [Public]
Go

/*
Links_GetSiteLinkPages 1

*/

