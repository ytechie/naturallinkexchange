if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ranking_GetRankings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ranking_GetRankings]
GO

CREATE PROCEDURE dbo.Ranking_GetRankings
  @SiteRankUrlId Int = Null,
  @SearchEngineId Int = Null,
  @KeyPhraseId Int = Null,
  @StartTime DateTime = Null,
  @EndTime DateTime = Null
AS

Select [Timestamp], Rank, SearchEngineId, KeyPhraseId
From SearchEngineRankChecks
Where (@SiteRankUrlId Is Null Or SiteRankUrlId = @SiteRankUrlId)
And (@SearchEngineId Is Null Or SearchEngineId = @SearchEngineId)
And (@KeyPhraseId Is Null Or KeyPhraseId = @KeyPhraseId)
And (@StartTime Is Null Or [Timestamp] >= @StartTime)
And (@EndTime Is Null Or [Timestamp] <= @EndTime)
Order By KeyPhraseId, [Timestamp]

GO
GRANT EXECUTE ON dbo.Ranking_GetRankings TO [Public]
Go

/*
Ranking_GetRankings 2, 1, 8

select * from SearchEngineRankChecks
*/

