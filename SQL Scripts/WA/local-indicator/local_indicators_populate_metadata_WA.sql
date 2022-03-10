use [sdgs_palestina]

-- GOAL_TYPE
INSERT INTO sdgs_palestina.dbo.Goal_Type (Type_ID,Label_En,Label_Ar,Descr_En,Descr_Ar,url_img,Descr_Short,Subindicator_Separator,order_code) VALUES
	 (2,'Subject',' Monitoring  AR','National Statistical Monitoring System ','National Statistical Monitoring System ','https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcRDdPM3ncUOMO4bNWXDLmVxz4seFfyyGUkLyw&usqp=CAU','Monitoring',NULL,1);

--extract subject in GOAL
insert into goal(Goal_ID,Goal_DescEn,Goal_DescAr,GoalImageEn,GoalImageAr,Type_ID,Goal_Code)
select distinct 100+mt.subject,mt.[subject name],mt.[subject name],null,null,2,mt.subject from matrix_table mt 

select * from  goal  

--target
select * from sdgs_palestina.dbo.Target t 

insert into target
select distinct 100+mt.subject,CAST(100+mt.subject as varchar(20))+ CAST(mt.target as varchar(20)),mt.target ,mt.target ,null,mt.target from matrix_table mt 

select * from  target 
 
-- ind_code

insert into Ind_Code
select CAST(100+mt.subject as varchar(20))+ CAST(mt.target as varchar(20)), mt.[Original code], mt.indcode from matrix_table mt 

select * from  Ind_Code ic where ic.Indicator_Code ='C200304'


select ic.Indicator_Code ,count(*) from  Ind_Code ic  
group by ic.Indicator_Code
where ic.Indicator_Code ='C200304'


--- indicators & code_mapping
select  mt.indcode , mt.[Original indicator Reference] from matrix_table mt  

insert into indicator
 select  mt.indcode , mt.[ indicators name ] ,mt.[ indicators name ]  from matrix_table mt where mt.indcode not in (
select Indicator_Code from indicator)



select * from indicator

insert into Code_Mapping
select  mt.indcode ,2 , mt.[Database Code] from matrix_table mt 


insert into Code_Mapping	
select distinct s.Subindicator_Code,2, s.Subindicator_Code from  subindicator s 
 join matrix_table mt  on s.Indicator_Code= mt._indcode_
 

select mt.indcode  ,count(*)  from matrix_table mt
group by mt.indcode  


select * from Code_Mapping 