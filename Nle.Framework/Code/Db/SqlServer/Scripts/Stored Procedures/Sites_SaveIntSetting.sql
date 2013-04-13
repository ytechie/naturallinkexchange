if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_SaveIntSetting]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_SaveIntSetting]
GO

CREATE PROCEDURE dbo.Sites_SaveIntSetting
	@SiteId Int,
	@ParameterId Int,
	@Value Int
AS

/*

*/

Declare @Exists Bit

If (Select Count(*)
		From SiteSettings ss
		Where SiteParameterId = @ParameterId
		And SiteId = @SiteId) > 0
	Set @Exists = 1
Else
	Set @Exists = 0

If @Exists = 1
	Update SiteSettings
	Set IntValue = @Value
	Where SiteId = @SiteId
	And SiteParameterId = @ParameterId
Else
	Insert Into SiteSettings
	(SiteId, SiteParameterId, IntValue)
	Values(@SiteId, @ParameterId, @Value)


GO

GRANT EXECUTE ON dbo.Sites_SaveIntSetting TO [Public]
Go

/*
select * from siteparameters
select * from sitesettings

Sites_SaveIntSetting 1, 6, 0
*/

