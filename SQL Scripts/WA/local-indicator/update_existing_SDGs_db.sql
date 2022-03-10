use SDGs;

-- clean Code_Mapping
delete from Code_Mapping;

-- clean Code_Mapping Ind_Code
delete from Ind_Code where Target_ID in (
select t.Target_ID from Target t where t.Goal_ID>100
);

-- clean Code_Mapping indicator
delete from Indicator where Indicator_Code like '97%';

--update matrix_table mt 
 
MERGE  matrix_table mt
USING  ( 
select idc.* from Goal_Type gt
join Goal g on gt.Type_ID=g.Type_ID
join Target t on t.Goal_ID=g.Goal_ID
join Ind_Code idc on idc.Target_ID=t.Target_ID 
where gt.Type_ID=1  and  idc.Indicator_NL not in ('3.7.2','7.b.1')

)   ic    
ON mt.[_Original_code_] = ic.Indicator_NL
     WHEN MATCHED 
        THEN update set mt._indcode_= ic.Indicator_Code 
     WHEN NOT MATCHED  BY SOURCE 
        THEN update set mt._indcode_=mt.[_Database_Code_] ;

/****** Load Indicator label in Code_Mapping  for SDGs ******/
insert into Code_Mapping	
SELECT distinct
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
WHERE   g.Type_ID=1

AND
	not EXISTS (
	SELECT
		1
	FROM
		code_mapping cm
	WHERE
		cm.Code = i.Indicator_Code
		AND cm.Goal_Type = g.Type_ID );
		
/****** Load subindicator_code  label in Code_Mapping  for SDGs ******/
insert into Code_Mapping	

SELECT distinct
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
WHERE g.Type_ID=1
AND
	not EXISTS (
	SELECT
		1
	FROM
		code_mapping cm
	WHERE
		cm.Code = s.Subindicator_Code
		AND cm.Goal_Type = g.Type_ID )
	
--INSERT  Local indicator
insert into Ind_Code
	select
		CAST(100+mt._subject_ as varchar(20))+ CAST(mt._target_ as varchar(20)),
		mt.[_Original_code_],
		mt._indcode_
	from matrix_table mt;

insert into indicator
	select
		mt._indcode_,
		mt.[_indicators_name_],
		mt.[_indicators_name_]
	from matrix_table mt
	where mt._indcode_ not in (
		select Indicator_Code from indicator
	);

insert into Code_Mapping
	select distinct
		mt._indcode_,
		2,
		mt.[_Database_Code_]
	from matrix_table mt;

 	 

 
 insert into Code_Mapping	
 select distinct s.Subindicator_Code,2,CONCAT (_Database_CODE_, SUBSTRING(s.Subindicator_Code,CHARINDEX('_',s.Subindicator_Code)+1,LEN(s.Subindicator_Code))) from  subindicator s 
 join matrix_table mt  on s.Indicator_Code= mt._indcode_;
 

 