-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               Microsoft SQL Server 2025 (RTM) - 17.0.1000.7
-- Server OS:                    Windows 10 Home Single Language 10.0 <X64> (Build 26200: ) (Hypervisor)
-- HeidiSQL Version:             12.15.0.7171
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES  */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for Harith_Local
CREATE DATABASE IF NOT EXISTS "Harith_Local";
USE "Harith_Local";

-- Dumping structure for table Harith_Local.Owners
CREATE TABLE IF NOT EXISTS "Owners" (
	"Id" INT,
	"GithubId" BIGINT,
	"Login" NVARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"Url" NVARCHAR(500) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"Type" NVARCHAR(50) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	PRIMARY KEY ("Id"),
	UNIQUE INDEX "UQ__Owners__60B79515F35C1D19" ("GithubId")
);

-- Data exporting was unselected.

-- Dumping structure for table Harith_Local.Repositories
CREATE TABLE IF NOT EXISTS "Repositories" (
	"Id" INT,
	"GithubId" BIGINT,
	"Name" NVARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"FullName" NVARCHAR(300) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"Description" NVARCHAR(max) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"StargazersCount" INT DEFAULT NULL,
	"ForksCount" INT DEFAULT NULL,
	"OpenIssuesCount" INT DEFAULT NULL,
	"Language" NVARCHAR(100) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"CreatedAt" DATETIME2(7) DEFAULT NULL,
	"UpdatedAt" DATETIME2(7) DEFAULT NULL,
	"OwnerId" INT,
	"InsertedAt" DATETIME2(7) DEFAULT N'getutcdate()',
	FOREIGN KEY INDEX "FK_Repository_Owner" ("OwnerId"),
	PRIMARY KEY ("Id"),
	UNIQUE INDEX "UQ__Reposito__60B79515AD7082C4" ("GithubId"),
	CONSTRAINT "FK_Repository_Owner" FOREIGN KEY ("OwnerId") REFERENCES "Owners" ("Id") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Data exporting was unselected.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
