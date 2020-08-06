CREATE TABLE Users (
    ID             SERIAL PRIMARY KEY,
    UserName       VARCHAR (16) NOT NULL,
    Password       VARCHAR (50)  NOT NULL,
    PassSalt       VARCHAR (50)  NOT NULL,
    DisplayName    VARCHAR (50)  NOT NULL,
    RegisterTime   DATE      NOT NULL,
    ApprovedTime   DATE,
    ApprovedBy     VARCHAR (50),
    Description    VARCHAR (500) NOT NULL,
    RejectedBy     VARCHAR (50),
    RejectedTime   DATE,
    RejectedReason VARCHAR (50),
    Icon           VARCHAR (50),
    Css            VARCHAR (50),
	App			   VARCHAR (50)
);

CREATE TABLE UserRole (
	ID 				SERIAL PRIMARY KEY,
	UserID 			INT NOT NULL,
	RoleID 			INT NOT NULL
);

CREATE TABLE UserGroup(
	ID   			SERIAL PRIMARY KEY,
	UserID 			INT NOT NULL,
	GroupID 		INT NOT NULL
);

CREATE TABLE Roles(
	ID 				SERIAL PRIMARY KEY,
	RoleName 		VARCHAR (50) NOT NULL,
	Description 	VARCHAR (500) NULL
);

CREATE TABLE RoleGroup(
	ID 				SERIAL PRIMARY KEY,
	RoleID 			INT NOT NULL,
	GroupID 		INT NOT NULL
);

CREATE TABLE RoleApp(
	ID 				SERIAL PRIMARY KEY,
	AppID 			VARCHAR (50) NOT NULL,
	RoleID 			INT NOT NULL
);

CREATE TABLE Notifications(
	ID 				SERIAL PRIMARY KEY,
	Category 		VARCHAR (50) NOT NULL,
	Title 			VARCHAR (50) NOT NULL,
	Content 		VARCHAR (50) NOT NULL,
	RegisterTime 	DATE NOT NULL,
	ProcessTime 	DATE NULL,
	ProcessBy 		VARCHAR (50) NULL,
	ProcessResult 	VARCHAR (50) NULL,
	Status 			VARCHAR (50) DEFAULT 0
);

CREATE TABLE Navigations(
	ID 				SERIAL PRIMARY KEY,
	ParentId 		INT DEFAULT 0,
	Name 			VARCHAR (50) NOT NULL,
	"order"   		INT NOT NULL DEFAULT 0,
	Icon 			VARCHAR (50) DEFAULT 'fa fa-fa',
	Url 			VARCHAR (4000) NULL,
	Category 		VARCHAR (50) DEFAULT 0,
	Target 			VARCHAR (10) DEFAULT '_self',
	IsResource 		INT DEFAULT 0,
	Application 	VARCHAR (200) DEFAULT 0
);

CREATE TABLE NavigationRole(
	ID 				SERIAL PRIMARY KEY,
	NavigationID 	INT NOT NULL,
	RoleID 			INT NOT NULL
);

CREATE TABLE Logs(
	ID 				SERIAL PRIMARY KEY,
	CRUD 			VARCHAR (50) NOT NULL,
	UserName 		VARCHAR (50) NOT NULL,
	LogTime			DATE NOT NULL,
	Ip				VARCHAR (15) NOT NULL,
	Browser			VARCHAR (50) NULL,
	OS				VARCHAR (50) NULL,
	City			VARCHAR (50) NULL,
	RequestUrl		VARCHAR (500) NOT NULL,
	RequestData		TEXT NULL,
	UserAgent		VARCHAR (2000) NULL,
    Referer         VARCHAR (2000) 
);

CREATE TABLE Groups(
	ID 				SERIAL PRIMARY KEY,
    GroupCode       VARCHAR (50) NOT NULL,
	GroupName 		VARCHAR (50) NOT NULL,
	Description 	VARCHAR (500) NULL
);

