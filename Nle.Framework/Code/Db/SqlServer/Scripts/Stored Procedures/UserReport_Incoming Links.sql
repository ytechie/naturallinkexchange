if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UserReport_Incoming Links]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UserReport_Incoming Links]
GO

CREATE PROCEDURE dbo.[UserReport_Incoming Links]
	@UserId Int
AS

Exec Reporting_GetReportDefinition 
	@Type = 1,
	@Description = 'This reports shows a list of all incoming links for each site that the user has.',
	@Title = ''

/* Report Section 1 Data */
Select 'This report is currently in development. Please note that this list may not be a ' +
				'perfect representation of the sites that are linking to you, due to numerous factors.' As [Information]


Declare @Sites Table (SiteId Int, SiteName Varchar(Max))

Insert Into @Sites
Select s.[Id], s.[Name]
From Sites s
Where s.UserId = @UserId

Declare @CurrSiteId Int
Declare @CurrSiteName Varchar(Max)

While (Select Count(*) From @Sites) > 0
	Begin
		--Get a site, and delete it from the queue
		Select @CurrSiteId = SiteId, @CurrSiteName = SiteName
		From @Sites

		Delete From @Sites Where SiteId = @CurrSiteId

		Print 'Processing Site ' + @CurrSiteName

		Exec Reporting_GetReportDefinition 
			@Type = 1,
			@Description = '',
			@Title = @CurrSiteName

		/* Report Section 1 Data */
		Select u.Url
		From Sites s
		Join SiteUrls u On u.Id = s.RootUrlId
		Where s.[Id] In (Select SiteId From dbo.[Links_GetIncomingLinkedPartners](@CurrSiteId))

	End


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

GRANT EXECUTE ON dbo.[UserReport_Incoming Links] TO [Public]
Go

/*
[UserReport_Outgoing Links] 4

select top 10 * from sites

select * From dbo.[Links_GetOutgoingLinkedPartners](1)
Select SiteId From dbo.[Links_GetOutgoingLinkedPartners](1)
*/