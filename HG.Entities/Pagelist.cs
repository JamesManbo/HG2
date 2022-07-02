using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG
{
    public class Pagelist
    {
        public int PageIndex { get; set; }

        public int RecordsPerPage { get; set; }

        public int TotalRecords { get; set; } = 0;
        public int CurrentPage { get; set; } = 0;
    }

    public class PagelistMain
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int RecoredFrom { get; set; } = 0;
        public int RecoredTo { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;
    }
}
