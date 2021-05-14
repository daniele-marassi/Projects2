CREATE TABLE [EventManager].[Events] (
    [Id]          BIGINT        NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Description] NVARCHAR (1024) NULL,
    [ActionId]     BIGINT       NOT NULL,
    [ConditionId] BIGINT        NOT NULL,
    [DateTime]    DATETIME      CONSTRAINT [DF_Events_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]   ROWVERSION    NOT NULL,
    CONSTRAINT [PkEvents_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkEvents_ActionsId] FOREIGN KEY ([ActionId]) REFERENCES [EventManager].[Actions] ([Id]),
    CONSTRAINT [FkEvents_ConditionsId] FOREIGN KEY ([ConditionId]) REFERENCES [EventManager].[Conditions] ([Id])
);

