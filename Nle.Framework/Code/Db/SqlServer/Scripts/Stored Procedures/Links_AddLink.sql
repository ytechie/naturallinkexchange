if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_AddLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Links_AddLink]
Go

CREATE PROCEDURE dbo.Links_AddLink
  @SiteId Int --the site to link to
AS

/*

Summary: This SP add an incoming link to the specified site Id.

Process:
- Find a another site that is related to this site as possible
- Find the best article to use on their site
- Create the link

*/

Declare @FromSiteId Int

Print 'Looking for a site to get a link from'

If dbo.Sites_IsSiteFullyConfigured(@SiteId) = 0
	Begin
		Print 'This site is not yet configured, so they will not get a link'
		Return
	End

Select Top 1 @FromSiteId = s.[Id]
From Sites s
--Exclude those that we already link to
Where s.[Id] Not In (Select SiteId From dbo.[Links_GetOutgoingLinkedPartners](@SiteId))
--Exclude our own site
And s.Id <> @SiteId
--Exclude those that already link to us
And s.[Id] Not In (Select SiteId From dbo.[Links_GetIncomingLinkedPartners](@SiteId))
--Only include sites that are fully configured
And dbo.Sites_IsSiteFullyConfigured(s.[Id]) = 1
--Only include enabled sites
And dbo.Sites_IsSiteDisabled(s.[Id]) = 0
--Order them by how related they are
Order By dbo.Sites_GetRelatedScore(s.[Id], @SiteId) Desc,
--Also order them by how few links they have given out
dbo.Links_GetOutgoingLinkedPartnersCount(s.Id) Asc

If @FromSiteId Is Null
  Begin
    Print 'There Are No Available Link Sources'
    Return
  End

Print 'Site #' + Cast(@FromSiteId As Varchar(10)) + ' was chosen to link back to this site'
Print 'Site has a related score of ' + Cast(dbo.Sites_GetRelatedScore(@FromSiteId, @SiteId) As Varchar(100))

Declare @Link Int

Print 'Determining the article to link to'
Exec @Link = Links_GetNextSiteLink @SiteId

If @Link = 0
  Begin
    Print 'There are no articles to choose from, we can''t add a link'
    Return
  End

Print 'Creating the actual link'
Exec Links_LinkArticle @FromSiteId, @Link

Go

GRANT EXECUTE ON dbo.Links_AddLink TO [Public]
Go

/*

select * from linkpages
select * from linkpages_articles
select * from sites where id = 102

select * from subscriptions where siteid = 102

Select * From dbo.[Links_GetIncomingLinkedPartners](1)

select * from linkparagraphs
begin transaction
Links_AddLink 1
commit transaction

select * from linkparagraphs
select * from linkparagraphgroups

Links_GetNextSiteLink 1

Declare @ret Int
Exec @ret = Links_GetNextSiteLink 52
print @ret

select * from linkparagraphs where paragraph like '%test%'
*/

