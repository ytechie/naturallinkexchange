if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Services_SetLastPollTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Services_SetLastPollTime]
Go

CREATE PROCEDURE dbo.[Services_SetLastPollTime]
  @ServiceId Int,
	@PollTime DateTime = Null --Null = Now
AS

/*

Summary: Updates the poll time of a service.

*/



Go

GRANT EXECUTE ON dbo.[Services_SetLastPollTime] TO [Public]
Go

/*
select * from services

*/