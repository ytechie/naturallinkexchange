if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_GetAllEnabledUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_GetAllEnabledUsers]
GO

CREATE PROCEDURE dbo.Users_GetAllEnabledUsers
AS

Select *
From Users
Where Enabled = 1

GO

GRANT EXECUTE ON dbo.Users_GetAllEnabledUsers TO [Public]
Go


/*
Users_GetAllUsers
*/

