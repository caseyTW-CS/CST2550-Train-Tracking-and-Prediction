-- ===================================================================================================================================
-- Create database template for Azure SQL Database and Azure Synapse Analytics Database
--
-- This script will only run in the context of the master database. To manage this database in 
-- SQL Server Management Studio, either connect to the created database, or connect to master.
--
-- SQL Database is a relational database-as-a-service that makes tier-1 capabilities easily accessible 
-- for cloud architects and developers by delivering predictable performance, scalability, business 
-- continuity, data protection and security, and near-zero administration � all backed by the power 
-- and reach of Microsoft Azure.
--
-- SQL Database is available in the following service tiers: GeneralPurpose, Basic, Standard, Premium , DataWarehouse, Web (Retired) 
-- and Business (Retired).
-- General Purpose service tier is a default service tier in Azure SQL Database that is designed for most of the generic workloads. 
-- If you need a fully managed database engine with 99.99% SLA with storage latency between 5 and 10 ms that match Azure SQL 
-- IaaS in most of the cases, General Purpose tier is the option for you.
-- Standard is the go-to option for getting started with cloud-designed business applications and 
-- offers mid-level performance and business continuity features. Performance objectives for Standard 
-- deliver predictable per minute transaction rates.
--
-- See https://go.microsoft.com/fwlink/p/?LinkId=306622 for more information about Azure SQL Database.
-- 
-- See https://go.microsoft.com/fwlink/?LinkId=787140 for more information about Azure Synapse Analytics.
--
-- See https://go.microsoft.com/fwlink/p/?LinkId=402063 for more information about CREATE DATABASE for Azure SQL Database.
--
-- See https://go.microsoft.com/fwlink/?LinkId=787139 for more information about CREATE DATABASE for Azure Synapse Analytics Database.
-- ===================================================================================================================================

 
--Create user database
CREATE DATABASE Users;
GO

USE Users;
GO

--Create Info table
CREATE TABLE userInfo (
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	userName NVARCHAR(20) NOT NULL,
	userPass NVARCHAR(20) NOT NULL,
	userAge SMALLINT NOT NULL,
	userEmail NVARCHAR(100) NULL,
	userPhone NVARCHAR(20) NOT NULL,
);
GO

--Sample data:
INSERT INTO userInfo (userID, userName, userPass, userAge, userEmail, userPhone)
VALUES
	('00000001', 'John Smith', 'Pass123', '45', 'joSmith@email.com', '079-203-6522'),
	('00000002', 'Jane Smith', 'Pass456', '47', 'jaSmith@email.com', '079-293-4444');
GO



--=====================================
-- COMMENTED OUT FOR TESTING
--====================================


--Stores all the train services we track
--CREATE TABLE Train (
--	trainID INT IDENTITY(1,1) PRIMARY KEY,
--	trainNumber NVARCHAR(20) NOT NULL,
	--e.g. commuter, freight etc:
--	trainType NVARCHAR(50) NOT NULL,
	--e.g. thameslink, great northern:
--	trainCompany NVARCHAR(100) NOT NULL,
--	trainCarriages SMALLINT NOT NULL
--);
--GO

--Sample data:
--INSERT INTO Train (trainNumber, trainType, trainCompany, trainCarriages)
--VALUES
--	('TL1234', 'Commuter', 'Thameslink', 8),
--	('GN5678', 'Commuter', 'Great Northern', 6);
--GO

--Stores all the stations in the system
--CREATE TABLE Station (
--	stationID INT IDENTITY(1,1) PRIMARY KEY,
--	stationName NVARCHAR(100) NOT NULL,
	--for storing the size of the platforms:
--	stationSize NVARCHAR(20) NOT NULL,
	--for the platform the train will be stopping on:
--	stationPlatform SMALLINT NOT NULL
--);
--GO

--Sample data:
--INSERT INTO Station (stationName, stationSize, stationPlatform)
--VALUES
--	('London Kings Cross', 'Large', 5),
--	('Cambridge', 'Medium', 2),
--	('Stevenage', 'Small', 1);
--GO

--Stores each journey a train makes, links trains to their departure/arrival stations
--CREATE TABLE Journey (
--	journeyID INT IDENTITY(1,1) PRIMARY KEY,
	--which train is doing this journey:
--	trainID INT NOT NULL FOREIGN KEY REFERENCES Train(trainID),
	--where its coming from and going to:
--	departureStationID INT NOT NULL FOREIGN KEY REFERENCES Station(stationID),
--	arrivalStationID INT NOT NULL FOREIGN KEY REFERENCES Station(stationID),
	--what the timetable says:
--	scheduledDeparture DATETIME NOT NULL,
--	scheduledArrival DATETIME NOT NULL,
	--what actually happened or our prediction:
--	actualDeparture DATETIME NULL,
--	actualArrival DATETIME NULL,
	--how many minutes late the train is, starts at 0 obviously:
--	currentDelayMinutes SMALLINT DEFAULT 0,
	--on time, slight delay, delayed etc:
--	journeyStatus NVARCHAR(50) DEFAULT 'On Time'
--);
--GO

--Sample data:
--INSERT INTO Journey (trainID, departureStationID, arrivalStationID, scheduledDeparture, scheduledArrival, currentDelayMinutes, journeyStatus)
--VALUES
--	(1, 1, 2, '2026-03-05 08:00:00', '2026-03-05 09:10:00', 0, 'On Time'),
--	(2, 2, 3, '2026-03-05 09:00:00', '2026-03-05 09:30:00', 5, 'Slight Delay (5 mins)');
--GO
