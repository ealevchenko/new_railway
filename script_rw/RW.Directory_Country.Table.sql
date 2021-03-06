USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[Directory_Country]    Script Date: 08.11.2018 17:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[Directory_Country](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[country_ru] [nvarchar](100) NOT NULL,
	[country_en] [nvarchar](100) NOT NULL,
	[code] [int] NOT NULL,
 CONSTRAINT [PK_Directory_Country] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [RW].[Directory_Country] ON 

INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (0, N'?', N'?', 0)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (1, N'Украина', N'Украина', 804)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (2, N'Литва', N'Литва', 440)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (3, N'Россия', N'Россия', 643)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (4, N'Эстония', N'Эстония', 233)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (5, N'Казахстан', N'Казахстан', 398)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (6, N'Молдавия', N'Молдавия', 498)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (7, N'Белоруссия', N'Белоруссия', 112)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (8, N'Азербайджан', N'Азербайджан', 31)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (9, N'Латвия', N'Латвия', 428)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (10, N'Босния и Герцеговина', N'Босния и Герцеговина', 70)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (11, N'Египет', N'Египет', 818)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (12, N'Алжир', N'Алжир', 12)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (13, N'Израиль', N'Израиль', 376)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (14, N'Ирак', N'Ирак', 368)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (15, N'Турция', N'Турция', 792)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (16, N'Румыния', N'Румыния', 642)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (17, N'Сенегал', N'Сенегал', 686)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (18, N'Гамбия', N'Гамбия', 270)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (19, N'ОАЭ', N'ОАЭ', 784)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (20, N'Польша', N'Польша', 616)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (21, N'Чехия', N'Чехия', 203)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (22, N'Нидерланды', N'Нидерланды', 528)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (23, N'Мавритания', N'Мавритания', 478)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (24, N'Камерун', N'Камерун', 120)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (25, N'Ливан', N'Ливан', 422)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (26, N'Коста-Рика', N'Коста-Рика', 188)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (27, N'Грузия', N'Грузия', 268)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (28, N'Сирия', N'Сирия', 760)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (29, N'Туркмения', N'Туркмения', 795)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (30, N'Иран', N'Иран', 364)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (31, N'Гана', N'Гана', 288)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (32, N'Габон', N'Габон', 266)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (33, N'Того', N'Того', 768)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (34, N'Бельгия', N'Бельгия', 56)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (35, N'Испания', N'Испания', 724)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (36, N'Нигерия', N'Нигерия', 566)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (37, N'Тунис', N'Тунис', 788)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (38, N'Нигер', N'Нигер', 562)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (39, N'Гвинея', N'Гвинея', 324)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (40, N'Кот-д’Ивуар', N'Кот-д’Ивуар', 384)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (41, N'Буркина-Фасо', N'Буркина-Фасо', 854)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (42, N'Мали', N'Мали', 466)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (43, N'Индия', N'Индия', 356)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (44, N'Италия', N'Италия', 380)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (45, N'Ливия', N'Ливия', 434)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (46, N'Болгария', N'Болгария', 100)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (47, N'Греция', N'Греция', 300)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (48, N'Кения', N'Кения', 404)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (49, N'Танзания', N'Танзания', 834)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (50, N'Руанда', N'Руанда', 646)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (51, N'Уганда', N'Уганда', 800)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (52, N'Германия', N'Германия', 276)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (53, N'Македония', N'Македония', 807)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (54, N'Бурунди', N'Бурунди', 108)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (55, N'Бенин', N'Бенин', 204)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (56, N'Великобритания', N'Великобритания', 826)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (57, N'Узбекистан', N'Узбекистан', 860)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (58, N'Ангола', N'Ангола', 24)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (59, N'Индонезия', N'Индонезия', 360)
INSERT [RW].[Directory_Country] ([id], [country_ru], [country_en], [code]) VALUES (60, N'Иордания', N'Иордания', 400)
SET IDENTITY_INSERT [RW].[Directory_Country] OFF
