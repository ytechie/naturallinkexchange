if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetFtpUploadInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetFtpUploadInfo]
GO

CREATE PROCEDURE dbo.Sites_GetFtpUploadInfo
  @SiteId Int
AS

Select *
From FtpUploadInfo
Where SiteId = @SiteId

GO

GRANT EXECUTE ON dbo.Sites_GetFtpUploadInfo TO [Public]
Go

/*
Declare @utcNow Datetime 
Select @utcNow = GetUtcDate()
exec Sites_GetNextFtpUploadInfo @utcNow

select * from Sites
sp_columns sites
*/

