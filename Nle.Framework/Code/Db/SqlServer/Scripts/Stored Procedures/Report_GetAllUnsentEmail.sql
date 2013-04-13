if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Report_GetAllUnsentEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Report_GetAllUnsentEmail]
GO

CREATE PROCEDURE dbo.Report_GetAllUnsentEmail
	@StartTime DateTime = Null --The timestamp to begin the search at
AS

Exec Reporting_GetReportDefinition 1, ''

Select *
From EmailQueue
Where SentOn Is Null
And (@StartTime Is Null Or QueuedOn >= @StartTime)

GO

GRANT EXECUTE ON dbo.Report_GetAllUnsentEmail TO [Public]
Go

/*

Report_GetAllUnsentEmail

select * from emailqueue

*/