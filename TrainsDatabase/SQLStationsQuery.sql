-- ===================================================================================================================================
-- Create database template for Azure SQL Database and Azure Synapse Analytics Database
--
-- This script will only run in the context of the master database. To manage this database in 
-- SQL Server Management Studio, either connect to the created database, or connect to master.
--
-- SQL Database is a relational database-as-a-service that makes tier-1 capabilities easily accessible 
-- for cloud architects and developers by delivering predictable performance, scalability, business 
-- continuity, data protection and security, and near-zero administration — all backed by the power 
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

 
 CREATE DATABASE Stations;
 GO

 USE Stations;
 CREATE TABLE stationInfo (
    stationName NVARCHAR(100) PRIMARY KEY,
    stationLocation NVARCHAR(100) NOT NULL,
    stationSize NVARCHAR(20) NOT NULL,
    stationPlatforms SMALLINT NOT NULL,
    stationLONG FLOAT NOT NULL,
    stationLAT FLOAT NOT NULL
);
GO

-- Sample data:
INSERT INTO stationInfo (stationName, stationLocation, stationSize, stationPlatforms, stationLONG, stationLAT)
VALUES
    ('Reading', '51.4587, -0.9719', 'Large', 7),
    ('Twyford', '51.4754, -0.8614', 'Small', 2),
    ('Maidenhead', '51.5183, -0.7177', 'Medium', 2),
    ('Taplow', '51.5238, -0.6882', 'Small', 1),
    ('Burnham', '51.5238, -0.6527', 'Small', 1),
    ('Slough', '51.5113, -0.5950', 'Medium', 4),
    ('Langley', '51.5057, -0.5541', 'Small', 1),
    ('Iver', '51.5100, -0.5050', 'Small', 1),
    ('West Drayton', '51.5096, -0.4472', 'Small', 2),
    ('Hayes & Harlington', '51.5069, -0.4225', 'Small', 2),
    ('Heathrow Terminal 2 & 3', '51.4713, -0.4524', 'Large', 2),
    ('Heathrow Terminal 4', '51.4585, -0.4466', 'Medium', 2),
    ('Heathrow Terminal 5', '51.4733, -0.4889', 'Large', 2),
    ('Southall', '51.5057, -0.3776', 'Small', 2),
    ('Hanwell', '51.5100, -0.3394', 'Small', 1),
    ('West Ealing', '51.5132, -0.3228', 'Small', 2),
    ('Ealing Broadway', '51.5152, -0.3017', 'Medium', 3),
    ('Paddington', '51.5154, -0.1755', 'Large', 6),
    ('Bond Street', '51.5142, -0.1494', 'Large', 3),
    ('Tottenham Court Road', '51.5165, -0.1299', 'Large', 4),
    ('Farringdon', '51.5203, -0.1051', 'Medium', 2),
    ('City Thameslink', '51.5141, -0.1028', 'Small', 1),
    ('Liverpool Street', '51.5183, -0.0823', 'Large', 4),
    ('Whitechapel', '51.5197, -0.0594', 'Medium', 3);
GO

