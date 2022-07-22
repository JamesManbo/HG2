USE [HG]
GO
/****** Object:  StoredProcedure [dbo].[dm_them_sua_phong_ban]    Script Date: 7/20/2022 10:35:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--declare @ma_loi int exec dm_them_sua_phong_ban 'ttttt','aaaa','','00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000','2121',null @ma_loi out select @ma_loi
alter PROCEDURE [dbo].[ghs_them_sua_tuyen_sinh_cap_mam_non] 
	@Id int,
	@ma_ho_so varchar(25)='',
	@nam_hoc nvarchar(50)='',
	@thong_tin_chi_tiet nvarchar(500),
	@id_truong int=null,
	@ho_ten_hoc_sinh varchar(250)='',
	@so_dinh_danh varchar(250)='',
	@ngay_sinh datetime=null,
	@gioi_tinh int=null,
	@ma_dan_toc varchar(25)='',
	@id_tinh_noi_sinh int=null,
	@id_huyen_noi_sinh int=null,
	@id_tinh_noi_cu_tru int=null,
	@id_huyen_noi_cu_tru int=null,
	@id_xa_noi_cu_tru int=null,
	@thon_xom varchar(250)='',
	@ho_ten_lien_he varchar(250)='',
	@so_cccd_lien_he varchar(50)='',
	@dien_thoai_lien_he varchar(50)='',
	@id_nguyen_vong int=null,

	@uid uniqueidentifier,
	@uid_name nvarchar(50),
	@stt int=null,
	@ma_loi int out

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	DECLARE @thanh_cong int=0		
	SET @ma_loi = 99 	

	-- Kiểm tra trạng tồn tại thì cập nhật không thì thêm mới
	IF EXISTS(SELECT top 1 1 from ghs_tuyen_sinh_cap_mam_non where Id=@Id)
	BEGIN			
			Update ghs_tuyen_sinh_cap_mam_non 
			set 
			ma_ho_so=@ma_ho_so,
			nam_hoc=@nam_hoc,
			thong_tin_chi_tiet=@thong_tin_chi_tiet,
			id_truong=@id_truong,
			ho_ten_hoc_sinh=@ho_ten_hoc_sinh,
			so_dinh_danh=@so_dinh_danh,
			ngay_sinh=@ngay_sinh,
			gioi_tinh=@gioi_tinh,
			ma_dan_toc=@ma_dan_toc,
			id_tinh_noi_sinh=@id_tinh_noi_sinh,
			id_huyen_noi_sinh=@id_huyen_noi_sinh,
			id_tinh_noi_cu_tru=@id_tinh_noi_cu_tru,
			id_huyen_noi_cu_tru=@id_huyen_noi_cu_tru,
			id_xa_noi_cu_tru=@id_xa_noi_cu_tru,
			thon_xom=@thon_xom,
			ho_ten_lien_he=@ho_ten_lien_he,
			so_cccd_lien_he=@so_cccd_lien_he,
			dien_thoai_lien_he = @dien_thoai_lien_he,
			id_nguyen_vong = @id_nguyen_vong,

			UpdatedDateUtc=GETDATE(),
			UpdatedUid=@uid,
			Deleted=0,
			UidName=@uid_name
			--stt=@stt
			where ma_ho_so=@ma_ho_so

			IF(@@ROWCOUNT>0)
			SET @ma_loi=@thanh_cong
	END
	ELSE	
	BEGIN
			
		INSERT INTO ghs_tuyen_sinh_cap_mam_non 
		(
		ma_ho_so,
		nam_hoc,
		thong_tin_chi_tiet,
		id_truong,
		ho_ten_hoc_sinh,
		so_dinh_danh,
		ngay_sinh,
		gioi_tinh,
		ma_dan_toc,
		id_tinh_noi_sinh,
		id_huyen_noi_sinh,
		id_tinh_noi_cu_tru,
		id_huyen_noi_cu_tru,
		id_xa_noi_cu_tru,
		thon_xom,
		ho_ten_lien_he,
		so_cccd_lien_he,
		dien_thoai_lien_he,
		id_nguyen_vong,

		CreatedDateUtc,
		CreatedUid,
		UpdatedDateUtc,
		UpdatedUid,
		--stt,
		Status,
		Deleted,
		UidName
		)
		VALUES (
		@ma_ho_so,@nam_hoc,
		@thong_tin_chi_tiet,@id_truong,@ho_ten_hoc_sinh,@so_dinh_danh,
		@ngay_sinh,@gioi_tinh,@ma_dan_toc,@id_tinh_noi_sinh,@id_huyen_noi_sinh,
		@id_tinh_noi_cu_tru,@id_huyen_noi_cu_tru,@id_xa_noi_cu_tru, @thon_xom,
		@ho_ten_lien_he,@so_cccd_lien_he,@dien_thoai_lien_he,@id_nguyen_vong,
		
		GETDATE(),@uid,GETDATE(),@uid,
		--@stt,
		1,0,@uid_name)
		IF(@@ROWCOUNT>0)
		SET @ma_loi=@thanh_cong		
	END

	
		

	
	
END