

/****** Object:  View [dbo].[V_GOAL]    Script Date: 18/11/2021 10:38:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE     VIEW [dbo].[V_GOAL] as
select distinct  g.Goal_ID, g.Goal_CODE,g.Type_ID, CONCAT(gt.Label_En, g.Goal_CODE, ' - ', g.Goal_DescEn) as Goal_DescEn, CONCAT(gt.Label_Ar, g.Goal_CODE, ' - ', ISNULL(g.Goal_DescAr,'')) as Goal_DescAr
FROM Goal g inner join Goal_type gt on g.Type_ID=gt.Type_ID
INNER JOIN Target t on t.goal_id=g.goal_id
INNER JOIN Ind_Code IC ON t.Target_ID = IC.Target_ID
  where
  gt.IS_ACTIVE=1 and g.IS_ACTIVE=1 and t.IS_ACTIVE=1 and
  IC.Indicator_Code  in 
(select Indicator_Code from SUBINDICATOR_DATA where  OBS_VALUE is not null)
GO
/****** Object:  View [dbo].[V_TARGET]    Script Date: 18/11/2021 10:43:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE     VIEW [dbo].[V_TARGET] as
SELECT distinct T.Goal_ID, T.Target_ID,T.Target_CODE, CONCAT('TARGET ', T.Target_CODE, ' - ', T.Target_DescEn) as Target_DescEn, CONCAT('TARGET ', T.Target_CODE, ' - ', ISNULL(Target_DescAr,'')) as Target_DescAr
  FROM Target T
  INNER JOIN Ind_Code IC ON t.Target_ID = IC.Target_ID
  where  T.IS_ACTIVE=1 and  IC.Indicator_Code  in 
(select Indicator_Code from SUBINDICATOR_DATA where  OBS_VALUE is not null)
 
 
GO
/****** Object:  View [dbo].[V_INDICATOR]    Script Date: 18/11/2021 10:45:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE    VIEW [dbo].[V_INDICATOR] as
	select distinct I.Indicator_Code, cm.Value as Indicator_CodeValue,cm.Goal_Type, IC.Indicator_NL+' '+I.Indicator_descEn as Indicator_descEn, IC.Indicator_NL+' '+ISNULL(I.Indicator_descAr,'') as Indicator_descAr, IC.Target_ID
FROM Indicator I 
INNER JOIN Ind_Code IC ON ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON 	t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on t.Goal_ID = g.Goal_ID
INNER JOIN Code_mapping cm on 	cm.Code = I.Indicator_Code and cm.Goal_Type =g.Type_ID 
where 
   G.IS_ACTIVE=1 and T.IS_ACTIVE=1 and I.IS_ACTIVE=1 and I.Indicator_Code  in 
(select Indicator_Code from SUBINDICATOR_DATA where  OBS_VALUE is not null)
 
 
GO
/****** Object:  View [dbo].[V_SUBINDICATOR]    Script Date: 18/11/2021 10:48:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE    VIEW [dbo].[V_SUBINDICATOR] as
SELECT distinct
s.Indicator_Code, s.Subindicator_Code, cm.Value as
Subindicator_CodeValue,cm.Goal_Type,IC.Target_ID, CONCAT(cm.Value, ' - ', s.Subindicator_DescEn) as Subindicator_DescEn, CONCAT(cm.Value, ' - ', ISNULL(s.Subindicator_DescAr,'')) as Subindicator_DescAr,s.Dimensions, s.Is_Valid
FROM Subindicator s
INNER JOIN Indicator I ON s.Indicator_Code = i.Indicator_Code
INNER JOIN Ind_Code IC ON ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on t.Goal_ID = g.Goal_ID
INNER JOIN Code_mapping cm on cm.Code = s.Subindicator_Code and cm.Goal_Type =g.Type_ID
where 
 g.IS_ACTIVE=1 and t.IS_ACTIVE=1 and I.IS_ACTIVE=1 and S.Is_Valid=1 and
s.Subindicator_Code in(
Select Subindicator_Code from SUBINDICATOR_DATA where OBS_VALUE is not null
)
GO