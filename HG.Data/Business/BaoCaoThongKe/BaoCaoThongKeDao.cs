using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.BaoCaoThongKe;
using HG.Entities.Entities.DanhMuc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.DanhMuc
{
    public class BaoCaoThongKeDao : BaseDAL
    {
        public BaoCaoThongKeDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

       


        public List<BaoCaoSoLuong> BaoCaoSoLuongHoSo(int nam, int quy, bool chonngay, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                DbProvider.SetCommandText2("usp_hoso_baocao_soluong", CommandType.StoredProcedure);
                DbProvider.AddParameter("nam", nam, SqlDbType.Int);
                DbProvider.AddParameter("quy", quy, SqlDbType.Int);
                DbProvider.AddParameter("chonngay", (chonngay ? 1 : 0), SqlDbType.Bit);
                DbProvider.AddParameter("tuNgayInt", tuNgay.ToString("yyyyMMdd"), SqlDbType.BigInt);
                DbProvider.AddParameter("denNgayInt", denNgay.ToString("yyyyMMdd"), SqlDbType.BigInt);

                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteListObject<BaoCaoSoLuong>();
                return obj;
            }
            catch (Exception ex)
            {
                return new List<BaoCaoSoLuong>();
            }
        }

    }
}