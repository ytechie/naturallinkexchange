if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EmailFilter_SitesNotFullyConfigured]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EmailFilter_SitesNotFullyConfigured]
GO

CREATE PROCEDURE dbo.[EmailFilter_SitesNotFullyConfigured]
AS

--Include users from the NoLinkPage email list
Declare @NoLinkPage Table(UserId Int)

Insert Into @NoLinkPage
Exec dbo.EmailFilter_NoLinkPage

--Include users that haven't fully configured their site
Declare @NotConfigured Table(UserId Int)

Insert Into @NotConfigured
Select Id 'UserId'
From Users
Where Id In
	(Select UserId
	From Sites s
	Where dbo.Sites_IsSiteFullyConfigured(s.Id) = 0
	--Filter out disabled sites
	And s.Enabled = 1)
--Exclude customers that have paying sites
And dbo.Users_IsPayingCustomer(Id) <> 1
--Exclude new customers
And CreatedOn < DateAdd(ww, -1, GetUtcDate())
--Exclude disabled users
And Enabled = 1

--Return all unique results
Select UserId
From @NoLinkPage
	Union
Select UserId
From @NotConfigured

--Add the Description to the stored procedure's extended properties.
If Exists (Select * From ExtendedProperties Where name = 'Description' And objname = 'EmailFilter_SitesNotFullyConfigured')
	Exec sp_dropExtendedProperty 'Description', 'user', dbo, 'Procedure', [EmailFilter_SitesNotFullyConfigured]
Exec sp_addExtendedProperty 'Description', 'Users that have not fully configured a site', 'user', dbo, 'Procedure', [EmailFilter_SitesNotFullyConfigured]

GO

GRANT EXECUTE ON dbo.[EmailFilter_SitesNotFullyConfigured] TO [Public]
Go

/*
[EmailFilter_SitesNotFullyConfigured]

select *, dbo.sites_issitefullyconfigured(id) from sites where userid = 70


select * from users
*/