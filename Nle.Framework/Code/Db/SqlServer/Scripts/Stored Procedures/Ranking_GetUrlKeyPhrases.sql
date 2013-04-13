if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ranking_GetUrlKeyPhrases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ranking_GetUrlKeyPhrases]
GO

CREATE PROCEDURE dbo.Ranking_GetUrlKeyPhrases
  @SiteRankUrlId Int
AS

Select kp.*
From KeyPhrases kp
Where kp.[Id] In (Select Distinct(KeyPhraseId) From SearchEngineRankChecks Where SiteRankUrlId = @SiteRankUrlId)

GO
GRANT EXECUTE ON dbo.Ranking_GetUrlKeyPhrases TO [Public]


/*
Ranking_GetUrlKeyPhrases 1

select * from searchenginerankchecks
select * from SiteRankUrls

select * from Sites
sp_columns sites
*/

