if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_SaveLinkGroupInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_SaveLinkGroupInformation]
GO

CREATE PROCEDURE dbo.Links_SaveLinkGroupInformation
  @GroupId Int = Null,
  @SiteId Int,
	@Title nVarChar(50),
	@Distribution Float,
	@Url1 nVarChar(100),
	@Url2 nVarChar(100),
	@AnchorText1 nVarChar(50),
	@AnchorText2 nVarChar(50),
	@Keyword1 nVarChar(100) = null,
	@Keyword2 nVarchar(100) = null
AS

If @GroupId Is Null
  Insert Into LinkParagraphGroups
  (SiteId, Title, Distribution, Url1, Url2, AnchorText1, AnchorText2, Keyword1, Keyword2)
  Values(@SiteId, @Title, @Distribution, @Url1, @Url2, @AnchorText1, @AnchorText2, @Keyword1, @Keyword2)
Else
  Update LinkParagraphGroups
  Set SiteId = @SiteId,
	Title = @Title,
  Distribution = @Distribution,
	Url1 = @Url1,
	Url2 = @Url2,
	AnchorText1 = @AnchorText1,
	AnchorText2 = @AnchorText2,
	Keyword1 = @Keyword1,
	Keyword2 = @Keyword2
  Where [Id] = @GroupId

GO

GRANT EXECUTE ON dbo.Links_SaveLinkGroupInformation TO [Public]
Go

/*
*/

