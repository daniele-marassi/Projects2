CREATE TABLE [Automation].[SimulatedConnector]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1),
	[TagId] BIGINT NOT NULL UNIQUE, 
	[Value] VARCHAR(MAX),
	[LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [PkSimulatedConnector_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FkSimulatedConnector_TagId] FOREIGN KEY ([TagId]) REFERENCES [Automation].[Tags]([Id])
)
