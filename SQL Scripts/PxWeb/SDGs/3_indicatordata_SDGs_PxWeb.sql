USE [pxweb_sdgs]
GO
 
SELECT sd.*  into  INDICATORDATA FROM SDGS.dbo.SUBINDICATOR_DATA  sd
join SDGS.dbo.code_mapping cm on sd.subindicator_code= cm.code
where cm.goal_type=1  and sb.obs_value is not null
GO




