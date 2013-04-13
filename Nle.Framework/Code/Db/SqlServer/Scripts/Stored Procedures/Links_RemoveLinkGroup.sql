if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_RemoveLinkGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_RemoveLinkGroup]
GO

CREATE PROCEDURE dbo.Links_RemoveLinkGroup
  @LinkGroupId Int
AS

Update LinkParagraphs
Set MarkedForDeletion = 1
Where LinkParagraphGroupId = @LinkGroupId

Update LinkParagraphGroups
Set MarkedForDeletion = 1
Where Id = @LinkGroupId

GO
GRANT EXECUTE ON dbo.Links_RemoveLinkGroup TO [Public]