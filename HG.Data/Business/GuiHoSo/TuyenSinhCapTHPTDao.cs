using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.GuiHoSo
{
    public class TuyenSinhCapTHPTDao : BaseDAL
    {
        public TuyenSinhCapTHPTDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Phong Ban
        public List<DanhSachNguoiDung> DanhSachNguoiDung(string ma_phong_ban)
        {
            try
            {
                DbProvider.SetCommandText2("user_danh_sach_nguoi_dung", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_phong_ban", ma_phong_ban, SqlDbType.Char);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteListObject<DanhSachNguoiDung>();
                return obj;
            }
            catch (Exception ex)
            {
                return new List<DanhSachNguoiDung>();
            }
        }

        public Ghs_Tuyen_Sinh_Cap_THPT mapdata(TuyenSinhCapTHPTModel item)
        {
            var data = new Ghs_Tuyen_Sinh_Cap_THPT();
            data.ma_ho_so = item.ma_ho_so;
            data.nam_hoc = item.nam_hoc;
            data.thong_tin_chi_tiet = item.thong_tin_chi_tiet;
            data.id_truong = item.id_truong;
            data.ho_ten_hoc_sinh = item.ho_ten_hoc_sinh;
            data.so_dinh_danh = item.so_dinh_danh;
            data.ngay_sinh = item.ngay_sinh;
            data.ma_gioi_tinh = item.ma_gioi_tinh;
            data.ma_dan_toc = item.ma_dan_toc;
            data.ma_dia_ban_tinh_noi_sinh = item.ma_dia_ban_tinh_noi_sinh;
            data.ma_dia_ban_huyen_noi_sinh = item.ma_dia_ban_huyen_noi_sinh;
            data.ma_dia_ban_tinh_noi_cu_tru = item.ma_dia_ban_tinh_noi_cu_tru;
            data.ma_dia_ban_huyen_noi_cu_tru = item.ma_dia_ban_huyen_noi_cu_tru;
            data.ma_dia_ban_xa_noi_cu_tru = item.ma_dia_ban_xa_noi_cu_tru;
            data.thon_xom = item.thon_xom;
            data.ho_ten_lien_he = item.ho_ten_lien_he;
            data.so_cccd_lien_he = item.so_cccd_lien_he;
            data.dien_thoai_lien_he = item.dien_thoai_lien_he;
            data.ten_truong = item.ten_truong;
            data.ma_uu_tien = item.ma_uu_tien;
            data.dat_giai = item.dat_giai;
            data.diem_khuyen_khich_dat_giai = item.diem_khuyen_khich_dat_giai;
            data.diem_uu_tien = item.diem_uu_tien;
            data.giai_khac = item.giai_khac;
            data.diem_khuyen_khich_giai_khac = item.diem_khuyen_khich_giai_khac;
            data.lop_6_kq_hl = item.lop_6_kq_hl;
            data.lop_6_kq_hk = item.lop_6_kq_hk;
            data.lop_7_kq_hl = item.lop_7_kq_hl;
            data.lop_7_kq_hk = item.lop_7_kq_hk;
            data.lop_8_kq_hl = item.lop_8_kq_hl;
            data.lop_8_kq_hk = item.lop_8_kq_hk;
            data.lop_9_kq_hl = item.lop_9_kq_hl;
            data.lop_9_kq_hk = item.lop_9_kq_hk;

            return data;
        }
        //public Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso mapdata(TuyenSinhCapMamNonModel item )
        //{
        //    var data = new Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso();
        //    var ds = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoRecords();

        //    return data;
        //}

        public int XoaPhongBan(string ma_phong_ban, Guid uid)
        {
            try
            {
                DbProvider.SetCommandText2("dm_xoa_phong_ban", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_phong_ban", ma_phong_ban, SqlDbType.VarChar);
                DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
                var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "100");
                return ma_loi;
            }
            catch (Exception ex)
            {
                return 101;
            }
        }
        #endregion
        public class TuyenSinhCapTHPTModel
        {
            //public int Id { get; set; }
            public string? ma_ho_so { get; set; }
            public string? nam_hoc { get; set; }
            public string thong_tin_chi_tiet { get; set; }
            public int? id_truong { get; set; }
            public string? ho_ten_hoc_sinh { get; set; }
            public string? so_dinh_danh { get; set; }
            public DateTime? ngay_sinh { get; set; }

            // 1 nam, 2 nũ, 3 khác
            public string? ma_gioi_tinh { get; set; }
            public string? ma_dan_toc { get; set; }
            public string? ma_dia_ban_tinh_noi_sinh { get; set; }
            public string? ma_dia_ban_huyen_noi_sinh { get; set; }
            public string? ma_dia_ban_tinh_noi_cu_tru { get; set; }
            public string? ma_dia_ban_huyen_noi_cu_tru { get; set; }
            public string? ma_dia_ban_xa_noi_cu_tru { get; set; }
            public string? thon_xom { get; set; }
            public string? ho_ten_lien_he { get; set; }
            public string? so_cccd_lien_he { get; set; }
            public string? dien_thoai_lien_he { get; set; }
            public string? filesName_0 { get; set; }
            public string? filesName_1 { get; set; }
            public string? filesName_2 { get; set; }
            public string? filesName_3 { get; set; }
            public string? filesName_4 { get; set; }
            public string? filesName_5 { get; set; }
            public int? id_nguyen_vong { get; set; }
            public int? Stt { get; set; }
            // add
            public string? ten_truong { get; set; }
            public string? ma_uu_tien { get; set; }
            public string? dat_giai { get; set; }
            public decimal? diem_khuyen_khich_dat_giai { get; set; }
            public decimal? diem_uu_tien { get; set; }
            public string? giai_khac { get; set; }
            public decimal? diem_khuyen_khich_giai_khac { get; set; }
            public decimal? lop_6_kq_hl { get; set; }
            public string? lop_6_kq_hk { get; set; }
            public decimal? lop_7_kq_hl { get; set; }
            public string? lop_7_kq_hk { get; set; }
            public decimal? lop_8_kq_hl { get; set; }
            public string? lop_8_kq_hk { get; set; }
            public decimal? lop_9_kq_hl { get; set; }
            public string? lop_9_kq_hk { get; set; }
            public string? nv1_ma_nhom { get; set; }
            public string? nv1_ma_mon_khtn { get; set; }
            public string? nv1_ma_mon_cn { get; set; }
            public string? nv12_ma_nhom { get; set; }
            public string? nv12_ma_mon_khtn { get; set; }
            public string? nv12_ma_mon_cn { get; set; }
            public string? nv2_ma_nguyen_vong { get; set; }

            public bool? chkIsCamKet { get; set; }

            // Base 
            public int? Status { get; set; }
            public DateTime? CreatedDateUtc { get; set; }
            public Guid? CreatedUid { get; set; }
            public DateTime? UpdatedDateUtc { get; set; }
            public Guid? UpdatedUid { get; set; }
            public int? Deleted { get; set; }
            public Guid? DeletedBy { get; set; }
            public string? UidName { get; set; }

            public string type_view { get; set; }

        }
    }
}