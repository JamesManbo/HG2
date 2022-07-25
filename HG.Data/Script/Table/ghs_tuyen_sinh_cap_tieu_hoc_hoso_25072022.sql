USE [HG]
GO

/****** Object:  Table [dbo].[ghs_tuyen_sinh_cap_tieu_hoc_hoso]    Script Date: 7/25/2022 2:41:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ghs_tuyen_sinh_cap_tieu_hoc_hoso](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ten_thanh_phan] [nvarchar](500) NULL,
	[file_dinh_kem] [nvarchar](500) NULL,
	[bieu_mau] [nvarchar](500) NULL,
	[ban_chinh] [int] NULL,
	[ban_sao] [int] NULL,
	[ban_photo] [int] NULL,
	[id_ghs_tuyen_sinh_cap_tieu_hoc] [int] NULL,
	[CreatedDateUtc] [datetime] NULL,
	[CreatedUid] [uniqueidentifier] NULL,
	[UpdatedDateUtc] [datetime] NULL,
	[UpdatedUid] [uniqueidentifier] NULL,
	[Status] [int] NULL,
	[Deleted] [int] NULL,
	[DeletedBy] [uniqueidentifier] NULL,
	[UidName] [nvarchar](50) NULL,
	[stt] [int] NULL,
 CONSTRAINT [PK_ghs_tuyen_sinh_cap_tieu_hoc_hoso] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


