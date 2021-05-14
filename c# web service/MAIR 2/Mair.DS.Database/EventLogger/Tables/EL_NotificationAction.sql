CREATE TABLE [dbo].[EL_NotificationAction] (
    [Id]          BIGINT        NOT NULL,
    [Type]        VARCHAR (50)  NOT NULL,
    [ToAddresses] VARCHAR (200) NOT NULL,
    [Descritpion] VARCHAR (50)  NULL,
    [Message]     VARCHAR (500) NULL,
    [DateTime]    DATETIME      CONSTRAINT [DF_EL_NotificationAction_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]   ROWVERSION    NOT NULL,
    CONSTRAINT [PK_EL_NotificationAction] PRIMARY KEY CLUSTERED ([Id] ASC)
);

