CREATE TABLE [auth].[Users]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[UserName] [nvarchar](256) NOT NULL Unique,
	[Name] [nvarchar](256) NOT NULL,
	[Surname] [nvarchar](256) NOT NULL,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime()
)