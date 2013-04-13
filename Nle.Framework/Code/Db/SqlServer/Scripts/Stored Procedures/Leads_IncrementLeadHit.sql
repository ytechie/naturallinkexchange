if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Leads_IncrementLeadHit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Leads_IncrementLeadHit]
GO

CREATE PROCEDURE dbo.Leads_IncrementLeadHit
  @LeadId Int
AS

/*

Summary:
Increments the lead hit counter for a particular lead.

*/

Update LeadSources
Set HitCount = HitCount + 1
Where [Id] = @LeadId

GO

GRANT EXECUTE ON dbo.Leads_IncrementLeadHit TO [Public]
Go


/*
select * from LeadSources

*/

