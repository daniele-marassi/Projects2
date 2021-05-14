CREATE TABLE [EventManager].[EmailActionsData] (
    [Id]              BIGINT         NOT NULL,
    [EmailActionId]   BIGINT         NOT NULL,
    [Name]            NVARCHAR (50) NULL,
    [Description]     NVARCHAR (1024) NULL,
    [Subject]         NVARCHAR (50)  NOT NULL,
    [MailToAddresses] NVARCHAR (200) NOT NULL,
    [Message]         NVARCHAR (500) NULL,
    [DateTime]        DATETIME       CONSTRAINT [DF_EmailActions_Data_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]       ROWVERSION     NOT NULL,
    CONSTRAINT [PkEmailActionsData_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

