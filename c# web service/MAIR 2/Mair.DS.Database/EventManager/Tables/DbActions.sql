CREATE TABLE [EventManager].[DbActions] (
    [Id]          BIGINT         NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [Description] NVARCHAR (1024) NULL,
    [DateTime]    DATETIME       CONSTRAINT [DF_DbActions_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]   ROWVERSION     NOT NULL,
    CONSTRAINT [PkDbActions_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkDbActions_Id] FOREIGN KEY ([Id]) REFERENCES [EventManager].[Actions] ([Id])
);

