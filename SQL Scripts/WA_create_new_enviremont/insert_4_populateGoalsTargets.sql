/****** Goal_Type   ******/
select * from [Goal_Type]
GO
SET IDENTITY_INSERT  [Goal_Type] ON
 
insert into [Goal_Type]([Label_En],[Label_Ar],[Descr_En],[Descr_Ar],[url_img],[Descr_Short],[Subindicator_Separator] ,[order_code],[Type_ID],[IS_ACTIVE])
SELECT   *
  FROM [SDGs_Viet].[dbo].[Goal_Type]
  GO
  SET IDENTITY_INSERT  [Goal_Type] OFF
  GO 
/****** Goal    ******/
select * from [Goal]
GO
SET IDENTITY_INSERT  [Goal ] ON
 
insert into [Goal]([Goal_ID],[Goal_DescEn],[Goal_DescAr],[GoalImageEn],[GoalImageAr],[Goal_Code],[Type_ID],[IS_ACTIVE])
SELECT   *  FROM [SDGs_Viet].[dbo].[Goal]
GO
  SET IDENTITY_INSERT  [Goal] OFF
  GO 

/****** Target    ******/
select * from [Target]
GO
SET IDENTITY_INSERT  [Target] ON
 
insert into [Target]([Goal_ID],[Target_DescEn],[Target_DescAr],[Target_Code],[Target_ID],[IS_ACTIVE])
SELECT   *  FROM [SDGs_Viet].[dbo].[Target]
GO
  SET IDENTITY_INSERT  [Target] OFF
  GO 
  
  
 /****** populate CL_UNIT_MEASURE     ******/

insert into [CL_UNIT_MEASURE]
SELECT   *  FROM [SDGs_Viet].[dbo].[CL_UNIT_MEASURE]
    
 /****** populate CL_UNIT_MULT     ******/

insert into [CL_UNIT_MULT]
SELECT   *  FROM [SDGs_Viet].[dbo].[CL_UNIT_MULT]
  
   /****** populate CL_OBS_status     ******/
  
  insert into [CL_OBS_status]
SELECT   *  FROM [SDGs_Viet].[dbo].[CL_OBS_status]



/****** remapping target_id     ******/
 merge [Ind_Code] ic
 using [Target] t
 on replace(t.Target_Code,'code ','')=left(ic.Indicator_NL,  LEN(ic.Indicator_NL) - CHARINDEX('.', REVERSE(ic.Indicator_NL))) 
 when matched 
     THEN UPDATE SET ic.Target_id=t.target_id;
	 
	 

insert Code_Mapping
 select Indicator_code,1,Indicator_Code from Indicator


 insert Code_Mapping
 select subIndicator_code,1,subIndicator_Code from subIndicator