using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Luong;
using HG.Entities.Entities.DanhMuc;
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
    public class TraKetQuaDao : BaseDAL
    {
        public TraKetQuaDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }
        public int LuuThongTinTraKetQua(ThongTinTraKQModels model,string userid)
        {
            try
            {
                DbProvider.SetCommandText2("insert into thong_tin_tra_kq values(newid(),'" + model.ma_ho_so + "',N'" + model.ten_nguoi_nhan + "','" + model.so_chung_thuc + "','" + model.so_dien_thoai + "','" + model.dia_chi + "','" + model.le_phi + "','"+model.da_thu_phi+"','"+model.y_kien+ "','" + model.file_dinh_kem + "',getdate(),'" + Guid.Parse(userid) + "',null,null)", CommandType.Text);



                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
        public ThongTinTraKQModels LayThongTinTraKetQua(int? ma_ho_so)
        {
            try
            {
                DbProvider.SetCommandText2("select * from thong_tin_tra_kq where ma_ho_so= '"+ma_ho_so+"'", CommandType.Text);



                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteObject<ThongTinTraKQModels>();
                return obj;
            }
            catch (Exception e)
            {
                return new ThongTinTraKQModels();
            }
        }
    }
}
