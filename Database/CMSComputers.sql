 /* Check if database already exists and delete it if it does exist*/
use master
go
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'CMSComputers') 
BEGIN

DROP DATABASE CMSComputers
END
GO

CREATE DATABASE CMSComputers
GO

USE [CMSComputers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

/**********************************************************************************/
/******************************* Tables *******************************************/
/**********************************************************************************/

create table status(
	StatusID int primary key not null,
	statusName varchar(100) not null
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Employee](
	[EmployeeID] 	[int] IDENTITY (1000,1)	NOT NULL,
	[FirstName]			[varchar](50)			NOT NULL,
	[LastName]			[varchar](100)			NOT NULL,
	[LocalPhone]			[varchar](10)			NULL,
	[EmailAddress]			[varchar](100)			Null,
	[UserName]			nvarchar(256)			NOT NULL,
	[Password]			[varchar](128)			NOT NULL,
	[Active]			[bit]		NOT NULL	DEFAULT 1,	
	CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ( [EmployeeID] ASC )
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY]
)  ON [PRIMARY]
GO

CREATE TABLE [dbo].[Customer]
(
    [CustomerID]    [int] IDENTITY (1000,1)    NOT NULL,
    [FirstName]     [varchar](50)                   NOT NULL,
    [LastName]      [varchar](100)                  NOT NULL,
	[BusinessName]	[varchar](100)					not null,
    [Address]       [varchar](100)                  NULL,
    [City]          [varchar](30)                   NULL,
    [State]         [varchar](2)                       NULL,
    [Zip]           [varchar](5)                       NULL,
    [LocalPhone]    [varchar](10)                   NOT NULL,
    [EmailAddress]  [varchar](100)                  NULL,
	[Active]		[bit] NOT NULL	DEFAULT 1,
    
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ( [CustomerID] ASC )
    
        WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY]
)  ON [PRIMARY]
GO

CREATE TABLE [dbo].[Role](
	[RoleID]			[varchar](30) NOT NULL,
	[Description]		[varchar](50) NOT NULL
	
	CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ( [RoleID] ASC )
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY]
)  ON [PRIMARY]
GO

CREATE TABLE [dbo].[RoleAssignments](
    [EmployeeID] 	[int] 	NOT NULL,
    [RoleID]		[varchar](30)	NOT NULL,
    [Active]		[bit]	NOT NULL DEFAULT 1

    CONSTRAINT [PK_assignments] PRIMARY KEY ( [RoleID],[EmployeeID] ASC )
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY]
)  ON [PRIMARY]
GO

create TABLE Bid
(
	BidID int Identity(1000,1) Not NULL,
	BidDate smalldatetime null,
	ExpectedDate smalldatetime null,
	CustomerID int not null,
	Description varchar(100) not null,
	EmployeeID int not null,
	Status char(1) default 'A',
	ContractType char(1) not null,
	ContractAmount money null,
	PartsMarkup int null,
	HourlyRate money null

	
	CONSTRAINT [PK_bid] PRIMARY KEY CLUSTERED([BidID])

)ON [PRIMARY]
GO


CREATE TABLE [dbo].[Workorder](
	[WorkorderID] 		int IDENTITY (1000,1)     	NOT NULL,
	[WorkorderDate]		smalldatetime 		NOT NULL,
	[BidID]				int null,
    [ExpectedDate]      smalldatetime       NUll,
    [CustomerID]		int                 NOT NULL,
    [Description]       varchar(150)         not NULL,                   
    [EmployeeID]		int					not null,
    [WorkOrderStatus]   char(1)             not NULL default 'A',
	ContractType char(1) null,
    ContractAmount money null,
	PartsMarkup int null,
	HourlyRate money null

    CONSTRAINT [PK_Workorder] PRIMARY KEY CLUSTERED ([WorkorderID] ASC)
    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY]) 
GO

create table WorkorderHours
(
	WorkorderID int not null,
	Date smalldatetime not null,
	EmployeeID int not null,
	HoursBid int not null,
	PricePerHour smallmoney null
	
	CONSTRAINT [PK_WorkorderHours] PRIMARY KEY ( [WorkorderID],[Date], EmployeeID ASC )
	WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, 
	IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY]
)ON [PRIMARY]
GO

