USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[CarsInternal]    Script Date: 08.11.2018 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[CarsInternal](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_sostav] [int] NOT NULL,
	[id_arrival] [int] NOT NULL,
	[num] [int] NOT NULL,
	[dt_uz] [datetime] NULL,
	[dt_inp_amkr] [datetime] NULL,
	[dt_out_amkr] [datetime] NULL,
	[natur_kis_inp] [int] NULL,
	[natur_kis_out] [int] NULL,
	[natur_rw] [int] NULL,
	[user_create] [nvarchar](50) NULL,
	[dt_create] [datetime] NOT NULL,
	[user_close] [nvarchar](50) NULL,
	[dt_close] [datetime] NULL,
	[parent_id] [int] NULL,
 CONSTRAINT [PK_CarsInternal] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RW].[CarsInternal]  WITH CHECK ADD  CONSTRAINT [FK_CarsInternal_CarsInternal] FOREIGN KEY([parent_id])
REFERENCES [RW].[CarsInternal] ([id])
GO
ALTER TABLE [RW].[CarsInternal] CHECK CONSTRAINT [FK_CarsInternal_CarsInternal]
GO
ALTER TABLE [RW].[CarsInternal]  WITH CHECK ADD  CONSTRAINT [FK_CarsInternal_Directory_Cars] FOREIGN KEY([num])
REFERENCES [RW].[Directory_Cars] ([num])
GO
ALTER TABLE [RW].[CarsInternal] CHECK CONSTRAINT [FK_CarsInternal_Directory_Cars]
GO
