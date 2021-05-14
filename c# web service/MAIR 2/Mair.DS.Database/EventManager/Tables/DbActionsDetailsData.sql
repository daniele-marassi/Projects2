CREATE TABLE [EventManager].[DbActionsDetailsData] (
    [Id]                 BIGINT         NOT NULL,
    [DbActionsDetailsId] BIGINT         NOT NULL,
    [Name]               NVARCHAR (100) NULL,
    [Value]              NVARCHAR (100) NULL,
    [Timestamp]          ROWVERSION     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FkDbActionsDetailsData_DbActionsDetailsId] FOREIGN KEY ([DbActionsDetailsId]) REFERENCES [EventManager].[DbActionsDetails] ([Id])
);