create table WorkorderItems 
(
	WorkorderID int not null,
	Date smalldatetime not null,
	ItemName varchar(50) not null,
	EmployeeID int not null,
	Quantity int not null,
	ItemCost smallmoney null,
	ItemSalePrice smallmoney not null
	
	CONSTRAINT [PK_WorkorderItems] PRIMARY KEY ( [WorkorderID],[Date], ItemName ASC )

)ON [PRIMARY]
GO

create table Invoice (
	InvoiceNo int identity (1000,1) primary key not null,
	InvoiceDate smalldatetime not null,
	CustomerID int not null,
	Description varchar(100) not null,
	EmployeeID int not null,
	InvoiceTotal smallmoney not null,
	ContractType char(1) null

)ON [PRIMARY]
GO

create table InvoiceLines (
	InvoiceNo int not null,
	LineNumber int not null,
	LineDescription varchar(100) not null,
	Quantity int not null,
	Price smallmoney not null
	
	CONSTRAINT [PK_InvoiceLines] PRIMARY KEY ( InvoiceNo, LineNumber  ASC )

)ON [PRIMARY]
GO

create table ContractType (
	ContractTypeID char(1) not null primary key,
	ContractTypeName varchar(30),
	Status char(1) default 'A'
)ON [PRIMARY]
GO


/**********************************************************************************/
/******************************* FK Constraints ***********************************/
/**********************************************************************************/


ALTER TABLE dbo.RoleAssignments  WITH NOCHECK ADD  CONSTRAINT [FK_Assignments_RoleID1] FOREIGN KEY(RoleID)
REFERENCES Role (RoleID)
GO
ALTER TABLE RoleAssignments CHECK CONSTRAINT FK_Assignments_RoleID1
GO
/*GOOD*/

ALTER TABLE dbo.RoleAssignments  WITH NOCHECK ADD  CONSTRAINT [FK_Assignments_EmployeeID] FOREIGN KEY(EmployeeID)
REFERENCES dbo.Employee (EmployeeID)
GO
ALTER TABLE dbo.RoleAssignments CHECK CONSTRAINT [FK_Assignments_EmployeeID]
GO
/*GOOD*/

ALTER TABLE dbo.Bid  WITH NOCHECK ADD  CONSTRAINT [FK_Bid_CustomerID] FOREIGN KEY(CustomerID)
REFERENCES dbo.Customer (CustomerID)
GO
ALTER TABLE dbo.Bid CHECK CONSTRAINT [FK_Bid_CustomerID]
GO
/*GOOD*/

ALTER TABLE dbo.Bid  WITH NOCHECK ADD  CONSTRAINT [FK_Bid_EmployeeID] FOREIGN KEY(EmployeeID)
REFERENCES dbo.Employee (EmployeeID)
GO
ALTER TABLE dbo.Bid CHECK CONSTRAINT [FK_Bid_EmployeeID]
GO
/*GOOD*/


ALTER TABLE dbo.Workorder  WITH NOCHECK ADD  CONSTRAINT [FK_Workorder_CustomerID] FOREIGN KEY(CustomerID)
REFERENCES dbo.Customer (CustomerID)
GO
ALTER TABLE dbo.Workorder CHECK CONSTRAINT [FK_Workorder_CustomerID]
GO
/*GOOD*/

ALTER TABLE dbo.Workorder  WITH NOCHECK ADD  CONSTRAINT [FK_Workorder_BidID] FOREIGN KEY(BidID)
REFERENCES dbo.Bid (BidID)
GO
ALTER TABLE dbo.Workorder CHECK CONSTRAINT [FK_Workorder_BidID]


ALTER TABLE dbo.Workorder  WITH NOCHECK ADD  CONSTRAINT [FK_Workorder_EmployeeID] FOREIGN KEY(EmployeeID)
REFERENCES dbo.Employee (EmployeeID)
GO
ALTER TABLE dbo.Workorder CHECK CONSTRAINT [FK_Workorder_EmployeeID]
GO
/*GOOD*/

