If exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Links_GetLinkPartners]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[Links_GetLinkPartners]
GO