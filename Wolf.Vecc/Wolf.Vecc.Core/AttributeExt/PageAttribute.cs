using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Core.AttributeExt
{
    public class PageAttribute : Attribute
    {
        public PageAttribute()
        {
            SortId = "0";
            Align = "center";
            Sortable = false;
        }
        public string Name { get; set; } //标签名
        public string ID { get; set; }//ID
        public string Type { get; set; }//类型
        public string IsShow { get; set; }//是否在table中显示
        public string IsSearch { get; set; }//是否用来查询 
        public string IsShwoInEdit { get; set; }//是否在编辑页面出现

        public string Url { get; set; }//操作所需要的Url
        public string LanguageID { get; set; }//语言ID

        public string SortId { get; set; }// 在页面上的排序
        /// <summary>
        /// 显示
        /// </summary>
        public string Align { get; set; }
        public bool Sortable { get; set; }
        public string SelectTable { get; set; }//1表名，2关联字段，3value字段
        public string IsInConfig { get; set; }//是否在配置表中 配置code
        public string StartOrEndTime { get; set; }//start  \ end
    }
}
