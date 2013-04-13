If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_GetRelatedScore]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Sites_GetRelatedScore]
GO

create function dbo.[Sites_GetRelatedScore] (@SiteId1 Int, @SiteId2 Int)
  Returns Float
As

/*

Summary:
Determines how related 2 sites are by giving them an arbitrary score.

Remarks:
Order in the parameters does matter.  This function calculates how related
the first site is to the second.  The reciprocal might give another score.
*/
Begin

Declare @Score Float
Set @Score = 0.0

Declare @Cat1 Int
Declare @Cat2 Int

--Look up the categories of the sites
Select @Cat1 = s.InitialCategoryId
From Sites s
Where s.[Id] = @SiteId1

Select @Cat2 = s.InitialCategoryId
From Sites s
Where s.[Id] = @SiteId2

Return dbo.Categories_GetRelatedScore(@Cat1, @Cat2)

End
Go

/*
--Same category, should be a score of 1
Select dbo.Sites_GetRelatedScore(1, 402) 

select * from sites

*/