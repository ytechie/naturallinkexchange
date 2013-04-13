If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetLinkPageRelatedCategories]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Categories_GetLinkPageRelatedCategories]
GO

create function dbo.Categories_GetLinkPageRelatedCategories (@LinkPageId Int)
  Returns Table
As

/*

Summary:
Retrieves all the related categories for a link page

*/

Return Select c.*
From LinkPages_RelatedCategories lprc
Join LinkPages lp On lprc.RelatedLinkPageId = lp.[Id]
Join Categories c On lp.CategoryId = c.[Id]
Where lprc.LinkPageId = @LinkPageId


Go

/*

Select * From dbo.Categories_GetLinkPageRelatedCategories(25)

select * from linkpages_relatedcategories

*/