CREATE TABLE [Auth].[Roles]
(
	[Id] BIGINT NOT NULL IDENTITY,
	[Type] [nvarchar](256) NOT NULL Unique,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime()
	PRIMARY KEY ([Id])
)