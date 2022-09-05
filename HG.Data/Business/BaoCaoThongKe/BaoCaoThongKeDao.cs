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




        public List<EBaoCaoSoLuong> GetBaoCaoSoLuongHoSo(int nam, int quy, bool chonngay, DateTime tuNgay, DateTime denNgay)
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
                var obj = DbProvider.ExecuteListObject<EBaoCaoSoLuong>();
                return obj;
            }
            catch (Exception ex)
            {
                return new List<EBaoCaoSoLuong>();
            }
        }



        public List<EBaoCaoTheoNguoiXuLy> GetBaoCaoTheoNguoiXuLy(string maphongban, string nguoixuly, string linhvuc, string thutuc, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                DbProvider.SetCommandText2("usp_hoso_baocao_theonguoixuly", CommandType.StoredProcedure);
                DbProvider.AddParameter("maphongban", maphongban);
                DbProvider.AddParameter("nguoixuly", nguoixuly);
                DbProvider.AddParameter("linhvuc", linhvuc);
                DbProvider.AddParameter("thutuc", thutuc);
                DbProvider.AddParameter("tuNgayInt", tuNgay.ToString("yyyyMMdd"), SqlDbType.BigInt);
                DbProvider.AddParameter("denNgayInt", denNgay.ToString("yyyyMMdd"), SqlDbType.BigInt);

                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteListObject<EBaoCaoTheoNguoiXuLy>();
                return obj;
            }
            catch (Exception ex)
            {
                return new List<EBaoCaoTheoNguoiXuLy>();
            }
        }

        public List<EBaoCaoHoSoBiLoai> GetBaoCaoHoSoBiLoai(string maphongban, string nguoitiepnhan, string linhvuc, string thutuc, int tinhtrang, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                DbProvider.SetCommandText2("usp_hoso_baocao_hosobiloai", CommandType.StoredProcedure);
                DbProvider.AddParameter("maphongban", maphongban);
                DbProvider.AddParameter("nguoitiepnhan", nguoitiepnhan);
                DbProvider.AddParameter("linhvuc", linhvuc);
                DbProvider.AddParameter("thutuc", thutuc);
                DbProvider.AddParameter("tinhtrang", tinhtrang);
                DbProvider.AddParameter("tuNgayInt", tuNgay.ToString("yyyyMMdd"), SqlDbType.BigInt);
                DbProvider.AddParameter("denNgayInt", denNgay.ToString("yyyyMMdd"), SqlDbType.BigInt);

                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteListObject<EBaoCaoHoSoBiLoai>();
                return obj;
            }
            catch (Exception ex)
            {
                return new List<EBaoCaoHoSoBiLoai>();
            }
        }
    }
}