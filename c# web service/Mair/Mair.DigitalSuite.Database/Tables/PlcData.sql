CREATE TABLE [tag].[PlcData]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Driver] NVARCHAR(1024) NOT NULL, 
    [ConnectionString] NVARCHAR(1024) NOT NULL, 
    [TagAddress] NVARCHAR(1024) NULL, 
    [TagValue] NVARCHAR(1024) NOT NULL,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime()
)
