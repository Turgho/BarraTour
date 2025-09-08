-- scripts/init-db.sql
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'BarraTourDb')
BEGIN
    CREATE DATABASE BarraTourDb;
END
GO

USE BarraTourDb;
GO