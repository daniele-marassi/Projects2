CREATE TABLE [auth].[Authentications]
(
	[Id] BIGINT NOT NULL IDENTITY,
    [UserId] BIGINT NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[PasswordExpiration] bit NOT NULL,
	[PasswordExpirationDays] int NOT NULL,
	[Enable] bit NOT NULL,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [FK_auth.Authentications_auth.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE, 
    PRIMARY KEY ([Id], [UserId])
)