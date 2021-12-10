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
	[OperationEnable] [bit] NOT NULL,
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

/****** Object:  Table [dbo].[GoogleAccounts]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoogleAccounts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Account] [nvarchar](256) NULL,
	[FolderToFilter] [nvarchar](256) NULL,
	[GoogleAuthId] [bigint] NULL,
	[UserId] [bigint] NULL,
	[AccountType] [nvarchar](256) NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GoogleAuths]    Script Date: 22/02/2020 16:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoogleAuths](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Client_id] [nvarchar](MAX) NULL,
	[Project_id] [nvarchar](MAX) NULL,
	[Client_secret] [nvarchar](MAX) NULL,
	[TokenFileInJson] [nvarchar](MAX) NULL,
	[GooglePublicKey] [nvarchar](MAX) NULL,
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
	[GoogleAccountId] [bigint] NULL,
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

/****** Object:  Table [dbo].[Songs]    Script Date: 07/07/2021 22:02:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Songs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FullPath] [nvarchar](max) NULL,
	[Position] [nvarchar](4000) NULL,
	[Order] [int] NOT NULL,
	[Listened] [bit] NOT NULL,
	[DurationInMilliseconds] [bigint] NOT NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
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

ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_OperationEnable]  DEFAULT ((1)) FOR [OperationEnable]
GO

CREATE NONCLUSTERED INDEX dbo_ExecutionQueues_Type   
    ON [dbo].[ExecutionQueues] (Type);
GO

CREATE NONCLUSTERED INDEX dbo_ExecutionQueues_FullPath   
    ON [dbo].[ExecutionQueues] (FullPath);
GO

ALTER TABLE [dbo].[ExecutionQueues] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[GoogleAccounts] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[GoogleAuths] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[MediaConfigurations] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[Media] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO

ALTER TABLE [dbo].[Songs] ADD  DEFAULT (getdate()) FOR [InsDateTime]
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
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [InsDateTime], [CustomizeParams]) VALUES (4, N'daniele.marassi@gmail.com', N'Daniele', N'Marassi', CAST(N'2021-04-28T22:15:21.000' AS DateTime), N'{"General":{"PageSize":"3","Culture":"it-IT"},"Speech":{"HostsArray":"[\"EV-PC\",\"EV-TB\"]","HostDefault":"EV-PC","ListeningWord1":"ehi","ListeningWord2":"box","ListeningAnswer":"si dimmi",/*Salutation - If it contains the key ''NAME'' it will be replaced with your profile name. If it contains the key ''SURNAME'' it will be replaced with your profile surname.*/"Salutation":"Ehi NAME","MinSpeechWordsCoefficient":"0,05","MaxSpeechWordsCoefficient":"0,05",/*MeteoParameterToTheSalutation - empty to disable it.*/"MeteoParameterToTheSalutation":"trieste-oggi-32006","DescriptionMeteoToTheSalutationActive": "True","RemindersActive": "True", "TimesToReset": 600}}')
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
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (1, N'Ehi_Box', N'Ehi box', N'C:\Program Files (x86)\Commands\RepositionSpeech\RepositionSpeech.exe', N'WindowWidth:530, WindowHeight:600, WindowCaption:Box, FullScreen:true, Hide:false', N'EV-PC', N'["Si dimmi","Eccomi amore"]', 1, 4, N'', N'', 0, N'SystemRunExe', CAST(N'2021-04-24T21:56:46.000' AS DateTime), 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (2, N'RequestNotImplemented_1', N'Mi dispiace, non conosco! Mi insegni?', NULL, NULL, N'EV-PC', N'Mi dispiace, non conosco! Mi insegni?', 0, 0, N'', N'', 0, N'SystemRequest', CAST(N'2021-04-08T21:46:19.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (3, N'RequestNotImplemented_2', N'["no", "non ora", "no grazie"]', NULL, NULL, N'EV-PC', N'Ok', 1, 0, N'[2]', NULL, 0, N'SystemRequest', CAST(N'2021-04-08T21:47:40.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (4, N'RequestNotImplemented_3', N'["sì","ok","sì va bene","si","ok","si va bene", "ok va bene"]', NULL, NULL, N'EV-PC', NULL, 0, 0, N'[2]', NULL, 0, N'SystemRequest', CAST(N'2021-04-08T21:51:07.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (5, N'RequestNotImplemented_4', N'Cosa devo fare?', NULL, NULL, N'EV-PC', N'Cosa devo fare?', 0, 0, N'[2]', NULL, 0, N'SystemRequest', CAST(N'2021-04-22T23:34:29.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (6, N'Find_in_browser', N'["cerca","trova"]', NULL, NULL, N'all', NULL, 1, 4, NULL, N'', 0, N'SystemWebSearch', CAST(N'2021-05-27T23:17:49.220' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (7, N'Meteo', N'["com''è il meteo", "com''è il tempo", "dimmi il meteo", "dimmi il tempo"]', NULL, N'trieste-oggi-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (8, N'Meteo_domani', N'["com''è il meteo domani","com''è il tempo domani","com''è il meteo di domani","com''è il tempo di domani","dimmi il meteo domani","dimmi il tempo domani","dimmi il meteo di domani","dimmi il tempo di domani"]', NULL, N'trieste-domani-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (9, N'Meteo_mttina', N'["com''è il meteo nel mattino", "com''è il meteo al mattino", "com''è il meteo del mattino", "com''è il meteo questa mattina", "com''è il meteo di questa mattina","com''è il tempo nel mattino", "com''è il tempo al mattino", "com''è il tempo del mattino", "com''è il tempo questa mattina", "com''è il tempo di questa mattina","dimmi il meteo del mattino", "dimmi il meteo di questa mattina","dimmi il tempo del mattino", "dimmi il tempo di questa mattina"]', NULL, N'trieste-oggi-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (10, N'Meteo_domani_mattina', N'["com''è il meteo di domani mattina","com''è il meteo domani mattina","com''è il tempo di domani mattina","com''è il tempo domani mattina","dimmi il meteo di domani mattina","dimmi il tempo di domani mattina"]', NULL, N'trieste-domani-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (11, N'Meteo_pomeriggio', N'["com''è il meteo nel pomeriggio", "com''è il meteo al pomeriggio", "com''è il meteo del pomeriggio", "com''è il meteo questo pomeriggio", "com''è il meteo di questo pomeriggio","com''è il tempo nel pomeriggio", "com''è il tempo al pomeriggio", "com''è il tempo del pomeriggio", "com''è il tempo questo pomeriggio", "com''è il tempo di questo pomeriggio","dimmi il meteo del pomeriggio", "dimmi il meteo di questo pomeriggio","dimmi il tempo del pomeriggio", "dimmi il tempo di questo pomeriggio"]', NULL, N'trieste-oggi-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (12, N'Meteo_domani_pomeriggio', N'["com''è il meteo di domani pomeriggio","com''è il meteo domani pomeriggio","com''è il tempo di domani pomeriggio","com''è il tempo domani pomeriggio","dimmi il meteo di domani pomeriggio","dimmi il tempo di domani pomeriggio"]', NULL, N'trieste-domani-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (13, N'Meteo_sera', N'["com''è il meteo nella sera", "com''è il meteo alla sera", "com''è il meteo della sera", "com''è il meteo questa sera", "com''è il meteo di questa sera","com''è il tempo nella sera","com''è il tempo alla sera", "com''è il tempo della sera", "com''è il tempo questa sera", "com''è il tempo di questa sera","dimmi il meteo della sera","dimmi il meteo di questa sera","dimmi il tempo della sera","dimmi il tempo di questa sera"]', NULL, N'trieste-oggi-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (14, N'Meteo_domani_sera', N'["com''è il meteo di domani sera","com''è il meteo domani sera","com''è il tempo di domani sera","com''è il tempo domani sera","dimmi il meteo di domani sera","dimmi il tempo di domani sera"]', NULL, N'trieste-domani-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (15, N'Meteo_notte', N'["com''è il meteo nella notte", "com''è il meteo di notte","com''è il meteo questa notte", "com''è il meteo di questa notte","com''è il tempo nella notte", "com''è il tempo della notte", "com''è il tempo questa notte", "com''è il tempo di questa notte","dimmi il meteo della notte", "dimmi il meteo di questa notte","dimmi il tempo della notte", "dimmi il tempo di questa notte"]', NULL, N'trieste-oggi-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (16, N'Meteo_domani_notte', N'["com''è il meteo di domani notte","com''è il meteo domani notte","com''è il tempo di domani notte","com''è il tempo domani notte","dimmi il meteo di domani notte","dimmi il tempo di domani notte"]', NULL, N'trieste-domani-32006', N'all', NULL, 1, 0, N'null', N'/Images/Shortcuts/meteo.png', 0, N'Meteo', CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1)
--DEFAULT
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (17, N'Alza_il_volume', N'Alza il volume', N'C:\Program Files (x86)\Commands\Volume\Alza_il_volume.exe', NULL, N'EV-PC', N'Volume alzato', 1, 4, N'', N'/Images/Shortcuts/generic.png', 0, N'RunExe', CAST(N'2020-01-10T13:42:42.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (18, N'Abbassa_il_volume', N'Abbassa il volume', N'C:\Program Files (x86)\Commands\Volume\Abbassa_il_volume.exe', NULL, N'EV-PC', N'Volume abbassato', 1, 4, N'null', N'/Images/Shortcuts/generic.png', 0, N'RunExe', CAST(N'2021-04-08T20:42:19.000' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (19, N'Silenzia', N'Silenzia', N'C:\Program Files (x86)\Commands\Volume\Silenzia.exe', NULL, N'EV-PC', NULL, 1, 4, N'', N'/Images/Shortcuts/generic.png', 0, N'RunExe', CAST(N'2021-04-08T20:52:40.517' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (20, N'Ora', N'["che ora è", "qual''è l''ora", "dimmi l''ora"]', NULL, NULL, N'all', NULL, 1, 0, NULL, N'/Images/Shortcuts/clock.png', 0, N'Time', CAST(N'2021-06-04T12:46:13.400' AS DateTime), 1)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [InsDateTime], [OperationEnable]) VALUES (21, N'Mostrami_Canale_5', N'["mostrami canale 5"]', N'https://www.mediasetplay.mediaset.it/diretta/canale5_cC5', NULL, N'all', N'ok', 1, 4, N'null', N'/Images/Shortcuts/generic.png', 0, N'Link', CAST(N'2021-06-23T23:15:18.000' AS DateTime), 1)


SET IDENTITY_INSERT [dbo].[WebSpeeches] OFF
SET ANSI_PADDING ON
GO