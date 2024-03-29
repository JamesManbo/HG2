﻿using HG.Data.SqlService;
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
        public PhanCongThucHienModels GetPhanCongXuLy(int id,int trang_thai)
        {
            try
            {
                DbProvider.SetCommandText2("Get_phancong_xuly", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_ho_so", id, SqlDbType.Int);
                DbProvider.AddParameter("trang_thai", trang_thai, SqlDbType.Int);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteObject<PhanCongThucHienModels>();
                
                return menu;
            }
            catch (Exception e)
            {
                return new PhanCongThucHienModels();
            }
        }
        
        public List<PhieuHenModel> Getphieuhen(int id)
        {
            try
            {
                DbProvider.SetCommandText2("Get_phieu_hen", CommandType.StoredProcedure);
                DbProvider.AddParameter("ID", id, SqlDbType.Int);
               
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<PhieuHenModel>();

                return menu;
            }
            catch (Exception e)
            {
                return new List<PhieuHenModel>();
            }
        }
        public List<dm_thanh_phan> GetThanhPhanHoSo(string ma_thu_tuc)
        {
            try
            {
                DbProvider.SetCommandText2("select * from dm_thanh_phan where ma_thu_tuc ='" + ma_thu_tuc +"'", CommandType.Text);
               

                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<dm_thanh_phan>();

                return menu;
            }
            catch (Exception e)
            {
                return new List<dm_thanh_phan>();
            }
        }
        public List<QuaTrinhXuLy> GetQuaTrinhXuLy(int id)
        {
            try
            {
                DbProvider.SetCommandText2("Get_quatrinh_xuly_hoso", CommandType.StoredProcedure);
                DbProvider.AddParameter("Id_ho_so", id, SqlDbType.Int);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<QuaTrinhXuLy>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<QuaTrinhXuLy>();
            }
        }
        
        public int ThemSuaPhanCongThucHien(PhanCongThucHienModels item,string userid)
        {
            try
            {
               

                DbProvider.SetCommandText2("them_sua_phan_luong_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("Id", item.Id == null? Guid.Empty : item.Id, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("Id_ho_so", item.Id_ho_so, SqlDbType.Int);
                DbProvider.AddParameter("trang_thai_xl", item.trang_thai, SqlDbType.Int);
                DbProvider.AddParameter("han_xu_ly", item.han_xu_ly == null? "" : item.han_xu_ly, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_quy_trinh", item.ma_quy_trinh == null ? "" : item.ma_quy_trinh, SqlDbType.NVarChar);
                DbProvider.AddParameter("Id_nguoi_nhan", item.Id_nguoi_nhan == null ? Guid.Empty : item.Id_nguoi_nhan, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("Id_nguoi_phoi_hop", item.Id_nguoi_phoi_hop == null ? Guid.Empty : item.Id_nguoi_phoi_hop, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("file_dinh_kem", item.file_dinh_kem == null ? "" : item.file_dinh_kem, SqlDbType.NVarChar);
                DbProvider.AddParameter("y_kien", item.y_kien == null? "" : item.y_kien, SqlDbType.NVarChar);
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
        public int GuiHoSoLienThong(int id,string donvinhan,string userid)
        {
            try
            {
                DbProvider.SetCommandText2("insert into ho_so_lien_thong values(newid(),'"+id+"',N'"+donvinhan+ "',null,N'xử lý',null,getdate(),'"+ Guid.Parse(userid) + "',null,null)", CommandType.Text);

               

                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
        public List<ListHoSoLienThong> GetHoSoLienThong(int? id)
        {
            try
            {
                DbProvider.SetCommandText2("select a.CreatedDateUtc Ngay,c.ten_vi_nhan Don_vi_nhan,a.Trang_thai_gui trang_thai_gui,a.Kieu_lien_thong,b.UserName Nguoi_thuc_hien,a.Trang_thai trang_thai  from ho_so_lien_thong a inner join AspNetUsers b on a.CreatedUid = b.Id inner join dm_don_vi c on a.Ma_don_vi = c.ma_don_vi  where Id_ho_so= " + id+"", CommandType.Text);



                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteListObject<ListHoSoLienThong>();
                return obj;
            }
            catch (Exception e)
            {
                return new List<ListHoSoLienThong>();
            }
        }
        public List<ListKetQuaLienThong> GetHoSoNhanLienThong(int? id)
        {
            try
            {
                DbProvider.SetCommandText2("select a.CreatedDateUtc Ngay,c.ten_don_vi Don_vi_tra_loi,a.Noi_dung,a.File_dinh_kem  from ho_so_lien_thong a  inner join dm_don_vi c on a.Id_don_vi = c.ma_don_vi  where Id_ho_so= " + id + "", CommandType.Text);



                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteListObject<ListKetQuaLienThong>();
                return obj;
            }
            catch (Exception e)
            {
                return new List<ListKetQuaLienThong>();
            }
        }


    }
}
