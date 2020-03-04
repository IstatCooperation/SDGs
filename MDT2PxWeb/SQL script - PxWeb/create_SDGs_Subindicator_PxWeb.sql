USE [pxweb_sdgs]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDICATORDATA](
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
) 
GO

