using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Comm.TableDictionary
{
    public class ExcelSelectList
    {
        /// <summary>
        /// 列索引
        /// </summary>
        public int colIndex { get; set; }

        /// <summary>
        /// 下拉内容
        /// </summary>
        public string[] items { get; set; }
    }
}
