if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkGroupInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_GetLinkGroupInformation]
GO

CREATE PROCEDURE dbo.Links_GetLinkGroupInformation
  @LinkGroupId Int = Null
AS

Select [Id], SiteId, Title, Distribution, Url1, Url2, AnchorText1, AnchorText2, MarkedForDeletion, Enabled,
	Coalesce(Keyword1, '{anchor1}') [Keyword1], Coalesce(Keyword2, '{anchor2}') [Keyword2]
From LinkParagraphGroups
Where @LinkGroupId Is Null Or [Id] = @LinkGroupId

GO
GRANT EXECUTE ON dbo.Links_GetLinkGroupInformation TO [Public]