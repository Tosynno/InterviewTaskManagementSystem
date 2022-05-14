using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class tbl_Onboarding
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<tbl_TaskAssign> tblTaskAssign { get; set; } = new List<tbl_TaskAssign>();
    }
}
