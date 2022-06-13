using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HG.WebApp
{
    [Table("Category")]
    public class Category
    {
        public int Id { set; get; }
        public string? Name { set; get; }
        public string? NameEn { set; get; }
        public string? Description { set; get; }
        public int? SortOrder { set; get; }
        public int? IsShowOnHome { set; get; }
        public string? ImagePath { set; get; }
        public int? ParentId { set; get; }
        public int Status { set; get; }

    }
}
