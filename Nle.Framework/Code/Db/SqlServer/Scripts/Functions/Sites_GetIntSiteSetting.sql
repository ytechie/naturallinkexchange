If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetIntSiteSetting]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_GetIntSiteSetting]
GO

create function dbo.Sites_GetIntSiteSetting (@SiteId Int, @ParameterId Int)
  Returns Int
As

/*

Summary:

*/

Begin
  Declare @ReturnVal Int

  Select @ReturnVal = Coalesce(ss.IntValue, sp.DefaultInt)
  From SiteParameters sp
  Left Outer Join SiteSettings ss On (ss.SiteId = @SiteId and ss.SiteParameterId = sp.[Id])
  Where sp.[Id] = @ParameterId

  Return(@ReturnVal)
End

Go

/*

Select dbo.Sites_GetIntSiteSetting(1, 5)

select * from sitesettings
select * from siteparameters
*/