if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetReportDefinition]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetReportDefinition]
GO

CREATE PROCEDURE dbo.Reporting_GetReportDefinition
	@Type Int,
	@Description NVarchar(1000) = NULL,
	@Title NVarchar(512) = NULL
AS


Select ReportType = Case @Type 
	When 1 Then 'Nle.Components.Reporting.TableReport, Nle.Framework' 
	Else ''
	End,
	@Description ReportLongDescription, @Title Title

GO

GRANT EXECUTE ON dbo.Reporting_GetReportDefinition TO [Public]
Go

/*
Reporting_GetReportDefinition 1, ''
*/