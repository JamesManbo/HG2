using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.DanhMuc
{
    public class DanhMucDao : BaseDAL
    {
        public DanhMucDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Phong Ban

        //public List<DanhSachPhongBan> DanhSachPhongBan(string ma_phong_ban, int page, int pageSize, out int total)
        //{
        //    total = 0;
        //    DbProvider.SetCommandText2("dm_danh_sach_phong_ban_tong", CommandType.StoredProcedure);
        //    // Input params
        //    DbProvider.AddParameter("ma_phong_ban", ma_phong_ban, SqlDbType.Char);
        //    DbProvider.AddParameter("page", page, SqlDbType.Int);
        //    DbProvider.AddParameter("pageSize", pageSize, SqlDbType.Int);
        //    // Output params
        //    DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
        //    // Lấy về danh sách các trường học
        //    var obj = DbProvider.ExecuteListObject<DanhSachPhongBan>();
        //    total = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
        //    return obj;
        //}

        public List<DanhSachNguoiDung> DanhSachNguoiDung(string ma_phong_ban)
        {
            DbProvider.SetCommandText2("user_danh_sach_nguoi_dung", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_phong_ban", ma_phong_ban, SqlDbType.Char);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteListObject<DanhSachNguoiDung>();
            return obj;
        }

        public int LuuPhongBan(Dm_Phong_Ban item)
        {
            DbProvider.SetCommandText2("dm_them_sua_phong_ban", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_phong_ban", item.ten_phong_ban, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("nguoi_dai_dien", item.nguoi_dai_dien, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaPhongBan(string ma_phong_ban, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_phong_ban", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_phong_ban", ma_phong_ban, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Linh vực
        public int LuuLinhVuc(Dm_Linh_Vuc item)
        {
            DbProvider.SetCommandText2("dm_them_sua_linh_vuc", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_linh_vuc", item.ma_linh_vuc, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_linh_vuc", item.ten_linh_vuc, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaLinhVuc(string ma_linh_vuc, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_linh_vuc", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_linh_vuc", ma_linh_vuc, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion
    }
}