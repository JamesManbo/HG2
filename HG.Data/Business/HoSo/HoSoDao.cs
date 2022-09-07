using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.HoSo;
using HG.Entities.Search;
using HG.Entities.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.HoSo
{
    public class HoSoDao : BaseDAL
    {
        public HoSoDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public List<Ho_So> HoSoPaging(HoSoPaging item, out int total)
        {
            try
            {
                List<Ho_So> result = new List<Ho_So>();

                var abc = 1;
                if (item.CurrentPage > 1)
                {
                    abc = (item.CurrentPage - 1) * item.RecordsPerPage;
                }
                else if (item.CurrentPage == 1)
                {
                    abc = 0;
                }
                DbProvider.SetCommandText2("[dbo].[hoso$paging]", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("StartingRow", abc, SqlDbType.Int);
                DbProvider.AddParameter("RecordsPerPage", item.RecordsPerPage, SqlDbType.Int);
                DbProvider.AddParameter("tu_khoa", item.tu_khoa = null ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_thu_tuc", item.ma_thu_tuc, SqlDbType.NVarChar);
                DbProvider.AddParameter("tat_ca", item.tat_ca, SqlDbType.Int);
                DbProvider.AddParameter("dung_han", item.dung_han, SqlDbType.Int);
                DbProvider.AddParameter("qua_han", item.qua_han, SqlDbType.Int);
                DbProvider.AddParameter("trang_thai", item.trang_thai_hs, SqlDbType.Int);
                DbProvider.AddParameter("userid", item.userid == null ? "" : item.userid, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                result = DbProvider.ExecuteListObject<Ho_So>();
                total = result.Count();
                return result;
            }
            catch (Exception e)
            {
                total = 0;
                return new List<Ho_So>();
            }
        }
        public List<Ho_So> TimKiemHoSoTheoTieuChi(HoSoPaging item , HoSoFilter hoSoFilter, out int total)
        {
            try
            {
                List<Ho_So> result = new List<Ho_So>();
                var abc = 1;
                if (item.CurrentPage > 1)
                {
                    abc = (item.CurrentPage - 1) * item.RecordsPerPage;
                }
                else if (item.CurrentPage == 1)
                {
                    abc = 0;
                }
                DbProvider.SetCommandText2("[dbo].[hoso$filter]", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("StartingRow", abc, SqlDbType.Int);
                DbProvider.AddParameter("RecordsPerPage", item.RecordsPerPage, SqlDbType.Int);
                DbProvider.AddParameter("ma_trang_thai",  hoSoFilter.ma_trang_thai, SqlDbType.Int);
                DbProvider.AddParameter("ma_ho_so",  hoSoFilter.ma_ho_so, SqlDbType.Int);
                DbProvider.AddParameter("ten_nguoi_nop",  hoSoFilter.ten_nguoi_nop, SqlDbType.NVarChar);
                DbProvider.AddParameter("can_bo_tiep_nhan", hoSoFilter.can_bo_tiep_nhan, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_phong_ban",  hoSoFilter.ma_phong_ban, SqlDbType.NVarChar);
                DbProvider.AddParameter("can_bo_xu_ly", hoSoFilter.can_bo_xu_ly, SqlDbType.NVarChar);
                DbProvider.AddParameter("tinh_trang",  hoSoFilter.tinh_trang ?? "00", SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_linh_vuc",  hoSoFilter.ma_linh_vuc , SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_thu_tuc_hc",  hoSoFilter.ma_thu_tuc_hc, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                result = DbProvider.ExecuteListObject<Ho_So>();
                total = Convert.ToInt32(DbProvider.Command.Parameters["tong_ban_ghi"].Value.ToString());
                return result;
            }
            catch (Exception e)
            {
                total = 0;
                return new List<Ho_So>();

            }
        }

    }
}
