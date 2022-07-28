
Create PROCEDURE dm_xoa_kenh_tin
	@ma_kenh_tin varchar(500),
	@uid uniqueidentifier,
	@ma_loi int out 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here
	SET @ma_loi=1
	Update dm_kenh_tin set Deleted=1,DeletedBy=@uid,UpdatedDateUtc=GETDATE() where Exists
	(SELECT name from [dbo].[SplitStringChar] (@ma_kenh_tin,',') as t  WHERE dm_kenh_tin.ma_kenh_tin = t.Name  )
	IF(@@ROWCOUNT>0)
	SET @ma_loi=0

END
