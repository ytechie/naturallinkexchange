if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_DeleteLinkInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_DeleteLinkInformation]
GO

CREATE PROCEDURE dbo.Links_DeleteLinkInformation
  @LinkId Int
AS

Update LinkParagraphs
Set MarkedForDeletion = 1
Where [Id] = @LinkId

GO

GRANT EXECUTE ON dbo.Links_DeleteLinkInformation TO [Public]
Go

/*
select * from LinkParagraphs
sp_columns LinkParagraphs
*/

