If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_AvailableArticleCount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_AvailableArticleCount]
GO

create function dbo.[Sites_AvailableArticleCount] (@SiteId Int)
  Returns Int
As

/*

Summary:
Counts the number of articles that the site has available for linking.

*/

Begin

Declare @ArticleCount Int

Select @ArticleCount = Count(lp.Id)
From LinkParagraphGroups lpg
Join LinkParagraphs lp On lp.LinkParagraphGroupId = lpg.[Id]
Where lpg.SiteId = @SiteId


Return @ArticleCount

End
Go

/*
Select dbo.[Sites_AvailableArticleCount](1) 

select * from sites
select * from linkparagraphs

*/