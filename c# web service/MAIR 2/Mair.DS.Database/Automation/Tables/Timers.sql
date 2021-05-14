CREATE TABLE [Automation].[Timers]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1),
    [Name] NVARCHAR(256) NOT NULL, 
    [Description] NVARCHAR(1024) NULL,
    [Inteval] INT NOT NULL,
    [Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [PkTimers_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
