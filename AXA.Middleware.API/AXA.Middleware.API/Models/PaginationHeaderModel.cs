using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AXA.Middleware.API.Models
{
    [Serializable]
    public class PaginationHeaderModel
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string PrevPageLink { get; set; }
        public string NextPageLink { get; set; }
    }
}