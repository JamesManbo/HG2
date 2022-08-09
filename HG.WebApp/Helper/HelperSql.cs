using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;

namespace HG.WebApp.Helper
{
    public class HelperSql
    {
      
        readonly static EAContext db = new EAContext();
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
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-C2VM5FN\\SQLEXPRESS; Database=HG; Trusted_Connection=True;"))
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

    }
}
