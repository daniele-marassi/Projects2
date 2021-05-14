CREATE TABLE [EventManager].[Actions] (
    [Id]           BIGINT     NOT NULL,
    [ActionTypeId] BIGINT     NOT NULL,
    [ActionId]     BIGINT     NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Description] NVARCHAR (1024) NULL,
    [DateTime]     DATETIME   CONSTRAINT [DF_Actions_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]    ROWVERSION NOT NULL,
    CONSTRAINT [PkActions_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkActions_ActionsTypeId] FOREIGN KEY ([ActionTypeId]) REFERENCES [EventManager].[ActionsType] ([Id])
);

