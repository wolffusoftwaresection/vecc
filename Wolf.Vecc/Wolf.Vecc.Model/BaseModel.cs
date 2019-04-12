using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model
{
    public class BaseModel
    {
        /// <summary>
        /// 操作者
        /// </summary>
        public int Operator { get; set; }

        /// <summary>
        /// 逻辑删除标识 0未删除 1删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
