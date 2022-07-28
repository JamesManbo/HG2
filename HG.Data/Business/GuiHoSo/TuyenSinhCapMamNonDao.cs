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
    public class TuyenSinhCapMamNonDao : BaseDAL
    {
        public TuyenSinhCapMamNonDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Phong Ban

        //public List<DanhSachPhongBan> DanhSachPhongBan(string ma_phong_ban, int page, int pageSize, out int total)
        //{
        //    total = 0;
        //    DbProvider.SetCommandText2("dm_danh_sach_phong_ban_tong", CommandType.StoredProcedure);
        //    // Input params
        //    DbProvider.AddParameter("ma_phong_ban", ma_phong_ban, SqlDbType.Char);
        //    DbProvider.AddParameter("page", page, SqlDbType.Int);
        //    DbProvider.AddParameter("pageSize", pageSize, SqlDbType.Int);
        //    // Output params
        //    DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
        //    // Lấy về danh sách các trường học
        //    var obj = DbProvider.ExecuteListObject<DanhSachPhongBan>();
        //    total = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
        //    return obj;
        //}

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

        public Ghs_Tuyen_Sinh_Cap_Mam_Non mapdata(TuyenSinhCapMamNonModel item)
        {
            var data = new Ghs_Tuyen_Sinh_Cap_Mam_Non();
            data.ma_ho_so = item.ma_ho_so;
            data.nam_hoc = item.nam_hoc;
            data.thong_tin_chi_tiet = item.thong_tin_chi_tiet;
            data.id_truong = item.id_truong;
            data.ho_ten_hoc_sinh = item.ho_ten_hoc_sinh;
            data.so_dinh_danh = item.so_dinh_danh;
            data.ngay_sinh = item.ngay_sinh;
            data.gioi_tinh = item.gioi_tinh;
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
            data.id_nguyen_vong = item.id_nguyen_vong;

            return data;
        }
        //public Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso mapdata(TuyenSinhCapMamNonModel item )
        //{
        //    var data = new Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso();
        //    var ds = HG.WebApp.Helper.HelperString.ListThanhPhanHoSoRecords();

        //    return data;
        //}
        public Response Luu(TuyenSinhCapMamNonModel item)
        {
            var res = new Response();
            try
            {

                DbProvider.SetCommandText2("ghs_them_sua_tuyen_sinh_cap_mam_non", CommandType.StoredProcedure);
                //DbProvider.AddParameter("Id", item.Id, SqlDbType.Int);
                DbProvider.AddParameter("ma_ho_so", item.ma_ho_so, SqlDbType.VarChar);
                DbProvider.AddParameter("nam_hoc", item.nam_hoc, SqlDbType.NVarChar);
                DbProvider.AddParameter("thong_tin_chi_tiet", item.thong_tin_chi_tiet, SqlDbType.NVarChar);
                DbProvider.AddParameter("id_truong", item.id_truong ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ho_ten_hoc_sinh", item.ho_ten_hoc_sinh ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("so_dinh_danh", item.so_dinh_danh ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("ngay_sinh", item.ngay_sinh ?? null, SqlDbType.DateTime);
                DbProvider.AddParameter("gioi_tinh", item.gioi_tinh ?? null, SqlDbType.Int);

                DbProvider.AddParameter("ma_dan_toc", item.ma_dan_toc ?? null, SqlDbType.VarChar);
                DbProvider.AddParameter("ma_dia_ban_tinh_noi_sinh", item.ma_dia_ban_tinh_noi_sinh ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_dia_ban_huyen_noi_sinh", item.ma_dia_ban_huyen_noi_sinh ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_dia_ban_tinh_noi_cu_tru", item.ma_dia_ban_tinh_noi_cu_tru ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_dia_ban_huyen_noi_cu_tru", item.ma_dia_ban_huyen_noi_cu_tru ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_dia_ban_xa_noi_cu_tru", item.ma_dia_ban_xa_noi_cu_tru ?? null, SqlDbType.Int);
                DbProvider.AddParameter("thon_xom", item.thon_xom, SqlDbType.NVarChar);
                DbProvider.AddParameter("ho_ten_lien_he", item.ho_ten_lien_he, SqlDbType.NVarChar);
                DbProvider.AddParameter("so_cccd_lien_he", item.so_cccd_lien_he, SqlDbType.NVarChar);
                DbProvider.AddParameter("dien_thoai_lien_he", item.dien_thoai_lien_he, SqlDbType.NVarChar);
                DbProvider.AddParameter("id_nguyen_vong", item.id_nguyen_vong ?? null, SqlDbType.Int);


                DbProvider.AddParameter("uid", item.CreatedUid ?? Guid.Empty, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid_name", item.UidName ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.Stt ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                //DbProvider.AddParameter("ten_loi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                res.ErrorCode = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "100");
               // res.ErrorMsg = DbProvider.Command.Parameters["ten_loi"].Value.ToString();

                return res;
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }

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
        public class TuyenSinhCapMamNonModel
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
            public int? gioi_tinh { get; set; }
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
            public int? id_nguyen_vong { get; set; }
            public int? Stt { get; set; }

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

            public string? chkIsCamKet { get; set; }

        }
        public class ThanhPhanHoSo
        {
            public string? ten_thanh_phan { get; set; }
            public string? file_dinh_kem { get; set; }
            public string? bieu_mau { get; set; }
            public int? ban_chinh { get; set; }
            public int? ban_sao { get; set; }
            public int? ban_photo { get; set; }
            public int? id_ghs_tuyen_sinh_cap_mam_non { get; set; }
            public int? stt { get; set; }
        }
    }
}