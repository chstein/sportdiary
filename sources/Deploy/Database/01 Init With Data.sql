USE [Sporty]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 02/16/2011 20:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Application] ([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) VALUES (N'/', N'/', N'd290bf6d-85cd-4a00-b62a-cf72cf477e8c', NULL)
/****** Object:  Table [dbo].[TrainingType]    Script Date: 02/16/2011 20:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TrainingType](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_TrainingType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[TrainingType] ([Id], [Name]) VALUES (1, N'Training')
INSERT [dbo].[TrainingType] ([Id], [Name]) VALUES (2, N'Competition')
INSERT [dbo].[TrainingType] ([Id], [Name]) VALUES (3, N'Test')
/****** Object:  Table [dbo].[SportType]    Script Date: 02/16/2011 20:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SportType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_SportType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SportType] ON
INSERT [dbo].[SportType] ([Id], [Name]) VALUES (1, N'Run')
INSERT [dbo].[SportType] ([Id], [Name]) VALUES (2, N'Swim')
INSERT [dbo].[SportType] ([Id], [Name]) VALUES (3, N'Cycle')
INSERT [dbo].[SportType] ([Id], [Name]) VALUES (4, N'Duathlon')
INSERT [dbo].[SportType] ([Id], [Name]) VALUES (5, N'Triathlon')
INSERT [dbo].[SportType] ([Id], [Name]) VALUES (11, N'Athletics')
SET IDENTITY_INSERT [dbo].[SportType] OFF
/****** Object:  Table [dbo].[Zone]    Script Date: 02/16/2011 20:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Zone](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Zone] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Zone] ([Id], [Name]) VALUES (1, N'Recovery')
INSERT [dbo].[Zone] ([Id], [Name]) VALUES (2, N'Endurance')
INSERT [dbo].[Zone] ([Id], [Name]) VALUES (3, N'Higher Endurance')
INSERT [dbo].[Zone] ([Id], [Name]) VALUES (4, N'Lactate Threshold')
INSERT [dbo].[Zone] ([Id], [Name]) VALUES (5, N'Anaerobic')
/****** Object:  Table [dbo].[User]    Script Date: 02/16/2011 20:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
	[ActivationCode] [uniqueidentifier] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[User] ([ApplicationId], [UserId], [Name], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment], [ActivationCode]) VALUES (N'd290bf6d-85cd-4a00-b62a-cf72cf477e8c', N'b5498922-b74b-45c9-b370-e932a1383ac1', N'steini', N'frettchen', 0, N'', NULL, N'steinisweb@web.de', N'steinisweb@web.de', NULL, NULL, 1, 0, CAST(0x00009E210104C49B AS DateTime), CAST(0x00009E210104C49B AS DateTime), CAST(0x00009E210104C49B AS DateTime), CAST(0x00009E210104C49B AS DateTime), 0, CAST(0x00009E210104C49B AS DateTime), 0, CAST(0x00009E210104C49B AS DateTime), NULL, NULL)
/****** Object:  Table [dbo].[Exercise]    Script Date: 02/16/2011 20:55:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exercise](
	[UserId] [uniqueidentifier] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [text] NULL,
	[Duration] [time](0) NULL,
	[Distance] [float] NULL,
	[Date] [datetime] NOT NULL,
	[SportTypeId] [int] NOT NULL,
	[Heartrate] [int] NULL,
	[TrainingTypeId] [int] NULL,
	[ZoneId] [int] NULL,
	[Speed] [float] NULL
 CONSTRAINT [PK_Exercise] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Exercise] ON
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 2, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2200000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 3, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2300000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 4, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2200000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 5, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2300000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 6, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2400000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 7, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2400000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 8, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2500000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 9, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2500000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 10, N'erste Mal mit GPS :-)
5 STL', CAST(0x00C80E0000000000 AS Time), 11.48, CAST(0x00009E2300000000 AS DateTime), 1, 138, 1, 2, 10.9)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 11, N'Schöne Runde von Rugensee nach Willigrad.', CAST(0x005C0D0000000000 AS Time), 10.6, CAST(0x00009E2700000000 AS DateTime), 1, 138, 1, 2, 10.9)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 12, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2800000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 13, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2800000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 14, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2900000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 15, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2900000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 16, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 17, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 18, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 19, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 20, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2C00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 21, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E2C00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 22, N'', CAST(0x00850B0000000000 AS Time), 8.6, CAST(0x00009E2900000000 AS DateTime), 1, 139, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 23, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3800000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 24, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3800000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 25, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3900000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 26, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3900000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 27, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 28, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 29, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3D00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 30, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3D00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 31, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3E00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 32, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E3E00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 33, N'runde durch tiefen schnee :)', CAST(0x00AC080000000000 AS Time), NULL, CAST(0x00009E4600000000 AS DateTime), 1, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 35, N'', CAST(0x008C0A0000000000 AS Time), 1.4, CAST(0x00009E4700000000 AS DateTime), 2, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 36, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E4700000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 37, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E4700000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 38, N'', CAST(0x00C40E0000000000 AS Time), 11, CAST(0x00009E4A00000000 AS DateTime), 1, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 39, N'', CAST(0x0034080000000000 AS Time), NULL, CAST(0x00009E4B00000000 AS DateTime), 2, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 40, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E4B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 41, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E4B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 42, N'', CAST(0x008C0A0000000000 AS Time), 1.3, CAST(0x00009E6300000000 AS DateTime), 2, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 43, N'Schnellere Runde quer durch die Gegend. Biesdorfer See, Sportplatz (AdK)', CAST(0x00040B0000000000 AS Time), 9.07, CAST(0x00009E6600000000 AS DateTime), 1, 152, 1, 3, 11.16)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 44, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6300000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 45, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6300000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 46, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6700000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 47, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6400000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 48, N'Runde mit Thomas im Tiefschnee in Strietfeld', CAST(0x00E0050000000000 AS Time), 4.17, CAST(0x00009E5A00000000 AS DateTime), 1, NULL, 1, 1, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 49, N'Runde bis zur Zimmermannstr. ', CAST(0x0067120000000000 AS Time), 13.35, CAST(0x00009E5000000000 AS DateTime), 1, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 50, N'Runde bei Glatteis und Schnee', CAST(0x00C2100000000000 AS Time), 11.88, CAST(0x00009E6100000000 AS DateTime), 1, 140, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 51, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6400000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 52, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6700000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 53, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6800000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 54, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6800000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 55, N'Runde bis zur Hessestr.', CAST(0x00010F0000000000 AS Time), 11, CAST(0x00009E6F00000000 AS DateTime), 1, 142, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 56, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6900000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 57, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6900000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 58, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 59, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 60, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 61, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 62, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6E00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 63, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6E00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 64, N'Schwere Einheit. Aber das Gefühl wird langsam besser :-)', CAST(0x0034080000000000 AS Time), 1.1, CAST(0x00009E6A00000000 AS DateTime), 2, NULL, 1, 3, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 65, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6F00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 66, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E6F00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 67, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7000000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 68, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7000000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 69, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7100000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 70, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7100000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 71, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7200000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 72, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7200000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 73, N'', CAST(0x0060090000000000 AS Time), 1.5, CAST(0x00009E7100000000 AS DateTime), 2, NULL, 1, 3, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 74, N'Athletik und Dehnung mit Bändern
Beine Hüftbreit
Abdukt. 3 x 15 (Weg 30-40cm) 
Addukt. 3 x 15 (Weg 30cm)
Kraularmzug probiert (Video?)
Crunch, 2x20 Hände auf Brusthöhe
Rudern, 2x15 Hände 20xm auseinander, Band doppelt
Liegestütz 2x15', CAST(0x00B80B0000000000 AS Time), NULL, CAST(0x00009E7700000000 AS DateTime), 11, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 75, N'leicht angeschlagen. Versuch trotzdem locker zu laufen.
Leider zusätzliche "Klopause". Zum Glück TT dabei ;-)', CAST(0x00000F0000000000 AS Time), 11.48, CAST(0x00009E7800000000 AS DateTime), 1, 150, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 76, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7C00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 77, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E7C00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 78, N'Athletik und Dehnung mit Bändern
Beine Hüftbreit
Abdukt. 4 x 15 (Weg 30-40cm) 
Addukt. 4 x 15 (Weg 30cm)
Kraularmzug probiert (Video?)
Crunch, 2x30 Hände auf Brusthöhe
Liegestütz 1x15', CAST(0x0008070000000000 AS Time), NULL, CAST(0x00009E7E00000000 AS DateTime), 11, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 79, N'Athletik und Dehnung mit Bändern
Beine Hüftbreit
Abdukt. 4 x 20
Addukt. 4 x 20
halben Kraularmzug 2x30 (oben)
ganzen Armzug 1x30', CAST(0x0008070000000000 AS Time), NULL, CAST(0x00009E8200000000 AS DateTime), 11, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 80, N'leicht angeschlagen. Versuch trotzdem locker zu laufen zur Videothek', CAST(0x0008070000000000 AS Time), 5.8, CAST(0x00009E8200000000 AS DateTime), 1, 140, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 81, N'lockere Runde Richtung Heesestr.
HAC 5 kaputt :-(', CAST(0x00000F0000000000 AS Time), 11.48, CAST(0x00009E8500000000 AS DateTime), 1, 145, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 82, N'lockere Runde um Kienberg, Ri Ahrensfelde, bis zur Straßenbahn', CAST(0x007C0B0000000000 AS Time), 9, CAST(0x00009E8700000000 AS DateTime), 1, 140, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 83, N'Runde bis kurz vor Ende Ahrensfelde. Musste wegen unterwasserstehendem Weg umkehren. Wunderbar gefühlt :-)', CAST(0x001C110000000000 AS Time), 13.68, CAST(0x00009E8B00000000 AS DateTime), 1, 143, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 84, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E8B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 85, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E8B00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 86, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E8A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 87, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E8A00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 88, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E8C00000000 AS DateTime), 3, NULL, 1, 2, NULL)
INSERT [dbo].[Exercise] ([UserId], [Id], [Description], [Duration], [Distance], [Date], [SportTypeId], [Heartrate], [TrainingTypeId], [ZoneId], [Speed]) VALUES (N'b5498922-b74b-45c9-b370-e932a1383ac1', 89, N'Arbeitsweg', CAST(0x00B0040000000000 AS Time), 10, CAST(0x00009E8C00000000 AS DateTime), 3, NULL, 1, 2, NULL)
SET IDENTITY_INSERT [dbo].[Exercise] OFF
/****** Object:  Default [DF__Applicati__Appli__276EDEB3]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[Application] ADD  DEFAULT (newid()) FOR [ApplicationId]
GO
/****** Object:  Default [DF__User__PasswordFo__286302EC]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[User] ADD  DEFAULT ((0)) FOR [PasswordFormat]
GO
/****** Object:  ForeignKey [FK_Exercise_SportType]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[Exercise]  WITH CHECK ADD  CONSTRAINT [FK_Exercise_SportType] FOREIGN KEY([SportTypeId])
REFERENCES [dbo].[SportType] ([Id])
GO
ALTER TABLE [dbo].[Exercise] CHECK CONSTRAINT [FK_Exercise_SportType]
GO
/****** Object:  ForeignKey [FK_Exercise_TrainingType]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[Exercise]  WITH CHECK ADD  CONSTRAINT [FK_Exercise_TrainingType] FOREIGN KEY([TrainingTypeId])
REFERENCES [dbo].[TrainingType] ([Id])
GO
ALTER TABLE [dbo].[Exercise] CHECK CONSTRAINT [FK_Exercise_TrainingType]
GO
/****** Object:  ForeignKey [FK_Exercise_User]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[Exercise]  WITH CHECK ADD  CONSTRAINT [FK_Exercise_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Exercise] CHECK CONSTRAINT [FK_Exercise_User]
GO
/****** Object:  ForeignKey [FK_Exercise_Zone]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[Exercise]  WITH CHECK ADD  CONSTRAINT [FK_Exercise_Zone] FOREIGN KEY([ZoneId])
REFERENCES [dbo].[Zone] ([Id])
GO
ALTER TABLE [dbo].[Exercise] CHECK CONSTRAINT [FK_Exercise_Zone]
GO
/****** Object:  ForeignKey [FK__User__Applicatio__29572725]    Script Date: 02/16/2011 20:55:37 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([ApplicationId])
GO
