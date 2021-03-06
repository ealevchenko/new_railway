USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[Directory_Cargo]    Script Date: 08.11.2018 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[Directory_Cargo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name_ru] [nvarchar](200) NOT NULL,
	[name_en] [nvarchar](200) NOT NULL,
	[name_full_ru] [nvarchar](500) NOT NULL,
	[name_full_en] [nvarchar](500) NOT NULL,
	[etsng] [int] NOT NULL,
	[id_type] [int] NOT NULL,
	[user_create] [nvarchar](50) NOT NULL,
	[dt_create] [datetime] NOT NULL,
	[user_edit] [nvarchar](50) NULL,
	[dt_edit] [datetime] NULL,
 CONSTRAINT [PK_Directory_Cargo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RW].[Directory_Cargo]  WITH CHECK ADD  CONSTRAINT [FK_Directory_Cargo_Directory_TypeCargo] FOREIGN KEY([id_type])
REFERENCES [RW].[Directory_TypeCargo] ([id])
GO
ALTER TABLE [RW].[Directory_Cargo] CHECK CONSTRAINT [FK_Directory_Cargo_Directory_TypeCargo]
GO
