
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LocalDbEntities];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ProductsCatalog]', 'U') IS NOT NULL
    DROP TABLE [dbo].ProductsCatalog;
GO

IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO

IF OBJECT_ID(N'[dbo].[Catalog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Catalog];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

CREATE TABLE [dbo].[Product] (
    [Code]        NVARCHAR (10) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL
);
GO	

CREATE TABLE [dbo].[ProductsCatalog] (
    [Id]   		  INT           NOT NULL,
    [CatalogCode]   NVARCHAR (10)           NOT NULL,
	[ProductCode]   NVARCHAR (10)           NOT NULL
);
GO

CREATE TABLE [dbo].[Catalog] (
    [Code]        NVARCHAR (10) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL
);
GO
-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([Code] ASC);
GO

ALTER TABLE [dbo].[ProductsCatalog]
ADD CONSTRAINT [PK_ProductsCatalog] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [dbo].[Catalog]
ADD CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED ([Code] ASC)
GO

-- --------------------------------------------------
-- Creating all NONCLUSTERED INDEX
-- --------------------------------------------------

CREATE NONCLUSTERED INDEX [IX_CatalogCode]
    ON [dbo].[ProductsCatalog]([CatalogCode] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_ProductCode]
    ON [dbo].[ProductsCatalog]([ProductCode] ASC);
GO
-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

ALTER TABLE [dbo].[ProductsCatalog]
ADD 
CONSTRAINT [FK_dbo.ProductsCatalog_dbo.Catalog_Code] FOREIGN KEY ([CatalogCode]) REFERENCES [dbo].[Catalog] ([Code]) ON DELETE CASCADE;
GO	

ALTER TABLE [dbo].[ProductsCatalog]
ADD 
CONSTRAINT [FK_dbo.ProductsCatalog_dbo.Product_Code] FOREIGN KEY ([ProductCode]) REFERENCES [dbo].[Product] ([Code]) ON DELETE CASCADE;
GO	

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------