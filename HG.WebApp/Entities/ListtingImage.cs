using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("ListtingImage")]
    public class ListtingImage
    {
        public int Id { get; set; }

        public string? ImagePath { get; set; }

        public string? Caption { get; set; }

        public bool IsDefault { get; set; }

        public DateTime? DateCreated { get; set; }
        public int ListtingID { get; set; }

        //public Listting New { get; set; }
    }
}
