if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_SetFtpInfoProcessed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_SetFtpInfoProcessed]
GO

CREATE PROCEDURE dbo.Sites_SetFtpInfoProcessed
  @Id Int,
  @LastUpload DateTime
AS

Update FtpUploadInfo
Set LastUpload = @LastUpload
Where [Id] = @Id
And Enabled = 1

GO

GRANT EXECUTE ON dbo.Sites_SetFtpInfoProcessed TO [Public]
Go

/*
Declare @utcNow Datetime 
Select @utcNow = GetUtcDate()
exec Sites_GetNextFtpUploadInfo @utcNow

select * from ftpUploadInfo
sp_columns ftpUploadInfo
*/

