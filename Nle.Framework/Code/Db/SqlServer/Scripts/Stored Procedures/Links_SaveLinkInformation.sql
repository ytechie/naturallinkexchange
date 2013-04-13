if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_SaveLinkInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_SaveLinkInformation]
GO

CREATE PROCEDURE dbo.Links_SaveLinkInformation
  @LinkId Int = Null,
  @Title Varchar(Max),
  @Paragraph Varchar(500),
  @Enabled Bit,
  @GroupId Int
AS

If @LinkId Is Null
  Insert Into LinkParagraphs
  (Title, Paragraph, Enabled, LinkParagraphGroupId)
  Values(@Title, @Paragraph, @Enabled, @GroupId)
Else
  Update LinkParagraphs
  Set Title = @Title,
  Paragraph = @Paragraph,
  Enabled = @Enabled,
  LinkParagraphGroupId = @GroupId
  Where [Id] = @LinkId

GO

GRANT EXECUTE ON dbo.Links_SaveLinkInformation TO [Public]
Go

/*
Links_GetLinkInformation 1
Links_GetLinkInformation 10

select * from LinkParagraphs
sp_columns LinkParagraphs
*/

