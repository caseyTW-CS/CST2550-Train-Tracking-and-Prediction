-- =========================
-- USERS TABLE
-- =========================
CREATE TABLE IF NOT EXISTS userInfo (
    userName TEXT PRIMARY KEY,
    userPass TEXT NOT NULL,
    userAge INTEGER NOT NULL,
    userEmail TEXT,
    userPhone TEXT NOT NULL
);

-- Sample users (password = "password123" hashed with BCrypt)
INSERT OR IGNORE INTO userInfo VALUES
('JohnSmith', '$2a$11$7QJ8Q7y8H6m5GzK6b7Zc3uOQ2Q0l9w6v5Y6k8wQ2xF5H9yK3zP1bG', 45, 'john@email.com', '07900000001'),
('JaneSmith', '$2a$11$7QJ8Q7y8H6m5GzK6b7Zc3uOQ2Q0l9w6v5Y6k8wQ2xF5H9yK3zP1bG', 47, 'jane@email.com', '07900000002');


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

INSERT OR IGNORE INTO trainsInfo (trainType, trainCompany, trainCarriages, trainLatestStop, trainNextStop)
VALUES
('Commuter', 'Elizabeth Line', 9, 'Paddington', 'Bond Street'),
('Commuter', 'Elizabeth Line', 9, 'Liverpool Street', 'Stratford');


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

INSERT OR IGNORE INTO stationInfo 
(stationName, stationLocation, stationSize, stationPlatforms, stationLAT, stationLONG, stationLine)
VALUES
('Reading', 'Berkshire', 'Large', 7, 51.4587, -0.9719, 'Main'),
('Slough', 'Berkshire', 'Medium', 4, 51.5113, -0.5950, 'Main'),
('Paddington', 'London', 'Large', 6, 51.5154, -0.1755, 'Main'),
('Bond Street', 'London', 'Large', 3, 51.5142, -0.1494, 'Main'),
('Liverpool Street', 'London', 'Large', 4, 51.5183, -0.0823, 'Main'),
('Stratford', 'London', 'Large', 6, 51.5416, -0.0042, 'Shenfield'),
('Shenfield', 'Essex', 'Medium', 2, 51.6297, 0.3267, 'Shenfield'),
('Canary Wharf', 'London', 'Large', 3, 51.5054, -0.0235, 'AbbeyWood'),
('Custom House', 'London', 'Medium', 2, 51.5096, 0.0262, 'AbbeyWood'),
('Woolwich', 'London', 'Medium', 2, 51.4915, 0.0695, 'AbbeyWood'),
('Abbey Wood', 'London', 'Medium', 2, 51.4908, 0.1198, 'AbbeyWood');