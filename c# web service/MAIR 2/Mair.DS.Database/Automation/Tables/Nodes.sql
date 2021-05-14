CREATE TABLE [Automation].[Nodes]
(
	[Id] BIGINT NOT NULL IDENTITY(1, 1), 
    [Name] NVARCHAR(256) NOT NULL, 
    [Description] NVARCHAR(1024) NULL, 
    [Driver] NVARCHAR(1024) NOT NULL, 
    [ConnectionString] NVARCHAR(1024) NULL, 
    [Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [PkNodes_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
)
