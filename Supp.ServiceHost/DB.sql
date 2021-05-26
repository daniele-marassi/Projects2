CREATE DATABASE [Supp.DataBase];
GO
USE [Supp.DataBase]
GO
CREATE SCHEMA [auth];
GO
/****** Object:  Table [auth].[UserRoleTypes]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[UserRoleTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](256) NOT NULL,

	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [auth].[Users]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Surname] [nvarchar](256) NOT NULL,
	[CustomizeParams] [nvarchar](MAX) NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [auth].[UserRoles]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[UserRoles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[UserRoleTypeId] [bigint] NOT NULL,

	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [auth].[Authentications]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[Authentications](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[PasswordExpiration] [bit] NOT NULL,
	[PasswordExpirationDays] [int] NOT NULL,
	[Enable] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,

	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[WebSpeeches]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebSpeeches](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Phrase] [nvarchar](256) NOT NULL,
	[Operation] [nvarchar](256) NULL,
	[Parameters] [nvarchar](256) NULL,
	[Host] [nvarchar](256) NOT NULL,
	[Answer] [nvarchar](MAX) NULL,
	[FinalStep] [bit] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ParentIds] [nvarchar](max) NULL,
	[Ico] [nvarchar](256) NULL,
	[Order] [int] NOT NULL,
	[Type] [nvarchar](256) NOT NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ExecutionQueues]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExecutionQueues](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](256) NOT NULL,
	[FullPath] [nvarchar](4000) NULL,
	[Arguments] [nvarchar](4000) NULL,
	[Output] [nvarchar](MAX) NULL,
	[Host] [nvarchar](256) NULL,
	[StateQueue] [nvarchar](256) NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GoogleDriveAccounts]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoogleDriveAccounts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Account] [nvarchar](256) NULL,
	[FolderToFilter] [nvarchar](256) NULL,
	[GoogleDriveAuthId] [bigint] NULL,
	[UserId] [bigint] NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GoogleDriveAuths]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoogleDriveAuths](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Client_id] [nvarchar](MAX) NULL,
	[Project_id] [nvarchar](MAX) NULL,
	[Client_secret] [nvarchar](MAX) NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[MediaConfigurations]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MediaConfigurations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[MaxThumbnailSize] [int] NULL,
	[MinThumbnailSize] [int] NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Media]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GoogleDriveAccountId] [bigint] NULL,
	[FileId] [nvarchar](MAX) NULL,
	[Name] [nvarchar](256) NULL,
	[Path] [nvarchar](MAX) NULL,
	[ModifiedTime] [datetime] NULL,
	[CreatedTime] [datetime] NULL,
	[Size] [bigint] NULL,
	[FileExtension] [nvarchar](256) NULL,
	[MimeType] [nvarchar](256) NULL,
	[VideoDurationMillis] [bigint] NULL,
	[VideoHeight] [int] NULL,
	[VideoWidth] [int] NULL,
	[ImageTime] [nvarchar](256) NULL,
	[ImageWidth] [int] NULL,
	[ImageHeight] [int] NULL,
	[ImageLocationAltitude] [float] NULL,
	[ImageLocationLatitude] [float] NULL,
	[ImageLocationLongitude] [float] NULL,
	[UserName] [nvarchar](256) NULL,
	[Type] [nvarchar](256) NULL,
	[ThumbnailWidth] [int] NULL,
	[ThumbnailHeight] [int] NULL,	
	[File] [varbinary](max) NULL,	
	[Thumbnail] [varbinary](max) NULL,	
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [auth].[UserRoleTypes] ADD UNIQUE NONCLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [auth].[UserRoleTypes] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [auth].[UserRoles] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [auth].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_auth.UserRoles_auth.UserRoleTypes_UserRoleTypeId] FOREIGN KEY([UserRoleTypeId])
REFERENCES [auth].[UserRoleTypes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[Users] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [auth].[Users] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [auth].[UserRoles] CHECK CONSTRAINT [FK_auth.UserRoles_auth.UserRoleTypes_UserRoleTypeId]
GO

ALTER TABLE [auth].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_auth.UserRoles_auth.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [auth].[Users] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [auth].[UserRoles] CHECK CONSTRAINT [FK_auth.UserRoles_auth.Users_UserId]
GO

ALTER TABLE [auth].[Authentications] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [auth].[Authentications]  WITH CHECK ADD  CONSTRAINT [FK_auth.Authentications_auth.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [auth].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [auth].[Authentications] CHECK CONSTRAINT [FK_auth.Authentications_auth.Users_UserId]
GO

ALTER TABLE [dbo].[WebSpeeches] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WebSpeeches] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_Order]  DEFAULT ((0)) FOR [Order]
GO

CREATE NONCLUSTERED INDEX dbo_ExecutionQueues_Type   
    ON [dbo].[ExecutionQueues] (Type);
GO

CREATE NONCLUSTERED INDEX dbo_ExecutionQueues_FullPath   
    ON [dbo].[ExecutionQueues] (FullPath);
GO

ALTER TABLE [dbo].[ExecutionQueues] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[GoogleDriveAccounts] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[GoogleDriveAuths] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[MediaConfigurations] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[Media] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

SET IDENTITY_INSERT [auth].[UserRoleTypes] ON 
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (1, N'Admin', CAST(N'2020-01-10T13:42:42.150' AS DateTime))
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (2, N'User', CAST(N'2020-01-10T13:42:42.153' AS DateTime))
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (3, N'SuperUser', CAST(N'2020-01-27T14:18:48.353' AS DateTime))
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (4, N'Guest', CAST(N'2020-01-27T14:18:57.567' AS DateTime))
SET IDENTITY_INSERT [auth].[UserRoleTypes] OFF

SET IDENTITY_INSERT [auth].[Users] ON 
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [InsDateTime], [CustomizeParams]) VALUES (1, N'Admin', N'Admin', N'Admin', CAST(N'2020-01-10T13:42:42.153' AS DateTime), NULL)
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [InsDateTime], [CustomizeParams]) VALUES (2, N'SuperUser', N'SuperUser', N'SuperUser', CAST(N'2020-01-27T14:20:42.970' AS DateTime), NULL)
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [InsDateTime], [CustomizeParams]) VALUES (3, N'Guest', N'Guest', N'Guest', CAST(N'2020-01-27T14:20:55.193' AS DateTime), NULL)
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [InsDateTime], [CustomizeParams]) VALUES (4, N'daniele.marassi@gmail.com', N'Daniele', N'Marassi', CAST(N'2021-04-28T22:15:21.000' AS DateTime), N'{"General":{"PageSize":"3","Culture":"it-IT"},"Speech":{"HostsArray":"[\"EV-PC\",\"EV-TB\"]","HostDefault":"EV-PC","ListeningWord1":"ehi","ListeningWord2":"box","ListeningAnswer":"si dimmi",/*Salutation - If it contains the key ''NAME'' it will be replaced with your profile name. If it contains the key ''SURNAME'' it will be replaced with your profile surname.*/"Salutation":"Ehi NAME","SpeechWordsCoefficient":"0,6666666666666667"}}')
SET IDENTITY_INSERT [auth].[Users] OFF

