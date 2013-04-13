if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetIncomingLinkCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Sites_GetIncomingLinkCount]
Go

CREATE PROCEDURE dbo.[Sites_GetIncomingLinkCount]
  @SiteId Int --the site to link to
AS

/*

Summary: Gets a count of all the sites that link to the specified site.

*/

Select Count(*)
From dbo.[Links_GetIncomingLinkedPartners](@SiteId)

Go

GRANT EXECUTE ON dbo.[Sites_GetIncomingLinkCount] TO [Public]
Go

/*
Sites_GetIncomingLinkCount 1
*/

