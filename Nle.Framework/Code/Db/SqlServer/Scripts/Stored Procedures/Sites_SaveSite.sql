if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_SaveSite]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_SaveSite]
GO

CREATE PROCEDURE dbo.Sites_SaveSite
	@SiteId Int = Null,
	@Name nVarChar(50),
	@UserId Int,
	@Url nVarChar(100),
	@Enabled Int,
	@InitialCategoryId Int,
	@PageTemplate Text = Null,
	@StartLinkPageId Int = Null,
	@UpgradeFlag Int = Null,
	@LinkPageUrl Varchar(Max) = Null,
	@HideInitialSetupMessage Bit = 0
AS

If @SiteId Is Null
	Begin
		Insert Into Sites
		([Name], UserId, Enabled, InitialCategoryId, GroupId, PageTemplate, SiteGuid, StartLinkPageId, UpgradeFlag, LinkPageUrl, HideInitialSetupMessage)
		Values(@Name, @UserId, @Enabled, @InitialCategoryId, 1, @PageTemplate, NewId(), @StartLinkPageId, @UpgradeFlag, @LinkPageUrl, @HideInitialSetupMessage)

		Set @SiteId = @@Identity

		Declare @SiteUrlId Int

		Insert Into SiteUrls
		(SiteId, Url)
		Values(@SiteId, @Url)

		Set @SiteUrlId = @@Identity
		Update Sites
		Set RootUrlId = @SiteUrlId
		Where Id = @SiteId
	End
Else
  Update Sites
  Set [Name] = @Name,
	UserId = @UserId,
	Enabled = @Enabled,
	InitialCategoryId = @InitialCategoryId,
	PageTemplate = @PageTemplate,
	StartLinkPageId = @StartLinkPageId,
	UpgradeFlag = @UpgradeFlag,
	LinkPageUrl = @LinkPageUrl,
	HideInitialSetupMessage = @HideInitialSetupMessage
  Where [Id] = @SiteId

Return @SiteId

GO

GRANT EXECUTE ON dbo.Sites_SaveSite TO [Public]
Go

/*
sp_columns sites
*/

