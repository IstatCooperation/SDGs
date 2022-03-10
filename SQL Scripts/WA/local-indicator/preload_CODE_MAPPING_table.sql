/****** Preload table with existing sdgs code******/
/****** Load indicator_code from indicator ******/
SELECT
	i.Indicator_Code ,
	g.Type_ID,
	i.Indicator_Code 
FROM
	Indicator i
INNER JOIN Ind_Code IC ON
	ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON
	t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on
	t.Goal_ID = g.Goal_ID
WHERE
	not EXISTS (
	SELECT
		1
	FROM
		code_mapping cm
	WHERE
		cm.Code = i.Indicator_Code
		AND cm.Goal_Type = g.Type_ID )
		
/****** Load subindicato_code from Subindicator ******/

SELECT
	s.Subindicator_Code  ,
	g.Type_ID,
	s.Subindicator_Code 
FROM
	Subindicator s
INNER JOIN Indicator I ON
	s.Indicator_Code = i.Indicator_Code
INNER JOIN Ind_Code IC ON
	ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON
	t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on
	t.Goal_ID = g.Goal_ID
WHERE
	not EXISTS (
	SELECT
		1
	FROM
		code_mapping cm
	WHERE
		cm.Code = s.Subindicator_Code
		AND cm.Goal_Type = g.Type_ID )
	
		