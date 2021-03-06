USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[Directory_TypeCars]    Script Date: 08.11.2018 17:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[Directory_TypeCars](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_group] [int] NOT NULL,
	[type_cars_ru] [nvarchar](50) NOT NULL,
	[type_cars_en] [nvarchar](50) NOT NULL,
	[type_cars_abr_ru] [nvarchar](5) NULL,
	[type_cars_abr_en] [nvarchar](5) NULL,
 CONSTRAINT [PK_Directory_TypeCars] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [RW].[Directory_TypeCars] ON 

INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (0, 0, N'Не определена', N'Not determined', N'?', N'?')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (1, 1, N'Цистерна', N'Tank', N'цс', N'цс')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (2, 1, N'Цистерна для перевозки бензола', N'Tank for transportation of benzene', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (3, 1, N'Цистерна для перевозки смолы', N'Tank for transportation of resin', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (4, 1, N'Цистерна для перевозки сульфата', N'Sulphate transportation tank', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (5, 2, N'Полувагон с крышей', N'Gondola car with a roof', N'пвк', N'пвк')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (6, 2, N'Полувагон открытый', N'Gondola open', N'пв', N'пв')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (7, 2, N'Полувагон шадринской конструкции', N'Gondola truck Shadrinskoy construction', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (8, 2, N'Полувагон глуходонный', N'Gondola car', N'глх', N'глх')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (9, 2, N'Полувагон люковой', N'Gondola car', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (10, 3, N'Хоппер', N'Hopper', N'хп', N'хп')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (11, 3, N'Коксовоз', N'Coke', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (12, 3, N'Цементовоз', N'Cement carrier', N'цмв', N'цмв')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (13, 3, N'Аглохоппер', N'Aglochopper', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (14, 3, N'Окатышевоз', N'Oakashishvoz', N'ов', N'ов')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (15, 4, N'Платформа', N'Platform', N'плф', N'плф')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (16, 4, N'Ленточная платформа', N'Ribbon platform', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (17, 4, N'Обрезная платформа', N'Edging platform', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (18, 4, N'Металлошихтовая платформа', N'Metal-clad platform', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (19, 4, N'Скрапная платформыа', N'Scrap flooring', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (20, 4, N'Платформа под заготовку', N'Platform for workpiece', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (21, 4, N'Платформа универсальная', N'Platform universal', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (22, 5, N'Думпкар', N'Dumpcar', N'дк', N'дк')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (23, 5, N'Думпкар под мусор', N'Dumpkar for rubbish', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (24, 5, N'Думпкар под граншлак', N'Dumpkar under the grind slag', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (25, 5, N'Думпкар под металлолом', N'Dumpkar for scrap metal', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (26, 6, N'Чугуновоз', N'Chigunovoz', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (27, 6, N'Шлаковоз', N'Slag truck', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (28, 6, N'Изложницы', N'Molds', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (29, 6, N'Ракета', N'Rocket', N'', N'')
INSERT [RW].[Directory_TypeCars] ([id], [id_group], [type_cars_ru], [type_cars_en], [type_cars_abr_ru], [type_cars_abr_en]) VALUES (30, 7, N'Крытый вагон', N'Boxcar', N'кр', N'кр')
SET IDENTITY_INSERT [RW].[Directory_TypeCars] OFF
ALTER TABLE [RW].[Directory_TypeCars]  WITH CHECK ADD  CONSTRAINT [FK_Directory_TypeCars_Directory_GroupCars] FOREIGN KEY([id_group])
REFERENCES [RW].[Directory_GroupCars] ([id])
GO
ALTER TABLE [RW].[Directory_TypeCars] CHECK CONSTRAINT [FK_Directory_TypeCars_Directory_GroupCars]
GO
