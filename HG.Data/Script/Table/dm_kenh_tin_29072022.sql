USE [HG]
GO

/****** Object:  Table [dbo].[dm_kenh_tin]    Script Date: 7/29/2022 9:09:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[dm_kenh_tin](
	[ma_kenh_tin] [varchar](50) NOT NULL,
	[ten_danh_muc] [nvarchar](500) NULL,
	[is_hien_thi] [bit] NULL,
	[Stt] [int] NULL,
	[Status] [int] NULL,
	[CreatedDateUtc] [datetime] NULL,
	[CreatedUid] [uniqueidentifier] NULL,
	[UpdatedDateUtc] [datetime] NULL,
	[UpdatedUid] [uniqueidentifier] NULL,
	[Deleted] [int] NULL,
	[DeletedBy] [uniqueidentifier] NULL,
	[UidName] [nvarchar](50) NULL,
 CONSTRAINT [PK_dm_kenh_tien] PRIMARY KEY CLUSTERED 
(
	[ma_kenh_tin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


