using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.DanhMuc
{
    public class LuongXuLyDao : BaseDAL
    {
        public LuongXuLyDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Luồng xử lý

        public List<Dm_Thu_Tuc> DanhSachThuTuc()
        {
            try
            {
                DbProvider.SetCommandText2("dm_danh_sach_thu_tuc_key", CommandType.StoredProcedure);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<Dm_Thu_Tuc>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<Dm_Thu_Tuc>();
            }
        }
        public Dm_Luong_Xu_Ly_paging DanhSanhLuongXuLy(DanhMucModel item)
        {
            try
            {
                Dm_Luong_Xu_Ly_paging menu = new Dm_Luong_Xu_Ly_paging();
                menu.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("dm_danh_sach_luong_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("tu_khoa", "", SqlDbType.NVarChar);
                // Input params
                DbProvider.AddParameter("page", item.CurrentPage, SqlDbType.Int);
                DbProvider.AddParameter("page_size", item.RecordsPerPage, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                menu.lstLuongXuLy = DbProvider.ExecuteListObject<Dm_Luong_Xu_Ly>();
                menu.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new Dm_Luong_Xu_Ly_paging();
            }
        }

        public int LuuLuongXuLy(Dm_Luong_Xu_Ly item)
        {
            DbProvider.SetCommandText2("dm_them_sua_luong_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_luong", item.ten_luong, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("tt_hai_gd", item.tt_hai_gd, SqlDbType.Bit);
            DbProvider.AddParameter("ma_thu_tuc", item.ma_thu_tuc, SqlDbType.NVarChar);
            DbProvider.AddParameter("file_excel", item.file_excel, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaLuongXuLy(string ma_luong, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_luong_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_luong", ma_luong, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion




    }
}