CREATE TABLE [EventManager].[EmailActions] (
    [Id]              BIGINT         NOT NULL,
    [Name]            NVARCHAR (50)  NULL,
    [Description]     NVARCHAR (1024) NULL,
    [Subject]         NVARCHAR (50)  NOT NULL,
    [MailToAddresses] NVARCHAR (200) NOT NULL,
    [Message]         NVARCHAR (500) NULL,
    [DateTime]        DATETIME       CONSTRAINT [DF_EmailActions_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]       ROWVERSION     NOT NULL,
    CONSTRAINT [PkEmailActions_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkEmailActions_Id] FOREIGN KEY ([Id]) REFERENCES [EventManager].[Actions] ([Id])
);

