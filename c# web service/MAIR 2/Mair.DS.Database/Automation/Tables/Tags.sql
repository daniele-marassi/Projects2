CREATE TABLE [Automation].[Tags]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1), 
    [Name] NVARCHAR(256) NOT NULL, 
    [Description] NVARCHAR(1024) NULL, 
    [NodeId] BIGINT NOT NULL,
    [Address] NVARCHAR(1024) NOT NULL, 
    [IsEnabled] BIT NOT NULL DEFAULT 1,
    [Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [PkTags_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkTags_NodeId] FOREIGN KEY ([NodeId]) REFERENCES [Automation].[Nodes]([Id]) on delete cascade
)
