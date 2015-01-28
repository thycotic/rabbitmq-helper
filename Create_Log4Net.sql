USE [master]
GO
/****** Object:  Database [Log4NetSSA2]    Script Date: 1/28/2015 6:16:26 AM ******/
CREATE DATABASE [Log4NetSSA2]
GO

USE [Log4NetSSA2]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 1/28/2015 6:16:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserID] [nvarchar](50) NULL,
	[ServiceRole] [nvarchar](255) NULL,
	[Correlation] [nvarchar](50) NULL,
	[Context] [nvarchar](1000) NULL,
	[Thread] [nvarchar](255) NOT NULL,
	[Level] [nvarchar](50) NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_Log_Date]    Script Date: 1/28/2015 6:16:26 AM ******/
CREATE NONCLUSTERED INDEX [IX_Log_Date] ON [dbo].[Log]
(
	[Date] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Log_ServiceRole_Correlation]    Script Date: 1/28/2015 6:16:26 AM ******/
CREATE NONCLUSTERED INDEX [IX_Log_ServiceRole_Correlation] ON [dbo].[Log]
(
	[ServiceRole] ASC,
	[Correlation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
