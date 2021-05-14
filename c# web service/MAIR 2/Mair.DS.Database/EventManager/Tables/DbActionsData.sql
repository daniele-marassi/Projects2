CREATE TABLE [EventManager].[DbActionsData] (
    [Id]          BIGINT         NOT NULL,
    [DbActionsId] BIGINT         NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Description] NVARCHAR (1024) NULL,
    [DateTime]    DATETIME       CONSTRAINT [DF_DbActions_Data_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]   ROWVERSION     NOT NULL,
    CONSTRAINT [PkDbActionsData_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkDbActionsData_DbActionId] FOREIGN KEY ([DbActionsId]) REFERENCES [EventManager].[DbActions] ([Id])
);

