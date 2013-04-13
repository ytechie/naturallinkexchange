if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sites_EligibleForMultiSiteDiscount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sites_EligibleForMultiSiteDiscount]
GO

CREATE PROCEDURE dbo.[Sites_EligibleForMultiSiteDiscount]
	@SiteId Int,
	@RequestedLevel Int
AS

/*

Summary:
Determines if the specified site is eligible for a multi-site discount for
the account level requested.

*/

Declare @Eligible Bit
Declare @UserId Int
Declare @QualifyingSites Int

--Look up the owner of the site
Select @UserId = UserId
From Sites
Where Id = @SiteId

--Todo: Figure out a way to see if they CURRENTLY have a site that is paying full price, there
--			doesn't appear to be a way to do that right now.

--Get a list of all paying sites at the requested level or above
Select @QualifyingSites = Count(*)
From Sites s
Left Outer Join Subscriptions subs On subs.SiteId = s.Id
Where s.UserId = @UserId
And subs.PlanId >= @RequestedLevel

If @QualifyingSites = 0
	Set @Eligible = 0
Else
	Set @Eligible = 1

Select @Eligible

GO
GRANT EXECUTE ON dbo.[Sites_EligibleForMultiSiteDiscount] TO [Public]


/*
--should be 0
Sites_EligibleForMultiSiteDiscount 1, 2
--should be 0
Sites_EligibleForMultiSiteDiscount 745, 3
--should be 1
Sites_EligibleForMultiSiteDiscount 745, 2
--should be 1
Sites_EligibleForMultiSiteDiscount 73, 3

select * from subscriptions where planid > 1

*/

