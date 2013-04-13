if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UserReport_Affiliate List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UserReport_Affiliate List]
GO

CREATE PROCEDURE dbo.[UserReport_Affiliate List]
	@UserId Int
AS

Exec Reporting_GetReportDefinition 
	@Type = 1,
	@Description = 'This reports shows a list of all the sites that you referred to this service.',
	@Title = ''

/* Report Section 1 Data */
Select '' As [Information]

Exec Reporting_GetReportDefinition 
	@Type = 1,
	@Description = '',
	@Title = 'Affiliate List'

Select u.[Name] [Referred User], s.[Name] [Site Name], u.CreatedOn [Referred On]
From Users u
Join Sites s On s.UserId = u.Id
Where ReferrerId = @UserId

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

GRANT EXECUTE ON dbo.[UserReport_Affiliate List] TO [Public]
Go

/*
exec [UserReport_Affiliate List] 820

select * from users where id = 820
*/