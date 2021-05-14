CREATE TABLE [tag].[Nodes]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(256) NOT NULL Unique, 
    [Description] NVARCHAR(1024) NULL, 
    [Driver] NVARCHAR(1024) NOT NULL, 
    [ConnectionString] NVARCHAR(1024) NULL, 
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime()
)
