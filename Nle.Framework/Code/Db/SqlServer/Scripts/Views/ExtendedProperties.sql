if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExtendedProperties]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].ExtendedProperties
GO

CREATE VIEW dbo.ExtendedProperties
AS

Select *
From ::fn_listextendedproperty(null, 'user', 'dbo', 'Procedure', null, null, null)

GO

--GRANT EXECUTE ON dbo.Filter_AdministratorsOnly TO [Public]
--Go

/*
Select * From Filter_AdministratorsOnly
select * from emailqueue
delete from emailqueue
Administration_SendEmailToAllUsers 'Test Message', 'Hello to all.'

*/