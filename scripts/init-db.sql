-- scripts/init-db.sql
USE master;
GO

-- Aguardar SQL Server ficar totalmente pronto
WAITFOR DELAY '00:00:10';
GO

-- Criar banco de dados se não existir
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'BarraTourDb')
BEGIN
    CREATE DATABASE BarraTourDb;
    PRINT 'Database BarraTourDb created successfully.';
END
ELSE
BEGIN
    PRINT 'Database BarraTourDb already exists.';
END
GO

-- Usar o banco de dados
USE BarraTourDb;
GO