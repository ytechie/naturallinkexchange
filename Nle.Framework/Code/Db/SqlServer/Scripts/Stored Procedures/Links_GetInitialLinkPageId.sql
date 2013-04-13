if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetInitialLinkPageId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetInitialLinkPageId]
GO

CREATE PROCEDURE dbo.Links_GetInitialLinkPageId
  @SiteId Int
AS

/*

Summary:
Retrieves the initial link page Id for a site.  If there is no initial
link page, a blank one is created.

*/

Declare @LinkPageId Int
Declare @CategoryId Int

--Make sure the site even exists
If Not Exists(Select [Id] From Sites Where [Id] = @SiteId)
	Begin
		Raiserror('Cannot look up the inital link page for a non-existant site', 16, 1)
		Return -1
	End

--Look up the site category and start link page
Select @CategoryId = InitialCategoryId, @LinkPageId = StartLinkPageId
From Sites s
Where s.[Id] = @SiteId

If @LinkPageId Is Null
	Begin
		--Check if there is an existing link page to use that is in our category
		Exec Links_SelectInitialLinkPage @SiteId, @LinkPageId Output
		
		If @LinkPageId Is Null		
			Begin		
				--Create an initial link page for the site since an appropriate one doesn't exist yet
				Print 'There are no link pages for the site, so one is being created'
				Exec @LinkPageId = Links_CreateLinkPage @SiteId, @CategoryId
				Exec Links_SetInitialLinkPage @SiteId, @LinkPageId
			End
	End

Return @LinkPageId

GO

GRANT EXECUTE ON dbo.Links_GetInitialLinkPageId TO [Public]
Go


/*
Declare @Id Int
Exec @Id = Links_GetInitialLinkPageId 1
print @Id

update sites
set startlinkpageid  = null
where id = 1

select * from sites where id = 57
select * from linkpages
sp_columns LinkParagraphs
*/

