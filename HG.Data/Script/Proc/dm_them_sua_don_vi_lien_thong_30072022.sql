
Create PROCEDURE [dbo].dm_them_sua_don_vi_lien_thong
	@ma_don_vi varchar(25),
	@ten_don_vi varchar(50)='',
	@Ip varchar(50)='',
	@stt int=null,
	@uid uniqueidentifier,
	@uid_name nvarchar(50),
	@ma_loi int out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	DECLARE @thanh_cong int=0		
	SET @ma_loi = 99 	

	-- Kiểm tra trạng tồn tại thì cập nhật không thì thêm mới
	IF EXISTS(SELECT top 1 1 from dm_don_vi_lien_thong where ma_don_vi=@ma_don_vi)
	BEGIN			
			Update dm_don_vi_lien_thong set ten_don_vi=@ten_don_vi,
				Ip = @Ip,
				UpdatedDateUtc=GETDATE(),
				UpdatedUid=@uid,
				Deleted=0,
				UidName=@uid_name,
				Stt=@stt
			where ma_don_vi=@ma_don_vi

			IF(@@ROWCOUNT>0)
			SET @ma_loi=@thanh_cong
	END
	ELSE	
	BEGIN
			
		INSERT INTO 
			dm_don_vi_lien_thong(
				ma_don_vi,
				ten_don_vi,
				Ip,
				CreatedDateUtc,CreatedUid,UpdatedDateUtc,UpdatedUid,Stt,Status,Deleted,UidName)
		VALUES (@ma_don_vi,@ten_don_vi,@Ip,GETDATE(),@uid,GETDATE(),@uid,@stt,1,0,@uid_name)
		IF(@@ROWCOUNT>0)
		SET @ma_loi=@thanh_cong
		
	END
END
