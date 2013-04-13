If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_HasPublishedArticles]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_HasPublishedArticles]
GO

create function dbo.[Sites_HasPublishedArticles] (@SiteId Int)
  Returns Bit
As

/*

Summary: Determines if the site has valid articles published.

*/

Begin

Declare @Configured Bit

If Exists
		(Select Distinct lpg.SiteId
		From LinkParagraphGroups lpg
		Join LinkParagraphs lp On lp.LinkParagraphGroupId = lpg.Id
		Where lpg.SiteId = @SiteId
			And lp.Enabled = 1 And lp.MarkedForDeletion = 0)
	Set @Configured = 1
Else
	Set @Configured = 0

Return @Configured

End

Go

/*
select dbo.sites_haspublishedarticles(800)
select * from linkparagraphs where enabled = 0

		(select *
		From LinkParagraphGroups lpg
		--left outer Join LinkParagraphs lp On lp.LinkParagraphGroupId = lpg.Id
		Where lpg.SiteId = 675
			And lp.Enabled = 1 And lp.MarkedForDeletion = 0)

select * from siteurls where url like '%comparingsoftware%'

*/