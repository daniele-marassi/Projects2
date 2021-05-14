CREATE TABLE [tag].[Events]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(256) NOT NULL Unique, 
    [Description] NVARCHAR(1024) NULL, 
    [Type] TINYINT NOT NULL DEFAULT 0,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
    [TimerId] BIGINT NULL, 
    [PlcStartId] BIGINT NULL, 
    [PlcEndId] BIGINT NULL, 
    [PlcAckId] BIGINT NULL, 
    CONSTRAINT [FK_Events_Timers] FOREIGN KEY ([TimerId]) REFERENCES [tag].[Timers]([Id]),
    CONSTRAINT [FK_Events_Tags_PlcStart] FOREIGN KEY ([PlcStartId]) REFERENCES [tag].[Tags]([Id]),
    CONSTRAINT [FK_Events_Tags_PlcEnd] FOREIGN KEY ([PlcEndId]) REFERENCES [tag].[Tags]([Id]),
    CONSTRAINT [FK_Events_Tags_PlcAck] FOREIGN KEY ([PlcAckId]) REFERENCES [tag].[Tags]([Id])

)