ALTER TABLE dbo.WorkorderHours  WITH NOCHECK ADD  CONSTRAINT [FK_WorkorderHours_EmployeeID] FOREIGN KEY(EmployeeID)
REFERENCES dbo.Employee (EmployeeID)
GO
ALTER TABLE dbo.WorkorderHours CHECK CONSTRAINT [FK_WorkorderHours_EmployeeID]
GO
/*GOOD*/

ALTER TABLE dbo.WorkorderItems  WITH NOCHECK ADD  CONSTRAINT [FK_WorkorderItems_EmployeeID] FOREIGN KEY(EmployeeID)
REFERENCES dbo.Employee (EmployeeID)
GO
ALTER TABLE dbo.WorkorderItems CHECK CONSTRAINT [FK_WorkorderItems_EmployeeID]
GO
/*GOOD*/

ALTER TABLE dbo.Invoice  WITH NOCHECK ADD  CONSTRAINT [FK_Invoice_CustomerID] FOREIGN KEY(CustomerID)
REFERENCES dbo.Customer (CustomerID)
GO
ALTER TABLE dbo.Invoice CHECK CONSTRAINT [FK_Invoice_CustomerID]
GO
/*GOOD*/

ALTER TABLE dbo.Invoice  WITH NOCHECK ADD  CONSTRAINT [FK_Invoice_EmployeeID] FOREIGN KEY(EmployeeID)
REFERENCES dbo.Employee (EmployeeID)
GO
ALTER TABLE dbo.Invoice CHECK CONSTRAINT [FK_Invoice_EmployeeID]
GO
/*GOOD*/



ALTER TABLE employee
ADD CONSTRAINT AK_username UNIQUE (username); 
GO


/**********************************************************************************/
/******************************* Indexes ******************************************/
/**********************************************************************************/


CREATE NONCLUSTERED INDEX IX_Employee_LastName ON dbo.Employee (LastName)
GO

CREATE NONCLUSTERED INDEX IX_Employee_UserName ON dbo.Employee (UserName)
GO

CREATE NONCLUSTERED INDEX IX_Customer_LastName ON dbo.Customer (LastName)
GO

CREATE NONCLUSTERED INDEX IX_Customer_BusinessName ON dbo.Customer (BusinessName)
GO

CREATE NONCLUSTERED INDEX IX_RoleAssignments_EmployeeID ON dbo.RoleAssignments (EmployeeID)
GO

CREATE NONCLUSTERED INDEX IX_RoleAssignments_RoleID ON dbo.RoleAssignments (RoleID)
GO

CREATE NONCLUSTERED INDEX IX_Bid_CustomerID ON dbo.Bid (CustomerID)
GO

CREATE NONCLUSTERED INDEX IX_Workorder_CustomerID ON dbo.Workorder (CustomerID)
GO

CREATE NONCLUSTERED INDEX IX_Invoice_CustomerID ON dbo.Invoice (CustomerID)
GO


/**********************************************************************************/
/******************************* Stored Procedures ********************************/
/**********************************************************************************/


CREATE PROCEDURE [dbo].[spUpdateEmployee]
	(
	 @EmployeeID					int,
	 @firstname 					varchar(50),
	 @LastName 						varchar(100),
	 @LocalPhone					varchar(10),
	 @EmailAddress					varchar(100),
	 @username						nvarchar(256),
	 @Password						varchar(128),
	 @active						bit,
	 @original_firstname 					varchar(50),
	 @original_LastName 						varchar(100),
	 @original_LocalPhone					varchar(10),
	 @original_EmailAddress					varchar(100),
	 @original_username						varchar(20),
	 @original_Password						varchar(128),
	 @original_active						bit
	 )
AS
	 UPDATE Employee
		SET FirstName  	= @firstname,
			LastName	= @LastName,
			LocalPhone 	= @LocalPhone,
			EmailAddress 	= @EmailAddress,
			UserName 	= @username,
			Password	= @password,
			Active = @active
	  WHERE EmployeeID 	= @EmployeeID
		AND LastName  = @original_LastName
		AND LocalPhone 	= @original_LocalPhone
		AND EmailAddress 	= @original_EmailAddress
		AND UserName 	= @original_username
		AND Password 	= @original_Password
		AND Active 	= @original_active
	RETURN @@ROWCOUNT
