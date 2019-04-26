using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ViewModel
{
    public class LayOutUserModel
    {
        public string UserName { get; set;}

        public string UserEmail { get; set; }

        public int UserRole { get; set; }

        /// <summary>
        /// 还有多少未审批数量
        /// </summary>
        //public int ApprovalNumber { get; set; }
    }
}
