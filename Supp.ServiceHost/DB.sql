USE [master]
GO
/****** Object:  Database [Supp.DataBase]    Script Date: 15/11/2022 23:01:47 ******/
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
/****** Object:  User [Admin]    Script Date: 15/11/2022 23:01:47 ******/
CREATE USER [Admin] FOR LOGIN [Admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [Admin]
GO
/****** Object:  Schema [auth]    Script Date: 15/11/2022 23:01:48 ******/
CREATE SCHEMA [auth]
GO
/****** Object:  Table [auth].[Authentications]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [auth].[UserRoles]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [auth].[UserRoleTypes]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [auth].[Users]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[ExecutionQueues]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[GoogleAccounts]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[GoogleAuths]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[Media]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[MediaConfigurations]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[Songs]    Script Date: 15/11/2022 23:01:48 ******/
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
/****** Object:  Table [dbo].[WebSpeeches]    Script Date: 15/11/2022 23:01:48 ******/
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
	[Groupable] [bit] NOT NULL,
	[GroupName] [nvarchar](256) NULL,
	[GroupOrder] [int] NOT NULL,
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
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (4, N'daniele.marassi@gmail.com', N'Daniele', N'Marassi', N'{"General":{"PageSize":"5","Culture":"it-IT"},"Speech":{"HostsArray":"[\"EV-PC\",\"EV-TB\"]","HostDefault":"EV-PC","ListeningWord1":"","ListeningWord2":"cortana","ListeningAnswer":"si dimmi",/*Salutation - If it contains the key ''NAME'' it will be replaced with your profile name. If it contains the key ''SURNAME'' it will be replaced with your profile surname.*/"Salutation":"Ehi NAME",/*MeteoParameterToTheSalutation - empty to disable it.*/"MeteoParameterToTheSalutation":"trieste-oggi-32006","DescriptionMeteoToTheSalutationActive": "True","RemindersActive": "True", "TimeToResetInSeconds": 900, "TimeToEhiTimeoutInSeconds": 15, "WakeUpScreenAfterEhiActive": "False", "GoogleCalendarAccount": "daniele.marassi"}}', CAST(N'2021-04-28T22:15:21.000' AS DateTime))
INSERT [auth].[Users] ([Id], [UserName], [Name], [Surname], [CustomizeParams], [InsDateTime]) VALUES (5, N'daniele.marassi.ev.tb@gmail.com', N'Daniele', N'Marassi', N'{"General":{"PageSize":"5","Culture":"it-IT"},"Speech":{"HostsArray":"[\"EV-PC\",\"EV-TB\"]","HostDefault":"EV-TB","ListeningWord1":"","ListeningWord2":"cortana","ListeningAnswer":"si dimmi",/*Salutation - If it contains the key ''NAME'' it will be replaced with your profile name. If it contains the key ''SURNAME'' it will be replaced with your profile surname.*/"Salutation":"Ehi NAME",/*MeteoParameterToTheSalutation - empty to disable it.*/"MeteoParameterToTheSalutation":"trieste-oggi-32006","DescriptionMeteoToTheSalutationActive": "True","RemindersActive": "True", "TimeToResetInSeconds": 900, "TimeToEhiTimeoutInSeconds": 15, "WakeUpScreenAfterEhiActive": "True", "GoogleCalendarAccount": "daniele.marassi"}}', CAST(N'2022-01-02T12:12:06.000' AS DateTime))
SET IDENTITY_INSERT [auth].[Users] OFF
SET IDENTITY_INSERT [dbo].[GoogleAccounts] ON 

INSERT [dbo].[GoogleAccounts] ([Id], [Account], [FolderToFilter], [GoogleAuthId], [UserId], [AccountType], [InsDateTime]) VALUES (30, N'daniele.marassi', N'', 30, 5, N'Calendar', CAST(N'2022-02-05T23:25:53.083' AS DateTime))
INSERT [dbo].[GoogleAccounts] ([Id], [Account], [FolderToFilter], [GoogleAuthId], [UserId], [AccountType], [InsDateTime]) VALUES (31, N'daniele.marassi', N'', 31, 4, N'Calendar', CAST(N'2022-02-05T23:26:51.887' AS DateTime))
INSERT [dbo].[GoogleAccounts] ([Id], [Account], [FolderToFilter], [GoogleAuthId], [UserId], [AccountType], [InsDateTime]) VALUES (32, N'daniele.marassi.media1', N'Roberta_me', 32, 4, N'Drive', CAST(N'2022-02-05T23:29:44.887' AS DateTime))
INSERT [dbo].[GoogleAccounts] ([Id], [Account], [FolderToFilter], [GoogleAuthId], [UserId], [AccountType], [InsDateTime]) VALUES (33, N'daniele.marassi.media1', N'Roberta_me', 33, 5, N'Drive', CAST(N'2022-02-05T23:30:27.597' AS DateTime))
SET IDENTITY_INSERT [dbo].[GoogleAccounts] OFF
SET IDENTITY_INSERT [dbo].[GoogleAuths] ON 

INSERT [dbo].[GoogleAuths] ([Id], [Client_id], [Project_id], [Client_secret], [InsDateTime], [TokenFileInJson], [GooglePublicKey]) VALUES (30, N'982569746577-4lnrb3udcu2dqqk2u2mts0j2rqmiripd.apps.googleusercontent.com', N'ace-case-311111', N'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', CAST(N'2022-02-05T23:25:53.060' AS DateTime), N'{"FileName":"Google.Apis.Auth.OAuth2.Responses.TokenResponse-daniele.marassi","Content":"{\"access_token\":\"ya29.A0ARrdaM-SPCBmqNPBiwrufKqqS2fcBeB_Sgh6QuwfrNn68YQE3v1Y3LCDJF8G788xVLF9J07Ie3BM2MoZoOCguNcp_Y12M3ccKmaWMAmpbiSt73yLHNPtQ6YBzFic-mVBQqaVpgZ3BsNNV-X0tq-CPt77vcsG\",\"token_type\":\"Bearer\",\"expires_in\":3599,\"refresh_token\":\"1//09N15zt-bDjNQCgYIARAAGAkSNwF-L9IrBbTl8Nz-AGXsR71LWBM1IInjpKRxs87TydFsIYWa-TyuvJF9hO_ZGSqtXRgnbQEbiTY\",\"scope\":\"https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/calendar.readonly\",\"Issued\":\"2022-02-05T23:25:24.795+01:00\",\"IssuedUtc\":\"2022-02-05T22:25:24.795Z\"}"}', N'AIzaSyCdWVUdy3QmmYLjDwQWqP03gV49hfvWMhc')
INSERT [dbo].[GoogleAuths] ([Id], [Client_id], [Project_id], [Client_secret], [InsDateTime], [TokenFileInJson], [GooglePublicKey]) VALUES (31, N'982569746577-4lnrb3udcu2dqqk2u2mts0j2rqmiripd.apps.googleusercontent.com', N'ace-case-311111', N'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', CAST(N'2022-02-05T23:26:51.860' AS DateTime), N'{"FileName":"Google.Apis.Auth.OAuth2.Responses.TokenResponse-daniele.marassi","Content":"{\"access_token\":\"ya29.A0ARrdaM9pFT8DyQs-j3guYJ_Ww0KUVGwrk2HUOnqV223cWFc8oSrsATmwBBOeeZDt50qvz9CUJXN-6gPwsoK9kAa1tdb3rEoZNX3PNzsvNktcdDQ86xPhwrw_Hz0NYz_lWkqoznD2VSuTtd6kCp1aVBoeDK9W\",\"token_type\":\"Bearer\",\"expires_in\":3599,\"refresh_token\":\"1//09645mZP99rnUCgYIARAAGAkSNwF-L9Irytcc-wHJUxTx1B-oHDrNsgGgLIjN0M51y5A7XgwxJsP6IaVOqLOXX_BD77ol8c2cAdI\",\"scope\":\"https://www.googleapis.com/auth/calendar.readonly https://www.googleapis.com/auth/calendar\",\"Issued\":\"2022-02-05T23:26:45.443+01:00\",\"IssuedUtc\":\"2022-02-05T22:26:45.443Z\"}"}', N'AIzaSyCdWVUdy3QmmYLjDwQWqP03gV49hfvWMhc')
INSERT [dbo].[GoogleAuths] ([Id], [Client_id], [Project_id], [Client_secret], [InsDateTime], [TokenFileInJson], [GooglePublicKey]) VALUES (32, N'575087398329-b1icnrsk8co9rf8gq6p22gle1odj4b39.apps.googleusercontent.com', N'inspired-cortex-333417', N'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', CAST(N'2022-02-05T23:29:44.853' AS DateTime), N'{"FileName":"Google.Apis.Auth.OAuth2.Responses.TokenResponse-daniele.marassi.media1","Content":"{\"access_token\":\"ya29.A0ARrdaM-HSrbNAPf6W5dGUX2SyLvMySHm77WVPzqm3Rzgu07FJcjpY8fO9UN5jKZ-nR_Kego4nqO6waZ-EVo9pxopzDDzG1vPgA4EarMcY7Vnyg2GOlXTYWV3AJiFsE9fpAsS0L57h0AXEmaWv51KD8DDGQ4M\",\"token_type\":\"Bearer\",\"expires_in\":3599,\"refresh_token\":\"1//09LpQVl9uOjH5CgYIARAAGAkSNwF-L9IrdlSAbk1LV3PuSUpgrJIeJKUQvdJ7Mlo_IIzd8cecT5kXvOSCEcac6_-jYfI7snfWHY8\",\"scope\":\"https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.file\",\"Issued\":\"2022-02-05T23:29:25.045+01:00\",\"IssuedUtc\":\"2022-02-05T22:29:25.045Z\"}"}', N'AIzaSyAZ2ir-QrwdIl9MYUi0U057hRAijHXxyIk')
INSERT [dbo].[GoogleAuths] ([Id], [Client_id], [Project_id], [Client_secret], [InsDateTime], [TokenFileInJson], [GooglePublicKey]) VALUES (33, N'575087398329-b1icnrsk8co9rf8gq6p22gle1odj4b39.apps.googleusercontent.com', N'inspired-cortex-333417', N'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', CAST(N'2022-02-05T23:30:27.570' AS DateTime), N'{"FileName":"Google.Apis.Auth.OAuth2.Responses.TokenResponse-daniele.marassi.media1","Content":"{\"access_token\":\"ya29.A0ARrdaM8lk1MTjJlCe6HrFiTZswA4pZawh6tZU6Z4KbVbzxkMsVZb6jzFoHRS5_H_xgDdu7qFK7iv8Ty-YxfQ4rEqqgXHpjG30enDTjV-zxAK_Lh_REph4mYC4Hh17CvLSw7OCwmx98UyQS3zXThvAznOr-JS\",\"token_type\":\"Bearer\",\"expires_in\":3599,\"refresh_token\":\"1//09wJMubtARZRbCgYIARAAGAkSNwF-L9IrsEoFu7udmH-6PpbdFo8FnqjAWuFU14bjRtH7cpltPiJIgGCEkaD1WojXhGczKeIf4NM\",\"scope\":\"https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.file\",\"Issued\":\"2022-02-05T23:30:14.360+01:00\",\"IssuedUtc\":\"2022-02-05T22:30:14.360Z\"}"}', N'AIzaSyAZ2ir-QrwdIl9MYUi0U057hRAijHXxyIk')
SET IDENTITY_INSERT [dbo].[GoogleAuths] OFF
SET IDENTITY_INSERT [dbo].[MediaConfigurations] ON 

INSERT [dbo].[MediaConfigurations] ([Id], [UserId], [MaxThumbnailSize], [MinThumbnailSize], [InsDateTime]) VALUES (1, 4, 200, 50, CAST(N'2021-11-27T19:17:33.500' AS DateTime))
INSERT [dbo].[MediaConfigurations] ([Id], [UserId], [MaxThumbnailSize], [MinThumbnailSize], [InsDateTime]) VALUES (2, 5, 200, 50, CAST(N'2022-01-02T22:08:53.453' AS DateTime))
SET IDENTITY_INSERT [dbo].[MediaConfigurations] OFF
SET IDENTITY_INSERT [dbo].[WebSpeeches] ON 

INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (1, N'Meteo_Oggi', N'[["meteo", "tempo"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (2, N'Meteo_Domani', N'[["meteo", "tempo"], ["domani"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (3, N'Meteo_Oggi:_Mattina', N'[["meteo", "tempo"], ["mattina", "mattino"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (4, N'Meteo_Domani:_Mattina', N'[["meteo", "tempo"], ["domani"], ["mattina", "mattino"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (5, N'Meteo_Oggi:_Pomeriggio', N'[["meteo", "tempo"], ["pomeriggio"]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (6, N'Meteo_Domani:_Pomeriggio', N'[["meteo", "tempo"], ["domani"], ["pomeriggio"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (7, N'Meteo_Oggi:_Sera', N'[["meteo", "tempo"], ["sera"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (8, N'Meteo_Domani:_Sera', N'[["meteo", "tempo"], ["domani"], ["sera"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (9, N'Meteo_Oggi:_Notte', N'[["meteo", "tempo"], ["notte"]]', NULL, N'trieste-oggi-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (10, N'Meteo_Domani:_Notte', N'[["meteo", "tempo"], ["domani"], ["notte"]]', NULL, N'trieste-domani-32006', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/meteo.png', 0, N'Meteo', 1, NULL, 0, NULL, 0, CAST(N'2021-06-01T23:57:41.000' AS DateTime), 1, N'Meteo', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (11, N'Ora', N'[["ora","ore"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/clock.png', 0, N'Time', 1, NULL, 0, NULL, 0, CAST(N'2021-12-18T22:01:14.000' AS DateTime), 1, NULL, 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (12, N'Volume:_al_n_percento', N'[["volume"], ["a", "al"]]', N'C:\Program Files (x86)\Commands\Volume\volume_with_percentage.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-percentage.png', 0, N'RunExeWithNumericParameter', 1, N'SystemDialogueRunExe', 0, NULL, 0, CAST(N'2021-08-14T23:14:07.000' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (13, N'Promemoria:_Oggi', N'[["appuntamenti", "appuntamento",  "promemoria", "da fare","eventi","memo"], ["oggi"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/reminder-read.png', 0, N'ReadRemindersToday', 1, NULL, 0, NULL, 0, CAST(N'2021-12-08T23:26:27.000' AS DateTime), 1, N'Promemoria', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (14, N'Promemoria:_Domani', N'[["appuntamenti", "appuntamento",  "promemoria", "da fare","eventi","memo"], ["domani"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/reminder-read.png', 0, N'ReadRemindersTomorrow', 1, NULL, 0, NULL, 0, CAST(N'2021-12-08T23:27:47.000' AS DateTime), 1, N'Promemoria', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (15, N'Lista_Spesa', N'[["spesa"]]', NULL, N'lista spesa', N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/buy-read.png', 0, N'ReadNotes', 1, NULL, 0, NULL, 0, CAST(N'2021-12-08T23:30:18.000' AS DateTime), 1, N'Lista Spesa', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (16, N'Nota:_Scrivi', N'[["scrivi","aggiungi","inserisci","accoda"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-write.png', 0, N'EditNote', 1, N'SystemDialogueAddToNote', 0, NULL, 0, CAST(N'2021-12-19T21:51:58.000' AS DateTime), 1, N'Nota', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (17, N'Nota:_Pulisci', N'[["pulisci","svuota"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-clear.png', 0, N'EditNote', 1, N'SystemDialogueClearNote', 0, NULL, 0, CAST(N'2021-12-23T15:54:49.000' AS DateTime), 1, N'Nota', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (18, N'Cerca_in_browser', N'[["cerca","trova"]]', NULL, NULL, N'All', NULL, 1, 0, NULL, N'/Images/Shortcuts/google.png', 0, N'WebSearch', 1, N'SystemDialogueWebSearch', 0, NULL, 0, CAST(N'2021-05-27T23:17:49.220' AS DateTime), 1, NULL, 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (19, N'Ehi', N'cortana', N'C:\Program Files (x86)\Commands\RepositionSpeech\RepositionSpeech.exe', N'WindowWidth:530, WindowHeight:600, WindowCaption:Box, FullScreen:true, Hide:false', N'All', N'["Si dimmi","Eccomi amore"]', 1, 4, NULL, NULL, 0, N'SystemRunExe', 0, NULL, 0, NULL, 0, CAST(N'2021-04-24T21:56:46.000' AS DateTime), 1, NULL, 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (20, N'Volume:_Alza', N'[["volume"], ["più", "alza", "aumenta"]]', N'C:\Program Files (x86)\Commands\Volume\Alza_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-up.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2020-01-10T13:42:42.000' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (21, N'Volume:_Abbassa', N'[["volume"], ["meno", "diminuisci", "abbassa"]]', N'C:\Program Files (x86)\Commands\Volume\Abbassa_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-down.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:42:19.000' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (22, N'Volume:_Disattiva-Riattiva', N'[["silenzia", "mute", "muto","disattiva audio","disattiva volume","riattiva audio","ripristina audio","riattiva volume","ripristina volume"]]', N'C:\Program Files (x86)\Commands\Volume\Silenzia.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/volume-mute.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:52:40.000' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (23, N'TV:_Canale_5', N'[["canale"],["5"]]', N'https://www.mediasetplay.mediaset.it/diretta/canale5_cC5', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/tv.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-06-23T23:15:18.000' AS DateTime), 1, N'TV', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (24, N'Playlist:_Soft', N'[["playlist", "play list"], ["soft"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Soft&_volume=40&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:49:23.000' AS DateTime), 1, N'Playlist', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (25, N'Playlist:_Tango', N'[["playlist", "play list"], ["tango"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Tango&_volume=60&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:50:06.000' AS DateTime), 1, N'Playlist', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (26, N'Spotify:_Tango_Nuevo', N'[["spotify"],["tango"],["nuevo"]]', NULL, N'https://open.spotify.com/playlist/36A11xexKdLaMZWpUzLq2O', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (27, N'Spotify:_Tango_Tradizionale', N'[["spotify"],["tango"],["tradizionale"]]', NULL, N'https://open.spotify.com/playlist/2LOKmWnLCkiizHwiljfhX9', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:57:48.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (28, N'Spotify:_2000', N'[["spotify"], ["2000"]]', NULL, N'https://open.spotify.com/playlist/3W4uVzChUd70KYwpAJEBDo', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:59:01.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (29, N'Restart', N'[["pc", "computer"], ["restart", "riavvia"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'restart', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/restart.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-10-25T23:00:57.000' AS DateTime), 1, N'PC', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (30, N'Shutdown_PC', N'[["pc", "computer"], ["spegni", "chiudi", "shutdown"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'shutdown', N'EV-PC', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/shutdown.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-10-25T23:02:13.000' AS DateTime), 1, N'PC', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (31, N'Spotify:_Dance', N'[["spotify"], ["dance"]]', NULL, N'https://open.spotify.com/playlist/0XNgv74fN0N4usS4vLjNAA?si=5041b18841e7434d', N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (32, N'Lista_Spesa:_Scrivi', N'[["scrivi","aggiungi","inserisci","accoda"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 4, NULL, N'/Images/Shortcuts/buy-add.png', 0, N'EditNote', 1, N'SystemDialogueAddToNoteWithName', 0, NULL, 0, CAST(N'2021-12-22T21:54:43.000' AS DateTime), 1, N'Lista Spesa', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (33, N'Lista_Spesa:_Pulisci', N'[["pulisci","svuota"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 4, NULL, N'/Images/Shortcuts/buy-clear.png', 0, N'EditNote', 1, N'SystemDialogueClearNoteWithName', 0, NULL, 0, CAST(N'2021-12-23T15:55:00.000' AS DateTime), 1, N'Lista Spesa', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (34, N'Come_stai', N'[["come stai", "come ti senti"]]', NULL, NULL, N'All', N'["benissimo", "alla grande"]', 1, 4, NULL, N'/Images/Shortcuts/answer.png', 0, N'Request', 1, NULL, 0, NULL, 0, CAST(N'2021-12-28T23:18:58.000' AS DateTime), 1, N'Answer', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (36, N'Ehi', N'cortana', N'C:\Program Files (x86)\Commands\RepositionSpeech\RepositionSpeech.exe', N'WindowWidth:530, WindowHeight:600, WindowCaption:Box, FullScreen:true, Hide:false', N'All', N'["Si dimmi","Eccomi amore"]', 1, 5, NULL, NULL, 0, N'SystemRunExe', 0, NULL, 0, NULL, 0, CAST(N'2021-04-24T21:56:46.000' AS DateTime), 1, NULL, 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (37, N'Volume:_Alza', N'[["volume"], ["più", "alza", "aumenta"]]', N'C:\Program Files (x86)\Commands\Volume\Alza_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-up.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2020-01-10T13:42:42.000' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (38, N'Volume:_Abbassa', N'[["volume"], ["meno", "diminuisci", "abbassa"]]', N'C:\Program Files (x86)\Commands\Volume\Abbassa_il_volume.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-down.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:42:19.000' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (39, N'Volume:_Silenzia', N'[["silenzia", "mute", "muto"]]', N'C:\Program Files (x86)\Commands\Volume\Silenzia.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-mute.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2021-04-08T20:52:40.517' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (40, N'TV:_Canale_5', N'[["canale"],["5"]]', N'https://www.mediasetplay.mediaset.it/diretta/canale5_cC5', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/tv.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-06-23T23:15:18.000' AS DateTime), 1, N'TV', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (41, N'Playlist:_Soft', N'[["playlist", "play list"], ["soft"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Soft&_volume=40&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:49:23.000' AS DateTime), 1, N'Playlist', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (42, N'Playlist:_Tango', N'[["playlist", "play list"], ["tango"]]', NULL, N'https://192.168.1.105:83/Songs/SongsPlayer?_playListSelected=Tango&_volume=60&_command=play&_shuffle=true', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/music.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-15T00:50:06.000' AS DateTime), 1, N'Playlist', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (43, N'Spotify:_Tango_Nuevo', N'[["spotify"],["tango"],["nuevo"]]', NULL, N'https://open.spotify.com/playlist/36A11xexKdLaMZWpUzLq2O', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (44, N'Spotify:_Tango_Tradizionale', N'[["spotify"],["tango"],["tradizionale"]]', NULL, N'https://open.spotify.com/playlist/2LOKmWnLCkiizHwiljfhX9', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:57:48.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (45, N'Spotify:_2000', N'[["spotify"], ["2000"]]', NULL, N'https://open.spotify.com/playlist/3W4uVzChUd70KYwpAJEBDo', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:59:01.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (48, N'Spotify:_Dance', N'[["spotify"], ["dance"]]', NULL, N'https://open.spotify.com/playlist/0XNgv74fN0N4usS4vLjNAA?si=5041b18841e7434d', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/spotify.png', 0, N'Link', 1, NULL, 0, NULL, 0, CAST(N'2021-08-16T09:56:31.000' AS DateTime), 1, N'Spotify', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (49, N'Lista_Spesa:_Scrivi', N'[["scrivi","aggiungi","inserisci","accoda"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 5, NULL, N'/Images/Shortcuts/buy-add.png', 0, N'EditNote', 1, N'SystemDialogueAddToNoteWithName', 0, NULL, 0, CAST(N'2021-12-22T21:54:43.000' AS DateTime), 1, N'Lista Spesa', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (50, N'Lista_Spesa:_Pulisci', N'[["pulisci","svuota"],["spesa"]]', NULL, N'lista spesa', N'All', NULL, 0, 5, NULL, N'/Images/Shortcuts/buy-clear.png', 0, N'EditNote', 1, N'SystemDialogueClearNoteWithName', 0, NULL, 0, CAST(N'2021-12-23T15:55:00.000' AS DateTime), 1, N'Lista Spesa', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (51, N'Come_stai', N'[["come stai", "come ti senti"]]', NULL, NULL, N'All', N'["benissimo", "alla grande"]', 1, 5, NULL, N'/Images/Shortcuts/answer.png', 0, N'Request', 1, NULL, 0, NULL, 0, CAST(N'2021-12-28T23:18:58.267' AS DateTime), 1, N'Answer', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (52, N'Nota:_Crea', N'[["crea","inserisci","nuovo"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-create.png', 0, N'CreateNote', 1, N'SystemDialogueCreateNote', 0, NULL, 0, CAST(N'2022-01-03T20:54:39.000' AS DateTime), 1, N'Nota', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (53, N'Nota:_Elimina', N'[["elimina","rimuovi","cancella"],["nota","note"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/note-delete.png', 0, N'DeleteNote', 1, N'SystemDialogueDeleteNote', 0, NULL, 0, CAST(N'2022-01-03T20:54:54.000' AS DateTime), 1, N'Nota', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (54, N'Volume:_Percentage', N'[["volume"], ["a", "al"]]', N'C:\Program Files (x86)\Commands\Volume\volume_with_percentage.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/volume-percentage.png', 0, N'RunExeWithNumericParameter', 1, N'SystemDialogueRunExe', 0, NULL, 0, CAST(N'2022-01-04T20:51:00.897' AS DateTime), 1, N'Volume', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (56, N'Credenziali_Google', N'[["Credenziali"],["Google"]]', N'C:\Program Files (x86)\GoogleCreateCredentialsTool\GoogleCreateCredentials.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 4, NULL, N'/Images/Shortcuts/generic.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-08T10:10:37.000' AS DateTime), 1, N'App', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (58, N'Credenziali_Google', N'[["Credenziali"],["Google"]]', N'C:\Program Files (x86)\GoogleCreateCredentialsTool\GoogleCreateCredentials.exe', NULL, N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/generic.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-09T11:36:09.053' AS DateTime), 1, N'App', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (61, N'Promemoria_Esteso:_Crea', N'[["crea","inserisci","nuovo"],["appuntamento","promemoria","evento","memo"],["esteso","completo","full","extended","estesa","completa"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/reminder-create.png', 0, N'CreateExtendedReminder', 1, N'SystemDialogueCreateExtendedReminder', 0, NULL, 0, CAST(N'2022-01-13T17:18:36.450' AS DateTime), 1, N'Promemoria', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (62, N'Promemoria:_Elimina', N'[["elimina","rimuovi","cancella"],["appuntamento","promemoria","evento","memo"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/reminder-delete.png', 0, N'DeleteReminder', 1, N'SystemDialogueDeleteReminder', 0, NULL, 0, CAST(N'2022-01-13T17:18:53.587' AS DateTime), 1, N'Promemoria', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (63, N'Timer:_Set', N'[["timer"], ["imposta", "aggiungi","avvia","set","crea","start"]]', NULL, N'[''Ehi NAME, il tempo è scaduto!'',''Ehi NAME, è finito il tempo'']', N'All', N'["Avviato"]', 1, 0, NULL, N'/Images/Shortcuts/timer.gif', 0, N'SetTimer', 1, N'SystemDialogueSetTimer', 0, NULL, 0, CAST(N'2022-01-22T14:26:54.283' AS DateTime), 1, N'Timer', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (64, N'Shutdown_PC', N'[["pc", "computer"], ["spegni", "chiudi", "shutdown"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'shutdown', N'EV-PC', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/shutdown.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-23T10:02:11.610' AS DateTime), 1, N'PC', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (65, N'Restart', N'[["pc", "computer"], ["restart", "riavvia"]]', N'C:\Program Files (x86)\Shut\Shut.exe', N'restart', N'All', N'["ok","va bene","certo","bene"]', 1, 5, NULL, N'/Images/Shortcuts/restart.png', 0, N'RunExe', 1, NULL, 0, NULL, 0, CAST(N'2022-01-23T10:02:54.310' AS DateTime), 1, N'PC', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (66, N'Promemoria:_Crea', N'[["crea","inserisci","nuovo"],["appuntamento","promemoria","evento","memo"]]', NULL, NULL, N'All', NULL, 0, 0, NULL, N'/Images/Shortcuts/reminder-create.png', 0, N'CreateReminder', 1, N'SystemDialogueCreateReminder', 0, NULL, 0, CAST(N'2022-01-24T12:15:15.397' AS DateTime), 1, N'Promemoria', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (67, N'Sveglia:_Set', N'[["sveglia"], ["imposta", "aggiungi","avvia","set","crea","start"]]', NULL, N'[''NAME su!, sveglia, sono le TIME'']', N'All', N'["ok","va bene","certo","bene"]', 1, 0, NULL, N'/Images/Shortcuts/alarmClock.gif', 0, N'SetAlarmClock', 1, N'SystemDialogueSetAlarmClock', 0, NULL, 0, CAST(N'2022-02-01T15:04:50.000' AS DateTime), 1, N'Timer', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (68, N'Svegliami', N'[["svegliami"]]', NULL, N'[''NAME su!, sveglia, sono le TIME'']', N'All', N'["ok","va bene","certo","bene"]', 1, 0, NULL, N'/Images/Shortcuts/alarmClock.gif', 0, N'SetAlarmClock', 1, N'SystemDialogueSetAlarmClock', 0, NULL, 0, CAST(N'2022-02-01T15:09:00.000' AS DateTime), 1, N'Timer', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (69, N'Timer:_Elimina', N'[["elimina","rimuovi","cancella"],["timer"]]', NULL, NULL, N'All', N'["Eliminato","Rimosso"]', 1, 0, NULL, N'/Images/Shortcuts/timer-delete.png', 0, N'SystemDeleteTimer', 1, N'SystemDialogueDeleteTimer', 0, NULL, 0, CAST(N'2022-02-02T14:44:32.597' AS DateTime), 1, N'Timer', 0)
INSERT [dbo].[WebSpeeches] ([Id], [Name], [Phrase], [Operation], [Parameters], [Host], [Answer], [FinalStep], [UserId], [ParentIds], [Ico], [Order], [Type], [OperationEnable], [SubType], [Step], [StepType], [ElementIndex], [InsDateTime], [Groupable], [GroupName], [GroupOrder]) VALUES (71, N'Sveglia:_Elimina', N'[["elimina","rimuovi","cancella"],["sveglia"]]', NULL, NULL, N'All', N'["Eliminato","Rimosso"]', 1, 0, NULL, N'/Images/Shortcuts/alarmClock-delete.png', 0, N'SystemDeleteAlarmClock', 1, N'SystemDialogueDeleteAlarmClock', 0, NULL, 0, CAST(N'2022-02-02T21:23:19.927' AS DateTime), 1, N'Timer', 0)
SET IDENTITY_INSERT [dbo].[WebSpeeches] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__UserRole__F9B8A48B1C9ABB32]    Script Date: 15/11/2022 23:01:49 ******/
ALTER TABLE [auth].[UserRoleTypes] ADD UNIQUE NONCLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__UserRole__F9B8A48BFF704118]    Script Date: 15/11/2022 23:01:49 ******/
ALTER TABLE [auth].[UserRoleTypes] ADD UNIQUE NONCLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__C9F28456912940F4]    Script Date: 15/11/2022 23:01:49 ******/
ALTER TABLE [auth].[Users] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__C9F28456A789176C]    Script Date: 15/11/2022 23:01:49 ******/
ALTER TABLE [auth].[Users] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__WebSpeec__A20D0832C5693F06]    Script Date: 15/11/2022 23:01:49 ******/
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
ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_Groupable]  DEFAULT ((1)) FOR [Groupable]
GO
ALTER TABLE [dbo].[WebSpeeches] ADD  CONSTRAINT [DF_WebSpeeches_GroupOrder]  DEFAULT ((0)) FOR [GroupOrder]
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
