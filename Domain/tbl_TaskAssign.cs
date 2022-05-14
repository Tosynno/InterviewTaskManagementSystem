using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class tbl_TaskAssign
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public tbl_Task tblTask { get; set; }
        public Guid UserId { get; set; }
        public tbl_Onboarding tblOnboarding { get; set; }
        public string Status { get; set; }
        public string IncomeTaskResponse { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Rate { get; set; }
    }
}
