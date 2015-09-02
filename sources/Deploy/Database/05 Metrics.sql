USE [Sporty]
GO

/****** Object:  Table [dbo].[Metrics]    Script Date: 10/27/2011 15:53:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Metrics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Date] [date] NOT NULL,
	[Weight] [float] NULL,
	[RestingPulse] [int] NULL,
	[SleepDuration] [float] NULL,
	[SleepQuality] [smallint] NULL,
	[StressLevel] [smallint] NULL,
	[Motivation] [smallint] NULL,
	[Mood] [smallint] NULL,
	[Sick] [smallint] NULL,
	[Description] [text] NULL,
	[YesterdaysTraining] [smallint] NULL,
 CONSTRAINT [PK_Metrics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Metrics]  WITH CHECK ADD  CONSTRAINT [FK_Metrics_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[Metrics] CHECK CONSTRAINT [FK_Metrics_User]
GO


