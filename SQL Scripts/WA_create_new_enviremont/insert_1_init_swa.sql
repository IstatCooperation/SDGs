USE [SDGs_viet]
GO
GO
INSERT [dbo].[Roles] ([Role_ID], [Role_Name]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Roles] ([Role_ID], [Role_Name]) VALUES (2, N'User')
GO
 
INSERT [dbo].[Users] ([User_ID], [username], [password], [Role_ID]) VALUES (1, N'user1', N'24c9e15e52afc47c225b757e7bee1f9d', 1)
GO
INSERT [dbo].[Users] ([User_ID], [username], [password], [Role_ID]) VALUES (53, N'user4', N'3F02EBE3D7929B091E3D8CCFDE2F3BC6', 1)
GO
INSERT [dbo].[Users] ([User_ID], [username], [password], [Role_ID]) VALUES (55, N'user20', N'10880C7F4E4209EEDA79711E1EA1723E', 1)
GO
INSERT [dbo].[Users] ([User_ID], [username], [password], [Role_ID]) VALUES (51, N'user2', N'7E58D63B60197CEB55A1C487989A3720', 2)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO



INSERT INTO [dbo].[swa_properties]
           ([id]
           ,[name]
           ,[label]
		   ,[description]
		   ,[description2]
           ,[url_logo]
		   ,[url_website]
           ,[secondary_language]
           ,[secondary_language_code])
     VALUES
           (1,'GSO','GSO','Viet Nam','GENERAL STATISTICS OFFICE' ,'images/logowhite129.png','https://www.gso.gov.vn','Tiếng Việt','vn')
GO