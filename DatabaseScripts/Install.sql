USE [master]
GO

Create database [ExtendedChecker]
GO

ALTER DATABASE [ExtendedChecker] SET RECOVERY SIMPLE
GO

ALTER DATABASE [ExtendedChecker] SET AUTO_SHRINK ON 
GO

USE [ExtendedChecker]
GO

/****** Object:  Table [dbo].[Rules]    Script Date: 9/1/2016 7:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Terminals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ClientIP] [varchar](15) NOT NULL,
	[ClientPort] [int] NOT NULL,
	[ServerIP] [varchar](15) NOT NULL,
	[ServerPort] [int] NOT NULL,
	[DeviceIP] [varchar](15) NOT NULL,
	[DevicePort] [int] NOT NULL,
	[DatabaseName] [varchar](50) NULL,
	[DatabaseUserName] [varchar](50) NULL,
	[DatabasePassword] [varchar](50) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Terminal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TerminalRuleConfig]    Script Date: 09/06/2016 14:31:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerminalRuleConfig](
	[TerminalID] [int] NOT NULL,
	[RuleID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScanInventory]    Script Date: 09/06/2016 14:31:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ScanInventory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BarCode] [varchar](32) NOT NULL,
	[TerminalID] [int] NOT NULL,
	[ScanTime] [datetime] NOT NULL,
	[BarCodeType] [int] NOT NULL,
 CONSTRAINT [PK_ScanInventory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rules]    Script Date: 09/06/2016 14:31:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rules](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Memo] [nvarchar](2000) NULL,
	[Interval] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_Rules] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Rules_Enabled]    Script Date: 09/06/2016 14:31:10 ******/
ALTER TABLE [dbo].[Rules] ADD  CONSTRAINT [DF_Rules_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
/****** Object:  Default [DF_Terminals_Satus]    Script Date: 09/06/2016 14:31:10 ******/
ALTER TABLE [dbo].[Terminals] ADD  CONSTRAINT [DF_Terminals_Satus]  DEFAULT ((0)) FOR [Status]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Argo Zhang
-- Create date: 2016-09-06
-- Description:	
-- =============================================
CREATE PROCEDURE Proc_StartTerminal
	-- Add the parameters for the stored procedure here
	@tId int, 
	@rId int
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	delete from TerminalRuleConfig where TerminalID = @tId;
	insert into TerminalRuleConfig (TerminalID, RuleID) values (@tId, @rId);
	update Terminals set Status = 1 where Id = @tId;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Argo Zhang
-- Create date: 2016-09-06
-- Description:	
-- =============================================
CREATE PROCEDURE Proc_StopTerminal
	-- Add the parameters for the stored procedure here
	@tId int
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	delete from TerminalRuleConfig where TerminalID = @tId;
	update Terminals set Status = 0 where Id = @tId;
END
GO
