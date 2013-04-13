if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LinkPackages_GetLinkPackage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LinkPackages_GetLinkPackage]
GO

CREATE PROCEDURE dbo.LinkPackages_GetLinkPackage
	@LinkPackageId Int
AS

Select *
From LinkPackages
Where [Id] = @LinkPackageId

GO

GRANT EXECUTE ON dbo.LinkPackages_GetLinkPackage TO [Public]
Go

/*
LinkPackages_GetLinkPackage 1


*/


