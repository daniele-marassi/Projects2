CREATE TABLE [EventManager].[ActionsType] (
    [Id]          BIGINT        NOT NULL,
    [Name]        NVARCHAR (20) NOT NULL,
    [Description] NVARCHAR (1024) NULL,
    [DateTime]    DATETIME      CONSTRAINT [DF_ActionsType_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]   ROWVERSION    NOT NULL,
    CONSTRAINT [Pk_ActionsTypeId] PRIMARY KEY CLUSTERED ([Id] ASC)
);

