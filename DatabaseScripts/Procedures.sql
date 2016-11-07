SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Drop PROCEDURE Proc_DeleteUsers
GO
-- =============================================
-- Author:		Argo Zhang
-- Create date: 2016-09-06
-- Description:	
-- =============================================
Create PROCEDURE Proc_DeleteUsers
	-- Add the parameters for the stored procedure here
	@ids varchar(max)
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	declare @sql varchar(max)
	set @sql = 'Delete from UserRole where UserID in (' + @ids + ');'
	set @sql += 'delete from UserGroup where UserID in (' + @ids + ');'
	set @sql += 'delete from Users where ID in (' + @ids + ');'
	exec(@sql)
END
GO

Drop PROCEDURE Proc_DeleteRoles
GO
-- =============================================
-- Author:		LiuChun
-- Create date: 2016-11-07
-- Description:	
-- =============================================
Create PROCEDURE Proc_DeleteRoles
	-- Add the parameters for the stored procedure here
	@ids varchar(max)
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	declare @sql varchar(max)
	set @sql = 'delete from UserRole where RoleID in (' + @ids + ');'
	set @sql += 'delete from RoleGroup where RoleID in (' + @ids + ');'
	set @sql += 'delete from NavigationRole where RoleID in (' + @ids + ');'
	set @sql += 'delete from Roles where ID in (' + @ids + ');'
	exec(@sql)
END
GO


Drop PROCEDURE Proc_DeleteGroups
GO
-- =============================================
-- Author:		LiuChun
-- Create date: 2016-11-07
-- Description:	
-- =============================================
Create PROCEDURE Proc_DeleteGroups
	-- Add the parameters for the stored procedure here
	@ids varchar(max)
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	declare @sql varchar(max)
	set @sql = 'delete from UserGroup where GroupID in (' + @ids + ');'
	set @sql += 'delete from RoleGroup where GroupID in (' + @ids + ');'
	set @sql += 'delete from Groups where ID in (' + @ids + ');'
	exec(@sql)
END
GO

Drop PROCEDURE Proc_DeleteMenus
GO
-- =============================================
-- Author:		LiuChun
-- Create date: 2016-11-07
-- Description:	
-- =============================================
Create PROCEDURE Proc_DeleteMenus
	-- Add the parameters for the stored procedure here
	@ids varchar(max)
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	declare @sql varchar(max)
	set @sql = 'delete from NavigationRole where NavigationID in (' + @ids + ');'
	set @sql += 'delete from Navigations where ID in (' + @ids + ');'
	exec(@sql)
END
GO