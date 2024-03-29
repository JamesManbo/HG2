﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    public class Response
    {
        public int ErrorCode { get; set; }
        public string? ErrorMsg { get; set; }
        public string? ReturnMsg { get; set; }
        public Guid NewId { get; set; }
    }

    public class ResponseData
    {
        public int ErrorCode { get; set; }
        public string? ErrorMsg { get; set; }
        public string? ReturnMsg { get; set; }
        public int Code { get; set; }
    }
}