GO	

CREATE PROCEDURE [dbo].[spInsertEmployee]
	(	 
		 @firstname 					varchar(50),
		 @LastName 						varchar(100),
		 @LocalPhone					varchar(10),
		 @EmailAddress					varchar(100),
		 @username						nvarchar(256),
		 @Password						varchar(128)
	 )
	 as
	 begin
	 SET NOCOUNT ON 
	 insert into dbo.employee
	 (
		FirstName,
		LastName, 
		LocalPhone, 
		EmailAddress,
		UserName,
		Password
		)
	values
	(
		@FirstName,
		@LastName,
		@LocalPhone,
		@EmailAddress,
		@UserName,
		@Password
	)
	end
go

create Procedure spUpdateCustomer
	(
	@CustomerID int,
	@FirstName varchar(50),
	@LastName varchar(100),
	@BusinessName varchar(100),
	@Address varchar(100),
	@City varchar(30),
	@State varchar(2),
	@Zip varchar(5),
	@LocalPhone varchar(10),
	@Active bit,
	@EmailAddress varchar(100),
	@original_FirstName varchar(50),
	@original_LastName varchar(100),
	@original_BusinessName varchar(100),
	@original_Address varchar(100),
	@original_City varchar(30),
	@original_State varchar(2),
	@original_Zip varchar(5),
	@original_LocalPhone varchar(10),
	@original_EmailAddress varchar(100),
	@original_Active bit
	)
	as 
	SET NOCOUNT ON
	update Customer 
	set FirstName = @FirstName,
		LastName = @LastName,
		BusinessName = @BusinessName,
		Address = @Address,
		City = @City,
		State = @State,
		Zip = @Zip,
		LocalPhone = @LocalPhone,
		EmailAddress = @EmailAddress,
		Active = @Active
	where
		CustomerID = @CustomerID
		and FirstName = @original_FirstName
		and LastName = @original_LastName
		and BusinessName = @original_BusinessName
		and Address = @original_Address
		and City = @original_City
		and State = @original_State
		and Zip = @original_Zip
		and LocalPhone = @original_LocalPhone
		and EmailAddress = @original_EmailAddress
		and Active = @original_Active
	RETURN @@ROWCOUNT
go

CREATE PROCEDURE [dbo].[spInsertCustomer]
	(	 
		 @FirstName 		varchar(50),
		 @LastName 			varchar(100),
		 @BusinessName		varchar(100),
		 @Address			varchar(100),		
		 @City				varchar(30),
		 @State				varchar(2),	
		 @Zip				varchar(9),
		 @LocalPhone		varchar(10),
		 @EmailAddress		varchar(100),
		 @Active			bit
	 )
	 as
	 begin
	 SET NOCOUNT ON 
	 insert into dbo.Customer
	 (
		FirstName,
		LastName, 
		BusinessName, 
		Address,
		City,
		State,
		Zip,
		LocalPhone,
		EmailAddress
		)
	values
	(
		@FirstName,
		@LastName,
		@BusinessName,
		@Address,
		@City,
		@State,
		@Zip,
		@LocalPhone,
		@EmailAddress
	)
	end
go

create Procedure spInsertBid
(
	@BidDate smalldatetime,
	@ExpectedDate smalldatetime,
	@CustomerID int,
	@Description varchar(100),
	@EmployeeID int,
	@Status char(1),
	@ContractType char(1),
	@ContractAmount money,
	@PartsMarkup int,
	@HourlyRate money
	)
as
	 begin
	 SET NOCOUNT ON 
	 insert into dbo.Bid
	 (
	 biddate,
	 ExpectedDate,
	CustomerID ,
	Description ,
	EmployeeID, 
	Status ,
	ContractType, 
	ContractAmount ,
	PartsMarkup ,
	HourlyRate)
	values
	(@biddate,
	@ExpectedDate ,
	@CustomerID ,
	@Description ,
	@EmployeeID ,
	@Status ,
	@ContractType ,
	@ContractAmount ,
	@PartsMarkup ,
	@HourlyRate)
	end