SET IDENTITY_INSERT [auth].[UserRoles] ON 
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (1, 1, 1, CAST(N'2020-01-21T16:06:19.297' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (2, 1, 2, CAST(N'2020-01-21T16:06:26.683' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (3, 1, 3, CAST(N'2020-01-21T16:06:46.540' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (4, 2, 2, CAST(N'2020-01-27T14:27:41.370' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (5, 2, 3, CAST(N'2020-01-27T14:27:44.573' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (6, 4, 1, CAST(N'2021-04-28T22:15:46.890' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (7, 4, 2, CAST(N'2021-04-28T22:15:46.960' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (8, 4, 3, CAST(N'2021-04-28T22:15:46.997' AS DateTime))
SET IDENTITY_INSERT [auth].[UserRoles] OFF

SET IDENTITY_INSERT [auth].[Authentications] ON 
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (1, 1, N'4002c2e86fafc017bc7f87af22b5b531', 0, 90, 1, CAST(N'2021-04-28T00:00:00.000' AS DateTime), CAST(N'2020-01-10T13:42:42.000' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (2, 2, N'a90071436830469ec57745b621ae32ad', 1, 90, 1, CAST(N'2020-01-27T14:23:37.443' AS DateTime), CAST(N'2020-01-27T14:23:37.443' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (3, 3, N'a90071436830469ec57745b621ae32ad', 0, 90, 1, CAST(N'2020-01-27T14:24:20.730' AS DateTime), CAST(N'2020-01-27T14:24:20.730' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (4, 4, N'4002c2e86fafc017bc7f87af22b5b531', 0, 90, 1, CAST(N'2021-04-28T00:00:00.000' AS DateTime), CAST(N'2021-04-28T22:15:35.940' AS DateTime))
SET IDENTITY_INSERT [auth].[Authentications] OFF

SET IDENTITY_INSERT [dbo].[WebSpeeches] ON 
--DEFAULT
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (1, N'RequestNotImplemented_1', N'Mi dispiace, non conosco! Mi insegni?', NULL, NULL, N'EV-PC', N'Mi dispiace, non conosco! Mi insegni?', 0, 0, N'', N'', 0, 'SystemRequest', CAST(N'2021-04-08T21:46:19.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (2, N'RequestNotImplemented_2', N'No grazie', NULL, NULL, N'EV-PC', N'Ok', 1, 0, N'[2]', N'', 0, 'SystemRequest', CAST(N'2021-04-08T21:47:40.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (3, N'RequestNotImplemented_3', N'Si', NULL, NULL, N'EV-PC', N'Dimmi il nome', 0, 0, N'[2]', N'', 0, 'SystemRequest', CAST(N'2021-04-08T21:51:07.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (4, N'RequestNotImplemented_4', N'Sì', NULL, NULL, N'EV-PC', N'Cosa devo fare?', 0, 0, N'[2]', N'', 0, 'SystemRequest', CAST(N'2021-04-22T23:34:29.000' AS DateTime))
--DEFAULT
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (5, N'Ehi_Box', N'Ehi box', N'C:\Program Files (x86)\Commands\RepositionSpeech\RepositionSpeech.exe', N'WindowWidth:530, WindowHeight:600, WindowCaption:Box, FullScreen:true, Hide:false', N'EV-PC', N'["Si dimmi","Eccomi amore"]', 1, 4, N'', N'', 0, 'SystemRunExe', CAST(N'2021-04-24T21:56:46.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (6, N'Alza_il_volume', N'Alza il volume', N'C:\Program Files (x86)\Commands\Volume\Alza_il_volume.exe', NULL, N'EV-PC', N'Volume alzato', 1, 4, N'', N'/Images/application.png', 0, 'RunExe', CAST(N'2020-01-10T13:42:42.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (7, N'Abbassa_il_volume', N'Abbassa il volume', N'C:\Program Files (x86)\Commands\Volume\Abbassa_il_volume.exe', NULL, N'EV-PC', N'Volume abbassato', 1, 4, N'', N'/Images/application.png', 0, 'RunExe', CAST(N'2021-04-08T20:42:19.320' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime]) VALUES (8, N'Silenzia', N'Silenzia', N'C:\Program Files (x86)\Commands\Volume\Silenzia.exe', NULL, N'EV-PC', NULL, 1, 4, N'', N'/Images/application.png', 0, 'RunExe', CAST(N'2021-04-08T20:52:40.517' AS DateTime))
SET IDENTITY_INSERT [dbo].[WebSpeeches] OFF

SET ANSI_PADDING ON
GO