CREATE TABLE [EventManager].[EventLog] (
    [Id]        BIGINT     NOT NULL,
    [EventId]   BIGINT     NOT NULL,
    [DateTime]  DATETIME   CONSTRAINT [DF_EventLog_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp] ROWVERSION NOT NULL,
    CONSTRAINT [PkEventLog_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkEventLog_EventsId] FOREIGN KEY ([EventId]) REFERENCES [EventManager].[Events] ([Id])
);

