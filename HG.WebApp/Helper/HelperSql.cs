using HG.WebApp.Data;
using Microsoft.AspNetCore.Identity;

namespace HG.WebApp.Helper
{
    public class HelperSql
    {
        readonly static EAContext db = new EAContext();
        public static string? GetStringIMG(int id)
        {
            var query = (from ep in db.ListtingImages
                                  //join e in db.CustomerCeos.DefaultIfEmpty() on 1 equals 1
                                  //join t in db.ListtingImages.DefaultIfEmpty() on 1 equals 1
                              where ep.ListtingID == id
                         select new
                              {
                                 Url = ep.ImagePath
                              }).FirstOrDefault();
            //var path = db.ListtingImages.Where(n => n.ListtingID == id).FirstOrDefault();
            return query == null ? "" : query.Url;
        }


        public static List<Tags> GetListTags(int id)
        {
            var query = (from ep in db.Tags
                         where ep.ListtingId == id
                         select new Tags
                         {
                             Id = ep.Id,
                             TagName = ep.TagName
                         }).ToList();
           
            return query;
        }

        public static int GetTotalReviews(int ratting)
        {
            var query = db.Reviews.Where(n => n.Rating == ratting).GroupBy(test => test.ListtingID)
                   .Select(grp => grp.First())
                   .ToList(); 
            return query == null ? 0 : query.Count();
        }


        public static int GetIdCategory(string name)
        {
            try
            {
                var query = db.Categorys.Where(n => n.Name.Trim().ToLower().Contains(name.Trim().ToLower().ToString()));
                return query == null ? 0 : query.First().Id;
            }catch(Exception ex)
            {
                return 0;
            }
            
        }
        public static int SumNationsByListtingId(int idNations)
        {
            try
            {
                var countrecord = db.Listting.Where(n => n.NationsID == idNations).ToList();
                return countrecord == null ? 0 : countrecord.Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public static int SumCategoiresnameByListting(string idCategories)
        {
            try
            {
                var countrecord = db.Listting.Where(n => (n.CategorysString == null ? "" : n.CategorysString).Contains(idCategories)).ToArray();
                return countrecord == null ? 0 : countrecord.Count();
            }
            catch (Exception e)
            {
                return 0;
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
