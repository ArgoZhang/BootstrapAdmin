USE [BootstrapAdmin]
GO

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

/****** Object:  StoredProcedure [dbo].[Proc_SaveUsers]    Script Date: 11/11/2016 08:51:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Drop PROCEDURE Proc_SaveUsers
GO
-- =============================================
-- Author:		LiuChun
-- Create date: 2016-11-10
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SaveUsers]
	-- Add the parameters for the stored procedure here
	@userName varchar(50),
	@password varchar(50),
	@passSalt varchar(50),
	@displayName nvarchar(50),
	@approvedBy nvarchar(50) = null,
	@description nvarchar(500)
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	declare @approvedTime datetime
	if @approvedBy is not null set @approvedTime = GETDATE()

	begin
		if(not exists (select 1 from Users Where UserName = @userName))
		begin
			begin tran
				Insert Into Users (UserName, [Password], PassSalt, DisplayName, RegisterTime, ApprovedBy, ApprovedTime, [Description]) values (@userName, @password, @passSalt, @displayName, GETDATE(), @approvedBy, @approvedTime, @description)
				insert into UserRole (UserID, RoleID) select @@IDENTITY, ID from Roles where RoleName = N'Default'
			commit tran
		end
	end
END
GO

Drop PROCEDURE Proc_RejectUsers
GO
-- =============================================
-- Author:		Argo
-- Create date: 2018-09-07
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[Proc_RejectUsers]
	-- Add the parameters for the stored procedure here
	@id int,
	@rejectedBy varchar(50),
	@rejectedReason nvarchar(50)
	WITH ENCRYPTION
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET XACT_ABORT ON;
    -- Insert statements for procedure here
	begin
		declare @registerTime datetime
		declare @userName varchar(50)
		declare @displayName nvarchar(50)
		select @registerTime = Registertime, @userName = UserName, @displayName = DisplayName from Users where ID = @id

		if @userName is not null 
		begin
			begin tran
				insert into RejectUsers (UserName, DisplayName, RegisterTime, RejectedBy, RejectedTime, RejectedReason) values (@userName, @displayName, @registerTime, @rejectedBy, GETDATE(), @rejectedReason)
				delete from UserRole where UserId = @id
				delete from users where ID = @id
			commit tran
		end
	end
END
GO
