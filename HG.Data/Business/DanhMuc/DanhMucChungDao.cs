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
    public class DanhMucChungDao : BaseDAL
    {
        public DanhMucChungDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Quốc tịch

        public int LuuQuocTich(Dm_Quoc_Tich item)
        {
            DbProvider.SetCommandText2("dm_them_sua_quoc_tich", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_quoc_tich", item.ma_quoc_tich, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_quoc_tich", item.ten_quoc_tich, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);         
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaQuocTich(string ma_quoc_tich, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_quoc_tich", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_quoc_tich", ma_quoc_tich, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Giới tính

        public int LuuGioiTinh(Dm_Gioi_Tinh item)
        {
            DbProvider.SetCommandText2("dm_them_sua_gioi_tinh", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_gioi_tinh", item.ma_gioi_tinh, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_gioi_tinh", item.ten_gioi_tinh, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaGioiTinh(string ma_gioi_tinh, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_gioi_tinh", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_gioi_tinh", ma_gioi_tinh, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Tôn giáo

        public int LuuTonGiao(Dm_Ton_Giao item)
        {
            DbProvider.SetCommandText2("dm_them_sua_ton_giao", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_ton_giao", item.ma_ton_giao, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_ton_giao", item.ten_ton_giao, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaTonGiao(string ma_ton_giao, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_ton_giao", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_ton_giao", ma_ton_giao, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Dân tộc

        public int LuuDanToc(Dm_Dan_Toc item)
        {
            DbProvider.SetCommandText2("dm_them_sua_dan_toc", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_dan_toc", item.ma_dan_toc, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_dan_toc", item.ten_dan_toc, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("ma_so", item.ma_so, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_goi_khac", item.ten_goi_khac, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaDanToc(string ma_dan_toc, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_dan_toc", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_dan_toc", ma_dan_toc, SqlDbType.VarChar);
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