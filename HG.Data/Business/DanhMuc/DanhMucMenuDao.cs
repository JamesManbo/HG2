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
    public class DanhMucMenuDao : BaseDAL
    {
        public DanhMucMenuDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Menu chính      

        public int LuuMenu(Dm_menu item)
        {
            DbProvider.SetCommandText2("dm_them_sua_menu", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_trang", item.ma_trang, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_trang", item.ten_trang, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("ma_trang_cha", item.ma_trang_cha, SqlDbType.VarChar);
            DbProvider.AddParameter("level", item.level, SqlDbType.Int);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaMenu(string ma_trang, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_menu", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_phong_ban", ma_trang, SqlDbType.VarChar);
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