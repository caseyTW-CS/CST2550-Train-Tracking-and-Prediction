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
    stationLAT FLOAT NOT NULL,
    stationLine NVARCHAR(20) NOT NULL
);
GO

-- Sample data:
INSERT INTO stationInfo (stationName, stationSize, stationPlatforms, stationLAT, stationLONG, stationLine)
VALUES
('Reading', 'Large', 7, 51.4587, -0.9719, 'Main'),
('Twyford', 'Small', 2, 51.4754, -0.8614, 'Main'),
('Maidenhead', 'Medium', 2, 51.5183, -0.7177, 'Main'),
('Taplow', 'Small', 1, 51.5238, -0.6882, 'Main'),
('Burnham', 'Small', 1, 51.5238, -0.6527, 'Main'),
('Slough', 'Medium', 4, 51.5113, -0.5950, 'Main'),
('Langley', 'Small', 1, 51.5057, -0.5541, 'Main'),
('Iver', 'Small', 1, 51.5100, -0.5050, 'Main'),
('West Drayton', 'Small', 2, 51.5096, -0.4472, 'Main'),
('Hayes & Harlington', 'Small', 2, 51.5069, -0.4225, 'Main'),
('Heathrow Terminal 2 & 3', 'Large', 2, 51.4713, -0.4524, 'Main'),
('Heathrow Terminal 4', 'Medium', 2, 51.4585, -0.4466, 'Main'),
('Heathrow Terminal 5', 'Large', 2, 51.4733, -0.4889, 'Main'),
('Southall', 'Small', 2, 51.5057, -0.3776, 'Main'),
('Hanwell', 'Small', 1, 51.5100, -0.3394, 'Main'),
('West Ealing', 'Small', 2, 51.5132, -0.3228, 'Main'),
('Ealing Broadway', 'Medium', 3, 51.5152, -0.3017, 'Main'),
('Paddington', 'Large', 6, 51.5154, -0.1755, 'Main'),
('Bond Street', 'Large', 3, 51.5142, -0.1494, 'Main'),
('Tottenham Court Road', 'Large', 4, 51.5165, -0.1299, 'Main'),
('Farringdon', 'Medium', 2, 51.5203, -0.1051, 'Main'),
('City Thameslink', 'Small', 1, 51.5141, -0.1028, 'Main'),
('Liverpool Street', 'Large', 4, 51.5183, -0.0823, 'Main'),
('Whitechapel', 'Medium', 3, 51.5197, -0.0594, 'Main'),
('Whitechapel', 'Medium', 3, 51.5197, -0.0594, 'Shenfield'),
('Stratford', 'Large', 6, 51.5416, -0.0042, 'Shenfield'),
('Maryland', 'Small', 2, 51.5461, 0.0039, 'Shenfield'),
('Forest Gate', 'Small', 2, 51.5496, 0.0311, 'Shenfield'),
('Manor Park', 'Small', 2, 51.5511, 0.0513, 'Shenfield'),
('Ilford', 'Medium', 3, 51.5583, 0.0750, 'Shenfield'),
('Seven Kings', 'Small', 2, 51.5641, 0.0894, 'Shenfield'),
('Goodmayes', 'Small', 2, 51.5652, 0.1047, 'Shenfield'),
('Chadwell Heath', 'Small', 2, 51.5657, 0.1267, 'Shenfield'),
('Romford', 'Medium', 3, 51.5752, 0.1833, 'Shenfield'),
('Gidea Park', 'Small', 2, 51.5816, 0.2122, 'Shenfield'),
('Harold Wood', 'Small', 2, 51.5908, 0.2336, 'Shenfield'),
('Brentwood', 'Medium', 2, 51.6185, 0.3005, 'Shenfield'),
('Shenfield', 'Medium', 2, 51.6297, 0.3267, 'Shenfield'),
('Whitechapel', 'Medium', 3, 51.5197, -0.0594, 'AbbeyWood'),
('Shadwell', 'Small', 2, 51.5113, -0.0569, 'AbbeyWood'),
('Wapping', 'Small', 2, 51.5044, -0.0553, 'AbbeyWood'),
('Rotherhithe', 'Small', 2, 51.5006, -0.0522, 'AbbeyWood'),
('Canada Water', 'Medium', 3, 51.4982, -0.0502, 'AbbeyWood'),
('Surrey Quays', 'Small', 2, 51.4933, -0.0478, 'AbbeyWood'),
('New Cross', 'Small', 2, 51.4760, -0.0325, 'AbbeyWood'),
('New Cross Gate', 'Small', 2, 51.4757, -0.0402, 'AbbeyWood'),
('Abbey Wood', 'Medium', 2, 51.4908, 0.1198, 'AbbeyWood');
GO

