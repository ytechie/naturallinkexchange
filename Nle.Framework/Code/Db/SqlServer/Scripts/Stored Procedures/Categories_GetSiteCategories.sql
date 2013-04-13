if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetSiteCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Categories_GetSiteCategories]
GO

CREATE PROCEDURE dbo.Categories_GetSiteCategories
  
AS

Select * From Categories

GO

GRANT EXECUTE ON dbo.Categories_GetSiteCategories TO [Public]
Go