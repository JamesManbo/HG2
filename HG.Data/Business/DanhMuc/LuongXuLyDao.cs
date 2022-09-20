using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.GanLuongXuLy;
using HG.Entities.Entities.Luong;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
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

        public List<Dm_luong_Key> DanhSachLuongKey()
        {
            try
            {
                DbProvider.SetCommandText2("dm_danh_sach_luong_key", CommandType.StoredProcedure);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<Dm_luong_Key>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<Dm_luong_Key>();
            }
        }

        public LuongThanhPhanModels LayLuongThanhPhanByMaTTHC(string ma_thu_tuc)
        {
            try
            {
                var result = new LuongThanhPhanModels();
                DbProvider.SetCommandText2("[LayLuongVaThanhPhanBoiTTHC]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_thu_tuc", ma_thu_tuc, SqlDbType.VarChar);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                result.dm_Luong_Xu_Lies = DbProvider.ExecuteReader_frmMyReader<Dm_Luong_Xu_Ly>();
                //Lấy về danh sách nhóm
                DbProvider.ExecuteReader_NextResult();
                result.dm_thanh_phan = DbProvider.ExecuteReader_frmMyReader<dm_thanh_phan>();
                DbProvider.ExecuteReader_Close();
                var ErrCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                if (ErrCode != 0)
                {
                    return new LuongThanhPhanModels();
                }
                return result;
            }
            catch (Exception e)
            {
                return new LuongThanhPhanModels();
            }
        }

        public NguoiXL LayNguoiXLNguoiPHXLByMaLuong(string ma_luong, string ten_buoc)
        {
            try
            {
                var result = new NguoiXL();
                DbProvider.SetCommandText2("[LayNguoiXL$NguoiPHXLByMaLuong]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_luong", ma_luong, SqlDbType.VarChar);
                DbProvider.AddParameter("ten_buoc", ten_buoc, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                result = DbProvider.ExecuteObject<NguoiXL>();
                //Lấy về danh sách nhóm

                var ErrCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                if (ErrCode != 0)
                {
                    return new NguoiXL();
                }
                return result;
            }
            catch (Exception e)
            {
                return new NguoiXL();
            }
        }
        public Dm_Luong_Xu_Ly_paging DanhSanhLuongXuLy(DanhMucModel item)
        {
            try
            {
                Dm_Luong_Xu_Ly_paging menu = new Dm_Luong_Xu_Ly_paging();
                menu.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("dm_danh_sach_luong_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("tu_khoa", item.tu_khoa ?? "", SqlDbType.NVarChar);
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
            try
            {
                DbProvider.SetCommandText2("dm_them_sua_luong_xu_ly_v2", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
                DbProvider.AddParameter("ten_luong", item.ten_luong, SqlDbType.NVarChar);
                DbProvider.AddParameter("mo_ta", item.mo_ta ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("tt_hai_gd", item.tt_hai_gd, SqlDbType.Bit);
                DbProvider.AddParameter("ma_thu_tuc", item.ma_thu_tuc, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_thu_tuc_map", item.ma_thu_tuc_map, SqlDbType.NVarChar);
                DbProvider.AddParameter("luong_cha", item.luong_cha ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("file_excel", item.file_excel ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("uid", item.CreatedUid ?? Guid.Empty, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid_name", item.UidName ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.Stt ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "99");
                return ma_loi;
            }
            catch (Exception ex)
            {
                return 101;
            }
        }

        public int LuuLuongXuLyExcel(Dm_Luong_Xu_Ly item)
        {
            try
            {
                DbProvider.SetCommandText2("dm_them_sua_luong_xu_ly_excel", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
                DbProvider.AddParameter("ten_luong", item.ten_luong, SqlDbType.NVarChar);
                DbProvider.AddParameter("mo_ta", item.mo_ta ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("tt_hai_gd", item.tt_hai_gd, SqlDbType.Bit);
                DbProvider.AddParameter("ma_thu_tuc", item.ma_thu_tuc, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_thu_tuc_map", item.ma_thu_tuc_map, SqlDbType.NVarChar);
                DbProvider.AddParameter("luong_cha", item.luong_cha ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("file_excel", item.file_excel ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("uid", item.CreatedUid ?? Guid.Empty, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid_name", item.UidName ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.Stt ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "99");
                return ma_loi;
            }
            catch (Exception ex)
            {
                return 101;
            }
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
        #region Gán luồng xử lý
        public GanLuongXuLy_paging DanhSanhGanLuongXuLy(DanhMucModel item)
        {
            try
            {
                
                GanLuongXuLy_paging menu = new GanLuongXuLy_paging();
                menu.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("Get_danh_sach_gan_luong", CommandType.StoredProcedure);
                DbProvider.AddParameter("tu_khoa", item.tu_khoa ?? "", SqlDbType.NVarChar);
                // Input params
                DbProvider.AddParameter("page", item.CurrentPage, SqlDbType.Int);
                DbProvider.AddParameter("page_size", item.RecordsPerPage, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                menu.lstGanLuongXuLy = DbProvider.ExecuteListObject<Gan_Luong_Xu_Ly>();
                menu.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new GanLuongXuLy_paging();
            }
        }
        public Asp_GanLuong_XuLy GanLuongXuLyBoiId(Guid Id)
        {
            try
            {
                Asp_GanLuong_XuLy result = new Asp_GanLuong_XuLy();
                result.responseErr = new Response();
                DbProvider.SetCommandText2("Get_danh_sach_gan_luong_byCode", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("Id", Id, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                result.ganLuongXuLyModel = DbProvider.ExecuteReader_frmMyReader<Gan_Luong_Xu_Ly>().FirstOrDefault();
                //Lấy về danh sách nhóm
                //DbProvider.ExecuteReader_NextResult();
                //result.asp_nhom = DbProvider.ExecuteReader_frmMyReader<Asp_nhom>();
                DbProvider.ExecuteReader_Close();
                result.responseErr.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                result.responseErr.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return result;
            }
            catch (Exception e)
            {
                return new Asp_GanLuong_XuLy();
            }
        }
        public int LuuGanLuongXuLy(Gan_Luong_Xu_Ly item)
        {
            try
            {
                DbProvider.SetCommandText2("them_sua_gan_luong_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("Id", item.Id == null? Guid.Empty : item.Id, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("ma_gan_luong", item.ma_gan_luong, SqlDbType.VarChar);
                DbProvider.AddParameter("ten_gan_luong", item.ten_gan_luong, SqlDbType.NVarChar);
                DbProvider.AddParameter("mo_ta", item.mo_ta ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("mac_dinh", item.mac_dinh, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_dich_vu", item.ma_dich_vu ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_linh_vuc", item.ma_linh_vuc ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_luong_xu_ly", item.ma_luong_xu_ly ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("uid", item.CreatedUid ?? Guid.Empty, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid_name", item.UidName ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("Stt", item.Stt ?? null, SqlDbType.Int);
               
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                //  var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "99");
                return 0;
            }
            catch (Exception ex)
            {
                return 101;
            }
        }
        public List<Dm_thu_tuc_hc> DanhSachThuTucHC()
        {
            try
            {
                DbProvider.SetCommandText2("Get_thu_tuc_hc", CommandType.StoredProcedure);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<Dm_thu_tuc_hc>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<Dm_thu_tuc_hc>();
            }
        }
        public DmThuTuc GetThuTucByCode(string ma_thu_tuc)
        {
            try
            {
                DbProvider.SetCommandText2("select * from dm_thu_tuc_hc where ma_thu_tuc = '"+ma_thu_tuc+"'", CommandType.Text);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteObject<DmThuTuc>();
                return menu;
            }
            catch (Exception e)
            {
                return new DmThuTuc();
            }
        }
        public string CheckMaGanLuong(string ma_gan_luong)
        {
            DbProvider.SetCommandText2("check_ma_gan_luong", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_gan_luong", ma_gan_luong, SqlDbType.VarChar);
            
            DbProvider.AddParameter("Code", DBNull.Value, SqlDbType.UniqueIdentifier, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var code = DbProvider.Command.Parameters["code"].Value.ToString();
            return code;
        }
        public int XoaGanLuongXuLy(Guid uid)
        {
            DbProvider.SetCommandText2("Xoa_gan_luong", CommandType.StoredProcedure);
           
            DbProvider.AddParameter("Id", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
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
                DbProvider.AddParameter("ten_luong", DBNull.Value, SqlDbType.NVarChar, 200, ParameterDirection.Output);
                DbProvider.AddParameter("tong_ngay_tt", DBNull.Value, SqlDbType.Float, ParameterDirection.Output);
                DbProvider.AddParameter("tong_ngay_qt", DBNull.Value, SqlDbType.Float, ParameterDirection.Output);
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                menu.lstQuyTrinhXuLy = DbProvider.ExecuteReader_frmMyReader<QuyTrinhXuLy>();

                DbProvider.ExecuteReader_NextResult();
                menu.lstNhanhXuLy = DbProvider.ExecuteReader_frmMyReader<Dm_Nhanh_Xu_Ly>();
                DbProvider.ExecuteReader_Close();

                menu.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());
                menu.ten_luong = DbProvider.Command.Parameters["ten_luong"].Value.ToString() ?? "";
                menu.tong_ngay_tt = float.Parse(DbProvider.Command.Parameters["tong_ngay_tt"].Value.ToString());
                menu.tong_ngay_qt = float.Parse(DbProvider.Command.Parameters["tong_ngay_qt"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new QuyTrinhXuLy_paging();
            }
        }

        public Response LuuQuyTrinhXuLy(QuyTrinhXuLy item)
        {
            try
            {
                var result = new Response();
                DbProvider.SetCommandText2("dm_them_sua_quy_trinh_xu_ly", CommandType.StoredProcedure);
                DbProvider.AddParameter("id", item.Id, SqlDbType.Int);
                DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
                DbProvider.AddParameter("ma_buoc", item.ma_buoc ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("ten_buoc", item.ten_buoc, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_nhanh", item.ma_nhanh ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("so_ngay_xl", item.so_ngay_xl, SqlDbType.Float);
                DbProvider.AddParameter("buoc_xl_chinh", item.buoc_xl_chinh, SqlDbType.Bit);
                DbProvider.AddParameter("chuc_nang", item.chuc_nang ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("nguoi_xl_mac_dinh", item.nguoi_xl_mac_dinh ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("nguoi_co_the_xl", item.nguoi_co_the_xl ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("nguoi_phoi_hop_xl", item.nguoi_phoi_hop_xl ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("uid", item.CreatedUid ?? Guid.Empty, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid_name", item.UidName ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.Stt ?? 0, SqlDbType.Int);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ten_loi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                result.ErrorCode = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "99");
                result.ErrorMsg = DbProvider.Command.Parameters["ten_loi"].Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return new Response();
            }
        }

        public Response LuuQuyTrinhXuLyExcel(QuyTrinhXuLy item)
        {
            try
            {
                var result = new Response();
                DbProvider.SetCommandText2("dm_them_sua_quy_trinh_xu_ly_excel", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_luong", item.ma_luong, SqlDbType.VarChar);
                DbProvider.AddParameter("ten_buoc", item.ten_buoc, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_nhanh", item.ma_nhanh ?? "", SqlDbType.VarChar);
                DbProvider.AddParameter("so_ngay_xl", item.so_ngay_xl, SqlDbType.Float);
                DbProvider.AddParameter("buoc_xl_chinh", item.buoc_xl_chinh, SqlDbType.Bit);
                DbProvider.AddParameter("chuc_nang", item.chuc_nang ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("nguoi_xl", item.nguoi_xl, SqlDbType.NVarChar);
                DbProvider.AddParameter("nguoi_co_the_xl", item.nguoi_co_the_xl ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("nguoi_phoi_hop_xl", item.nguoi_phoi_hop_xl ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("uid", item.CreatedUid ?? Guid.Empty, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid_name", item.UidName ?? "", SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.Stt ?? null, SqlDbType.Int);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ten_loi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                result.ErrorCode = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString() ?? "99");
                result.ErrorMsg = DbProvider.Command.Parameters["ten_loi"].Value.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return new Response();
            }

        }

        public int XoaQuyTrinhXuLy(string ma_luong, int id, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_quy_trinh_xu_ly", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_luong", ma_luong, SqlDbType.VarChar);
            DbProvider.AddParameter("Id", id, SqlDbType.Int);
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

        public List<DataLuong> LuongExcel(string ma_luong)
        {
            try
            {
                DbProvider.SetCommandText2("excel_thong_tin_luong", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_luong", ma_luong, SqlDbType.NVarChar);
                var menu = DbProvider.ExecuteListObject<DataLuong>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<DataLuong>();
            }
        }
    }
}