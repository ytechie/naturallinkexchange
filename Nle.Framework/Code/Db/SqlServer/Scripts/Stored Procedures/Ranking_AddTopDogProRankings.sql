if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ranking_AddTopDogProRankings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ranking_AddTopDogProRankings]
GO

CREATE PROCEDURE dbo.Ranking_AddTopDogProRankings
  @Timestamp DateTime,
  @Rank Int,
  @SearchEngineName Varchar(100),
  @SiteUrl Varchar(100),
  @SearchPhrase Varchar(100)
AS

--Look up the ID of the search engine
Declare @SearchEngineId Int

Select @SearchEngineId = se.[Id]
From SearchEngines se
Where se.TopDogName = @SearchEngineName

--If the search engine can't be found, add it
If @SearchEngineId Is Null
  Begin
    Insert Into SearchEngines
    ([Name], TopDogName)
    Values(@SearchEngineName, @SearchEngineName)

    Set @SearchEngineId = @@Identity
  End

--Look up the ID of the site rank url
Declare @SiteUrlId Int

Select @SiteUrlId = su.[Id]
From SiteUrls su
Where su.Url = @SiteUrl

--Not much we can do if we don't find the site
If @SiteUrlId Is Null
  Begin
    Declare @Msg Varchar(100)

    Set @Msg = 'Could Not Find Site With Url ' + @SiteUrl

    Raiserror(@Msg, 16, -1)
 
    Return
  End


--Look up the Id of the key phrase
Declare @KeyPhraseId Int

Select @KeyPhraseId = kp.[Id]
From KeyPhrases kp
Where kp.Phrase = @SearchPhrase

--If the key phrase can't be found, add it
If @KeyPhraseId Is Null
  Begin
    Insert Into KeyPhrases
    (Phrase)
    Values(@SearchPhrase)

    Set @KeyPhraseId = @@Identity
  End

Insert Into SearchEngineRankChecks
([Timestamp], Rank, SearchEngineId, KeyPhraseId, SiteRankUrlId)
Values(@Timestamp, @Rank, @SearchEngineId, @KeyPhraseId, @SiteUrlId)


GO

GRANT EXECUTE ON dbo.Ranking_AddTopDogProRankings TO [Public]
Go

/*
select * from searchenginerankchecks
select * from siterankurls

begin transaction

Ranking_AddTopDogProRankings '1/1/2000', 5, 'MSN', 'http://www.SuperJason.com', 'SuperJason'

rollback transaction
commit transaction
*/

