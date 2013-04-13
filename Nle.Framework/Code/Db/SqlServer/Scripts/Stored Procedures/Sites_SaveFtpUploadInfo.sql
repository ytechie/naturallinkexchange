if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_SaveFtpUploadInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_SaveFtpUploadInfo]
GO

CREATE PROCEDURE dbo.Sites_SaveFtpUploadInfo
  @FtpInfoId Int = Null, --If null, indicates an insert operation
  @Siteid Int,
  @Enabled Bit,
  @Url Varchar(100),
	@FtpPath Varchar(255),
  @Username Varchar(50),
  @Password Varchar(50),
  @LastUpload DateTime = Null,
  @DailyUploadTime DateTime = Null,
	@ActiveMode Bit = 0
AS

If @FtpInfoId Is Null
  Insert Into FtpUploadInfo
  (SiteId, Enabled, Url, Username, [Password], LastUpload, DailyUploadtime, FtpPath, ActiveMode)
  Values(@SiteId, @Enabled, @Url, @Username, @Password, @LastUpload, @DailyUploadTime, @FtpPath, @ActiveMode)
Else
  Update FtpUploadInfo
  Set SiteId = @SiteId,
  Enabled = @Enabled,
  Url = @Url,
  Username = @Username,
  [Password] = @Password,
  LastUpload = @LastUpload,
  DailyUploadTime = @DailyUploadTime,
	FtpPath = @FtpPath,
	ActiveMode = @ActiveMode
  Where [Id] = @FtpInfoId

Go

GRANT EXECUTE ON dbo.Sites_SaveFtpUploadInfo TO [Public]
Go

/*
Declare @utcNow Datetime 
Select @utcNow = GetUtcDate()
exec Sites_GetNextFtpUploadInfo @utcNow

select * from FtpUploadInfo
sp_columns FtpUploadInfo
*/

