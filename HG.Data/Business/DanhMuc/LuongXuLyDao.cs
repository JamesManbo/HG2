using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Luong;
using HG.Entities.Entities.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.DanhMuc
{
    public class LuongXuLyDao : BaseDAL
    {
        public LuongXuLyDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Luồng xử lý

        public List<Dm_Thu_Tuc> DanhSachThuTuc()
        {
            try
            {
                DbProvider.SetCommandText2("dm_danh_sach_thu_tuc_key", CommandType.StoredProcedure);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<Dm_Thu_Tuc>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<Dm_Thu_Tuc>();
            }
        }
        public Dm_Luong_Xu_Ly_paging DanhSanhLuongXuLy(DanhMucModel item)
        {
            try
            {
                Dm_Luong_Xu_Ly_paging menu = new Dm_Luong_Xu_Ly_paging();
                menu.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("dm_danh_sach_luong_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("tu_khoa", "", SqlDbType.NVarChar);
                // Input params
                DbProvider.AddParameter("page", item.CurrentPage, SqlDbType.Int);
                DbProvider.AddParameter("page_size", item.RecordsPerPage, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                menu.lstLuongXuLy = DbProvider.ExecuteListObject<Dm_Luong_Xu_Ly>();
                menu.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new Dm_Luong_Xu_Ly_paging();
            }
        }

        public int LuuLuongXuLy(Dm_Luong_Xu_Ly item)
        {
            DbProvider.SetCommandText2("dm_them_sua_luong_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_luong", item.ten_luong, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("tt_hai_gd", item.tt_hai_gd, SqlDbType.Bit);
            DbProvider.AddParameter("ma_thu_tuc", item.ma_thu_tuc, SqlDbType.NVarChar);
            DbProvider.AddParameter("file_excel", item.file_excel, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaLuongXuLy(string ma_luong, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_luong_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_luong", ma_luong, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Bước xử lý
        public int LuuBuocXuLy(Dm_Buoc_Xu_Ly item)
        {
            DbProvider.SetCommandText2("dm_them_sua_buoc_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_buoc_xu_ly", item.ma_buoc_xu_ly, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_buoc_xu_ly", item.ten_buoc_xu_ly, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaBuocXuLy(string ma_buoc_xu_ly, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_buoc_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_buoc_xu_ly", ma_buoc_xu_ly, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Quy trình xử lý
        public QuyTrinhXuLy_paging DanhSanhQuyTrinhXuLy(QuyTrinhModel item)
        {
            try
            {
                QuyTrinhXuLy_paging menu = new QuyTrinhXuLy_paging();
                menu.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("dm_danh_sach_quy_trinh_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("tu_khoa", "", SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
                // Input params
                DbProvider.AddParameter("page", item.CurrentPage, SqlDbType.Int);
                DbProvider.AddParameter("page_size", item.RecordsPerPage, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                menu.lstQuyTrinhXuLy = DbProvider.ExecuteReader_frmMyReader<QuyTrinhXuLy>();

                DbProvider.ExecuteReader_NextResult();
                menu.lstNhanhXuLy = DbProvider.ExecuteReader_frmMyReader<NhanhXuLy>();
                DbProvider.ExecuteReader_Close();

                menu.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new QuyTrinhXuLy_paging();
            }
        }

        public Response LuuQuyTrinhXuLy(QuyTrinhXuLy item)
        {
            var result = new Response();
            DbProvider.SetCommandText2("dm_them_sua_quy_trinh_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("id", item.Id, SqlDbType.Int);
            DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
            DbProvider.AddParameter("ma_buoc", item.ma_buoc, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_buoc", item.ten_buoc, SqlDbType.NVarChar);
            DbProvider.AddParameter("ma_nhanh", item.ma_nhanh, SqlDbType.VarChar);
            DbProvider.AddParameter("so_ngay_xl", item.so_ngay_xl, SqlDbType.Float);
            DbProvider.AddParameter("buoc_xl_chinh", item.buoc_xl_chinh, SqlDbType.Bit);
            DbProvider.AddParameter("chuc_nang", item.chuc_nang, SqlDbType.NVarChar);
            DbProvider.AddParameter("nguoi_xl_mac_dinh", item.nguoi_xl_mac_dinh, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("nguoi_co_the_xl", item.nguoi_co_the_xl, SqlDbType.NVarChar);
            DbProvider.AddParameter("nguoi_phoi_hop_xl", item.nguoi_phoi_hop_xl, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            DbProvider.AddParameter("ten_loi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            result.ErrorCode = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            result.ErrorMsg = DbProvider.Command.Parameters["ten_loi"].Value.ToString();
            return result;
        }

        public int XoaQuyTrinhXuLy(int id, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_quy_trinh_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("Id", id, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion

        #region Bước thực hiện
        public int LuuBuocThucHien(Dm_Buoc_Thuc_Hien item)
        {
            DbProvider.SetCommandText2("dm_them_sua_buoc_thuc_hien", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_buoc", item.ma_buoc, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_buoc", item.ten_buoc, SqlDbType.NVarChar);
            DbProvider.AddParameter("so_ngay", item.so_ngay, SqlDbType.Float);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("uid", item.CreatedUid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("uid_name", item.UidName, SqlDbType.NVarChar);
            DbProvider.AddParameter("stt", item.Stt, SqlDbType.Int);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public int XoaBuocThucHien(string ma_buoc, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_buoc_thuc_hien", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_buoc", ma_buoc, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
        #endregion
    }
}