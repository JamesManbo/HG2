

using HG.WebApp.Data;

namespace HG.WebApp.Sercurity
{
    public class Sercutiry
    {
        private EAContext db = new EAContext();
        
        public bool IsAthentication(AspNetUsers item, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {

                var lstIdModuleByRole = db.AspNetRoleModules.Where(n => n.RoleId == item.RoleId).Select(n => n.ModuleId).ToArray();
                if (lstIdModuleByRole == null) return false;
                //var lstRole = db.AspModules.Where(n => lstIdModuleByRole.Contains(n.Id)).Select(n => n.link).ToArray();
                //if (lstRole == null) { return false; } else { if (lstRole.Contains(url)) return true; }

            }
            return false;
        }
        public bool IsCollaborators(AspNetUsers item)
        {
                var lstIdModuleByRole = db.AspNetRoles.Where(n => n.Id == item.RoleId).FirstOrDefault();
                if(lstIdModuleByRole != null)
                {
                    if (lstIdModuleByRole.Name == "IsCollaborators" || lstIdModuleByRole.Name == "Editor")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
        }

    }
}