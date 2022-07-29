USE [HG]
GO

/****** Object:  Table [dbo].[tin_tuc]    Script Date: 7/29/2022 9:10:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tin_tuc](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ma_dm_kenh_tin] [varchar](50) NULL,
	[tieu_de] [nvarchar](500) NULL,
	[noi_dung] [nvarchar](500) NULL,
	[noi_dung_chi_tiet] [nvarchar](max) NULL,
	[anh_dai_dien] [varchar](500) NULL,
	[is_hien_thi] [bit] NULL,
	[Stt] [int] NULL,
	[Status] [int] NULL,
	[CreatedDateUtc] [datetime] NULL,
	[CreatedUid] [uniqueidentifier] NULL,
	[UpdatedDateUtc] [datetime] NULL,
	[UpdatedUid] [uniqueidentifier] NULL,
	[Deleted] [int] NULL,
	[DeletedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_tin_tuc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


