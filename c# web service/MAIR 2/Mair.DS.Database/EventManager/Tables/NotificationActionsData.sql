CREATE TABLE [EventManager].[NotificationActionsData] (
    [Id]                   BIGINT         NOT NULL,
    [NotificationActionId] BIGINT         NOT NULL,
    [Type]                 NVARCHAR (50)  NULL,
    [ToAddresses]          NVARCHAR (200) NULL,
    [Description]          NVARCHAR (50)  NULL,
    [Message]              NVARCHAR (500) NULL,
    [DateTime]             DATETIME       CONSTRAINT [DF_NotificationAction_Data_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]            ROWVERSION     NOT NULL,
    CONSTRAINT [PkNotificationAction_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

