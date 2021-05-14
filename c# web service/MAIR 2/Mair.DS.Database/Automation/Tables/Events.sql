CREATE TABLE [Automation].[Events]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1), 
    [Name] NVARCHAR(256) NOT NULL, 
    [Description] NVARCHAR(1024) NULL, 
    [TriggerType] TINYINT NOT NULL DEFAULT 0,
    [TimerId] BIGINT NULL, 
    [StartTagId] BIGINT NULL, 
    [EndTagId] BIGINT NULL, 
    [AcknowledgeTagId] BIGINT NULL,
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [PkEvents_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkEvents_TimerId] FOREIGN KEY ([TimerId]) REFERENCES [Automation].[Timers]([Id]),
    CONSTRAINT [FkEvents_PlcStartId] FOREIGN KEY ([StartTagId]) REFERENCES [Automation].[Tags]([Id]),
    CONSTRAINT [FkEvents_PlcEndId] FOREIGN KEY ([EndTagId]) REFERENCES [Automation].[Tags]([Id]),
    CONSTRAINT [FkEvents_PcAckId] FOREIGN KEY ([AcknowledgeTagId]) REFERENCES [Automation].[Tags]([Id])

)
