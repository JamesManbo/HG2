USE [HG]
GO

/****** Object:  Table [dbo].[ghs_tuyen_sinh_cap_thcs]    Script Date: 7/25/2022 6:00:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ghs_tuyen_sinh_cap_thcs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ma_ho_so] [varchar](25) NULL,
	[nam_hoc] [nvarchar](50) NULL,
	[thong_tin_chi_tiet] [nvarchar](500) NOT NULL,
	[id_truong] [int] NULL,
	[ho_ten_hoc_sinh] [nvarchar](250) NULL,
	[so_dinh_danh] [nvarchar](250) NULL,
	[ngay_sinh] [datetime] NULL,
	[ma_gioi_tinh] [varchar](25) NULL,
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
	[ten_truong] [nvarchar](500) NULL,
	[ma_uu_tien] [varchar](25) NULL,
	[diem_uu_tien] [decimal](18, 2) NULL,
	[dat_giai] [nvarchar](200) NULL,
	[diem_khuyen_khich_dat_giai] [decimal](18, 2) NULL,
	[giai_khac] [nvarchar](200) NULL,
	[diem_khuyen_khich_giai_khac] [decimal](18, 2) NULL,
	[lop_1_kq] [varchar](25) NULL,
	[lop_2_kq] [varchar](25) NULL,
	[lop_3_kq] [varchar](25) NULL,
	[lop_4_kq] [varchar](25) NULL,
	[lop_5_kq] [varchar](25) NULL,
	[CreatedDateUtc] [datetime] NULL,
	[CreatedUid] [uniqueidentifier] NULL,
	[UpdatedDateUtc] [datetime] NULL,
	[UpdatedUid] [uniqueidentifier] NULL,
	[Status] [int] NULL,
	[Deleted] [int] NULL,
	[DeletedBy] [uniqueidentifier] NULL,
	[UidName] [nvarchar](50) NULL,
	[stt] [int] NULL,
 CONSTRAINT [PK_ghs_tuyen_sinh_cap_thcs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


