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

        public Response Luu(Ghs_Tuyen_Sinh_Cap_Mam_Non item)
        {
            var res = new Response();
            try
            {

                DbProvider.SetCommandText2("ghs_them_sua_tuyen_sinh_cap_mam_non", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_ho_so", item.ma_ho_so, SqlDbType.VarChar);
                DbProvider.AddParameter("thong_tin_chi_tiet", item.thong_tin_chi_tiet, SqlDbType.NVarChar);
                DbProvider.AddParameter("id_truong", item.id_truong ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ho_ten_hoc_sinh", item.ho_ten_hoc_sinh ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("so_dinh_danh", item.so_dinh_danh ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("ngay_sinh", item.ngay_sinh ?? null, SqlDbType.DateTime);
                DbProvider.AddParameter("gioi_tinh", item.gioi_tinh ?? null, SqlDbType.Int);

                DbProvider.AddParameter("id_dan_toc", item.id_dan_toc ?? null, SqlDbType.Int);
                DbProvider.AddParameter("id_tinh_noi_sinh", item.id_tinh_noi_sinh ?? null, SqlDbType.Int);
                DbProvider.AddParameter("id_huyen_noi_sinh", item.id_huyen_noi_sinh ?? null, SqlDbType.Int);
                DbProvider.AddParameter("id_tinh_noi_cu_tru", item.id_tinh_noi_cu_tru ?? null, SqlDbType.Int);
                DbProvider.AddParameter("id_huyen_noi_cu_tru", item.id_huyen_noi_cu_tru ?? null, SqlDbType.Int);
                DbProvider.AddParameter("id_xa_noi_cu_tru", item.id_xa_noi_cu_tru ?? null, SqlDbType.Int);
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

    }
}