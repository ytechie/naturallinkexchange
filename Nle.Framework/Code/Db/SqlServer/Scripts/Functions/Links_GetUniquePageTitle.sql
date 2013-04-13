If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetUniquePageTitle]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetUniquePageTitle]
GO

create function dbo.Links_GetUniquePageTitle(@SiteId Int, @CategoryId Int)
  Returns Varchar(70)
As

/*

Summary:
Generates a unique page title for a link page based off the standard
page name from the categories table.

*/

Begin

Declare @Suffix Int
Declare @PageTitle Varchar(50)
Declare @CurrPageTitle Varchar(50)

Select @PageTitle = c.Title
From Categories c
Where c.[Id] = @CategoryId

Set @Suffix = 2
Set @CurrPageTitle = @PageTitle

While @CurrPageTitle In (Select PageTitle From LinkPages Where SiteId = @SiteId And CategoryId = @CategoryId)
	Begin
		Set @CurrPageTitle = @PageTitle + '-' + Cast(@Suffix As Varchar(10))
		Set @Suffix = @Suffix + 1
	End

Set @PageTitle = @CurrPageTitle

Return @PageTitle

End

Go

/*

Select dbo.Links_GetUniquePageTitle(1, 57)

select * from linkpages_articles
select * from sites
select * from LinkPages
select * from categories

update linkpages
set pagename = dbo.Links_GetUniquePageName(SiteId, CAtegoryId)
from linkpages lp

*/