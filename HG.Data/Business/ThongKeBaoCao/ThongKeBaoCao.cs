using HG.Data.SqlService;
using HG.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.ThongKeBaoCao
{
    public class ThongKeBaoCao : BaseDAL
    {
        public ThongKeBaoCao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public List<SoLuongHoSoModel> BaoCaoSoLuongHoSo(int nam, int quy) {
            try
            {
                List<SoLuongHoSoModel> soLuongHS = new List<SoLuongHoSoModel>();
                DbProvider.SetCommandText2("[dbo].[bao_cao_so_luong_ho_so]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@nam", nam, SqlDbType.NVarChar);
                DbProvider.AddParameter("@quy", quy, SqlDbType.NVarChar);

                // Output params
                //DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                soLuongHS = DbProvider.ExecuteListObject<SoLuongHoSoModel>();
                //var maloi = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                //if (maloi == 1)
                //{
                //    return new List<SoLuongHoSoModel>();
                //}
                return soLuongHS;
            }
            catch (Exception e)
            {
                return new List<SoLuongHoSoModel>();
            }
        }
    }
}
