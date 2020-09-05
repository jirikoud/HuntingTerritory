/****** Object:  Table [dbo].[AclUser]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AclUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[PasswordHash] [nvarchar](250) NULL,
	[IsDisabled] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysUpdated] [datetime] NULL,
	[SysEditor] [int] NULL,
	[AccountType] [int] NOT NULL,
	[MaxTerritoryCount] [int] NOT NULL,
	[Fullname] [nvarchar](250) NOT NULL,
	[EmailCode] [nvarchar](50) NULL,
	[EmailCodeExpire] [datetime] NULL,
 CONSTRAINT [PK_AclUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Answer]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StringValue] [nvarchar](max) NULL,
	[FloatValue] [float] NULL,
	[IntValue] [int] NULL,
	[BoolValue] [bit] NULL,
	[OptionId] [int] NULL,
	[CheckInId] [int] NOT NULL,
	[QuestionId] [int] NOT NULL,
 CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailInfo]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SenderAddress] [nvarchar](max) NULL,
	[ReceiverAddress] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](250) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysCreator] [int] NOT NULL,
	[RetryCount] [int] NOT NULL,
	[SendState] [int] NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[SendDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[SysEditor] [int] NULL,
	[SysUpdated] [datetime] NULL,
 CONSTRAINT [PK_EmailInfoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckIn](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MapItemId] [int] NOT NULL,
	[QuestionnaireId] [int] NULL,
	[Note] [nvarchar](max) NULL,
	[CheckTime] [datetime] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysUpdated] [datetime] NULL,
	[SysCreator] [int] NOT NULL,
	[SysEditor] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CheckIns] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Order] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Shortcut] [nvarchar](10) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[DateFormat] [nvarchar](100) NOT NULL,
	[TimeFormat] [nvarchar](100) NOT NULL,
	[DateFormatJS] [nvarchar](100) NOT NULL,
	[TimeFormatJS] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MapArea]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapArea](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerritoryId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PolygonData] [nvarchar](max) NULL,
	[SysCreator] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysEditor] [int] NULL,
	[SysUpdated] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_MapAreas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MapItem]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocationX] [float] NOT NULL,
	[LocationY] [float] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[MapItemType] [int] NOT NULL,
	[TerritoryId] [int] NOT NULL,
	[SysCreator] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysEditor] [int] NULL,
	[SysUpdated] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_MapItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MapItemType]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapItemType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[TerritoryId] [int] NOT NULL,
	[SysCreator] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysEditor] [int] NULL,
	[SysUpdated] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MapItemTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelVersion]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelVersion](
	[Version] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_ModelVersions] PRIMARY KEY CLUSTERED 
(
	[Version] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Option]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Option](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysUpdated] [datetime] NULL,
	[SysCreator] [int] NOT NULL,
	[SysEditor] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Options] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionnaireId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[QuestionType] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysUpdated] [datetime] NULL,
	[SysCreator] [int] NOT NULL,
	[SysEditor] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsRequired] [bit] NOT NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questionnaire]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questionnaire](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MapItemTypeId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysUpdated] [datetime] NULL,
	[SysCreator] [int] NOT NULL,
	[SysEditor] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Questionnaires] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Territory]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Territory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[StewardId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysUpdated] [datetime] NULL,
	[SysCreator] [int] NOT NULL,
	[SysEditor] [int] NULL,
	[IsDemoTemplate] [bit] NOT NULL,
	[IsPublic] [bit] NOT NULL,
 CONSTRAINT [PK_Territories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerritoryUser]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerritoryUser](
	[AclUserId] [int] NOT NULL,
	[TerritoryId] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerritoryUserRole] [int] NOT NULL,
 CONSTRAINT [PK_TerritoryUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TerritoryUserContact]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TerritoryUserContact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TerritoryId] [int] NOT NULL,
	[AclUserId] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[SysCreator] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysEditor] [int] NULL,
	[SysUpdated] [datetime] NULL,
 CONSTRAINT [PK_TerritoryUserContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Track]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Track](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SysCreator] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Tracks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLocation]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLocation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SysCreator] [int] NOT NULL,
	[LocationX] [float] NOT NULL,
	[LocationY] [float] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[TrackId] [int] NOT NULL,
 CONSTRAINT [PK_UserLocations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMapPoint]    Script Date: 9/5/2020 10:26:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMapPoint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LocationX] [float] NOT NULL,
	[LocationY] [float] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsPublic] [bit] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[SysCreator] [int] NOT NULL,
	[SysEditor] [int] NULL,
	[SysUpdated] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[TerritoryId] [int] NOT NULL,
	[AclUserId] [int] NOT NULL,
 CONSTRAINT [PK_UserMapPoints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMapPointShare]    Script Date: 9/5/2020 10:26:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMapPointShare](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserMapPointId] [int] NOT NULL,
	[AclUserId] [int] NOT NULL,
 CONSTRAINT [PK_UserMapPointShares] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSession]    Script Date: 9/5/2020 10:26:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSession](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Session] [nvarchar](50) NOT NULL,
	[AclUserId] [int] NOT NULL,
	[SysCreated] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_UserSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_AclUser_Editor] ON [dbo].[AclUser]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Answer_CheckIn]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Answer_CheckIn] ON [dbo].[Answer]
(
	[CheckInId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Answer_Option]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Answer_Option] ON [dbo].[Answer]
(
	[OptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Answer_Question]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Answer_Question] ON [dbo].[Answer]
(
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_CheckIn_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_CheckIn_AclUser_Creator] ON [dbo].[CheckIn]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_CheckIn_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_CheckIn_AclUser_Editor] ON [dbo].[CheckIn]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_CheckIn_MapItem]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_CheckIn_MapItem] ON [dbo].[CheckIn]
(
	[MapItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_CheckIn_Questionnaire]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_CheckIn_Questionnaire] ON [dbo].[CheckIn]
(
	[QuestionnaireId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapArea_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapArea_Creator] ON [dbo].[MapArea]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapArea_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapArea_Editor] ON [dbo].[MapArea]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapArea_Territory]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapArea_Territory] ON [dbo].[MapArea]
(
	[TerritoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItem_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItem_AclUser_Creator] ON [dbo].[MapItem]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItem_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItem_AclUser_Editor] ON [dbo].[MapItem]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItem_MapItemType]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItem_MapItemType] ON [dbo].[MapItem]
(
	[MapItemType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItem_Territory]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItem_Territory] ON [dbo].[MapItem]
(
	[TerritoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItemType_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItemType_AclUser_Creator] ON [dbo].[MapItemType]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItemType_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItemType_AclUser_Editor] ON [dbo].[MapItemType]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_MapItemType_Territory]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_MapItemType_Territory] ON [dbo].[MapItemType]
(
	[TerritoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Option_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Option_AclUser_Creator] ON [dbo].[Option]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Option_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Option_AclUser_Editor] ON [dbo].[Option]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Option_Question]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Option_Question] ON [dbo].[Option]
(
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Question_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Question_AclUser_Creator] ON [dbo].[Question]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Question_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Question_AclUser_Editor] ON [dbo].[Question]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Question_Questionnaire]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Question_Questionnaire] ON [dbo].[Question]
(
	[QuestionnaireId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Questionnaire_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Questionnaire_AclUser_Creator] ON [dbo].[Questionnaire]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Questionnaire_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Questionnaire_AclUser_Editor] ON [dbo].[Questionnaire]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Questionnaire_MapItemType]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Questionnaire_MapItemType] ON [dbo].[Questionnaire]
(
	[MapItemTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Territory_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Territory_AclUser_Creator] ON [dbo].[Territory]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Territory_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Territory_AclUser_Editor] ON [dbo].[Territory]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Territory_AclUser_Steward]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Territory_AclUser_Steward] ON [dbo].[Territory]
(
	[StewardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerritoryUser_AclUser]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerritoryUser_AclUser] ON [dbo].[TerritoryUser]
(
	[AclUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerritoryUser_Territory]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerritoryUser_Territory] ON [dbo].[TerritoryUser]
(
	[TerritoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerritoryUserContact_AclUser]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerritoryUserContact_AclUser] ON [dbo].[TerritoryUserContact]
(
	[AclUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerritoryUserContact_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerritoryUserContact_Creator] ON [dbo].[TerritoryUserContact]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerritoryUserContact_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerritoryUserContact_Editor] ON [dbo].[TerritoryUserContact]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_TerritoryUserContact_Territory]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_TerritoryUserContact_Territory] ON [dbo].[TerritoryUserContact]
(
	[TerritoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Track_AclUser]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Track_AclUser] ON [dbo].[Track]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserLocation_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserLocation_AclUser_Creator] ON [dbo].[UserLocation]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserLocation_Track]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserLocation_Track] ON [dbo].[UserLocation]
(
	[TrackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserMapPoint_AclUser]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMapPoint_AclUser] ON [dbo].[UserMapPoint]
(
	[AclUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserMapPoint_AclUser_Creator]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMapPoint_AclUser_Creator] ON [dbo].[UserMapPoint]
(
	[SysCreator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserMapPoint_AclUser_Editor]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMapPoint_AclUser_Editor] ON [dbo].[UserMapPoint]
(
	[SysEditor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserMapPoint_Territory]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMapPoint_Territory] ON [dbo].[UserMapPoint]
(
	[TerritoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserMapPointShare_AclUser]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMapPointShare_AclUser] ON [dbo].[UserMapPointShare]
(
	[AclUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserMapPointShare_UserMapPoint]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserMapPointShare_UserMapPoint] ON [dbo].[UserMapPointShare]
(
	[UserMapPointId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_UserSession_AclUser]    Script Date: 9/5/2020 10:26:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_UserSession_AclUser] ON [dbo].[UserSession]
(
	[AclUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AclUser]  WITH CHECK ADD  CONSTRAINT [FK_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[AclUser] CHECK CONSTRAINT [FK_AclUser_Editor]
GO
ALTER TABLE [dbo].[Answer]  WITH CHECK ADD  CONSTRAINT [FK_Answer_CheckIn] FOREIGN KEY([CheckInId])
REFERENCES [dbo].[CheckIn] ([Id])
GO
ALTER TABLE [dbo].[Answer] CHECK CONSTRAINT [FK_Answer_CheckIn]
GO
ALTER TABLE [dbo].[Answer]  WITH CHECK ADD  CONSTRAINT [FK_Answer_Option] FOREIGN KEY([OptionId])
REFERENCES [dbo].[Option] ([Id])
GO
ALTER TABLE [dbo].[Answer] CHECK CONSTRAINT [FK_Answer_Option]
GO
ALTER TABLE [dbo].[Answer]  WITH CHECK ADD  CONSTRAINT [FK_Answer_Question] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Question] ([Id])
GO
ALTER TABLE [dbo].[Answer] CHECK CONSTRAINT [FK_Answer_Question]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_AclUser_Creator]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_AclUser_Editor]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_MapItem] FOREIGN KEY([MapItemId])
REFERENCES [dbo].[MapItem] ([Id])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_MapItem]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD  CONSTRAINT [FK_CheckIn_Questionnaire] FOREIGN KEY([QuestionnaireId])
REFERENCES [dbo].[Questionnaire] ([Id])
GO
ALTER TABLE [dbo].[CheckIn] CHECK CONSTRAINT [FK_CheckIn_Questionnaire]
GO
ALTER TABLE [dbo].[MapArea]  WITH CHECK ADD  CONSTRAINT [FK_MapArea_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[MapArea] CHECK CONSTRAINT [FK_MapArea_Creator]
GO
ALTER TABLE [dbo].[MapArea]  WITH CHECK ADD  CONSTRAINT [FK_MapArea_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[MapArea] CHECK CONSTRAINT [FK_MapArea_Editor]
GO
ALTER TABLE [dbo].[MapArea]  WITH CHECK ADD  CONSTRAINT [FK_MapArea_Territory] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territory] ([Id])
GO
ALTER TABLE [dbo].[MapArea] CHECK CONSTRAINT [FK_MapArea_Territory]
GO
ALTER TABLE [dbo].[MapItem]  WITH CHECK ADD  CONSTRAINT [FK_MapItem_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[MapItem] CHECK CONSTRAINT [FK_MapItem_AclUser_Creator]
GO
ALTER TABLE [dbo].[MapItem]  WITH CHECK ADD  CONSTRAINT [FK_MapItem_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[MapItem] CHECK CONSTRAINT [FK_MapItem_AclUser_Editor]
GO
ALTER TABLE [dbo].[MapItem]  WITH CHECK ADD  CONSTRAINT [FK_MapItem_MapItemType] FOREIGN KEY([MapItemType])
REFERENCES [dbo].[MapItemType] ([Id])
GO
ALTER TABLE [dbo].[MapItem] CHECK CONSTRAINT [FK_MapItem_MapItemType]
GO
ALTER TABLE [dbo].[MapItem]  WITH CHECK ADD  CONSTRAINT [FK_MapItem_Territory] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territory] ([Id])
GO
ALTER TABLE [dbo].[MapItem] CHECK CONSTRAINT [FK_MapItem_Territory]
GO
ALTER TABLE [dbo].[MapItemType]  WITH CHECK ADD  CONSTRAINT [FK_MapItemType_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[MapItemType] CHECK CONSTRAINT [FK_MapItemType_AclUser_Creator]
GO
ALTER TABLE [dbo].[MapItemType]  WITH CHECK ADD  CONSTRAINT [FK_MapItemType_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[MapItemType] CHECK CONSTRAINT [FK_MapItemType_AclUser_Editor]
GO
ALTER TABLE [dbo].[MapItemType]  WITH CHECK ADD  CONSTRAINT [FK_MapItemType_Territory] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territory] ([Id])
GO
ALTER TABLE [dbo].[MapItemType] CHECK CONSTRAINT [FK_MapItemType_Territory]
GO
ALTER TABLE [dbo].[Option]  WITH CHECK ADD  CONSTRAINT [FK_Option_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Option] CHECK CONSTRAINT [FK_Option_AclUser_Creator]
GO
ALTER TABLE [dbo].[Option]  WITH CHECK ADD  CONSTRAINT [FK_Option_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Option] CHECK CONSTRAINT [FK_Option_AclUser_Editor]
GO
ALTER TABLE [dbo].[Option]  WITH CHECK ADD  CONSTRAINT [FK_Option_Question] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Question] ([Id])
GO
ALTER TABLE [dbo].[Option] CHECK CONSTRAINT [FK_Option_Question]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_AclUser_Creator]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_AclUser_Editor]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_Questionnaire] FOREIGN KEY([QuestionnaireId])
REFERENCES [dbo].[Questionnaire] ([Id])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_Questionnaire]
GO
ALTER TABLE [dbo].[Questionnaire]  WITH CHECK ADD  CONSTRAINT [FK_Questionnaire_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Questionnaire] CHECK CONSTRAINT [FK_Questionnaire_AclUser_Creator]
GO
ALTER TABLE [dbo].[Questionnaire]  WITH CHECK ADD  CONSTRAINT [FK_Questionnaire_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Questionnaire] CHECK CONSTRAINT [FK_Questionnaire_AclUser_Editor]
GO
ALTER TABLE [dbo].[Questionnaire]  WITH CHECK ADD  CONSTRAINT [FK_Questionnaire_MapItemType] FOREIGN KEY([MapItemTypeId])
REFERENCES [dbo].[MapItemType] ([Id])
GO
ALTER TABLE [dbo].[Questionnaire] CHECK CONSTRAINT [FK_Questionnaire_MapItemType]
GO
ALTER TABLE [dbo].[Territory]  WITH CHECK ADD  CONSTRAINT [FK_Territory_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Territory] CHECK CONSTRAINT [FK_Territory_AclUser_Creator]
GO
ALTER TABLE [dbo].[Territory]  WITH CHECK ADD  CONSTRAINT [FK_Territory_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Territory] CHECK CONSTRAINT [FK_Territory_AclUser_Editor]
GO
ALTER TABLE [dbo].[Territory]  WITH CHECK ADD  CONSTRAINT [FK_Territory_AclUser_Steward] FOREIGN KEY([StewardId])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Territory] CHECK CONSTRAINT [FK_Territory_AclUser_Steward]
GO
ALTER TABLE [dbo].[TerritoryUser]  WITH CHECK ADD  CONSTRAINT [FK_TerritoryUser_AclUser] FOREIGN KEY([AclUserId])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[TerritoryUser] CHECK CONSTRAINT [FK_TerritoryUser_AclUser]
GO
ALTER TABLE [dbo].[TerritoryUser]  WITH CHECK ADD  CONSTRAINT [FK_TerritoryUser_Territory] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territory] ([Id])
GO
ALTER TABLE [dbo].[TerritoryUser] CHECK CONSTRAINT [FK_TerritoryUser_Territory]
GO
ALTER TABLE [dbo].[TerritoryUserContact]  WITH CHECK ADD  CONSTRAINT [FK_TerritoryUserContact_AclUser] FOREIGN KEY([AclUserId])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[TerritoryUserContact] CHECK CONSTRAINT [FK_TerritoryUserContact_AclUser]
GO
ALTER TABLE [dbo].[TerritoryUserContact]  WITH CHECK ADD  CONSTRAINT [FK_TerritoryUserContact_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[TerritoryUserContact] CHECK CONSTRAINT [FK_TerritoryUserContact_Creator]
GO
ALTER TABLE [dbo].[TerritoryUserContact]  WITH CHECK ADD  CONSTRAINT [FK_TerritoryUserContact_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[TerritoryUserContact] CHECK CONSTRAINT [FK_TerritoryUserContact_Editor]
GO
ALTER TABLE [dbo].[TerritoryUserContact]  WITH CHECK ADD  CONSTRAINT [FK_TerritoryUserContact_Territory] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territory] ([Id])
GO
ALTER TABLE [dbo].[TerritoryUserContact] CHECK CONSTRAINT [FK_TerritoryUserContact_Territory]
GO
ALTER TABLE [dbo].[Track]  WITH CHECK ADD  CONSTRAINT [FK_Track_AclUser] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[Track] CHECK CONSTRAINT [FK_Track_AclUser]
GO
ALTER TABLE [dbo].[UserLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserLocation_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[UserLocation] CHECK CONSTRAINT [FK_UserLocation_AclUser_Creator]
GO
ALTER TABLE [dbo].[UserLocation]  WITH CHECK ADD  CONSTRAINT [FK_UserLocation_Track] FOREIGN KEY([TrackId])
REFERENCES [dbo].[Track] ([Id])
GO
ALTER TABLE [dbo].[UserLocation] CHECK CONSTRAINT [FK_UserLocation_Track]
GO
ALTER TABLE [dbo].[UserMapPoint]  WITH CHECK ADD  CONSTRAINT [FK_UserMapPoint_AclUser] FOREIGN KEY([AclUserId])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[UserMapPoint] CHECK CONSTRAINT [FK_UserMapPoint_AclUser]
GO
ALTER TABLE [dbo].[UserMapPoint]  WITH CHECK ADD  CONSTRAINT [FK_UserMapPoint_AclUser_Creator] FOREIGN KEY([SysCreator])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[UserMapPoint] CHECK CONSTRAINT [FK_UserMapPoint_AclUser_Creator]
GO
ALTER TABLE [dbo].[UserMapPoint]  WITH CHECK ADD  CONSTRAINT [FK_UserMapPoint_AclUser_Editor] FOREIGN KEY([SysEditor])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[UserMapPoint] CHECK CONSTRAINT [FK_UserMapPoint_AclUser_Editor]
GO
ALTER TABLE [dbo].[UserMapPoint]  WITH CHECK ADD  CONSTRAINT [FK_UserMapPoint_Territory] FOREIGN KEY([TerritoryId])
REFERENCES [dbo].[Territory] ([Id])
GO
ALTER TABLE [dbo].[UserMapPoint] CHECK CONSTRAINT [FK_UserMapPoint_Territory]
GO
ALTER TABLE [dbo].[UserMapPointShare]  WITH CHECK ADD  CONSTRAINT [FK_UserMapPointShare_AclUser] FOREIGN KEY([AclUserId])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[UserMapPointShare] CHECK CONSTRAINT [FK_UserMapPointShare_AclUser]
GO
ALTER TABLE [dbo].[UserMapPointShare]  WITH CHECK ADD  CONSTRAINT [FK_UserMapPointShare_UserMapPoint] FOREIGN KEY([UserMapPointId])
REFERENCES [dbo].[UserMapPoint] ([Id])
GO
ALTER TABLE [dbo].[UserMapPointShare] CHECK CONSTRAINT [FK_UserMapPointShare_UserMapPoint]
GO
ALTER TABLE [dbo].[UserSession]  WITH CHECK ADD  CONSTRAINT [FK_UserSession_AclUser] FOREIGN KEY([AclUserId])
REFERENCES [dbo].[AclUser] ([Id])
GO
ALTER TABLE [dbo].[UserSession] CHECK CONSTRAINT [FK_UserSession_AclUser]
GO
