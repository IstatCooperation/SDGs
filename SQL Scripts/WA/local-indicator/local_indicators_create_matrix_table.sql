
---
-- create tables by file excel....
--

---
--- create table  matrix_table
 delete  TABLE matrix_table  
 
 --create empty table  matrix_table
 select * into matrix_table from Environment_16$ d where d.[ indicators name ] ='aaa6'
 
 select *  from matrix_table mt
 
  select count(*)  from matrix_table mt
 
 select mt.indcode,count(*)  from matrix_table mt
group by mt.indcode 


 
-- fill TABLE matrix_table

DECLARE @tableName varchar(1000);
DECLARE @subject varchar(1000);
DECLARE @code varchar(30);
DECLARE @sqlCommand varchar(3000);

DECLARE sbjts_cursor CURSOR  
    FOR
SELECT 
    QUOTENAME(ts.TABLE_NAME) FROM  information_schema.tables ts  
 
OPEN sbjts_cursor  
FETCH NEXT FROM sbjts_cursor INTO @tableName
 
WHILE @@FETCH_STATUS = 0  
BEGIN  
      
        SET @sqlCommand = ' Insert into matrix_table  select t.* FROM ' + @tableName +' t'

EXEC (@sqlCommand)
     FETCH NEXT FROM sbjts_cursor INTO @tableName
END  
 
CLOSE sbjts_cursor  
DEALLOCATE sbjts_cursor

select * into matrix_table from pcbs_matrix.dbo.matrix_table mt 


UPDATE  matrix_table 
set subject=20 
WHERE [subject name]='Energy'
  
-- ind_code

ALTER TABLE matrix_table ADD indcode nvarchar(255) NULL 


select count(*)  from matrix_table mt where mt.[Original indicator Reference] ='SDGs'

select * from matrix_table mt  where mt.[Original indicator Reference] ='SDGs' and mt.[Original code] not in (select ic.Indicator_NL collate Latin1_General_CI_AS  from sdgs_palestina.dbo.Ind_Code ic)

select * from sdgs_palestina.dbo.Ind_Code ic  


MERGE  matrix_table mt
USING sdgs_palestina.dbo.Ind_Code ic    
ON mt.[Original code] = ic.Indicator_NL collate Latin1_General_CI_AS
     WHEN MATCHED 
        THEN update set mt.indcode= ic.Indicator_Code 
     WHEN NOT MATCHED  BY SOURCE 
        THEN update set mt.indcode=mt.[Database Code] ;
   
-- nota: SDGs non presenti
select mt.indcode,count(*)  from matrix_table mt
group by mt.indcode 

