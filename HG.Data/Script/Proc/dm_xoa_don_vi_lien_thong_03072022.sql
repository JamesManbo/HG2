

Create PROCEDURE [dbo].[dm_xoa_don_vi_lien_thong]
	@ma_don_vi varchar(25),
	@uid uniqueidentifier,
	@ma_loi int out 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here
	SET @ma_loi=1
	Update dm_don_vi_lien_thong
	set Deleted=1,DeletedBy=@uid,UpdatedDateUtc=GETDATE() where Exists
	(SELECT name from [dbo].[SplitStringChar] (@ma_don_vi,',') as t 
		WHERE dm_don_vi_lien_thong.ma_don_vi = t.Name  )
	IF(@@ROWCOUNT>0)
	SET @ma_loi=0

END
