If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetUniquePageName]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetUniquePageName]
GO

create function dbo.Links_GetUniquePageName(@SiteId Int, @CategoryId Int)
  Returns Varchar(70)
As

/*

Summary:
Generates a unique page name for a link page based off the standard
page name from the categories table.

*/

Begin

Declare @Suffix Int
Declare @PageName Varchar(50)
Declare @CurrPageName Varchar(50)

Select @PageName = c.PageName
From Categories c
Where c.[Id] = @CategoryId

Set @Suffix = 2
Set @CurrPageName = @PageName

While @CurrPageName In (Select PageName From LinkPages Where SiteId = @SiteId And CategoryId = @CategoryId)
	Begin
		Set @CurrPageName = @PageName + '-' + Cast(@Suffix As Varchar(10))
		Set @Suffix = @Suffix + 1
	End

Set @PageName = @CurrPageName

Return @PageName

End

Go

/*

Select dbo.Links_GetUniquePageName(1, 57)

select * from linkpages_articles
select * from sites
select * from LinkPages
select * from categories

update linkpages
set pagename = dbo.Links_GetUniquePageName(SiteId, CAtegoryId)
from linkpages lp

*/