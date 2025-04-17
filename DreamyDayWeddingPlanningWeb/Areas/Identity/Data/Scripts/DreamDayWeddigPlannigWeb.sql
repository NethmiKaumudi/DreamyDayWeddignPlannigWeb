-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: dreamydayweddingplanningweb
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250414132915_add','8.0.13'),('20250417053928_AddWeddingIdToVendors','8.0.13');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `activitylogs`
--

DROP TABLE IF EXISTS `activitylogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `activitylogs` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Action` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Timestamp` datetime(6) NOT NULL,
  `Details` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ActivityLogs_UserId` (`UserId`),
  CONSTRAINT `FK_ActivityLogs_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `activitylogs`
--

LOCK TABLES `activitylogs` WRITE;
/*!40000 ALTER TABLE `activitylogs` DISABLE KEYS */;
/*!40000 ALTER TABLE `activitylogs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('85714fc9-a020-47aa-82b9-5ae4e4357362','Planner','PLANNER',NULL),('b7398463-d0ba-454b-b834-37b26a74cba5','Admin','ADMIN',NULL),('cdc0e65a-8184-4542-a389-9055dce23b93','Couple','COUPLE',NULL);
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('1651cb82-ab5f-4184-b723-f890ed96a3b8','cdc0e65a-8184-4542-a389-9055dce23b93'),('7b8ea96d-773f-42e3-910a-294c4c6c01bb','cdc0e65a-8184-4542-a389-9055dce23b93'),('f65e2cee-5f69-4600-afe9-1b676dc0f897','cdc0e65a-8184-4542-a389-9055dce23b93');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ContactNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('1651cb82-ab5f-4184-b723-f890ed96a3b8','Couple','0765974052','Test2','TEST2','grandnauro@gmail.com','GRANDNAURO@GMAIL.COM',1,'AQAAAAIAAYagAAAAECPBiACbJL+pihFcCYnCcdaCDskaS8SU0yxPQbAi0y57BU3H/6+fqBBCaxvtnNvZPg==','GYWO7RJPZR5IMNMO6OJSQVOYT4V5TSXU','5764b5a6-bbbc-4ad8-a31b-70cdb33e591f',NULL,0,0,NULL,1,0),('7b8ea96d-773f-42e3-910a-294c4c6c01bb','Couple','0765814060','Test','TEST','nethmiweeracoon@gmail.com','NETHMIWEERACOON@GMAIL.COM',1,'AQAAAAIAAYagAAAAED5T8Us8tBp+pQgFddV8I9pWWVxe5eoZLk3w/pg/LLjAVQvFsAvSXjbECKTQ7JbBqA==','W5UP4FSU6OWDJG4AEINJ7236KJ4VKI2J','9f6f25c3-fa49-45e2-a762-29857401dc1d',NULL,0,0,NULL,1,0),('d83dd2d3-1934-11f0-9d0c-106fd9b56f2c','Planner','123-456-7890','Planner1','PLANNER1','planner1@dreamyday.com','PLANNER1@DREAMYDAY.COM',1,'TEMP_PASSWORD_HASH','d83dd339-1934-11f0-9d0c-106fd9b56f2c','d83dd34c-1934-11f0-9d0c-106fd9b56f2c',NULL,0,0,NULL,1,0),('f65e2cee-5f69-4600-afe9-1b676dc0f897','Couple','0765974052','Test3','TEST3','nethmikaumudi7@gmail.com','NETHMIKAUMUDI7@GMAIL.COM',1,'AQAAAAIAAYagAAAAECXSDZ7EfmczhQN/dNI3ycCFzfZlRPSopgAhbWMAdOjhXzrx0Z0/ZV3TspkqCbYFVQ==','K4QCOHDIXWVRQIQMZ2K2T5G67KMUJLHO','78c05d6a-2be6-4fa1-b7e6-ae5fcf8b1bb6',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LoginProvider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `budgets`
--

DROP TABLE IF EXISTS `budgets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `budgets` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `WeddingId` int NOT NULL,
  `Category` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AllocatedAmount` decimal(65,30) NOT NULL,
  `SpentAmount` decimal(65,30) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Budgets_WeddingId` (`WeddingId`),
  CONSTRAINT `FK_Budgets_Weddings_WeddingId` FOREIGN KEY (`WeddingId`) REFERENCES `weddings` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `budgets`
--

LOCK TABLES `budgets` WRITE;
/*!40000 ALTER TABLE `budgets` DISABLE KEYS */;
INSERT INTO `budgets` VALUES (1,1,'Venue',800000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.018670',0),(2,1,'Catering',400000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019246',0),(3,1,'Photography',200000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019247',0),(4,1,'Decorations',200000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019250',0),(5,1,'Entertainment',200000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019250',0),(6,1,'Attire',100000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019251',0),(7,1,'Invitations',60000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019251',0),(8,1,'Other',40000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 19:13:29.019252',0),(9,2,'Venue',2000000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.249980',0),(10,2,'Catering',1000000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250134',0),(11,2,'Photography',500000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250135',0),(12,2,'Decorations',500000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250136',0),(13,2,'Entertainment',500000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250136',0),(14,2,'Attire',250000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250139',0),(15,2,'Invitations',150000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250139',0),(16,2,'Other',100000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 22:03:23.250140',0),(17,3,'Venue',800000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888726',0),(18,3,'Catering',400000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888763',0),(19,3,'Photography',200000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888763',0),(20,3,'Decorations',200000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888763',0),(21,3,'Entertainment',200000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888764',0),(22,3,'Attire',100000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888764',0),(23,3,'Invitations',60000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888764',0),(24,3,'Other',40000.000000000000000000000000000000,0.000000000000000000000000000000,'2025-04-14 23:51:55.888764',0);
/*!40000 ALTER TABLE `budgets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guests`
--

DROP TABLE IF EXISTS `guests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `guests` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `WeddingId` int NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `HasRSVPed` tinyint(1) NOT NULL,
  `MealPreference` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SeatingArrangement` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Guests_WeddingId` (`WeddingId`),
  CONSTRAINT `FK_Guests_Weddings_WeddingId` FOREIGN KEY (`WeddingId`) REFERENCES `weddings` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guests`
--

LOCK TABLES `guests` WRITE;
/*!40000 ALTER TABLE `guests` DISABLE KEYS */;
INSERT INTO `guests` VALUES (1,1,'Kamala',0,'Vegetarian','2',0),(2,3,'Kamala',0,'Vegetarian','1',0),(3,3,'Kamal',1,'Non-Vegetarian','2',0);
/*!40000 ALTER TABLE `guests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `timelineevents`
--

DROP TABLE IF EXISTS `timelineevents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `timelineevents` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `WeddingId` int NOT NULL,
  `EventName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartTime` datetime(6) NOT NULL,
  `EndTime` datetime(6) NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TimelineEvents_WeddingId` (`WeddingId`),
  CONSTRAINT `FK_TimelineEvents_Weddings_WeddingId` FOREIGN KEY (`WeddingId`) REFERENCES `weddings` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `timelineevents`
--

LOCK TABLES `timelineevents` WRITE;
/*!40000 ALTER TABLE `timelineevents` DISABLE KEYS */;
INSERT INTO `timelineevents` VALUES (1,3,'Temple Worship','2025-04-30 00:00:00.000000','2025-04-30 00:20:00.000000','For get blessing',0);
/*!40000 ALTER TABLE `timelineevents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vendors`
--

DROP TABLE IF EXISTS `vendors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vendors` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Category` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Price` decimal(65,30) NOT NULL,
  `IsApproved` tinyint(1) NOT NULL,
  `Reviews` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsBooked` tinyint(1) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `WeddingId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Vendors_WeddingId` (`WeddingId`),
  CONSTRAINT `FK_Vendors_Weddings_WeddingId` FOREIGN KEY (`WeddingId`) REFERENCES `weddings` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vendors`
--

LOCK TABLES `vendors` WRITE;
/*!40000 ALTER TABLE `vendors` DISABLE KEYS */;
/*!40000 ALTER TABLE `vendors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `weddings`
--

DROP TABLE IF EXISTS `weddings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `weddings` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PlannerId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `WeddingDate` datetime(6) NOT NULL,
  `TotalBudget` decimal(65,30) NOT NULL,
  `SpentBudget` decimal(65,30) NOT NULL,
  `Progress` double NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Weddings_PlannerId` (`PlannerId`),
  KEY `IX_Weddings_UserId` (`UserId`),
  CONSTRAINT `FK_Weddings_AspNetUsers_PlannerId` FOREIGN KEY (`PlannerId`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `FK_Weddings_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `weddings`
--

LOCK TABLES `weddings` WRITE;
/*!40000 ALTER TABLE `weddings` DISABLE KEYS */;
INSERT INTO `weddings` VALUES (1,'7b8ea96d-773f-42e3-910a-294c4c6c01bb','d83dd2d3-1934-11f0-9d0c-106fd9b56f2c','2025-04-30 00:00:00.000000',2000000.000000000000000000000000000000,0.000000000000000000000000000000,0,'2025-04-14 19:13:28.759960',1),(2,'1651cb82-ab5f-4184-b723-f890ed96a3b8','d83dd2d3-1934-11f0-9d0c-106fd9b56f2c','2025-05-29 00:00:00.000000',5000000.000000000000000000000000000000,0.000000000000000000000000000000,0,'2025-04-14 22:03:23.134662',0),(3,'7b8ea96d-773f-42e3-910a-294c4c6c01bb','d83dd2d3-1934-11f0-9d0c-106fd9b56f2c','2025-04-30 00:00:00.000000',2000000.000000000000000000000000000000,0.000000000000000000000000000000,50,'2025-04-14 23:51:55.817313',0);
/*!40000 ALTER TABLE `weddings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `weddingtasks`
--

DROP TABLE IF EXISTS `weddingtasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `weddingtasks` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TaskName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Deadline` datetime(6) NOT NULL,
  `IsCompleted` tinyint(1) NOT NULL,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_WeddingTasks_UserId` (`UserId`),
  CONSTRAINT `FK_WeddingTasks_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `weddingtasks`
--

LOCK TABLES `weddingtasks` WRITE;
/*!40000 ALTER TABLE `weddingtasks` DISABLE KEYS */;
INSERT INTO `weddingtasks` VALUES (1,'PhotoShoot','2025-04-22 00:00:00.000000',1,'7b8ea96d-773f-42e3-910a-294c4c6c01bb',0),(2,'Venue Book','2025-04-24 00:00:00.000000',0,'7b8ea96d-773f-42e3-910a-294c4c6c01bb',0);
/*!40000 ALTER TABLE `weddingtasks` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-17 22:36:55
