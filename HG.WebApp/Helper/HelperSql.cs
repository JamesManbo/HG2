using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;

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
        public static string GetUserName(Guid? userid)
        {
            try
            {
                var countrecord = db.AspNetUsers.Where(n => n.Id == userid).FirstOrDefault();
                if(countrecord != null)
                {
                    var Nickname = countrecord.ho_dem;
                    return Nickname == null? "": Nickname;
                }
                else
                {
                    return "Admin upload file";
                }
                 
            }
            catch (Exception e)
            {
                return "Admin upload file";
            }
        }

    }
}
