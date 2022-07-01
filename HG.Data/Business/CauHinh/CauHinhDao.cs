using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.CauHinh
{
    public class CauHinhDao : BaseDAL
    {
        public CauHinhDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public List<Nguoi_PHXLModel> LayDsNhomPhanTrang(Guid ma_nguoi_dung)
        {
            try
            {
                List<Nguoi_PHXLModel> nguoi_PHXLModel = new List<Nguoi_PHXLModel>();
                DbProvider.SetCommandText2("LayDanhSachLuongBoiNguoiDung", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@ma_nguoi_dung", ma_nguoi_dung, SqlDbType.UniqueIdentifier);
             
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                nguoi_PHXLModel = DbProvider.ExecuteListObject<Nguoi_PHXLModel>();
                var maloi = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                if(maloi == 1)
                {
                    return new List<Nguoi_PHXLModel>();
                }
                return nguoi_PHXLModel;
            }
            catch(Exception e) {
                return new List<Nguoi_PHXLModel>();  }
        }
        public string XoaNguoiXL(Guid ma_nguoi_dung, Guid uid)
        {
            try
            {
                List<Nguoi_PHXLModel> nguoi_PHXLModel = new List<Nguoi_PHXLModel>();
                DbProvider.SetCommandText2("XoaNguoiXuLy", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@ma_nguoi_dung", ma_nguoi_dung, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("@uid", uid, SqlDbType.UniqueIdentifier);

                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                nguoi_PHXLModel = DbProvider.ExecuteListObject<Nguoi_PHXLModel>();
                var maloi = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                if (maloi == 1)
                {
                    return "Xóa thất bại!";
                }
                return "Xóa thành công!";
            }
            catch (Exception e)
            {
                return "Xóa thất bại!";
            }
        }
    }
}
