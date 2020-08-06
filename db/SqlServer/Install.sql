USE [master]
GO

if exists (select * from sys.databases where name = 'BootstrapAdmin')
	drop database [BootstrapAdmin]

Create database [BootstrapAdmin]
GO

ALTER DATABASE [BootstrapAdmin] SET RECOVERY SIMPLE
GO

ALTER DATABASE [BootstrapAdmin] SET AUTO_SHRINK ON 
GO

USE [BootstrapAdmin]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](16) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[PassSalt] [varchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[RegisterTime] [datetime] NOT NULL,
	[ApprovedTime] [datetime] NULL,
	[ApprovedBy] [varchar](50) NULL,
	[Description] [nvarchar](500) NOT NULL,
	[RejectedBy] [varchar](50) NULL,
	[RejectedTime] [datetime] NULL,
	[RejectedReason] [nvarchar](50) NULL,
	[Icon] [varchar](50) NULL,
	[Css] [varchar](50) NULL,
	[App] [varchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码盐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'PassSalt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'DisplayName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'RegisterTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'批复时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'ApprovedTime'
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'RoleID'
GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
 CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'GroupID'
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles', @level2type=N'COLUMN',@level2name=N'RoleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles', @level2type=N'COLUMN',@level2name=N'Description'
GO
/****** Object:  Table [dbo].[RoleGroup]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
 CONSTRAINT [PK_RoleGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleGroup', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleGroup', @level2type=N'COLUMN',@level2name=N'RoleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoleGroup', @level2type=N'COLUMN',@level2name=N'GroupID'
GO

/****** Object:  Table [dbo].[RoleApp]    Script Date: 02/24/2019 14:56:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RoleApp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AppID] [varchar](50) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_RoleApp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Notifications]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](50) NOT NULL,
	[RegisterTime] [datetime] NOT NULL,
	[ProcessTime] [datetime] NULL,
	[ProcessBy] [nvarchar](50) NULL,
	[ProcessResult] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 标示未处理 1 标示已处理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Notifications', @level2type=N'COLUMN',@level2name=N'Status'
GO

/****** Object:  Default [DF_Notifications_Status]    Script Date: 11/12/2016 15:49:11 ******/
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_Status]  DEFAULT ((0)) FOR [Status]
GO

/****** Object:  Table [dbo].[Navigations]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Navigations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Order] [int] NOT NULL,
	[Icon] [varchar](50) NULL,
	[Url] [varchar](4000) NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Target] [varchar](10) NOT NULL,
	[IsResource] [int] NOT NULL,
	[Application] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Navigations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Default [DF_Navigations_ParentId]    Script Date: 11/12/2016 15:49:11 ******/
ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_ParentId]  DEFAULT ((0)) FOR [ParentId]
GO
/****** Object:  Default [DF_Navigations_Order]    Script Date: 11/12/2016 15:49:11 ******/
ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_Order]  DEFAULT ((0)) FOR [Order]
GO
/****** Object:  Default [DF_Navigations_Icon]    Script Date: 11/12/2016 15:49:11 ******/
ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_Icon]  DEFAULT ('none') FOR [Icon]
GO
/****** Object:  Default [DF_Navigations_Category]    Script Date: 11/12/2016 15:49:11 ******/
ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_Category]  DEFAULT ((0)) FOR [Category]
GO

ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_IsResource]  DEFAULT ((0)) FOR [IsResource]
GO

ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_Application]  DEFAULT (N'BA') FOR [Application]
GO

ALTER TABLE [dbo].[Navigations] ADD  CONSTRAINT [DF_Navigations_Target]  DEFAULT ('_self') FOR [Target]
GO

SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NavigationRole]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NavigationRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NavigationID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_NavigationRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Logs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CRUD] [nvarchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[Ip] [varchar](15) NOT NULL,
	[Browser] [varchar](50) NULL,
	[OS] [varchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[RequestUrl] [nvarchar](500) NOT NULL,
	[RequestData] [nvarchar](max) NULL,
	[UserAgent] [varchar](2000) NULL,
    [Referer]   [varchar](2000) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'CRUD'
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupCode] [nvarchar](50) NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Groups', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Groups', @level2type=N'COLUMN',@level2name=N'GroupName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Groups', @level2type=N'COLUMN',@level2name=N'Description'
GO
/****** Object:  Table [dbo].[Exceptions]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Exceptions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AppDomainName] [varchar](50) NOT NULL,
	[ErrorPage] [varchar](50) NOT NULL,
	[UserID] [varchar](50) NULL,
	[UserIp] [varchar](15) NULL,
	[ExceptionType] [nvarchar](max) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[StackTrace] [nvarchar](max) NULL,
	[LogTime] [datetime] NOT NULL,
	[Category] [varchar](50) NULL,
 CONSTRAINT [PK_Exceptions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Dicts]    Script Date: 11/12/2016 15:49:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dicts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](2000) NOT NULL,
	[Define] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Dict] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dicts', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典种类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dicts', @level2type=N'COLUMN',@level2name=N'Category'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dicts', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dicts', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0表示系统使用，1表示自定义' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dicts', @level2type=N'COLUMN',@level2name=N'Define'
GO

/****** Object:  Default [DF_Dicts_Define]    Script Date: 11/12/2016 15:49:11 ******/
ALTER TABLE [dbo].[Dicts] ADD  CONSTRAINT [DF_Dicts_Define]  DEFAULT ((1)) FOR [Define]
GO

/****** Object:  Table [dbo].[Messages]    Script Date: 11/14/2016 13:59:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Messages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](500) NOT NULL,
	[From] [varchar](50) NOT NULL,
	[To] [varchar](50) NOT NULL,
	[SendTime] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Flag] [int] NOT NULL,
	[IsDelete] [int] NOT NULL,
	[Label] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_Mark]  DEFAULT ((0)) FOR [Flag]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_Label]  DEFAULT ((0)) FOR [Label]
GO

/****** Object:  Table [dbo].[Tasks]    Script Date: 11/16/2016 15:40:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TaskName] [varchar](500) NOT NULL,
	[AssignName] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[TaskTime] [int] NOT NULL,
	[TaskProgress] [float] NOT NULL,
	[AssignTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tasks', @level2type=N'COLUMN',@level2name=N'TaskName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分配人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tasks', @level2type=N'COLUMN',@level2name=N'AssignName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'完成任务人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tasks', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务所需时间(天)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tasks', @level2type=N'COLUMN',@level2name=N'TaskTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'完成进度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tasks', @level2type=N'COLUMN',@level2name=N'TaskProgress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分配时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tasks', @level2type=N'COLUMN',@level2name=N'AssignTime'
GO

/****** Object:  Table [dbo].[RejectUsers]    Script Date: 09/08/2018 15:34:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[RejectUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[RegisterTime] [datetime] NOT NULL,
	[RejectedBy] [varchar](50) NOT NULL,
	[RejectedTime] [datetime] NOT NULL,
	[RejectedReason] [nvarchar](50) NULL,
 CONSTRAINT [PK_RejectUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RejectUsers', @level2type=N'COLUMN',@level2name=N'UserName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RejectUsers', @level2type=N'COLUMN',@level2name=N'DisplayName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RejectUsers', @level2type=N'COLUMN',@level2name=N'RegisterTime'
GO

/****** Object:  Table [dbo].[LoginLogs]    Script Date: 03/03/2019 20:05:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LoginLogs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	[Ip] [varchar](15) NOT NULL,
	[OS] [varchar](50) NULL,
	[Browser] [varchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Result] [nvarchar](50) NOT NULL,
	[UserAgent] [varchar](2000) NULL,
 CONSTRAINT [PK_LoginLogs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[ResetUsers]    Script Date: 03/05/2019 12:28:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ResetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[Reason] [nvarchar](500) NOT NULL,
	[ResetTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ResetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Traces]    Script Date: 03/16/2019 18:37:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Traces](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[Ip] [varchar](15) NOT NULL,
	[Browser] [varchar](2000) NULL,
	[OS] [varchar](2000) NULL,
	[City] [nvarchar](50) NULL,
	[RequestUrl] [nvarchar](2000) NOT NULL,
	[UserAgent] [varchar](2000) NULL,
    [Referer] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Traces] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[DBLogs]    Script Date: 09/22/2019 22:06:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DBLogs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[SQL] [nvarchar](max) NOT NULL,
	[LogTime] [datetime] NOT NULL,
 CONSTRAINT [PK_DBLogs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
