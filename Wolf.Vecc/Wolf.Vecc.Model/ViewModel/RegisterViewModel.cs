using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Vecc.Model.ViewModel
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string EnterpriseName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }
        public int UserType { get; set; }
    }
}
