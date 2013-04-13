if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LinkPackages_GetAllLinkPackages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LinkPackages_GetAllLinkPackages]
GO

CREATE PROCEDURE dbo.LinkPackages_GetAllLinkPackages
AS

Select *
From LinkPackages

GO

GRANT EXECUTE ON dbo.LinkPackages_GetAllLinkPackages TO [Public]
Go

/*
LinkPackages_GetAllLinkPackages


*/


