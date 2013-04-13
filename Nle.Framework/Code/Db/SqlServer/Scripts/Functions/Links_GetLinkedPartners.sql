If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetIncomingLinkedPartners]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetIncomingLinkedPartners]
GO

create function dbo.[Links_GetIncomingLinkedPartners] (@SiteId Int)
  Returns Table
As

/*

Summary:
Retrieves all the sites that link to the specified site.

*/

Return Select Distinct(lp.SiteId)
From LinkPages_Articles lpa
--Joins for the source
Join LinkPages lp On lp.[Id] = lpa.LinkPageId
--Joins for the destination
Join LinkParagraphs a On a.[Id] = lpa.ArticleId
Join LinkParagraphGroups lpg On a.LinkParagraphGroupId = lpg.[Id] --Look up the group for the site
Where lpg.SiteId = @SiteId

Go

/*

Select * From dbo.[Links_GetIncomingLinkedPartners](675)
Select * From dbo.[Links_GetIncomingLinkedPartners](52)

select * from linkpages_articles
select * from sites
select * from LinkParagraphs
select * from linkpages where siteid = 675

select * from ftpuploadinfo where siteid = 675

select * from sites where id = 675
report_getallunsentemail
*/