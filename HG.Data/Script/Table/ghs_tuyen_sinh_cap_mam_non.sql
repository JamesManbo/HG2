USE [HG]
GO

/****** Object:  Table [dbo].[ghs_tuyen_sinh_cap_mam_non]    Script Date: 7/22/2022 7:48:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ghs_tuyen_sinh_cap_mam_non](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ma_ho_so] [varchar](25) NULL,
	[nam_hoc] [nvarchar](50) NULL,
	[thong_tin_chi_tiet] [nvarchar](500) NOT NULL,
	[id_truong] [int] NULL,
	[ho_ten_hoc_sinh] [nvarchar](250) NULL,
	[so_dinh_danh] [nvarchar](250) NULL,
	[ngay_sinh] [datetime] NULL,
	[gioi_tinh] [int] NULL,
	[ma_dan_toc] [varchar](25) NULL,
	[ma_dia_ban_tinh_noi_sinh] [varchar](50) NULL,
	[ma_dia_ban_huyen_noi_sinh] [varchar](50) NULL,
	[ma_dia_ban_tinh_noi_cu_tru] [varchar](50) NULL,
	[ma_dia_ban_huyen_noi_cu_tru] [varchar](50) NULL,
	[ma_dia_ban_xa_noi_cu_tru] [varchar](50) NULL,
	[thon_xom] [nvarchar](250) NULL,
	[ho_ten_lien_he] [nvarchar](250) NULL,
	[so_cccd_lien_he] [nvarchar](50) NULL,
	[dien_thoai_lien_he] [nvarchar](50) NULL,
	[id_nguyen_vong] [int] NULL,
	[CreatedDateUtc] [datetime] NULL,
	[CreatedUid] [uniqueidentifier] NULL,
	[UpdatedDateUtc] [datetime] NULL,
	[UpdatedUid] [uniqueidentifier] NULL,
	[Status] [int] NULL,
	[Deleted] [int] NULL,
	[DeletedBy] [uniqueidentifier] NULL,
	[UidName] [nvarchar](50) NULL,
	[stt] [int] NULL,
 CONSTRAINT [PK_ghs_tuyen_sinh_cap_mam_non_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


