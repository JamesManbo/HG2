﻿using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.Entities.Luong;
using HG.Entities.Entities.Model;
using HG.Entities.Entities.ThuTuc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.ThuTuc
{
    public class ThuTucDao : BaseDAL
    {
        public ThuTucDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        #region Thủ tục

        public ThuTucPaging DanhSanhThuTuc(DanhMucModel item)
        {
            try
            {
                ThuTucPaging menu = new ThuTucPaging();
                menu.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("dm_danh_sach_thu_tuc_list", CommandType.StoredProcedure);
                DbProvider.AddParameter("tu_khoa", "", SqlDbType.NVarChar);
                // Input params
                DbProvider.AddParameter("page", item.CurrentPage, SqlDbType.Int);
                DbProvider.AddParameter("page_size", item.RecordsPerPage, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                menu.lstThuTuc = DbProvider.ExecuteListObject<DmThuTuc>();
                menu.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new ThuTucPaging();
            }
        }

        public Response LuuThuTuc(DmThuTuc item)
        {
            var response = new Response();
            DbProvider.SetCommandText2("dm_them_sua_thu_tuc", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_thu_tuc_old", item.ma_thu_tuc_old, SqlDbType.Int);
            DbProvider.AddParameter("ma_thu_tuc", item.ma_thu_tuc, SqlDbType.VarChar);
            DbProvider.AddParameter("ma_quoc_gia", item.ma_quoc_gia, SqlDbType.VarChar);
            DbProvider.AddParameter("ten_thu_tuc", item.ten_thu_tuc, SqlDbType.NVarChar);
            DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
            DbProvider.AddParameter("nop_online", item.nop_online, SqlDbType.Bit);
            DbProvider.AddParameter("hd_online", item.hd_online, SqlDbType.Bit);
            DbProvider.AddParameter("tthcc", item.tthcc, SqlDbType.Bit);
            DbProvider.AddParameter("tthtks", item.tthtks, SqlDbType.Bit);

            DbProvider.AddParameter("ma_linh_vuc", item.ma_linh_vuc, SqlDbType.VarChar);
            DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.VarChar);
            DbProvider.AddParameter("so_ngay_xl", item.so_ngay_xl, SqlDbType.Float);
            DbProvider.AddParameter("le_phi_truoc", item.le_phi_truoc, SqlDbType.Decimal);
            DbProvider.AddParameter("le_phi_sau", item.le_phi_sau, SqlDbType.Decimal);
            DbProvider.AddParameter("so_bo_hs", item.so_bo_hs, SqlDbType.Int);
            DbProvider.AddParameter("duong_dan_kq", item.duong_dan_kq, SqlDbType.NVarChar);
            DbProvider.AddParameter("thong_tin_mo_rong", item.thong_tin_mo_rong, SqlDbType.NVarChar);
            DbProvider.AddParameter("noi_dung_hd", item.noi_dung_hd, SqlDbType.NVarChar);

            DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
            DbProvider.AddParameter("thu_le_phi_knhs", item.thu_le_phi_knhs, SqlDbType.Bit);
            DbProvider.AddParameter("thuc_hien_hai_gd", item.thuc_hien_hai_gd, SqlDbType.Bit);
            DbProvider.AddParameter("tra_kq_ktn", item.tra_kq_ktn, SqlDbType.Bit);
            DbProvider.AddParameter("cho_phep_lien_thong", item.cho_phep_lien_thong, SqlDbType.Bit);
            DbProvider.AddParameter("cho_phep_gui_lien_thong", item.cho_phep_gui_lien_thong, SqlDbType.Bit);
            DbProvider.AddParameter("cho_phep_nhan_lien_thong", item.cho_phep_nhan_lien_thong, SqlDbType.Bit);
            DbProvider.AddParameter("lst_ma_dvlt_xl", item.lst_ma_dvlt_xl, SqlDbType.NVarChar);
            DbProvider.AddParameter("lst_ma_dvlt_ph", item.lst_ma_dvlt_ph, SqlDbType.NVarChar);

            DbProvider.AddParameter("gui_lt_kem_kq_xl", item.gui_lt_kem_kq_xl, SqlDbType.Bit);
            DbProvider.AddParameter("chi_tra_kq_kckqlt", item.chi_tra_kq_kckqlt, SqlDbType.Bit);
            DbProvider.AddParameter("thuc_hien", item.thuc_hien, SqlDbType.NVarChar);
            DbProvider.AddParameter("thu_le_phi_kckq", item.thu_le_phi_kckq, SqlDbType.Bit);
            DbProvider.AddParameter("uid", item.uid, SqlDbType.UniqueIdentifier);

            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            DbProvider.AddParameter("ten_loi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            response.ErrorCode = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            response.ErrorMsg = DbProvider.Command.Parameters["ten_loi"].Value.ToString();
            return response;
        }

        public int XoaThuTuc(string ma_thu_tuc, Guid uid)
        {
            DbProvider.SetCommandText2("dm_xoa_thu_tuc", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_thu_tuc", ma_thu_tuc, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = int.Parse(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public ThuTucModel ThuTuc(string ma_thu_tuc, string ma_thanh_phan, string ma_van_ban,
            string ma_nhom_tp, string ma_tp_kq, int CurrentPage, int RecordsPerPage)
        {
            try
            {
                ThuTucModel menu = new ThuTucModel();
                menu.PagelistThanhPhan = new Pagelist();
                menu.PagelistVanBanLQ = new Pagelist();
                menu.PagelistNhomTP = new Pagelist();
                menu.PagelisthanhPhanKQXL = new Pagelist();

                DbProvider.SetCommandText2("dm_danh_sach_thu_tuc", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_thu_tuc", ma_thu_tuc, SqlDbType.VarChar);
                DbProvider.AddParameter("ma_thanh_phan", ma_thanh_phan, SqlDbType.VarChar);
                DbProvider.AddParameter("ma_van_ban", ma_van_ban, SqlDbType.VarChar);
                DbProvider.AddParameter("ma_nhom_tp", ma_nhom_tp, SqlDbType.VarChar);
                DbProvider.AddParameter("ma_tp_kq", ma_tp_kq, SqlDbType.VarChar);
                // Input params
                DbProvider.AddParameter("page", CurrentPage, SqlDbType.Int);
                DbProvider.AddParameter("page_size", RecordsPerPage, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                menu.thuTuc = DbProvider.GetObjectFromMyReader<DmThuTuc>();

                DbProvider.ExecuteReader_NextResult();
                menu.lstThanhPhan = DbProvider.ExecuteReader_frmMyReader<ThanhPhan>();
                DbProvider.ExecuteReader_NextResult();
                menu.lstVanBanLQ = DbProvider.ExecuteReader_frmMyReader<VanBanLQ>();
                DbProvider.ExecuteReader_NextResult();
                menu.lstNhomTP = DbProvider.ExecuteReader_frmMyReader<NhomTP>();
                DbProvider.ExecuteReader_NextResult();
                menu.lstThanhPhanKQXL = DbProvider.ExecuteReader_frmMyReader<ThanhPhanKQXL>();
                DbProvider.ExecuteReader_Close();

                menu.PagelistThanhPhan.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total_tp"].Value.ToString());
                menu.PagelistVanBanLQ.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total_vp"].Value.ToString());
                menu.PagelistNhomTP.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total_nhom_tp"].Value.ToString());
                menu.PagelisthanhPhanKQXL.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["total_tpkq"].Value.ToString());
                return menu;
            }
            catch (Exception e)
            {
                return new ThuTucModel();
            }
        }
        #endregion


    }
}