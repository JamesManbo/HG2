
Create PROCEDURE dm_them_sua_kenh_tin
	@ma_kenh_tin varchar(50),
	@ten_danh_muc varchar(500)='',
	@is_hien_thi bit = null,
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
	IF EXISTS(SELECT top 1 1 from dm_kenh_tin where ma_kenh_tin=@ma_kenh_tin)
	BEGIN			
			Update dm_kenh_tin set ten_danh_muc=@ten_danh_muc,
				UpdatedDateUtc=GETDATE(),
				UpdatedUid=@uid,
				Deleted=0,UidName=@uid_name,Stt=@stt
			where ma_kenh_tin=@ma_kenh_tin

			IF(@@ROWCOUNT>0)
			SET @ma_loi=@thanh_cong
	END
	ELSE	
	BEGIN
			
		INSERT INTO 
			dm_kenh_tin(
				ma_kenh_tin,ten_danh_muc,
				CreatedDateUtc,CreatedUid,UpdatedDateUtc,UpdatedUid,Stt,Status,Deleted,UidName)
		VALUES (@ma_kenh_tin,@ten_danh_muc,GETDATE(),@uid,GETDATE(),@uid,@stt,1,0,@uid_name)
		IF(@@ROWCOUNT>0)
		SET @ma_loi=@thanh_cong
		
	END
END
