using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;

namespace HG.WebApp.Helper
{
    
    public class HelperSql
    {
      
        private readonly static EAContext db = new EAContext();
        public static string GetTenTrang(string ma_trang)
        {
            try
            {
                var countrecord = db.Dm_menu.Where(n => n.ma_trang == ma_trang).FirstOrDefault();
                return countrecord == null ? "" : countrecord.ten_trang;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string GetFullName(Guid? id)
        {
            try
            {
                var countrecord = db.AspNetUsers.Where(n => n.Id == id).FirstOrDefault();
                return countrecord == null ? "" : countrecord.ho_dem + " " + countrecord.ten;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string GetRoleName(Guid? roleid)
        {
            try
            {
                var countrecord = db.AspNetRoles.Where(n=>n.Id == roleid).FirstOrDefault();
                return countrecord == null ? "" : countrecord.Name;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string GetNameHTHC(string ma_thu_tuc_hc)
        {
            try
            {
                var tentthc = "";
                //using (SqlConnection conn = new SqlConnection("Server=DESKTOP-ER8BTDC\\SQLEXPRESS; Database=HG; Trusted_Connection=True;"))
                using (SqlConnection conn = new SqlConnection("Server=WIN-20421CI14U0\\SQLEXPRESS2014;Database=HG; User ID=sa;Password=abcABC123"))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("GetNameThuTucHCByMa", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@ma_thu_tuc_hc", ma_thu_tuc_hc));
                    
                    // execute the command
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // iterate through results, printing each to console
                        while (rdr.Read())
                        {
                            tentthc = (string)rdr["ten_thu_tuc"];
                        }
                    }
                }
                return tentthc;
            }
            catch (Exception e)
            {
                return ma_thu_tuc_hc;
            }
        }
        public static string GetUserName(Guid? userid)
        {
            try
            {
                var countrecord = db.AspNetUsers.Where(n => n.Id == userid).FirstOrDefault();
                if(countrecord != null)
                {
                    var Nickname = countrecord.ho_dem + " " + countrecord.ten;
                    return Nickname == null? "": Nickname;
                }
                else
                {
                    return "Admin";
                }
                 
            }
            catch (Exception e)
            {
                return "Admin";
            }
        }
        public static int TotalHSTS()
        {
            var manon = db.Ghs_Tuyen_Sinh_Cap_Mam_Non.Count();
            var tieuhoc = db.Ghs_Tuyen_Sinh_Cap_Tieu_Hoc.Count();
            var trunghoc = db.Ghs_Tuyen_Sinh_Cap_THCS.Count();
            var phothong = db.Ghs_Tuyen_Sinh_Cap_THPT.Count();
            return manon + tieuhoc+ trunghoc + phothong;
        }
        public static int TotalDangKyKhieuNai()
        {
            return db.Ho_So.Where(n => n.trang_thai == (int)StatusTiepNhanHoSo.HoSoDangTiepNhan && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSDangSoan()
        {
            return db.Ho_So.Where(n => n.trang_thai == (int) StatusTiepNhanHoSo.HoSoTrucTuyen && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSMoiDangKy()
        {
            return db.Ho_So.Where(n => (n.trang_thai == (int)StatusTiepNhanHoSo.HoSoDangTiepNhan || n.trang_thai == (int)StatusTiepNhanHoSo.HoSoTrucTuyen) && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSYeuCauNapTien()
        {
            return db.Ho_So.Where(n => n.type == (int)TypeHS.CongDan && ( n.le_phi == 0 || n.le_phi == null )).Count();
        }
        public static int TotalHSDaNapTien()
        {
            return db.Ho_So.Where(n => n.le_phi > 0 && n.type == (int)TypeHS.CongDan).Count();
        }
        
        public static int TotalHSDaTraKQ()
        {
            return db.Ho_So.Where(n => n.trang_thai == (int)StatusTraKetQua.HoSoDaTraKQ && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSDungXuLy()
        {
            return db.Ho_So.Where(n => n.trang_thai == (int)StatusTraKetQua.HoSoChoTraKS && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSHoanThanhXuLy()
        {
            return db.Ho_So.Where(n => n.trang_thai  == (int)StatusTraKetQua.HoSoDaTraKQ && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSDangXuLy()
        {
            return db.Ho_So.Where(n => n.trang_thai == (int)StatusXuLyHoSo.HoSoDangXuLy && n.type == (int)TypeHS.CongDan).Count();
        }
        public static int TotalHSKhongDuocNhan()
        {
            return db.Ho_So.Where(n => n.trang_thai == 0 && n.type == (int)TypeHS.CongDan).Count();
        }


        public static string GetNamePB(string ma_phong_ban)
        {
            try
            {
                var countrecord = db.Dm_Phong_Ban.Where(n => n.ma_phong_ban == ma_phong_ban).FirstOrDefault();
                return countrecord == null ? "" : countrecord.ten_phong_ban;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static int TongNguoiDanhGia(string Username)
        {
            try
            {
                var countrecord = db.GopYDanhGia.Where(n => n.ma_can_bo == Username).Count();
                return countrecord;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static int TongHaiLong(string Username)
        {
            try
            {
                var countrecord = db.GopYDanhGia.Where(n => n.hai_long == 1 && n.ma_can_bo == Username).Count();
                return countrecord;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static int TongKhongHaiLong(string Username)
        {
            try
            {
                var countrecord = db.GopYDanhGia.Where(n => n.khong_hai_long == 1 && n.ma_can_bo == Username).Count();
                return countrecord;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
