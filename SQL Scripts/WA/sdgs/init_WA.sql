USE [master]
GO


/****** Object:  Database [sdgs_palestina]    Script Date: 17/11/2019 17:00:12 ******/
CREATE DATABASE [sdgs_palestina]
 CONTAINMENT = NONE
 COLLATE ARABIC_CI_AS_KS_WS
GO
USE [sdgs_palestina];
Go

ALTER DATABASE [sdgs_palestina] SET COMPATIBILITY_LEVEL = 140
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [sdgs_palestina].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [sdgs_palestina] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [sdgs_palestina] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [sdgs_palestina] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [sdgs_palestina] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [sdgs_palestina] SET ARITHABORT OFF 
GO

ALTER DATABASE [sdgs_palestina] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [sdgs_palestina] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [sdgs_palestina] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [sdgs_palestina] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [sdgs_palestina] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [sdgs_palestina] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [sdgs_palestina] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [sdgs_palestina] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [sdgs_palestina] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [sdgs_palestina] SET  ENABLE_BROKER 
GO

ALTER DATABASE [sdgs_palestina] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [sdgs_palestina] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [sdgs_palestina] SET TRUSTWORTHY ON 
GO

ALTER DATABASE [sdgs_palestina] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [sdgs_palestina] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [sdgs_palestina] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [sdgs_palestina] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [sdgs_palestina] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [sdgs_palestina] SET  MULTI_USER 
GO

ALTER DATABASE [sdgs_palestina] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [sdgs_palestina] SET DB_CHAINING ON 
GO

ALTER DATABASE [sdgs_palestina] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [sdgs_palestina] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [sdgs_palestina] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [sdgs_palestina] SET QUERY_STORE = OFF
GO

ALTER DATABASE [sdgs_palestina] SET  READ_WRITE 
GO

