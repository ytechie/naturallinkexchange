if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetReports]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetReports]
GO

CREATE PROCEDURE dbo.Reporting_GetReports
	@UserReportsOnly Bit = 0,
	@SiteId Int = Null
AS

If @SiteId Is Null
Begin
	If @UserReportsOnly = 0
		Select 'Administrator Reports' ReportCategory, name, Replace(name, 'Report_', '') DisplayName
		From sysobjects 
		Where xtype = 'P' And name Like 'Report[_]%'

	Select 'User Reports' ReportCategory, name, Replace(name, 'UserReport_', '') DisplayName
	From sysobjects 
	Where xtype = 'P' And name Like 'UserReport[_]%'
End
Else
Begin

Declare @AccountType Int
Declare @Now DateTime
Declare @PlanId Int

Select @AccountType = AccountType
From Users u
Join Sites s On s.UserId = u.Id
Where s.Id = @SiteID

Print 'User has account type ' + Cast(@AccountType As Varchar(1))

Select @Now = GETUTCDATE()

Select @PlanId = PlanId
From Subscriptions
Where @Now > StartTime And @Now < EndTime And SiteId = @SiteId
Order By Id Desc

Print 'User is on plan #' + Cast(Coalesce(@PlanId, 1) As Varchar(100))

If @AccountType = 1
	Select 'Administrator Reports' ReportCategory, o.name, Replace(o.name, 'Report_', '') DisplayName, ex2.value Description
	From sysobjects o
	Left Outer Join ExtendedProperties ex2 On o.name = Convert(sysname, ex2.objname) collate Latin1_General_CI_AI And ex2.name = 'Description'
	Where o.xtype = 'P' And o.name Like 'Report[_]%'

Select 'User Reports' ReportCategory, o.name, Replace(o.name, 'UserReport_', '') DisplayName, ex2.value Description
From sysobjects o
Left Outer Join ExtendedProperties ex On o.name = Convert(sysname, ex.objname) collate Latin1_General_CI_AI And ex.name = 'SubscriptionLevel'
Left Outer Join ExtendedProperties ex2 On o.name = Convert(sysname, ex2.objname) collate Latin1_General_CI_AI And ex2.name = 'Description'
Where o.xtype = 'P' And o.name Like 'UserReport[_]%' And (@AccountType = 1 Or Coalesce(@PlanId, 1) >= Cast(Coalesce(ex.value, 1) As Int))

End
GO

GRANT EXECUTE ON dbo.Reporting_GetReports TO [Public]
Go

/*
select * from sites where userid = 11
Reporting_GetReports Null, 60
reporting_getreports 0, 366
Reporting_GetReport 'UserReport_GetMyInfo'

Select * From LinkPackages

Report_GetAllUnsentEmail
Report_AccountTypes

Select * From Users
select * from sites where userid = 820
*/