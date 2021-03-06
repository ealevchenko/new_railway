USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[Directory_Cars]    Script Date: 08.11.2018 17:38:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[Directory_Cars](
	[num] [int] NOT NULL,
	[id_type] [int] NULL,
	[sap] [nvarchar](50) NULL,
	[note] [nvarchar](250) NULL,
	[lifting_capacity] [numeric](18, 3) NULL,
	[tare] [numeric](18, 3) NULL,
	[id_country] [int] NULL,
	[count_axles] [int] NULL,
	[is_output_uz] [bit] NULL,
	[user_create] [nvarchar](50) NOT NULL,
	[dt_create] [datetime] NOT NULL,
	[user_edit] [nvarchar](50) NULL,
	[dt_edit] [datetime] NULL,
 CONSTRAINT [PK_Directory_Cars] PRIMARY KEY CLUSTERED 
(
	[num] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RW].[Directory_Cars]  WITH CHECK ADD  CONSTRAINT [FK_Directory_Cars_Directory_TypeCars] FOREIGN KEY([id_type])
REFERENCES [RW].[Directory_TypeCars] ([id])
GO
ALTER TABLE [RW].[Directory_Cars] CHECK CONSTRAINT [FK_Directory_Cars_Directory_TypeCars]
GO
