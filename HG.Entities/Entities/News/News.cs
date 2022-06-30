using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.News
{
    public class News : BaseEntities
    {
        public string title { get; set; }
        public string? sort_body { get; set; }
        public string? body { get; set; }
        public int? status { get; set; }
        public int? stt { get; set; }
    }
}
