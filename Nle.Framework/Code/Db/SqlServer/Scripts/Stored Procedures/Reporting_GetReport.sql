if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetReport]
GO

CREATE PROCEDURE dbo.Reporting_GetReport
	@ReportName NVarchar(500)
AS

Select o.name, Coalesce(ex.value, '1') SubscriptionLevel, ex2.value Description
From sysobjects o
Left Outer Join ExtendedProperties ex On o.name = Convert(sysname, ex.objname) collate Latin1_General_CI_AI And ex.name = 'SubscriptionLevel'
Left Outer Join ExtendedProperties ex2 On o.name = Convert(sysname, ex2.objname) collate Latin1_General_CI_AI And ex2.name = 'Description'
Where o.name = @ReportName

GO

GRANT EXECUTE ON dbo.Reporting_GetReport TO [Public]
Go

/*
Reporting_GetReport 'Report_GetUsers'

*/