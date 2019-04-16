using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model
{
    public class BaseModel<T>
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
        ///// <summary>
        ///// 操作者
        ///// </summary>
        //public int Operator { get; set; }

        /// <summary>
        /// 逻辑删除标识 0未删除 1删除
        /// </summary>
        [Display(Name = "逻辑删除标识 0未删除 1删除")]
        public int IsDel { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateDate { get; set; }

        ///// <summary>
        ///// 修改时间
        ///// </summary>
        //public DateTime? UpdateDate { get; set; }
    }
}
