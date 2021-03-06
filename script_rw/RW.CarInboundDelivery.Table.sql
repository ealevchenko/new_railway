USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [RW].[CarInboundDelivery]    Script Date: 08.11.2018 17:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [RW].[CarInboundDelivery](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_car_internal] [int] NOT NULL,
	[datetime] [datetime] NULL,
	[composition_index] [nvarchar](50) NOT NULL,
	[id_arrival] [int] NOT NULL,
	[num_car] [int] NOT NULL,
	[position] [int] NOT NULL,
	[note] [nvarchar](250) NULL,
	[num_nakl_sap] [nvarchar](35) NULL,
	[country_code] [int] NULL,
	[id_country] [int] NULL,
	[weight_cargo] [numeric](18, 3) NULL,
	[num_doc_reweighing_sap] [int] NULL,
	[dt_doc_reweighing_sap] [datetime] NULL,
	[weight_reweighing_sap] [numeric](18, 3) NULL,
	[dt_reweighing_sap] [datetime] NULL,
	[post_reweighing_sap] [int] NULL,
	[cargo_code] [int] NULL,
	[id_cargo] [int] NULL,
	[material_code_sap] [nvarchar](18) NULL,
	[material_name_sap] [nvarchar](50) NULL,
	[station_shipment] [nvarchar](50) NULL,
	[station_shipment_code_sap] [nvarchar](3) NULL,
	[station_shipment_name_sap] [nvarchar](50) NULL,
	[consignee] [int] NOT NULL,
	[id_consignee] [int] NULL,
	[shop_code_sap] [nvarchar](4) NULL,
	[shop_name_sap] [nvarchar](50) NULL,
	[new_shop_code_sap] [nvarchar](4) NULL,
	[new_shop_name_sap] [nvarchar](50) NULL,
	[permission_unload_sap] [bit] NULL,
	[step1_sap] [bit] NULL,
	[step2_sap] [bit] NULL,
 CONSTRAINT [PK_CarInboundDelivery] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [RW].[CarInboundDelivery]  WITH CHECK ADD  CONSTRAINT [FK_CarInboundDelivery_CarsInternal] FOREIGN KEY([id_car_internal])
REFERENCES [RW].[CarsInternal] ([id])
GO
ALTER TABLE [RW].[CarInboundDelivery] CHECK CONSTRAINT [FK_CarInboundDelivery_CarsInternal]
GO
ALTER TABLE [RW].[CarInboundDelivery]  WITH CHECK ADD  CONSTRAINT [FK_CarInboundDelivery_Directory_Cargo] FOREIGN KEY([id_cargo])
REFERENCES [RW].[Directory_Cargo] ([id])
GO
ALTER TABLE [RW].[CarInboundDelivery] CHECK CONSTRAINT [FK_CarInboundDelivery_Directory_Cargo]
GO
ALTER TABLE [RW].[CarInboundDelivery]  WITH CHECK ADD  CONSTRAINT [FK_CarInboundDelivery_Directory_Consignee] FOREIGN KEY([id_consignee])
REFERENCES [RW].[Directory_Consignee] ([id])
GO
ALTER TABLE [RW].[CarInboundDelivery] CHECK CONSTRAINT [FK_CarInboundDelivery_Directory_Consignee]
GO
ALTER TABLE [RW].[CarInboundDelivery]  WITH CHECK ADD  CONSTRAINT [FK_CarInboundDelivery_Directory_Country] FOREIGN KEY([id_country])
REFERENCES [RW].[Directory_Country] ([id])
GO
ALTER TABLE [RW].[CarInboundDelivery] CHECK CONSTRAINT [FK_CarInboundDelivery_Directory_Country]
GO
