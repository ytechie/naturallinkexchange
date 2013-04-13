If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetOutgoingLinkedPartners]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetOutgoingLinkedPartners]
GO

create function dbo.[Links_GetOutgoingLinkedPartners] (@SiteId Int)
  Returns Table
As

/*

Summary:
Retrieves all the sites that the specified site already links to.

*/

Return Select Distinct(lpg.SiteId)
--Select *
From LinkPages_Articles lpa
--Joins for the source
Join LinkPages lp On lp.[Id] = lpa.LinkPageId
--Joins for the destination
Join LinkParagraphs a On a.[Id] = lpa.ArticleId
Join LinkParagraphGroups lpg On a.LinkParagraphGroupId = lpg.[Id] --Look up the group for the site
Where lp.SiteId = @SiteId
--Where lpg.SiteId = @SiteId

Go

/*

Select * From dbo.[Links_GetOutgoingLinkedPartners](1)
Select * From dbo.[Links_GetIncomingLinkedPartners](53)

select * from linkpages_articles
select * from sites
select * from LinkParagraphs

*/