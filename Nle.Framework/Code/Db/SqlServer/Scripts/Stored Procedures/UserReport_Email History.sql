if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UserReport_Email History]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UserReport_Email History]
GO

CREATE PROCEDURE dbo.[UserReport_Email History]
	@UserId Int
AS

Exec Reporting_GetReportDefinition 
	@Type = 1,
	@Description = '',
	@Title = ''

Select SentOn [Sent], QueuedOn [Queued], Subject
From EmailQueue
Where UserId = @UserId
Order By SentOn Desc


GO

----Add the Description to the stored procedure's extended properties.
--If Exists (Select * From ExtendedProperties Where name = 'Description' And objname = 'Report_Reporting Demo')
--	Exec sp_dropExtendedProperty 'Description', 'user', dbo, 'Procedure', [Report_Reporting Demo]
--	Exec sp_addExtendedProperty 'Description', 'This report demonstrates the abilities of the NLE reporting system.', 'user', dbo, 'Procedure', [Report_Reporting Demo]
--
----Add the SubscriptionLevel to the stored procedure's extended properties.
----This does not have an affect on an Administrative report, but on it does on a User report.
----If no subscription level is supplied, defaults to 1 (Free) when retrieved/executed.
--If Exists (Select * From ExtendedProperties Where name = 'SubscriptionLevel' And objname = 'Report_Reporting Demo')
--	Exec sp_dropExtendedProperty 'SubscriptionLevel', 'user', dbo, 'Procedure', [Report_Reporting Demo]
--	Exec sp_addExtendedProperty 'SubscriptionLevel', '1', 'user', dbo, 'Procedure', [Report_Reporting Demo]

GRANT EXECUTE ON dbo.[UserReport_Email History] TO [Public]
Go

/*
[UserReport_Email History] 4

select * from emailqueue where userid = 4
*/