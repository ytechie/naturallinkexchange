if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].Links_RecalculateLinkRatio') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].Links_RecalculateLinkRatio
GO

CREATE PROCEDURE dbo.Links_RecalculateLinkRatio
  @SiteId Int = Null
AS

/*

Recalculates the ratio between the number if incoming links
and the number of outgoing links.  This should be called any time
that a link is made.

*/

Declare @SiteLinks As Table
(
	FromSiteId Int,
	ToSiteId Int
)

--Get a list of all site links
Insert Into @SiteLinks
Select lp.SiteId 'From', lpg.SiteId 'To'
From LinkPages_Articles lpa
Join LinkPages lp On lp.Id = lpa.LinkPageId --Find the site the link was from
Join LinkParagraphs para On para.Id = lpa.ArticleId  --Find where the link was to
Join LinkParagraphGroups lpg On para.LinkParagraphGroupId = lpg.Id
Where (@SiteId Is Null Or lp.SiteId = @SiteId Or lpg.SiteId = @SiteId)

--Select * From @SiteLinks

/*
Note: I check if the outgoing links are 0, and if they are, I make it a really
		small value, so that we can avoid division by 0 errors.  It's alright if
		it makes the link ratio number huge, because they are out of balance, and
		a single outgoing link will fix it.
*/

Update Sites
Set ActualInOutLinkRatio =
	(Cast((Select Count(*)
	From @SiteLinks
	Where FromSiteId = [Id]) As Float) /
	Cast((Select Case Count(*) When 0 Then 0.000001 Else Count(*) End
	From @SiteLinks
	Where ToSiteId = [Id]) As Float))
Where (@SiteId Is Null Or [Id] = @SiteId)

GO

GRANT EXECUTE ON dbo.Links_RecalculateLinkRatio TO [Public]
Go


/*
select * from sites

Links_RecalculateLinkRatio 1
*/

