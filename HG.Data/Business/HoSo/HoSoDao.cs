using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.HoSo;
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
                if(item.CurrentPage > 1)
                {
                    abc = (item.CurrentPage - 1) * item.RecordsPerPage;
                }
                else if(item.CurrentPage == 1)
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
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                result = DbProvider.ExecuteListObject<Ho_So>();
                total = Convert.ToInt32(DbProvider.Command.Parameters["tong_ban_ghi"].Value.ToString());
                return result;
            }
            catch(Exception e) {
                total = 0;
                return new List<Ho_So>(); 
                
            }
        }
      

    }
}
