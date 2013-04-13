if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPageIdForClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetLinkPageIdForClient]
GO

CREATE PROCEDURE dbo.Links_GetLinkPageIdForClient
  @SiteGuid UniqueIdentifier,
  @LinkPageName Varchar(255) = Null
AS

/*

Summary:
Retrieves the link page of the link page for a specific client request.  In
other words, this is how we look up the link page when a customer uses a dynamic
page that requests the link pages from us.

*/

Declare @LinkPageId Int
Declare @SiteId Int
Declare @CategoryId Int

--Look up the site Id
Select @SiteId = s.[Id], @CategoryId = InitialCategoryId, @LinkPageId = StartLinkPageId
From Sites s
Where s.SiteGuid = @SiteGuid

--If no link page name is specified, we need to look up the link page id
--of the link page that they specified
If @LinkPageName Is Not Null
	Begin
		Print 'A link page name is specified, looking up the id'

		Select @LinkPageId = lp.[Id]
		From LinkPages lp
		Where lp.SiteId = @SiteId
		And lp.PageName = @LinkPageName
	End

--If we still haven't found the page they wanted, get the default.
If @LinkPageId Is Null
	--Note: This will always get a link page, or even create one if necessary
	Exec @LinkPageId = Links_GetInitialLinkPageId @SiteId

Select lp.*
From LinkPages lp
Where [Id] = @LinkPageId

GO

GRANT EXECUTE ON dbo.Links_GetLinkPageIdForClient TO [Public]
Go


/*
Links_GetLinkPageIdForClient 'F39CF146-FB7B-4F8B-88EB-60F2BF43063C','Business'
Links_GetLinkPageIdForClient 'F57AE67C-56E1-4C7B-9A4F-3211BCA6E76A', null
Links_GetLinkPageIdForClient 'F57AE67C-56E1-4C7B-9A4F-3211BCA6E76A', 'People'

Links_GetAllSiteLinks 1
Links_GetAllSiteLinks 1, 57

select * from sites
select * from linkpages
sp_columns LinkParagraphs
*/

