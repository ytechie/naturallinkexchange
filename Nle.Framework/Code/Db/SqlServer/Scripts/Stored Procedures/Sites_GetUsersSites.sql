if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetUsersSites]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_GetUsersSites]
GO

CREATE PROCEDURE dbo.Sites_GetUsersSites
  @UserId Int,
	@FullyConfiguredSitesOnly Bit = 0
AS

Select s.[Id], s.[Name], su.Url, s.Enabled,
	s.InitialCategoryId,
	IsNull(s.PageTemplate, gs.[TextValue]) PageTemplate, LastGeneration, UserId, RootUrlId, SiteGuid, 		dbo.Sites_GetIntSiteSetting(s.[Id], 5), UpgradeFlag
From Sites s
Join SiteUrls su On s.RootUrlId = su.[Id]
Left Outer Join GlobalSettings gs On gs.[Id] = 2
Where @UserId = s.UserId And s.MarkedForDeletion = 0 And
	(
		@FullyConfiguredSitesOnly = 0 Or 
		Exists (
			--Only sites that have subscriptions that are now
			Select Top 1 SiteId
			From Subscriptions
			Where Subscriptions.SiteId = s.[Id]
				And ((GetUtcDate() Between StartTime And EndTime) Or (GetUtcDate() >= StartTime And EndTime Is Null))
		) And
		Exists (
			--Only sites that have published Link Paragraphs
			Select Distinct lpg.SiteId
			From LinkParagraphGroups lpg
			Join LinkParagraphs lp On lp.LinkParagraphGroupId = lpg.Id
			Where lpg.SiteId = s.[Id]
				And lp.Enabled = 1 And lp.MarkedForDeletion = 0
		) And
		--Only Sites that have Page Templates
		(Substring(s.PageTemplate, 1, 1) <> '') And
		Exists (
			--Only Sites that have Page generation configuration set
			Select SiteId
			From SiteSettings
			Where SiteSettings.SiteId = s.[Id]
				And SiteParameterId = 6 And IntValue <> 0
		)
	)

GO
GRANT EXECUTE ON dbo.Sites_GetUsersSites TO [Public]


/*
Sites_GetUsersSites 11, 1

print 'Char: ' + substring('f', 1, 1)

select * from users
select id, name from Sites
select * from GlobalSettings

select * from 

sp_columns sites
*/

