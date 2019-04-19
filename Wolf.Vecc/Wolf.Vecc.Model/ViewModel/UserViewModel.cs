using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Vecc.Model.SysModel;

namespace Wolf.Vecc.Model.ViewModel
{
    [NotMapped]
    public class UserViewModel: SysUsers
    {
        /// <summary>
        /// 是否记住我
        /// </summary>
        [NotMapped]
        public bool View_RememberFlag;
    }
}
