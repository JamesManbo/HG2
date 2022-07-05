using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{

    [Table("AspNetRoles")]
    public class AspNetRoles : IdentityRole<Guid>
    {
        public string ma_quyen { get; set; }
        public string? Description { get; set; }
        
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Deleted { get; set; }
        public int? Stt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
