using Microsoft.AspNetCore.Identity;

namespace HG.WebApp.Entities
{
    public class AspNetRoleClaims : IdentityRoleClaim<Guid>
    {
        public int Id { get; set; }
        public Guid? RoleId { get; set; }
        public string? ClaimType { get; set; }
        public int ModuleValue { get; set; }
    }

    public class AspNetUserClaims : IdentityUserLogin<Guid>
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        //AspNetRoleClaims Id
        public int ClaimValue { get; set; }
    }
}
