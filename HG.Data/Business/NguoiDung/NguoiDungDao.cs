using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.NguoiDung
{
    public class NguoiDungDao : BaseDAL
    {
        public NguoiDungDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public List<ds_nguoi_dung> LayDsNguoiDungPhanTrang(NguoiDungSearchItem item , out int Totalrecords)
        {
            try
            {
                DbProvider.SetCommandText2("nguoidung$danhsanh$phantrang", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("StartingRow", item.PageIndex, SqlDbType.Int);
                DbProvider.AddParameter("RecordsPerPage", item.RecordsPerPage, SqlDbType.Int);
                DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.NVarChar);
                DbProvider.AddParameter("tu_khoa", "", SqlDbType.NVarChar);
                DbProvider.AddParameter("trang_thai", item.da_xoa == null ? 0 : 1, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                var obj = DbProvider.ExecuteListObject<ds_nguoi_dung>();
                Totalrecords = Convert.ToInt32(DbProvider.Command.Parameters["tong_ban_ghi"].Value.ToString());
                return obj;
            }
            catch(Exception e) {
                Totalrecords = 0;
                return new List<ds_nguoi_dung>();  }
        } 
        public ds_nguoi_dung_paging LayDsNguoiDungPhanTrang2(NguoiDungSearchItem item)
        {
            try
            {
                ds_nguoi_dung_paging ds_Nguoi_Dung_Paging = new ds_nguoi_dung_paging();
                var abc = 1;
                DbProvider.SetCommandText2("nguoidung$danhsanh$phantrang", CommandType.StoredProcedure);
                if(item.CurrentPage > 1)
                {
                    abc = (item.CurrentPage - 1) * item.RecordsPerPage;
                }
                else if(item.CurrentPage == 1)
                {
                    abc = 0;
                }

                // Input params
                DbProvider.AddParameter("StartingRow", abc, SqlDbType.Int);
                DbProvider.AddParameter("RecordsPerPage", item.RecordsPerPage, SqlDbType.Int);
                DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.VarChar);
                DbProvider.AddParameter("tu_khoa", item.tu_khoa, SqlDbType.NVarChar);
                DbProvider.AddParameter("trang_thai", item.trang_thai, SqlDbType.Int);
                DbProvider.AddParameter("da_xoa", item.da_xoa, SqlDbType.Int);
                DbProvider.AddParameter("user", item.userId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                ds_Nguoi_Dung_Paging.asp_Nhoms = DbProvider.ExecuteListObject<ds_nguoi_dung>();
                ds_Nguoi_Dung_Paging.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["tong_ban_ghi"].Value.ToString());
                return ds_Nguoi_Dung_Paging;
            }
            catch(Exception e) {
                return new  ds_nguoi_dung_paging();  }
        }
        public ds_nguoi_dung_paging_online LayDsNguoiDungOnlPhanTrang(NguoiDungOnlSearchItem item)
        {
            try
            {
                ds_nguoi_dung_paging_online ds_Nguoi_Dung_Paging = new ds_nguoi_dung_paging_online();
                var abc = 1;
                DbProvider.SetCommandText2("nguoidungOnl$danhsanh$phantrang", CommandType.StoredProcedure);
                if (item.CurrentPage > 1)
                {
                    abc = (item.CurrentPage - 1) * item.RecordsPerPage;
                }
                else if (item.CurrentPage == 1)
                {
                    abc = 0;
                }

                // Input params
                DbProvider.AddParameter("StartingRow", abc, SqlDbType.Int);
                DbProvider.AddParameter("RecordsPerPage", item.RecordsPerPage, SqlDbType.Int);
                DbProvider.AddParameter("tu_khoa", item.tu_khoa, SqlDbType.NVarChar);
                DbProvider.AddParameter("trang_thai", item.trang_thai, SqlDbType.Int);
                DbProvider.AddParameter("user", item.userId, SqlDbType.UniqueIdentifier);
                //DbProvider.AddParameter("da_xoa", item.da_xoa, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                ds_Nguoi_Dung_Paging.asp_Nhoms = DbProvider.ExecuteListObject<ds_nguoi_dung_online>();
                ds_Nguoi_Dung_Paging.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["tong_ban_ghi"].Value.ToString());
                return ds_Nguoi_Dung_Paging;
            }
            catch (Exception e)
            {
                return new ds_nguoi_dung_paging_online();
            }
        }
        public string ThemMoi(NguoiDungModels item, Guid guid)
        {
            try
            {
                DbProvider.SetCommandText2("sp_qt_BE_User_Add", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("UserName", item.UserName, SqlDbType.NVarChar);
                DbProvider.AddParameter("Email", item.Email, SqlDbType.NVarChar);
                DbProvider.AddParameter("PhoneNumber", item.PhoneNumber, SqlDbType.NVarChar);
                DbProvider.AddParameter("ho_dem", item.ho_dem, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten", item.ten, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_dai_dien", item.anh_dai_dien, SqlDbType.NVarChar);
                DbProvider.AddParameter("tinh_trang_hon_nhan", item.tinh_trang_hon_nhan, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_chuc_vu", item.ma_chuc_vu, SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
                DbProvider.AddParameter("ngay_sinh", item.ngay_sinh, SqlDbType.DateTime);
                // Output params
                DbProvider.AddParameter("ma_nguoi_dung_moi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                var obj = DbProvider.ExecuteNonQuery();
                var NewId = DbProvider.Command.Parameters["ma_nguoi_dung_moi"].Value.ToString();
                return NewId == null ? "" : NewId;
            }
            catch(Exception e)
            {
                return "";
            }
           
        }
        public string AddUserOnline(UserOnlineModels item, Guid guid)
        {
            try
            {
                DbProvider.SetCommandText2("sp_qt_BE_User_Add", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("Email", item.Email, SqlDbType.NVarChar);
                DbProvider.AddParameter("PhoneNumber", item.PhoneNumber, SqlDbType.NVarChar);
                DbProvider.AddParameter("mat_khau", item.mat_khau, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten", item.ten, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_dai_dien", item.anh_dai_dien, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_cmt", item.anh_cmt, SqlDbType.NVarChar);
                DbProvider.AddParameter("ho_khau_tt", item.ho_khau_tt, SqlDbType.NVarChar);
                DbProvider.AddParameter("khoa_tai_khoan", item.khoa_tai_khoan, SqlDbType.Int);
                
                // Output params
                DbProvider.AddParameter("ma_nguoi_dung_moi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                var obj = DbProvider.ExecuteNonQuery();
                var NewId = DbProvider.Command.Parameters["ma_nguoi_dung_moi"].Value.ToString();
                return NewId == null ? "" : NewId;
            }
            catch (Exception e)
            {
                return "";
            }

        }
        public Response UpdateUserOnline(UserOnlineModels item, Guid guid)
        {
            try
            {
                Response response = new Response();
                DbProvider.SetCommandText2("nguoidungOnl$chinhsua", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("Ma_nguoi_dung", item.Id, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("Email", item.Email, SqlDbType.NVarChar);
                DbProvider.AddParameter("PhoneNumber", item.PhoneNumber, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten", item.ten, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_dai_dien", item.anh_dai_dien, SqlDbType.NVarChar);           
                DbProvider.AddParameter("ho_khau_tt", item.ho_khau_tt, SqlDbType.NVarChar);                         
                DbProvider.AddParameter("UserId", guid, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("khoa_tai_khoan", item.khoa_tai_khoan, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                DbProvider.ExecuteNonQuery();
                response.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                response.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return response;
            }
            catch (Exception e)
            {
                return new Response();
            }

        }
        public Response SuaNguoiDung(NguoiDungModels item, Guid guid)
        {
            try
            {
                Response response = new Response();
                DbProvider.SetCommandText2("nguoidung$chinhsua", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("Ma_nguoi_dung", item.Id, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("UserName", item.UserName, SqlDbType.NVarChar);
                DbProvider.AddParameter("Email", item.Email, SqlDbType.NVarChar);
                DbProvider.AddParameter("PhoneNumber", item.PhoneNumber, SqlDbType.NVarChar);
                DbProvider.AddParameter("ho_dem", item.ho_dem, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten", item.ten, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_dai_dien", item.anh_dai_dien, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_chuc_vu", item.ma_chuc_vu, SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
                DbProvider.AddParameter("ngay_sinh", item.ngay_sinh, SqlDbType.DateTime);
                DbProvider.AddParameter("UserId", guid, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("khoa_tai_khoan", item.khoa_tai_khoan, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                DbProvider.ExecuteNonQuery();
                response.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                response.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return response;
            }
            catch(Exception e)
            {
                return new Response();
            }
           
        }
        public Response SuaNguoiDungOnline(UserOnlineModels item, Guid guid)
        {
            try
            {
                Response response = new Response();
                DbProvider.SetCommandText2("nguoidungOnl$chinhsua", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("Ma_nguoi_dung", item.Id, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("UserName", item.Email, SqlDbType.NVarChar);
                DbProvider.AddParameter("Email", item.Email, SqlDbType.NVarChar);
                DbProvider.AddParameter("PhoneNumber", item.PhoneNumber, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten", item.ten, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_dai_dien", item.anh_dai_dien, SqlDbType.NVarChar);
                DbProvider.AddParameter("anh_cmt", item.anh_cmt, SqlDbType.NVarChar);
                DbProvider.AddParameter("ho_khau_tt", item.ho_khau_tt, SqlDbType.NVarChar);
               
              //  DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
                
                DbProvider.AddParameter("UserId", guid, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("khoa_tai_khoan", item.khoa_tai_khoan, SqlDbType.Int);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                DbProvider.ExecuteNonQuery();
                response.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                response.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return response;
            }
            catch (Exception e)
            {
                return new Response();
            }

        }
        public void ThemMoi_NguoiDung_Nhom(Guid ma_nguoi_dung, string asp_nhom_ma, Guid CreatedUid)
        {
            try
            {
                DbProvider.SetCommandText2("[dbo].[themmoi$nguoidung$nhom]", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("ma_nguoi_dung", ma_nguoi_dung, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("asp_nhom_ma", asp_nhom_ma, SqlDbType.NVarChar);
                DbProvider.AddParameter("CreatedUid", CreatedUid, SqlDbType.UniqueIdentifier);
                // Lấy về danh sách các người dung
                var obj = DbProvider.ExecuteNonQuery();
            }
            catch (Exception e)
            {
             
            }
        }

        public int Xoa(Guid ma_nguoi_dung, Guid uid)
        {
            try
            {
                DbProvider.SetCommandText2("nguoidung$Xoa", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_nguoi_dung", ma_nguoi_dung, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                var ma_loi = Convert.ToInt32(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
                return ma_loi;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
        public int DeleteUserOnline(Guid ma_nguoi_dung, Guid uid)
        {
            try
            {
                DbProvider.SetCommandText2("nguoidungOnl$Xoa", CommandType.StoredProcedure);
                DbProvider.AddParameter("ma_nguoi_dung", ma_nguoi_dung, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                // Lấy về danh sách các trường học
                var obj = DbProvider.ExecuteNonQuery();
                var ma_loi = Convert.ToInt32(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
                return ma_loi;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
        public Asp_NguoiDung_Nhom LayNguoiDungBoiId(Guid UserId)
        {
            try
            {
                Asp_NguoiDung_Nhom result = new Asp_NguoiDung_Nhom();
                result.responseErr = new Response();
                DbProvider.SetCommandText2("[nguoidung$danhsach$id]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("UserId", UserId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                result.aspNetUsersModel = DbProvider.ExecuteReader_frmMyReader<AspNetUsersModel2>();
                //Lấy về danh sách nhóm
                DbProvider.ExecuteReader_NextResult();
                result.asp_nhom = DbProvider.ExecuteReader_frmMyReader<Asp_nhom>();
                DbProvider.ExecuteReader_Close();
                result.responseErr.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                result.responseErr.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return result;
            }
            catch (Exception e)
            {
                return new Asp_NguoiDung_Nhom();
            }
        }
        public Asp_NguoiDung_Nhom_Onl LayNguoiDungOnlBoiId(Guid UserId)
        {
            try
            {
                Asp_NguoiDung_Nhom_Onl result = new Asp_NguoiDung_Nhom_Onl();
                result.responseErr = new Response();
                DbProvider.SetCommandText2("nguoidungOnl$danhsach$id", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("UserId", UserId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                result.aspNetUsersModel = DbProvider.ExecuteReader_frmMyReader<UserOnlineModels>();
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
                return new Asp_NguoiDung_Nhom_Onl();
            }
        }
        public Response ThemMoiNhomNguoiDung(phong_ban_nhom_nguoi_dung item)
        {
            try
            {
                Response result = new Response();
                DbProvider.SetCommandText2("[nhom$dsnguoidung]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_nhom", item.ma_nhom, SqlDbType.NVarChar);
                DbProvider.AddParameter("lst_ma_nguoi_dung", item.lstGroup, SqlDbType.NVarChar);
                DbProvider.AddParameter("userId", item.CreatedUid, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("NewId", DBNull.Value, SqlDbType.UniqueIdentifier, ParameterDirection.Output);
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                DbProvider.ExecuteNonQuery();
                // Lấy về danh sách các người dung
                result.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                result.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                var guiID = DbProvider.Command.Parameters["NewId"].Value.ToString();
                result.NewId = Guid.Parse(guiID == null ? "": guiID);
                return result;
            }
            catch (Exception e)
            {
                return new Response();
            }
        }
        public phong_ban_nhom_nguoi_dung GetNhomNguoiDungByMaNhom(string ma_nhom)
        {
            try
            {
                phong_ban_nhom_nguoi_dung result = new phong_ban_nhom_nguoi_dung();
                DbProvider.SetCommandText2("[nhom$dsnguoidung$ma_nhom]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_nhom", ma_nhom, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                result = DbProvider.ExecuteObject<phong_ban_nhom_nguoi_dung>();
                // Lấy về danh sách các người dung
                var ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                var ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return result;
            }
            catch (Exception e)
            {
                return new phong_ban_nhom_nguoi_dung();
            }
        }

        public Response ThemNhomVaitro(string ma_nhom, string danhsachvaitro)
        {
            try
            {
                Response result = new Response();
                DbProvider.SetCommandText2("[nhom$vaitro$themmoi]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_nhom", ma_nhom, SqlDbType.NVarChar);
                DbProvider.AddParameter("ma_vai_tro", danhsachvaitro, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                DbProvider.ExecuteNonQuery();
                // Lấy về danh sách các người dung
                var ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                var ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return result;
            }
            catch (Exception e)
            {
                return new Response();
            }
        }
        public Nhom_Vaitro LayVaitroThemMaNhom(string ma_nhom)
        {
            try
            {
                Nhom_Vaitro result = new Nhom_Vaitro();
                DbProvider.SetCommandText2("[nhom$vaitro$Laytheomanhom]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_nhom", ma_nhom, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                result = DbProvider.ExecuteObject<Nhom_Vaitro>();
                // Lấy về danh sách các người dung
                var ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                return result;
            }
            catch (Exception e)
            {
                return new Nhom_Vaitro();
            }
        }
        public List<Nhom_NguoiDung> ListNguoiDungByMaNhom(string ma_nhom)
        {
            try
            {
                List<Nhom_NguoiDung> result = new List<Nhom_NguoiDung>();
                DbProvider.SetCommandText2("[ListNguoiDungByMaNhom]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("ma_nhom", ma_nhom, SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                result = DbProvider.ExecuteListObject<Nhom_NguoiDung>();
                // Lấy về danh sách các người dung
                var ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                return result;
            }
            catch (Exception e)
            {
                return new List<Nhom_NguoiDung>();
            }
        }

    }
}
