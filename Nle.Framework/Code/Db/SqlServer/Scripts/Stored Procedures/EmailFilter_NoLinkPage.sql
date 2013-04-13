if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EmailFilter_NoLinkPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EmailFilter_NoLinkPage]
GO

CREATE PROCEDURE dbo.[EmailFilter_NoLinkPage]
AS

Declare @LastStatuses Table
(
	SiteId Int,
	MaxId Int
)

Insert Into @LastStatuses
	Select SiteId, Max(Id)
	From LinkPageStatuses
	Group By SiteId

Select Id 'UserId'
From Users
Where Id In
	(Select UserId
	From Sites s
	Join @LastStatuses ls On s.Id = ls.SiteId
	Join LinkPageStatuses lps On ls.MaxId = lps.Id
	Where lps.Valid = 0
	--If they haven't configured their account, they have other problems
	And dbo.Sites_IsSiteFullyConfigured(s.Id) = 1
	--Filter out disabled sites
	And s.Enabled = 1)
--Exclude customers that have paying sites
And dbo.Users_IsPayingCustomer(Id) <> 1
--Exclude new customers
And CreatedOn < DateAdd(ww, -1, GetUtcDate())
--Exclude disabled users
And Enabled = 1

GO

--Add the Description to the stored procedure's extended properties.
If Exists (Select * From ExtendedProperties Where name = 'Description' And objname = 'EmailFilter_NoLinkPage')
	Exec sp_dropExtendedProperty 'Description', 'user', dbo, 'Procedure', [EmailFilter_NoLinkPage]
Exec sp_addExtendedProperty 'Description', 'Users that don''t have a valid link page', 'user', dbo, 'Procedure', [EmailFilter_NoLinkPage]

GO

GRANT EXECUTE ON dbo.[EmailFilter_NoLinkPage] TO [Public]
Go

/*
select * from users

select * from linkpagestatuses



[EmailFilter_NoLinkPage]

select * from sites where 
select * from emailqueue where bounced > 1

*/