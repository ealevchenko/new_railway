USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[Directory_TypeCargo]    Script Date: 08.11.2018 17:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[Directory_TypeCargo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_group] [int] NOT NULL,
	[type_name_ru] [nvarchar](50) NOT NULL,
	[type_name_en] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Directory_TypeCargo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [RW].[Directory_TypeCargo] ON 

INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (0, 0, N'Не определен', N'Indefined')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (1, 0, N'Порожний', N'Empty')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (2, 2, N'	Балки', N'	Beams')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (3, 2, N'	Бензин', N'	Gasoline')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (4, 2, N'	Бензол', N'	Benzene')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (5, 2, N'	Выключатели', N'	Switches')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (6, 2, N'	Гематит', N'	Hematite')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (7, 2, N'	Глина', N'	Clay')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (8, 2, N'	Диз. Топливо', N'	Diesel')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (9, 2, N'	Доломит', N'	Dolomite')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (10, 2, N'	Заготовка', N'	Billet')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (11, 1, N'	Известняк', N'	Limestone')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (12, 2, N'	Камень', N'	Stone')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (13, 2, N'	Катанка', N'	Wire rod')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (14, 2, N'	Кварцит', N'	Quartzite')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (15, 2, N'	Кирпич', N'	Brick')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (16, 1, N'	Кокс', N'	Coke')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (17, 2, N'	Коксовая мелочь', N'	Coke breeze')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (18, 2, N'	Концентрат', N'	Concentrate')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (19, 1, N'	Концентрат угольный', N'	Coal concentrate')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (20, 2, N'	Краны', N'	Cranes')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (21, 1, N'	Лом', N'	Scrap')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (22, 2, N'	Магнезит', N'	Magnesite')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (23, 2, N'	Магний', N'	Magnesium')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (24, 2, N'	Магния хлорид', N'	Magnesium chloride')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (25, 2, N'	Масло', N'	Oil')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (26, 2, N'	Натрий', N'	Sodium')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (27, 2, N'	Оборудование', N'	Equipment')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (28, 2, N'	Огнеупоры', N'	Refractories')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (29, 2, N'	Окатыши', N'	Pellets')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (30, 2, N'	Отсев', N'	Screenings')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (31, 2, N'	Пек', N'	Peck')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (32, 2, N'	Песок', N'	Sand')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (33, 2, N'	Полукокс', N'	Semi-Coke')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (35, 2, N'	Прокат', N'	Rent')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (36, 2, N'	Пропсы', N'	Propsy')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (37, 2, N'	Прочие грузы', N'	Other goods')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (38, 2, N'	Реактивное топливо', N'	Jet fuel')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (39, 2, N'	Рельсы', N'	Rails')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (40, 2, N'	Руда', N'	Ore')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (41, 2, N'	Самосвалы', N'	Dumpers')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (42, 2, N'	Сборный груз', N'	General cargo')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (43, 2, N'	Селитра', N'	Nitrate')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (44, 2, N'	Серная кислота', N'	Sulphuric acid')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (45, 2, N'	Смола', N'	Resin')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (46, 2, N'	Сода', N'	Soda')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (47, 2, N'	Соль', N'	Salt')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (48, 2, N'	Сталь', N'	Steel')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (49, 2, N'	Сульфат', N'	Sulfate')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (50, 2, N'	Сырье КХП', N'	Raw Materials Of The CCP')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (51, 2, N'	Тепловоз', N'	Diesel locomotive')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (52, 2, N'	Теплушка', N'	Caboose')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (53, 2, N'	Торф', N'	Peat')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (54, 2, N'	Трубы', N'	Трубы')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (55, 2, N'	Тягачи', N'	Movers')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (56, 1, N'	Уголь', N'	Coal')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (57, 2, N'	Удобрения', N'	Fertilizers')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (58, 2, N'	Ферросплавы', N'	Ferroalloys')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (59, 2, N'	Цемент', N'	Cement')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (60, 2, N'	Чугун', N'	Cast iron')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (61, 2, N'	Шары', N'	Balls')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (62, 2, N'	Шины', N'	Tyres')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (63, 2, N'	Шлак', N'	The slag')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (64, 2, N'	Шпалы', N'	Sleepers')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (65, 2, N'	Щебень', N'	Macadam')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (66, 2, N' Аммофос', N' Ammophos')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (67, 2, N'Автомотрисы', N'Rail Cars')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (68, 2, N'Вода', N'Water')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (69, 2, N'Гравий', N'Gravel')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (70, 2, N'Железобетон', N'Reinforced concrete')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (72, 2, N'Дерево', N'Wood')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (74, 2, N'Мрамор', N'Мрамор')
INSERT [RW].[Directory_TypeCargo] ([id], [id_group], [type_name_ru], [type_name_en]) VALUES (76, 2, N'Хим. Комоненты', N'Хим. Комоненты')
SET IDENTITY_INSERT [RW].[Directory_TypeCargo] OFF
ALTER TABLE [RW].[Directory_TypeCargo]  WITH CHECK ADD  CONSTRAINT [FK_Directory_TypeCargo_Directory_GroupCargo] FOREIGN KEY([id_group])
REFERENCES [RW].[Directory_GroupCargo] ([id])
GO
ALTER TABLE [RW].[Directory_TypeCargo] CHECK CONSTRAINT [FK_Directory_TypeCargo_Directory_GroupCargo]
GO