go

create Procedure spUpdateWorkorder
(
	@workorderID int,
	@workorderdate smalldatetime,
	@bidID int,
	@ExpectedDate smalldatetime,
	@CustomerID int,
	@Description varchar(150),
	@EmployeeID int,
	@workorderstatus char(1),
	@ContractType char(1),
	@ContractAmount money,
	@PartsMarkup int,
	@HourlyRate money,
	@Original_workorderdate smalldatetime,
	@Original_bidID int,
	@Original_ExpectedDate smalldatetime,
	@Original_CustomerID int,
	@Original_Description varchar(150),
	@Original_EmployeeID int,
	@Original_workorderstatus char(1),
	@Original_ContractType char(1),
	@Original_ContractAmount money,
	@Original_PartsMarkup int,
	@Original_HourlyRate money
)
as
update workorder 

set		
		workorderdate = @workorderdate,
		bidid = @bidID,
		expecteddate = @ExpectedDate,
		customerid = @CustomerID,
		description = @Description,
		EmployeeID = @EmployeeID,
		workorderStatus = @workorderStatus,
		ContractType = @ContractType,
		ContractAmount = @ContractAmount,
		PartsMarkup = @PartsMarkup,
		HourlyRate = @HourlyRate
where
		workorderid = @workorderid
		and workorderdate = @Original_workorderdate
		and bidid = @original_Bidid
		and expecteddate = @original_ExpectedDate
		and customerid = @original_CustomerID
		and description =@original_Description
		and EmployeeID = @original_EmployeeID
		and workorderStatus = @original_workorderStatus
		and ContractType = @original_ContractType
		and ContractAmount = @original_ContractAmount
		and PartsMarkup = @original_PartsMarkup
		and HourlyRate = @original_HourlyRate
		RETURN @@ROWCOUNT
go

create Procedure spUpdateBidStatus
(
	@bidID int,
	@Status char(1)
)
as
update bid
set	status = @Status where
BidID = @bidID
RETURN @@ROWCOUNT
go

create Procedure spUpdateBid
(
	@bidID int,
	@BidDate smalldatetime,
	@ExpectedDate smalldatetime,
	@CustomerID int,
	@Description varchar(100),
	@EmployeeID int,
	@Status char(1),
	@ContractType char(1),
	@ContractAmount money,
	@PartsMarkup int,
	@HourlyRate money,
	@Original_BidDate smalldatetime,
	@Original_ExpectedDate smalldatetime,
	@Original_CustomerID int,
	@Original_Description varchar(100),
	@Original_EmployeeID int,
	@Original_Status char(1),
	@Original_ContractType char(1),
	@Original_ContractAmount money,
	@Original_PartsMarkup int,
	@Original_HourlyRate money
)
as
update bid 
set		biddate = @BidDate,
		expecteddate = @ExpectedDate,
		customerid = @CustomerID,
		description = @Description,
		EmployeeID = @EmployeeID,
		Status = @Status,
		ContractType = @ContractType,
		ContractAmount = @ContractAmount,
		PartsMarkup = @PartsMarkup,
		HourlyRate = @HourlyRate
where
		BidID = @bidID 
		and biddate = @original_BidDate
		and expecteddate = @original_ExpectedDate
		and customerid = @original_CustomerID
		and description =@original_Description
		and EmployeeID = @original_EmployeeID
		and Status = @original_Status
		and ContractType = @original_ContractType
		and ContractAmount = @original_ContractAmount
		and PartsMarkup = @original_PartsMarkup
		and HourlyRate = @original_HourlyRate
		RETURN @@ROWCOUNT
go

create Procedure spInsertWorkorder
(
	@WorkorderDate smalldatetime,
	@BidID int,
	@ExpectedDate smalldatetime,
	@CustomerID int,
	@Description varchar(100),
	@EmployeeID int,
	@WorkorderStatus char(1),
	@ContractType char(1),
	@ContractAmount money,
	@PartsMarkup int,
	@HourlyRate money
	)
as
	 begin
	 SET NOCOUNT ON 
