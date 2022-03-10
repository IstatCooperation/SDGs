USE [pxweb_local]
GO

 
SELECT sd.*  into  INDICATORDATA FROM sdgs.dbo.SUBINDICATOR_DATA  sd
join sdgs.dbo.code_mapping cm on sd.subindicator_code= cm.code
where cm.goal_type=2 and sb.obs_value is not null
GO

