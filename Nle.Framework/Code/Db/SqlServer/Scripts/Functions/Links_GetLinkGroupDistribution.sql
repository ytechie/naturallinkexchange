If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkGroupDistribution]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetLinkGroupDistribution]
GO

create function dbo.Links_GetLinkGroupDistribution (@SiteId Int)
  Returns Table
As

/*

Gets a list of the link groups and the number of times
that they are currently being used.

*/

Return Select lpg.[Id], Count(lpa.[Id]) 'LinkCount'
From LinkParagraphs lp
Join LinkParagraphGroups lpg On lp.LinkParagraphGroupId = lpg.[Id]
Left Outer Join LinkPages_Articles lpa On lpa.ArticleId = lp.[Id]
Where lpg.SiteId = @SiteId
And lp.Enabled = 1
And lpg.Enabled = 1
And lp.MarkedForDeletion = 0
Group By lpg.[Id]

Go

/*

Select * From dbo.Links_GetLinkGroupDistribution(1)

select * from sites

*/