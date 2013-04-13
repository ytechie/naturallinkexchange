if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Report_Reporting Demo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Report_Reporting Demo]
GO

CREATE PROCEDURE dbo.[Report_Reporting Demo]
AS

/* Report Section 1 Definition */
--Execute Reporting_GetReport definition to define a new section of the report.
Exec Reporting_GetReportDefinition 
	@Type = 1,
	@Description = 'Shows information about NLE administrators for deomonstration purposes.',
	@Title = 'NLE Administrators'


/* Report Section 1 Data */
--Perform the select statements to retrieve the information for this section of the report.
Select u.Id, u.Name, u.CreatedOn, u.LastLogin, Count(s.Id) Sites, r.Name ReferredBy
From Users u
Left Outer Join Users r On r.Id = u.ReferrerId
Left Outer Join Sites s On s.UserId = u.Id
Where u.AccountType = 1
Group By u.Id, u.Name, u.CreatedOn, u.LastLogin, r.Name

--You can perform more than one select in a given report section
Select u.Name, s.Name SiteName, p.FriendlyName LinkPackage, sub.StartTime, sub.EndTime
From Users u
Join Sites s On s.UserId = u.Id
Join Subscriptions sub On sub.SiteId = s.Id
Join LinkPackages p On sub.PlanId = p.Id
Where u.AccountType = 1
Order By u.Id, s.Id


/* Report Section 2 Definition */
Exec Reporting_GetReportDefinition
	@Type = 1,
	@Description = 'Displays the sites of the NLE Administrator for deomonstration purposes.',
	@Title = 'NLE Administrator Sites'

/* Report Section 2 Data */
Select u.Name, s.Name SiteName, su.Url
From Sites s
Join Users u On s.UserId = u.Id
Join SiteUrls su On su.SiteId = s.Id
Where u.AccountType = 1
Order By u.Id, s.Id

--You can execute other report stored procedures to "embed" them into this report
Exec Report_AccountTypes

GO

--Add the Description to the stored procedure's extended properties.
If Exists (Select * From ExtendedProperties Where name = 'Description' And objname = 'Report_Reporting Demo')
	Exec sp_dropExtendedProperty 'Description', 'user', dbo, 'Procedure', [Report_Reporting Demo]
	Exec sp_addExtendedProperty 'Description', 'This report demonstrates the abilities of the NLE reporting system.', 'user', dbo, 'Procedure', [Report_Reporting Demo]

--Add the SubscriptionLevel to the stored procedure's extended properties.
--This does not have an affect on an Administrative report, but on it does on a User report.
--If no subscription level is supplied, defaults to 1 (Free) when retrieved/executed.
If Exists (Select * From ExtendedProperties Where name = 'SubscriptionLevel' And objname = 'Report_Reporting Demo')
	Exec sp_dropExtendedProperty 'SubscriptionLevel', 'user', dbo, 'Procedure', [Report_Reporting Demo]
	Exec sp_addExtendedProperty 'SubscriptionLevel', '1', 'user', dbo, 'Procedure', [Report_Reporting Demo]

GRANT EXECUTE ON dbo.[Report_Reporting Demo] TO [Public]
Go

/*
[Report_Reporting Demo]
*/