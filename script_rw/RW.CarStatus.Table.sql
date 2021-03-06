USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[CarStatus]    Script Date: 08.11.2018 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[CarStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[status_ru] [nvarchar](50) NULL,
	[status_en] [nvarchar](50) NULL,
	[id_status_next] [int] NULL,
	[order] [int] NULL,
 CONSTRAINT [PK_CarStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [RW].[CarStatus] ON 

INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (1, N'на очистке', N'on cleaning', 7, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (2, N'на разогреве', N'warm-up', 8, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (3, N'под выгрузкой', N'under unloading', 11, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (4, N'под загрузкой', N'under load', 14, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (5, N'на отстое', N'sludge', 9, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (6, N'на ремонте', N'on repair', 10, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (7, N'после очистки', N'after cleaning', NULL, 2)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (8, N'после разогрева', N'after heating', NULL, 2)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (9, N'после отстоя', N'after sludge', NULL, 2)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (10, N'после ремонта', N'after repair', NULL, 2)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (11, N'для очистки', N'for the cleaning', NULL, 2)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (12, N'для отправки на УЗ', N'for sending to UZ', 16, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (13, N'прибывший с УЗ', N'arrived with UZ', NULL, NULL)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (14, N'после загрузки', N'after downloading', NULL, NULL)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (15, N'ожидаем прибытия с УЗ', N'we expect arrival with UZ', 13, 1)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (16, N'принят на УЗ', N'adopted by US', NULL, 2)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (17, N'транзит по УЗ', N'transit by UZ', NULL, NULL)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (18, N'возврат на УЗ', N'return to UZ', NULL, NULL)
INSERT [RW].[CarStatus] ([id], [status_ru], [status_en], [id_status_next], [order]) VALUES (19, N'ТСП по УЗ Кривого Рога', N'TSP on UZ Krivoy Rog', NULL, NULL)
SET IDENTITY_INSERT [RW].[CarStatus] OFF
