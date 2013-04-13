if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EmailFiltering_GetFilters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EmailFiltering_GetFilters]
GO

CREATE PROCEDURE dbo.EmailFiltering_GetFilters
AS

Select o.name, Replace(o.name, 'EmailFilter_', '') DisplayName, ex.value Description
From sysobjects o
Left Outer Join ExtendedProperties ex On o.name = Convert(sysname, ex.objname) collate Latin1_General_CI_AI And ex.name = 'Description'
Where o.xtype = 'P' And o.name Like 'EmailFilter[_]%'

GO

GRANT EXECUTE ON dbo.EmailFiltering_GetFilters TO [Public]
Go

/*
EmailFiltering_GetFilters
*/