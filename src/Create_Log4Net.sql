CREATE DATABASE SSDELog4Net;

GO

USE SSDELog4Net;

GO

-- DROP TABLE [dbo].[Log]

CREATE TABLE [dbo].[Log]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Date] [datetime] NOT NULL,
[HostName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserID] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ServiceRole] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Correlation] [nvarchar] (350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Context] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Thread] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Level] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Logger] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Message] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Exception] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Log_Date_Correlation_Level] ON [dbo].[Log] ([Date], [Correlation], [Level]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_Log_Date] ON [dbo].[Log] ([Date] DESC) ON [PRIMARY]

ALTER TABLE [dbo].[Log] ADD 
CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
