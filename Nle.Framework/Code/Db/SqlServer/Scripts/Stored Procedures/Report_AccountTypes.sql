/****** Object:  StoredProcedure [dbo].[Report_GeneralLinkStats]    Script Date: 12/15/2005 09:24:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].Report_AccountTypes') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].Report_AccountTypes
go
Create Procedure dbo.Report_AccountTypes
as

Exec Reporting_GetReportDefinition 1, ''

Select s.PlanId, Count(*) 
  from subscriptions s
  group by planId
  order by planid desc 

Select u.Name, s.name, b.PlanId, u.LastLogin, u.CreatedOn 
  from users u 
  left outer join sites s on u.id = s.userid
  left outer join subscriptions b on b.siteid = s.id
  order by b.planid desc 
go 
Grant execute on dbo.Report_AccountTypes to public

/*
Report_AccountTypes
*/