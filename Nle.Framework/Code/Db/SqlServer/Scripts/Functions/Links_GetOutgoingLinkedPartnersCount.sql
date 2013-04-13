If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetOutgoingLinkedPartnersCount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetOutgoingLinkedPartnersCount]
GO

create function dbo.[Links_GetOutgoingLinkedPartnersCount] (@SiteId Int)
  Returns Int
As

/*

Summary:
Retrieves a count of all the sites that the specified site already links to.

*/

Begin

Declare @Ret Int

Select @Ret = Count(*) From dbo.Links_GetOutgoingLinkedPartners(@SiteId)

Return @Ret

End

Go

/*

Select dbo.Links_GetOutgoingLinkedPartnersCount(1)

select * from linkpages_articles
select * from sites
select * from LinkParagraphs

*/