INSERT INTO [dbo].[Workorder]
           ([WorkorderDate]
           ,[BidID]
           ,[ExpectedDate]

           ,[CustomerID]
           ,[Description]
           ,[EmployeeID]
           ,[WorkOrderStatus]
           ,[ContractType]
		   ,ContractAmount
		   ,PartsMarkup
		   ,HourlyRate)
     VALUES
        (   @WorkorderDate,
	@BidID,
	@ExpectedDate,
	@CustomerID,
	@Description,
	@EmployeeID,
	@WorkorderStatus ,
	@ContractType ,
	@ContractAmount ,
	@PartsMarkup ,
	@HourlyRate )
	end
GO

create procedure sp_CheckPassword
@username varchar(20), @password varchar(128)
as
select count(*) from employee where userName = @username and Password = @password
go

create procedure sp_UpdatePassword
(@username varchar(20), @newpassword varchar(128), @oldpassword varchar(128))
as
begin
update employee set password = @newpassword where username = @username and 
	password = @oldpassword
return @@rowcount
end
go

create procedure sp_GetRoles
(@username varchar(20))
as
select r.Roleid from employee e inner join RoleAssignments a on e.employeeid = a.employeeid
	inner join role r on a.roleid = r.roleid where e.username = @username
go

create procedure sp_GetEmployeeInfo
(@username varchar(20))
as
SELECT [EmployeeID]
      ,[FirstName]
      ,[LastName]
      ,[LocalPhone]
      ,[EmailAddress]
      ,[UserName]
      ,[Password]
      ,[Active]
  FROM [dbo].[Employee]
 where username = @username
go


/**********************************************************************************/
/******************************* Test Data  ***************************************/
/**********************************************************************************/


INSERT [dbo].[Employee] ([FirstName], [LastName], [LocalPhone], [EmailAddress], [UserName], [Password])
	VALUES ('Chris', 'Sheehan', '3192134542', 'csheehan1975@gmail.com', 'user', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
	('Jesse', 'Sheehan', '3195555678', 'jessesheehan03@gmail.com', 'user1', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
	('John', 'Smith', '3195550012', 'john.smith@email.com', 'user2', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8')
GO

INSERT [dbo].[Customer] ([FirstName],[LastName], [BusinessName],[Address],[City],[State],[Zip],[LocalPhone],[EmailAddress]) 
	VALUES ('Jeff', 'Johnson', 'JJ Company', '33 10th Ave', 'Cedar Rapids', 'IA', '52403', '5551113333', 'jj@jjco.com'),
	('Randy', 'Watson', 'Watson EnterPrises', '820 33rd Ave SW', 'Cedar Rapids', 'IA', '52404', '8008887777', 'Randy@watsonenterprises.com'),
	('Sam', 'Smith', 'Smith, Sam', '88 8th Ave', 'Cedar Rapids', 'IA', '52403', '8881113883', 'Sam@smithco.com.com')
GO

INSERT [dbo].[Role] ([RoleID], [Description]) VALUES ('Administrator','Program Administrator'),('User','Program User')
go

Insert [dbo].[RoleAssignments] ([EmployeeID], [RoleID]) VALUES (1000, 'Administrator'),(1001, 'User'),(1002, 'User')
go

INSERT INTO [dbo].[Bid] ([BidDate],[ExpectedDate],[CustomerID],[Description],[EmployeeID],[Status], contractType, contractamount, PartsMarkup, HourlyRate)
     VALUES (getdate(), '12-31-2015', 1000, 'Replace laptop wil new dell 17 inch, transfer files', 1000, 'A', 'T', 0, 20, 45.00),
			(getdate(), '12-15-2015', 1001, 'Set up new laptop transfer files from old to new', 1001, 'A', 'N', 200.00, 10, 45.00),
			(getdate(), '12-16-2015', 1002, '5 new PCs, transfer files', 1002, 'A', 'C', 1500.00,0,0)
GO

insert into ContractType (contracttypeid, ContractTypeName, Status)
values ('C',  'Contract - Fixed Amount', 'A'), ('N', 'Not to Exceed', 'A'), ('T', 'Time and Materials', 'A')
go

insert into status (StatusID, statusName)
values (0, 'Not Active'), (1, 'Active')
go












	













