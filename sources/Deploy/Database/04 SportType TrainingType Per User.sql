USE [Sporty]
GO

/****** Object:  Table [dbo].[Plan]    Script Date: 03/27/2011 22:32:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[SportType] ADD
Type int null,
UserId uniqueidentifier null

GO

ALTER TABLE [dbo].[SportType]  WITH CHECK ADD  CONSTRAINT [FK_SportType_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[TrainingType] ADD
UserId uniqueidentifier null

GO

ALTER TABLE [dbo].[TrainingType]  WITH CHECK ADD  CONSTRAINT [FK_TrainingType_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[Zone] ADD
Color nvarchar(7) 
GO