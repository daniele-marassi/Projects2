USE [master]
GO
/****** Object:  Database [Supp.DataBase]    Script Date: 24/01/2022 17:54:01 ******/
CREATE DATABASE [Supp.DataBase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Supp.DataBase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Supp.DataBase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Supp.DataBase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\Supp.DataBase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Supp.DataBase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Supp.DataBase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Supp.DataBase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Supp.DataBase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Supp.DataBase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Supp.DataBase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Supp.DataBase] SET ARITHABORT OFF 
GO
ALTER DATABASE [Supp.DataBase] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Supp.DataBase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Supp.DataBase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Supp.DataBase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Supp.DataBase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Supp.DataBase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Supp.DataBase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Supp.DataBase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Supp.DataBase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Supp.DataBase] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Supp.DataBase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Supp.DataBase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Supp.DataBase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Supp.DataBase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Supp.DataBase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Supp.DataBase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Supp.DataBase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Supp.DataBase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Supp.DataBase] SET  MULTI_USER 
GO
ALTER DATABASE [Supp.DataBase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Supp.DataBase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Supp.DataBase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Supp.DataBase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Supp.DataBase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Supp.DataBase] SET QUERY_STORE = OFF
GO
USE [Supp.DataBase]
GO
/****** Object:  User [Admin]    Script Date: 24/01/2022 17:54:01 ******/
CREATE USER [Admin] FOR LOGIN [Admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Admin]
GO
/****** Object:  Schema [auth]    Script Date: 24/01/2022 17:54:01 ******/
CREATE SCHEMA [auth]
GO
/****** Object:  Table [auth].[Authentications]    Script Date: 24/01/2022 17:54:01 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [auth].[UserRoles]    Script Date: 24/01/2022 17:54:01 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [auth].[UserRoleTypes]    Script Date: 24/01/2022 17:54:01 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [auth].[Users]    Script Date: 24/01/2022 17:54:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [auth].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Surname] [nvarchar](256) NOT NULL,
	[CustomizeParams] [nvarchar](max) NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExecutionQueues]    Script Date: 24/01/2022 17:54:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExecutionQueues](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](256) NOT NULL,
	[FullPath] [nvarchar](4000) NULL,
	[Arguments] [nvarchar](4000) NULL,
	[Output] [nvarchar](max) NULL,
	[WebSpeechId] [bigint] NULL,
	[ScheduledDateTime] [datetime] NULL,
	[Host] [nvarchar](256) NULL,
	[StateQueue] [nvarchar](256) NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoogleAccounts]    Script Date: 24/01/2022 17:54:01 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GoogleAuths]    Script Date: 24/01/2022 17:54:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GoogleAuths](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Client_id] [nvarchar](max) NULL,
	[Project_id] [nvarchar](max) NULL,
	[Client_secret] [nvarchar](max) NULL,
	[InsDateTime] [datetime] NOT NULL,
	[TokenFileInJson] [nvarchar](max) NULL,
	[GooglePublicKey] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Media]    Script Date: 24/01/2022 17:54:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GoogleAccountId] [bigint] NULL,
	[FileId] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[Path] [nvarchar](max) NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MediaConfigurations]    Script Date: 24/01/2022 17:54:01 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Songs]    Script Date: 24/01/2022 17:54:01 ******/
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
	[InsDateTime] [datetime] NOT NULL,
	[DurationInMilliseconds] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebSpeeches]    Script Date: 24/01/2022 17:54:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebSpeeches](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Phrase] [nvarchar](max) NOT NULL,
	[Operation] [nvarchar](256) NULL,
	[Parameters] [nvarchar](256) NULL,
	[Host] [nvarchar](256) NOT NULL,
	[Answer] [nvarchar](max) NULL,
	[FinalStep] [bit] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ParentIds] [nvarchar](max) NULL,
	[Ico] [nvarchar](256) NULL,
	[Order] [int] NOT NULL,
	[Type] [nvarchar](256) NOT NULL,
	[OperationEnable] [bit] NULL,
	[SubType] [nvarchar](256) NULL,
	[Step] [int] NOT NULL,
	[StepType] [nvarchar](256) NULL,
	[ElementIndex] [int] NOT NULL,
	[InsDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [auth].[Authentications] ON 

INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (1, 1, N'4002c2e86fafc017bc7f87af22b5b531', 0, 90, 1, CAST(N'2021-04-28T00:00:00.000' AS DateTime), CAST(N'2020-01-10T13:42:42.000' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (2, 2, N'a90071436830469ec57745b621ae32ad', 1, 90, 1, CAST(N'2020-01-27T14:23:37.443' AS DateTime), CAST(N'2020-01-27T14:23:37.443' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (3, 3, N'a90071436830469ec57745b621ae32ad', 0, 90, 1, CAST(N'2020-01-27T14:24:20.730' AS DateTime), CAST(N'2020-01-27T14:24:20.730' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (4, 4, N'4002c2e86fafc017bc7f87af22b5b531', 0, 90, 1, CAST(N'2021-04-28T00:00:00.000' AS DateTime), CAST(N'2021-04-28T22:15:35.940' AS DateTime))
INSERT [auth].[Authentications] ([Id], [UserId], [Password], [PasswordExpiration], [PasswordExpirationDays], [Enable], [CreatedAt], [InsDateTime]) VALUES (5, 5, N'4002c2e86fafc017bc7f87af22b5b531', 0, 90, 1, CAST(N'2021-04-28T00:00:00.000' AS DateTime), CAST(N'2021-04-28T22:15:35.940' AS DateTime))
SET IDENTITY_INSERT [auth].[Authentications] OFF
SET IDENTITY_INSERT [auth].[UserRoles] ON 

INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (1, 1, 1, CAST(N'2020-01-21T16:06:19.297' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (2, 1, 2, CAST(N'2020-01-21T16:06:26.683' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (3, 1, 3, CAST(N'2020-01-21T16:06:46.540' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (4, 2, 2, CAST(N'2020-01-27T14:27:41.370' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (5, 2, 3, CAST(N'2020-01-27T14:27:44.573' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (7, 4, 2, CAST(N'2021-04-28T22:15:46.960' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (8, 4, 3, CAST(N'2021-04-28T22:15:46.997' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (10, 5, 2, CAST(N'2021-04-28T22:15:46.960' AS DateTime))
INSERT [auth].[UserRoles] ([Id], [UserId], [UserRoleTypeId], [InsDateTime]) VALUES (11, 5, 3, CAST(N'2021-04-28T22:15:46.997' AS DateTime))
SET IDENTITY_INSERT [auth].[UserRoles] OFF
SET IDENTITY_INSERT [auth].[UserRoleTypes] ON 

INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (1, N'Admin', CAST(N'2020-01-10T13:42:42.150' AS DateTime))
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (2, N'User', CAST(N'2020-01-10T13:42:42.153' AS DateTime))
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (3, N'SuperUser', CAST(N'2020-01-27T14:18:48.353' AS DateTime))
INSERT [auth].[UserRoleTypes] ([Id], [Type], [InsDateTime]) VALUES (4, N'Guest', CAST(N'2020-01-27T14:18:57.567' AS DateTime))
SET IDENTITY_INSERT [auth].[UserRoleTypes] OFF
SET IDENTITY_INSERT [auth].[Users] ON 

INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (1, N'Admin', N'Admin', N'Admin', NULL, CAST(N'2020-01-10T13:42:42.153' AS DateTime))
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (2, N'SuperUser', N'SuperUser', N'SuperUser', NULL, CAST(N'2020-01-27T14:20:42.970' AS DateTime))
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (3, N'Guest', N'Guest', N'Guest', NULL, CAST(N'2020-01-27T14:20:55.193' AS DateTime))
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (4, N'daniele.marassi@gmail.com', N'Daniele', N'Marassi', N'{"General":{"PageSize":"5","Culture":"it-IT"},"Speech":{"HostsArray":"[\"EV-PC\",\"EV-TB\"]","HostDefault":"EV-PC","ListeningWord1":"","ListeningWord2":"cortana","ListeningAnswer":"si dimmi",/*Salutation - If it contains the key ''NAME'' it will be replaced with your profile name. If it contains the key ''SURNAME'' it will be replaced with your profile surname.*/"Salutation":"Ehi NAME",/*MeteoParameterToTheSalutation - empty to disable it.*/"MeteoParameterToTheSalutation":"trieste-oggi-32006","DescriptionMeteoToTheSalutationActive": "True","RemindersActive": "True", "TimeToResetInSeconds": 120, "TimeToEhiTimeoutInSeconds": 15, "WakeUpScreenAfterEhiActive": "False", "GoogleCalendarAccount": "daniele.marassi"}}', CAST(N'2021-04-28T22:15:21.000' AS DateTime))
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (5, N'daniele.marassi.ev.tb@gmail.com', N'Daniele', N'Marassi', N'{"General":{"PageSize":"5","Culture":"it-IT"},"Speech":{"HostsArray":"[\"EV-PC\",\"EV-TB\"]","HostDefault":"EV-TB","ListeningWord1":"","ListeningWord2":"cortana","ListeningAnswer":"si dimmi",/*Salutation - If it contains the key ''NAME'' it will be replaced with your profile name. If it contains the key ''SURNAME'' it will be replaced with your profile surname.*/"Salutation":"Ehi NAME",/*MeteoParameterToTheSalutation - empty to disable it.*/"MeteoParameterToTheSalutation":"trieste-oggi-32006","DescriptionMeteoToTheSalutationActive": "True","RemindersActive": "True", "TimeToResetInSeconds": 120, "TimeToEhiTimeoutInSeconds": 15, "WakeUpScreenAfterEhiActive": "True", "GoogleCalendarAccount": "daniele.marassi"}}', CAST(N'2022-01-02T12:12:06.000' AS DateTime))
SET IDENTITY_INSERT [auth].[Users] OFF
SET IDENTITY_INSERT [dbo].[WebSpeeches] ON 

INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (1, N'Meteo_Oggi', N'[["meteo", "tempo"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (2, N'Meteo_Domani', N'[["meteo", "tempo"], ["domani"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (3, N'Meteo_Oggi:_Mattina', N'[["meteo", "tempo"], ["mattina", "mattino"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (4, N'Meteo_Domani:_Mattina', N'[["meteo", "tempo"], ["domani"], ["mattina", "mattino"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (5, N'Meteo_Oggi:_Pomeriggio', N'[["meteo", "tempo"], ["pomeriggio"]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (6, N'Meteo_Domani:_Pomeriggio', N'[["meteo", "tempo"], ["domani"], ["pomeriggio"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (7, N'Meteo_Oggi:_Sera', N'[["meteo", "tempo"], ["sera"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (8, N'Meteo_Domani:_Sera', N'[["meteo", "tempo"], ["domani"], ["sera"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (9, N'Meteo_Oggi:_Notte', N'[["meteo", "tempo"], ["notte"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (10, N'Meteo_Domani:_Notte', N'[["meteo", "tempo"], ["domani"], ["notte"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (11, N'Ora', N'[["ora","ore"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/clock.png', 0, N'Time', 1, NULL, 0, NULL, 0, CAST(N'2021-12-18T22:01:14.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (12, N'Volume:_al_n_percento', N'[["volume"], ["a", "al"]]', N'C:\Program Files (x86)\Commands\Volume\volume_with_percentage.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, N'null', N'/Images/Shortcuts/volume-percentage.png', 0, N'RunExeWithNumericParameter', 1, N'SystemDialogueRunExe', 0, NULL, 0, CAST(N'2021-08-14T23:14:07.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (13, N'Promemoria:_Oggi', N'[["appuntamenti", "appuntamento",  "promemoria", "da fare","eventi","memo"], ["oggi"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/reminder-read.png', 0, N'ReadRemindersToday', 1, NULL, 0, NULL, 0, CAST(N'2021-12-08T23:26:27.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (14, N'Promemoria:_Domani', N'[["appuntamenti", "appuntamento",  "promemoria", "da fare","eventi","memo"], ["domani"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/reminder-read.png', 0, N'ReadRemindersTomorrow', 1, NULL, 0, NULL, 0, CAST(N'2021-12-08T23:27:47.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (15, N'Lista_Spesa', N'[["spesa"]]', NULL, N'lista spesa', N'All', NULL, 1, 0, N'null', N'/Images/Shortcuts/buy-read.png', 0, N'ReadNotes', 1, NULL, 0, NULL, 0, CAST(N'2021-12-08T23:30:18.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (16, N'Nota:_Scrivi', N'[["scrivi","aggiungi","inserisci","accoda"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-write.png', 0, N'EditNote', 1, N'SystemDialogueAddToNote', 0, NULL, 0, CAST(N'2021-12-19T21:51:58.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (17, N'Nota:_Pulisci', N'[["pulisci","svuota"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-clear.png', 0, N'EditNote', 1, N'SystemDialogueClearNote', 0, NULL, 0, CAST(N'2021-12-23T15:54:49.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (18, N'Cerca_in_browser', N'[["cerca","trova"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/google.png', 0, N'WebSearch', 1, N'SystemDialogueWebSearch', 0, NULL, 0, CAST(N'2021-05-27T23:17:49.220' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (19, N'Ehi', N'cortana', N'C:\Program Files (x86)\Commands\RepositionSpeech\RepositionSpeech.exe', N'WindowWidth:530, WindowHeight:600, WindowCaption:Box, FullScreen:true, Hide:false', N'All', N'["Si dimmi","Eccomi amore"]', 1, 4, NULL, NULL, 0, N'SystemRunExe', 0, NULL, 0, NULL, 0, CAST(N'2021-04-24T21:56:46.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (20, N'Volume:_Alza', N'[["volume"], ["più", "alza", "aumenta"]]', N'C:\Program Files (x86)\Commands\Volume\Alza_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-up.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2020-01-10T13:42:42.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (21, N'Volume:_Abbassa', N'[["volume"], ["meno", "diminuisci", "abbassa"]]', N'C:\Program Files (x86)\Commands\Volume\Abbassa_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-down.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:42:19.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (22, N'Volume:_Silenzia', N'[["silenzia", "mute", "muto"]]', N'C:\Program Files (x86)\Commands\Volume\Silenzia.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-mute.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:52:40.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (23, N'TV:_Canale_5', N'[["canale"],["5"]]', N'https://www.mediasetplay.mediaset.it/diretta/canale5_cC5', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/tv.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-06-23T23:15:18.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (24, N'Playlist:_Soft', N'[["playlist", "play list"], ["soft"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Soft&_volume=40&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:49:23.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (25, N'Playlist:_Tango', N'[["playlist", "play list"], ["tango"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Tango&_volume=60&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:50:06.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (26, N'Spotify:_Tango_Nuevo', N'[["spotify"],["tango"],["nuevo"]]', NULL, N'https://open.spotify.com/playlist/36A11xexKdLaMZWpUzLq2O', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (27, N'Spotify:_Tango_Tradizionale', N'[["spotify"],["tango"],["tradizionale"]]', NULL, N'https://open.spotify.com/playlist/2LOKmWnLCkiizHwiljfhX9', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:57:48.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (28, N'Spotify:_2000', N'[["spotify"], ["2000"]]', NULL, N'https://open.spotify.com/playlist/3W4uVzChUd70KYwpAJEBDo', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:59:01.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (29, N'Restart', N'[["pc", "computer"], ["restart", "riavvia"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'restart', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/restart.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-10-25T23:00:57.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (30, N'Shutdown_PC', N'[["pc", "computer"], ["spegni", "chiudi", "shutdown"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'shutdown', N'EV-PC', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/shutdown.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-10-25T23:02:13.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (31, N'Spotify:_Dance', N'[["spotify"], ["dance"]]', NULL, N'https://open.spotify.com/playlist/0XNgv74fN0N4usS4vLjNAA?si=5041b18841e7434d', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (32, N'Lista_Spesa:_Scrivi', N'[["scrivi","aggiungi","inserisci","accoda"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 4, NULL, N'/Images/Shortcuts/buy-add.png', 0, N'EditNote', 1, N'SystemDialogueAddToNoteWithName', 0, NULL, 0, CAST(N'2021-12-22T21:54:43.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (33, N'Lista_Spesa:_Pulisci', N'[["pulisci","svuota"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 4, NULL, N'/Images/Shortcuts/buy-clear.png', 0, N'EditNote', 1, N'SystemDialogueClearNoteWithName', 0, NULL, 0, CAST(N'2021-12-23T15:55:00.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (34, N'Come_stai', N'[["come stai", "come ti senti"]]', NULL, NULL, N'All', N'["benissimo", "alla grande"]', 1, 4, NULL, N'/Images/Shortcuts/answer.png', 0, N'Request', 1, NULL, 0, NULL, 0, CAST(N'2021-12-28T23:18:58.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (36, N'Ehi', N'cortana', N'C:\Program Files (x86)\Commands\RepositionSpeech\RepositionSpeech.exe', N'WindowWidth:530, WindowHeight:600, WindowCaption:Box, FullScreen:true, Hide:false', N'All', N'["Si dimmi","Eccomi amore"]', 1, 5, NULL, NULL, 0, N'SystemRunExe', 0, NULL, 0, NULL, 0, CAST(N'2021-04-24T21:56:46.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (37, N'Volume:_Alza', N'[["volume"], ["più", "alza", "aumenta"]]', N'C:\Program Files (x86)\Commands\Volume\Alza_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-up.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2020-01-10T13:42:42.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (38, N'Volume:_Abbassa', N'[["volume"], ["meno", "diminuisci", "abbassa"]]', N'C:\Program Files (x86)\Commands\Volume\Abbassa_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-down.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:42:19.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (39, N'Volume:_Silenzia', N'[["silenzia", "mute", "muto"]]', N'C:\Program Files (x86)\Commands\Volume\Silenzia.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-mute.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:52:40.517' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (40, N'TV:_Canale_5', N'[["canale"],["5"]]', N'https://www.mediasetplay.mediaset.it/diretta/canale5_cC5', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/tv.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-06-23T23:15:18.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (41, N'Playlist:_Soft', N'[["playlist", "play list"], ["soft"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Soft&_volume=40&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:49:23.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (42, N'Playlist:_Tango', N'[["playlist", "play list"], ["tango"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Tango&_volume=60&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:50:06.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (43, N'Spotify:_Tango_Nuevo', N'[["spotify"],["tango"],["nuevo"]]', NULL, N'https://open.spotify.com/playlist/36A11xexKdLaMZWpUzLq2O', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (44, N'Spotify:_Tango_Tradizionale', N'[["spotify"],["tango"],["tradizionale"]]', NULL, N'https://open.spotify.com/playlist/2LOKmWnLCkiizHwiljfhX9', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:57:48.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (45, N'Spotify:_2000', N'[["spotify"], ["2000"]]', NULL, N'https://open.spotify.com/playlist/3W4uVzChUd70KYwpAJEBDo', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:59:01.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (48, N'Spotify:_Dance', N'[["spotify"], ["dance"]]', NULL, N'https://open.spotify.com/playlist/0XNgv74fN0N4usS4vLjNAA?si=5041b18841e7434d', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (49, N'Lista_Spesa:_Scrivi', N'[["scrivi","aggiungi","inserisci","accoda"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 5, NULL, N'/Images/Shortcuts/buy-add.png', 0, N'EditNote', 1, N'SystemDialogueAddToNoteWithName', 0, NULL, 0, CAST(N'2021-12-22T21:54:43.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (50, N'Lista_Spesa:_Pulisci', N'[["pulisci","svuota"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 5, NULL, N'/Images/Shortcuts/buy-clear.png', 0, N'EditNote', 1, N'SystemDialogueClearNoteWithName', 0, NULL, 0, CAST(N'2021-12-23T15:55:00.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (51, N'Come_stai', N'[["come stai", "come ti senti"]]', NULL, NULL, N'All', N'["benissimo", "alla grande"]', 1, 5, NULL, N'/Images/Shortcuts/answer.png', 0, N'Request', 1, NULL, 0, NULL, 0, CAST(N'2021-12-28T23:18:58.267' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (52, N'Nota:_Crea', N'[["crea","inserisci","nuovo"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-create.png', 0, N'CreateNote', 1, N'SystemDialogueCreateNote', 0, NULL, 0, CAST(N'2022-01-03T20:54:39.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (53, N'Nota:_Elimina', N'[["elimina","rimuovi","cancella"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-delete.png', 0, N'DeleteNote', 1, N'SystemDialogueDeleteNote', 0, NULL, 0, CAST(N'2022-01-03T20:54:54.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (54, N'Volume:_Percentage', N'[["volume"], ["a", "al"]]', N'C:\Program Files (x86)\Commands\Volume\volume_with_percentage.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-percentage.png', 0, N'RunExeWithNumericParameter', 1, N'SystemDialogueRunExe', 0, NULL, 0, CAST(N'2022-01-04T20:51:00.897' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (56, N'Credenziali_Google', N'[["Credenziali"],["Google"]]', N'C:\Program Files (x86)\GoogleCreateCredentialsTool\GoogleCreateCredentials.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, N'null', N'/Images/Shortcuts/generic.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-08T10:10:37.000' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (58, N'Credenziali_Google', N'[["Credenziali"],["Google"]]', N'C:\Program Files (x86)\GoogleCreateCredentialsTool\GoogleCreateCredentials.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, N'null', N'/Images/Shortcuts/generic.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-09T11:36:09.053' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (61, N'Promemoria_Esteso:_Crea', N'[["crea","inserisci","nuovo"],["appuntamento","promemoria","evento","memo"],["esteso","completo","full","extended","estesa","completa"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/reminder-create.png', 0, N'CreateExtendedReminder', 1, N'SystemDialogueCreateExtendedReminder', 0, NULL, 0, CAST(N'2022-01-13T17:18:36.450' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (62, N'Promemoria:_Elimina', N'[["elimina","rimuovi","cancella"],["appuntamento","promemoria","evento","memo"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/reminder-delete.png', 0, N'DeleteReminder', 1, N'SystemDialogueDeleteReminder', 0, NULL, 0, CAST(N'2022-01-13T17:18:53.587' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (63, N'Timer:_Set', N'[["timer"], ["imposta", "aggiungi","avvia","set","crea","start"]]', N'', N'[''Ehi NAME, il tempo è scaduto!'',''Ehi NAME, è finito il tempo'']', N'All', N'["Avviato"]', 1, 0, NULL, N'/Images/Shortcuts/timer.gif', 0, N'SetTimer', 1, N'SystemDialogueSetTimer', 0, NULL, 0, CAST(N'2022-01-22T14:26:54.283' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (64, N'Shutdown_PC', N'[["pc", "computer"], ["spegni", "chiudi", "shutdown"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'shutdown', N'EV-PC', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/shutdown.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-23T10:02:11.610' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (65, N'Restart', N'[["pc", "computer"], ["restart", "riavvia"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'restart', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/restart.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-23T10:02:54.310' AS DateTime))
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime]) VALUES (66, N'Promemoria:_Crea', N'[["crea","inserisci","nuovo"],["appuntamento","promemoria","evento","memo"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/reminder-create.png', 0, N'CreateReminder', 1, N'SystemDialogueCreateReminder', 0, NULL, 0, CAST(N'2022-01-24T12:15:15.397' AS DateTime))
SET IDENTITY_INSERT [dbo].[WebSpeeches] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__UserRole__F9B8A48B19E8FF47]    Script Date: 24/01/2022 17:54:02 ******/
ALTER TABLE [auth].[UserRoleTypes] ADD UNIQUE NONCLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__UserRole__F9B8A48BC8D95E58]    Script Date: 24/01/2022 17:54:02 ******/
ALTER TABLE [auth].[UserRoleTypes] ADD UNIQUE NONCLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__C9F2845699A9457B]    Script Date: 24/01/2022 17:54:02 ******/
ALTER TABLE [auth].[Users] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__C9F28456E4139C74]    Script Date: 24/01/2022 17:54:02 ******/
ALTER TABLE [auth].[Users] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__WebSpeec__A20D08325E0C1009]    Script Date: 24/01/2022 17:54:02 ******/
ALTER TABLE [dbo].[WebSpeeches] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [auth].[Authentications] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [auth].[UserRoles] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [auth].[UserRoleTypes] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [auth].[Users] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[ExecutionQueues] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[GoogleAccounts] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[GoogleAuths] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[Media] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[MediaConfigurations] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[Songs] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_Order]  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_OperationEnable]  DEFAULT ((1)) FOR [OperationEnable]
GO
ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_Step]  DEFAULT ((0)) FOR [Step]
GO
ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_ElementIndex]  DEFAULT ((0)) FOR [ElementIndex]
GO
ALTER TABLE [dbo].[WebSpeeches] ADD  DEFAULT (getdate()) FOR [InsDateTime]
GO
ALTER TABLE [auth].[Authentications]  WITH CHECK ADD  CONSTRAINT [FK_auth.Authentications_auth.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [auth].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [auth].[Authentications] CHECK CONSTRAINT [FK_auth.Authentications_auth.Users_UserId]
GO
ALTER TABLE [auth].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_auth.UserRoles_auth.UserRoleTypes_UserRoleTypeId] FOREIGN KEY([UserRoleTypeId])
REFERENCES [auth].[UserRoleTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [auth].[UserRoles] CHECK CONSTRAINT [FK_auth.UserRoles_auth.UserRoleTypes_UserRoleTypeId]
GO
ALTER TABLE [auth].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_auth.UserRoles_auth.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [auth].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [auth].[UserRoles] CHECK CONSTRAINT [FK_auth.UserRoles_auth.Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [Supp.DataBase] SET  READ_WRITE 
GO
