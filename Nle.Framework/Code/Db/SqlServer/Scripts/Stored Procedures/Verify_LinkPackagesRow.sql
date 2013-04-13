if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Verify_LinkPackagesRow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Verify_LinkPackagesRow]
GO

CREATE PROCEDURE dbo.Verify_LinkPackagesRow
	@Id Int,
	@FriendlyName Varchar(50),
	@LinkGroups Int,
	@AnchorCount Int,
	@ArticlesPerGroup Int,
	@MonthlyPrice float,
	@LinkPercentDays Int,
	@Bans Int,
	@FeedUpdateDays Int,
	@MinFeedsPerLinkPage Int,
	@MaxFeedsPerLinkPage Int,
	@OutInRatio float,
	@YearlyPrice float,
	@MinLinksPerCycle Int,
	@MaxLinksPerCycle Int,
	@MonthlyPriceMultiple float,
	@YearlyPriceMultiple float
AS

/*

Summary: Verifies the contents of the static data row in the LinkPackages
	table and makes it match the values specified.

*/

If Exists(Select [Id] From LinkPackages Where [Id] = @Id)
	Begin
		Update LinkPackages
		Set FriendlyName = @FriendlyName,
		LinkGroups = @LinkGroups,
		AnchorCount = @AnchorCount,
		ArticlesPerGroup = @ArticlesPerGroup,
		MonthlyPrice = @MonthlyPrice,
		LinkPercentDays = @LinkPercentDays,
		Bans = @Bans,
		FeedUpdateDays = @FeedUpdateDays,
		MinFeedsPerLinkPage = @MinFeedsPerLinkPage,
		MaxFeedsPerLinkPage = @MaxFeedsPerLinkPage,
		OutInRatio = @OutInRatio,
		YearlyPrice = @YearlyPrice,
		MinLinksPerCycle = @MinLinksPerCycle,
		MaxLinksPerCycle = @MaxLinksPerCycle,
		MonthlyPriceMultiple = @MonthlyPriceMultiple,
		YearlyPriceMultiple = @YearlyPriceMultiple
		Where [Id] = @Id
	End
Else
	Begin
		Set Identity_Insert LinkPackages On

		Insert Into LinkPackages
		([Id], FriendlyName, LinkGroups, AnchorCount, ArticlesPerGroup, MonthlyPrice, LinkPercentDays, Bans, FeedUpdateDays, MinFeedsPerLinkPage, MaxFeedsPerLinkPage, OutInRatio, YearlyPrice, MinLinksPerCycle, MaxLinksPerCycle, MonthlyPriceMultiple, YearlyPriceMultiple)
		Values(@Id, @FriendlyName, @LinkGroups, @AnchorCount, @ArticlesPerGroup, @MonthlyPrice, @LinkPercentDays, @Bans, @FeedUpdateDays, @MinFeedsPerLinkPage, @MaxFeedsPerLinkPage, @OutInRatio, @YearlyPrice, @MinLinksPerCycle, @MaxLinksPerCycle, @MonthlyPriceMultiple, @YearlyPriceMultiple)

		Set Identity_Insert LinkPackages Off
	End

GO

GRANT EXECUTE ON dbo.Verify_LinkPackagesRow TO [Public]
Go

/*

sp_columns linkpackages
*/