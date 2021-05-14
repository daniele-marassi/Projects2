CREATE TABLE [tag].[Timers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(256) NOT NULL Unique, 
    [Description] NVARCHAR(1024) NULL,
    [Inteval] NVARCHAR(128) NOT NULL,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime()
)
