if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Administration_ResetAllLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Administration_ResetAllLinks]
GO

CREATE PROCEDURE dbo.[Administration_ResetAllLinks]
	@Confirm Bit = Null
AS

/*

Summary: Resets all the link pages and links in the system by deleting all of them.  Use
	with extreme caution.  This is only for development database purposes.  Never run this on
	the production database, as it will cause massive data loss.

*/

If @Confirm Is Null Or @Confirm <> 1
	Begin
		Print 'You must pass in 1 to confirm this stored procedure.  This SP will delete data!!!!'
		Return
	End

Begin Try
	Begin Transaction

	Print 'Removing the link history'
	Delete From LinkHistory

	Print 'Resetting the starting link page for all sites'
	Update Sites Set StartLinkPageId = Null Where StartLinkPageId Is Not Null

	Print 'Deleting relationships between the link pages'
	Delete From LinkPages_RelatedCategories

	Print 'Deleting all link page RSS items'
	Delete From LinkPages_RssItems

	Print 'Deleting all link page articles (just the links, not the actual articles)'
	Delete From LinkPages_Articles

	Print 'Deleting all link pages'
	Delete From LinkPages

	Print 'Committing all changes'
	Commit Transaction
End Try
Begin Catch
	Print 'There was a failure that was caught, the transaction will be rolled back'

	IF XACT_STATE() <> 0
		Rollback Transaction

	--Select XACT_STATE()

	Declare @ErrorMsg Varchar(1000)
	Declare @ErrorSeverity Int
	Declare @ErrorState Int

	Set @ErrorMsg = Error_Message()
	Set @ErrorSeverity = Error_severity()
	Set @ErrorState = Error_State()

	Print 'Caught Error: ' + @ErrorMsg
End Catch

GO

GRANT EXECUTE ON dbo.[Administration_ResetAllLinks] TO [Public]
Go

/*
[Administration_ResetAllLinks] 1

select * from linkpages
select * from linkpages_rssitems
select * from linkpages_articles
*/

