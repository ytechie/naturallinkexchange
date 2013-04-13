If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Categories_GetRelatedScore]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Categories_GetRelatedScore]
GO

create function dbo.[Categories_GetRelatedScore] (@CatId1 Int, @CatId2 Int)
  Returns Float
As

/*

Summary:
Determines how related 2 categories are by giving them an arbitrary score.

Remarks:
Order in the parameters does matter.  This function calculates how related
the first category is to the second.  The reciprocal might give another score.

Todo:
If 2 categories have the same parent, and are the same category, they get a score of 1.5, which is wrong
*/
Begin

Declare @Points Int
Set @Points = 0

--10 points for being the same category
If @CatId1 = @CatId2
	Begin
		--Print 'The categories are the same'
		Set @Points = @Points + 100
	End

--Look up the category parents
Declare @Parent1 Int
Declare @Parent2 Int

Select @Parent1 = c.ParentCategoryId
From Categories c
Where c.[Id] = @CatId1

Select @Parent2 = c.ParentCategoryId
From Categories c
Where c.[Id] = @CatId2

--Check if one is the parent of the other
If @Parent1 = @CatId2 Or @Parent2 = @CatId1
	Begin
		--Print 'One category is the parent of the other'
		Set @Points = @Points + 75
	End

--Check if the categories are siblings
If @Parent1 = @Parent2
	Begin
		--Print 'The categories are siblings'
		Set @Points = @Points + 50
	End

--Check if the categories have an explicit relationship
If Exists(Select *
			From CategoryRelationships
			Where FromCategoryId = @CatId1
			And ToCategoryId = @CatId2)
	Begin
		--Print 'The categories have an explicit relationship set up'
		Set @Points = @Points + 25
	End

--Calcuate a score based on the total number of possible sites
Declare @Score Float
Set @Score = @Points / 100.0

Return @Score

End
Go

/*
--Same category, should be a score of 1.00
Select dbo.[Categories_GetRelatedScore](2, 2) 

--Parent relationship should be .75
Select dbo.[Categories_GetRelatedScore](17, 2) 
Select dbo.[Categories_GetRelatedScore](17, 2) 

--Sibbling should be .50
Select dbo.[Categories_GetRelatedScore](25, 26)

--Explicit relationship should be .25
Select dbo.[Categories_GetRelatedScore](3, 4)

--No relationship should be 0
Select dbo.[Categories_GetRelatedScore](73, 20)


select * from Categories

select * From CategoryRelationships

*/