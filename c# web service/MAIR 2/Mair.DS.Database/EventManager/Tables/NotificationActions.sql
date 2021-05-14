CREATE TABLE [EventManager].[NotificationActions] (
    [Id]          BIGINT         NOT NULL,
    [ToAddresses] NVARCHAR (200) NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Description] NVARCHAR (1024) NULL,
    [Message]     NVARCHAR (500) NULL,
    [DateTime]    DATETIME       CONSTRAINT [DF_NotificationActions_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]   ROWVERSION     NOT NULL,
    CONSTRAINT [PkNotificationActions_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FkNotificationActions_Id] FOREIGN KEY ([Id]) REFERENCES [EventManager].[Actions] ([Id])
);

