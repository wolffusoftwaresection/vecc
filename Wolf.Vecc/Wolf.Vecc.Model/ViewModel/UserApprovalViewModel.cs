using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ViewModel
{
    public class UserApprovalViewModel: PageBaseModel
    {
        public string UserName { get; set; }

        public int State { get; set; }
    }
}
