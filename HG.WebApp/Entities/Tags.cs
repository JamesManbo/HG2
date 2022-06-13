using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("Tags")]
    public class Tags
    {
        public int Id { get; set; }
        public int ListtingId { get; set; }
        public string? TagName { get; set; }
    }
}
