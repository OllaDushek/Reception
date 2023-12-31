USE [master]
GO
/****** Object:  Database [Reception]    Script Date: 05.12.2023 16:39:21 ******/
CREATE DATABASE [Reception]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Reception', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Reception.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Reception_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Reception_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Reception] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Reception].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Reception] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Reception] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Reception] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Reception] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Reception] SET ARITHABORT OFF 
GO
ALTER DATABASE [Reception] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Reception] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Reception] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Reception] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Reception] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Reception] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Reception] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Reception] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Reception] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Reception] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Reception] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Reception] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Reception] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Reception] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Reception] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Reception] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Reception] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Reception] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Reception] SET  MULTI_USER 
GO
ALTER DATABASE [Reception] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Reception] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Reception] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Reception] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Reception] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Reception] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Reception] SET QUERY_STORE = ON
GO
ALTER DATABASE [Reception] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Reception]
GO
/****** Object:  Table [dbo].[CheckIn]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckIn](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateCheckIn] [datetime] NOT NULL,
	[DateCheckOut] [datetime] NOT NULL,
	[RoomID] [int] NOT NULL,
	[BusinessTrip] [bit] NOT NULL,
	[Sum] [money] NOT NULL,
	[StatusID] [int] NOT NULL,
	[WorkerID] [int] NOT NULL,
	[PaymentID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassRoom]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassRoom](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupVisitors]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupVisitors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VisitorID] [int] NOT NULL,
	[CheckInID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListDopService]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListDopService](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[CheckInID] [int] NOT NULL,
	[DayStart] [datetime] NOT NULL,
	[DayOver] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListService]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListService](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[CheckInID] [int] NOT NULL,
	[DayStart] [datetime] NOT NULL,
	[DayOver] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Photo] [nvarchar](100) NOT NULL,
	[NumberHuman] [int] NOT NULL,
	[RoomNumber] [int] NOT NULL,
	[ClassRoomID] [int] NOT NULL,
	[Cost] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Cost] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visitor]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visitor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](30) NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[Patronymic] [nvarchar](30) NULL,
	[Bith] [datetime] NOT NULL,
	[NumberPasport] [nvarchar](6) NOT NULL,
	[SeriesPassport] [nvarchar](4) NOT NULL,
	[Phone] [nvarchar](11) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Worker]    Script Date: 05.12.2023 16:39:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Worker](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](30) NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[Patronymic] [nvarchar](30) NULL,
	[Bith] [datetime] NOT NULL,
	[Phone] [nvarchar](11) NOT NULL,
	[RoleID] [int] NULL,
	[Login] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ClassRoom] ON 

INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (1, N'Все')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (2, N'Сюит')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (3, N'Апартамент')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (4, N'Люкс')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (5, N'Джуниор сюит')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (6, N'1-ая категория')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (7, N'2-ая категория')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (8, N'3-я категория')
INSERT [dbo].[ClassRoom] ([ID], [Name]) VALUES (9, N'4-ая категория')
SET IDENTITY_INSERT [dbo].[ClassRoom] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([ID], [Name]) VALUES (1, N'Картой')
INSERT [dbo].[Payment] ([ID], [Name]) VALUES (2, N'Наличными')
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([ID], [Name]) VALUES (1, N'Админ')
INSERT [dbo].[Role] ([ID], [Name]) VALUES (2, N'Портье')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Room] ON 

INSERT [dbo].[Room] ([ID], [Photo], [NumberHuman], [RoomNumber], [ClassRoomID], [Cost]) VALUES (1, N'C:\Users\Slava\Pictures\фоточки\руки.png', 3, 1, 2, 20000.0000)
INSERT [dbo].[Room] ([ID], [Photo], [NumberHuman], [RoomNumber], [ClassRoomID], [Cost]) VALUES (2, N'C:\Users\Slava\Pictures\Screenshots\зонт.png', 2, 4, 4, 10000.0000)
INSERT [dbo].[Room] ([ID], [Photo], [NumberHuman], [RoomNumber], [ClassRoomID], [Cost]) VALUES (3, N'C:\Users\Slava\Pictures\Screenshots\зонт.png', 2, 7, 7, 2000.0000)
INSERT [dbo].[Room] ([ID], [Photo], [NumberHuman], [RoomNumber], [ClassRoomID], [Cost]) VALUES (4, N'C:\Users\Slava\Pictures\Screenshots\зонт.png', 1, 9, 6, 5000.0000)
INSERT [dbo].[Room] ([ID], [Photo], [NumberHuman], [RoomNumber], [ClassRoomID], [Cost]) VALUES (5, N'C:\Users\Slava\Pictures\Screenshots\зонт.png', 1, 3, 7, 2000.0000)
SET IDENTITY_INSERT [dbo].[Room] OFF
GO
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([ID], [Name], [Cost]) VALUES (1, N'Уборка номера', 500.0000)
INSERT [dbo].[Service] ([ID], [Name], [Cost]) VALUES (2, N'Заказать такси', 1000.0000)
INSERT [dbo].[Service] ([ID], [Name], [Cost]) VALUES (3, N'Минибар', 5000.0000)
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 

INSERT [dbo].[Status] ([ID], [Name]) VALUES (1, N'Зарезервирован')
INSERT [dbo].[Status] ([ID], [Name]) VALUES (2, N'Забронирован')
INSERT [dbo].[Status] ([ID], [Name]) VALUES (3, N'Выселен')
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[Worker] ON 

INSERT [dbo].[Worker] ([ID], [LastName], [FirstName], [Patronymic], [Bith], [Phone], [RoleID], [Login], [Password]) VALUES (1, N'Шашков', N'Илья', N'Дмитриевич', CAST(N'1997-05-12T00:00:00.000' AS DateTime), N'89374376619', 2, N'123', N'123')
SET IDENTITY_INSERT [dbo].[Worker] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Worker__5E55825B23B9E4B5]    Script Date: 05.12.2023 16:39:23 ******/
ALTER TABLE [dbo].[Worker] ADD UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([ID])
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([ID])
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([ID])
GO
ALTER TABLE [dbo].[CheckIn]  WITH CHECK ADD FOREIGN KEY([WorkerID])
REFERENCES [dbo].[Worker] ([ID])
GO
ALTER TABLE [dbo].[GroupVisitors]  WITH CHECK ADD FOREIGN KEY([CheckInID])
REFERENCES [dbo].[CheckIn] ([ID])
GO
ALTER TABLE [dbo].[GroupVisitors]  WITH CHECK ADD FOREIGN KEY([VisitorID])
REFERENCES [dbo].[Visitor] ([ID])
GO
ALTER TABLE [dbo].[ListDopService]  WITH CHECK ADD FOREIGN KEY([CheckInID])
REFERENCES [dbo].[CheckIn] ([ID])
GO
ALTER TABLE [dbo].[ListDopService]  WITH CHECK ADD FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Service] ([ID])
GO
ALTER TABLE [dbo].[ListService]  WITH CHECK ADD FOREIGN KEY([CheckInID])
REFERENCES [dbo].[CheckIn] ([ID])
GO
ALTER TABLE [dbo].[ListService]  WITH CHECK ADD FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Service] ([ID])
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD FOREIGN KEY([ClassRoomID])
REFERENCES [dbo].[ClassRoom] ([ID])
GO
ALTER TABLE [dbo].[Worker]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
USE [master]
GO
ALTER DATABASE [Reception] SET  READ_WRITE 
GO
