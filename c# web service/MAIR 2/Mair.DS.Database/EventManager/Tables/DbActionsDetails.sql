CREATE TABLE [EventManager].[DbActionsDetails] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [DbActionsId] BIGINT         NOT NULL,
    [Reference]   NVARCHAR (100) NOT NULL,
    [DateTime]    DATETIME       DEFAULT (sysdatetime()) NOT NULL,
    [TimeStamp]   ROWVERSION     NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (1024) NULL,
    [Type]        NVARCHAR (100) NOT NULL,
    CONSTRAINT [PkDbActionsDetails_Id] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FkDbActionsDetails_DbActionsId] FOREIGN KEY ([DbActionsId]) REFERENCES [EventManager].[DbActions] ([Id])
);

