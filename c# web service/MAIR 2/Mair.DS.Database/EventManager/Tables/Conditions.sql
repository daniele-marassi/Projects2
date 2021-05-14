CREATE TABLE [EventManager].[Conditions] (
    [Id]            BIGINT          NOT NULL,
    [Name]          NVARCHAR (20)   NOT NULL,
    [Description]   NVARCHAR (1024)   NULL,
    [JsonCondition] NVARCHAR (MAX) NOT NULL,
    [DateTime]      DATETIME        CONSTRAINT [DF_Conditions_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]     ROWVERSION      NOT NULL,
    CONSTRAINT [PkConditions_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

