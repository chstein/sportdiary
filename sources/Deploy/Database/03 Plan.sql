USE [Sporty]
GO

/****** Object:  Table [dbo].[Plan]    Script Date: 03/27/2011 22:32:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Plan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Duration] [int] NULL,
	[SportTypeId] [int] NOT NULL,
	[ZoneId] [int] NOT NULL,
	[Description] [text] NULL,
	[Distance] [float] NULL,
	[Date] [date] NOT NULL,
	[TrainingTypeId] [int] NULL,
	[IsFavorite] [bit] NOT NULL,
 CONSTRAINT [PK_Plan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Plan]  WITH CHECK ADD  CONSTRAINT [FK_Plan_SportType] FOREIGN KEY([SportTypeId])
REFERENCES [dbo].[SportType] ([Id])
GO

ALTER TABLE [dbo].[Plan] CHECK CONSTRAINT [FK_Plan_SportType]
GO

ALTER TABLE [dbo].[Plan]  WITH CHECK ADD  CONSTRAINT [FK_Plan_TrainingType] FOREIGN KEY([TrainingTypeId])
REFERENCES [dbo].[TrainingType] ([Id])
GO

ALTER TABLE [dbo].[Plan] CHECK CONSTRAINT [FK_Plan_TrainingType]
GO

ALTER TABLE [dbo].[Plan]  WITH CHECK ADD  CONSTRAINT [FK_Plan_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[Plan] CHECK CONSTRAINT [FK_Plan_User]
GO

ALTER TABLE [dbo].[Plan]  WITH CHECK ADD  CONSTRAINT [FK_Plan_Zone] FOREIGN KEY([ZoneId])
REFERENCES [dbo].[Zone] ([Id])
GO

ALTER TABLE [dbo].[Plan] CHECK CONSTRAINT [FK_Plan_Zone]
GO


