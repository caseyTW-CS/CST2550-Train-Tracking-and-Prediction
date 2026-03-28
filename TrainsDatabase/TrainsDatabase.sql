-- =========================
-- USERS TABLE
-- =========================
CREATE TABLE IF NOT EXISTS userInfo (
    userID INTEGER PRIMARY KEY AUTOINCREMENT,
    userName TEXT NOT NULL,
    userPass TEXT NOT NULL,
    userAge INTEGER NOT NULL,
    userEmail TEXT,
    userPhone TEXT NOT NULL
);

INSERT INTO userInfo (userName, userPass, userAge, userEmail, userPhone)
VALUES
('John Smith', 'Pass123', 45, 'joSmith@email.com', '079-203-6522'),
('Jane Smith', 'Pass456', 47, 'jaSmith@email.com', '079-293-4444');


-- =========================
-- STATIONS TABLE
-- =========================
CREATE TABLE IF NOT EXISTS stationInfo (
    stationID INTEGER PRIMARY KEY AUTOINCREMENT,
    stationName TEXT NOT NULL,
    stationLocation TEXT NOT NULL,
    stationSize TEXT NOT NULL,
    stationPlatforms INTEGER NOT NULL,
    stationLONG REAL NOT NULL,
    stationLAT REAL NOT NULL,
    stationLine TEXT NOT NULL
);

INSERT INTO stationInfo (stationName, stationLocation, stationSize, stationPlatforms, stationLAT, stationLONG, stationLine)
VALUES
('Reading', 'Reading', 'Large', 7, 51.4587, -0.9719, 'Main'),
('Twyford', 'Twyford', 'Small', 2, 51.4754, -0.8614, 'Main'),
('Maidenhead', 'Maidenhead', 'Medium', 2, 51.5183, -0.7177, 'Main'),
('Slough', 'Slough', 'Medium', 4, 51.5113, -0.5950, 'Main'),
('Paddington', 'London', 'Large', 6, 51.5154, -0.1755, 'Main'),
('Liverpool Street', 'London', 'Large', 4, 51.5183, -0.0823, 'Main'),
('Stratford', 'London', 'Large', 6, 51.5416, -0.0042, 'Shenfield'),
('Shenfield', 'Shenfield', 'Medium', 2, 51.6297, 0.3267, 'Shenfield'),
('Canary Wharf', 'London', 'Large', 3, 51.5036, -0.0194, 'AbbeyWood'),
('Custom House', 'London', 'Medium', 2, 51.5095, 0.0267, 'AbbeyWood'),
('Woolwich', 'London', 'Medium', 2, 51.4915, 0.0718, 'AbbeyWood'),
('Abbey Wood', 'London', 'Medium', 2, 51.4908, 0.1198, 'AbbeyWood');


-- =========================
-- TRAINS TABLE
-- =========================
CREATE TABLE IF NOT EXISTS trainsInfo (
    trainNumber INTEGER PRIMARY KEY AUTOINCREMENT,
    trainType TEXT NOT NULL,
    trainCompany TEXT NOT NULL,
    trainCarriages INTEGER NOT NULL,
    trainLatestStop TEXT,
    trainNextStop TEXT NOT NULL
);

INSERT INTO trainsInfo (trainType, trainCompany, trainCarriages, trainLatestStop, trainNextStop)
VALUES
('Commuter', 'Elizabeth Line', 9, 'Paddington', 'Liverpool Street'),
('Commuter', 'Elizabeth Line', 9, 'Stratford', 'Shenfield');