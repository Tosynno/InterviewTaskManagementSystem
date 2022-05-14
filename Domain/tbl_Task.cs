using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class tbl_Task
    {
        public Guid Id { get; set; }
        public string TaskControlId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<tbl_TaskAssign> tblTaskAssign { get; set; } = new List<tbl_TaskAssign>();
    }
}
