if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetAllFtpSites]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetAllFtpSites]
GO

CREATE PROCEDURE dbo.[Sites_GetAllFtpSites]
AS

Select s.*
From Sites s
Join FtpUploadInfo fui On fui.SiteId = s.Id
Join SiteSettings ss On ss.SiteId = s.Id And ss.SiteParameterId = 6
Where IntValue = 1
And dbo.Sites_IsSiteDisabled(s.Id) = 0

GO

GRANT EXECUTE ON dbo.[Sites_GetAllFtpSites] TO [Public]
Go

/*
Declare @utcNow Datetime 
Select @utcNow = GetUtcDate()
exec Sites_GetNextFtpUploadInfo @utcNow

select * from Sites
sp_columns sites
*/

