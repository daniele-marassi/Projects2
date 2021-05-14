CREATE TABLE [Auth].[RolePaths] (
	[Id] BIGINT NOT NULL IDENTITY(1, 1), 
    [Name] NVARCHAR(256) NOT NULL, 
	[RoleId]  BIGINT      NOT NULL,
    [Description] NVARCHAR(1024) NULL, 
     -- Path gestiti gerarchicamente se nego path/ nego tutti i discendenti (logica gerarchica)
    -- I Path sono Path base ovvero il nome delle pagine web
    -- Per il momento gestire solamente il menu (niente gestione di api)
    [Path]	NVARCHAR(1024) NOT NULL,
    -- Flag per decidere se negare o dare l'accessibilità al Path
    [IsEnabled]	BIT	DEFAULT ((1)) NOT NULL, 
    [Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FkRolePaths_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Auth].[Roles] ([Id]) ON DELETE CASCADE
);

