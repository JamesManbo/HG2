using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Luong;
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
    public class XuLyHoSoDao : BaseDAL
    {
        public XuLyHoSoDao(SqlDbProvider dbProvider) : base(dbProvider)
        {
           
        }
        public List<QuyTrinhXuLy> DanhSachQuyTrinhXuLyKey()
        {
            try
            {
                DbProvider.SetCommandText2("Get_quytrinh_xuly", CommandType.StoredProcedure);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<QuyTrinhXuLy>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<QuyTrinhXuLy>();
            }
        }
        public PhanCongThucHienModels GetPhanCongXuLy(int id)
        {
            try
            {
                DbProvider.SetCommandText2("Get_phancong_xuly", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_ho_so", id, SqlDbType.Int);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteObject<PhanCongThucHienModels>();
                return menu;
            }
            catch (Exception e)
            {
                return new PhanCongThucHienModels();
            }
        }
        public List<QuaTrinhXuLyModels> GetQuaTrinhXuLy(int id)
        {
            try
            {
                DbProvider.SetCommandText2("Get_quatrinh_xuly_hoso", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_ho_so", id, SqlDbType.Int);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<QuaTrinhXuLyModels>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<QuaTrinhXuLyModels>();
            }
        }
        public int ThemSuaPhanCongThucHien(PhanCongThucHienModels item,string userid)
        {
            try
            {
                DbProvider.SetCommandText2("them_sua_phan_luong_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("Id", item.Id == null? Guid.Empty : item.Id, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("Id_ho_so", item.Id_ho_so, SqlDbType.Int);
                DbProvider.AddParameter("han_xu_ly", item.han_xu_ly == null? "" : item.han_xu_ly, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_quy_trinh", item.ma_quy_trinh, SqlDbType.NVarChar);
                DbProvider.AddParameter("Id_nguoi_nhan", item.Id_nguoi_nhan, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("Id_nguoi_phoi_hop", item.Id_nguoi_phoi_hop, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("file_dinh_kem", item.file_dinh_kem, SqlDbType.NVarChar);
                DbProvider.AddParameter("y_kien", item.y_kien, SqlDbType.NVarChar);
                DbProvider.AddParameter("CreatedUid", Guid.Parse(userid), SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("UpdatedUid", Guid.Parse(userid), SqlDbType.UniqueIdentifier);
              

                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }


    }
}
