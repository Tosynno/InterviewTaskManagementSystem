namespace TaskManager.API.Models
{
    public class AcceptAssignRequest
    {
        public string TaskControlId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class CompleteAssignRequest
    {
        public string TaskControlId { get; set; }
        public DateTime EndDate { get; set; }
    }
}
