CREATE TABLE [tag].[Tags]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(256) NOT NULL Unique, 
    [Description] NVARCHAR(1024) NULL, 
    [NodeId] BIGINT NOT NULL,
    [Address] NVARCHAR(1024) NOT NULL, 
    [Enable] BIT NOT NULL DEFAULT 1,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
    CONSTRAINT [FK_Tags_Nodes] FOREIGN KEY ([NodeId]) REFERENCES [tag].[Nodes]([Id]) on delete cascade
)
