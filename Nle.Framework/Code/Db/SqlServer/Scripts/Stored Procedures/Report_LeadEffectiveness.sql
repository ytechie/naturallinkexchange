if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Report_LeadEffectiveness]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Report_LeadEffectiveness]
GO

CREATE PROCEDURE dbo.Report_LeadEffectiveness
AS

Exec Reporting_GetReportDefinition 1, ''

Declare @SignupCounts Table
(
	LeadId Int,
	[Count] Int
)

Insert Into @SignupCounts
Select Distinct(u.LeadId), Count(u.LeadId) 'Lead Count'
From Users u
Where LeadId Is Not Null
Group By u.LeadId

Select [Id] 'Ad/Lead Id',
		Description 'Lead Description',
		CONVERT(money, Cost) 'Cost',
		EstimatedDistribution 'Estimated Distribution',
		'Sign-up Rate' =
		convert(varchar(100), 
			(convert(numeric(10,7),
				Case HitCount 
					when 0 then 0 
					else Coalesce(Cast(sc.[Count] As Float) / Cast(HitCount As Float), 0) 
				end
			)*100)) 
		+ '%',
		HitCount 'Clicks',
		Coalesce(sc.[Count], 0) 'Sign Ups',
		'Cost/Click' =
		CONVERT(money, 
			Case HitCount 
				when 0 then 0 
				else Coalesce(Cost / Cast(HitCount As Float), 0) 
			end
		),
		'Cost/Sign-up'=
		CONVERT(money, 
			Coalesce(Cost / Cast(sc.[Count] As Float), 0) 
		),
		'Overall Sign-up Rate' = 
		convert(varchar(100), 
			(convert(numeric(10,7),
				Coalesce(Cast(sc.[Count] As Float) / Cast(EstimatedDistribution As Float), 0)
			)*100)) 
		+ '%',
		'Cost/1000 (CPM)' =  
		CONVERT(money, 
			Case  
				when HitCount = 0 then 0 
				when EstimatedDistribution = 0 then 0 
				else Coalesce(Coalesce(Cost , 0)/ (Cast(EstimatedDistribution As float)/1000), 0)
			end 
		),
		'Clicks/1000' = 
		CONVERT(money, 
			Case HitCount 
				when 0 then 0 
				else Coalesce(Cast(HitCount As float) / (Cast(EstimatedDistribution As float)/1000), 0)
			end
		)
From LeadSources ls
Left Outer Join @SignupCounts sc On sc.LeadId = ls.[Id]

GO

GRANT EXECUTE ON dbo.Report_LeadEffectiveness TO [Public]
GO

/*
Report_LeadEffectiveness

select * from LeadSources
select * from users

*/