CREATE TABLE Exceptions(
	ID 				SERIAL PRIMARY KEY,
	AppDomainName 	VARCHAR (50) NOT NULL,
	ErrorPage 		VARCHAR (50) NOT NULL,
	UserID 			VARCHAR (50) NULL,
	UserIp 			VARCHAR (15) NULL,
	ExceptionType 	TEXT NOT NULL,
	Message 		TEXT NOT NULL,
	StackTrace 		TEXT NULL,
	LogTime 		DATE NOT NULL,
	Category		VARCHAR (50) NULL
);

CREATE TABLE Dicts(
	ID 				SERIAL PRIMARY KEY,
	Category 		VARCHAR (50) NOT NULL,
	Name 			VARCHAR (50) NOT NULL,
	Code 			VARCHAR (2000) NOT NULL,
	Define 			INT NOT NULL DEFAULT 1
);

CREATE TABLE Messages(
	ID 				SERIAL PRIMARY KEY,
	Title 			VARCHAR (50) NOT NULL,
	Content 		VARCHAR (500) NOT NULL,
	"from" 			VARCHAR (50) NOT NULL,
	"to" 			VARCHAR (50) NOT NULL,
	SendTime 		DATE NOT NULL,
	Status 			VARCHAR (50) NOT NULL,
	Flag 			INT DEFAULT 0,
	IsDelete 		INT DEFAULT 0,
	Label 			VARCHAR (50)
);

CREATE TABLE Tasks(
	ID 				SERIAL PRIMARY KEY,
	TaskName 		VARCHAR (500) NOT NULL,
	AssignName 		VARCHAR (50) NOT NULL,
	UserName 		VARCHAR (50) NOT NULL,
	TaskTime 		INT NOT NULL,
	TaskProgress 	INT NOT NULL,
	AssignTime 		DATE NOT NULL
);

CREATE TABLE RejectUsers(
	ID 				SERIAL PRIMARY KEY,
	UserName 		VARCHAR (50) NOT NULL,
	DisplayName 	VARCHAR (50) NOT NULL,
	RegisterTime 	DATE NOT NULL,
	RejectedBy 		VARCHAR (50) NOT NULL,
	RejectedTime 	DATE NOT NULL,
	RejectedReason 	VARCHAR (50) NULL
);

CREATE TABLE RejectUsers(
	ID 				SERIAL PRIMARY KEY,
	UserName 		VARCHAR (50) NOT NULL,
	LoginTime 	    DATE NOT NULL,
	Ip 	            VARCHAR NOT NULL,
	OS		        VARCHAR (50) NULL,
	Browser	        VARCHAR (50) NULL,
	City 	        VARCHAR (50) NULL,
	Result 	        VARCHAR (50) NOT NULL,
	UserAgent		VARCHAR (2000) NULL
);

CREATE TABLE ResetUsers(
	ID 				SERIAL PRIMARY KEY,
	UserName 		VARCHAR (50) NOT NULL,
	DisplayName 	VARCHAR (50) NOT NULL,
	Reason			VARCHAR (500) NOT NULL,
	ResetTime		DATE NOT NULL
);

CREATE TABLE Traces(
	ID 				SERIAL PRIMARY KEY,
    UserName   		VARCHAR (50)  NOT NULL,
    LogTime    		DATE      NOT NULL,
    IP         		VARCHAR (15)  NOT NULL,
    Browser    		VARCHAR (2000),
    OS         		VARCHAR (2000),
    City       		VARCHAR (50),
    RequestUrl 		VARCHAR (2000) NOT NULL,
	UserAgent		VARCHAR (2000) NULL,
    Referer         VARCHAR (2000) 
);

CREATE TABLE DBLogs (
    ID       SERIAL PRIMARY KEY,
    UserName VARCHAR (50)   NULL,
    SQL      VARCHAR (2000) NOT NULL,
    LogTime  DATE           NOT NULL
);
