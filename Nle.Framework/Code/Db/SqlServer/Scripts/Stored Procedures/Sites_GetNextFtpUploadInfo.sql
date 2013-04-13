if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetNextFtpUploadInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetNextFtpUploadInfo]
GO

CREATE PROCEDURE dbo.Sites_GetNextFtpUploadInfo
  @CurrentTime Datetime,
  @IntervalMS BigInt --The number of milliseconds between each run
AS

Select Top 1 *
From FtpUploadInfo
Where LastUpload Is Null
Or DateAdd(ms, @IntervalMS, LastUpload) < @CurrentTime
Order By LastUpload

GO

GRANT EXECUTE ON dbo.Sites_GetNextFtpUploadInfo TO [Public]
Go

/*
Declare @utcNow Datetime 
Select @utcNow = GetUtcDate()
exec Sites_GetNextFtpUploadInfo @utcNow

select * from Sites
sp_columns sites
*/

