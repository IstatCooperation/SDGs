USE [master]
GO

/****** Object:  Database [SDGs_WA]    Script Date: 17/11/2019 17:00:12 ******/
IF DB_ID (N'[SDGs_WA]') IS NOT NULL
DROP DATABASE [SDGs_WA]
GO

/****** Object:  Database [SDGs_WA]    Script Date: 17/11/2019 17:00:12 ******/
CREATE DATABASE [SDGs_WA]
 CONTAINMENT = NONE
 COLLATE ARABIC_CI_AS_KS_WS
GO
USE [SDGs_WA];
Go

/****** Object:  Table [dbo].[CL_AGE]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_AGE](
	[AGE] [nchar](50) NULL,
	[AGE_DESC_EN] [nvarchar](max) NULL,
	[AGE_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_DISABILITY_STATUS]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_DISABILITY_STATUS](
	[DISABILITY_STATUS] [nchar](10) NULL,
	[DISABILITY_STATUS_DESC_EN] [nvarchar](max) NULL,
	[DISABILITY_STATUS_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_EDU_LEVEL]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_EDU_LEVEL](
	[EDUCATION_LEV] [nvarchar](50) NULL,
	[EDUCATION_LEV_DESC_EN] [nvarchar](max) NULL,
	[EDUCATION_LEV_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_EmploymentStatus]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_EmploymentStatus](
	[EmploymentStatus] [nvarchar](50) NULL,
	[EmploymentStatus_DESC_En] [nvarchar](max) NULL,
	[EmploymentStatus_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_Income]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_Income](
	[Income] [nvarchar](50) NULL,
	[Income_DESC_EN] [nvarchar](max) NULL,
	[Income_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_InternetSpeed]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_InternetSpeed](
	[InternetSpeed] [nvarchar](50) NULL,
	[InternetSpeed_DESC_EN] [nvarchar](max) NULL,
	[InternetSpeed_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_OBS_STATUS]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CL_OBS_STATUS](
	[OBS_STATUS] [varchar](100) NOT NULL,
	[OBS_STATUS_DESC_EN] [nvarchar](max) NULL,
	[OBS_STATUS_DESC_AR] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[OBS_STATUS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CL_OCCUPATION]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_OCCUPATION](
	[OCCUPATION] [nvarchar](50) NULL,
	[OCCUPATION_DESC_EN] [nvarchar](max) NULL,
	[OCCUPATION_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_REF_AREA]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_REF_AREA](
	[REF_AREA] [int] NULL,
	[REF_AREA_TYPE] [nvarchar](50) NULL,
	[REF_AREA_DESC_EN] [nvarchar](max) NULL,
	[REF_AREA_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_REPORTING_TYPE]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_REPORTING_TYPE](
	[REPORTING_TYPE] [nchar](10) NULL,
	[REPORTING_TYPE_DESC_EN] [nvarchar](50) NULL,
	[REPORTING_TYPE_DESC_AR] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_SEX]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_SEX](
	[SEX] [nchar](10) NULL,
	[SEX_DESC_EN] [nvarchar](50) NULL,
	[SEX_DESC_AR] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_UNIT_MEASURE]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CL_UNIT_MEASURE](
	[UNIT_MEASURE] [varchar](100) NOT NULL,
	[UNIT_MEASURE_DESC_EN] [nvarchar](max) NULL,
	[UNIT_MEASURE_DESC_AR] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UNIT_MEASURE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CL_UNIT_MULT]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CL_UNIT_MULT](
	[UNIT_MULT] [varchar](100) NOT NULL,
	[UNIT_MULT_DESC_EN] [nvarchar](max) NULL,
	[UNIT_MULT_DESC_AR] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[UNIT_MULT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CL_URBANISATION]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_URBANISATION](
	[URBANISATION] [nchar](10) NULL,
	[URBANISATION_DESC_EN] [nvarchar](max) NULL,
	[URBANISATION_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_ViolenceType]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_ViolenceType](
	[ViolenceType] [nvarchar](50) NULL,
	[ViolenceType_DESC_EN] [nvarchar](max) NULL,
	[ViolenceType_DESC_AR] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CL_YearofSchooling]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CL_YearofSchooling](
	[code] [nchar](10) NULL,
	[Desc_En] [nvarchar](max) NULL,
	[Desc_Ar] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[dep_indicator]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[dep_indicator](
	[DEP_ID] [int] NULL,
	[Ind_Code] [varchar](10) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[department]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[department](
	[dep_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[dep_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DIMENSION]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DIMENSION](
	[SEQUENCE] [int] NOT NULL,
	[NAME] [varchar](250) NOT NULL,
	[LABEL] [varchar](250) NOT NULL,
	[TABLE_NAME] [varchar](250) NULL,
	[CODE] [varchar](250) NULL,
	[DESCRIPTION] [varchar](250) NULL,
	[IS_TIME] [tinyint] NOT NULL DEFAULT ((0)),
	[INT_CODE] [tinyint] NOT NULL DEFAULT ((0)),
	[LABEL_ENG] [varchar](250) NULL,
	[DESCRIPTION_ENG] [varchar](250) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Goal]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goal](
	[Goal_ID] [int] NOT NULL,
	[Goal_DescEn] [nvarchar](max) NULL,
	[Goal_DescAr] [nvarchar](max) NULL,
	[GoalImageEn] [nvarchar](max) NULL,
	[GoalImageAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Goal] PRIMARY KEY CLUSTERED 
(
	[Goal_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ind_Code]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ind_Code](
	[Target_ID] [varchar](10) NOT NULL,
	[Indicator_NL] [varchar](10) NOT NULL,
	[Indicator_Code] [varchar](10) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Indicator]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Indicator](
	[Indicator_Code] [nchar](10) NOT NULL,
	[Indicator_descEn] [nvarchar](max) NULL,
	[Indicator_descAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Indicator] PRIMARY KEY CLUSTERED 
(
	[Indicator_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Roles](
	[Role_ID] [int] NOT NULL,
	[Role_Name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Role_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Subindicator]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subindicator](
	[Indicator_Code] [nchar](10) NULL,
	[Subindicator_Code] [nvarchar](50) NULL,
	[Subindicator_DescEn] [nvarchar](max) NULL,
	[Subindicator_DescAr] [nvarchar](max) NULL,
	[Is_Uploaded] [int] NOT NULL DEFAULT ((0)),
	[Is_Valid] [int] NOT NULL DEFAULT ((1)),
	[Dimensions] [nchar](50) NULL,
	[Series] [nvarchar](50) NULL,
	[UNIT_MULT] [nvarchar](50) NULL,
	[UNIT_MEASURE] [nvarchar](50) NULL,
	[OBS_STATUS] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SUBINDICATOR_DATA]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SUBINDICATOR_DATA](
	[INDICATOR_CODE] [nchar](50) NULL,
	[SUBINDICATOR_CODE] [nchar](50) NULL,
	[TIME_PERIOD] [int] NULL,
	[REF_AREA] [nchar](50) NULL,
	[SEX] [nchar](50) NULL,
	[AGE] [nchar](50) NULL,
	[URBANISATION] [nchar](50) NULL,
	[EDUCATION_LEV] [nchar](50) NULL,
	[DISABILITY_STATUS] [nchar](50) NULL,
	[OCCUPATION] [nchar](50) NULL,
	[Sector] [nchar](50) NULL,
	[AreaOfStudy] [nchar](50) NULL,
	[TypeOfViolence] [nchar](50) NULL,
	[EmploymentStatus] [nchar](50) NULL,
	[PregnancyStatus] [nchar](50) NULL,
	[WorkingInjuryStatus] [nchar](50) NULL,
	[PovertyStatus] [nchar](50) NULL,
	[YearsOfSchooling] [nchar](50) NULL,
	[MobileNetworkTechnology] [nchar](50) NULL,
	[EcosystemType] [nchar](50) NULL,
	[InternetSpeed] [nchar](50) NULL,
	[OBS_VALUE] [float] NULL,
	[UNIT_MULT] [nchar](50) NULL,
	[UNIT_MEASURE] [nchar](50) NULL,
	[OBS_STATUS] [nchar](50) NULL,
	[Income] [nchar](50) NULL,
	[REPORTING_TYPE] [nchar](50) NULL,
	[TIME_DETAIL] [nchar](50) NULL,
	[COMMENT_OBS] [ntext] NULL,
	[BASE_PER] [nchar](50) NULL,
	[SOURCE_DETAIL] [ntext] NULL,
	[SOURCE_DETAIL_AR] [ntext] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Target]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Target](
	[Goal_ID] [int] NULL,
	[Target_ID] [nchar](10) NOT NULL,
	[Target_DescEn] [nvarchar](max) NULL,
	[Target_DescAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Target] PRIMARY KEY CLUSTERED 
(
	[Target_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TIME_PERIOD]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TIME_PERIOD](
	[TIME_PERIOD] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User_INDICATOR]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_INDICATOR](
	[User_ID] [int] NOT NULL,
	[IND_CODE] [nchar](10) NOT NULL,
 CONSTRAINT [PK_User_IND] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC,
	[IND_CODE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](64) NOT NULL,
	[password] [varchar](128) NOT NULL,
	[Role_ID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_username] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[V_GOAL]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[V_GOAL] as
select distinct G.Goal_ID,
	CONCAT('GOAL ', G.Goal_ID, ' - ', ISNULL(Goal_DescEn, Goal_DescAr)) as Goal_DescEn,
	CONCAT('الهدف ', G.Goal_ID, ' - ', ISNULL(Goal_DescAr, Goal_DescEn)) as Goal_DescAr
FROM Goal G
	join Target T on G.Goal_ID = T.Goal_ID
	join Ind_code IC on T.Target_ID = IC.Target_ID
	join SUBINDICATOR_DATA SD on IC.Indicator_Code = SD.INDICATOR_CODE
	join Subindicator S on S.Subindicator_Code = SD.SUBINDICATOR_CODE
WHERE SD.OBS_VALUE is not null and S.Is_Valid = 1

GO
/****** Object:  View [dbo].[V_INDICATOR]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[V_INDICATOR] as
select distinct i1.Indicator_Code,
	i2.Indicator_NL+' '+ISNULL(Indicator_descEn, Indicator_descAr) as Indicator_descEn,
	i2.Indicator_NL+' '+ISNULL(Indicator_descAr, Indicator_descEn) as Indicator_descAr, Target_ID
FROM INDICATOR i1 join Ind_Code i2 on i1.Indicator_Code = i2.Indicator_Code
	join SUBINDICATOR_DATA SD on i1.Indicator_Code = SD.INDICATOR_CODE
	join Subindicator S on S.Subindicator_Code = SD.SUBINDICATOR_CODE
WHERE SD.OBS_VALUE is not null and S.Is_Valid = 1

GO
/****** Object:  View [dbo].[V_SUBINDICATOR]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[V_SUBINDICATOR] as
SELECT distinct S.Indicator_Code, S.Subindicator_Code,
	CONCAT(S.Subindicator_Code, ' - ', ISNULL(Subindicator_DescEn, Subindicator_DescAr)) as Subindicator_DescEn,
	CONCAT(S.Subindicator_Code, ' - ', ISNULL(Subindicator_DescAr, Subindicator_DescEn)) as Subindicator_DescAr, Dimensions, Is_Valid
FROM Subindicator S
	join SUBINDICATOR_DATA SD on S.Subindicator_Code = SD.SUBINDICATOR_CODE
WHERE OBS_VALUE is not null and Is_Valid = 1

GO
/****** Object:  View [dbo].[V_TARGET]    Script Date: 27/02/2020 16:54:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[V_TARGET] as
SELECT distinct T.Goal_ID, T.Target_ID,
	CONCAT('TARGET ', T.Target_ID, ' - ', ISNULL(Target_DescEn, Target_DescAr)) as Target_DescEn,
	CONCAT('الغاية ', T.Target_ID, ' - ', ISNULL(Target_DescAr, Target_DescEn)) as Target_DescAr
  FROM Target T
	join Ind_code IC on T.Target_ID = IC.Target_ID
	join SUBINDICATOR_DATA SD on IC.Indicator_Code = SD.INDICATOR_CODE
	join Subindicator S on S.Subindicator_Code = SD.SUBINDICATOR_CODE
WHERE SD.OBS_VALUE is not null and S.Is_Valid = 1

GO
ALTER TABLE [dbo].[User_INDICATOR]  WITH CHECK ADD  CONSTRAINT [FK_User_INDICATOR_User] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[User_INDICATOR] CHECK CONSTRAINT [FK_User_INDICATOR_User]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([Role_ID])
REFERENCES [dbo].[Roles] ([Role_ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
