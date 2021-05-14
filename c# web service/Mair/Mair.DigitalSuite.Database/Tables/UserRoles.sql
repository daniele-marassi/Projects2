CREATE TABLE [auth].[UserRoles]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] BIGINT NOT NULL,
	[UserRoleTypeId] BIGINT NOT NULL,
    [LastUpdated] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	[Created] DATETIME2 NOT NULL DEFAULT sysdatetime(),
	CONSTRAINT [FK_auth.UserRoles_auth.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [auth].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_auth.UserRoles_auth.UserRoleTypes_UserRoleTypeId] FOREIGN KEY ([UserRoleTypeId]) REFERENCES [auth].[UserRoleTypes] ([Id]) ON DELETE CASCADE,
)