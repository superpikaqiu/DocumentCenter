/*
 Navicat Premium Data Transfer

 Source Server         : 本地sqlserver
 Source Server Type    : SQL Server
 Source Server Version : 14001000
 Source Host           : localhost:1433
 Source Catalog        : onlyoffice
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 14001000
 File Encoding         : 65001

 Date: 11/02/2020 18:39:17
*/


-- ----------------------------
-- Table structure for DocumentHistories
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentHistories]') AND type IN ('U'))
	DROP TABLE [dbo].[DocumentHistories]
GO

CREATE TABLE [dbo].[DocumentHistories] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [DocumentInfoId] int DEFAULT ((0)) NOT NULL,
  [Version] int  NOT NULL,
  [PreviousVersion] int  NOT NULL,
  [ServerVersion] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Key] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Path] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Changes] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ChangesUrl] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [UserId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [UserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [AutoSave] bit DEFAULT ((0)) NOT NULL,
  [CreateTime] datetime DEFAULT ('1900-01-01T00:00:00.000') NOT NULL
)
GO

ALTER TABLE [dbo].[DocumentHistories] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for DocumentInfoes
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[DocumentInfoes]') AND type IN ('U'))
	DROP TABLE [dbo].[DocumentInfoes]
GO

CREATE TABLE [dbo].[DocumentInfoes] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [FileName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [FileType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CurrentVersion] int DEFAULT ((0)) NOT NULL,
  [Key] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Path] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExternalFlag] bit DEFAULT ((0)) NOT NULL,
  [ExternalCallbackUrl] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExternalID] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExternalStartEditUrl] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExternalEndEditUrl] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
  [CreateUserID] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateUserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [RelativePath] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [LastUpdateTime] datetime  NULL,
  [ExternalFilePath] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [IsEditing] bit DEFAULT ((0)) NOT NULL,
  [Size] bigint DEFAULT ((0)) NOT NULL,
  [ModifyTime] datetime DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
  [ModifyUserId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ModifyUserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExtendData] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [SystemType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ExternalSaveOpinionUrl] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[DocumentInfoes] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for FileUsers
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[FileUsers]') AND type IN ('U'))
	DROP TABLE [dbo].[FileUsers]
GO

CREATE TABLE [dbo].[FileUsers] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [FileId] int  NOT NULL,
  [UserId] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreationTime] datetime  NOT NULL,
  [Permission] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[FileUsers] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for PersonalFiles
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalFiles]') AND type IN ('U'))
	DROP TABLE [dbo].[PersonalFiles]
GO

CREATE TABLE [dbo].[PersonalFiles] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [DocumentID] int  NOT NULL,
  [CreateUserID] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime  NOT NULL,
  [CreateType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [FileType] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [FileName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateUserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ModifyUserID] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ModifyUserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ModifyTime] datetime DEFAULT ('1900-01-01T00:00:00.000') NOT NULL
)
GO

ALTER TABLE [dbo].[PersonalFiles] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for Users
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type IN ('U'))
	DROP TABLE [dbo].[Users]
GO

CREATE TABLE [dbo].[Users] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [CreationTime] datetime  NOT NULL,
  [UserID] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [UserName] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Users] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Primary Key structure for table DocumentHistories
-- ----------------------------
ALTER TABLE [dbo].[DocumentHistories] ADD CONSTRAINT [PK_dbo.DocumentHistories] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table DocumentInfoes
-- ----------------------------
ALTER TABLE [dbo].[DocumentInfoes] ADD CONSTRAINT [PK_dbo.DocumentInfoes] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table FileUsers
-- ----------------------------
ALTER TABLE [dbo].[FileUsers] ADD CONSTRAINT [PK_dbo.FileUsers] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table PersonalFiles
-- ----------------------------
ALTER TABLE [dbo].[PersonalFiles] ADD CONSTRAINT [PK_dbo.PersonalFiles] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

