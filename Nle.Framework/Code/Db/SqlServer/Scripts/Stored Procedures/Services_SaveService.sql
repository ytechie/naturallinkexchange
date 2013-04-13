if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Services_SaveService]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
  drop procedure [dbo].[Services_SaveService]
Go

CREATE PROCEDURE dbo.[Services_SaveService]
  @ServiceId Int,
	@Description Varchar(Max),
	@LastHeartbeat DateTime,
	@RunIntervalMinutes Int,
	@Enabled Bit,
	@LastRunTime DateTime,
	@ReloadConfiguration Bit,
	@ForceRun Bit
AS

/*

Summary: Saves the service information back to the database.

*/

If @ServiceId Is Null
	Begin
		Insert Into Services
		(Id, Description, LastHeartbeat, RunIntervalMinutes, Enabled, LastRunTime, ReloadConfiguration, ForceRun)
		Values(@ServiceId, @Description, @LastHeartbeat, @RunIntervalMinutes, @Enabled, @LastRunTime, @ReloadConfiguration, @ForceRun)
	
		Return @@Identity
	End
Else
	Begin
	  Update Services
	  Set Description = @Description,
		LastHeartbeat = @LastHeartbeat,
		RunIntervalMinutes = @RunIntervalMinutes,
		Enabled = @Enabled,
		LastRunTime = @LastRunTime,
		ReloadConfiguration = @ReloadConfiguration,
		ForceRun = @ForceRun
		Where Id = @ServiceId

		Return @ServiceId
	End

Go

GRANT EXECUTE ON dbo.[Services_SaveService] TO [Public]
Go

/*
select * from services

[Services_GetService] 1

sp_columns services
*/

