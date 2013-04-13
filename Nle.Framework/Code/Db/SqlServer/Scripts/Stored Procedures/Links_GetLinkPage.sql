if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_GetLinkPage]
Go

CREATE PROCEDURE dbo.Links_GetLinkPage
  @LinkPageId Int
AS

/*

Summary:
Retrieves the link page with the specified ID>

*/

Select *
From LinkPages
Where [Id] = @LinkPageId

Go

GRANT EXECUTE ON dbo.Links_GetLinkPage TO [Public]
Go

/*
Links_GetLinkPage 25

*/

