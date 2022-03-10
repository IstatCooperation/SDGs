delete   from SUBINDICATOR_DATA   where SUBINDICATOR_CODE like '9%'

select * from Subindicator s  where SUBINDICATOR_CODE like '9%'

delete   from Subindicator   where SUBINDICATOR_CODE like '9%'

select * from indicator s  where INDICATOR_CODE like '9%'

delete  from indicator   where INDICATOR_CODE like '9%'

select * from Code_Mapping   where Value like '9%'

delete  from Code_Mapping   where Value like '9%'

select * from Ind_Code ic where Target_ID in (
 SELECT t2.Target_ID from Target t2  where t2.Goal_ID >100
)

DELETE from Ind_Code  where Target_ID in (
 SELECT t2.Target_ID from Target t2  where t2.Goal_ID >100
)

SELECT * from Target t2  where t2.Goal_ID >100

SELECT Goal_ID ,count(*) from Target t2  where t2.Goal_ID >100
GROUP  by Goal_ID 
 
delete from Target  where Goal_ID >100
  
SELECT * from goal   where Goal_ID >100
   
delete from goal  where Goal_ID >100