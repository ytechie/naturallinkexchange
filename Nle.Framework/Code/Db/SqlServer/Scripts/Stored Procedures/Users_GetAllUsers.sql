if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users_GetAllUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Users_GetAllUsers]
GO

CREATE PROCEDURE dbo.Users_GetAllUsers
AS

Select *
From Users

GO

GRANT EXECUTE ON dbo.Users_GetAllUsers TO [Public]
Go


/*
Users_GetAllUsers
*/

