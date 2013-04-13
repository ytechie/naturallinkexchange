if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UsersBouncedEmails]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].UsersBouncedEmails
GO

CREATE VIEW dbo.UsersBouncedEmails
AS

Select UserId [UserId], Count(Bounced) [BounceCount]
From EmailQueue
Where Bounced = 1
Group By UserId

GO

/*
Select * From UsersBouncedEmails

*/