
-- create views into WA application needs for PxWeb export --

USE [SDGS]
GO

CREATE OR ALTER  VIEW V_GOAL as
select g.Goal_ID, g.Goal_CODE,g.Type_ID, CONCAT(gt.Label_En, g.Goal_CODE, ' - ', g.Goal_DescEn) as Goal_DescEn, CONCAT(gt.Label_Ar, g.Goal_CODE, ' - ', ISNULL(g.Goal_DescAr,'')) as Goal_DescAr
FROM Goal g inner join Goal_type gt on g.Type_ID=gt.Type_ID
GO

CREATE OR ALTER  VIEW V_TARGET as
SELECT Goal_ID, Target_ID,Target_CODE, CONCAT('TARGET ', Target_CODE, ' - ', Target_DescEn) as Target_DescEn, CONCAT('TARGET ', Target_CODE, ' - ', ISNULL(Target_DescAr,'')) as Target_DescAr
  FROM Target
GO


CREATE OR ALTER  VIEW [dbo].[V_INDICATOR] as
	select I.Indicator_Code, cm.Value as Indicator_CodeValue,cm.Goal_Type, IC.Indicator_NL+' '+I.Indicator_descEn as Indicator_descEn, IC.Indicator_NL+' '+ISNULL(I.Indicator_descAr,'') as Indicator_descAr, IC.Target_ID
FROM Indicator I 
INNER JOIN Ind_Code IC ON ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON 	t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on t.Goal_ID = g.Goal_ID
INNER JOIN Code_mapping cm on 	cm.Code = I.Indicator_Code and cm.Goal_Type =g.Type_ID 
GO

CREATE OR ALTER  VIEW V_SUBINDICATOR as
SELECT s.Indicator_Code, s.Subindicator_Code, cm.Value as Subindicator_CodeValue,cm.Goal_Type,IC.Target_ID, CONCAT(cm.Value, ' - ', s.Subindicator_DescEn) as Subindicator_DescEn, CONCAT(cm.Value, ' - ', ISNULL(s.Subindicator_DescAr,'')) as Subindicator_DescAr, s.Dimensions, s.Is_Valid
FROM	Subindicator s 
INNER JOIN Indicator I ON s.Indicator_Code = i.Indicator_Code
INNER JOIN Ind_Code IC ON ic.Indicator_Code = i.Indicator_Code 
INNER JOIN Target T ON 	t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on t.Goal_ID = g.Goal_ID
INNER JOIN Code_mapping cm on cm.Code = s.Subindicator_Code and cm.Goal_Type =g.Type_ID 
where Subindicator_Code in(
Select Subindicator_Code from SUBINDICATOR_DATA
)
GO
