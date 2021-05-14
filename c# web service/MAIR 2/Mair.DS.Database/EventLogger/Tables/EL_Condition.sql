CREATE TABLE [dbo].[EL_Condition] (
    [Id]            BIGINT       NOT NULL,
    [Name]          VARCHAR (20) NOT NULL,
    [Description]   VARCHAR (50) NULL,
    [JsonCondition] XML          NOT NULL,
    [DateTime]      DATETIME     CONSTRAINT [DF_EL_Condition_DateTime] DEFAULT (sysdatetime()) NOT NULL,
    [Timestamp]     ROWVERSION   NOT NULL,
    CONSTRAINT [PK_EL_condition] PRIMARY KEY CLUSTERED ([Id] ASC)
);

