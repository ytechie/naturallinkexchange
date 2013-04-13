if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EmailFilter_Administrators]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EmailFilter_Administrators]
GO

CREATE PROCEDURE dbo.EmailFilter_Administrators
AS

Select [Id] [UserId]
From Users
Where AccountType = 1

GO

--Add the Description to the stored procedure's extended properties.
If Exists (Select * From ExtendedProperties Where name = 'Description' And objname = 'EmailFilter_Administrators')
	Exec sp_dropExtendedProperty 'Description', 'user', dbo, 'Procedure', [EmailFilter_Administrators]
	Exec sp_addExtendedProperty 'Description', 'Filters only NLE Administrators.', 'user', dbo, 'Procedure', [EmailFilter_Administrators]

GO

GRANT EXECUTE ON dbo.EmailFilter_Administrators TO [Public]
Go

/*

EmailFilter_Administrators

select * from emailqueue where bounced > 1

*/