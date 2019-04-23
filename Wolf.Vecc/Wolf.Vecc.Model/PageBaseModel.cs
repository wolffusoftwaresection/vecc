using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model
{
    public class PageBaseModel
    {
        //每页显示行数
        public int PageSize { get; set; }
        //当前页数
        public int PageIndex { get; set; }
    }
}