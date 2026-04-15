BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "favouriteStations" (
	"id"	INTEGER,
	"userName"	TEXT,
	"stationName"	TEXT,
	PRIMARY KEY("id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "trainSchedule" (
	"id"	INTEGER,
	"stationName"	TEXT,
	"destination"	TEXT,
	"departureTime"	TEXT,
	"platform"	TEXT,
	PRIMARY KEY("id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "userInfo" (
	"userID"	INTEGER,
	"userName"	TEXT NOT NULL,
	"userPass"	TEXT NOT NULL,
	"userAge"	INTEGER,
	"userEmail"	TEXT,
	"userPhone"	TEXT,
	PRIMARY KEY("userID" AUTOINCREMENT)
);
INSERT INTO "trainSchedule" VALUES (1,'Paddington','Abbey Wood','12:05','3');
INSERT INTO "trainSchedule" VALUES (2,'Paddington','Shenfield','12:10','4');
INSERT INTO "trainSchedule" VALUES (3,'Liverpool Street','Reading','12:07','1');
INSERT INTO "trainSchedule" VALUES (4,'Whitechapel','Heathrow','12:15','2');
INSERT INTO "userInfo" VALUES (1,'Ishaan','$2a$11$cYUe92EiMsvwcv7hum0v4eVZlP.17omShZl4hPE6W9DK78tJWRpnG',20,'ishaan.hari12@gmail.com','123456789');
COMMIT;
