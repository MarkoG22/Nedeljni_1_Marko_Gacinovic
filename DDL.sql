-- Dropping the tables before recreating the database in the order depending how the foreign keys are placed.
IF OBJECT_ID('tblProject', 'U') IS NOT NULL DROP TABLE tblProject;
IF OBJECT_ID('tblWorker', 'U') IS NOT NULL DROP TABLE tblWorker;
IF OBJECT_ID('tblManager', 'U') IS NOT NULL DROP TABLE tblManager;
IF OBJECT_ID('tblAdmin', 'U') IS NOT NULL DROP TABLE tblAdmin;
IF OBJECT_ID('tblUser', 'U') IS NOT NULL DROP TABLE tblUser;
IF OBJECT_ID('tblSector', 'U') IS NOT NULL DROP TABLE tblSector;
IF OBJECT_ID('tblPosition', 'U') IS NOT NULL DROP TABLE tblPosition;
if OBJECT_ID('vwWorker','v') IS NOT NULL DROP VIEW vwWorker;
if OBJECT_ID('vwAdmin','v') IS NOT NULL DROP VIEW vwAdmin;
if OBJECT_ID('vwManager','v') IS NOT NULL DROP VIEW vwManager;

-- Checks if the database already exists.
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CompanyDB')
CREATE DATABASE CompanyDB;
GO

USE CompanyDB
CREATE TABLE tblSector (
	SectorID INT IDENTITY(1,1) PRIMARY KEY		NOT NULL,
	SectorName VARCHAR (40) UNIQUE 				NOT NULL,
	SectorDescription VARCHAR (40)
);

USE CompanyDB
CREATE TABLE tblPosition (
	PositionID INT IDENTITY(1,1) PRIMARY KEY		NOT NULL,
	PositionName VARCHAR (40) UNIQUE				NOT NULL,
	PositionDescription VARCHAR (40)
);

USE CompanyDB
CREATE TABLE tblUser(
	UserID INT IDENTITY(1,1) PRIMARY KEY 	NOT NULL,
	FirstName VARCHAR (40)					NOT NULL,
	LastName VARCHAR (40)					NOT NULL,
	JMBG VARCHAR (13) UNIQUE				NOT NULL,
	Gender CHAR								NOT NULL,
	Residence VARCHAR (40)					NOT NULL,
	MarriageStatus VARCHAR (13)				NOT NULL,
	Username VARCHAR (40) UNIQUE			NOT NULL,
	UserPassword VARCHAR (40)				NOT NULL,
);

USE CompanyDB
CREATE TABLE tblWorker (
	WorkerID INT IDENTITY(1,1) PRIMARY KEY		NOT NULL,
	YearsOfService INT DEFAULT 0				NOT NULL,
	Salary VARCHAR (40),
	EducationDegree VARCHAR (3)					NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID) NOT NULL,
	SectorID INT FOREIGN KEY REFERENCES tblSector(SectorID) NOT NULL,
	PositionID INT FOREIGN KEY REFERENCES tblPosition(PositionID),
);

USE CompanyDB
CREATE TABLE tblManager (
	ManagerID INT IDENTITY(1,1) PRIMARY KEY		NOT NULL,
	Email VARCHAR (40)							NOT NULL,
	ReservedPassword VARCHAR (40)				NOT NULL,
	LevelOfResponsibility	CHAR,
	SuccessfulProjects INT DEFAULT 0,
	Salary VARCHAR (40),
	OfficeNumber VARCHAR (20),
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID) NOT NULL,
);

USE CompanyDB
CREATE TABLE tblAdmin (
	AdminID INT IDENTITY(1,1) PRIMARY KEY		NOT NULL,
	ExpirationDate DATE							NOT NULL,
	AdminType VARCHAR (40)						NOT NULL,
	UserID INT FOREIGN KEY REFERENCES tblUser(UserID) NOT NULL,
);

USE CompanyDB
CREATE TABLE tblProject (
	ProjectID INT IDENTITY(1,1) PRIMARY KEY		NOT NULL,
	ProjectName VARCHAR (40)					NOT NULL,
	ProjectDescription VARCHAR (40),
	ClientName VARCHAR (60)						NOT NULL,
	ContractDate DATE							NOT NULL,
	ContractManager  VARCHAR (60)				NOT NULL,
	ProjectStartDate DATE						NOT NULL,
	ProjectDeadline DATE						NOT NULL,
	HourlyRate INT								NOT NULL,
	Realisation CHAR DEFAULT 0					NOT NULL,
	ManagerID INT FOREIGN KEY REFERENCES tblManager(ManagerID) NOT NULL,
);

GO
CREATE VIEW vwWorker AS
	SELECT	tblUser.*, 
			tblWorker.YearsOfService, tblWorker.Salary, tblWorker.EducationDegree, tblWorker.SectorID, 
			tblWorker.PositionID, tblWorker.WorkerID 
	FROM	tblUser, tblWorker
	WHERE	tblUser.UserID = tblWorker.UserID

GO
CREATE VIEW vwManager AS
	SELECT	tblUser.*, 
			tblManager.Email, tblManager.ReservedPassword, tblManager.Salary, tblManager.LevelOfResponsibility, 
			tblManager.SuccessfulProjects, tblManager.OfficeNumber, tblManager.ManagerID
	FROM	tblUser, tblManager 
	WHERE	tblUser.UserID = tblManager.UserID

GO
CREATE VIEW vwAdmin AS
	SELECT	tblUser.*, tblAdmin.ExpirationDate, tblAdmin.AdminType, tblAdmin.AdminID
	FROM	tblUser, tblAdmin
	WHERE	tblUser.UserID = tblAdmin.UserID