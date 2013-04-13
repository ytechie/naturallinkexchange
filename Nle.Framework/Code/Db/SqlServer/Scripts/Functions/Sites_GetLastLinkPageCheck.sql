If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetLastLinkPageCheck]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_GetLastLinkPageCheck]
GO

create function dbo.[Sites_GetLastLinkPageCheck] (@SiteId Int)
  Returns Int
As

/*

Summary:
Gets the ID of the row that represents the result of the last link page check.

*/

Begin

Declare @Id Int

Select @Id = lps.Id
From LinkPageStatuses lps
Where lps.SiteId = @SiteId
And lps.CheckedOn = (Select Max(CheckedOn) From LinkPageStatuses Where SiteId = @SiteId)

Return @Id

End
Go

/*
Select dbo.[Sites_GetLastLinkPageCheck](1) 

select * from linkpagestatuses

*/