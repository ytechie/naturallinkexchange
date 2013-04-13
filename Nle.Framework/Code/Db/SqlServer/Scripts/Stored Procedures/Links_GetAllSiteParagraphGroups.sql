if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetAllSiteParagraphGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetAllSiteParagraphGroups]
GO

CREATE PROCEDURE dbo.Links_GetAllSiteParagraphGroups
  @SiteId Int
AS

Select [Id], SiteId, Title, Distribution, Url1, Url2, AnchorText1, AnchorText2, MarkedForDeletion, Enabled,
	Coalesce(Keyword1, '{anchor1}') [Keyword1], Coalesce(Keyword2, '{anchor2}') [Keyword2]
From LinkParagraphGroups
Where SiteId = @SiteId And MarkedForDeletion = 0

GO

GRANT EXECUTE ON dbo.Links_GetAllSiteParagraphGroups TO [Public]
Go


/*
Links_GetAllSiteParagraphGroups 1
*/

