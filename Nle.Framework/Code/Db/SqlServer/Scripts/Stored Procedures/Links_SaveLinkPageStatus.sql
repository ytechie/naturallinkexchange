if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_SaveLinkPageStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Links_SaveLinkPageStatus]
GO

CREATE PROCEDURE dbo.[Links_SaveLinkPageStatus]
	@StatusId Int = Null,
	@CheckedOn DateTime,
	@Valid Bit,
	@SiteId Int
AS

If @StatusId Is Null
	Begin
		Insert Into LinkPageStatuses
		(CheckedOn, Valid, SiteId)
		Values(@CheckedOn, @Valid, @SiteId)
	
		Return @@Identity
	End
Else
	Begin
	  Update LinkPageStatuses
	  Set CheckedOn = @CheckedOn,
		Valid = @Valid,
		SiteId = @SiteId
	  Where [Id] = @StatusId

		Return @StatusId
	End



GO

GRANT EXECUTE ON dbo.[Links_SaveLinkPageStatus] TO [Public]
Go

/*
select * from linkpagestatuses
*/