/****** Object:  Table [dbo].[Ind_Code]    Script Date: 17/11/2019 16:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ind_Code](
	[Target_ID] [varchar](10) NOT NULL,
	[Indicator_NL] [varchar](50) NOT NULL,
	[Indicator_Code] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Indicator]    Script Date: 17/11/2019 16:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Indicator](
	[Indicator_Code] [nchar](50) NOT NULL,
	[Indicator_descEn] [nvarchar](max) NULL,
	[Indicator_descAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Indicator] PRIMARY KEY CLUSTERED 
(
	[Indicator_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CL_AGE]    Script Date: 17/11/2019 16:59:26 ******/
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
/****** Object:  Table [dbo].[CL_DISABILITY_STATUS]    Script Date: 17/11/2019 16:59:26 ******/
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
/****** Object:  Table [dbo].[CL_EDU_LEVEL]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_EmploymentStatus]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_Income]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_InternetSpeed]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_OCCUPATION]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_REF_AREA]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_REPORTING_TYPE]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_SEX]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_URBANISATION]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_ViolenceType]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[CL_YearofSchooling]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[dep_indicator]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[dep_indicator](
	[DEP_ID] [int] NULL,
	[Ind_Code] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[department]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[department](
	[dep_id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[dep_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DIMENSION]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DIMENSION](
	[SEQUENCE] [int] NOT NULL,
	[NAME] [varchar](250) NOT NULL,
	[LABEL] [nvarchar](250) NOT NULL,
	[LABEL_ENG] [varchar](250) NOT NULL,
	[TABLE_NAME] [varchar](250) NULL,
	[CODE] [varchar](250) NULL,
	[DESCRIPTION] [nvarchar](250) NULL,
	[DESCRIPTION_ENG] [varchar](250) NULL,
	[IS_TIME] [tinyint] NOT NULL,
	[INT_CODE] [tinyint] NOT NULL,
 CONSTRAINT [PK_DIMENSION] PRIMARY KEY CLUSTERED 
(
	[NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Goal]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Goal_Type](
    [Type_ID] [smallint] NOT NULL,
    [Label_En] [nvarchar](50) NULL,
    [Label_Ar] [nvarchar](50) NULL,
    [Descr_En] [nvarchar](max) NULL,
    [Descr_Ar] [nvarchar](max) NULL,
    [url_img] [nvarchar](max) NULL,
    [Subindicator_Separator] [nchar](1) NULL,
    [Descr_Short] [nvarchar](20) NULL,
	[ORDER_CODE] [smallint] NULL
) ON [PRIMARY] 
GO
CREATE TABLE [dbo].[Target_Type](
	[Type_ID] [smallint] NOT NULL,
	[Label_En] [nvarchar](50) NULL,
	[Label_Ar] [nvarchar](50) NULL,
	[Descr_En] [nvarchar](max) NULL,
	[Descr_Ar] [nvarchar](max) NULL
) ON [PRIMARY] 
GO
CREATE TABLE [dbo].[Goal](
	[Goal_ID] [int] NOT NULL,
	[Goal_DescEn] [nvarchar](max) NULL,
	[Goal_DescAr] [nvarchar](max) NULL,
	[GoalImageEn] [nvarchar](max) NULL,
	[GoalImageAr] [nvarchar](max) NULL,
	[Type_ID] [smallint] NULL,
	[Goal_Code] [nvarchar](10) NULL,
 CONSTRAINT [PK_Goal] PRIMARY KEY CLUSTERED 
(
	[Goal_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Code_Mapping]    Script Date: 13/12/2020 11:43:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Code_Mapping](
	[Code] [varchar](50) NOT NULL,
	[Goal_Type] [smallint] NOT NULL,
	[Value] [varchar](50) NOT NULL,
 CONSTRAINT [Code_Mapping_PK] PRIMARY KEY CLUSTERED 
(
	[Code] ASC,
	[Goal_Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subindicator]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subindicator](
	[Indicator_Code] [nchar](50) NULL,
	[Subindicator_Code] [nvarchar](50) NULL,
	[Subindicator_DescEn] [nvarchar](max) NULL,
	[Subindicator_DescAr] [nvarchar](max) NULL,
	[Is_Uploaded] [int] NOT NULL,
	[Is_Valid] [int] NOT NULL,
	[Dimensions] [nchar](50) NULL,
	[Series] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SUBINDICATOR_DATA]    Script Date: 17/11/2019 16:59:27 ******/
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
/****** Object:  Table [dbo].[Target]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Target](
	[Goal_ID] [int] NOT NULL,
	[Target_ID] [nchar](10) NOT NULL,
	[Target_DescEn] [nvarchar](max) NULL,
	[Target_DescAr] [nvarchar](max) NULL,
	[Target_Code] [nvarchar](10)  NULL,
 CONSTRAINT [PK_Target] PRIMARY KEY CLUSTERED 
(
	[Target_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TIME_PERIOD]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TIME_PERIOD](
	[TIME_PERIOD] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_INDICATOR]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_INDICATOR](
	[User_ID] [int] NOT NULL,
	[IND_CODE] [nchar](50) NOT NULL,
 CONSTRAINT [PK_User_IND] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC,
	[IND_CODE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 17/11/2019 16:59:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](64) NOT NULL,
	[password] [varchar](128) NOT NULL,
	[Role_ID] [int] NOT NULL
) ON [PRIMARY]
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
/****** Object:  Table [dbo].[CL_UNIT_MEASURE]    Script Date: 03/11/2020 10:55:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[CL_UNIT_MULT]    Script Date: 03/11/2020 10:55:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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


create view V_GOAL as
select g.Goal_ID, g.Goal_CODE,g.Type_ID, CONCAT(gt.Label_En, g.Goal_CODE, ' - ', g.Goal_DescEn) as Goal_DescEn, CONCAT(gt.Label_Ar, g.Goal_CODE, ' - ', ISNULL(g.Goal_DescAr,'')) as Goal_DescAr
FROM Goal g inner join Goal_type gt on g.Type_ID=gt.Type_ID
GO

create view V_TARGET as
SELECT Goal_ID, Target_ID,Target_CODE, CONCAT('TARGET ', Target_CODE, ' - ', Target_DescEn) as Target_DescEn, CONCAT('TARGET ', Target_CODE, ' - ', ISNULL(Target_DescAr,'')) as Target_DescAr
  FROM Target
GO


create view [dbo].[V_INDICATOR] as
	select I.Indicator_Code, cm.Value as Indicator_CodeValue,cm.Goal_Type, IC.Indicator_NL+' '+I.Indicator_descEn as Indicator_descEn, IC.Indicator_NL+' '+ISNULL(I.Indicator_descAr,'') as Indicator_descAr, IC.Target_ID
FROM Indicator I 
INNER JOIN Ind_Code IC ON ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON 	t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on t.Goal_ID = g.Goal_ID
INNER JOIN Code_mapping cm on 	cm.Code = I.Indicator_Code and cm.Goal_Type =g.Type_ID 
GO

CREATE OR ALTER VIEW V_SUBINDICATOR as
SELECT s.Indicator_Code, s.Subindicator_Code, cm.Value as Subindicator_CodeValue,cm.Goal_Type,IC.Target_ID, CONCAT(cm.Value, ' - ', s.Subindicator_DescEn) as Subindicator_DescEn, CONCAT(cm.Value, ' - ', ISNULL(s.Subindicator_DescAr,'')) as Subindicator_DescAr, s.Dimensions, s.Is_Valid
FROM Subindicator s
INNER JOIN Indicator I ON s.Indicator_Code = i.Indicator_Code
INNER JOIN Ind_Code IC ON ic.Indicator_Code = i.Indicator_Code
INNER JOIN Target T ON t.Target_ID = ic.Target_ID
INNER JOIN GOAL G on t.Goal_ID = g.Goal_ID
INNER JOIN Code_mapping cm on cm.Code = s.Subindicator_Code and cm.Goal_Type =g.Type_ID
INNER JOIN SUBINDICATOR_DATA SD on s.indicator_Code = sd.Indicator_Code and s.Subindicator_Code = sd.Subindicator_Code
GO